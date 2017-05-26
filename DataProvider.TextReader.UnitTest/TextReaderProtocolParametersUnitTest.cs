using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
  }
}
