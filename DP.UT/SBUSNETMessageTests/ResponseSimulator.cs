using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAS.Lib.CommonBus.ApplicationLayer;
using CAS.Lib.CommonBus.ApplicationLayer.SBUS.PRIVATE;

namespace CAS.UnitTests.CommonBus.ApplicationLayer.SBUS.NET
{
  public class ResponseSimulator: List<byte>
  {

    byte[] _header = new byte[]
    {
      //UDP preamble
      0x0, 0x0, 0x0, 0x0E, //L
      0x1, //V
      0x0, //T
      0x0, 52, //S 
    };
    internal ResponseSimulator( int length, object value )
    {
      this.AddRange( _header );
      Add( (byte)AttributeCharacter.ResponseData );
      if ( value.GetType() == typeof( bool ) )
      {
        byte _val = FrameBitConverter.GetBytes( (bool)value );
        for ( int _ix = 0; _ix < ( ( length - 1 ) / 8 + 1 ); _ix++ )
          Add( _val );
      }
      else if ( value.GetType() == typeof( string ) )
        this.AddRange( FrameBitConverter.GetBytes( (string)value, length ) );
      else
      {
        byte[] _valueBytes = FrameBitConverter.GetBytes( (int)value );
        for ( int _ix = 0; _ix < length; _ix++ )
          AddRange( _valueBytes );
      }
    }

  }
}
