//_______________________________________________________________
//  Title   : TextReaderDataProviderID
//  System  : Microsoft VisualStudio 2015 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
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
using System.ComponentModel;
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
      return new TextReaderApplicationLayerMaster(pStatistic, pParent, ProtocolParameters);
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

    #region Settings
    /// <summary>
    /// Gets or sets the S bus protocol parameters.
    /// </summary>
    /// <value>The S bus protocol parameters.</value>
    [TypeConverterAttribute(typeof(ExpandableObjectConverter))]
    [DisplayName("Parameters")]
    [Description("TextReader DataProvider Specific Parameters")]
    public TextReaderProtocolParameters ProtocolParameters
    {
      get; private set;
    } = new TextReaderProtocolParameters();
    #endregion

    #region private
    protected override void ReadSettings(XmlReader pSettings)
    {
      ProtocolParameters.ReadSettings(pSettings);
    }
    protected override void WriteSettings(XmlWriter pSettings)
    {
      ProtocolParameters.WriteSettings(pSettings);
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
      } = new Type[] { typeof(string), typeof(float), typeof(long), typeof(int) };

      public Type DefaultType
      {
        get; private set;
      } = typeof(string);
      public string Name
      {
        get
        {
          return $"Column[{m_AddressInTheAddressSpace}]";
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
