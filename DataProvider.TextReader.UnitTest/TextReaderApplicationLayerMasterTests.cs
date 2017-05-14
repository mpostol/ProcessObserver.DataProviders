using CAS.Lib.CommonBus;
using CAS.Lib.CommonBus.ApplicationLayer;
using CAS.Lib.RTLib.Management;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace CAS.CommServer.DataProvider.TextReader.Tests
{
  [TestClass()]
  public class TextReaderApplicationLayerMasterTests
  {

    [TestInitialize]
    public void TestInitializeMethod()
    {
      m_Item2Test = LocalTextReaderApplicationLayerMaster.Instance();
    }
    [TestCleanup]
    public void TestCleanupMethod()
    {
      m_Item2Test.Dispose();
    }
    private LocalTextReaderApplicationLayerMaster m_Item2Test;
    private const string m_TestFileName = @"TestingData\ProviderIDConfiguration.xml";
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
        m_Item2Test = LocalTextReaderApplicationLayerMaster.Instance(_component, () => _disposedCalled++);
      }
      Assert.AreEqual<int>(1, _disposedCalled);
    }
    [TestMethod()]
    public void DisReqTest()
    {
      using (LocalTextReaderApplicationLayerMaster m_Item2Test = LocalTextReaderApplicationLayerMaster.Instance())
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
      using (LocalTextReaderApplicationLayerMaster m_Item2Test = LocalTextReaderApplicationLayerMaster.Instance())
      {
        Assert.AreEqual<TConnectReqRes>(TConnectReqRes.Success, m_Item2Test.ConnectReq(RemoteAddress.Instance(m_TestFileName)));
        Assert.IsTrue(m_Item2Test.Connected);
        IReadValue _value = null;
        Assert.AreEqual<AL_ReadData_Result>(AL_ReadData_Result.ALRes_Success, m_Item2Test.ReadData(new TestBlockDescription(0, int.MaxValue, 0), 0, out _value, 0));
        Assert.IsNotNull(_value);
        Assert.AreEqual<int>(0, _value.dataType);
        Assert.IsFalse(_value.InPool);
        Assert.AreEqual<int>(5, _value.length);
        Assert.AreEqual<int>(0, _value.startAddress);
        float _tagValue = 0;
        for (int i = 0; i < _value.length; i++)
          _tagValue = (float)_value.ReadValue(i, typeof(float));
        _value.ReturnEmptyEnvelope();
        Assert.AreEqual<AL_ReadData_Result>(AL_ReadData_Result.ALRes_Success, m_Item2Test.ReadData(new TestBlockDescription(0, int.MaxValue, 0), 0, out _value, 0));
        Assert.AreEqual<float>(1, (float)_value.ReadValue(0, typeof(float)));
        m_Item2Test.DisReq();
        Assert.IsFalse(m_Item2Test.Connected);
        Assert.AreEqual<AL_ReadData_Result>(AL_ReadData_Result.ALRes_DisInd, m_Item2Test.ReadData(new TestBlockDescription(0, int.MaxValue, 0), 0, out _value, 0));
      }
      _sw.Stop();
      Assert.IsTrue(_sw.ElapsedMilliseconds > 200, $"Elapsed {_sw.ElapsedMilliseconds } mS");
    }

    #region Not Implemented
    [TestMethod()]
    [ExpectedException(typeof(NotImplementedException))]
    public void ConnectIndTest()
    {
      m_Item2Test.ConnectInd(null, 0);
    }
    [TestMethod()]
    [ExpectedException(typeof(NotImplementedException))]
    public void GetEmptyWriteDataBufforTest()
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

    #region private instrumentation
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
      public LocalTextReaderApplicationLayerMaster(IProtocolParent statistic, IComponent parentComponent) : base(statistic, parentComponent) { }
      internal static LocalTextReaderApplicationLayerMaster Instance()
      {
        return new LocalTextReaderApplicationLayerMaster(LocalProtocolParent.Instance(), LocalComponent.Instance());
      }
      internal static LocalTextReaderApplicationLayerMaster Instance(LocalComponent _component, Action disposedFunction)
      {
        return new LocalTextReaderApplicationLayerMaster(LocalProtocolParent.Instance(), _component) { m_DisposedCalled = disposedFunction };
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

        public void IncStRxFrameCounter()
        {
        }
        public void IncStRxInvalid()
        {
          throw new NotImplementedException();
        }

        public void IncStRxNAKCounter()
        {
          throw new NotImplementedException();
        }

        public void IncStRxNoResponseCounter()
        {
        }

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

        public void IncStTxFrameCounter()
        {
        }
        public void IncStTxNAKCounter()
        {
          throw new NotImplementedException();
        }

        public void RxDataBlock(bool succ)
        {
        }

        public void TimeCharGapAdd(long val)
        {
          throw new NotImplementedException();
        }

        public void TimeMaxResponseDelayAdd(long val)
        {
        }

        public void TxDataBlock(bool succ)
        {
          throw new NotImplementedException();
        }
      }
      //private class LocalTextReaderMessagesPool : TextReaderMessagesPool
      //{
      //  internal static TextReaderMessagesPool Instance()
      //  {
      //    return new LocalTextReaderMessagesPool();
      //  }
      //}
      private Action m_DisposedCalled = () => { };

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
