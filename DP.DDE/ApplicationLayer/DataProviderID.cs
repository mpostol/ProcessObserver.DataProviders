//<summary>
//  Title   : DDE plugin Data Provider inentifier
//  System  : Microsoft Visual C# .NET
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//  20080905: mzbrzezny: Created based on BK.Plugin_NULLbus.csproj
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Lib.RTLib.Management;

namespace CAS.Lib.CommonBus.ApplicationLayer.DDE
{
  /// <summary>
  /// Data Provider Identifier for DDE Protocol
  /// </summary>
  public class DS_DataProviderID: DataProviderID
  {
    /// <summary>
    /// Supported Excel Languages
    /// </summary>
    public enum ExcelLanguageEnum:short
    {
      /// <summary>
      /// English 
      /// </summary>
      English=0,
      /// <summary>
      /// Polish
      /// </summary>
      Polish=1
    }
    #region private
    static private IAddressSpaceDescriptor[] m_AvailiableAddressSpaces = null;
    static private ExcelLanguageEnum ms_ExcelLanguage = ExcelLanguageEnum.English;
    private const string m_Tag_ExcelLanguage = "ExcelLanguage";
    private ProtocolParameters m_ProtocolParameters = new DDEProtocolParameters();
    #endregion
    #region Settings
    /// <summary>
    /// Gets or sets the excel language.
    /// </summary>
    /// <value>The excel language.</value>
    public ExcelLanguageEnum ExcelLanguage
    {
      get
      {
        return ms_ExcelLanguage;
      }
      set
      {
        ms_ExcelLanguage = value;
      }
    }
    #endregion
    #region DataProviderID implementation
    /// <summary>
    /// This function is writing Data Provider specifics settings to the XML stream.
    /// </summary>
    /// <param name="pSettings">XML stream to save settings <see cref="System.Xml.XmlWriter"/></param>
    protected override void WriteSettings( System.Xml.XmlWriter pSettings )
    {
      pSettings.WriteStartElement( m_Tag_ExcelLanguage );
      pSettings.WriteValue( (short)ms_ExcelLanguage );
      pSettings.WriteEndElement();
    }
    /// <summary>
    /// This function is reading Data Provider specifics settings from the XML stream.
    /// </summary>
    /// <param name="pSettings">Settings in the XML stream <see cref="System.Xml.XmlReader"/></param>
    protected override void ReadSettings( System.Xml.XmlReader pSettings )
    {
      ms_ExcelLanguage = (ExcelLanguageEnum)pSettings.ReadElementContentAsInt( m_Tag_ExcelLanguage, "" );
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
      PluginHelper m_ApplicationLayerPluginHelper = new PluginHelper( pParent );
      return m_ApplicationLayerPluginHelper.CreateApplicationLayerMaster
        ( pStatistic, this.CreateCommunicationLayer( pParent ), m_ProtocolParameters );
    }
    /// <summary>
    /// This function is responsible for returning the list of addressspaces in the data provider
    /// </summary>
    /// <returns>Hashtable with addressspaces</returns>
    public override IAddressSpaceDescriptor[] GetAvailiableAddressspaces()
    {
      return m_AvailiableAddressSpaces;
    }
    /// <summary>
    /// Gets the item default settings.
    /// </summary>
    /// <param name="AddressSpaceIdentifier">The address space identifier.</param>
    /// <param name="AddressInTheAddressSpace">The address in the address space.</param>
    /// <returns>default settings for the item</returns>
    public override IItemDefaultSettings GetItemDefaultSettings( short AddressSpaceIdentifier, ulong AddressInTheAddressSpace )
    {
      return new ItemDefaultSettings( AddressSpaceIdentifier, AddressInTheAddressSpace );
    }
    static internal ExcelLanguageEnum GetExcelLanguage()
    {
      return ms_ExcelLanguage;
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
      this.Add( new CAS.Lib.CommonBus.CommunicationLayer.DDE.DDECommunicationLayerID() );
    }
    static DS_DataProviderID()
    {
      string base_addressspace = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
      short addresspaces_count = (short)((short)base_addressspace.Length * (short)(base_addressspace.Length + 1));
      m_AvailiableAddressSpaces = new IAddressSpaceDescriptor[addresspaces_count];
      short idx_addresspace = 1;
      for (int i = 0; i < base_addressspace.Length; i++)
      {
        m_AvailiableAddressSpaces[idx_addresspace - 1] =
          new AddressSpaceDescriptor(base_addressspace[i].ToString(),
          idx_addresspace, 1, short.MaxValue);
        idx_addresspace++;
      }
      for (int j = 0; j < base_addressspace.Length; j++)
      {
        for (int i = 0; i < base_addressspace.Length; i++)
        {
          m_AvailiableAddressSpaces[idx_addresspace - 1] =
            new AddressSpaceDescriptor(base_addressspace[j].ToString() + base_addressspace[i].ToString(),
            idx_addresspace, 1, short.MaxValue);
          idx_addresspace++;
        }
      }
    }
    static internal IAddressSpaceDescriptor[] AvailiableAddressSpaces
    {
      get
      {
        return m_AvailiableAddressSpaces;
      }
    }
    #endregion
  }
}
