//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAS.CommServer.DataProvider.TextReader.UnitTest
{
  [TestClass]
  public class IsLicensedTextReaderDataProviderIDUnitTest
  {
    [TestMethod]
    public void TestMethod1()
    {
      IsLicensedTextReaderDataProviderID _isLicense = new IsLicensedTextReaderDataProviderID();
      Assert.IsFalse(_isLicense.Licensed);
      Assert.IsFalse(_isLicense.RunTime.HasValue);
      Assert.IsFalse(_isLicense.Volume.HasValue);
      Assert.IsNull(_isLicense.Warning);
    }
  }
}