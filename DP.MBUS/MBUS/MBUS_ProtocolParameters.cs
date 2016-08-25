//<summary>
//  Title   : MBUS Protocol parameters for ApplicationLayer
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

using System;
using System.ComponentModel;
using System.Xml;
using CAS.Lib.CommonBus.Xml;
using CAS.Lib.RTLib.Processes;

namespace CAS.Lib.CommonBus.ApplicationLayer
{
  /// <summary>
  /// Protocol parameters for ApplicationLayer
  /// </summary>
  [System.Serializable]
  public class MBUS_ProtocolParameters: ProtocolParameters
  {
    #region PRIVATE
    #region XML identifiers
    private const string m_Tag_CharacterTimeoutmin = "CharacterTimeoutmin";
    private const string m_Tag_CharacterTimeout = "CharacterTimeout";
    #endregion
    private TimeSpan ppCharacterTimeoutmin;
    private TimeSpan ppIntergrameGapMin;
    private TimeSpan ppCharacterTimeout;
    private void countTimeIntervals( uint portSpeed, out TimeSpan CharacterTimeOut, out TimeSpan InterframeGap )
    {
      if ( portSpeed > 19200 )
      {
        ppCharacterTimeoutmin = TimeSpan.FromMilliseconds( 1 );
        CharacterTimeOut = ppCharacterTimeoutmin;
      }
      else
      {
        double CharacterPerSecond = portSpeed / 11.0;
        // ustalmy minimalny timeout na dwa znaki
        ppCharacterTimeoutmin = Timer.FromUSeconds( 2000000 / CharacterPerSecond );
        CharacterTimeOut = ppCharacterTimeoutmin;
      }
      InterframeGap = TimeSpan.FromMilliseconds( 1 );
    }
    #endregion
    #region PUBLIC

    /// <summary>
    /// Gets a period of time that server is waiting for one character in the stream.
    /// </summary>
    /// <value>The character timeout <see cref="TimeSpan"/>.</value>
    [BrowsableAttribute( false )]
    public TimeSpan CharacterTimeoutSpan
    {
      get
      {
        return ppCharacterTimeout;
      }
    }
    /// <summary>
    /// Gets or sets a period of time that server is waiting for one character in the stream in us
    /// </summary>
    /// <remarks>In ms.</remarks>
    [DescriptionAttribute( "A period of time that server is waiting for one character in the stream in us" )]
    public uint CharacterTimeout
    {
      set
      {
        ppCharacterTimeout = Timer.Max( ppCharacterTimeoutmin, Timer.FromUSeconds( value ) );
      }
      get
      {
        return Timer.ToUSeconds( ppCharacterTimeout );
      }
    }
    /// <summary>
    /// Set all parameters according to the baudrate.
    /// </summary>
    public override uint LineSpeed
    {
      set
      {
        countTimeIntervals( value, out ppCharacterTimeoutmin, out ppIntergrameGapMin );
        ppCharacterTimeout = ppCharacterTimeoutmin;
        //ppInterfameGap = Math.Max(ppIntergrameGapMin,ppInterfameGap);
      }
    }
    /// <summary>
    /// Gets the ProtocolParameters description as a string. 
    /// </summary>
    /// <returns>Description in form: [ResponseTimeOut][InterframeGap][Timeout35][Timeout15]</returns>
    public override string ToString()
    {
      return base.ToString() + "\\CharacterTimeout=" + CharacterTimeout.ToString();
    }
    /// <summary>
    /// this function  writes settings to xml stream
    /// </summary>
    /// <param name="pSettings">XmlWriter strea</param>
    public override void WriteSettings( System.Xml.XmlWriter pSettings )
    {
      pSettings.WriteStartElement( m_TagClass );
      XmlHelper.WriteTimeInMicroseconds( pSettings, m_Tag_InterfameGap, ppInterfameGap );
      XmlHelper.WriteStandardIntegerVale( pSettings, m_Tag_NumberOfRetries, ppNumberOfRetries );
      XmlHelper.WriteTimeInMicroseconds( pSettings, m_Tag_CharacterTimeout, ppCharacterTimeout );
      XmlHelper.WriteTimeInMicroseconds( pSettings, m_Tag_CharacterTimeoutmin, ppCharacterTimeoutmin );
      XmlHelper.WriteTimeInMicroseconds( pSettings, m_Tag_TimeoutResponse, ppTimeoutResponse );
      pSettings.WriteEndElement();
    }
    /// <summary>
    /// this function  reads settings from xml stream
    /// </summary>
    /// <param name="pSettings">XmlReader strea</param>
    public override void ReadSettings( System.Xml.XmlReader pSettings )
    {
      if ( !pSettings.IsStartElement( m_TagClass ) )
        throw new XmlException
          ( string.Format( "Expected element {0} not found at current position of the configuration file", m_TagClass ) );
      ppInterfameGap = XmlHelper.ReadTimeFromMicroseconds( pSettings, m_Tag_InterfameGap );
      ppNumberOfRetries = (ushort)XmlHelper.ReadStandardIntegerValue( pSettings, m_Tag_NumberOfRetries );
      ppCharacterTimeout = XmlHelper.ReadTimeFromMicroseconds( pSettings, m_Tag_CharacterTimeout );
      ppCharacterTimeoutmin = XmlHelper.ReadTimeFromMicroseconds( pSettings, m_Tag_CharacterTimeoutmin );
      ppTimeoutResponse = XmlHelper.ReadTimeFromMicroseconds( pSettings, m_Tag_TimeoutResponse );
      pSettings.ReadStartElement( m_TagClass );
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="MBUS_ProtocolParameters"/> class.
    /// </summary>
    /// <param name="portSpeed">Baudrate of the communication line.</param>
    /// <param name="TimeoutResponse">Maximum response time this station is willing to wait.</param>
    /// <param name="NumberOfRetries">Maximum number of retries this station will try.</param>
    /// <param name="InterframeGapInMiliSeconds">The interframe gap in mili seconds.</param>
    public MBUS_ProtocolParameters( uint portSpeed, uint TimeoutResponse, ushort NumberOfRetries, uint InterframeGapInMiliSeconds )
      : base( portSpeed, TimeoutResponse, NumberOfRetries )
    {
      LineSpeed = portSpeed;
      ResponseTimeOut = TimeoutResponse;
      this.ppNumberOfRetries = NumberOfRetries;
      this.InterframeGap = InterframeGapInMiliSeconds;
    }
    #endregion
  } //ProtocolParameters
}
