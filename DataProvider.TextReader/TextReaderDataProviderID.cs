//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

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
  /// <summary>
  /// Class TextReaderDataProviderID. This class cannot be inherited.
  /// It captures information that can be used for the DataProvider identification.
  /// </summary>
  /// <seealso cref="CAS.Lib.CommonBus.DataProviderID" />
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

    #endregion constructor

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
    /// This method is responsible for returning the list of address spaces in the data provider.
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

    #endregion DataProviderID

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

    #endregion Settings

    #region DataProviderID

    /// <summary>
    /// This method creates a string representation of the DataProvider settings
    /// </summary>
    /// <returns>human readable information about the DataProvider settings</returns>
    public override string GetSettingsHumanReadableFormat()
    {
      return $"{ProtocolParameters} {base.GetSettingsHumanReadableFormat()}";
    }

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

    #endregion DataProviderID

    #region private

    private class ItemDefaultSettings : IItemDefaultSettings
    {
      //private
      private readonly ulong m_AddressInTheAddressSpace;

      private readonly short m_AddressSpaceIdentifier;

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

      public string Name => $"Column[{m_AddressInTheAddressSpace}]";
    }

    private class TextReaderAddressSpaceDescriptor : IAddressSpaceDescriptor
    {
      public long EndAddress { get; private set; } = byte.MaxValue;
      public short Identifier { get; private set; } = 0;
      public string Name { get; private set; } = "Text File";
      public long StartAddress { get; private set; } = 0;
    }

    #endregion private
  }
}