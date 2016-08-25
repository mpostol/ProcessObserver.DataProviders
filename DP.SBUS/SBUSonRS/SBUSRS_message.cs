//<summary>
//  Title   : Sbus message implementation
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    20080904: mzbrzezny: adaptation for new umessage that supports returning of information about status of write operation
//	MZbrzezny - 2005-12-16
//	divided into 2 parts
//    MZbrzezny - 29-07-04:
//     module creation
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.Diagnostics;
using CAS.Lib.CommonBus.CommunicationLayer.Generic;

namespace CAS.Lib.CommonBus.ApplicationLayer.SBUS.PRIVATE
{
  /// <summary>
  /// Summary description for SBUS_message.
  /// </summary>
  internal class SBUSRS_message: FrameStateMachine
  {

    #region PUBLIC
    /// <summary>
    /// DepositeChar - this function stores the carracter in frame 
    /// </summary>
    /// <param name="lastChr">character to deposite</param>
    /// <returns>type of received character</returns>
    internal override DepCharacterTypeEnum DepositeChar( byte lastChr )
    {
      if ( rxMsgCurrError != CheckResponseResult.CR_OK )
        return DepCharacterTypeEnum.DCT_Last;
      if ( !m_InSync && ( lastChr != FrameSynchronisationChar ) )
        return DepCharacterTypeEnum.DCT_Ordinary;
      if ( m_InSync )
      {
        if ( DLESeq ) // czy jestesmy podaczas odbierania danych w przypadku dodanych DLE bajtow?
        {
          if ( ( Part != FramePart.CRC_1 ) && ( Part != FramePart.CRC_2 ) )
            // jesli nie odbieramy teraz CRC to uzywamy tego do obliczania
            crc.CRC_Calc( lastChr );
          if ( lastChr == 0 ) // w zaleznosci od odczytanego bajtu stwierdzamy co odczytywalismy
            lastChr = FrameSynchronisationChar;
          else if ( lastChr == 1 )
            lastChr = DataLinkEscapeChar;
          else
          {
            //bylismy w sekwencji DLE i nie pojawilo sie ani 0 ani 1 - czyli ramka niewlasciwa 
            rxMsgCurrError = CheckResponseResult.CR_Invalid;
            return DepCharacterTypeEnum.DCT_Ordinary;
          }
          DLESeq = false; //wychodzimy z sekwencji
        }
        else if ( lastChr == DataLinkEscapeChar )
        {
          //odczytalismy bajt DLE - wiec wchodzimy do sewencji DLE - aby odczytac teraz 0 lub 1
          DLESeq = true;
          if ( ( Part != FramePart.CRC_1 ) && ( Part != FramePart.CRC_2 ) )
            crc.CRC_Calc( lastChr );
          return DepCharacterTypeEnum.DCT_Ordinary;
        }
        else if ( ( Part != FramePart.CRC_1 ) && ( Part != FramePart.CRC_2 ) )
          crc.CRC_Calc( lastChr );
      }
      else
        m_InSync = true;
      return base.DepositeChar( lastChr );
    }
    internal override void InitMsg( FrameStateMachine txmsg )
    {
      base.InitMsg( txmsg );
      DLESeq = false;
    }

    /// <summary>
    /// SBUS message constructor
    /// </summary>
    /// <param name="homePool">The home pool.</param>
    internal SBUSRS_message( IBufferLink homePool ) :
      base( homePool, 1 ) { }
    #endregion

    #region private
    protected override bool ReadHead( byte lastChr )
    {
      Debug.Assert( lastChr == FrameSynchronisationChar, "The only possible character here is FrameSynchronisationChar" );
      WriteByte( lastChr );
      crc.CRC_Calc( lastChr );
      return true;
    }
    protected override void CopyNextByte( byte var )
    {
      if ( var == FrameSynchronisationChar )
      {
        if ( this.offset != 0 ) // pomijamy piewszy bajt FS - bo to poczatek ramki
        {
          WriteByte( DataLinkEscapeChar );
          crc.CRC_Calc( DataLinkEscapeChar );
          WriteByte( 0 );
          crc.CRC_Calc( 0 );
        }
        else
        {
          WriteByte( FrameSynchronisationChar );
          crc.CRC_Calc( FrameSynchronisationChar );
        }
      }
      else if ( var == DataLinkEscapeChar )
      {
        WriteByte( DataLinkEscapeChar );
        crc.CRC_Calc( DataLinkEscapeChar );
        WriteByte( 1 );
        crc.CRC_Calc( 1 );
      }
      else
      {
        WriteByte( (byte)var );
        crc.CRC_Calc( (byte)var );
      }
    }
    /// <summary>
    /// Writes the CRC to the frame at current position.
    /// </summary>
    /// <param name="Crc">The CRC.</param>
    protected override void WriteCRC( ushort Crc )
    {
      byte _lowByte = (byte)( Crc & 0xff );
      byte _highByte = (byte)( Crc >> 8 );
      if ( _highByte == FrameSynchronisationChar )
      {
        WriteByte( DataLinkEscapeChar );
        WriteByte( 0 );
      }
      else if ( _highByte == DataLinkEscapeChar )
      {
        WriteByte( DataLinkEscapeChar );
        WriteByte( 1 );
      }
      else
        WriteByte( _highByte );
      if ( _lowByte == FrameSynchronisationChar )
      {
        WriteByte( DataLinkEscapeChar );
        WriteByte( 0 );
      }
      else if ( _lowByte == DataLinkEscapeChar )
      {
        WriteByte( DataLinkEscapeChar );
        WriteByte( 1 );
      }
      else
        WriteByte( _lowByte );
    }
    /// <summary>
    /// Prepares the frame header.
    /// </summary>
    /// <param name="station">The station.</param>
    /// <param name="block">The block description.</param>
    /// <param name="code">The code.</param>
    protected override void PrepareFrameHeader( int station, IBlockDescription block, SbusCode code )
    {
      base.PrepareFrameHeader( station, block, code );
      FSPart = FrameSynchronisationChar;
    }
    private bool DLESeq = false;
    private bool m_InSync = false; //true if  FS character has been read.
    #endregion
  }
}
