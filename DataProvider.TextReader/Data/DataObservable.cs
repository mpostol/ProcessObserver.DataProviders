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
  internal class DataObservable : ObservableBase<DataEntity>, IDisposable
  {

    #region private
    private class DateTimeEqualityComparer : IEqualityComparer<FileSystemEventPattern>
    {
      #region IEqualityComparer
      public bool Equals(FileSystemEventPattern x, FileSystemEventPattern y)
      {
        return (x.TimeStamp.Date == y.TimeStamp.Date) && (x.TimeStamp.Hour == y.TimeStamp.Hour) && (x.TimeStamp.Minute == y.TimeStamp.Minute) && (x.TimeStamp.Second == y.TimeStamp.Second);
      }
      public int GetHashCode(FileSystemEventPattern obj)
      {
        return obj.GetHashCode();
      }
      #endregion
    }
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
    private IObservable<DataEntity> m_DataEntityObservable = null;
    private void LogData(DataEntity data)
    {
      DateTime _now = DateTime.Now;
      TraceSource.TraceMessage(TraceEventType.Verbose, 66, $"Recorded data modification at: {_now.ToLongTimeString()}.{_now.Millisecond} from file {m_FileFullPath} modified at: {data.TimeStamp}");
    }
    private void LogException(Exception exception)
    {
      DateTime _now = DateTime.Now;
      TraceSource.TraceMessage(TraceEventType.Error, 71, $"Recorded exception at: {_now.ToLongTimeString()}.{_now.Millisecond} message {exception}");
    }
    private DataEntity ParseText(EventPattern<FileSystemEventArgs> eventArgs)
    {
      DataEntity _ret = null;
      string[] _content = File.ReadAllLines(eventArgs.EventArgs.FullPath);
      int _line2Read = Int32.Parse(_content[0].Trim());
      _ret = new DataEntity() { TimeStamp = File.GetLastWriteTime(eventArgs.EventArgs.FullPath), Tags = _content[_line2Read].Split(new string[] { m_Settings.ColumnSeparator }, StringSplitOptions.None) };
      return _ret;
    }
    #endregion

    #region ObservableBase
    protected override IDisposable SubscribeCore(IObserver<DataEntity> observer)
    {
      return m_DataEntityObservable.Subscribe(observer);
    }
    #endregion

    #region API
    /// <summary>
    /// Initializes a new instance of the <see cref="DataObservable" /> class.
    /// </summary>
    /// <param name="filename">The filename to be scanned.</param>
    /// <param name="dueTime">The duetime - applies a timeout policy for file modification notification. If the next file modification notification isn't received within the specified timeout duration starting from
    /// its predecessor, a <see cref="TimeoutException"/> is propagated to the observer.</param>
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
        .Select<EventPattern<FileSystemEventArgs>, FileSystemEventPattern>(x => new FileSystemEventPattern(x))
        .DistinctUntilChanged<FileSystemEventPattern>(new DateTimeEqualityComparer())
        .Delay<FileSystemEventPattern>(TimeSpan.FromMilliseconds(settings.DelayFileScann))
        .Select<FileSystemEventPattern, DataEntity>(x => ParseText(x.EventPattern))
        .Timeout<DataEntity>(TimeSpan.FromMilliseconds(settings.FileModificationNotificationTimeout))
        .Do<DataEntity>(data => LogData(data), exception => LogException(exception));
      TraceSource.TraceMessage(TraceEventType.Verbose, 107, $"Succesfully created obserwer for the file {filename} with parameter {settings}");
    }
    /// <summary>
    /// Gets or sets the trace source.
    /// </summary>
    /// <value>The trace source to be used for logging important data.</value>
    public ITraceSource TraceSource { get; set; } = AssemblyTraceEvent.Tracer;
    #endregion

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls
    private ITextReaderProtocolParameters m_Settings;
    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    /// TODO Edit XML Comment Template for Dispose
    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
          m_FileSystemWatcher.Dispose();
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
      // TODO: uncomment the following line if the finalizer is overridden above.
      // GC.SuppressFinalize(this);
    }
    #endregion

  }

}
