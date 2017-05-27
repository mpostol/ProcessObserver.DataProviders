
using CAS.CommServer.DataProvider.TextReader.Data;
using CAS.Lib.CommonBus.ApplicationLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CAS.CommServer.DataProvider.TextReader.UnitTest.Data
{
  [TestClass]
  public class ReadDataEntityUnitTest
  {
    [TestMethod]
    public void ConstructorTestMethod()
    {
      TestDataEntity _data = new TestDataEntity() { Tags = new string[8], TimeStamp = DateTime.Now };
      ReadDataEntity _instance = new ReadDataEntity(_data, new Block());
      _instance.ReturnEmptyEnvelope(); //do nothing
      Assert.AreEqual(4, _instance.length);
      Assert.AreEqual(0, _instance.dataType);
      Assert.IsFalse(_instance.InPool);
      Assert.AreEqual(4, _instance.startAddress);
    }
    [TestMethod]
    public void InPoolTestMethod()
    {
      TestDataEntity _data = new TestDataEntity() { Tags = new string[8], TimeStamp = DateTime.Now };
      ReadDataEntity _instance = new ReadDataEntity(_data, new Block());
      Assert.IsFalse(_instance.InPool);
      _instance.InPool = true;
      Assert.IsFalse(_instance.InPool);
    }
    [TestMethod]
    public void IsInBlockTestMethod()
    {
      TestDataEntity _data = new TestDataEntity() { Tags = new string[8], TimeStamp = DateTime.Now };
      ReadDataEntity _instance = new ReadDataEntity(_data, new Block());
      Assert.IsTrue(_instance.IsInBlock(0, 0, 0));
      Assert.IsFalse(_instance.IsInBlock(0, 10, 0));
      Assert.IsFalse(_instance.IsInBlock(0, 0, 1));
      Assert.IsFalse(_instance.IsInBlock(1, 0, 0));
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ReadValueArgumentOutOfRangeTest()
    {
      TestDataEntity _data = new TestDataEntity() { Tags = GetData(), TimeStamp = DateTime.Now };
      ReadDataEntity _instance = new ReadDataEntity(_data, new Block());
      object _value = _instance.ReadValue(8, typeof(float));
    }
    [TestMethod]
    public void ReadValueStringTest()
    {
      TestDataEntity _data = new TestDataEntity() { Tags = GetData(), TimeStamp = DateTime.Now };
      ReadDataEntity _instance = new ReadDataEntity(_data, new Block());
      object _value = _instance.ReadValue(4, typeof(string));
      Assert.IsTrue(_value is string);
      Assert.AreEqual<string>("09-12-16", (string)_value);
      _value = _instance.ReadValue(5, typeof(string));
      Assert.IsTrue(_value is string);
      Assert.AreEqual<string>("09:24:02", (string)_value);
      _value = _instance.ReadValue(6, typeof(string));
      Assert.IsTrue(_value is string);
      Assert.AreEqual<string>("26.9", (string)_value);
      _value = _instance.ReadValue(7, typeof(string));
      Assert.IsTrue(_value is string);
      Assert.AreEqual<string>("1368", (string)_value);
    }
    [TestMethod]
    public void ReadValueFloatTest()
    {
      TestDataEntity _data = new TestDataEntity() { Tags = GetData(), TimeStamp = DateTime.Now };
      ReadDataEntity _instance = new ReadDataEntity(_data, new Block());
      object _value = _instance.ReadValue(6, typeof(float));
      Assert.IsTrue(_value is float);
      Assert.AreEqual<float>(26.9f, (float)_value);
      _value = _instance.ReadValue(7, typeof(float));
      Assert.IsTrue(_value is float);
      Assert.AreEqual<float>(1368f, (float)_value);
    }
    [TestMethod]
    public void ReadValueLongTest()
    {
      TestDataEntity _data = new TestDataEntity() { Tags = GetData(), TimeStamp = DateTime.Now };
      ReadDataEntity _instance = new ReadDataEntity(_data, new Block());
      object _value = _instance.ReadValue(7, typeof(long));
      Assert.IsTrue(_value is long);
      Assert.AreEqual<long>(1368, (long)_value);
    }
    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ReadValueLongFormatExceptionTest()
    {
      TestDataEntity _data = new TestDataEntity() { Tags = GetData(), TimeStamp = DateTime.Now };
      ReadDataEntity _instance = new ReadDataEntity(_data, new Block());
      object _value = _instance.ReadValue(6, typeof(long));
    }
    [TestMethod]
    public void ReadValueIntTest()
    {
      TestDataEntity _data = new TestDataEntity() { Tags = GetData(), TimeStamp = DateTime.Now };
      ReadDataEntity _instance = new ReadDataEntity(_data, new Block());
      object _value = _instance.ReadValue(7, typeof(int));
      Assert.IsTrue(_value is int);
      Assert.AreEqual<long>(1368, (int)_value);
    }
    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ReadValueIntFormatExceptionTest()
    {
      TestDataEntity _data = new TestDataEntity() { Tags = GetData(), TimeStamp = DateTime.Now };
      ReadDataEntity _instance = new ReadDataEntity(_data, new Block());
      object _value = _instance.ReadValue(4, typeof(int));
    }
    [TestMethod]
    public void ReadValueShortTest()
    {
      TestDataEntity _data = new TestDataEntity() { Tags = GetData(), TimeStamp = DateTime.Now };
      ReadDataEntity _instance = new ReadDataEntity(_data, new Block());
      object _value = _instance.ReadValue(7, typeof(short));
      Assert.IsTrue(_value is short);
      Assert.AreEqual<long>(1368, (short)_value);
    }
    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ReadValueShortFormatExceptionTest()
    {
      TestDataEntity _data = new TestDataEntity() { Tags = GetData(), TimeStamp = DateTime.Now };
      ReadDataEntity _instance = new ReadDataEntity(_data, new Block());
      object _value = _instance.ReadValue(6, typeof(short));
    }
    [TestMethod]
    [ExpectedException(typeof(OverflowException))]
    public void ReadValueShortOverflowExceptionTest()
    {
      TestDataEntity _data = new TestDataEntity() { Tags = GetData(), TimeStamp = DateTime.Now };
      _data.Tags[7] = "33000";
      ReadDataEntity _instance = new ReadDataEntity(_data, new Block());
      object _value = _instance.ReadValue(7, typeof(short));
    }
    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ReadValueWrongFormatTest()
    {
      TestDataEntity _data = new TestDataEntity() { Tags = GetData(), TimeStamp = DateTime.Now };
      ReadDataEntity _instance = new ReadDataEntity(_data, new Block());
      object _value = _instance.ReadValue(4, typeof(float));
    }
    private static string[] GetData()
    {
      return new string[] { "09-12-16", "09:24:02", "26.9", "1368", "09-12-16", "09:24:02", "26.9", "1368" };
    }
    private class Block : IBlockDescription
    {
      public short dataType
      {
        get; private set;
      } = 0;
      public int length
      {
        get; private set;
      } = 4;
      public int startAddress
      {
        get; private set;
      } = 4;
    }
    private class TestDataEntity : IDataEntity
    {
      public string[] Tags
      {
        get; set;
      }
      public DateTime TimeStamp
      {
        get; set;
      }
    }
  }
}
