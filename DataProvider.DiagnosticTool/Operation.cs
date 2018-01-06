//<summary>
//  Title   : Description of an operation to be done by the worker.
//  System  : Microsoft Visual C# .NET 2013
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using CAS.Lib.CommonBus;
using CAS.Lib.CommonBus.ApplicationLayer;
using CAS.Lib.RTLib.Management;

namespace CAS.DPDiagnostics
{
  /// <summary>
  /// Description of an operation to be done by the worker.
  /// </summary>
  internal abstract class Operation
  {

    #region ctor
    public Operation()
    {
      m_TestTimeStopWatch.Start();
    }
    #endregion

    #region public
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable")]
    internal class ConnectReqResException : Exception
    {
      public ConnectReqResException() { }
      public ConnectReqResException(TConnectReqRes result, string message)
        : base(message)
      {
        Result = result;
      }
      public ConnectReqResException(TConnectReqRes result, string message, Exception inner)
        : base(message, inner)
      {
        Result = result;
      }
      public TConnectReqRes Result { get; set; }
    }
    internal void DoOperation()
    {
      Debug.Assert(m_DoOperation != null, "Application error m_DoOperation is not assigned");
      m_OperationResult.OperationsRunTime.Start();
      m_DoOperation(ref m_NumberOfBytes);
      m_OperationResult.OperationsRunTime.Stop();
      m_OperationResult.NumberOfOperationCycles++;
    }
    internal void ConnectReq()
    {
      TConnectReqRes _connectReqResult = ApplicationLayerMaster.ConnectReq(m_CommLayerAddress);
      if (_connectReqResult != TConnectReqRes.Success)
      {
        string _msg = String.Format("Connect request to remote {0} failed the with error: {1}.", m_CommLayerAddress, _connectReqResult);
        throw new ConnectReqResException(_connectReqResult, _msg);
      }
    }
    internal void DisReq()
    {
      if (!ApplicationLayerMaster.Connected)
        return;
      ApplicationLayerMaster.DisReq();
    }
    internal IOperationResult GetOperationsResult()
    {
      OperationResult _ret = m_OperationResult;
      m_OperationResult = new OperationResult();
      _ret.RunTime = m_TestTimeStopWatch.ElapsedMilliseconds;
      _ret.NumberOfBytes = m_NumberOfBytes;
      return _ret;
    }
    #endregion

    #region private
    private class OperationResult : IOperationResult
    {
      #region IOperationResult Members
      IEnumerable<string> IOperationResult.Log
      {
        get { return Log; }
      }
      long IOperationResult.OperationsRunTime
      {
        get { return OperationsRunTime.ElapsedMilliseconds; }
      }
      long IOperationResult.NumberOfOperationCycles
      {
        get { return NumberOfOperationCycles; }
      }
      long IOperationResult.NumberOfBytes
      {
        get { return NumberOfBytes; }
      }
      long IOperationResult.RunTime
      {
        get { return RunTime; }
      }
      #endregion

      internal List<string> Log = new List<string>();
      internal long NumberOfOperationCycles = 0;
      internal long RunTime = 0;
      internal Stopwatch OperationsRunTime = new Stopwatch();
      internal long NumberOfBytes = 0;
    }
    private delegate void ReadWriteAction(ref long bytesCounter);
    #region abstract
    protected abstract object ValueToBeWritten(Type dataTypeOfConversion);
    protected abstract int Length { get; }
    protected abstract short ResourceSelected { get; }
    protected abstract int StationAddress { get; }
    protected abstract int RegisterStartAddress { get; }
    protected abstract Type DataTypeOfConversion { get; }
    protected abstract IAddress Commlayeraddress { get; }
    private IApplicationLayerMaster ApplicationLayerMaster { get; set; }
    #endregion

    #region vars
    internal long m_NumberOfBytes = 0;
    private OperationResult m_OperationResult = new OperationResult();
    private ReadWriteAction m_DoOperation;
    private BLOK m_Block = new BLOK();
    private short m_ResourceSelected = 0;
    private int m_StationAddress = 0;
    private Type m_DataTypeOfConversion = null;
    private int m_Length = 0;
    private int m_RegistersCount = 0;
    private const byte c_Retries = 5;
    private object m_ValueToBeWritten = null;
    protected IAddress m_CommLayerAddress = null;
    private int m_RegisterAddress = 0;
    private Stopwatch m_TestTimeStopWatch = new Stopwatch();
    #endregion

    protected void InitializeOperation(IDataProviderID dataProviderID, CommonBusControl commonBusControl, IProtocolParent protocolParent, bool readOperation)
    {
      m_ResourceSelected = ResourceSelected;
      m_StationAddress = StationAddress;
      m_RegisterAddress = RegisterStartAddress;
      m_DataTypeOfConversion = DataTypeOfConversion;
      m_Length = Length;
      if (m_DataTypeOfConversion == typeof(string))
        m_RegistersCount = 1;
      else
        m_RegistersCount = Length;
      m_CommLayerAddress = Commlayeraddress;
      ApplicationLayerMaster = dataProviderID.GetApplicationLayerMaster(protocolParent, commonBusControl);
      if (readOperation)
        m_DoOperation = ReadOperation;
      else
      {
        m_ValueToBeWritten = ValueToBeWritten(m_DataTypeOfConversion);
        m_DoOperation = WriteOperation;
      }
    }
    private void ReadOperation(ref long bytesCounter)
    {
      m_Block.Change(m_RegisterAddress, m_Length, m_ResourceSelected);
      IReadValue _data;
      m_OperationResult.Log.Add(String.Format(Properties.Resources.OperationLogHeaderFormat, m_Block.dataType, m_Block.length, m_Block.startAddress, StationAddress, "Read"));
      switch (ApplicationLayerMaster.ReadData(m_Block, StationAddress, out _data, c_Retries))
      {
        case AL_ReadData_Result.ALRes_DatTransferErrr:
          m_OperationResult.Log.Add("-- Reading is completed, but communication errors occurred.");
          break;
        case AL_ReadData_Result.ALRes_DisInd:
          m_OperationResult.Log.Add("-- Reading is completed, but disconnect indication received.");
          break;
        case AL_ReadData_Result.ALRes_Success:
          Debug.Assert(_data != null);
          if (_data is ProtocolALMessage)
            bytesCounter += ((ProtocolALMessage)_data).userDataLength;
          m_OperationResult.Log.Add("-- Reading is completed, data:");
          if (m_DataTypeOfConversion == null)
            m_DataTypeOfConversion = typeof(object);
          try
          {
            if (m_DataTypeOfConversion == typeof(String))
              m_OperationResult.Log.Add(string.Format("---- data[{0}]={1}", 0, _data.ReadValue(0, m_DataTypeOfConversion)));
            else
              for (int idx = 0; idx < m_Block.length; idx++)
                m_OperationResult.Log.Add(string.Format("---- data[{0}]={1}", idx, _data.ReadValue(idx, m_DataTypeOfConversion)));
          }
          catch (Exception ex)
          {
            m_OperationResult.Log.Add(ex.ToString());
          }
          break;
        default:
          break;
      }
      //return to pool
      if (_data != null)
        _data.ReturnEmptyEnvelope();
    }
    private void WriteOperation(ref long bytesCounter)
    {
      m_Block.Change(m_RegisterAddress, m_Length, m_ResourceSelected);
      m_OperationResult.Log.Add(String.Format(Properties.Resources.OperationLogHeaderFormat, m_Block.dataType, m_Block.length, m_Block.startAddress, StationAddress, "Write"));
      IWriteValue _value = ApplicationLayerMaster.GetEmptyWriteDataBuffor(m_Block, m_StationAddress);
      for (int _vix = 0; _vix < m_RegistersCount; _vix++)
        _value.WriteValue(m_ValueToBeWritten, _vix);
      bytesCounter += ((ProtocolALMessage)_value).userDataLength;
      ApplicationLayerMaster.WriteData(ref _value, c_Retries);
    }
    #endregion

  }
}
