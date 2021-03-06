//<summary>
//  Title   : DataProviderID provide basic information about DP plugin
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//  20081006: mzbrzezny: AddressSpaceDescriptor and Item Default Settings are implemented.
//    mpostol 2007-04 created
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.ComponentModel;
using CAS.Lib.RTLib.Management;

namespace CAS.Lib.CommonBus.ApplicationLayer.SBUS
{
  /// <summary>
  /// DataProviderID provide basic information about DP plugin
  /// </summary>
  public class SBUS_DataProviderID: DataProviderID
  {
    #region private
    private ProtocolParameters m_ProtocolParameters = new SBUS_ProtocolParameters( 9600, 500, 5 );
    #endregion
    #region Settings
    /// <summary>
    /// Gets or sets the S bus protocol parameters.
    /// </summary>
    /// <value>The S bus protocol parameters.</value>
    [TypeConverterAttribute( typeof( ExpandableObjectConverter ) ), DisplayName( "Protocol Specific Parameters" )]
    public ProtocolParameters SBusProtocolParameters
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
    protected override void WriteSettings( System.Xml.XmlWriter pSettings )
    {
      m_ProtocolParameters.WriteSettings( pSettings );
    }
    /// <summary>
    /// This function is reading Data Provider specifics settings from the XML stream.
    /// </summary>
    /// <param name="pSettings">Settings in the XML stream <see cref="System.Xml.XmlReader"/></param>
    protected override void ReadSettings( System.Xml.XmlReader pSettings )
    {
      m_ProtocolParameters.ReadSettings( pSettings );
    }
    /// <summary>
    /// Instantiate object providing  <see cref="IApplicationLayerMaster"/> - an object implementing 
    /// master (playing the role on the field network of the master station,) interfaces defined for the application layer. 
    /// </summary>
    /// <param name="pStatistic">Statistical information about the communication performance.</param>
    /// <param name="pParent"><seealso cref="CommonBusControl"/> - Base class responsible for all of resources management used 
    /// by the component and providing tracing sources.</param>
    /// <returns>Return an object implementing IApplicationLayerMaster.</returns>
    /// <exception cref="System.ComponentModel.LicenseException">The type is licensed, but a <see cref="System.ComponentModel.License"/> cannot be granted.</exception>
    public override IApplicationLayerMaster GetApplicationLayerMaster
      ( IProtocolParent pStatistic, CommonBusControl pParent )
    {
      SBUS_ApplicationLayerPluginHelper m_ApplicationLayerPluginHelper = new SBUS_ApplicationLayerPluginHelper( pParent );
      return m_ApplicationLayerPluginHelper.CreateApplicationLayerMaster
        ( pStatistic, this.CreateCommunicationLayer( pParent ), m_ProtocolParameters );
    }
    /// <summary>
    /// This function is responsible for returning the list of addressspaces in the data provider
    /// </summary>
    /// <returns>Hashtable with addressspaces</returns>
    public override IAddressSpaceDescriptor[] GetAvailiableAddressspaces()
    {
      return GetAvailiableAddressspacesHelper( typeof( PRIVATE.Medium_T ), null );
    }
    /// <summary>
    /// Gets the item default settings.
    /// </summary>
    /// <param name="AddressSpaceIdentifier">The address space identifier.</param>
    /// <param name="AddressInTheAddressSpace">The address in the address space.</param>
    /// <returns>default settings for the item</returns>
    public override IItemDefaultSettings GetItemDefaultSettings( short AddressSpaceIdentifier, ulong AddressInTheAddressSpace )
    {
      return new ItemDefaultSettings( (PRIVATE.Medium_T)AddressSpaceIdentifier, AddressInTheAddressSpace );
    }
    #endregion
    #region creator
    /// <summary>
    /// Initializes a new instance of the <see cref="SBUS_DataProviderID"/> class.
    /// </summary>
    /// <remarks>
    /// Inheritor have to instantiate anobject providing  <see cref="IDataProviderID"/> - a helper interface allowing
    /// in turn to configure and instantiate another objects implementing interfaces defined for the application layer.
    /// </remarks>
    public SBUS_DataProviderID()
    {
      this.Add( new CAS.Lib.CommonBus.CommunicationLayer.Net.NetCommunicationLayerID() );
#if !SBUSNET
      this.Add( new CAS.Lib.CommonBus.CommunicationLayer.RS.RSCommunicationLayerID() );
#endif
    }
    #endregion
  }
}
