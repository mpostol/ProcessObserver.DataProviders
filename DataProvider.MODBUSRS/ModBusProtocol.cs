//<summary>
//  Title   : MODBUS implementation of the Application Layer Protocol
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    20080904: mzbrzezny: adaptation for new message that supports returning of information about status of write operation
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
using CAS.Lib.CommonBus.CommunicationLayer.Generic;
using System.Diagnostics;

namespace CAS.Lib.CommonBus.ApplicationLayer.ModBus
{
  /// <summary>
  /// MODBUS implementation of the Application Layer Protocol
  /// </summary>
  public partial class ModBusProtocol : CommServer.DataProvider.MODBUSCore.ModbusProtocol<ModBusMessage>
  {
    public ModBusProtocol(ICommunicationLayer pCommLayer, ProtocolParameters pProtParameters, RTLib.Management.IProtocolParent pStatistic, SesDBufferPool<ModBusMessage> pPool) :
     base(pCommLayer, pProtParameters, pStatistic, pPool)
    { }

    #region ALProtocol
    /// <summary>
    /// This function gets message from the remote unit.
    /// </summary>
    /// <param name="receiveMessage">Received message</param>
    /// <param name="transmitMessage">Transited message, information about this frame could be necessary to properly initialize received frame.
    /// </param>
    /// <returns>
    ///   ALRes_Success: Operation accomplished successfully 
    ///   ALRes_DatTransferErrr: Data transfer is imposable because of a communication error – loss of 
    ///      communication with a station
    ///   ALRes_DisInd: Disconnect indication – connection has been shut down remotely or lost because of 
    ///      communication error. Data is unavailable
    /// </returns>
    protected override AL_ReadData_Result GetMessage(out ModBusMessage receiveMessage, ModBusMessage transmitMessage)
    {
      receiveMessage = null;
      try
      {
        InterCharStopwatch.Reset();
        InterCharStopwatch.Start();
        if (!CheckCharTimeout(GetProtocolParameters.ResponseTimeOutSpan, InterCharStopwatch))
        {
          GetIProtocolParent.IncStRxNoResponseCounter();
          return AL_ReadData_Result.ALRes_DatTransferErrr;
        }
        GetIProtocolParent.TimeMaxResponseDelayAdd(InterCharStopwatch.ElapsedMilliseconds);
        receiveMessage = m_Pool.GetEmptyISesDBuffer();
        receiveMessage.userDataLength = receiveMessage.userBuffLength;
        receiveMessage.offset = 0;
        bool first = true;
        do
        {
          byte lastChar;
          InterCharStopwatch.Reset();
          InterCharStopwatch.Start();
          switch (GetICommunicationLayer.GetChar(out lastChar))
          {
            case TGetCharRes.Success:
              if (!receiveMessage.WriteByte(lastChar))
              {
                AssemblyTraceEvent.Tracer.TraceEvent(TraceEventType.Warning, 77, "ModBusProtocol.GetMessage: cannot write character received from the Communication Layer");
                return AL_ReadData_Result.ALRes_DatTransferErrr;
              }
              if (first)
                first = false;
              else
                GetIProtocolParent.TimeCharGapAdd(CAS.Lib.RTLib.Processes.Timer.ToUSeconds(InterCharStopwatch.Elapsed));
              break;
            case TGetCharRes.DisInd:
              return AL_ReadData_Result.ALRes_DisInd;
          }
        }
        while (CheckCharTimeout(((ModBus_ProtocolParameters)GetProtocolParameters).Timeout15Span, InterCharStopwatch));
        receiveMessage.userDataLength = receiveMessage.offset;
        receiveMessage.offset = 0;
        if (CheckCharTimeout(((ModBus_ProtocolParameters)GetProtocolParameters).Timeout35Span, InterCharStopwatch))
        {
          Flush(((ModBus_ProtocolParameters)GetProtocolParameters).Timeout35Span);
          receiveMessage.ReturnEmptyEnvelope();
          receiveMessage = null;
          GetIProtocolParent.IncStRxFragmentedCounter();
          return AL_ReadData_Result.ALRes_DatTransferErrr;
        }
        return AL_ReadData_Result.ALRes_Success;
      }
      catch (DisconnectException)
      {
        return AL_ReadData_Result.ALRes_DisInd;
      }
    }
    /// <summary>
    /// Transmit message to the remote unit.
    /// </summary>
    /// <param name="message">Message to be transmitted</param>
    /// <returns>
    ///   ALRes_Success: 
    ///      Operation accomplished successfully 
    ///   ALRes_DisInd: 
    ///      Disconnect indication – connection has been shut down remotely or lost because of 
    ///      communication error. Data is unavailable
    /// </returns>
    protected override AL_ReadData_Result TransmitMessage(ModBusMessage message)
    {
      try
      {
        Flush(((ModBus_ProtocolParameters)GetProtocolParameters).Timeout35Span);
        //flush may throw DisconnectException
      }
      catch (DisconnectException ex)
      {
        AssemblyTraceEvent.Tracer.TraceEvent(TraceEventType.Information, 130, $"ModBusProtocol.TransmitMessage.Flush: exception has been caught {ex.Message}");
        return AL_ReadData_Result.ALRes_DisInd;
      }
      GetIProtocolParent.IncStTxFrameCounter();
      switch (GetICommunicationLayer.FrameEndSignal(message))
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