//_______________________________________________________________
//  Title   : Name of Application
//  System  : Microsoft VisualStudio 2015 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2017, CAS LODZ POLAND.
//  TEL: +48 608 61 98 99 
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using CAS.CommServer.DataProvider.TextReader.Data;
using CAS.Lib.CommonBus;
using CAS.Lib.CommonBus.ApplicationLayer;
using CAS.Lib.RTLib.Management;
using CAS.Lib.RTLib.Processes;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive.Linq;

namespace CAS.CommServer.DataProvider.TextReader
{
  public class TextReaderApplicationLayerMaster : IApplicationLayerMaster
  {

    private IComponent m_parentComponent;
    private IProtocolParent m_statistic;
    private EventsSynchronization m_Fifo = new EventsSynchronization();
    private IDisposable m_Observer = null;
    private DataObservable m_Observable;
    private ITextReaderProtocolParameters m_TextReaderProtocolParameters = null;
    private void ParentComponent_Disposed(object sender, EventArgs e)
    {
      Dispose();
    }
    /// <summary>
    /// Gets or sets the trace source.
    /// </summary>
    /// <value>The trace source to be used for logging important data.</value>
    public ITraceSource TraceSource { get; set; } = AssemblyTraceEvent.Tracer;

    #region constructors
    public TextReaderApplicationLayerMaster(IProtocolParent statistic, IComponent parentComponent, ITextReaderProtocolParameters setting)
    {
      m_statistic = statistic;
      m_parentComponent = parentComponent;
      m_TextReaderProtocolParameters = setting;
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
      if (String.IsNullOrEmpty((string)remoteAddress.address))
        return TConnectReqRes.NoConnection;
      m_Observable = new Data.DataObservable((string)remoteAddress.address, m_TextReaderProtocolParameters, AssemblyTraceEvent.Tracer);
      m_Observer = m_Observable.Subscribe<DataEntity>
       (
        x => m_Fifo.SetEvent(x),
        exception => { TraceSource.TraceMessage(TraceEventType.Error, 75, $"ConnectReq - an exception has been caught: {exception}"); Connected = false; m_Observable.Dispose(); },
        () => { Connected = false; m_Observable.Dispose(); }
       );
      Connected = true;
      return TConnectReqRes.Success;
    }
    public AL_ReadData_Result ReadData(IBlockDescription pBlock, int pStation, out IReadValue pData, byte pRetries)
    {
      System.Diagnostics.Stopwatch _responseTime = new System.Diagnostics.Stopwatch();
      _responseTime.Start();
      pData = null;
      if (pBlock.dataType != 0)
        throw new ArgumentOutOfRangeException($"Only data type = 0 is expected");
      if (!Connected)
        return AL_ReadData_Result.ALRes_DisInd;
      m_statistic.IncStTxFrameCounter();
      object _retValue = null;
      bool _retResult = m_Fifo.GetEvent(out _retValue, Convert.ToInt32(m_TextReaderProtocolParameters.FileModificationNotificationTimeout));
      m_statistic.RxDataBlock(_retResult);
      _responseTime.Stop();
      m_statistic.TimeMaxResponseDelayAdd(_responseTime.ElapsedMilliseconds);
      if (!_retResult)
      {
        m_statistic.IncStRxNoResponseCounter();
        return AL_ReadData_Result.ALRes_DatTransferErrr;
      }
      m_statistic.IncStRxFrameCounter();
      pData = (IReadValue)_retValue;
      return AL_ReadData_Result.ALRes_Success;
    }
    public void DisReq()
    {
      m_Observer?.Dispose();
      Connected = false;
    }

    #region Intentionally NotImplemented
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
    private bool m_DisposedValue = false; // To detect redundant calls
    protected virtual void Dispose(bool disposing)
    {
      if (m_DisposedValue)
        return;
      if (disposing)
        DisReq();
      // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
      // TODO: set large fields to null.
      m_DisposedValue = true;
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
