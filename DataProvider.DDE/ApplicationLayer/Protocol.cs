//<summary>
//  Title   : DDE implementation of the Application Layer Protocol
//  System  : Microsoft Visual C# .NET
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
using CAS.Lib.CommonBus.CommunicationLayer;
using CAS.Lib.CommonBus.CommunicationLayer.Generic;
using CAS.Lib.RTLib.Processes;

namespace CAS.Lib.CommonBus.ApplicationLayer.DDE
{
  /// <summary>
  /// MBUS implementation of the Application Layer Protocol
  /// </summary>
  internal class Protocol: ALProtocol<Message>
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
    private SesDBufferPool<Message> m_Pool;
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
    protected override AL_ReadData_Result GetMessage( out Message Rxmsg, Message Txmsg )
    {
      Rxmsg = m_Pool.GetEmptyISesDBuffer();
      Rxmsg.ResetContent();
      Rxmsg.userDataLength = Rxmsg.userBuffLength;
      ushort received = 0;
      bool receiving = true;
      byte lastChar;
      while ( receiving )
      {
        switch ( GetICommunicationLayer.GetChar( out lastChar, 0 ) )
        {
          case TGetCharRes.Success:
            if ( !Rxmsg.WriteByte( lastChar ) )
            {
              EventLogMonitor.WriteToEventLogError( "Response frame is too long. The data cannot be longer than " + Rxmsg.userBuffLength + " characters", 92 );
              return AL_ReadData_Result.ALRes_DatTransferErrr;
            }
            received++;
            break;
          default:
            receiving = false;
            break;
        }
      }
      if ( Rxmsg.offset == 0 )
        return AL_ReadData_Result.ALRes_DatTransferErrr;
      Rxmsg.userDataLength = received;
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
    /// <remarks>
    /// W moim przypadku transmit message wysle tyle zapytan przez DDE
    /// </remarks>
    protected override AL_ReadData_Result TransmitMessage( Message Txmsg )
    {
      Flush( ( (DDEProtocolParameters)GetProtocolParameters ).InterframeGapSpan );
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
    ///// DDE protocol initialization
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
    /// DDE protocol initialization
    /// </summary>
    /// <param name="pCommLayer">Interface responsible for providing the communication</param>
    /// <param name="pProtParameters">Protocol parameters</param>
    /// <param name="pStatistic">Statistical information about the communication performance</param>
    /// <param name="pPool">Empty data messages pool to be used by the protocol.</param>
    internal Protocol
     ( ICommunicationLayer pCommLayer, ProtocolParameters pProtParameters,
       CAS.Lib.RTLib.Management.IProtocolParent pStatistic, SesDBufferPool<Message> pPool )
      : base( pCommLayer, pProtParameters, pStatistic )
    {
      this.m_Pool = pPool;
    }
    #endregion
  }
}