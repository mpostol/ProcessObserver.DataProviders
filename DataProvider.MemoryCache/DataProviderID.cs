//<summary>
//  Title   : DemoSimulator plugin Data Provider inentifier
//  System  : Microsoft Visual C# .NET
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//  20081006: mzbrzezny: AddressSpaceDescriptor and Item Default Settings are implemented.
//  20080905: mzbrzezny: Created based on BK.Plugin_NULLbus.csproj
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.ComponentModel;
using CAS.Lib.RTLib.Management;

namespace CAS.Lib.CommonBus.ApplicationLayer.DemoSimulator
{
  /// <summary>
  /// Data Provider Identifier for DemoSimulator Protocol
  /// </summary>
  public class DS_DataProviderID: DataProviderID
  {
    #region private
    private int m_ErrorFrequency = 0;
    private const string m_Tag_ErrorFrequency = "ErrorFrequency";
    #endregion
    #region Settings
    /// <summary>
    /// Gets or sets the error frequency.
    /// </summary>
    /// <value>The error frequency.</value>
    [
    BrowsableAttribute( true ),
    DisplayName("Errors [%]"),
    CategoryAttribute( "Global Settings" ),
    DefaultValueAttribute( "0" ),
    DescriptionAttribute( "Percents of errors in communication. Percents of lost packets, etc..." )
    ]
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
      ApplicationLayerPluginHelper m_ApplicationLayerPluginHelper = new ApplicationLayerPluginHelper( pParent );
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
    /// Initializes a new instance of the <see cref="DS_DataProviderID"/> class.
    /// </summary>
    /// <remarks>
    /// Inheritor have to instantiate anobject providing  <see cref="IDataProviderID"/> - a helper interface allowing
    /// in turn to configure and instantiate another objects implementing interfaces defined for the application layer.
    /// </remarks>
    public DS_DataProviderID()
    {
      this.Add( new CAS.Lib.CommonBus.CommunicationLayer.NULL.NullCommunicationLayerID() );
    }
    #endregion
  }
}
