//<summary>
//  Title   :  UnitTests for SBUS_ApplicationLayerPluginHelper/NET DataProvider
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    <Author> - <date>:
//    <description>
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.com.pl
//  http:\\www.cas.eu
//</summary>
      
using CAS.Lib.CommonBus.ApplicationLayer.SBUS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CAS.Lib.CommonBus.ApplicationLayer.NET.UT
{
  /// <summary>
  ///This is a test class for <see cref="SBUS_ApplicationLayerPluginHelper"/> and is intended
  ///to contain all Unit Tests
  ///</summary>
  [TestClass()]
  public class SBUS_ApplicationLayerPluginHelperTest
  {
    private TestContext testContextInstance;
    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }
    #region Additional test attributes
    // 
    //You can use the following additional attributes as you write your tests:
    //
    //Use ClassInitialize to run code before running the first test in the class
    //[ClassInitialize()]
    //public static void MyClassInitialize(TestContext testContext)
    //{
    //}
    //
    //Use ClassCleanup to run code after all tests in a class have run
    //[ClassCleanup()]
    //public static void MyClassCleanup()
    //{
    //}
    //
    //Use TestInitialize to run code before running each test
    //[TestInitialize()]
    //public void MyTestInitialize()
    //{
    //}
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion
    /// <summary>
    /// A test for <see cref="SBUS_ApplicationLayerPluginHelper"/> Constructor
    /// </summary>
    [TestMethod()]
    public void SBUS_ApplicationLayerPluginHelperConstructorTest()
    {
      Type tp = typeof( SBUS_ApplicationLayerPluginHelper );
      Assert.AreEqual( tp.GUID.CompareTo( new Guid( "E1F88110-13B3-4c70-9234-C8A95AAB9BCF" ) ), 0, "Guid odes not much" );
      try
      {
        SBUS_ApplicationLayerPluginHelper target = new SBUS_ApplicationLayerPluginHelper();
      }
      catch ( System.ComponentModel.LicenseException ex )
      {
        TestContext.WriteLine( ex.Message );
        Assert.IsTrue( true );
      }
    }
  }
}
