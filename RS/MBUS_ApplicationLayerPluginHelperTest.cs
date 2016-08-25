//<summary>
//  Title   : Name of Application
//  System  : Microsoft Visual C# .NET 2008
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Lib.CommonBus.ApplicationLayer.MBUS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CAS.Lib.CommonBus.ApplicationLayer.RS.UT
{
  /// <summary>
  ///This is a test class for <see cref="MBUS_ApplicationLayerPluginHelper"/> and is intended
  ///to contain all Unit Tests
  ///</summary>
  [TestClass()]
  public class MBUS_ApplicationLayerPluginHelperTest
  {
    private TestContext testContextInstance;
    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get { return testContextInstance; }
      set { testContextInstance = value; }
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
    ///A test for MBUS_ApplicationLayerPluginHelper Constructor
    ///</summary>
    [TestMethod()]
    public void MBUS_ApplicationLayerPluginHelperConstructorTest()
    {
      Type tp = typeof( MBUS_ApplicationLayerPluginHelper );
      Assert.AreEqual( tp.GUID.CompareTo( new Guid( "B421239F-7051-44cf-BC87-B8B659BE31EC" ) ), 0, "Guid does not much" );
      try
      {
        MBUS_ApplicationLayerPluginHelper target = new MBUS_ApplicationLayerPluginHelper();
      }
      catch ( System.ComponentModel.LicenseException ex )
      {
        TestContext.WriteLine( ex.Message );
        Assert.IsTrue( true );
      }
    }
  }
}
