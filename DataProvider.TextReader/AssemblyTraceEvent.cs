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
using System.Reflection;

namespace CAS.CommServer.DataProvider.TextReader
{

  /// <summary>
  /// Singleton implementation of the <see cref="TraceSource"/>.
  /// </summary>
  public class AssemblyTraceEvent : TraceSource, ITraceSource
  {

    private static Lazy<AssemblyTraceEvent> m_TraceEventInternal = new Lazy<AssemblyTraceEvent>(() => new AssemblyTraceEvent(Assembly.GetCallingAssembly().GetName().Name));
    private AssemblyTraceEvent(string name) : base(name) { }

    /// <summary>
    /// Gets the tracer.
    /// </summary>
    /// <value>The tracer.</value>
    public static AssemblyTraceEvent Tracer
    {
      get
      {
        return m_TraceEventInternal.Value;
      }
    }
    /// <summary>
    /// a trace event message to the trace listeners in the System.Diagnostics.TraceSource.Listeners collection using the specified event type and event identifier.
    /// </summary>
    /// <param name="eventType">One of the enumeration values that specifies the event type of the trace data.</param>
    /// <param name="id">A numeric identifier for the event.</param>
    /// <param name="message">The trace message to write.</param>
    /// TODO Edit XML Comment Template for TraceMessage
    public void TraceMessage(TraceEventType eventType, int id, string message)
    {
      Tracer.TraceEvent(eventType, id, message);
    }
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
