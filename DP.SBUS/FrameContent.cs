//<summary>
//  Title   :SBUS message implementation of the content management functionality
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    20090123: mzbrzezny: assertion is now configured to restart the computer, 
//              read value is throwing an exception when problem with bad fram occurs
//              some new tracing messages
//    20081008: mabrzezny: item defaults are improved, conversion to unsigned is added
//    20080904: mzbrzezny: adaptation for new umessage that supports returning of information about status of write operation
//    Maciej Zbrzezny - 12-04-2006
//    zmienilem ze dostep do typu ramki jest typu internal
//	  MZbrzezny - 2005-12-16
//	  created from SBUSMessage (the oryginal class was divided into 2 parts)
//    MZbrzezny - 29-07-04:
//     module creation
//    20130330: Text implementation
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Diagnostics;
using System.Text;
using CAS.Lib.CommonBus.CommunicationLayer.Generic;

namespace CAS.Lib.CommonBus.ApplicationLayer.SBUS.PRIVATE
{

  /// <summary>
  /// SBUS message implementation of the content management functionality
  /// </summary>
  internal abstract class FrameContent: ProtocolALMessage
  {

    #region ctor
    internal FrameContent( IBufferLink homePool, byte fspartLength )
      : base( 300, homePool, true )
    {
      FSpartLength = fspartLength;
      ATbytePos = (byte)( FSbytePos + FSpartLength + 0 ); //pozycja bajtu AT - czyli co zawiera ramka
      stationAddressPos = (byte)( ATbytePos + ATbyteLength ); //pozycja adresu stacji w ramce
      m_DataTypePos = (byte)( stationAddressPos + 1 ); //pozycja typu danych / typu ramki w ramce
      regCountPos = (byte)( stationAddressPos + 2 ); //pozycja wartosci <r-count>; <w - count> w ramce (nie zawsze wystepuje)
      addressStartPos = (byte)( stationAddressPos + 3 ); //<address-RTC>/<address-IOF> pozycja adresy flagi / rejestru w ramce
    }
    #endregion

    #region ProtocolALMessage

    /// <summary>
    /// Read the selected contend (value) from the message in the requested type. 
    /// If the address space cannot contain values in this type no conversion is done. 
    /// </summary>
    /// <param name="regAddress">Address</param>
    /// <param name="canonicalType">Requested canonical type.</param>
    /// <returns>Converted value.</returns>
    public override object ReadValue( int regAddress, Type canonicalType )
    {
      if ( this.FrameAttribute != AttributeCharacter.ResponseData )
      {
        string _msg = "FrameContent.ReadValue: the frame is {0}, but expected frame type  is: {1}";
        _msg = String.Format( _msg, FrameAttribute, AttributeCharacter.ResponseData );
        TraceEvent( System.Diagnostics.TraceEventType.Error, 71, _msg );
        throw new ApplicationException( _msg );
      }
      object result = null;
      ushort _offst = DataPosition( regAddress );
      Debug.Assert( _offst < userDataLength );
      this.offset = _offst;
      switch ( DataType )
      {
        case SbusCode.coReadCounter:
        case SbusCode.coReadRegister:
        case SbusCode.coReadTimer:
          result = this.ReadInt32();
          break;
        case SbusCode.coReadFlag:
        case SbusCode.coReadInput:
        case SbusCode.coReadOutput:
          result = ( ReadByte() & ( 0x01 << ( regAddress % 8 ) ) ) != 0;
          break;
        case SbusCode.coReadText:
          byte[] _frame = this.GetManagedBuffer();
          Encoding _ascii = Encoding.ASCII;
          Debug.Assert( userDataLength - offset >= 0 );
          result = _ascii.GetString( _frame, offset, userDataLength - offset );
          break;
        case SbusCode.coReadDisplayRegister:
        case SbusCode.coReadRealTimeClock:
        case SbusCode.coRS0:
        case SbusCode.coRS1:
        case SbusCode.coRS2:
        case SbusCode.coRS3:
        case SbusCode.coRS4:
        case SbusCode.coRS5:
        case SbusCode.coRS6:
        case SbusCode.coRS7:
        default:
          {
            string _msg = String.Format( "Application error - datat type {0} not implemented", DataType );
            Debug.Fail( _msg );
            throw new NotImplementedException( _msg );
          }
      }
      return Convert2RequestedType( result, canonicalType );
    }
    /// <summary>
    /// Writes the value to the message in the requested type.
    /// If the address space cannot contain values in the type of pValue no conversion is done.
    /// </summary>
    /// <param name="value">Value to write.</param>
    /// <param name="registerAddress">Address of the register</param>
    /// <exception cref="System.ApplicationException">Application error - wrong datat type</exception>
    public override void WriteValue( object value, int registerAddress )
    {
      if ( this.FrameAttribute != AttributeCharacter.Telegram )
      {
        string _msg = "FrameContent.WriteValue: the frame is {0}, but expected frame type  is: {1}";
        _msg = String.Format( _msg, FrameAttribute, AttributeCharacter.Telegram );
        TraceEvent( System.Diagnostics.TraceEventType.Error, 128, _msg );
        throw new ApplicationException( _msg );
      }
      switch ( DataType )
      {
        case SbusCode.coWriteCounter:
        case SbusCode.coWriteRegister:
        case SbusCode.coWriteTimer:
          int pValue = ConvertToInternalRepresentation( value );
          this.offset = DataPosition( registerAddress );
          Assert( this.WriteInt32( pValue ) ); //TODO remove Assert and replace by the exception
          break;
        case SbusCode.coWriteFlag:
        case SbusCode.coWriteOutput:
          int _position = DataPosition( registerAddress );
          byte var = this[ _position ];
          if ( Convert.ToBoolean( value ) )
            var |= (byte)( 0x01 << ( registerAddress % 8 ) );
          else
            var &= (byte)( ~( 0x01 << ( registerAddress % 8 ) ) );
          this[ _position ] = var;
          break;
        case SbusCode.coWriteText:
          Debug.Assert( registerAddress == 0, "registerAddress must be equal 0" );
          this.offset = DataPosition( 0 );
          int _expectedStringLength = userDataLength - offset;
          Debug.Assert( _expectedStringLength > 0, "DataPosition error" );
          string _valString = Convert.ToString( value );
          if ( _valString == null )
            _valString = String.Empty;
          if ( _valString.Length < _expectedStringLength )
            _valString = _valString.PadRight( _expectedStringLength );
          else if ( _valString.Length > _expectedStringLength )
            _valString = _valString.Substring( 0, _expectedStringLength );
          Encoding _ascii = Encoding.ASCII;
          byte[] _bytes = _ascii.GetBytes( _valString );
          Debug.Assert( _bytes.Length == _expectedStringLength, "String to bytes array conversion error" );
          for ( int _cix = 0; _cix < _bytes.Length; _cix++ )
            WriteByte( _bytes[ _cix ] );
          Debug.Assert( offset == userDataLength );
          break;
        default:
          throw new ApplicationException( "Application error - wrong datat type" );
      }
    }
    /// <summary>
    /// prepare a telegram
    /// </summary>
    /// <param name="station">station that shoud received telegram</param>
    /// <param name="block">block description to be read</param>
    protected override void PrepareRequest( int station, IBlockDescription block )
    {
      PrepareFrameHeader( station, block, Definitions.DataType2SbusCode4Read( (Medium_T)block.dataType ) );
      this.RegisterCount = (byte)block.length;
    }
    /// <summary>
    /// prepare a telegram
    /// </summary>
    /// <param name="station">station that shoud received telegram</param>
    /// <param name="block">block description to be written</param>
    protected override void PrepareReqWriteValue( IBlockDescription block, int station )
    {
      if ( (Medium_T)block.dataType == Medium_T.Input )
      {
        string _msg = "Input is read only resource";
        TraceEvent( TraceEventType.Information, 192, _msg );
        throw new ArgumentException( _msg, "block" );
      }
      PrepareFrameHeader( station, block, Definitions.DataType2SbusCode4Write( (Medium_T)block.dataType ) );
      offset = (ushort)( m_DataTypePos + CmdLength );
      Debug.Assert( offset == regCountPos );
      WriteByte( (byte)( userDataLength - offset - W_countLength - 1 ) ); //<w-count>
      WriteInt16( (short)( block.startAddress ) ); //<address-RTC>, <address-IOF>, <text-number>
      switch ( DataType )
      {
        case SbusCode.coWriteFlag:
        case SbusCode.coWriteOutput:
          WriteByte( (byte)block.length ); //<fio-count>
          break;
        case SbusCode.coWriteRegister:
        case SbusCode.coWriteTimer:
        case SbusCode.coWriteCounter:
          break;
        case SbusCode.coWriteText:
          WriteInt16( 0 ); //<char-position>
          break;
        default:
          break;
      }
    }
    /// <summary>
    /// Writes the byte.
    /// </summary>
    /// <param name="val">The val.</param>
    /// <exception cref="CAS.Lib.CommonBus.ApplicationLayer.SBUS.PRIVATE.FrameContent.WriteException"></exception>
    protected new void WriteByte( byte val )
    {
      if ( !base.WriteByte( val ) )
      {
        TraceEvent( TraceEventType.Error, 221, String.Format( C_WriteErrorMessageTmpl, offset, this.userBuffLength ) );
        throw new WriteException();
      }
    }
    protected new void WriteInt16( short val )
    {
      if ( !base.WriteInt16( val ) )
      {
        TraceEvent( TraceEventType.Error, 229, String.Format( C_WriteErrorMessageTmpl, offset, this.userBuffLength ) );
        throw new WriteException();
      }
    }
    private const string C_WriteErrorMessageTmpl = "Write to buffer failed at positions {0}, current buffer length={1}.";
    protected short ReadInt16( ushort position )
    {
      offset = position;
      return base.ReadInt16();
    }

    #region IReadCMDValue
    /// <summary>
    /// A command a master station request to be processed by this station – slave station on the field bus network.
    /// </summary>
    /// <value></value>
    public override int GetCommand
    {
      get { throw ( new ApplicationLayerInterfaceNotImplementedException() ); }
    }
    #endregion

    #region Slave and sniffer
    /// <summary>
    /// function for reading flag/register that shoud be written to slave
    /// (SLAVE SIDE)
    /// </summary>
    /// <param name="regAddressOffset">offset of flag/register in frame</param>
    /// <returns>value that has been read</returns>
    protected override object ReadCMD( int regAddressOffset ) //odpowiednik getvalue i taka powinien miec nazwe
    {
      throw new ApplicationLayerInterfaceNotImplementedException();
    }
    /// <summary>
    /// (SLAVE SIDE)
    /// </summary>
    /// <param name="regValue">Value to write.</param>
    /// <param name="regAddressOffset">Position of the value.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    protected override void SetValue( object regValue, int regAddressOffset )
    {
      throw new ApplicationLayerInterfaceNotImplementedException();
    }
    #endregion

    #endregion

    #region Converters
    private uint ConvertInt2UnsigedInt( int i )
    {
      if ( i >= 0 )
        return (uint)i;
      uint j = ~(uint)( 0 - i );
      return j + 1;
    }
    private int ConvertUnsignedInt2Int( uint i )
    {
      if ( i <= 0x7FFFFFFF )
        return (int)i;
      uint j = ~(uint)( i - 1 );
      return ( 0 - (int)j );
    }
    private float ConvertInt2Float( int i )
    {
      double ret;
      byte[] b = System.BitConverter.GetBytes( i );
      // 2 do potegi 24 = 16777216;
      //najpierw obliczymy mantyse / (2^24)
      ret = (double)( (int)b[ 1 ] + (int)b[ 2 ] * 256 + (int)b[ 3 ] * 256 * 256 ) / 16777216.0;
      //teraz obliczymy eksponent
      //b[0]&0x7F-64 - eksponent bierzemy 7 bitow - przesuwamy o 64
      ret = ret * Math.Pow( 2.0, (int)( b[ 0 ] & 0x7F ) - 64 );
      if ( ( b[ 0 ] & 0x80 ) > 0 )
        ret = -ret;
      return (float)ret;
    }
    private int ConvertFloat2int( float i )
    {
      if ( i == 0 )
        return 0;
      double input = i;
      int ret;
      int s = 0, e = -65;
      if ( input < 0 )
      {
        s = 1;
        input = -input;
      };
      input = ( input * 16777216.0 );//pomnozylimy przez 2^24
      double mantisa;
      do
      {
        e++;
        mantisa = input / Math.Pow( 2.0, e );
      } while ( ( mantisa < 0x800000 || mantisa > 0xffffff ) && e <= 63 );
      input = mantisa;
      //e = e;
      byte exponent = (byte)( e + 64 );
      if ( s > 0 )
        exponent = (byte)( (byte)exponent | (byte)0x80 );
      byte[] b = System.BitConverter.GetBytes( System.Convert.ToInt32( input ) );
      // 2 do potegi 24 = 16777216;
      //najpierw obliczymy mantyse / (2^24)
      ret = (int)exponent + (int)b[ 0 ] * 256 + (int)b[ 1 ] * 256 * 256 + (int)b[ 2 ] * 256 * 256 * 256;
      return ret;
    }
    /// <summary>
    /// Convert to the requested type from internal representation.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="requestedType">Type of the requested.</param>
    /// <returns></returns>
    private object Convert2RequestedType( object source, Type requestedType )
    {
      //cannonical type is Sytem.Int32 (int) 
      if ( requestedType.Equals( typeof( float ) ) || requestedType.Equals( typeof( double ) ) )
      {
        return this.ConvertInt2Float( Convert.ToInt32( source ) );
      }
      if ( requestedType.Equals( typeof( uint ) ) )
      {
        return this.ConvertInt2UnsigedInt( Convert.ToInt32( source ) );
      }
      if ( requestedType.Equals( typeof( System.UInt16 ) )
          && Convert.ToInt32( source ) <= UInt16.MaxValue
          && Convert.ToInt32( source ) >= UInt16.MinValue )
      {
        return System.Convert.ToUInt16( source );
      }
      if ( requestedType.Equals( typeof( System.Int16 ) )
          && System.Convert.ToInt32( source ) <= System.Int16.MaxValue
          && System.Convert.ToInt32( source ) >= System.Int16.MinValue )
      {
        return System.Convert.ToInt16( source );
      }
      return source;
    }
    /// <summary>
    /// Converts to internal representation compatible with SBUS.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    private int ConvertToInternalRepresentation( object value )
    {
      if ( value.GetType().Equals( typeof( float ) ) || value.GetType().Equals( typeof( double ) ) )
      {
        return this.ConvertFloat2int( (float)System.Convert.ToDouble( value ) );
      }
      if ( value.GetType().Equals( typeof( uint ) ) )
      {
        return this.ConvertUnsignedInt2Int( System.Convert.ToUInt32( value ) );
      }
      if ( value.GetType().Equals( typeof( System.UInt16 ) ) || value.GetType().Equals( typeof( System.Int16 ) ) )
      {
        return System.Convert.ToInt32( value );
      }
      return System.Convert.ToInt32( value ); //last resort
    }
    #endregion

    #region calculations
    /// <summary>
    /// Calculates the position of the data part for <see cref="AttributeCharacter.Telegram" /> and <see cref="AttributeCharacter.ResponseData" />.
    /// </summary>
    /// <param name="registerAddress">The address of the register.</param>
    /// <returns>
    /// Number of bytes in the frame excluding crc and escape sequences
    /// </returns>
    /// <exception cref="System.NotImplementedException">Is thrown if selected <see cref="SbusCode" /> is not handled.</exception>
    private ushort DataPosition( int registerAddress )
    {
      ushort _position = HeaderLength( FSpartLength );
      switch ( DataType )
      {
        case SbusCode.coReadRegister:
        case SbusCode.coReadTimer:
        case SbusCode.coReadCounter:
          Debug.Assert( FrameAttribute == AttributeCharacter.ResponseData, "DataPosition error - the frame must be AttributeCharacter.ResponseData" );
          _position += (ushort)( registerAddress * 4 );  //<FS><AT>{r-count}*
          break;
        case SbusCode.coReadFlag:
        case SbusCode.coReadInput:
        case SbusCode.coReadOutput:
          Debug.Assert( FrameAttribute == AttributeCharacter.ResponseData, "DataPosition error - the frame must be AttributeCharacter.ResponseData" );
          _position += (ushort)( registerAddress / 8 );
          break;
        case SbusCode.coWriteRegister:
        case SbusCode.coWriteTimer:
        case SbusCode.coWriteCounter:
          Debug.Assert( FrameAttribute == AttributeCharacter.Telegram, "DataPosition error - the frame must be AttributeCharacter.Telegram" );
          //<FS><AT><station><cmd><w-count><address-RTC>{r-count}+
          _position += (ushort)( AddressPreable + W_countLength + Address_RTCLength + registerAddress * 4 );
          break;
        case SbusCode.coWriteOutput:
        case SbusCode.coWriteFlag:
          Debug.Assert( FrameAttribute == AttributeCharacter.Telegram, "DataPosition error - the frame must be AttributeCharacter.Telegram" );
          //<FS><AT><station><cmd><w-count><address-IOF><fio-count>)
          _position += (ushort)( AddressPreable + W_countLength + Address_IOFLength + FIOCountLength + registerAddress / 8 );
          break;
        case SbusCode.coReadText:
          Debug.Assert( FrameAttribute == AttributeCharacter.ResponseData, "DataPosition error - the frame must be AttributeCharacter.ResponseData" );
          _position += 0;
          break;
        case SbusCode.coWriteText:
          Debug.Assert( FrameAttribute == AttributeCharacter.Telegram, "DataPosition error - the frame must be AttributeCharacter.Telegram" );
          //<w-count> <text-number> <char-position> {<ascii-char>}+;  _char_positionLength is not implemented.
          _position += (ushort)( AddressPreable + W_countLength + Text_numberLength + Char_positionLength + Ascii_charLength * 0 );
          break;
        default:
          throw new ApplicationLayerInterfaceNotImplementedException();
      }
      return _position;
    }
    protected ushort FrameRequestLength( ushort registerCount )
    {
      ushort _length = (ushort)( HeaderLength( FSpartLength ) + AddressPreable );
      switch ( DataType )
      {
        case SbusCode.coReadCounter:
        case SbusCode.coReadRegister:
        case SbusCode.coReadTimer:
          _length += (ushort)( R_countLength + Address_RTCLength );
          break;
        case SbusCode.coReadFlag:
        case SbusCode.coReadInput:
        case SbusCode.coReadOutput:
          _length += (ushort)( R_countLength + Address_IOFLength );
          break;
        case SbusCode.coReadText:
          _length += (ushort)( R_countLength + Text_numberLength + Char_positionLength );
          break;
        case SbusCode.coWriteCounter:
        case SbusCode.coWriteRegister:
        case SbusCode.coWriteTimer:
          //<FS><AT><station><cmd><w-count><address-RTC><fio-count>)
          _length += (ushort)( W_countLength + Address_RTCLength + ( registerCount * 4 ) );
          break;
        case SbusCode.coWriteFlag:
        case SbusCode.coWriteOutput:
          //<FS><AT><station><cmd><w-count><address-IOF><fio-count>)
          _length += (ushort)( W_countLength + Address_IOFLength + FIOCountLength + registerCount / 8 + 1 );
          break;
        case SbusCode.coWriteText:
          //<w-count> <text-number> <char-position> {<ascii-char>}+;  _char_positionLength is not implemented.
          _length += (ushort)( W_countLength + Text_numberLength + Char_positionLength + Ascii_charLength * registerCount );
          break;
        default:
          throw new ApplicationLayerInterfaceNotImplementedException();
      }
      return _length;
    }
    protected ushort FrameResponseLength()
    {
      ushort _length = HeaderLength( FSpartLength );
      switch ( DataType )
      {
        case SbusCode.coReadCounter:
        case SbusCode.coReadRegister:
        case SbusCode.coReadTimer:
          _length += (ushort)( RegisterCount * 4 );  //<FS><AT>{r-count}*
          break;
        case SbusCode.coReadFlag:
        case SbusCode.coReadInput:
        case SbusCode.coReadOutput:
          _length += (ushort)( ( RegisterCount - 1 ) / 8 + 1 );
          break;
        case SbusCode.coReadText:
          // {<ascii-char>}+;  _char_positionLength is not implemented.
          _length += RegisterCount;
          break;
        case SbusCode.coWriteCounter:
        case SbusCode.coWriteRegister:
        case SbusCode.coWriteTimer:
        case SbusCode.coWriteFlag:
        case SbusCode.coWriteOutput:
        case SbusCode.coWriteText:
          _length = ACKNAKFrameLength;
          break;
        default:
          throw new ApplicationLayerInterfaceNotImplementedException();
      }
      return _length;
    }
    /// <summary>
    /// Prepares the frame header.
    /// </summary>
    /// <param name="station">The station address.</param>
    /// <param name="block">The block description.</param>
    /// <param name="code">The type of the SBUS frame.</param>
    protected virtual void PrepareFrameHeader( int station, IBlockDescription block, SbusCode code )
    {
      userDataLength = userBuffLength;
      this.FrameAttribute = AttributeCharacter.Telegram;
      this.offset = stationAddressPos;
      WriteByte( (byte)station );
      this.DataType = code;
      offset = addressStartPos;
      WriteInt16( (short)block.startAddress ); //<text-number> or <address-RTC> or <address-IOF>
      switch ( code )
      {
        case SbusCode.coReadText:
          WriteInt16( 0 ); // <char-position>
          break;
        default:
          break;
      }
      this.SetBlockDescription( station, block ); //It is used usaly while receiving.
      userDataLength = FrameRequestLength( (ushort)block.length );
    }
    /// <summary>
    /// checks the type of expected frame
    /// </summary>
    /// <returns>type of expected type</returns>
    protected AttributeCharacter ExpectedResultFrameType()
    {
      Debug.Assert( this.FrameAttribute == AttributeCharacter.Telegram, "SBUSbase_message:ExpectedResultFrameType, frame must be FT_TELEGRAM, but it is" + this.FrameAttribute.ToString() );
      switch ( this.DataType )
      {
        case SbusCode.coReadFlag:
        case SbusCode.coReadInput:
        case SbusCode.coReadOutput:
        case SbusCode.coReadRegister:
        case SbusCode.coReadCounter:
        case SbusCode.coReadTimer:
        case SbusCode.coReadText:
          return AttributeCharacter.ResponseData;
        case SbusCode.coWriteCounter:
        case SbusCode.coWriteRegister:
        case SbusCode.coWriteTimer:
        case SbusCode.coWriteFlag:
        case SbusCode.coWriteOutput:
        case SbusCode.coWriteText:
          return AttributeCharacter.ResponseACK_NAK;
      }
      throw new NotImplementedException( String.Format( "Frame of {0} data type out of scope", DataType ) );
    }
    private static ushort HeaderLength( ushort fsPartLength )
    {
      ushort _position = (ushort)( fsPartLength + ATbyteLength ); // FS><AT>;
      return _position;
    }
    #endregion

    #region SBUS_FRAME_FIELDS
    protected byte FSPart
    {
      set
      {
        this[ FSPartPosition ] = value;
      }
    }
    protected AttributeCharacter FrameAttribute
    {
      set
      {
        this[ ATbytePos ] = (byte)value;
      }
      get
      {
        return (AttributeCharacter)this[ ATbytePos ];
      }
    }
    protected short NakCode { get { return ReadInt16( (ushort)( ATbytePos + ATbyteLength ) ); } }
    /// <summary>
    /// station number property
    /// </summary>
    private byte Station
    {
      set
      {
        this[ stationAddressPos ] = value;
      }
      get
      {
        return this[ stationAddressPos ];
      }
    }
    protected SbusCode DataType
    {
      set
      {
        if ( this.FrameAttribute == AttributeCharacter.Telegram )
          this[ m_DataTypePos ] = (byte)value;
        bp_DataType = value;
      }
      get
      {
        return bp_DataType;
      }
    }
    private SbusCode bp_DataType = default( SbusCode );
    /// <summary>
    /// Return number of registers in frame
    /// </summary>
    /// <value>
    /// value: r-count + 1.
    /// </value>
    protected byte RegisterCount
    {
      set
      {
        this[ regCountPos ] = (byte)( value - 1 );
      }
      get
      {
        return (byte)( this[ regCountPos ] + 1 );
      }
    }
    #endregion

    #region SBUS frame positions
    private int FSPartPosition = 0;
    protected readonly byte FSpartLength;      //dlugosc jaka zajmuje pole FS - w przypadku sbusa po ethernecie - jest to dlugosc Pseudoheader for SBUS-Ethernet
    private readonly byte ATbytePos;          //pozycja bajtu AT - czyli co zawiera ramka
    private readonly byte stationAddressPos;  //pozycja adresu stacji w ramce
    private readonly byte m_DataTypePos;        //pozycja typu danych / typu ramki w ramce
    private readonly byte regCountPos;        //pozycja wartosci <r-count>, <w - count> w ramce (nie zawsze wystepuje)
    private readonly byte addressStartPos;    //pozycja adresy flagi / rejestru w ramce
    protected byte ACKNAKFrameLength { get { return (byte)( HeaderLength( FSpartLength ) + AckNakLength ); } }
    #endregion

    #region SBUS constans
    protected const byte FSbytePos = 0;      //pozycja bajtu FS
    protected const byte FrameSynchronisationChar = 0xB5; //Abreviated as FS
    protected const byte DataLinkEscapeChar = 0xC5;   //Abreviated as DLE
    private const ushort AddressPreable = (ushort)( StationLength + CmdLength ); //<station><cmd>
    //Fields length
    protected const byte ATbyteLength = 1;     //dlugosc pola AT
    private const ushort StationLength = 1;
    private const ushort CmdLength = 1;
    private const ushort Address_RTCLength = 2;
    private const ushort Address_IOFLength = 2;
    private const ushort W_countLength = 1;
    private const ushort R_countLength = 1;
    private const ushort Text_numberLength = 2;
    private const ushort Char_positionLength = 2;
    private const ushort Ascii_charLength = 1;
    private const ushort AckNakLength = 2;
    private const ushort FIOCountLength = 1;
    protected const ushort CRCLength = 2;
    #endregion

    #region error handling
    protected class WriteException: ApplicationException
    {
      public WriteException() { }
      public WriteException( string message ) : base( message ) { }
    }
    protected static void Assert( bool condition )
    {
      if ( !condition )
        throw new WriteException();
    }
    #endregion

#if DEBUG
    //Unit tests accessor.
    public void Test_PrepareRequest( int station, IBlockDescription block )
    {
      PrepareRequest( station, block );
    }
    public void Test_PrepareReqWriteValue( IBlockDescription block, int station )
    {
      PrepareReqWriteValue( block, station );
    }
#endif

  }

}
