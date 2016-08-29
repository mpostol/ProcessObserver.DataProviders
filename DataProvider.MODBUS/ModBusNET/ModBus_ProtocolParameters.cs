//<summary>
//  Title   : MODBUS Protocol parameters for ApplicationLayer
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    2008-06-20: mzbrzezny: unnecessary settings are removed (T15,T35)
//    2008-06-19: mzbrzezny: Modbus uses now ulong to store number of ticks instead of uint
//    2008-06-13: mzbrzezny: new function that comes from XML helper are used see: itr:[COM-801]
//    Mzbrzezny - created 20070521 based on ProtocolParameter
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.CommServer.DataProvider.MODBUSCore;
using CAS.Lib.CommonBus.Xml;
using CAS.Lib.RTLib.Processes;
using System;
using System.ComponentModel;
using System.Xml;

namespace CAS.Lib.CommonBus.ApplicationLayer.Modbus
{
  /// <summary>
  /// Protocol parameters for ApplicationLayer
  /// </summary>
  [System.Serializable]
  public class ModBus_ProtocolParameters: ModBus_CommonProtocolParameters
  {

    #region PRIVATE
    #region XML identifiers
    private const string m_Tag_TimeoutCharacter = "TimeoutCharacter";
    #endregion
    private TimeSpan ppTimeoutCharacter;
    #endregion

    #region PUBLIC
    /// <summary>
    /// Gets a time interval equal to at least 1.5 characters in internal ticks.
    /// </summary>
    /// <value>The timeout character <see cref="TimeSpan"/>.</value>
    [BrowsableAttribute( false )]
    public TimeSpan TimeoutCharacterSpan
    {
      get
      {
        return ppTimeoutCharacter;
      }
    }
    /// <summary>
    /// Gets and sets a time interval equal to at least 1.5 characters in us.
    /// </summary>
    /// <remarks>In ms.</remarks>
    [DescriptionAttribute( "Gets and sets a time interval equal to at least 1.5 characters in us." )]
    public uint TimeoutCharacter
    {
      set { ppTimeoutCharacter = Timer.FromUSeconds( value ); }
      get { return Timer.ToUSeconds( ppTimeoutCharacter ); }
    }
    /// <summary>
    /// Set all parameters according to the baud rate.
    /// </summary>
    public override uint LineSpeed
    {
      set
      {
        //TODO it is intentionally empty??;
      }
    }
    /// <summary>
    /// Gets the ProtocolParameters description as a string. 
    /// </summary>
    /// <returns>Description in form: [ResponseTimeOut][InterframeGap][Timeout35][Timeout15]</returns>
    public override string ToString()
    {
      return
        base.ToString() +
        "\\TimeoutCharacter=" + TimeoutCharacter.ToString();
    }
    /// <summary>
    /// this function  writes settings to xml stream
    /// </summary>
    /// <param name="pSettings">XmlWriter stream</param>
    public override void WriteSettings( System.Xml.XmlWriter pSettings )
    {
      pSettings.WriteStartElement( m_TagClass );
      XmlHelper.WriteTimeInMicroseconds( pSettings, m_Tag_InterfameGap, ppInterfameGap );
      XmlHelper.WriteStandardIntegerVale( pSettings, m_Tag_NumberOfRetries, ppNumberOfRetries );
      XmlHelper.WriteTimeInMicroseconds( pSettings, m_Tag_TimeoutCharacter, ppTimeoutCharacter );
      XmlHelper.WriteTimeInMicroseconds( pSettings, m_Tag_TimeoutResponse, ppTimeoutResponse );
      CommonWriteSettings( pSettings );
      pSettings.WriteEndElement();
    }
    /// <summary>
    /// this function  reads settings from xml stream
    /// </summary>
    /// <param name="settings">XmlReader stream</param>
    public override void ReadSettings( XmlReader settings )
    {
      if ( !settings.IsStartElement( m_TagClass ) )
        throw new XmlException
          ( string.Format( "Expected element {0} not found at current position of the configuration file", m_TagClass ) );
      ppInterfameGap = XmlHelper.ReadTimeFromMicroseconds( settings, m_Tag_InterfameGap );
      ppNumberOfRetries = (ushort)XmlHelper.ReadStandardIntegerValue( settings, m_Tag_NumberOfRetries );
      ppTimeoutCharacter = XmlHelper.ReadTimeFromMicroseconds( settings, m_Tag_TimeoutCharacter );
      ppTimeoutResponse = XmlHelper.ReadTimeFromMicroseconds( settings, m_Tag_TimeoutResponse );
      CommonReadSettings( settings );
      settings.ReadStartElement( m_TagClass );
    }
    /// <summary>
    /// Creator
    /// </summary>
    /// <param name="portSpeed">Baud rate of the communication line.</param>
    /// <param name="TimeoutResponse">Maximum response time this station is willing to wait.</param>
    /// <param name="NumberOfRetries">Maximum number of retries this station will try.</param>
    public ModBus_ProtocolParameters( uint portSpeed, uint TimeoutResponse, ushort NumberOfRetries )
      : base( portSpeed, TimeoutResponse, NumberOfRetries )
    {
      ResponseTimeOut = TimeoutResponse;
      this.ppNumberOfRetries = NumberOfRetries;
    }
    #endregion

  } //ProtocolParameters
}
