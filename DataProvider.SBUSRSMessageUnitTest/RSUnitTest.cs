using System;
using CAS.Lib.CommonBus.ApplicationLayer.SBUS;
using CAS.Lib.CommonBus.ApplicationLayer.SBUS.PRIVATE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAS.UnitTests.CommonBus.ApplicationLayer.SBUS
{
  [TestClass]
  public class RSUnitTest
  {
    private const int c_station = 5;
    private const short C_dataType = (short)Medium_T.Register;

    class BlockAddress: CAS.Lib.CommonBus.ApplicationLayer.IBlockAddress
    {

      #region IBlockAddress Members

      public int station
      {
        get { return c_station; }
      }

      #endregion

      #region IBlockDescription Members

      public short dataType
      {
        get { return C_dataType; }
      }

      public int length
      {
        get { return 1; }
      }

      public int startAddress
      {
        get { return 1; }
      }

      #endregion
    }

    [TestMethod]
    [TestCategoryAttribute("SBUS message")]
    public void CreationRS()
    {
      SBUSRS_message _msg = new SBUSRS_message( null );
      Assert.IsNotNull( _msg, "Message is not created." );
      BlockAddress _tb = new BlockAddress();
      _msg.Test_PrepareRequest( _tb.station, _tb );
      Assert.AreEqual( _msg.userDataLength, 7, "Request length is not valid" );
    }
  }
}

//Let’s take the telegram
//Write Register 100 12345 Dec
//to station 10 in the SAIA-BUS network. The telegram will look like so:
//<0A><0E><05><00><64><00><00><30><39>< CRC-16msbyte
//>
//< CRC-16lsbyte