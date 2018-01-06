//_______________________________________________________________
//  Title   : TextReaderApplicationLayerMaster
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading;

namespace CAS.CommServer.DataProvider.TextReader
{
  /// <summary>
  /// Class TextReaderApplicationLayerMaster - captures all functionality requires for gathering data from a file.
  /// </summary>
  /// <seealso cref="CAS.Lib.CommonBus.ApplicationLayer.IApplicationLayerMaster" />
  public class TextReaderApplicationLayerMaster : IApplicationLayerMaster
  {

    #region private
    private IComponent m_parentComponent;
    private IProtocolParent m_statistic;
    private IDataEntity m_Fifo = null;
    private int m_RetryCount = 0;
    private IDisposable m_Observer = null;
    private DataObservable m_Observable;
    private ITextReaderProtocolParameters m_TextReaderProtocolParameters = null;
    private string m_FileName = string.Empty;
    private void ParentComponent_Disposed(object sender, EventArgs e)
    {
      Dispose();
    }
    private void NotifyNewData(IList<IDataEntity> list)
    {
      IDataEntity _newData = null;
      if (list.Count == 0)
      {
        m_RetryCount++;
        TraceSource.TraceMessage(TraceEventType.Information, 71, $"Incomming data stream has been timed out RetryCount={m_RetryCount}.");
        if (m_TextReaderProtocolParameters.MaxNumberOfRetries > 0 && m_TextReaderProtocolParameters.MaxNumberOfRetries < m_RetryCount)
        {
          TraceSource.TraceMessage(TraceEventType.Information, 182, $"ReadData failed and caused disconnection because the number of reries={m_RetryCount} is greater than the limit {m_TextReaderProtocolParameters.MaxNumberOfRetries}");
          DisReq();
          m_RetryCount = 0;
        }
      }
      else
      {
        _newData = list[list.Count - 1];
        m_RetryCount = 0;
        TraceSource.TraceMessage(TraceEventType.Verbose, 93, $"New data from the file {m_FileName} modified at {_newData.TimeStamp} has been fetched");
      }
      IDataEntity oldData = Interlocked.Exchange<IDataEntity>(ref m_Fifo, _newData);
    }
    private void ExceptionHandler(Exception exception)
    {
      TraceSource.TraceMessage(TraceEventType.Error, 75, $"The data observable chain has thrown an exception. The connection is broken and must be established once more. Details: {exception}");
      Connected = false;
      Interlocked.Exchange<IDataEntity>(ref m_Fifo, null);
      m_Observable.Dispose();
    }
    #endregion

    #region Disgnostic
    /// <summary>
    /// Gets or sets the trace source <see cref="ITraceSource"/>.
    /// </summary>
    /// <remarks>
    /// By default <see cref="AssemblyTraceEvent.Tracer"/> is used.
    /// </remarks>
    /// <value>The trace source to be used for logging important data.</value>
    public ITraceSource TraceSource { get; set; } = AssemblyTraceEvent.Tracer;
    #endregion

    #region constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="TextReaderApplicationLayerMaster"/> class.
    /// </summary>
    /// <param name="statistic">The statistic data describing behavior of this DataProvider at run-time.</param>
    /// <param name="parentComponent">The parent component providing cleanup functionality.</param>
    /// <param name="setting">The current settings for this DataProvider.</param>
    public TextReaderApplicationLayerMaster(IProtocolParent statistic, IComponent parentComponent, ITextReaderProtocolParameters setting)
    {
      m_statistic = statistic;
      m_parentComponent = parentComponent;
      m_TextReaderProtocolParameters = setting;
      m_parentComponent.Disposed += ParentComponent_Disposed;
    }
    #endregion

    #region IApplicationLayerMaster
    /// <summary>
    /// true if the layer is connected for connection oriented communication or ready for communication
    /// for connectionless communication.
    /// </summary>
    /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
    public bool Connected
    {
      get; private set;
    } = false;
    /// <summary>
    /// Connect Request, this function is used for establishing the connection
    /// </summary>
    /// <param name="remoteAddress">address of the remote unit</param>
    /// <returns>Success:
    /// Operation accomplished successfully
    /// NoConnection:
    /// Connection cannot be established.</returns>
    /// <exception cref="System.InvalidOperationException">Connected</exception>
    /// <exception cref="System.ArgumentException">Wrong address format type</exception>
    public TConnectReqRes ConnectReq(IAddress remoteAddress)
    {
      if (Connected)
        throw new InvalidOperationException($"Operation {nameof(Connected)} is not allowed because the connection has been instantiated alredy");
      if (!(remoteAddress.address is string))
        throw new ArgumentException("Wrong address format type");
      if (String.IsNullOrEmpty((string)remoteAddress.address))
        return TConnectReqRes.NoConnection;
      m_Observable = new DataObservable((string)remoteAddress.address, m_TextReaderProtocolParameters, TraceSource);
      m_Observer = m_Observable
        .Buffer<IDataEntity>(TimeSpan.FromMilliseconds(m_TextReaderProtocolParameters.FileModificationNotificationTimeout), 1)
        .Subscribe<IList<IDataEntity>>
       (
        x => NotifyNewData(x),
        exception => ExceptionHandler(exception),
        () => { Connected = false; m_Observable.Dispose(); }
       );
      Connected = true;
      return TConnectReqRes.Success;
    }
    /// <summary>
    /// Reads process data from the selected location and device resources.
    /// </summary>
    /// <param name="pBlock"><see cref="T:CAS.Lib.CommonBus.ApplicationLayer.IBlockDescription" /> selecting the resource containing the data block to be read.</param>
    /// <param name="pStation">Address of the remote station connected to the common field bus. –1 if not applicable.</param>
    /// <param name="pData">The buffer <see cref="T:CAS.Lib.CommonBus.ApplicationLayer.IReadValue" /> containing the requested data.</param>
    /// <param name="pRetries">Number of retries to get data.</param>
    /// <returns>Result of the operation</returns>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public AL_ReadData_Result ReadData(IBlockDescription pBlock, int pStation, out IReadValue pData, byte pRetries)
    {
      pData = null;
      if (pBlock.dataType != 0)
      {
        TraceSource.TraceMessage(TraceEventType.Error, 149, $"Wrong dataType: {pBlock.dataType}; only data type = 0 is expected");
        m_statistic.IncStRxInvalid();
      }
      if (!Connected)
      {
        TraceSource.TraceMessage(TraceEventType.Verbose, 165, $"ReadData failed because it is not connectedt; reries/limit={m_RetryCount}/{this.m_TextReaderProtocolParameters.MaxNumberOfRetries}.");
        return AL_ReadData_Result.ALRes_DisInd;
      }
      m_statistic.IncStTxFrameCounter();
      IDataEntity _copy = Interlocked.Exchange<IDataEntity>(ref m_Fifo, m_Fifo);
      bool _retResult = _copy != null;
      m_statistic.RxDataBlock(_retResult);
      if (!_retResult)
      {
        m_statistic.IncStRxNoResponseCounter();
        TraceSource.TraceMessage(TraceEventType.Information, 186, $"ReadData failed; reries/limit={m_RetryCount}/{this.m_TextReaderProtocolParameters.MaxNumberOfRetries}.");
        return AL_ReadData_Result.ALRes_DatTransferErrr;
      }
      m_statistic.IncStRxFrameCounter();
      pData = new ReadDataEntity(_copy, pBlock);
      TraceSource.TraceMessage(TraceEventType.Verbose, 191, $"ReadData succeded for [{pStation}/{pBlock.startAddress}]");
      return AL_ReadData_Result.ALRes_Success;
    }
    /// <summary>
    /// Disconnect Request - Unconditionally disconnect the connection if any.
    /// </summary>
    public void DisReq()
    {
      m_Observer?.Dispose();
      m_Observable?.Dispose();
      Connected = false;
    }

    #region Intentionally NotImplemented
    /// <summary>
    /// Connect indication – Check if there is a connection accepted to the remote address.
    /// </summary>
    /// <param name="pRemoteAddress">The remote address we are waiting for connection from. Null if we are waiting for any connection.</param>
    /// <param name="pTimeOutInMilliseconds">How long the client is willing to wait for an incoming connection in ms.</param>
    /// <returns>ConnectInd:
    /// Connection initiated by a remote unit has been established.
    /// NoConnection:
    /// There is no incoming connection awaiting.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// TODO Edit XML Comment Template for ConnectInd
    public TConnIndRes ConnectInd(IAddress pRemoteAddress, int pTimeOutInMilliseconds)
    {
      throw new NotImplementedException();
    }
    /// <summary>
    /// Gets a buffer from a pool and initiates. After filling it up with the data can be send to the data provider remote
    /// unit by the <see cref="M:CAS.Lib.CommonBus.ApplicationLayer.IApplicationLayerMaster.WriteData(CAS.Lib.CommonBus.ApplicationLayer.IWriteValue@,System.Byte)" />.
    /// </summary>
    /// <param name="block"><see cref="T:CAS.Lib.CommonBus.ApplicationLayer.IBlockDescription" /> selecting the resource where the data block is to be written.</param>
    /// <param name="station">Address of the remote station connected to the common field bus. –1 if not applicable.</param>
    /// <returns>A buffer <see cref="T:CAS.Lib.CommonBus.ApplicationLayer.IWriteValue" /> ready to be filled up with the data and written down by the <see cref="M:CAS.Lib.CommonBus.ApplicationLayer.IApplicationLayerMaster.WriteData(CAS.Lib.CommonBus.ApplicationLayer.IWriteValue@,System.Byte)" />
    /// to the destination – remote station.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public IWriteValue GetEmptyWriteDataBuffor(IBlockDescription block, int station)
    {
      throw new NotImplementedException();
    }
    /// <summary>
    /// Writes process data down to the selected location and device resources.
    /// </summary>
    /// <param name="data">Data to be send. Always null after return. Data buffer must be returned to the pool.</param>
    /// <param name="retries">Number of retries to write data.</param>
    /// <returns><see cref="T:CAS.Lib.CommonBus.ApplicationLayer.AL_ReadData_Result" /> result of the operation.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// TODO Edit XML Comment Template for WriteData
    public AL_ReadData_Result WriteData(ref IWriteValue data, byte retries)
    {
      throw new NotImplementedException();
    }
    #endregion

    #endregion

    #region IDisposable Support
    private bool m_DisposedValue = false; // To detect redundant calls
    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (m_DisposedValue)
        return;
      if (disposing)
      {
        m_Observer?.Dispose();
        m_Observable?.Dispose();
        m_Observer = null;
        m_Observable = null;
      }
      m_DisposedValue = true;
    }
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
      Dispose(true);
    }
    #endregion

  }

}
