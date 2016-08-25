//<summary>
//  Title   : MODBUS Protocol parameters for ApplicationLayer
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    20080917: mzbrzezny: modbus implementation for CountTimeIntervals based on experiments with VersaMax
//    2008-06-19: mzbrzezny: Modbus uses now ulong to store number of ticks instead of uint
//    2008-06-13: mzbrzezny: new function that comes from XML helper are used see: itr:[COM-801]
//    Mzbrzezny - created 20070521 based on ProtocolParameter
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.ComponentModel;
using System.Xml;
using CAS.Lib.CommonBus.ApplicationLayer.ModBus.Common;
using CAS.Lib.CommonBus.Xml;
using CAS.Lib.RTLib.Processes;

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
    private const string m_Tag_Timeout35min = "Timeout35min";
    private const string m_Tag_Timeout15min = "Timeout15min";
    private const string m_Tag_Timeout35 = "Timeout35";
    private const string m_Tag_Timeout15 = "Timeout15";
    #endregion
    private TimeSpan ppTimeout35min;
    private TimeSpan ppTimeout15min;
    private TimeSpan ppTimeout35;
    private TimeSpan ppTimeout15;
    #endregion
    #region PUBLIC
    /// <summary>
    /// Counts the time intervals acording to Modbus rules.
    /// 
    /// based on experiments (20080917)with modbus implementation
    /// on Ge Fanuc VersaMax IC200UUD064 we need the folowing timings in ms:
    /// 
    ///   br  | t15 | t35
    ///  -----------------
    ///  9600 | 11  | 26
    /// 19200 |  6  | 14
    /// 38400 |  3  |  7
    /// 
    /// 
    /// </summary>
    /// <param name="portSpeed">The port speed [baudrate].</param>
    /// <param name="t35">The time of 3.5 characters.</param>
    /// <param name="t15">The time od 1.5 characters.</param>
    protected override void CountTimeIntervals( uint portSpeed, out TimeSpan t35, out TimeSpan t15 )
    {
      if ( portSpeed > 19200 )
      {
        t15 = TimeSpan.FromMilliseconds( 3 );
        t35 = TimeSpan.FromMilliseconds( 7 );
        return;
      }
      if ( portSpeed > 14400 )
      {
        t15 = TimeSpan.FromMilliseconds( 6 );
        t35 = TimeSpan.FromMilliseconds( 14 );
        return;
      }
      if ( portSpeed > 9600 )
      {
        t15 = TimeSpan.FromMilliseconds( 9 );
        t35 = TimeSpan.FromMilliseconds( 20 );
        return;
      }
      else
      {
        //double ticksPerCh = portSpeed / 11;
        //double timeofonecharacter = 1000.0 / ticksPerCh;
        //t35 = TimeSpan.FromMilliseconds( timeofonecharacter * 35 / 10 );
        //t15 = TimeSpan.FromMilliseconds( timeofonecharacter * 15 / 10 );
        t15 = TimeSpan.FromMilliseconds( 11 );
        t35 = TimeSpan.FromMilliseconds( 26 );
      }
    }
    /// <summary>
    /// Gets a time interval equal to at least 1.5 characters.
    /// </summary>
    /// <value>The timeout15 <see cref="TimeSpan"/>.</value>
    [BrowsableAttribute( false )]
    public TimeSpan Timeout15Span
    {
      get
      {
        return ppTimeout15;
      }
    }
    /// <summary>
    /// Gets and sets a time interval equal to at least 1.5 characters in us.
    /// </summary>
    /// <remarks>In us.</remarks>
    [DescriptionAttribute( "Gets and sets a time interval equal to at least 1.5 characters in us." )]
    public uint Timeout15
    {
      set
      {
        ppTimeout15 = Timer.Max( ppTimeout15min, Timer.FromUSeconds( value ) );
        ppTimeout35 = Timer.Max( CountTimeout35( ppTimeout15 ), ppTimeout35 );
        ppInterfameGap = Timer.Max( ppInterfameGap, ppTimeout35 );
      }
      get
      {
        return Timer.ToUSeconds( ppTimeout15 );
      }
    }
    /// <summary>
    /// Gets a time interval equal to at least 3.5 characters.
    /// </summary>
    /// <value>The timeout35 span.</value>
    [BrowsableAttribute( false )]
    public TimeSpan Timeout35Span
    {
      get
      {
        return ppTimeout35;
      }
    }
    /// <summary>
    /// Gets a time interval equal to at least 3.5 characters in us
    /// </summary>
    /// <remarks>In us.</remarks>
    [DescriptionAttribute( "Gets and sets a time interval equal to at least 3.5 characters in us." )]
    public uint Timeout35
    {
      set
      {
        TimeSpan minValue = Timer.Max( Timer.FromUSeconds( value ), ppTimeout35min );
        ppTimeout35 = Timer.Max( minValue, CountTimeout35( ppTimeout15 ) );
        ppInterfameGap = Timer.Max( ppInterfameGap, ppTimeout35 );
      }
      get
      {
        return Timer.ToUSeconds( ppTimeout35 );
      }
    }
    /// <summary>
    /// Set all parameters according to the baudrate.
    /// </summary>
    public override uint LineSpeed
    {
      set
      {
        CountTimeIntervals( value, out ppTimeout35min, out ppTimeout15min );
        ppTimeout35 = ppTimeout35min;
        ppTimeout15 = ppTimeout15min;
        ppInterfameGap = ppTimeout35;
      }
    }
    /// <summary>
    /// Gets the ProtocolParameters description as a string. 
    /// </summary>
    /// <returns>Description in form: [ResponseTimeOut][InterframeGap][Timeout35][Timeout15]</returns>
    public override string ToString()
    {
      return base.ToString() + "\\T35=" + Timeout35.ToString() + "\\T15=" + Timeout15.ToString();
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
      XmlHelper.WriteTimeInMicroseconds( pSettings, m_Tag_Timeout15, ppTimeout15 );
      XmlHelper.WriteTimeInMicroseconds( pSettings, m_Tag_Timeout15min, ppTimeout15min );
      XmlHelper.WriteTimeInMicroseconds( pSettings, m_Tag_Timeout35, ppTimeout35 );
      XmlHelper.WriteTimeInMicroseconds( pSettings, m_Tag_Timeout35min, ppTimeout35min );
      XmlHelper.WriteTimeInMicroseconds( pSettings, m_Tag_TimeoutResponse, ppTimeoutResponse );
      CommonWriteSettings( pSettings );
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
      ppTimeout15 = XmlHelper.ReadTimeFromMicroseconds( pSettings, m_Tag_Timeout15 );
      ppTimeout15min = XmlHelper.ReadTimeFromMicroseconds( pSettings, m_Tag_Timeout15min );
      ppTimeout35 = XmlHelper.ReadTimeFromMicroseconds( pSettings, m_Tag_Timeout35 );
      ppTimeout35min = XmlHelper.ReadTimeFromMicroseconds( pSettings, m_Tag_Timeout35min );
      ppTimeoutResponse = XmlHelper.ReadTimeFromMicroseconds( pSettings, m_Tag_TimeoutResponse );
      CommonReadSettings( pSettings );
      pSettings.ReadStartElement( m_TagClass );
    }
    /// <summary>
    /// Creator
    /// </summary>
    /// <param name="portSpeed">Baudrate of the communication line.</param>
    /// <param name="TimeoutResponse">Maximum response time this station is willing to wait.</param>
    /// <param name="NumberOfRetries">Maximum number of retries this station will try.</param>
    public ModBus_ProtocolParameters( uint portSpeed, uint TimeoutResponse, ushort NumberOfRetries )
      : base( portSpeed, TimeoutResponse, NumberOfRetries )
    {
      LineSpeed = portSpeed;
      ResponseTimeOut = TimeoutResponse;
      this.ppNumberOfRetries = NumberOfRetries;
    }
    #endregion
  } //ProtocolParameters
}
