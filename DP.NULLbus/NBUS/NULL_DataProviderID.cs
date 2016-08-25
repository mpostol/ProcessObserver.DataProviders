//<summary>
//  Title   : NULL plugin Data Provider inentifier
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//  20081003: mzbrzezny: AddressSpaceDescriptor implementation
//  20081003 mzbrzezny Item Default Settings are implemented
//  2007-04 mpostol created
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Lib.RTLib.Management;

namespace CAS.Lib.CommonBus.ApplicationLayer.NULL
{
  /// <summary>
  /// Data Provider Identifier for NULL Protocol
  /// </summary>
  public class NULL_DataProviderID: DataProviderID
  {
    #region private
    private int m_ErrorFrequency = 0;
    private const string m_Tag_ErrorFrequency = "ErrorFrequency";
    #endregion
    #region Settings
    /// <summary>
    /// the error frequency
    /// </summary>
    public int ErrorFrequency
    {
      get { return m_ErrorFrequency; }
      set { m_ErrorFrequency = value; }
    }
    #endregion
    #region DataProviderID implementation
    /// <summary>
    /// This function is writing Data Provider specifics settings to the XML stream.
    /// </summary>
    /// <param name="pSettings">XML stream to save settings <see cref="System.Xml.XmlWriter"/></param>
    protected override void WriteSettings( System.Xml.XmlWriter pSettings )
    {
      pSettings.WriteStartElement( m_Tag_ErrorFrequency );
      pSettings.WriteValue( m_ErrorFrequency );
      pSettings.WriteEndElement();
    }
    /// <summary>
    /// This function is reading Data Provider specifics settings from the XML stream.
    /// </summary>
    /// <param name="pSettings">Settings in the XML stream <see cref="System.Xml.XmlReader"/></param>
    protected override void ReadSettings( System.Xml.XmlReader pSettings )
    {
      m_ErrorFrequency = pSettings.ReadElementContentAsInt( m_Tag_ErrorFrequency, "" );
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
      ( IProtocolParent pStatistic, CommonBusControl pParent )
    {
      NULL_ApplicationLayerPluginHelper m_ApplicationLayerPluginHelper = new NULL_ApplicationLayerPluginHelper( pParent );
      return m_ApplicationLayerPluginHelper.CreateApplicationLayerMaster
        ( pStatistic, this.CreateCommunicationLayer( pParent ), m_ErrorFrequency );
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
    /// creates an object of NuLL DataProvider idetifier
    /// </summary>
    public NULL_DataProviderID()
    {
      this.Add( new CAS.Lib.CommonBus.CommunicationLayer.NULL.NullCommunicationLayerID() );
      this.Add( new CAS.Lib.CommonBus.CommunicationLayer.Net.NetCommunicationLayerID() );
      this.Add( new CAS.Lib.CommonBus.CommunicationLayer.RS.RSCommunicationLayerID() );

    }
    #endregion
  }
}
