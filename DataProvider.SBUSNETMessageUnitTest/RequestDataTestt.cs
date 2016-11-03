
using CAS.Lib.CommonBus.ApplicationLayer.SBUS.PRIVATE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAS.UnitTests.CommonBus.ApplicationLayer.SBUS.NET
{
  [TestClass]
  public class RequestDataTest
  {
    private const int c_station = 213;
    private const ushort C_startAddress = 1;
    private const byte C_RegistersCount = 8;
    private const byte c_HeaderLength = 8;
    private const byte c_ATLength = 1;

    #region m_TestTable
    const byte c_Version = 0;
    private Dictionary<Medium_T, List<byte>> m_TestTable = new Dictionary<Medium_T, List<byte>>()
      {
        { Medium_T.Counter, new List<byte>
              {
                //UDP preamble
                0x0, 0x0, 0x0, 0x10, //L
                c_Version, //V
                0x0, //T
                0x0, 52, //S 
                //SBUS Frame
                0x00, //AT
                c_station, //A
                0x00, //C
                (byte)(C_RegistersCount-1), (byte)(C_startAddress>>8), (byte)(C_startAddress & 0xFF), //DATA
                //0XDC, 0X62// CRC
              }
         },
         { Medium_T.Flag, new List<byte>
              {
                //UDP preamble
                0x0, 0x0, 0x0, 0x10, //L
                c_Version, //V
                0x0, //T
                0x0, 47, //S 
                //SBUS Frame
                0x00, //AT
                c_station, //A
                0x02, //C
                (byte)(C_RegistersCount-1), (byte)(C_startAddress>>8), (byte)(C_startAddress & 0xFF), //DATA
                //0XDC, 0X62// CRC
              }
         },
         { Medium_T.Input, new List<byte>
              {
                //UDP preamble
                0x0, 0x0, 0x0, 0x10, //L
                c_Version, //V
                0x0, //T
                0x0, 48, //S 
                //SBUS Frame
                0x00, //AT
                c_station, //A
                0x03, //C
                (byte)(C_RegistersCount-1), (byte)(C_startAddress>>8), (byte)(C_startAddress & 0xFF), //DATA
                //0XDC, 0X62// CRC
              }
         },
         { Medium_T.Output, new List<byte>
              {
                //UDP preamble
                0x0, 0x0, 0x0, 0x10, //L
                c_Version, //V
                0x0, //T
                0x0, 49, //S 
                //SBUS Frame
                0x00, //AT
                c_station, //A
                0x05, //C
                (byte)(C_RegistersCount-1), (byte)(C_startAddress>>8), (byte)(C_startAddress & 0xFF), //DATA
                //0XDC, 0X62// CRC
              }
         },
         { Medium_T.Register, new List<byte>
              {
                //UDP preamble
                0x0, 0x0, 0x0, 0x10, //L
                c_Version, //V
                0x0, //T
                0x0, 50, //S 
                //SBUS Frame
                0x00, //AT
                c_station, //A
                0x06, //C
                (byte)(C_RegistersCount-1), (byte)(C_startAddress>>8), (byte)(C_startAddress & 0xFF), //DATA
                //0XDC, 0X62// CRC
              }
         },
         { Medium_T.Timer, new List<byte>
              {
                //UDP preamble
                0x0, 0x0, 0x0, 0x10, //L
                c_Version, //V
                0x0, //T
                0x0, 51, //S 
                //SBUS Frame
                0x00, //AT
                c_station, //A
                0x07, //C
                (byte)(C_RegistersCount-1), (byte)(C_startAddress>>8), (byte)(C_startAddress & 0xFF), //DATA
                //0XDC, 0X62// CRC
              }
         },
        { Medium_T.Text, new List<byte>
              {
                //UDP preamble
                0x0, 0x0, 0x0, 0x12, //L
                c_Version, //V
                0x0, //T
                0x0, 53, //S 
                //SBUS Frame
                0x00, //AT
                c_station, //A
                33, //C
                (byte)(C_RegistersCount-1), (byte)(C_startAddress>>8), (byte)(C_startAddress & 0xFF), //DATA
                0, 0 //<char-position>
                //0XDC, 0X62// CRC
              }
         }
      };
    #endregion    [TestMethod]


    [TestMethod]
    [TestCategoryAttribute( "NET SBUS message" )]
    public void ResponseReadValue()
    {
#if !DEBUG
      Assert.Inconclusive("This test requires DEBUG context");
#endif
      foreach ( Medium_T _enumIdx in Enum.GetValues( typeof( Medium_T ) ) )
      {
        using ( SBUSNet_message _frame = new SBUSNet_message( null ) )
        {
          Assert.IsNotNull( _frame, "Message is not created." );
          BlockAddress _tb = new BlockAddress( (short)_enumIdx, c_station, C_startAddress, C_RegistersCount );
          _frame.Test_PrepareRequest( _tb.station, _tb );
          List<byte> _frameList = _frame.GetManagedBuffer().ToList();
          Assert.AreEqual( m_TestTable[ _enumIdx ].Count, _frame.userDataLength, "The length of the request is not valid" );
          string _msg = String.Format( "Frame must be equal to the template. Failed for {0} ", _enumIdx );
          m_TestTable[ _enumIdx ][ 7 ] = _frameList[ 7 ]; // sequence numbers must be the same
          CollectionAssert.AreEqual( m_TestTable[ _enumIdx ], _frameList, _msg );
          using ( SBUSNet_message _dateFrame = new SBUSNet_message( null ) )
          {
            _dateFrame.InitMsg( _frame );
            ushort _expectedFrameLength = c_HeaderLength + c_ATLength;
            switch ( _enumIdx )
            {
              case Medium_T.Flag:
              case Medium_T.Input:
              case Medium_T.Output:
                _expectedFrameLength += ( C_RegistersCount - 1 ) / 8 + 1; //<header><AT>{fio-count}+
                CheckExpectedLength( _enumIdx, _dateFrame, _expectedFrameLength );
                TestContent<bool>( _dateFrame, false, CompareType<bool> );
                break;
              case Medium_T.Register:
              case Medium_T.Timer:
              case Medium_T.Counter:
                _expectedFrameLength += C_RegistersCount * 4;
                CheckExpectedLength( _enumIdx, _dateFrame, _expectedFrameLength );
                TestContent<int>( _dateFrame, 123, CompareType<int> );
                break;
              case Medium_T.Text:
                _expectedFrameLength += C_RegistersCount;
                CheckExpectedLength( _enumIdx, _dateFrame, _expectedFrameLength );
                TestContent<string>( _dateFrame, "1234567890123".Substring(0, _tb.length), CompareString );
                break;
              default:
                Assert.Fail( "Application error: unknown requested data type" );
                break;
            }
          }
        }
      }
    }
    private static string CheckExpectedLength( Medium_T _enumIdx, SBUSNet_message _dateFrame, ushort _expectedFrameLength )
    {
      string _msg = string.Format( "The length of the response for {0} is not valid", _enumIdx );
      Assert.AreEqual( _expectedFrameLength, _dateFrame.userDataLength, _msg );
      return _msg;
    }
    private enum RecStateEnum { RSE_BeforeHeading, RSE_InsideFrame }
    private void TestContent<type>( SBUSNet_message _dateFrame, type _testValue, System.Action<SBUSNet_message, type> comparer )
    {
      ResponseSimulator _sim = new ResponseSimulator( C_RegistersCount, _testValue );
      RecStateEnum _state = RecStateEnum.RSE_BeforeHeading;
      foreach ( byte _bix in _sim )
      {
        FrameStateMachine.DepCharacterTypeEnum _res = _dateFrame.DepositeChar( _bix );
        switch ( _state )
        {
          case RecStateEnum.RSE_BeforeHeading:
            if ( _res == FrameStateMachine.DepCharacterTypeEnum.DCT_SOH )
              _state = RecStateEnum.RSE_InsideFrame;
            Assert.AreNotEqual( FrameStateMachine.DepCharacterTypeEnum.DCT_Reset_Answer, _res, "Wrong DepositeChar return value" );
            Assert.AreNotEqual( FrameStateMachine.DepCharacterTypeEnum.DCT_Last, _res, "Wrong DepositeChar return value" );
            break;
          case RecStateEnum.RSE_InsideFrame:
            Assert.AreEqual( FrameStateMachine.DepCharacterTypeEnum.DCT_Ordinary, _res, "Wrong DepositeChar return value" );
            break;
        }
      }
      comparer( _dateFrame, _testValue );
    }
    private static void CompareType<type>( SBUSNet_message _dateFrame, type _testValue )
    {
      for ( int _bidx = 0; _bidx < C_RegistersCount; _bidx++ )
      {
        type _rv = (type)_dateFrame.ReadValue( _bidx, typeof( type ) );
        Assert.AreEqual<type>( _testValue, _rv, String.Format( "Value of type {1} in the frame at {0} are not expected", _bidx, typeof( type ) ) );
      }
    }
    private static void CompareString( SBUSNet_message _dateFrame, string _testValue )
    {
      string _rv = (string)_dateFrame.ReadValue( 0, typeof( string ) );
      Assert.AreEqual<string>( _testValue, _rv, "Value of type string in the frame at are not equal" );
    }
  }
}
