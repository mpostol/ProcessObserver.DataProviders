//<summary>
//  Title   : MBUS Data Provider Identifier class
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  History :
//  20081006: mzbrzezny: AddressSpaceDescriptor and Item Default Settings are implemented.
//    20080519: mzbrzezny: created based on MBUS prlugin
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.ComponentModel;

namespace CAS.Lib.CommonBus.ApplicationLayer.MBUS
{
  /// <summary>
  /// MBUS Data Provider Identifier class
  /// </summary>
  public class MBUS_DataProviderID: DataProviderID
  {
    #region private
    //z przeprowadzonych eksperymentow na Multical66 (predkosc 2400) wynika, ze trzeba robic około 10 s 
    //przerwy przed zapytaniem kolejnego licznika (InterframeGap)
    //jeżeli chodzi o response time to pomiary wskazywaly: 63\64\75 (Mn\Av\Mx) [ms]
    //character gap: 53\4337\36387 (Mn\Av\Mx) [us] 
    private ProtocolParameters m_ProtocolParameters = new MBUS_ProtocolParameters( 2400, 1000, 2, 10000 );
    #endregion
    #region Settings
    /// <summary>
    /// Gets or sets the MBUS protocol parameters.
    /// </summary>
    /// <value>The MBUS protocol parameters.</value>
    [TypeConverterAttribute( typeof( ExpandableObjectConverter ) )]
    public ProtocolParameters MBUSProtocolParameters
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
    public override IApplicationLayerMaster GetApplicationLayerMaster
      ( CAS.Lib.RTLib.Management.IProtocolParent pStatistic, CommonBusControl pParent )
    {
      MBUS_ApplicationLayerPluginHelper m_ApplicationLayerPluginHelper = new MBUS_ApplicationLayerPluginHelper( pParent );
      return m_ApplicationLayerPluginHelper.CreateApplicationLayerMaster
        ( pStatistic, this.CreateCommunicationLayer( pParent ), m_ProtocolParameters );
    }
    /// <summary>
    /// This function is responsible for returning the list of addressspaces in the data provider
    /// </summary>
    /// <returns>Hashtable with addressspaces</returns>
    public override IAddressSpaceDescriptor[] GetAvailiableAddressspaces()
    {
      return GetAvailiableAddressspacesHelper( typeof( PRIVATE.MediumData ), null );
    }
    /// <summary>
    /// Gets the item default settings.
    /// </summary>
    /// <param name="AddressSpaceIdentifier">The address space identifier.</param>
    /// <param name="AddressInTheAddressSpace">The address in the address space.</param>
    /// <returns>default settings for the item</returns>
    public override IItemDefaultSettings GetItemDefaultSettings( short AddressSpaceIdentifier, ulong AddressInTheAddressSpace )
    {
      return new ItemDefaultSettings( (PRIVATE.MediumData)AddressSpaceIdentifier, AddressInTheAddressSpace );
    }
    #endregion
    #region creator
    /// <summary>
    /// Creator of MBUS Data Provider ID
    /// </summary>
    public MBUS_DataProviderID()
    {
      this.Add( new CAS.Lib.CommonBus.CommunicationLayer.RS.RSCommunicationLayerID() );
      this.Add( new CAS.Lib.CommonBus.CommunicationLayer.Net.NetCommunicationLayerID() );
    }
    #endregion
  }
}
