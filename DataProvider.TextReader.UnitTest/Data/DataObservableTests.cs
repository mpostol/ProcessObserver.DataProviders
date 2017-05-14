using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace CAS.CommServer.DataProvider.TextReader.Data.Tests
{
  [TestClass()]
  public class DataObservableTests
  {
    [TestMethod()]
    public void DataObservableTest()
    {
      IObservable<long> _observer = Observable.Interval(TimeSpan.FromSeconds(1));
      List<long> _buffer = new List<long>();
      IDisposable _client = _observer.Subscribe(x => { _buffer.Add(x); });
      while (_buffer.Count < 2)
        System.Threading.Thread.Sleep(800);
      _client.Dispose();

    }
    [TestMethod]
    public void TimeoutTestMethod()
    {
      IObservable<DataEntity> _timout =
        Observable.Interval(TimeSpan.FromMilliseconds(100)).
                   Where<long>(x => x < 10).
                   Select<long, DataEntity>(x => new DataEntity() { TimeStamp = DateTime.Now, Tags = new float[] { x, x, x, x, x } }).
                   Timeout<DataEntity>(TimeSpan.FromMilliseconds(2000));
      Queue<DataEntity> _buffer = new Queue<DataEntity>();
      Exception _lastError = null;
      bool _finished = false;
      IDisposable _observer = _timout.Subscribe<DataEntity>
        (
         x => _buffer.Enqueue(x),
         exception => { _lastError = exception; },
         () => _finished = true
        );
      System.Threading.Thread.Sleep(4000);
      Assert.AreEqual<int>(10, _buffer.Count);
      Assert.IsNotNull(_lastError);
      Console.WriteLine(_lastError);
      Assert.IsFalse(_finished);
      while (_buffer.Count > 0)
      {
        DataEntity _last = _buffer.Dequeue();
        Console.WriteLine($"{_last.TimeStamp.ToLongTimeString()}: {string.Join(", ", _last.Tags) }");
      }
    }
  }
}