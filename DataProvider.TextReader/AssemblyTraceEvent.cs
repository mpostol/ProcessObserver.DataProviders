//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using System;
using System.Diagnostics;

namespace CAS.CommServer.DataProvider.TextReader
{
  /// <summary>
  /// Singleton implementation of the <see cref="TraceSource"/>.
  /// </summary>
  public class AssemblyTraceEvent : ITraceSource
  {
    private Lazy<TraceSource> m_TraceEventInternal = new Lazy<TraceSource>(() => new TraceSource(Properties.Settings.Default.TraceSourceName));

    private AssemblyTraceEvent()
    {
    }

    /// <summary>
    /// Gets the tracer.
    /// </summary>
    /// <value>The tracer.</value>
    public static AssemblyTraceEvent Tracer { get; } = new AssemblyTraceEvent();

    internal string Name => m_TraceEventInternal.Value.Name;

    /// <summary>
    /// a trace event message to the trace listeners in the System.Diagnostics.TraceSource.Listeners collection using the specified event type and event identifier.
    /// </summary>
    /// <param name="eventType">One of the enumeration values that specifies the event type of the trace data.</param>
    /// <param name="id">A numeric identifier for the event.</param>
    /// <param name="message">The trace message to write.</param>
    public void TraceMessage(TraceEventType eventType, int id, string message)
    {
      m_TraceEventInternal.Value.TraceEvent(eventType, id, message);
    }

    /// <summary>
    /// Gets the wrapped instance of <see cref="TraceSource"/>, which provides a set of methods and properties that enable applications to trace the
    /// execution of code and associate trace messages with their source.
    /// </summary>
    /// <value>the wrapped instance of <see cref="TraceSource"/>.</value>
    internal TraceSource TraceSource => m_TraceEventInternal.Value;
  }

}