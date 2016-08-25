//<summary>
//  Title   : Unit Tests for Plug-in assemblies
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

using CAS.Lib.CommonBus.ApplicationLayer.ModBus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CAS.Lib.CommonBus.ApplicationLayer.NET.UT
{
  /// <summary>
  ///This is a test class for ModBus_ApplicationLayerPluginHelperTest and is intended
  ///to contain all ModBus_ApplicationLayerPluginHelperTest Unit Tests
  ///</summary>
  [TestClass()]
  public class ModBus_ApplicationLayerPluginHelperTest
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
    ///A test for ModBus_ApplicationLayerPluginHelper Constructor
    ///</summary>
    [TestMethod()]
    public void ModBus_ApplicationLayerPluginHelperConstructorTest()
    {
      Type tp = typeof( ModBus_ApplicationLayerPluginHelper );
      Assert.AreEqual( tp.GUID.CompareTo( new Guid( "1c38514b-6801-44de-aab9-1e406b3aae77" ) ), 0, "Guid odes not much" );
      try
      {
        ModBus_ApplicationLayerPluginHelper target = new ModBus_ApplicationLayerPluginHelper();
      }
      catch ( System.ComponentModel.LicenseException ex )
      {
        TestContext.WriteLine( ex.Message );
        Assert.IsTrue( true );
      }
    }
  }
}
