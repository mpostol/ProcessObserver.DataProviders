//<summary>
//  Title   : Operation Result Description
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.Text;

namespace CAS.DPDiagnostics
{
  internal interface IOperationResult
  {
    /// <summary>
    /// Gets the collection of loged messages.
    /// </summary>
    /// <value>
    /// The logs collection.
    /// </value>
    IEnumerable<string> Log { get; }
    /// <summary>
    /// Gets the number of data bytes transmited or received recently.
    /// </summary>
    /// <value>
    /// The number of bytes.
    /// </value>
    long NumberOfBytes { get; }
    /// <summary>
    /// Gets the total operations execution time in ms.
    /// </summary>
    /// <value>
    /// The operations run time [ms].
    /// </value>
    long OperationsRunTime { get; }
    /// <summary>
    /// Gets the number of operation cycles.
    /// </summary>
    /// <value>
    /// The number of operation cycles.
    /// </value>
    ushort NumberOfOperationCycles { get; }
    /// <summary>
    /// Gets the run time of the current test cycle in ms.
    /// </summary>
    /// <value>
    /// The run time [ms].
    /// </value>
    long RunTime { get; }
  }
}
