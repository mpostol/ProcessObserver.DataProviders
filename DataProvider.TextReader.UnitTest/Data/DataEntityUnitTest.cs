
using CAS.CommServer.DataProvider.TextReader.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CAS.CommServer.DataProvider.TextReader.UnitTest.Data
{
  [TestClass]
  public class DataEntityUnitTest
  {
    [TestMethod]
    public void ConstructorTestMethod()
    {
      DataEntity _instance = new DataEntity() { Tags = new string[10], TimeStamp = DateTime.Now };
      _instance.ReturnEmptyEnvelope(); //do nothing
      Assert.AreEqual(10, _instance.length);
      Assert.AreEqual(0, _instance.dataType);
      Assert.IsFalse(_instance.InPool);
      Assert.AreEqual(0, _instance.startAddress);
    }
    [TestMethod]
    public void InPoolTestMethod()
    {
      DataEntity _instance = new DataEntity() { Tags = new string[10], TimeStamp = DateTime.Now };
      Assert.IsFalse(_instance.InPool);
      _instance.InPool = true;
      Assert.IsFalse(_instance.InPool);
    }
    [TestMethod]
    public void IsInBlockTestMethod()
    {
      DataEntity _instance = new DataEntity() { Tags = new string[10], TimeStamp = DateTime.Now };
      Assert.IsTrue(_instance.IsInBlock(0, 0, 0));
      Assert.IsFalse(_instance.IsInBlock(0, 10, 0));
      Assert.IsFalse(_instance.IsInBlock(0, 0, 1));
      Assert.IsFalse(_instance.IsInBlock(1, 0, 0));
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ReadValueArgumentOutOfRangeTest()
    {
      DataEntity _instance = new DataEntity() { Tags = GetData(), TimeStamp = DateTime.Now };
      object _value = _instance.ReadValue(4, typeof(float));
    }
    [TestMethod]
    public void ReadValueStringTest()
    {
      DataEntity _instance = new DataEntity() { Tags = GetData(), TimeStamp = DateTime.Now };
      object _value = _instance.ReadValue(0, typeof(string));
      Assert.IsTrue(_value is string);
      Assert.AreEqual<string>("09-12-16", (string)_value);
      _value = _instance.ReadValue(2, typeof(string));
      Assert.IsTrue(_value is string);
      Assert.AreEqual<string>("26.9", (string)_value);
      _value = _instance.ReadValue(3, typeof(string));
      Assert.IsTrue(_value is string);
      Assert.AreEqual<string>("1368", (string)_value);
    }
    [TestMethod]
    public void ReadValueFloatTest()
    {
      DataEntity _instance = new DataEntity() { Tags = GetData(), TimeStamp = DateTime.Now };
      object _value = _instance.ReadValue(2, typeof(float));
      Assert.IsTrue(_value is float);
      Assert.AreEqual<float>(26.9f, (float)_value);
      _value = _instance.ReadValue(3, typeof(float));
      Assert.IsTrue(_value is float);
      Assert.AreEqual<float>(1368f, (float)_value);
    }
    [TestMethod]
    public void ReadValueLongTest()
    {
      DataEntity _instance = new DataEntity() { Tags = GetData(), TimeStamp = DateTime.Now };
      object _value = _instance.ReadValue(3, typeof(long));
      Assert.IsTrue(_value is long);
      Assert.AreEqual<long>(1368, (long)_value);
    }
    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ReadValueLongFormatExceptionTest()
    {
      DataEntity _instance = new DataEntity() { Tags = GetData(), TimeStamp = DateTime.Now };
      object _value = _instance.ReadValue(2, typeof(long));
    }
    [TestMethod]
    public void ReadValueIntTest()
    {
      DataEntity _instance = new DataEntity() { Tags = GetData(), TimeStamp = DateTime.Now };
      object _value = _instance.ReadValue(3, typeof(int));
      Assert.IsTrue(_value is int);
      Assert.AreEqual<long>(1368, (int)_value);
    }
    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ReadValueIntFormatExceptionTest()
    {
      DataEntity _instance = new DataEntity() { Tags = GetData(), TimeStamp = DateTime.Now };
      object _value = _instance.ReadValue(2, typeof(int));
    }
    [TestMethod]
    public void ReadValueShortTest()
    {
      DataEntity _instance = new DataEntity() { Tags = GetData(), TimeStamp = DateTime.Now };
      object _value = _instance.ReadValue(3, typeof(short));
      Assert.IsTrue(_value is short);
      Assert.AreEqual<long>(1368, (short)_value);
    }
    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ReadValueShortFormatExceptionTest()
    {
      DataEntity _instance = new DataEntity() { Tags = GetData(), TimeStamp = DateTime.Now };
      object _value = _instance.ReadValue(2, typeof(short));
    }
    [TestMethod]
    [ExpectedException(typeof(OverflowException))]
    public void ReadValueShortOverflowExceptionTest()
    {
      DataEntity _instance = new DataEntity() { Tags = GetData(), TimeStamp = DateTime.Now };
      _instance.Tags[3] = "33000";
      object _value = _instance.ReadValue(3, typeof(short));
    }
    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ReadValueWrongFormatTest()
    {
      DataEntity _instance = new DataEntity() { Tags = GetData(), TimeStamp = DateTime.Now };
      object _value = _instance.ReadValue(0, typeof(float));
    }
    private static string[] GetData()
    {
      return new string[] { "09-12-16", "09:24:02", "26.9", "1368" };
    }
  }
}
