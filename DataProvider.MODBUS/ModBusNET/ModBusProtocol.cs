//<summary>
//  Title   : MODBUS implementation of the Application Layer Protocol
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    20080904: mzbrzezny: adaptation for new umessage that supports returning of information about status of write operation
//    20080828: mzbrzezny: some parts are extracted to common (modbus rtu and net) file 
//    2008-08-26: mzbrzezny: synchronization Modbus RTU i Modbus NET
//    2008-06-19: mzbrzezny: Modbus uses now ulong to store number of ticks instead of uint, thats why there is change to flush
//    MPostol: 06-04-2007 created using MODB_ApplicationLayerMaster
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Lib.CommonBus.ApplicationLayer.Modbus;
using CAS.Lib.CommonBus.ApplicationLayer.ModBus.PRIVATE;
using CAS.Lib.CommonBus.CommunicationLayer;

namespace CAS.Lib.CommonBus.ApplicationLayer.ModBus
{
  /// <summary>
  /// MODBUS implementation of the Application Layer Protocol
  /// </summary>
  internal partial class ModBusProtocol: ALProtocol<ModBusMessage>
  {
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
    protected override AL_ReadData_Result GetMessage( out ModBusMessage Rxmsg, ModBusMessage Txmsg )
    {
      Rxmsg = null;
      try
      {
        IntercharStopwatch.Reset();
        IntercharStopwatch.Start();
        if ( !CheckCharTimeout( GetProtocolParameters.ResponseTimeOutSpan, IntercharStopwatch ) )
        {
          GetIProtocolParent.IncStRxNoResponseCounter();
          return AL_ReadData_Result.ALRes_DatTransferErrr;
        }
        GetIProtocolParent.TimeMaxResponseDelayAdd( IntercharStopwatch.ElapsedMilliseconds );
        Rxmsg = m_Pool.GetEmptyISesDBuffer();
        Rxmsg.userDataLength = Rxmsg.userBuffLength;
        Rxmsg.offset = 0;
        bool first = true;
        do
        {
          byte lastChar;
          IntercharStopwatch.Reset();
          IntercharStopwatch.Start();
          switch ( GetICommunicationLayer.GetChar( out lastChar ) )
          {
            case TGetCharRes.Success:
              if ( !Rxmsg.WriteByte( lastChar ) )
                return AL_ReadData_Result.ALRes_DatTransferErrr;
              if ( first )
                first = false;
              else
                GetIProtocolParent.TimeCharGapAdd( CAS.Lib.RTLib.Processes.Timer.ToUSeconds( IntercharStopwatch.Elapsed ) );
              break;
            case TGetCharRes.DisInd:
              return AL_ReadData_Result.ALRes_DisInd;
          }
        }
        while ( CheckCharTimeout( ( (ModBus_ProtocolParameters)GetProtocolParameters ).TimeoutCharacterSpan, IntercharStopwatch ) );
        Rxmsg.userDataLength = Rxmsg.offset;
        Rxmsg.offset = 0;
        if ( CheckCharTimeout( ( (ModBus_ProtocolParameters)GetProtocolParameters ).TimeoutCharacterSpan, IntercharStopwatch ) )
        {
          Flush( ( (ModBus_ProtocolParameters)GetProtocolParameters ).InterframeGapSpan );
          Rxmsg.ReturnEmptyEnvelope();
          Rxmsg = null;
          GetIProtocolParent.IncStRxFragmentedCounter();
          return AL_ReadData_Result.ALRes_DatTransferErrr;
        }
        return AL_ReadData_Result.ALRes_Success;
      }
      catch ( DisconnectException ) { return AL_ReadData_Result.ALRes_DisInd; }
    }
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
    protected override AL_ReadData_Result TransmitMessage( ModBusMessage Txmsg )
    {
      Flush( ( (ModBus_ProtocolParameters)GetProtocolParameters ).InterframeGapSpan );
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
  }
}