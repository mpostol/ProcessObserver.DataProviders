
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace CAS.CommServer.DataProvider.TextReader.UnitTest
{
  [TestClass]
  public class AssemblyTraceEventUnitTest
  {
    [TestMethod]
    public void TracerTestMethod()
    {
      TraceSource _trace = AssemblyTraceEvent.Tracer;
      Assert.IsNotNull(_trace);
      Assert.AreEqual<string>("CAS.CommSvrPlugin_TextReader", _trace.Name);
    }
  }
}

