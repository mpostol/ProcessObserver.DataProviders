using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAS.Lib.CommonBus.ApplicationLayer.SBUS.PRIVATE;

namespace CAS.UnitTests.CommonBus.ApplicationLayer.SBUS.NET
{
  /// <summary>
  /// Summary description for ResponseExpectedException
  /// </summary>
  [TestClass]
  public class ResponseExpectedException
  {
    public ResponseExpectedException()
    {
      //
      // TODO: Add constructor logic here
      //
    }

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
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use ClassCleanup to run code after all tests in a class have run
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // Use TestInitialize to run code before running each test 
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // Use TestCleanup to run code after each test has run
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //
    #endregion

    [TestMethod]
    [TestCategoryAttribute( "NET SBUS message" )]
    [ExpectedException( typeof( ApplicationException ), "Expected ApplicationException" )]
    public void ResponseWriteValue()
    {
      using ( SBUSNet_message _frame = new SBUSNet_message( null ) )
      {
        Assert.IsNotNull( _frame, "Message is not created." );
        BlockAddress _tb = new BlockAddress( (short)Medium_T.Text );
        _frame.Test_PrepareRequest( _tb.station, _tb );
        Assert.AreEqual( 16, _frame.userDataLength, "The length of the request is not valid" );
        using ( SBUSNet_message _dateFrame = new SBUSNet_message( null ) )
        {
          _dateFrame.InitMsg( _frame );
          _dateFrame.WriteValue( "sassasasa", 0 );
        }
      }
    }
  }
}
