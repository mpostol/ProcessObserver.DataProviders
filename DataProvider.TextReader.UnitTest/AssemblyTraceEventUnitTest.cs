//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CAS.CommServer.DataProvider.TextReader.UnitTest
{
  [TestClass]
  [DeploymentItem(@"log\", "log")]
  public class AssemblyTraceEventUnitTest
  {
    [TestMethod]
    public void TracerTestMethod()
    {
      AssemblyTraceEvent _tracerWrapper = AssemblyTraceEvent.Tracer;
      Assert.IsNotNull(_tracerWrapper);
      Assert.AreEqual<string>("DataProvider.TextReader", _tracerWrapper.Name);

      TraceSource _tracer = _tracerWrapper.TraceSource;
      Assert.IsNotNull(_tracer);
      Assert.AreEqual(2, _tracer.Listeners.Count, $"Available listeners: {string.Join(", ", _tracer.Listeners.Cast<TraceListener>().Select<TraceListener, string>(x => x.Name).ToArray<String>())}");
      Dictionary<string, TraceListener> _listeners = _tracer.Listeners.Cast<TraceListener>().ToDictionary<TraceListener, string>(x => x.Name);
      Assert.IsTrue(_listeners.ContainsKey("LogFile"));
      TraceListener _listener = _listeners["LogFile"];
      Assert.IsNotNull(_listener);
      Assert.IsInstanceOfType(_listener, typeof(DelimitedListTraceListener));
      DelimitedListTraceListener _advancedListener = _listener as DelimitedListTraceListener;
      Assert.IsNotNull(_advancedListener.Filter);
      Assert.IsInstanceOfType(_advancedListener.Filter, typeof(EventTypeFilter));
      EventTypeFilter _eventTypeFilter = _advancedListener.Filter as EventTypeFilter;
      Assert.AreEqual(SourceLevels.All, _eventTypeFilter.EventType);
      string _testPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      Assert.AreEqual<string>(Path.Combine(_testPath, @"log\CAS.CommServer.ProtocolHub.Communication.log"), GetFileName(_advancedListener));
    }

    [TestMethod]
    public void TraceMessageTestMethod()
    {
      //preparation
      TraceSource _tracer = AssemblyTraceEvent.Tracer.TraceSource;
      TraceListener _listener = _tracer.Listeners.Cast<TraceListener>().Where<TraceListener>(x => x.Name == "LogFile").First<TraceListener>();
      Assert.IsNotNull(_listener);
      DelimitedListTraceListener _advancedListener = _listener as DelimitedListTraceListener;
      Assert.IsNotNull(_advancedListener);
      Assert.IsFalse(string.IsNullOrEmpty(GetFileName(_advancedListener)));
      FileInfo _logFileInfo = new FileInfo(GetFileName(_advancedListener));
      long _length = _logFileInfo.Exists ? _logFileInfo.Length : 0;
      //trace to log file
      AssemblyTraceEvent.Tracer.TraceMessage(TraceEventType.Verbose, 0, "Trace Message for TraceMessageTestMethod");
      Assert.IsFalse(string.IsNullOrEmpty(GetFileName(_advancedListener)));
      _logFileInfo.Refresh();
      Assert.IsTrue(_logFileInfo.Exists, $"{_logFileInfo.FullName} doesn't exist");
      Assert.IsTrue(_logFileInfo.Length > _length + 10, $"The final file length = {_logFileInfo.Length} must be > {_length} + 10");
    }

    private static string GetFileName(DelimitedListTraceListener listener)
    {
      if (listener == null)
        throw new ArgumentNullException(nameof(listener));
      FieldInfo fi = typeof(TextWriterTraceListener).GetField("fileName", BindingFlags.NonPublic | BindingFlags.Instance);
      if (fi == null)
        throw new NullReferenceException("Cannot create FieldInfo object");
      return (string)fi.GetValue(listener);
    }

    private const string LogFileName = @"log\CAS.CommServer.ProtocolHub.Communication.log";
  }
}