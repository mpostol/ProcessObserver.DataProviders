//_______________________________________________________________
//  Title   : Name of Application
//  System  : Microsoft VisualStudio 2015 / C#
//  $LastChangedDate:  $
//  $Rev: $
//  $LastChangedBy: $
//  $URL: $
//  $Id:  $
//
//  Copyright (C) 2017, CAS LODZ POLAND.
//  TEL: +48 608 61 98 99 
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using CAS.Lib.CommonBus;
using CAS.Lib.CommonBus.ApplicationLayer;
using CAS.Lib.RTLib.Management;
using System;
using System.ComponentModel;
using System.IO;

namespace CAS.CommServer.DataProvider.TextReader
{
  public class TextReaderApplicationLayerMaster : IApplicationLayerMaster
  {
    private class ProcessData : IReadValue
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
    private IComponent m_parentComponent;
    private IProtocolParent m_statistic;
    private ProcessData m_LastProcessData = null;
    private bool UpdateData(FileInfo _dataFile)
    {
      m_LastProcessData = new ProcessData()
      {
        TimeStamp = DateTime.Now,
        Tags = new float[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }
      };
      return true;
    }
    private void ParentComponent_Disposed(object sender, EventArgs e)
    {
      Dispose();
    }

    #region constructors
    public TextReaderApplicationLayerMaster(IProtocolParent statistic, IComponent parentComponent)
    {
      m_statistic = statistic;
      m_parentComponent = parentComponent;
      m_parentComponent.Disposed += ParentComponent_Disposed;
    }
    #endregion

    #region IApplicationLayerMaster
    public bool Connected
    {
      get; private set;
    } = false;
    public TConnectReqRes ConnectReq(IAddress remoteAddress)
    {
      if (Connected)
        throw new InvalidOperationException($"Operation {nameof(Connected)} is not allowed because the connection has been instantiated alredy");
      if (!(remoteAddress.address is string))
        throw new ArgumentException("Wrong address format type");
      FileInfo _dataFile = new FileInfo((string)remoteAddress.address);
      if (!_dataFile.Exists)
        return TConnectReqRes.NoConnection;
      if (!UpdateData(_dataFile))
        return TConnectReqRes.NoConnection;
      Connected = true;
      return TConnectReqRes.Success;
    }
    public AL_ReadData_Result ReadData(IBlockDescription pBlock, int pStation, out IReadValue pData, byte pRetries)
    {
      pData = null;
      if (pBlock.dataType != 0)
        throw new ArgumentOutOfRangeException($"Only data type = 0 is expected");
      if (!Connected)
        return AL_ReadData_Result.ALRes_DisInd;
      pData = m_LastProcessData;
      return AL_ReadData_Result.ALRes_Success;
    }

    #region Intentionally NotImplemented
    public void DisReq()
    {
      Connected = false;
    }
    public TConnIndRes ConnectInd(IAddress pRemoteAddress, int pTimeOutInMilliseconds)
    {
      throw new NotImplementedException();
    }
    public IWriteValue GetEmptyWriteDataBuffor(IBlockDescription block, int station)
    {
      throw new NotImplementedException();
    }
    public AL_ReadData_Result WriteData(ref IWriteValue data, byte retries)
    {
      throw new NotImplementedException();
    }
    #endregion

    #endregion

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls
    protected virtual void Dispose(bool disposing)
    {
      if (disposedValue)
        return;
      if (disposing)
        DisReq();
      // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
      // TODO: set large fields to null.
      disposedValue = true;
    }
    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    // ~TextReaderApplicationLayerMaster() {
    //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
    //   Dispose(false);
    // }
    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
      // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
      Dispose(true);
      // TODO: uncomment the following line if the finalizer is overridden above.
      // GC.SuppressFinalize(this);
    }
    #endregion

  }

}
