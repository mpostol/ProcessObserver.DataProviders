//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.CommServer.DataProvider.TextReader.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CAS.CommServer.DataProvider.TextReader.UnitTest.Data
{
  [TestClass]
  [DeploymentItem(@"TestingData\", "TestingData")]
  public class DataEntityUnitTest
  {
    [TestMethod]
    public void ConstructorTestMethod()
    {
      IDataEntity _instance = DataEntity.ReadFile(m_FileName, new DateTime(2017, 05, 27), ",");
      Assert.IsNotNull(_instance);
      Assert.AreEqual<DateTime>(new DateTime(2017, 05, 27), _instance.TimeStamp);
      CollectionAssert.AreEqual(_line, _instance.Tags);
    }

    private const string m_FileName = @"TestingData\g1765xa1.1";

    private readonly string[] _line = new string[]
    {
      "09-12-16", "09:24:02", " 26.9", " 26.2", " 25.4", " 28.9", " 25.7", " 28.2", " 27.4", " 22.5",
      " 21.8",    " 22.5",    " 22.5", " 1368", "  900", " 1500", " 87",   " 85",   " 87",   "   59",
      "   20",    "   85",    "   85", "   60", "   60", " 21.8", " 20.3", " 23.8", " 14.7", " 14.3",
      "  6",      "  0",      "  0",   " 70",   "  5.8", " 84",   "  4.8", " 14.5", ""
     };
  }
}