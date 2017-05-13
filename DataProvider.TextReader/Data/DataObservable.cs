
using System;
using System.Reactive;

namespace CAS.CommServer.DataProvider.TextReader.Data
{
  public class DataObservable : ObservableBase<DataEntity>
  {
    protected override IDisposable SubscribeCore(IObserver<DataEntity> observer)
    {
      throw new NotImplementedException();
    }
    public DataObservable()
    {
      //Usi it to read strings.
      //FileStream _configurationStream = _configurationFile.OpenRead();
      //string _configurationText = String.Empty;
      //using (System.IO.TextReader _tr = new System.IO.StreamReader(_configurationStream))
      //  _configurationText = _tr.ReadToEnd();
    }
  }
  public class DataEntity
  {
    public DateTime TimeStamp { get; set; }
    public float[] Tags { get; set; }
  }

}
