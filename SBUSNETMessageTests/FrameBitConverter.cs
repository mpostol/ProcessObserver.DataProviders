using System;
using System.Diagnostics;
using System.Text;

namespace CAS.UnitTests.CommonBus.ApplicationLayer.SBUS.NET
{
  internal static class FrameBitConverter
  {
    internal static byte GetBytes( bool value )
    {
      return value ? (byte)0xFF : (byte)0x0;
    }
    internal static byte[] GetBytes( UInt16 value )
    {
      byte[] _valueBytes = BitConverter.GetBytes( value );
      if ( BitConverter.IsLittleEndian )
        Array.Reverse( _valueBytes );
      return _valueBytes;
    }
    internal static byte[] GetBytes( Int32 value )
    {
      byte[] _valueBytes = BitConverter.GetBytes( value );
      if ( BitConverter.IsLittleEndian )
        Array.Reverse( _valueBytes );
      return _valueBytes;
    }
    internal static byte[] GetBytes( string str, int length )
    {
      Debug.Assert( str.Length >= length );
      Encoding _encdg = Encoding.UTF8;
      byte[] bytes = _encdg.GetBytes( str.Substring( 0, length ) );
      return bytes;
    }
  }
}
