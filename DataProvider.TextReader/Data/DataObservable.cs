
using CAS.Lib.CommonBus.ApplicationLayer;
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
  public class DataEntity: IReadValue
  {
    public DateTime TimeStamp { get; set; }
    public float[] Tags { get; set; }

    #region IReadValue
    public int startAddress
    {
      get { return 0; }
    }
    public int length
    {
      get
      {
        return Tags.Length;
      }
    }
    public short dataType
    {
      get { return 0; }
    }
    public bool InPool
    {
      get
      {
        return false;
      }
      set { }
    }
    public bool IsInBlock(uint station, ushort address, short type)
    {
      return (station == 0) && (address <= startAddress + Tags.Length) && (type == dataType);
    }
    /// <summary>
    /// Reads the value.
    /// </summary>
    /// <param name="regAddress">The reg address.</param>
    /// <param name="pCanonicalType">Type of the p canonical.</param>
    /// <returns>System.Object.</returns>
    public object ReadValue(int regAddress, Type pCanonicalType)
    {
      if (pCanonicalType != typeof(float))
        throw new NotImplementedException($"The canical type {pCanonicalType.ToString()} is not implemented - only {typeof(float).ToString()} is supported");
      if (!IsInBlock(0, (ushort)regAddress, 0))
        throw new ArgumentOutOfRangeException($"The register address is out of the expected range");
      return Tags[regAddress - startAddress];
    }
    public void ReturnEmptyEnvelope() { }
    #endregion

  }

}
