//_______________________________________________________________
//  Title   : NameMODBUS Data Provider Identifier class
//  System  : Microsoft VisualStudio 2015 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2016, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________


using CAS.CommServer.DataProvider.MODBUSCore;
using CAS.Lib.CommonBus.ApplicationLayer.Modbus;
using CAS.Lib.RTLib.Management;
using System.ComponentModel;

namespace CAS.Lib.CommonBus.ApplicationLayer.ModBus
{
  /// <summary>
  /// MODBUS Data Provider Identifier class
  /// </summary>
  public class ModBus_DataProviderID : DataProviderID
  {
    #region private
    private ModBus_ProtocolParameters m_ProtocolParameters = new ModBus_ProtocolParameters(9600, 500, 5);
    #endregion

    #region Settings
    /// <summary>
    /// Protocol Parameters class for Modbus
    /// </summary>
    [TypeConverterAttribute(typeof(ExpandableObjectConverter))]
    public ModBus_ProtocolParameters MODBusProtocolParameters
    {
      get { return m_ProtocolParameters; }
      set { m_ProtocolParameters = value; }
    }
    #endregion

    #region DataProviderID implementation
    /// <summary>
    /// This function is writing Data Provider specifics settings to the XML stream.
    /// </summary>
    /// <param name="pSettings">XML stream to save settings <see cref="System.Xml.XmlWriter"/></param>
    protected override void WriteSettings(System.Xml.XmlWriter pSettings)
    {
      m_ProtocolParameters.WriteSettings(pSettings);
    }
    /// <summary>
    /// This function is reading Data Provider specifics settings from the XML stream.
    /// </summary>
    /// <param name="pSettings">Settings in the XML stream <see cref="System.Xml.XmlReader"/></param>
    protected override void ReadSettings(System.Xml.XmlReader pSettings)
    {
      m_ProtocolParameters.ReadSettings(pSettings);
    }
    /// <summary>
    /// Instantiate object providing  <see cref="IApplicationLayerMaster"/> - an object implementing 
    /// master (playing the role on the field network of the master station,) interfaces defined for the application layer. 
    /// </summary>
    /// <param name="pStatistic">Statistical information about the communication performance.</param>
    /// <param name="pParent"><seealso cref="CommonBusControl"/> - Base class responsible for all of resources management used 
    /// by the component and providing tracing sources.</param>
    /// <returns>Return an object implementing IApplicationLayerMaster.</returns>
    public override IApplicationLayerMaster GetApplicationLayerMaster(IProtocolParent pStatistic, CommonBusControl pParent)
    {
      ModBus_ApplicationLayerPluginHelper m_ApplicationLayerPluginHelper = new ModBus_ApplicationLayerPluginHelper();
      return m_ApplicationLayerPluginHelper.CreateApplicationLayerMaster(pStatistic, this.CreateCommunicationLayer(pParent), m_ProtocolParameters);
    }
    /// <summary>
    /// This function is responsible for returning the list of address spaces in the data provider
    /// </summary>
    /// <returns>Hashtable with address spaces</returns>
    public override IAddressSpaceDescriptor[] GetAvailiableAddressspaces()
    {
      return GetAvailiableAddressspacesHelper(typeof(Medium_T), null);
    }
    /// <summary>
    /// Gets the item default settings.
    /// </summary>
    /// <param name="AddressSpaceIdentifier">The address space identifier.</param>
    /// <param name="AddressInTheAddressSpace">The address in the address space.</param>
    /// <returns>default settings for the item</returns>
    public override IItemDefaultSettings GetItemDefaultSettings(short AddressSpaceIdentifier, ulong AddressInTheAddressSpace)
    {
      return new ItemDefaultSettings((Medium_T)AddressSpaceIdentifier, AddressInTheAddressSpace);
    }
    #endregion

    #region creator
    /// <summary>
    /// Creator of Modbus Data Provider ID
    /// </summary>
    public ModBus_DataProviderID()
    {
      this.Add(new CommunicationLayer.Net.NetCommunicationLayerID());
    }
    #endregion

  }
}
