//<summary>
//  Title   : MinMaxAvrInTime class
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

using System.Diagnostics;
using CAS.Lib.RTLib.Utils;

namespace CAS.DPDiagnostics.RTLib
{
  /// <summary>
  /// Calculates min, max and avr of value in time [s], i.e. <paramref name="value"/> / time [s]
  /// </summary>
  /// <remarks>To be moved to the CAS.Lib.RTLib.Utils.</remarks>
  public class MinMaxAvrInTime: MinMaxAvr
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MinMaxAvrInTime"/> class.
    /// </summary>
    /// <param name="elements">The elements.</param>
    public MinMaxAvrInTime( ushort elements )
      : base( elements )
    {
      m_Stopwatch.Start();
    }
    /// <summary>
    /// Adds the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    public void AddCounter( long value )
    {
      long _cl = m_Stopwatch.ElapsedMilliseconds - m_PreviousElapsedMilliseconds;
      base.Add = (( value - m_PreviusCounter) * 1000 ) / _cl;
      m_PreviousElapsedMilliseconds = m_Stopwatch.ElapsedMilliseconds;
      m_PreviusCounter = value;
    }

    private Stopwatch m_Stopwatch = new Stopwatch();
    private long m_PreviousElapsedMilliseconds = 0;
    private long m_PreviusCounter = 0;

  }
}
