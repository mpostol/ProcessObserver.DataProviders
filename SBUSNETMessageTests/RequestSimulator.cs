using System;
using System.Collections.Generic;
using System.Linq;
using CAS.Lib.CommonBus.ApplicationLayer.SBUS.PRIVATE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAS.UnitTests.CommonBus.ApplicationLayer.SBUS.NET
{
  public class RequestSimulator: List<byte>
  {

    byte[] _header = new byte[]
    {
      //UDP preamble
      //0x0, 0x0, 0x0, 0x0E, //L
      0x0, //V
      0x0, //T
      0x0, 52, //S 
    };
    internal RequestSimulator( int length, object value, UInt32 frameLengt, byte stationAddr, Medium_T dataType, UInt16 addressIOFRTC )
    {
      SbusCode code = GetSbusCode( dataType );
      this.AddRange( FrameBitConverter.GetBytes( (Int32)( frameLengt + 2 ) ) ); //<frame>+<CRC>
      this.AddRange( _header );
      Add( (byte)AttributeCharacter.Telegram );
      Add( stationAddr );
      Add( (byte)code );
      Add( (byte)( frameLengt - this.Count - 1 - 1  ) );
      AddRange( FrameBitConverter.GetBytes( addressIOFRTC ) );
      switch ( code )
      {
        case SbusCode.coWriteCounter:
        case SbusCode.coWriteRegister:
        case SbusCode.coWriteTimer:
          Assert.IsTrue( value.GetType() == typeof( Int32 ), "Wrong Sbus code" );
          byte[] _valueBytes = FrameBitConverter.GetBytes( (Int32)value );
          for ( int _ix = 0; _ix < length; _ix++ )
            AddRange( _valueBytes );
          break;
        case SbusCode.coWriteFlag:
        case SbusCode.coWriteOutput:
          Assert.IsTrue( value.GetType() == typeof( bool ), "Wrong Sbus code" );
          Add( (byte)length );
          byte _val = (bool)value ? (byte)0xFF : (byte)0x0;
          for ( int _ix = 0; _ix < ( length / 8 + 1 ); _ix++ )
            Add( _val );
          break;
        case SbusCode.coWriteText:
          Assert.IsTrue( ( value == null ) || ( value.GetType() == typeof( string ) ), "Wrong Sbus code" );
          AddRange( FrameBitConverter.GetBytes( (UInt16)0 ) );
          string _valString = (string)value;
          int _expectedStringLength = (int)( frameLengt - this.Count );
          if ( _valString == null )
            _valString = String.Empty;
          if ( _valString.Length < _expectedStringLength )
            _valString = _valString.PadRight( _expectedStringLength );
          else if ( _valString.Length > _expectedStringLength )
            _valString = _valString.Substring( 0, _expectedStringLength );
          this.AddRange( FrameBitConverter.GetBytes( _valString, length ) );
          break;
        default:
          Assert.Fail( "Error in code" );
          break;
      }
      Assert.AreEqual<uint>( frameLengt, (uint)this.Count, "Wrong length of the simulated frame" );
    }
    private static SbusCode GetSbusCode( Medium_T dataType )
    {
      SbusCode _ret = default( SbusCode );
      switch ( dataType )
      {
        case Medium_T.Flag:
          _ret = SbusCode.coWriteFlag;
          break;
        case Medium_T.Output:
          _ret = SbusCode.coWriteOutput;
          break;
        case Medium_T.Register:
          _ret = SbusCode.coWriteRegister;
          break;
        case Medium_T.Timer:
          _ret = SbusCode.coWriteTimer;
          break;
        case Medium_T.Counter:
          _ret = SbusCode.coWriteCounter;
          break;
        case Medium_T.Text:
          _ret = SbusCode.coWriteText;
          break;
        default:
          Assert.Fail( "Unexpected data type" );
          break;
      }
      return _ret;
    }
  }
}
