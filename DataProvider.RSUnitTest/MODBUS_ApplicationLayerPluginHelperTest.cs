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

namespace CAS.Lib.CommonBus.ApplicationLayer.RS.UT
{
  /// <summary>
  ///This is a test class for all *_ApplicationLayerPluginHelper constructors and is intended
  ///to contain all licenses tests.
  ///</summary>
  [TestClass()]
  public class ApplicationLayerPluginHelperTest
  {
    private TestContext testContextInstance;
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
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get { return testContextInstance; }
      set { testContextInstance = value; }
    }
    /// <summary>
    /// A test for ModBus_ApplicationLayerPluginHelper Constructor
    /// </summary>
    [TestMethod()]
    public void ModBus_ApplicationLayerPluginHelperConstructorTest()
    {
      Type tp = typeof( ModBus_ApplicationLayerPluginHelper );
      Assert.AreEqual( tp.GUID.CompareTo( new Guid( "746aa023-ece4-427e-bece-db0845716029" ) ), 0, "Guid odes not much" );
      try
      {
        ModBus_ApplicationLayerPluginHelper target = new ModBus_ApplicationLayerPluginHelper();
      }
      catch ( System.ComponentModel.LicenseException ex )
      {
        TestContext.WriteLine( "Text of expected license exception" );
        TestContext.WriteLine( ex.Message );
        Assert.IsTrue( true );
      }
    }
  }
}
