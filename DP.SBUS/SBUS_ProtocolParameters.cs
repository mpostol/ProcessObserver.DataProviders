//<summary>
//  Title   : SBUS Protocol parameters for ApplicationLayer
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    2008-06-13: mzbrzezny: after fixes in itr:[COM-801] it turns out that there are many problems with changing 
//                           the setting of character tiemout, values are too big to store as uint
//                           now all parameters are stored as ulong  
//    2008-06-13: mzbrzezny: new function that comes from XML helper are used see: itr:[COM-801]
//    Mzbrzezny - created 20070521 based on ProtocolParameter
//
//  Copyright (C) 2013, CAS LODZ POLAND.
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
  public class SBUS_ProtocolParameters: ProtocolParameters
  {
    #region PRIVATE
    #region XML identifiers
    private const string m_Tag_TimeoutAfterFramemin = "TimeoutAfterFramemin";
    private const string m_Tag_TimeoutAfterFrame = "TimeoutAfterFrame";
    #endregion
    private TimeSpan ppTimeoutAfterFramemin;
    private TimeSpan ppTimeoutAfterFrame;
    #endregion
    #region PUBLIC
    /// <summary>
    /// Time that elaps after receiving a frame.
    /// </summary>
    /// <remarks>In internal ticks</remarks>
    [BrowsableAttribute( false )]
    public TimeSpan TimeoutSpanAfterFrame
    {
      get
      {
        return ppTimeoutAfterFrame;
      }
    }
    /// <summary>
    /// Time that elaps after receiving a frame in ms
    /// </summary>
    /// <remarks>In ms.</remarks>
    [DescriptionAttribute( "Time that elaps after receiving a frame in [ms]. This time is the maximum time that can elapse during waiting for a new character(byte) inside the frame. If this time elapsed and any character is received, sever assumes that it is the end of the frame." )]
    public uint TimeoutAfterFrame
    {
      set
      {
        ppTimeoutAfterFrame = TimeSpan.FromMilliseconds( value );
        ppInterfameGap = Timer.Max( ppInterfameGap, ppTimeoutAfterFrame );
      }
      get
      {
        return Convert.ToUInt32( ppTimeoutAfterFrame.TotalMilliseconds );
      }
    }
    /// <summary>
    /// Set all parameters according to the baudrate.
    /// </summary>
    public override uint LineSpeed
    {
      set
      {
        TimeSpan ppCharacterTimeoutmin;
        CountTimeIntervals( value, out ppTimeoutAfterFramemin, out ppCharacterTimeoutmin );
        ppTimeoutAfterFrame = ppTimeoutAfterFramemin;
        ppInterfameGap = ppTimeoutAfterFrame;
      }
    }
    /// <summary>
    /// Gets the ProtocolParameters description as a string. 
    /// </summary>
    /// <returns>Description in form: [ResponseTimeOut][InterframeGap][Timeout35][Timeout15]</returns>
    public override string ToString()
    {
      return base.ToString() + "\\TimeoutAfterFrame=" + TimeoutAfterFrame.ToString();
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
      XmlHelper.WriteTimeInMicroseconds( pSettings, m_Tag_TimeoutAfterFrame, ppTimeoutAfterFrame );
      XmlHelper.WriteTimeInMicroseconds( pSettings, m_Tag_TimeoutAfterFramemin, ppTimeoutAfterFramemin );
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
      ppTimeoutAfterFrame = XmlHelper.ReadTimeFromMicroseconds( pSettings, m_Tag_TimeoutAfterFrame );
      ppTimeoutAfterFramemin = XmlHelper.ReadTimeFromMicroseconds( pSettings, m_Tag_TimeoutAfterFramemin );
      ppTimeoutResponse = XmlHelper.ReadTimeFromMicroseconds( pSettings, m_Tag_TimeoutResponse );
      pSettings.ReadStartElement( m_TagClass );
    }
    /// <summary>
    /// Creator
    /// </summary>
    /// <param name="portSpeed">Baudrate of the communication line.</param>
    /// <param name="TimeoutResponse">Maximum response time this station is willing to wait.</param>
    /// <param name="NumberOfRetries">Maximum number of retries this station will try.</param>
    public SBUS_ProtocolParameters( uint portSpeed, uint TimeoutResponse, ushort NumberOfRetries )
      : base( portSpeed, TimeoutResponse, NumberOfRetries )
    {
      LineSpeed = portSpeed;
      ResponseTimeOut = TimeoutResponse;
      InterframeGap = 13; // based on some experiments the best interframeGap is arround 13 ms
                          // ofcourse it is only initial value and it could be modified 
                          // (to less or grater by the user) 
      this.ppNumberOfRetries = NumberOfRetries;
    }
    #endregion
  } //ProtocolParameters
}
