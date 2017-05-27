
using CAS.Lib.CommonBus;
using CAS.Lib.CommonBus.ApplicationLayer;
using CAS.Lib.CommonBus.CommunicationLayer;
using CAS.Lib.RTLib;
using CAS.Lib.RTLib.Management;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace CAS.CommServer.DataProvider.TextReader.UnitTest
{
  [TestClass]
  [DeploymentItem(@"TestingData\", "TestingData")]
  public class TextReaderDataProviderIDUnitTest
  {

    #region TestMethod
    [TestMethod]
    public void ConstructorTestMethod()
    {
      Assert.AreEqual<int>(1, m_TestingProviderID.Count());
      ICommunicationLayerId _layerDefault = m_TestingProviderID.SelectedCommunicationLayer;
      Assert.IsNotNull(_layerDefault);
      ICommunicationLayerId _layerDefault2 = m_TestingProviderID["Simulator"];
      Assert.IsNotNull(_layerDefault2);
      Assert.AreSame(_layerDefault, _layerDefault);
      Assert.AreEqual<string>("CAS.CommServer.DataProvider.TextReader", m_TestingProviderID.Title);
    }
    [TestMethod]
    public void GetApplicationLayerMasterTest()
    {
      using (CommonBusParent _parent = new CommonBusParent())
      {
        IApplicationLayerMaster _al = m_TestingProviderID.GetApplicationLayerMaster(new ProtocolParent(), _parent);
        Assert.IsNotNull(_al);
        Assert.AreEqual<int>(0, _parent.GetNumberOfComponents);
      }
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GetApplicationLayerMasterNullParentTest()
    {
      IApplicationLayerMaster _al2 = m_TestingProviderID.GetApplicationLayerMaster(new ProtocolParent(), null);
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GetApplicationLayerMasterNullStationStatisticsParent()
    {
      IApplicationLayerMaster _al = m_TestingProviderID.GetApplicationLayerMaster(null, null);
    }
    [TestMethod]
    public void GetAvailableAddressSpaceTestMethod()
    {
      IAddressSpaceDescriptor[] _DescriptorsArray = m_TestingProviderID.GetAvailiableAddressspaces();
      Assert.AreEqual<int>(1, _DescriptorsArray.Length);
      Assert.AreEqual<long>(255, _DescriptorsArray[0].EndAddress);
      Assert.AreEqual<short>(0, _DescriptorsArray[0].Identifier);
      Assert.AreEqual<string>("Text File", _DescriptorsArray[0].Name);
      Assert.AreEqual<long>(0, _DescriptorsArray[0].StartAddress);
    }
    [TestMethod]
    public void GetItemDefaultSettingsTestMethod()
    {
      IItemDefaultSettings _settings = m_TestingProviderID.GetItemDefaultSettings(0, 123);
      Assert.AreEqual<ItemAccessRights>(ItemAccessRights.ReadOnly, _settings.AccessRights);
      Assert.AreEqual<int>(4, _settings.AvailiableTypes.Length);
      Assert.AreSame(typeof(string), _settings.DefaultType);
      Assert.AreEqual<String>("Column[123]", _settings.Name);
    }
    [TestMethod]
    public void GetSettingsTestMethod()
    {
      string _settings = m_TestingProviderID.GetSettings();
      Assert.AreEqual<int>(471, _settings.Length);
      Console.Write(_settings);
    }
    [TestMethod]
    public void SetSettingsTestMethod()
    {
      FileInfo _configurationFile = new FileInfo(@"TestingData\ProviderIDConfiguration.xml");
      Assert.IsTrue(_configurationFile.Exists);
      FileStream _configurationStream = _configurationFile.OpenRead();
      string _configurationText = String.Empty;
      using (System.IO.TextReader _tr = new System.IO.StreamReader(_configurationStream))
        _configurationText = _tr.ReadToEnd();
      Assert.AreEqual<int>(408, _configurationText.Length);
      m_TestingProviderID.SetSettings(_configurationText);
    }
    [TestMethod]
    public void GetSettingsHumanReadableFormatTestMethod()
    {
      string _setingsFormated4UI = m_TestingProviderID.GetSettingsHumanReadableFormat();
      Assert.IsTrue(_setingsFormated4UI.StartsWith(TextReaderProtocolParametersUnitTest.TextReaderProtocolParametersString), _setingsFormated4UI);
      Console.WriteLine(_setingsFormated4UI);
    }

    #endregion

    #region private instrumentation
    private TextReaderDataProviderID m_TestingProviderID = new TextReaderDataProviderID();
    private class CommonBusParent : CommonBusControl
    {
      internal int GetNumberOfComponents { get { return this.Components.Count; } }
    }
    private class ProtocolParent : IProtocolParent
    {

      #region IProtocolParent
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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
      }
      public void IncStTxNAKCounter()
      {
        throw new NotImplementedException();
      }
      public void RxDataBlock(bool success)
      {
        throw new NotImplementedException();
      }
      public void TimeCharGapAdd(long val)
      {
        throw new NotImplementedException();
      }
      public void TimeMaxResponseDelayAdd(long val)
      {
        throw new NotImplementedException();
      }
      public void TxDataBlock(bool success)
      {
        throw new NotImplementedException();
      }
      #endregion

    }
    #endregion

  }
}
