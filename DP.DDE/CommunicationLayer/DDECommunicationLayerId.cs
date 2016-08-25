//<summary>
//  Title   : DDECommunicationLayerDescription
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    2008 created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.ComponentModel;
using System.Xml;
using CAS.Lib.CommonBus.Xml;

namespace CAS.Lib.CommonBus.CommunicationLayer.DDE
{
  /// <summary>
  /// Description class for the RS communication layer
  /// </summary>
  public class DDECommunicationLayerDescription: CommunicationLayer.CommunicationLayerDescription
  {
    private string m_settings = "Excel";
    internal void SetSettings( string pTraceName )
    {
      m_settings = Title + "," + pTraceName;
    }
    #region ICommunicationLayerId Members
    /// <summary>
    /// Gets a title for the ICommunicationLayer provider.
    /// </summary>
    public override string Title
    {
      get { return "DDE"; }
    }
    /// <summary>
    /// Gets a description for the ICommunicationLayer provider.
    /// </summary>
    public override string Description
    {
      get { return "DDE implementation of the ICommunicationLayer"; }
    }
    /// <summary>
    /// returns settings of the communication layer in human readable form
    /// </summary>
    public override string HumanReadableSettings
    {
      get { return m_settings; }
    }
    #endregion
  }
  /// <summary>
  /// Identifier for the Null communication layer
  /// </summary>
  public class DDECommunicationLayerID: ICommunicationLayerId
  {
    #region private
    private const string m_Tag_Class = "DDECommunicationLayerID";
    private const string m_Tag_ServerDDE = "ServerDDE";
    string m_ServerDDE = "Excel";
    private const string m_Tag_RequestTimeout = "RequestTimeout";
    int m_RequestTimeout = 500;
    private const string m_Tag_PokeTimeout = "PokeTimeout";
    int m_PokeTimeout = 500;
    private DDECommunicationLayerDescription m_DDECommunicationLayerDescription = new DDECommunicationLayerDescription();
    #endregion
    #region ICommunicationLayerId Members
    /// <summary>
    /// Gets a title for the ICommunicationLayer provider.
    /// </summary>
    public string Title { get { return m_DDECommunicationLayerDescription.Title; } }
    /// <summary>
    /// Gets the short text description for the communication layer.
    /// </summary>
    [DisplayName( "Communication Layer" )]
    [DescriptionAttribute( "The short text description for the communication layer." )]
    [TypeConverterAttribute( typeof( ExpandableObjectConverter ) )]
    public ICommunicationLayerDescription GetCommunicationLayerDescription { get { return m_DDECommunicationLayerDescription; } }
    /// <summary>
    /// Instantiate new object providing <see>ICommunicationLayer</see> functionality.
    /// </summary>
    /// <param name="pParent">Base class responsible for the resources management.</param>
    /// An object providing the <returns><see>ICommunicationLayer</see>functionality</returns>
    public ICommunicationLayer CreateCommunicationLayer( CommonBusControl pParent )
    {
      m_DDECommunicationLayerDescription.SetSettings( m_ServerDDE );
      return new DDE_to_Serial( m_ServerDDE, pParent );
    }
    /// <summary>
    /// Instantiate new object providing <see>ICommunicationLayer</see> functionality.
    /// </summary>
    /// <param name="param">list of parameters that are required to create a communication layer. 
    /// This list is specific for the communication layer:
    /// param[0] is string pTraceName, param[1] is CommonBusControl pParent </param>
    /// <returns> An object providing the <see>ICommunicationLayer</see>functionality</returns>
    public ICommunicationLayer CreateCommunicationLayer( object[] param )
    {
      return new DDE_to_Serial( param );
    }
    void ICommunicationLayerId.GetSettings( System.Xml.XmlWriter pSettings )
    {
      pSettings.WriteStartElement( m_Tag_Class );
      pSettings.WriteAttributeString( m_Tag_ServerDDE, m_ServerDDE );
      XmlHelper.WriteStandardIntegerVale( pSettings, m_Tag_RequestTimeout, m_RequestTimeout );
      XmlHelper.WriteStandardIntegerVale( pSettings, m_Tag_PokeTimeout, m_PokeTimeout );
      pSettings.WriteEndElement();
    }
    void ICommunicationLayerId.SetSettings( System.Xml.XmlReader pSettings )
    {
      if ( !pSettings.IsStartElement( m_Tag_Class ) )
        throw new XmlException
          ( string.Format( "Expected element {0} not found at current position of the configuration file", m_Tag_Class ) );
      m_ServerDDE = XmlHelper.ReadattributeString( pSettings, m_Tag_ServerDDE );
      m_DDECommunicationLayerDescription.SetSettings( m_ServerDDE );
      m_RequestTimeout = XmlHelper.ReadStandardIntegerValue( pSettings, m_Tag_RequestTimeout );
      m_PokeTimeout = XmlHelper.ReadStandardIntegerValue( pSettings, m_Tag_PokeTimeout );
      pSettings.ReadStartElement( m_Tag_Class );
    }
    #endregion
    #region Settings
    /// <summary>
    /// Gets or sets the name of the layer.
    /// </summary>
    /// <value>The name of the layer.</value>
    public string ServerDDE
    {
      get { return m_ServerDDE; }
      set { m_ServerDDE = value; }
    }
    /// <summary>
    /// Gets or sets the poke timeout.
    /// </summary>
    /// <value>The poke timeout.</value>
    public int PokeTimeout
    {
      get
      {
        return m_PokeTimeout;
      }
      set
      {
        m_PokeTimeout = value;
      }
    }
    /// <summary>
    /// Gets or sets the request timeout.
    /// </summary>
    /// <value>The request timeout.</value>
    public int RequestTimeout
    {
      get
      {
        return m_RequestTimeout;
      }
      set
      {
        m_RequestTimeout = value;
      }
    }
    #endregion
  }
}
