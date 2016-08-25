//<summary>
//  Title   : DDE Protocol parameters for ApplicationLayer
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  History :
//    2008-06-13: mzbrzezny: new function that comes from XML helper are used see: itr:[COM-801]
//    20080519: mzbrzezny: created based on MBUS plugin
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.Xml;

namespace CAS.Lib.CommonBus.ApplicationLayer
{
  /// <summary>
  /// Protocol parameters for ApplicationLayer
  /// </summary>
  [System.Serializable]
  public class DDEProtocolParameters: ProtocolParameters
  {
    #region PRIVATE
    #endregion
    #region public
    /// <summary>
    /// Set all parameters according to the baudrate.
    /// </summary>
    /// <value></value>
    public override uint LineSpeed
    {
      set { }
    }
    /// <summary>
    /// Gets the ProtocolParameters description as a string. 
    /// </summary>
    /// <returns>Description in form: [ResponseTimeOut][InterframeGap][Timeout35][Timeout15]</returns>
    public override string ToString()
    {
      return base.ToString();
    }
    /// <summary>
    /// this function  writes settings to xml stream
    /// </summary>
    /// <param name="pSettings">XmlWriter strea</param>
    public override void WriteSettings( System.Xml.XmlWriter pSettings )
    {
      pSettings.WriteStartElement( m_TagClass );
      pSettings.WriteEndElement();
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="DDEProtocolParameters"/> class.
    /// </summary>
    public override void ReadSettings( System.Xml.XmlReader pSettings )
    {
      if ( !pSettings.IsStartElement( m_TagClass ) )
        throw new XmlException
          ( string.Format( "Expected element {0} not found at current position of the configuration file", m_TagClass ) );
      pSettings.ReadStartElement( m_TagClass );
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="DDEProtocolParameters"/> class.
    /// </summary>
    public DDEProtocolParameters()
      : base( 0, 0, 1 )
    {
    }
    #endregion
  } //ProtocolParameters
}
