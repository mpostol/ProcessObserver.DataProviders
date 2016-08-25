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
  /// Summary description for SBUSNet_message.
  /// </summary>
  internal class SBUSNet_message: FrameStateMachine
  {
    #region PUBLIC
    /// <summary>
    /// DepositeChar - this function stores the character in frame 
    /// </summary>
    /// <param name="lastChr">character to deposite</param>
    /// <returns>type of received character</returns>
    internal override DepCharacterTypeEnum DepositeChar( byte lastChr )
    {
      if ( rxMsgCurrError != CheckResponseResult.CR_OK )
        return DepCharacterTypeEnum.DCT_Ordinary;
      if ( ( Part != FramePart.CRC_1 ) && ( Part != FramePart.CRC_2 ) )
        crc.CRC_Calc( lastChr );
      return base.DepositeChar( lastChr );
    }
    /// <summary>
    /// SBUS message constructor
    /// </summary>
    /// <param name="homePool">The home pool.</param>
    internal SBUSNet_message( IBufferLink homePool )
      : base( homePool, 8 )
    { }
    #endregion

    #region private
    protected override bool ReadHead( byte lastChr )
    {
      Debug.Assert( this.offset < this.FSpartLength, "we must be inside header" );
      WriteByte( lastChr );
      if ( this.offset == this.FSpartLength )
        return true;
      return false;
    }
    protected override void CopyNextByte( byte val )
    {
      WriteByte( val );
      crc.CRC_Calc( val );
    }
    /// <summary>
    /// Writes the CRC.
    /// </summary>
    /// <param name="Crc">The CRC.</param>
    protected override void WriteCRC( ushort Crc )
    {
      WriteByte( (byte)( Crc >> 8 ) );
      WriteByte( (byte)Crc );
    }
    protected override void PrepareFrameHeader( int station, IBlockDescription block, SbusCode code )
    {
      base.PrepareFrameHeader( station, block, code );
      this.FRAMELENGTH = userDataLength + CRCLength;
      this.SetStandardVersion();
      this.SetStandardProtocolType();
      this.SEQUENCE_NUMBER = sequencenumber++;
    }

    #region IPHeader
    private static short sequencenumber = 0x2f;
    private const byte frame_length_pos = 0; // polozenie pola L  w SBUS Ethernet pseudo-header
    private const byte version_pos = 4; // polozenie pola V  w SBUS Ethernet pseudo-header
    private const byte prot_t_pos = 5; // polozenie pola T  w SBUS Ethernet pseudo-header
    private const byte sequence_pos = 6; // polozenie pola S  w SBUS Ethernet pseudo-header
    private int FRAMELENGTH
    {
      set
      {
        ushort off = this.offset;
        this.offset = frame_length_pos;
        Assert( this.WriteInt32( value ) );
        this.offset = off;
      }
      get
      {
        this.offset = frame_length_pos;
        return ( this.ReadInt32() );
      }
    }
    private short SEQUENCE_NUMBER
    {
      set
      {
        ushort off = this.offset;
        this.offset = sequence_pos;
        this.WriteInt16( value );
        this.offset = off;
      }
      get
      {
        this.offset = sequence_pos;
        return ( this.ReadInt16() );
      }
    }
    private byte VERSION
    {
      set
      {
        this[ version_pos ] = (byte)( value );
      }
      get
      {
        return (byte)( this[ version_pos ] );
      }
    }
    private void SetStandardVersion()
    {
      this.VERSION = 0;
    }
    private byte PROTOCOL_TYPE
    {
      set
      {
        this[ prot_t_pos ] = (byte)( value );
      }
      get
      {
        return (byte)( this[ prot_t_pos ] );
      }
    }
    private void SetStandardProtocolType()
    {
      this.PROTOCOL_TYPE = 0;
    }
    #endregion

    #endregion
  }
}
