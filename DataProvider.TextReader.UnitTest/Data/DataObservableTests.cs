using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAS.CommServer.DataProvider.TextReader.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Linq;

namespace CAS.CommServer.DataProvider.TextReader.Data.Tests
{
  [TestClass()]
  public class DataObservableTests
  {
    [TestMethod()]
    public void DataObservableTest()
    {
      IObservable<long> _observer = System.Reactive.Linq.Observable.Interval(TimeSpan.FromSeconds(1));
      List<long> _buffer = new List<long>();
      IDisposable _client = _observer.Subscribe(x => { _buffer.Add(x); });
      while (_buffer.Count < 2)
        System.Threading.Thread.Sleep(800);
      _client.Dispose();

    }
    [TestMethod]
    public void TimeoutTestMethod()
    {
      ProcessDataObservable _mainDataSource = new ProcessDataObservable();
      IObservable<DataEntity> _timout = _mainDataSource.Timeout(TimeSpan.FromMilliseconds(100));
      Exception _lastError = null;
      IDisposable _observer = _timout.Subscribe<DataEntity>
        (
         x => { },
         exception => { _lastError = exception; },
         () => { }
        );
      System.Threading.Thread.Sleep(2300);
      Assert.IsNotNull(_lastError);
      Console.WriteLine(_lastError);
    }
    private class ProcessDataObservable : IObservable<DataEntity>
    {
      public IDisposable Subscribe(IObserver<DataEntity> observer)
      {
        Assert.IsNotNull(observer);
        return new MyObserver();
      }
      private class MyObserver : IDisposable
      {
        public void Dispose()
        {
          throw new NotImplementedException();
        }
      }
    }
  }
}