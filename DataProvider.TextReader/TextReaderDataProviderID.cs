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

    #region constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="TextReaderDataProviderID"/> class.
    /// </summary>
    /// <remarks>Inheritor have to instantiate an object providing  <see cref="T:CAS.Lib.CommonBus.IDataProviderID" /> - a helper interface allowing
    /// in turn to configure and instantiate another objects implementing interfaces defined for the application layer.</remarks>
    public TextReaderDataProviderID()
    {
      Add(new NullCommunicationLayerID());
    }
    #endregion

    #region DataProviderID
    /// <summary>
    /// When overridden in a derived class, instantiates object providing  <see cref="T:CAS.Lib.CommonBus.ApplicationLayer.IApplicationLayerMaster" /> - an object
    /// implementing master side (playing the role on the field network of the master station,) interface defined for the
    /// application layer.
    /// </summary>
    /// <param name="pStatistic">Statistical information about the communication performance.</param>
    /// <param name="pParent"><seealso cref="T:CAS.Lib.CommonBus.CommonBusControl" /> - Base class responsible for all of resources management used
    /// by the component.</param>
    /// <returns>Return an object implementing <see cref="IApplicationLayerMaster"/>.</returns>
    /// <exception cref="System.ArgumentNullException">
    /// </exception>
    public override IApplicationLayerMaster GetApplicationLayerMaster(IProtocolParent pStatistic, CommonBusControl pParent)
    {
      if (pStatistic == null)
        throw new ArgumentNullException($"{pStatistic} cannot be null");
      if (pParent == null)
        throw new ArgumentNullException($"{pParent} cannot be null");
      return new TextReaderApplicationLayerMaster(pStatistic, pParent, ProtocolParameters);
    }
    /// <summary>
    /// This metchod is responsible for returning the list of addressspaces in the data provider.
    /// </summary>
    /// <returns>It returns assay of <see cref="IAddressSpaceDescriptor"/> representing supported address spaces.</returns>
    public override IAddressSpaceDescriptor[] GetAvailiableAddressspaces()
    {
      return new IAddressSpaceDescriptor[] { new TextReaderAddressSpaceDescriptor() };
    }
    /// <summary>
    /// Gets the item default settings.
    /// </summary>
    /// <param name="AddressSpaceIdentifier">The address space identifier.</param>
    /// <param name="AddressInTheAddressSpace">The address in the address space.</param>
    /// <returns>default settings for the item</returns>
    public override IItemDefaultSettings GetItemDefaultSettings(short AddressSpaceIdentifier, ulong AddressInTheAddressSpace)
    {
      return new ItemDefaultSettings(AddressSpaceIdentifier, AddressInTheAddressSpace);
    }
    #endregion

    #region Settings
    /// <summary>
    /// Gets or sets the protocol parameters.
    /// </summary>
    /// <remarks>It is used by the UI to configure the DataProvider.</remarks>
    /// <value>The protocol parameters.</value>
    [TypeConverterAttribute(typeof(ExpandableObjectConverter))]
    [DisplayName("Parameters")]
    [Description("TextReader DataProvider Specific Parameters")]
    public TextReaderProtocolParameters ProtocolParameters
    {
      get; private set;
    } = new TextReaderProtocolParameters();
    #endregion

    #region private
    /// <summary>
    /// When overridden in a derived class, reads custom settings from the XML stream.
    /// </summary>
    /// <param name="pSettings">Custom settings in the <see cref="T:System.Xml.XmlReader" /></param>
    protected override void ReadSettings(XmlReader pSettings)
    {
      ProtocolParameters.ReadSettings(pSettings);
    }
    /// <summary>
    /// When overridden in a derived class, writes custom settings to the XML stream.
    /// </summary>
    /// <param name="pSettings"><see cref="T:System.Xml.XmlWriter" /> to save settings.</param>
    protected override void WriteSettings(XmlWriter pSettings)
    {
      ProtocolParameters.WriteSettings(pSettings);
    }
    private class ItemDefaultSettings : IItemDefaultSettings
    {

      //private
      private ulong m_AddressInTheAddressSpace;
      private short m_AddressSpaceIdentifier;

      //public
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
      public long StartAddress { get; private set; } = 0;
    }
    #endregion

  }

}
