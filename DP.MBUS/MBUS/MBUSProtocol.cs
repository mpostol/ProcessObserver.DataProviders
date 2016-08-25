//<summary>
//  Title   : MBUS implementation of the Application Layer Protocol
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  History :
//    20080519: mzbrzezny: created based on ModBUS plugin
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using CAS.Lib.CommonBus.ApplicationLayer.MBUS.PRIVATE;
using CAS.Lib.CommonBus.CommunicationLayer;
using CAS.Lib.CommonBus.CommunicationLayer.Generic;
using CAS.Lib.RTLib.Processes;

namespace CAS.Lib.CommonBus.ApplicationLayer.MBUS
{
  /// <summary>
  /// MBUS implementation of the Application Layer Protocol
  /// </summary>
  internal class MBUSProtocol: ALProtocol<MBUS_message>
  {
    #region private
    /// <summary>
    /// enum used to indicate receiver state
    /// </summary>
    private enum RecStateEnum { RSE_BeforeHeading, RSE_InsideFrame }
    /// <summary>
    /// enum used to indicate receiver event 
    /// </summary>
    private enum RecEventEnum { REE_NewChar, REE_TimeOut, REE_NewCharSOH, REE_NewCharLastOne, REE_DisInd }
    private SesDBufferPool<MBUS_message> m_Pool;
    private CAS.Lib.RTLib.Processes.Stopwatch IntercharStopwatch = new Stopwatch();
    private System.Diagnostics.Stopwatch FlushStopWatch = new System.Diagnostics.Stopwatch();

    private class DisconnectException: Exception
    {
      internal DisconnectException() : base( "Communication layer unexpectedly disconnected." ) { }
    }
    private void Flush( TimeSpan timeout )
    {
      FlushStopWatch.Reset();
      FlushStopWatch.Start();
      do
      {
        if ( GetICommunicationLayer.CheckChar() == TCheckCharRes.DataInd )
        {
          GetICommunicationLayer.Flush();
        }
        System.Threading.Thread.Sleep( 1 );
      }
      while ( FlushStopWatch.Elapsed < timeout );
      FlushStopWatch.Stop();
    }
    #endregion
    #region ALProtocol
    /// <summary>
    /// This function gets message from the remote unit.
    /// </summary>
    /// <param name="Rxmsg">Received message</param>
    /// <param name="Txmsg">Transmited message, information about this frmae could be necessary to properly init received frame.
    /// </param>
    /// <returns>
    ///   ALRes_Success: Operation accomplished successfully 
    ///   ALRes_DatTransferErrr: Data transfer is imposible because of a communication error – loss of 
    ///      communication with a station
    ///   ALRes_DisInd: Disconnect indication – connection has been shut down remotely or lost because of 
    ///      communication error. Data is unavailable
    /// </returns>
    protected override AL_ReadData_Result GetMessage( out MBUS_message Rxmsg, MBUS_message Txmsg )
    {
      int QuantityOfByteInMessage = 0;
      bool cInfinitewait = false;
      IntercharStopwatch.StartReset();
      TimeSpan currTimeOut;
      if ( !cInfinitewait )
        currTimeOut = this.GetProtocolParameters.ResponseTimeOutSpan;
      else
        currTimeOut = TimeSpan.MaxValue;
      RecStateEnum currRecState = RecStateEnum.RSE_BeforeHeading;
      Rxmsg = m_Pool.GetEmptyISesDBuffer();
      Rxmsg.InitMsg( Txmsg );
      bool continueDo = true;
      bool flushWait = false;
      do
      {
        RecEventEnum lastRecEvent = RecEventEnum.REE_TimeOut;
        byte lastChar;
        switch ( GetICommunicationLayer.GetChar( out lastChar, Convert.ToInt32( currTimeOut.TotalMilliseconds ) ) )
        {
          case TGetCharRes.Success:
            switch ( Rxmsg.DepositeChar( lastChar ) )
            {
              case DepCharacterTypeEnum.DCT_Last:
                lastRecEvent = RecEventEnum.REE_NewCharLastOne;
                break;
              case DepCharacterTypeEnum.DCT_Reset_Answer:
                GetIProtocolParent.IncStRxSynchError(); //dostalismy ramke nie taka jak trzeba wiec ustawiamy blad jako blad synchronizacji
                lastRecEvent = RecEventEnum.REE_NewChar;
                break;
              case DepCharacterTypeEnum.DCT_Ordinary:
                lastRecEvent = RecEventEnum.REE_NewChar;
                break;
              case DepCharacterTypeEnum.DCT_SOH:
                lastRecEvent = RecEventEnum.REE_NewCharSOH;
                break;
            }
            QuantityOfByteInMessage++;
            break;
          case TGetCharRes.Timeout:
            lastRecEvent = RecEventEnum.REE_TimeOut;
            break;
          case TGetCharRes.DisInd:
            Rxmsg.ReturnEmptyEnvelope();
            return AL_ReadData_Result.ALRes_DisInd;
        }
        switch ( currRecState )
        {
          case RecStateEnum.RSE_BeforeHeading:
            {
              #region RecStateEnum.RSE_BeforeHeading
              switch ( lastRecEvent )
              {
                case RecEventEnum.REE_NewChar:
                  break;
                case RecEventEnum.REE_NewCharSOH:
                  {
                    currRecState = RecStateEnum.RSE_InsideFrame;
                    GetIProtocolParent.TimeMaxResponseDelayAdd( (long)( CAS.Lib.RTLib.Processes.Stopwatch.ConvertTo_ms( IntercharStopwatch.Reset ) ) );
                    currTimeOut = ( (MBUS_ProtocolParameters)GetProtocolParameters ).CharacterTimeoutSpan;
                    break;
                  }
                case RecEventEnum.REE_TimeOut:
                  GetIProtocolParent.IncStRxNoResponseCounter();
                  Rxmsg.ReturnEmptyEnvelope();
                  Rxmsg = null;
                  return AL_ReadData_Result.ALRes_DatTransferErrr;
              };
              break;
              #endregion
            }
          case RecStateEnum.RSE_InsideFrame:
            {
              #region RecStateEnum.RSE_InsideFrame
              switch ( lastRecEvent )
              {
                case RecEventEnum.REE_NewChar:
                  GetIProtocolParent.TimeCharGapAdd( (long)( CAS.Lib.RTLib.Processes.Stopwatch.ConvertTo_us( IntercharStopwatch.Reset ) ) );
                  break;
                case RecEventEnum.REE_NewCharLastOne:
                  GetIProtocolParent.TimeCharGapAdd( (long)( CAS.Lib.RTLib.Processes.Stopwatch.ConvertTo_us( IntercharStopwatch.Reset ) ) );
                  continueDo = false;
                  break;
                case RecEventEnum.REE_TimeOut:
                  if ( flushWait )
                  {
                    continueDo = false;
                    break;
                  }
                  else
                  {
                    currTimeOut = ( (MBUS_ProtocolParameters)GetProtocolParameters ).ResponseTimeOutSpan;//TimeoutAfterFrameTicks;
                    flushWait = true;
                    break;
                  }
              };
              break;
              #endregion
            }
        }
      }
      while ( continueDo );
      return AL_ReadData_Result.ALRes_Success;
    } //GetMessage

    /// <summary>
    /// Transmit message to the remote unit.
    /// </summary>
    /// <param name="Txmsg">Message to be transmitted</param>
    /// <returns>
    ///   ALRes_Success: 
    ///      Operation accomplished successfully 
    ///   ALRes_DisInd: 
    ///      Disconnect indication – connection has been shut down remotely or lost because of 
    ///      communication error. Data is unavailable
    /// </returns>
    protected override AL_ReadData_Result TransmitMessage( MBUS_message Txmsg )
    {
      Flush( ( (MBUS_ProtocolParameters)GetProtocolParameters ).InterframeGapSpan );
      GetIProtocolParent.IncStTxFrameCounter();
      switch ( GetICommunicationLayer.FrameEndSignal( Txmsg ) )
      {
        case TFrameEndSignalRes.Success:
          break;
        case TFrameEndSignalRes.DisInd:
          return AL_ReadData_Result.ALRes_DisInd;
      }
      return AL_ReadData_Result.ALRes_Success;
    }
    #endregion
    #region creators
    ///// <summary>
    ///// MBUS protocol initialization
    ///// </summary>
    ///// <param name="cCommLayer">Interface responsible for providing the communication</param>
    ///// <param name="cProtParameters">Protocol parameters</param>
    ///// <param name="cStatistic">Statistical information about the communication performance</param>
    ///// <param name="cPool">Empty data messages pool to be used by the protocol.</param>
    //internal MODBProtocol
    //  ( ICommunicationLayer cCommLayer, ProtocolParameters cProtParameters,
    //  BaseStation.Management.IProtocolParent cStatistic, SesDBufferPool<MBUS_message> cPool
    //  )
    //  : base( cCommLayer, cProtParameters, cStatistic )
    //{
    //  this.m_Pool = cPool;
    //}
    /// <summary>
    /// SBUS protocol initialization
    /// </summary>
    /// <param name="pCommLayer">Interface responsible for providing the communication</param>
    /// <param name="pProtParameters">Protocol parameters</param>
    /// <param name="pStatistic">Statistical information about the communication performance</param>
    /// <param name="pPool">Empty data messages pool to be used by the protocol.</param>
    internal MBUSProtocol
     ( ICommunicationLayer pCommLayer, ProtocolParameters pProtParameters,
       CAS.Lib.RTLib.Management.IProtocolParent pStatistic, SesDBufferPool<MBUS_message> pPool )
      : base( pCommLayer, pProtParameters, pStatistic )
    {
      this.m_Pool = pPool;
    }
    #endregion
  }
}