//<summary>
//  Title   : SBus implementation of the Application Layer Protocol
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//
//    20090123: mzbrzezny: some new tracing messages
//    20080904: mzbrzezny: namespaces cleanup
//    MPostol 2007-03-11
//      W wersji Release byl blad kompilacji od zawsze (od kiedy istnieje repozytorium)!!! 
//      W TX_DATA niepotrzebna zmienna "ramka" odwolywala sie do dostepnej tylko w DEBUG własciwosci STRING dl UMessage.
//    MZbrzezny 2007-01-31
//    usuwanie mechanizmu bazujacego na porcie 
//    w application layer i communication layer
//    MPostol - 14-08-2004:
//      wywalilem definicje klasy ProtocolParameters jako wspolna
//    MZbrzezny - 29-07-04:
//     module creation
//
//  Copyright (C)2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using CAS.Lib.CommonBus.ApplicationLayer.SBUS.PRIVATE;
using CAS.Lib.CommonBus.CommunicationLayer;
using CAS.Lib.CommonBus.CommunicationLayer.Generic;
using CAS.Lib.RTLib.Management;

namespace CAS.Lib.CommonBus.ApplicationLayer.SBUS
{
  ///<summary>
  /// SBus implementation of the Application Layer Protocol.
  ///</summary>
  internal class SBUSProtocol: ALProtocol<FrameStateMachine>
  {
    #region private
    private SesDBufferPool<FrameStateMachine> m_Pool;
    private CAS.Lib.RTLib.Processes.Stopwatch IntercharStopwatch = new CAS.Lib.RTLib.Processes.Stopwatch();
    /// <summary>
    /// enum used to indicate receiver state
    /// </summary>
    private enum RecStateEnum { RSE_BeforeHeading, RSE_InsideFrame }
    /// <summary>
    /// enum used to indicate receiver event 
    /// </summary>
    private enum RecEventEnum { REE_NewChar, REE_TimeOut, REE_NewCharSOH, REE_NewCharLastOne, REE_DisInd }
    /// <summary>
    ///This function gets message from the remote unit.
    /// </summary>
    /// <param name="cRxmsg">received message</param>
    /// <param name="cTxmsg">transmited message, information about this frmae are necessary for checking if this is correct answer</param>
    /// <param name="cInfinitewait">true if this function should wait infinite time for the first character - true for slave side , false in master </param>
    ///  <param name="cStation_addrress">address of station - When this function is used by slavet</param>
    ///  <param name="cReset">the value is set to true if we expected response, but have gor request - we must reset and wait for response </param>
    /// <returns>
    ///   ALRes_Success: Operation accomplished successfully 
    ///   ALRes_DatTransferErrr: Data transfer is imposible because of a communication error – loss of 
    ///      communication with a station
    ///   ALRes_DisInd: Disconnect indication – connection has been shut down remotely or lost because of 
    ///      communication error. Data is unavailable
    /// </returns>
    private AL_ReadData_Result GetMessage
      ( out FrameStateMachine cRxmsg, FrameStateMachine cTxmsg, bool cInfinitewait, int cStation_addrress, ref bool cReset )
    {
      IntercharStopwatch.StartReset();
      TimeSpan currTimeOut;
      if ( !cInfinitewait )
        currTimeOut = this.GetProtocolParameters.ResponseTimeOutSpan;
      else
        currTimeOut = TimeSpan.MaxValue;
      RecStateEnum currRecState = RecStateEnum.RSE_BeforeHeading;
      cRxmsg = m_Pool.GetEmptyISesDBuffer();
      cRxmsg.InitMsg( cTxmsg );
      bool continueDo = true;
      bool flushWait = false;
      do
      {
        RecEventEnum lastRecEvent = RecEventEnum.REE_TimeOut;
        byte lastChar;
        switch ( GetICommunicationLayer.GetChar( out lastChar, Convert.ToInt32( currTimeOut.TotalMilliseconds ) ) )
        {
          case TGetCharRes.Success:
            switch ( cRxmsg.DepositeChar( lastChar ) )
            {
              case FrameStateMachine.DepCharacterTypeEnum.DCT_Last:
                lastRecEvent = RecEventEnum.REE_NewCharLastOne;
                break;
              case FrameStateMachine.DepCharacterTypeEnum.DCT_Reset_Answer:
                GetIProtocolParent.IncStRxSynchError(); //dostalismy ramke nie taka jak trzeba wiec ustawiamy blad jako blad synchronizacji
                cReset = true;
                lastRecEvent = RecEventEnum.REE_NewChar;
                break;
              case FrameStateMachine.DepCharacterTypeEnum.DCT_Ordinary:
                lastRecEvent = RecEventEnum.REE_NewChar;
                break;
              case FrameStateMachine.DepCharacterTypeEnum.DCT_SOH:
                lastRecEvent = RecEventEnum.REE_NewCharSOH;
                break;
            }
            break;
          case TGetCharRes.Timeout:
            lastRecEvent = RecEventEnum.REE_TimeOut;
            break;
          case TGetCharRes.DisInd:
            cRxmsg.ReturnEmptyEnvelope();
            cRxmsg = null;
            TraceEvent.Tracer.TraceVerbose( 155, "SBUSProtocol.GetMessage()", "DisInd has occured during receiving frame, I am exitting the receiving loop and returning AL_ReadData_Result.ALRes_DisInd" );
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
                    currTimeOut = ( (SBUS_ProtocolParameters)GetProtocolParameters ).TimeoutSpanAfterFrame;
                    break;
                  }
                case RecEventEnum.REE_TimeOut:
                  GetIProtocolParent.IncStRxNoResponseCounter();
                  cRxmsg.ReturnEmptyEnvelope();
                  cRxmsg = null;
                  TraceEvent.Tracer.TraceVerbose( 178, "SBUSProtocol.GetMessage():RecStateEnum.RSE_BeforeHeading",
                    "RecEventEnum.REE_TimeOut has occured during receiving frame, I am exitting the receiving loop and returning AL_ReadData_Result.ALRes_DatTransferErrr" );
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
                    TraceEvent.Tracer.TraceInformation( 200, "SBUSProtocol.GetMessage()", "Timeout has occured during receiving frame, I am exitting the receiving loop" );
                    continueDo = false;
                    break;
                  }
                  else
                  {
                    currTimeOut = ( (SBUS_ProtocolParameters)GetProtocolParameters ).TimeoutSpanAfterFrame;
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
    }//GetMessage
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
    protected override AL_ReadData_Result GetMessage( out FrameStateMachine Rxmsg, FrameStateMachine Txmsg )
    {
      bool reset = false;
      return GetMessage( out Rxmsg, Txmsg, false, 0, ref reset );
    }
    /// <summary>
    /// Transmit message to the remote unit.
    /// </summary>
    /// <param name="toSendMessage">Message to be transmitted</param>
    /// <returns>
    ///   ALRes_Success: Operation accomplished successfully 
    ///   ALRes_DatTransferErrr: Data transfer is imposible because of a communication error – loss of 
    ///      communication with a station
    ///   ALRes_DisInd: Disconnect indication – connection has been shut down remotely or lost because of 
    ///      communication error. Data is unavailable
    /// </returns>
    protected override AL_ReadData_Result TransmitMessage( FrameStateMachine toSendMessage )
    {
      FrameStateMachine preparedToSend = m_Pool.GetEmptyISesDBuffer();
      //tworzymy nowego message'a do wyslania- bedzie on zawieral dodatkowo bajty sumy kontrolna i wstawione znaki
      preparedToSend.PrepareFrameToBeSend( toSendMessage );
      switch ( GetICommunicationLayer.FrameEndSignal( preparedToSend ) )
      {
        case TFrameEndSignalRes.Success:
          GetIProtocolParent.IncStTxFrameCounter(); //uzupelniamy statystyki
          break;
        case TFrameEndSignalRes.DisInd:
          return AL_ReadData_Result.ALRes_DisInd;
      }
      preparedToSend.ReturnEmptyEnvelope();
      return AL_ReadData_Result.ALRes_Success;
    }
    #endregion

    #region creators
    ///// <summary>
    ///// SBUS protocol initialization
    ///// </summary>
    ///// <param name="cCommLayer">Interface responsible for providing the communication</param>
    ///// <param name="cProtParameters">Protocol parameters</param>
    ///// <param name="cStatistic">Statistical information about the communication performance</param>
    ///// <param name="cPool">Empty data messages pool to be used by the protocol.</param>
    //internal SBUSProtocol
    //  ( ICommunicationLayer cCommLayer, ProtocolParameters cProtParameters,
    //  BaseStation.Management.IProtocolParent cStatistic, SesDBufferPool<SBUSbase_message> cPool
    //  )
    //  : base( cCommLayer, cProtParameters, cStatistic )
    //{
    //  this.m_Pool = cPool;
    //}
    /// <summary>
    /// SBUS protocol initialization
    /// </summary>
    /// <param name="pStatistic">Statistical information about the communication performance</param>
    /// <param name="pCommLayer">Interface responsible for providing the communication</param>
    /// <param name="pProtParameters">Protocol parameters</param>
    /// <param name="cPool">Empty data messages pool to be used by the protocol.</param>
    internal SBUSProtocol( IProtocolParent pStatistic, ICommunicationLayer pCommLayer,
      ProtocolParameters pProtParameters, SesDBufferPool<FrameStateMachine> cPool )
      : base( pCommLayer, pProtParameters, pStatistic )
    {
      this.m_Pool = cPool;
    }
    #endregion
  }
}
