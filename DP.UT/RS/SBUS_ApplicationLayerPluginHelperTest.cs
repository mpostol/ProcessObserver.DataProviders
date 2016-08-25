using System;
using CAS.Lib.CommonBus.ApplicationLayer.SBUS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace CAS.Lib.CommonBus.ApplicationLayer.RS.UT
{
  /// <summary>
  ///This is a test class for SBUS_ApplicationLayerPluginHelperTest and is intended
  ///to contain all SBUS_ApplicationLayerPluginHelperTest Unit Tests
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
    ///A test for SBUS_ApplicationLayerPluginHelper Constructor
    ///</summary>
    [TestMethod()]
    public void SBUS_ApplicationLayerPluginHelperConstructorTest()
    {
      Type tp = typeof( SBUS_ApplicationLayerPluginHelper );
      Assert.AreEqual( tp.GUID.CompareTo( new Guid( "D83FA3B0-161A-4426-A2EB-B6C513A8CEE8" ) ), 0, "Guid odes not much" );
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
