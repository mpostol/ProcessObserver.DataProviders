//_______________________________________________________________
//  Title   : AssemblyTraceEvent
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
using System.Diagnostics;

namespace CAS.CommServer.DataProvider.TextReader
{

  /// <summary>
  /// Singleton implementation of the <see cref="TraceSource"/>.
  /// </summary>
  public class AssemblyTraceEvent : ITraceSource
  {

    private static AssemblyTraceEvent m_singleton = new AssemblyTraceEvent();
    private Lazy<TraceSource> m_TraceEventInternal = new Lazy<TraceSource>(() => CAS.Lib.CommonBus.AssemblyTraceEvent.Tracer);
    private AssemblyTraceEvent() { }

    /// <summary>
    /// Gets the tracer.
    /// </summary>
    /// <value>The tracer.</value>
    public static AssemblyTraceEvent Tracer
    {
      get
      {
        return m_singleton;
      }
    }
    internal string Name { get { return m_TraceEventInternal.Value.Name; } }
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
    internal TraceSource TraceSource { get { return m_TraceEventInternal.Value; } }

  }
  /// <summary>
  /// Interface ITraceSource - declares basic functionality for the component behavior tracing.
  /// </summary>
  public interface ITraceSource
  {
    /// <summary>
    /// a trace event message to the trace listeners in the System.Diagnostics.TraceSource.Listeners collection using the specified event type and event identifier.
    /// </summary>
    /// <param name="eventType">One of the enumeration values that specifies the event type of the trace data.</param>
    /// <param name="id">A numeric identifier for the event.</param>
    /// <param name="message"> The trace message to write.</param>
    /// <exception cref="T:System.ObjectDisposedException">An attempt was made to trace an event during finalization.</exception>
    void TraceMessage(TraceEventType eventType, int id, string message);

  }

}
