
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using CAS.Lib.CommonBus;
using CAS.Lib.CommonBus.ApplicationLayer;
using CAS.Lib.RTLib.Management;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAS.CommServer.DataProvider.TextReader.Tests
{
  [TestClass()]
  [DeploymentItem(@"TestingData\", "TestingData")]
  public class TextReaderApplicationLayerMasterTests
  {

    #region tests
    [TestMethod()]
    public void ConnectReqTest()
    {
      Assert.IsFalse(m_Item2Test.Connected);
      switch (m_Item2Test.ConnectReq(RemoteAddress.Instance(String.Empty)))
      {
        case TConnectReqRes.Success:
          Assert.Fail();
          break;
        case TConnectReqRes.NoConnection:
          break;
        default:
          Assert.Fail();
          break;
      }
      Assert.IsFalse(m_Item2Test.Connected);
      FileInfo _configurationFile = new FileInfo(m_TestFileName);
      Assert.IsTrue(_configurationFile.Exists);
      switch (m_Item2Test.ConnectReq(RemoteAddress.Instance(m_TestFileName)))
      {
        case TConnectReqRes.Success:
          break;
        case TConnectReqRes.NoConnection:
          Assert.Fail();
          break;
        default:
          Assert.Fail();
          break;
      }
      Assert.IsTrue(m_Item2Test.Connected);
      m_Item2Test.DisReq();

    }
    [TestMethod()]
    public void DisposeTest()
    {
      int _disposedCalled = 0;
      using (LocalComponent _component = new LocalComponent())
      {
        m_Item2Test = LocalTextReaderApplicationLayerMaster.Instance(_component, () => _disposedCalled++, 10000);
      }
      Assert.AreEqual<int>(1, _disposedCalled);
    }
    [TestMethod()]
    public void DisReqTest()
    {
      using (LocalTextReaderApplicationLayerMaster m_Item2Test = LocalTextReaderApplicationLayerMaster.Instance(10000))
      {
        Assert.AreEqual<TConnectReqRes>(TConnectReqRes.Success, m_Item2Test.ConnectReq(RemoteAddress.Instance(m_TestFileName)));
        Assert.IsTrue(m_Item2Test.Connected);
        m_Item2Test.DisReq();
        Assert.IsFalse(m_Item2Test.Connected);
      }
    }
    [TestMethod()]
    public void ReadDataTest()
    {
      Stopwatch _sw = new Stopwatch();
      _sw.Start();
      string _fileName = @"TestingData\g1765xa1.1";
      FileInfo _testFile = new FileInfo(_fileName);
      Assert.IsTrue(File.Exists(_fileName));
      using (LocalTextReaderApplicationLayerMaster _textReader = LocalTextReaderApplicationLayerMaster.Instance(10000))
      {
        Assert.AreEqual<TConnectReqRes>(TConnectReqRes.Success, _textReader.ConnectReq(RemoteAddress.Instance(_fileName)));
        Assert.IsTrue(_textReader.Connected);
        Assert.AreEqual<int>(1, _textReader.TestTraceSource.TraceListener.Count, string.Join(",", _textReader.TestTraceSource.TraceListener));
        IReadValue _value = null;
        Assert.AreEqual<AL_ReadData_Result>(AL_ReadData_Result.ALRes_DatTransferErrr, _textReader.ReadData(new TestBlockDescription(0, 39, 0), 0, out _value, 0));
        string[] _content = File.ReadAllLines(_fileName);
        Assert.AreEqual<int>(2422, _content.Length);
        File.WriteAllLines(_fileName, _content);
        Thread.Sleep(3000);
        Assert.AreEqual<int>(3, _textReader.TestTraceSource.TraceListener.Count);
        Assert.AreEqual<AL_ReadData_Result>(AL_ReadData_Result.ALRes_Success, _textReader.ReadData(new TestBlockDescription(0, 39, 0), 0, out _value, 0));
        Assert.IsNotNull(_value);
        Assert.AreEqual<int>(0, _value.dataType);
        Assert.IsFalse(_value.InPool);
        Assert.AreEqual<int>(39, _value.length);
        Assert.AreEqual<int>(0, _value.startAddress);
        float _tagValue = 0;
        for (int i = 2; i < _value.length - 3; i++)
          _tagValue = (float)_value.ReadValue(i, typeof(float));
        _value.ReturnEmptyEnvelope();
        Thread.Sleep(15000);
        Assert.AreEqual<int>(5, _textReader.TestTraceSource.TraceListener.Count);
        Assert.AreEqual<AL_ReadData_Result>(AL_ReadData_Result.ALRes_DatTransferErrr, _textReader.ReadData(new TestBlockDescription(0, 39, 0), 0, out _value, 0));
        _textReader.DisReq();
        Assert.IsFalse(_textReader.Connected);
        Assert.AreEqual<AL_ReadData_Result>(AL_ReadData_Result.ALRes_DisInd, _textReader.ReadData(new TestBlockDescription(0, 39, 0), 0, out _value, 0));
      }
      _sw.Stop();
      Assert.IsTrue(_sw.ElapsedMilliseconds > 4000, $"Elapsed {_sw.ElapsedMilliseconds } mS");
    }
    [TestMethod()]
    public void ReadDataDisconnectedTest()
    {
      string _fileName = @"TestingData\g1765xa1.1";
      FileInfo _testFile = new FileInfo(_fileName);
      Assert.IsTrue(File.Exists(_fileName));
      using (LocalTextReaderApplicationLayerMaster _textReader = LocalTextReaderApplicationLayerMaster.Instance(5000))
      {
        for (int i = 0; i < 2; i++)
        {
          Assert.AreEqual<TConnectReqRes>(TConnectReqRes.Success, _textReader.ConnectReq(RemoteAddress.Instance(_fileName)));
          Assert.IsTrue(_textReader.Connected);
          IReadValue _value = null;
          Assert.AreEqual<AL_ReadData_Result>(AL_ReadData_Result.ALRes_DatTransferErrr, _textReader.ReadData(new TestBlockDescription(0, 39, 0), 0, out _value, 0));
          string[] _content = File.ReadAllLines(_fileName);
          File.WriteAllLines(_fileName, _content);
          Thread.Sleep(3000);
          Assert.AreEqual<AL_ReadData_Result>(AL_ReadData_Result.ALRes_Success, _textReader.ReadData(new TestBlockDescription(0, 39, 0), 0, out _value, 0));
          Assert.IsNotNull(_value);
          _value.ReturnEmptyEnvelope();
          Thread.Sleep(30000);
          Assert.AreEqual<AL_ReadData_Result>(AL_ReadData_Result.ALRes_DisInd, _textReader.ReadData(new TestBlockDescription(0, 39, 0), 0, out _value, 3));
          Assert.IsFalse(_textReader.Connected);
          Assert.AreEqual<AL_ReadData_Result>(AL_ReadData_Result.ALRes_DisInd, _textReader.ReadData(new TestBlockDescription(0, 39, 0), 0, out _value, 100));
        }
        Assert.AreEqual<int>(24, _textReader.TestTraceSource.TraceListener.Count);
      }
    }
    #endregion

    #region Not Implemented API
    [TestMethod()]
    [ExpectedException(typeof(NotImplementedException))]
    public void ConnectIndTest()
    {
      m_Item2Test.ConnectInd(null, 0);
    }
    [TestMethod()]
    [ExpectedException(typeof(NotImplementedException))]
    public void GetEmptyWriteDataBufferTest()
    {
      m_Item2Test.GetEmptyWriteDataBuffor(null, 0);
    }
    [TestMethod()]
    [ExpectedException(typeof(NotImplementedException))]
    public void WriteDataTest()
    {
      IWriteValue _valueToWrite = null;
      m_Item2Test.WriteData(ref _valueToWrite, 0);
    }
    #endregion

    #region instrumentation
    [TestInitialize]
    public void TestInitializeMethod()
    {
      m_Item2Test = LocalTextReaderApplicationLayerMaster.Instance(10000);
    }
    [TestCleanup]
    public void TestCleanupMethod()
    {
      m_Item2Test.Dispose();
    }
    private LocalTextReaderApplicationLayerMaster m_Item2Test;
    private const string m_TestFileName = @"TestingData\ProviderIDConfiguration.xml";
    private class TestBlockDescription : IBlockDescription
    {
      public TestBlockDescription(short typeOfData, int lengthOfData, int startAddressOfData)
      {
        dataType = typeOfData;
        length = lengthOfData;
        startAddress = startAddressOfData;
      }
      public short dataType
      {
        get; private set;
      }
      public int length
      {
        get; private set;
      }
      public int startAddress
      {
        get; private set;
      }
    }
    private class LocalTextReaderApplicationLayerMaster : TextReaderApplicationLayerMaster
    {
      public TestTraceSource TestTraceSource { get; private set; } = new TestTraceSource();
      public LocalTextReaderApplicationLayerMaster(IProtocolParent statistic, IComponent parentComponent, int _fileModificationNotificationTimeout) :
        base(statistic, parentComponent, new TextReaderProtocolParameters() { FileModificationNotificationTimeout = _fileModificationNotificationTimeout })
      {
        TraceSource = TestTraceSource;
      }
      internal static LocalTextReaderApplicationLayerMaster Instance(int fileModificationNotificationTimeout)
      {
        return new LocalTextReaderApplicationLayerMaster(LocalProtocolParent.Instance(), LocalComponent.Instance(), fileModificationNotificationTimeout);
      }
      internal static LocalTextReaderApplicationLayerMaster Instance(LocalComponent _component, Action disposedFunction, int fileModificationNotificationTimeout)
      {
        return new LocalTextReaderApplicationLayerMaster(LocalProtocolParent.Instance(), _component, fileModificationNotificationTimeout) { m_DisposedCalled = disposedFunction };
      }
      protected override void Dispose(bool disposing)
      {
        base.Dispose(disposing);
        if (disposing)
          m_DisposedCalled();
      }
      private class LocalProtocolParent : IProtocolParent
      {
        internal static IProtocolParent Instance()
        {
          return new LocalProtocolParent();
        }
        public void IncStRxCRCErrorCounter()
        {
          throw new NotImplementedException();
        }
        public void IncStRxFragmentedCounter()
        {
          throw new NotImplementedException();
        }
        public void IncStRxFrameCounter() { }
        public void IncStRxInvalid()
        {
          throw new NotImplementedException();
        }
        public void IncStRxNAKCounter()
        {
          throw new NotImplementedException();
        }
        public void IncStRxNoResponseCounter() { }
        public void IncStRxSynchError()
        {
          throw new NotImplementedException();
        }
        public void IncStTxACKCounter()
        {
          throw new NotImplementedException();
        }
        public void IncStTxDATACounter()
        {
          throw new NotImplementedException();
        }
        public void IncStTxFrameCounter() { }
        public void IncStTxNAKCounter()
        {
          throw new NotImplementedException();
        }
        public void RxDataBlock(bool success) { }
        public void TimeCharGapAdd(long val)
        {
          throw new NotImplementedException();
        }
        public void TimeMaxResponseDelayAdd(long val) { }
        public void TxDataBlock(bool success)
        {
          throw new NotImplementedException();
        }
      }
      private Action m_DisposedCalled = () => { };
    }
    private class TestTraceSource : ITraceSource
    {
      internal List<Tuple<TraceEventType, int, string>> TraceListener = new List<Tuple<TraceEventType, int, string>>();
      public void TraceMessage(TraceEventType eventType, int id, string message)
      {
        TraceListener.Add(new Tuple<TraceEventType, int, string>(eventType, id, message));
      }
    }
    private class LocalComponent : IComponent
    {
      internal static IComponent Instance()
      {
        return new LocalComponent();
      }
      public ISite Site
      {
        get
        {
          throw new NotImplementedException();
        }
        set
        {
          throw new NotImplementedException();
        }
      }
      public event EventHandler Disposed;
      public void Dispose()
      {
        Disposed?.Invoke(this, EventArgs.Empty);
      }
    }
    private class RemoteAddress : IAddress
    {
      private string m_Address = String.Empty;
      public object address
      {
        get
        {
          return m_Address;
        }
        set
        {
          m_Address = (string)value;
        }
      }
      internal static IAddress Instance(string fileName)
      {
        return new RemoteAddress() { m_Address = fileName };
      }
    }
    #endregion

  }
}
