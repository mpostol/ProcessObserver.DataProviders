
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Diagnostics;
using System.Threading;

namespace CAS.CommServer.DataProvider.TextReader.Data.Tests
{
  [TestClass()]
  [DeploymentItem(@"TestingData\", "TestingData")]
  public class DataObservableTests
  {
    [TestMethod()]
    public void DataObservableTest()
    {
      Assert.IsTrue(File.Exists(_fileName));
      List<DataEntity> _buffer = new List<DataEntity>();
      Stopwatch _watch = Stopwatch.StartNew();
      TestTraceSource _trace = new TestTraceSource();
      using (DataObservable _dataSource = new DataObservable(_fileName, TestTextReaderProtocolParameters.InstanceData(), _trace))
      {
        Exception _exception = null;
        int _nextExecutedCount = 0;
        using (IDisposable _client = _dataSource
          .Do<DataEntity>(x => _nextExecutedCount++)
          .Subscribe(x => { _buffer.Add(x); _watch.Stop(); }, exception => _exception = exception))
        {
          Assert.AreEqual<int>(0, _buffer.Count);
          string[] _content = File.ReadAllLines(_fileName);
          Assert.AreEqual<int>(2422, _content.Length);
          File.WriteAllLines(_fileName, _content);
          Thread.Sleep(4000);
          Assert.IsNull(_exception, $"{_exception}");
          Assert.AreEqual<int>(2, _trace.TraceBuffer.Count);
          Console.WriteLine(_trace.TraceBuffer[0].ToString());
          Console.WriteLine(_trace.TraceBuffer[1].ToString());
          Assert.AreEqual<int>(1, _nextExecutedCount, $"Execution cout: {_nextExecutedCount}");
          Assert.AreEqual<int>(1, _buffer.Count);
          Assert.AreEqual<int>(39, _buffer[0].Tags.Length);
          Assert.AreEqual<string>("09-12-16", _buffer[0].Tags[0]);
          Assert.AreEqual<string>("09:24:02", _buffer[0].Tags[1]);
          Assert.AreEqual<string>("", _buffer[0].Tags[38]);
          Assert.IsTrue(_watch.ElapsedMilliseconds < 2500, $"Elapsed: {_watch.ElapsedMilliseconds}");
          Console.WriteLine($"Time execution: {_watch.ElapsedMilliseconds}");
        }
      }
    }
    [TestMethod]
    public void TimeoutTestMethod()
    {
      Assert.IsTrue(File.Exists(_fileName));
      List<DataEntity> _buffer = new List<DataEntity>();
      Stopwatch _watch = Stopwatch.StartNew();
      TestTraceSource _trace = new TestTraceSource();
      using (DataObservable _dataSource = new DataObservable(_fileName, new TestTextReaderProtocolParameters() { FileModificationNotificationTimeout = 10000 }, _trace))
      {
        Exception _exception = null;
        int _nextExecutedCount = 0;
        using (IDisposable _client = _dataSource
          .Do<DataEntity>(x => _nextExecutedCount++)
          .Subscribe(x => { _buffer.Add(x); _watch.Stop(); }, exception => _exception = exception))
        {
          string[] _content = File.ReadAllLines(_fileName);
          File.WriteAllLines(_fileName, _content);
          Thread.Sleep(14000);
          Assert.IsNotNull(_exception, $"{_exception}");
          Assert.IsTrue(_exception is TimeoutException);
          Assert.AreEqual<int>(3, _trace.TraceBuffer.Count);
          Console.WriteLine(_trace.TraceBuffer[0].ToString());
          Console.WriteLine(_trace.TraceBuffer[1].ToString());
          Console.WriteLine(_trace.TraceBuffer[2].ToString());
          Assert.AreEqual<int>(1, _nextExecutedCount, $"Execution cout: {_nextExecutedCount}");
          Assert.AreEqual<int>(1, _buffer.Count);
          Assert.IsTrue(_watch.ElapsedMilliseconds < 2500, $"Elapsed: {_watch.ElapsedMilliseconds}");
          Console.WriteLine($"Time execution: {_watch.ElapsedMilliseconds}");
        }
      }
    }
    private const string _fileName = @"TestingData\g1765xa1.1";

    private class TestTraceSource : ITraceSource
    {

      public List<Tuple<TraceEventType, int, string>> TraceBuffer { get; private set; } = new List<Tuple<TraceEventType, int, string>>();
      public void TraceMessage(TraceEventType eventType, int id, string message)
      {
        TraceBuffer.Add(new Tuple<TraceEventType, int, string>(eventType, id, message));
      }
    }
    private class TestTextReaderProtocolParameters : TextReaderProtocolParameters
    {
      private static TestTextReaderProtocolParameters m_Signleton;
      public static TestTextReaderProtocolParameters InstanceData()
      {
        if (m_Signleton == null)
          m_Signleton = new TestTextReaderProtocolParameters();
        return m_Signleton;
      }
      public override string ToString()
      {
        return $"ColumnSeparator: \"{ColumnSeparator}\", DelayFileScann: {TimeSpan.FromMilliseconds(DelayFileScan)}, Timeou: {TimeSpan.FromMilliseconds(FileModificationNotificationTimeout)}";
      }
    }
  }
}