using System;
using CAS.Lib.CommonBus.ApplicationLayer.SBUS.PRIVATE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAS.UnitTests.CommonBus.ApplicationLayer.SBUS.NET
{
  [TestClass]
  public class RequestWriteTest
  {

    [TestMethod]
    [TestCategoryAttribute("NET SBUS message")]
    [ExpectedException(typeof(ApplicationException), "Expected ApplicationException")]
    public void RequestWrite_ReadValueTest()
    {
#if !DEBUG
      Assert.Inconclusive("This test requires DEBUG context");
#endif
      using (SBUSNet_message _frame = new SBUSNet_message(null))
      {
        Assert.IsNotNull(_frame, "Message is not created.");
        BlockAddress _tb = new BlockAddress((short)Medium_T.Text);
        _frame.Test_PrepareReqWriteValue(_tb, _tb.station);
        Assert.AreEqual(29, _frame.userDataLength, "The length of the request is not valid");
        object _retValue = _frame.ReadValue(0, typeof(string));
      }
    }

    [TestMethod]
    [TestCategoryAttribute("NET SBUS message")]
    [ExpectedException(typeof(ArgumentException), "Expected ApplicationException")]
    public void RequestWrite_ToInputException()
    {
#if !DEBUG
      Assert.Inconclusive("This test requires DEBUG context");
#endif
      using (SBUSNet_message _frame = new SBUSNet_message(null))
      {
        Assert.IsNotNull(_frame, "Message is not created.");
        BlockAddress _tb = new BlockAddress((short)Medium_T.Input);
        _frame.Test_PrepareReqWriteValue(_tb, _tb.station);
      }
    }

    [TestMethod]
    [TestCategoryAttribute("NET SBUS message")]
    public void RequestWrite_RequestContentTest()
    {
#if !DEBUG
      Assert.Inconclusive("This test requires DEBUG context");
#endif
      const int c_station = 231;
      const ushort c_startAddress = 1;
      const byte c_RegistersCount = 1;
      const byte c_HeaderLength = 8;
      const byte c_ATLength = 1;
      foreach (Medium_T _enumIdx in Enum.GetValues(typeof(Medium_T)))
      {
        if (_enumIdx == Medium_T.Input)
          continue;
        using (SBUSNet_message _frame = new SBUSNet_message(null))
        {
          Assert.IsNotNull(_frame, "Message is not created.");
          BlockAddress _tb = new BlockAddress((short)_enumIdx, c_station, c_startAddress, c_RegistersCount);
          _frame.Test_PrepareReqWriteValue(_tb, _tb.station);
          ushort _expectedFrameLength = c_HeaderLength + c_ATLength + 1 + 1; //<head><attribute><station><cmd>
          byte[] _frameBuff = null;
          RequestSimulator _sim = null;
          switch (_enumIdx)
          {
            case Medium_T.Flag:
            case Medium_T.Input:
            case Medium_T.Output:
              _expectedFrameLength += 1 + 2 + 1 + c_RegistersCount / 8 + 1; //<w-count> <address-IOF> <fio-count> {<fio-byte>}+
              bool _testBool = true;
              for (int _bx = 0; _bx < c_RegistersCount; _bx++)
                _frame.WriteValue(_testBool, _bx);
              CheckExpectedLength(_enumIdx, _frame, _expectedFrameLength);
              _frameBuff = _frame.GetManagedBuffer();
              _sim = new RequestSimulator(c_RegistersCount, _testBool, _expectedFrameLength, c_station, _enumIdx, c_startAddress);
              _sim[7] = _frameBuff[7];
              byte _mask = 0xff >> 8 - c_RegistersCount % 8;
              _sim[_expectedFrameLength - 1] &= _mask;
              _frameBuff[_expectedFrameLength - 1] &= _mask;
              CollectionAssert.AreEqual(_sim, _frameBuff, "Frame and its template must be equal");
              break;
            case Medium_T.Register:
            case Medium_T.Timer:
            case Medium_T.Counter:
              _expectedFrameLength += 1 + 2 + c_RegistersCount * 4; //<w-count> <address-RTC> {<4-byte>}+
              CheckExpectedLength(_enumIdx, _frame, _expectedFrameLength);
              int[] _testValuesArray = new int[] { int.MaxValue, int.MinValue, 0, -123456, 7654321 };
              foreach (int _itv in _testValuesArray)
              {
                for (int _iix = 0; _iix < c_RegistersCount; _iix++)
                  _frame.WriteValue(_itv, _iix);
                _frameBuff = _frame.GetManagedBuffer();
                _sim = new RequestSimulator(c_RegistersCount, _itv, _expectedFrameLength, c_station, _enumIdx, c_startAddress);
                _sim[7] = _frameBuff[7];
                CollectionAssert.AreEqual(_sim, _frameBuff, "Frame and its template must be equal");
              }
              break;
            case Medium_T.Text:
              _expectedFrameLength += 1 + 1 + 2 + 1 + c_RegistersCount;  //<w-count> <text-number> <char-position> {<ascii-char>}+
              CheckExpectedLength(_enumIdx, _frame, _expectedFrameLength);
              string[] _testString = new string[] { "0123456789012345678901", "0123456789012", "012345678", null, String.Empty };
              foreach (string _ts in _testString)
              {
                _frame.WriteValue(_ts, 0);
                _frameBuff = _frame.GetManagedBuffer();
                _sim = new RequestSimulator(c_RegistersCount, _ts, _expectedFrameLength, c_station, _enumIdx, c_startAddress);
                _sim[7] = _frameBuff[7];
                CollectionAssert.AreEqual(_sim, _frameBuff, "Frame and its template must be equal");
              }
              break;
            default:
              Assert.Fail("Application error: unknown requested data type");
              break;
          }
        }
      }
    }
    private static string CheckExpectedLength(Medium_T _enumIdx, SBUSNet_message _dateFrame, ushort _expectedFrameLength)
    {
      string _msg = string.Format("The length of the response for {0} is not valid", _enumIdx);
      Assert.AreEqual(_expectedFrameLength, _dateFrame.userDataLength, _msg);
      using (SBUSNet_message _newFrame = new SBUSNet_message(null))
      {
        _newFrame.PrepareFrameToBeSend(_dateFrame);
        Assert.AreEqual(_expectedFrameLength + c_CRCLength, _newFrame.userDataLength, _msg); //
      }
      return _msg;
    }

    private const int c_CRCLength = 2;
  }
}
