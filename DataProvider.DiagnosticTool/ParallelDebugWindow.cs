//<summary>
//  Title   : This <see cref="DebugWindow"/> allows to be opened as not modal (without loosing focus by the parent form).
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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using CAS.DPDiagnostics.Properties;
using CAS.Lib.ControlLibrary;

namespace CAS.DPDiagnostics
{
  /// <summary>
  /// This <see cref="DebugWindow"/> allows to be opened as not modal (without loosing focus by the parent form).
  /// </summary>
  internal class ParallelDebugWindow: DebugWindow
  {
    public ParallelDebugWindow( Icon icon, FormClosingEventHandler eventHandler, int bufferCapacity) :
      base( icon, eventHandler )
    {
      m_bufferCapacity = bufferCapacity;
    }
    /// <summary>
    /// Appends to log a line of text with sinchronisation.
    /// </summary>
    /// <param name="stringLineToBeAppended">The string line to be appended.</param>
    public new void AppendToLogLine( string stringLineToBeAppended )
    {
      if ( m_counter >= m_bufferCapacity )
        return;
      m_counter++;
      lock ( m_Queue )
        if ( m_counter < m_bufferCapacity )
          m_Queue.Enqueue( stringLineToBeAppended );
        else
          m_Queue.Enqueue( Resources.ParallelDebugWindowLogStpped );
    }
    private void m_Worker_ProgressChanged( object sender, ProgressChangedEventArgs e )
    {
      if ( m_ProgressChanged )
        return;
      m_ProgressChanged = true;
      lock ( m_Queue )
      {
        int _limit = 0;
        while ( m_Queue.Count > 0 && _limit < 20 )
        {
          base.AppendToLogLine( m_Queue.Dequeue() );
          _limit++;
        }
      }
      m_ProgressChanged = false;
    }
    private bool m_ProgressChanged = false;
    private void m_Worker_DoWork( object sender, DoWorkEventArgs e )
    {
      try
      {
        while ( true )
        {
          BackgroundWorker _wrkr = (BackgroundWorker)sender;
          lock ( m_Queue )
            _wrkr.ReportProgress( 0 );
          Thread.Sleep( 500 );
          if ( _wrkr.CancellationPending )
            break;
        }
      }
      catch ( Exception ex )
      {
        string _msg = String.Format( Resources.ParallelDebugWindowWorkerExceptionMessage, ex.Message );
        switch ( MessageBox.Show( _msg, "Debug Window", MessageBoxButtons.OKCancel, MessageBoxIcon.Error ) )
        {
          case DialogResult.Abort:
          case DialogResult.Cancel:
          case DialogResult.Ignore:
          case DialogResult.No:
          case DialogResult.None:
          case DialogResult.Retry:
          case DialogResult.Yes:
            this.Close();
            break;
          case DialogResult.OK:
            break;
          default:
            break;
        }
      }
    }
    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Form.Load" /> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected override void OnLoad( EventArgs e )
    {
      base.OnLoad( e );
      m_Worker.WorkerSupportsCancellation = true;
      m_Worker.WorkerReportsProgress = true;
      m_Worker.DoWork += m_Worker_DoWork;
      m_Worker.ProgressChanged += m_Worker_ProgressChanged;
      m_Worker.RunWorkerAsync();
    }
    protected override void OnClosing( CancelEventArgs e )
    {
      m_Worker.CancelAsync();
      base.OnClosing( e );
    }
    private Queue<string> m_Queue = new Queue<string>();
    private BackgroundWorker m_Worker = new BackgroundWorker();
    private int m_counter = 0;
    int m_bufferCapacity = 1000;
  }
}
