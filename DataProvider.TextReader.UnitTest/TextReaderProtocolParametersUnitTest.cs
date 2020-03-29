//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CAS.CommServer.DataProvider.TextReader.UnitTest
{
  [TestClass]
  public class TextReaderProtocolParametersUnitTest
  {
    [TestMethod]
    public void ConstructorTestMethod()
    {
      TextReaderProtocolParameters _instance = new TextReaderProtocolParameters();
      Assert.AreEqual<TimeSpan>(TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(_instance.DelayFileScan));
      Assert.AreEqual<TimeSpan>(TimeSpan.FromMinutes(1), TimeSpan.FromMilliseconds(_instance.FileModificationNotificationTimeout));
      Assert.AreEqual<string>(",", _instance.ColumnSeparator);
    }

    [TestMethod]
    public void ToStringTestMethod()
    {
      TextReaderProtocolParameters _instance = new TextReaderProtocolParameters();
      Assert.AreEqual<string>(TextReaderProtocolParametersString, _instance.ToString());
    }

    internal const string TextReaderProtocolParametersString = "ColumnSeparator: \",\", DelayFileScann: 00:00:01, Timeout: 00:01:00";
  }
}