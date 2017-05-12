//_______________________________________________________________
//  Title   : TextReaderDataProviderID
//  System  : Microsoft VisualStudio 2015 / C#
//  $LastChangedDate:  $
//  $Rev: $
//  $LastChangedBy: $
//  $URL: $
//  $Id:  $
//
//  Copyright (C) 2017, CAS LODZ POLAND.
//  TEL: +48 608 61 98 99 
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using CAS.Lib.CommonBus;
using CAS.Lib.CommonBus.ApplicationLayer;
using CAS.Lib.CommonBus.CommunicationLayer.NULL;
using CAS.Lib.RTLib;
using CAS.Lib.RTLib.Management;
using System;
using System.Xml;

namespace CAS.CommServer.DataProvider.TextReader
{
  public sealed class TextReaderDataProviderID : DataProviderID
  {
    #region ctor
    public TextReaderDataProviderID()
    {
      Add(new NullCommunicationLayerID());
    }
    #endregion

    #region DataProviderID
    public override IApplicationLayerMaster GetApplicationLayerMaster(IProtocolParent pStatistic, CommonBusControl pParent)
    {
      if (pStatistic == null)
        throw new ArgumentNullException($"{pStatistic} cannot me null");
      if (pParent == null)
        throw new ArgumentNullException($"{pParent} cannot me null");
      return new TextReaderApplicationLayerMaster(pStatistic, pParent);
    }
    public override IAddressSpaceDescriptor[] GetAvailiableAddressspaces()
    {
      return new IAddressSpaceDescriptor[] { new TextReaderAddressSpaceDescriptor() };
    }
    public override IItemDefaultSettings GetItemDefaultSettings(short AddressSpaceIdentifier, ulong AddressInTheAddressSpace)
    {
      return new ItemDefaultSettings(AddressSpaceIdentifier, AddressInTheAddressSpace);
    }
    #endregion

    #region private
    protected override void ReadSettings(XmlReader pSettings)
    {
      pSettings.ReadStartElement("StringFormat");
      string _StringFormat = pSettings.ReadContentAsString();
      pSettings.ReadEndElement();
    }
    protected override void WriteSettings(XmlWriter pSettings)
    {
      pSettings.WriteStartElement("StringFormat");
      pSettings.WriteValue("Dot delimiter");
      pSettings.WriteEndElement();
    }

    private class ItemDefaultSettings : IItemDefaultSettings
    {
      private ulong m_AddressInTheAddressSpace;
      private short m_AddressSpaceIdentifier;

      public ItemDefaultSettings(short addressSpaceIdentifier, ulong addressInTheAddressSpace)
      {
        this.m_AddressSpaceIdentifier = addressSpaceIdentifier;
        this.m_AddressInTheAddressSpace = addressInTheAddressSpace;
      }
      public ItemAccessRights AccessRights
      {
        get; private set;
      } = ItemAccessRights.ReadOnly;
      public Type[] AvailiableTypes
      {
        get; set;
      } = new Type[] { typeof(float) };
      public Type DefaultType
      {
        get; private set;
      } = typeof(float);
      public string Name
      {
        get
        {
          return $"Cplumn[{m_AddressInTheAddressSpace}]";
        }
      }
    }
    private class TextReaderAddressSpaceDescriptor : IAddressSpaceDescriptor
    {
      public long EndAddress { get; private set; } = byte.MaxValue;
      public short Identifier { get; private set; } = 0;
      public string Name { get; private set; } = "Text File";
      public long StartAddress { get; set; } = 0;
    }
    #endregion

  }

}
