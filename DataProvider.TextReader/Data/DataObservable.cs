//_______________________________________________________________
//  Title   : DataObservable
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;

namespace CAS.CommServer.DataProvider.TextReader.Data
{

  /// <summary>
  /// Class DataObservable - captures file watching functionality and provides services as <see cref="IObservable{T}"/>
  /// </summary>
  /// <seealso cref="System.Reactive.ObservableBase{DataEntity}" />
  /// <seealso cref="System.IDisposable" />
  internal class DataObservable : ObservableBase<IDataEntity>, IDisposable
  {

    #region private
    private class FileSystemEventPattern
    {
      public FileSystemEventPattern(EventPattern<FileSystemEventArgs> eventPattern)
      {
        EventPattern = eventPattern;
        TimeStamp = File.GetLastWriteTime(EventPattern.EventArgs.FullPath);
      }
      public EventPattern<FileSystemEventArgs> EventPattern { get; private set; }
      public DateTime TimeStamp { get; private set; }
    }
    private FileSystemWatcher m_FileSystemWatcher;
    private string m_FileFullPath;
    private IObservable<IDataEntity> m_DataEntityObservable = null;
    private ITextReaderProtocolParameters m_Settings;
    private void LogException(Exception exception)
    {
      TraceSource.TraceMessage(TraceEventType.Error, 51, $"Recorded exception thrown on the data processing observable chain {exception}");
    }
    #endregion

    #region ObservableBase
    /// <summary>
    /// Implement this method with the core subscription logic for the observable sequence.
    /// </summary>
    /// <param name="observer">Observer to send notifications to.</param>
    /// <returns>Disposable object representing an observer's subscription to the observable sequence.</returns>
    protected override IDisposable SubscribeCore(IObserver<IDataEntity> observer)
    {
      return m_DataEntityObservable.Subscribe(observer);
    }
    #endregion

    #region API
    /// <summary>
    /// Initializes a new instance of the <see cref="DataObservable" /> class.
    /// </summary>
    /// <param name="filename">The filename to be scanned.</param>
    /// <param name="settings">The application user settings.</param>
    /// <param name="traceSource">The trace source.</param>
    /// <exception cref="System.ArgumentNullException">
    /// settings
    /// or
    /// traceSource
    /// </exception>
    internal DataObservable(string filename, ITextReaderProtocolParameters settings, ITraceSource traceSource)
    {
      if (settings == null)
        throw new ArgumentNullException(nameof(settings));
      if (traceSource == null)
        throw new ArgumentNullException(nameof(traceSource));
      m_Settings = settings;
      TraceSource = traceSource;
      m_FileFullPath = Path.GetFullPath(filename);
      string _Path = Path.GetDirectoryName(m_FileFullPath);
      string _fileName = Path.GetFileName(m_FileFullPath);
      m_FileSystemWatcher = new FileSystemWatcher(_Path, _fileName) { IncludeSubdirectories = false, EnableRaisingEvents = true, NotifyFilter = NotifyFilters.LastWrite };
      m_DataEntityObservable = Observable
        .FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(x => m_FileSystemWatcher.Changed += x, y => m_FileSystemWatcher.Changed -= y)
        .Buffer<EventPattern<FileSystemEventArgs>>(TimeSpan.FromMilliseconds(settings.DelayFileScan))
        .Where<IList<EventPattern<FileSystemEventArgs>>>(_list => _list.Count > 0)
        .Select<IList<EventPattern<FileSystemEventArgs>>, FileSystemEventPattern>(x => new FileSystemEventPattern(x[x.Count - 1]))
        .Delay<FileSystemEventPattern>(TimeSpan.FromMilliseconds(settings.DelayFileScan))
        .Select<FileSystemEventPattern, IDataEntity>(x => DataEntity.ReadFile(x.EventPattern.EventArgs.FullPath, x.TimeStamp, m_Settings.ColumnSeparator))
        .Do<IDataEntity>(data => { }, exception => LogException(exception));
      TraceSource.TraceMessage(TraceEventType.Verbose, 107, $"Succesfully created data obserwer for the file {filename} with parameters {settings}");
    }
    /// <summary>
    /// Gets or sets the trace source <see cref="ITraceSource"/>.
    /// </summary>
    /// <remarks>
    /// By default <see cref="AssemblyTraceEvent.Tracer"/> is used.
    /// </remarks>
    /// <value>The trace source to be used for logging important data.</value>
    internal ITraceSource TraceSource { get; set; } = AssemblyTraceEvent.Tracer;
    #endregion

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls
    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          m_FileSystemWatcher.Dispose();
          TraceSource.TraceMessage( TraceEventType.Information, 130, "Disposing the data observer");
        }
        disposedValue = true;
      }
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
