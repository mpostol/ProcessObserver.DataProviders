//<summary>
//  Title   : XBUS Measurement main window
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//  20081102: mzbrzezny: Debug Window and menu settings are added 
//  20081006: mzbrzezny: AddressSpaceDescriptor and Item Default Settings are implemented.
//  20080829: mzbrzezny: XBUS measurement - improvement:
//            - new statistics from synchro error counter
//            - write operation can work for other than SBUS protocol
//               NOTE: in previous version of XBUS measurement (up to: svn rev 1772 2008-08-26 12:08:33Z mpostol)
//               the write methode was writing value as short (if datatype=10) and true or false in else case
//               this was not valied for other than SBUS protocols (and ealier version of MODBUS).
//              now the DataProvider is responsible for conversion
//            - small changes to main window
//            - new logo (from CommServer, not CAS)
//            - some cleanup
//            - read/write checkbox can enable/disable some other options
//  20080617: mzbrzezny: some improvement: new comboboxes: datatypes, communication layer address
//  Wptasinski: 11-05-2004
//	New components added: bytes sent over channel (text field based on getBytesTransferred method from commBase),
//	AvChannelUsageValue - calculations based on getAverageChannelUse() method, AvChannelUsage - progress bar, 
//	portsp - port speed value chosen in port settings is displayed
//	getAverageChannelUsage() method was added which calculates the average channel usage
//  Mariusz Postol - 2003: created
//
//  Copyright (C)2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.CommServer.CommonBus;
using CAS.DPDiagnostics.Properties;
using CAS.Lib.CodeProtect.LicenseDsc;
using CAS.Lib.CommonBus;
using CAS.Lib.CommonBus.Components;
using CAS.Lib.ControlLibrary;
using CAS.Lib.RTLib.Management;
using CAS.Windows.Forms.CodeProtectControls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace CAS.DPDiagnostics
{
  /// <summary>
  /// Main program form
  /// </summary>
  public partial class Program: Form
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Program"/> class.
    /// </summary>
    public Program()
    {
      InitializeComponent();
    }

    #region private

    private class LocalOperation: Operation
    {

      #region public
      internal LocalOperation( Program parent, IDataProviderID dataProviderID, CommonBusControl commonBusControl, IProtocolParent protocolParent, bool readOperation )
      {
        Parent = parent;
        Delay = System.Convert.ToInt32( parent.@_Delay.Text );
        InitializeOperation( dataProviderID, commonBusControl, protocolParent, readOperation );
      }
      internal string CommLayerAddress { get { return m_CommLayerAddress.ToString(); } }
      internal int Delay { get; private set; }
      #endregion

      #region private
      private readonly Program Parent = null;
      protected override short ResourceSelected
      {
        get { return Parent.ResourceSelected(); }
      }
      protected override int StationAddress
      {
        get { return Convert.ToInt32( Parent.m_StationAddressNumericUpDown.Value ); }
      }
      protected override Type DataTypeOfConversion
      {
        get
        {
          object _SelectedItem = Parent.@_DataTypeOfConversionComboBox.SelectedItem;
          Type _ret = typeof( object );
          if ( _SelectedItem != null && _SelectedItem as Type != null )
            _ret = _SelectedItem as Type;
          return _ret;
        }
      }
      protected override int Length
      {
        get { return Convert.ToInt32( Parent.@_DataBlockLengthNumericUpDown.Value ); }
      }
      protected override IAddress Commlayeraddress
      {
        get
        {
          return new StringAddress( Parent.@_ComlayerAddressComboBox.Text.Trim() );
        }
      }
      protected override int RegisterStartAddress
      {
        get
        {
          return Convert.ToInt32( Parent.@_RegisterAddressNumericUpDown.Value );
        }
      }
      protected override object ValueToBeWritten( Type dataTypeOfConversion )
      {
        object _ret = Parent.@_Value2BeWritten.Text;
        string _val = Parent.@_Value2BeWritten.Text;
        if ( dataTypeOfConversion == null )
          throw new InvalidCastException( "The conversion type is not selected - select it from available types." );
        TypeCode _code = Type.GetTypeCode( dataTypeOfConversion );
        switch ( _code )
        {
          case TypeCode.String:
            break;
          case TypeCode.Boolean:
            _ret = GetMessage<Boolean>( _val, Boolean.TryParse );
            break;
          case TypeCode.Byte:
            _ret = GetMessage<Byte>( _val, Byte.TryParse );
            break;
          case TypeCode.Decimal:
            _ret = GetMessage<Decimal>( _val, Decimal.TryParse );
            break;
          case TypeCode.Double:
            _ret = GetMessage<Double>( _val, Double.TryParse );
            break;
          case TypeCode.Int16:
            _ret = GetMessage<Int16>( _val, Int16.TryParse );
            break;
          case TypeCode.Int32:
            _ret = GetMessage<Int32>( _val, Int32.TryParse );
            break;
          case TypeCode.Int64:
            _ret = GetMessage<Int64>( _val, Int64.TryParse );
            break;
          case TypeCode.SByte:
            _ret = GetMessage<SByte>( _val, SByte.TryParse );
            break;
          case TypeCode.Single:
            _ret = GetMessage<Single>( _val, Single.TryParse );
            break;
          case TypeCode.UInt16:
            _ret = GetMessage<UInt16>( _val, UInt16.TryParse );
            break;
          case TypeCode.UInt32:
            _ret = GetMessage<UInt32>( _val, UInt32.TryParse );
            break;
          case TypeCode.UInt64:
            _ret = GetMessage<UInt64>( _val, UInt64.TryParse );
            break;
          case TypeCode.DateTime:
          case TypeCode.Empty:
          case TypeCode.Char:
          case TypeCode.DBNull:
          case TypeCode.Object:
            throw new NotImplementedException( "Conversion to object is not supported" );
          default:
            break;
        }
        return _ret; // Convert.ChangeType( Parent.m_Value2BeWrittenNumericUpDown.Value, dataTypeOfConversion );
      }
      private delegate bool Convert<t>( string value, out t result );
      private static tp GetMessage<tp>( string value, Convert<tp> converter )
      {
        tp _ret = default( tp );
        if ( !converter( value, out _ret ) )
        {
          string _msgTemplate = "Cannot convert {0} to {1} - review the text in the TextBox.";
          throw new InvalidCastException( String.Format( _msgTemplate, value, typeof( tp ) ) );
        }
        return _ret;
      }
      #endregion

    }
    //TODO to be implemented.
    private uint GetAverageChannelUse()
    {
      uint avChUse = 0;
      //uint time = Processes.Stopwatch.ConvertTo_s( testTimeStopWatch.Read );
      //if ( time > 0 )
      //  avChUse = ( (uint)m_Statistic.GetRxBytesTransferred * 1100 / (uint)m_SerialSet.BaudRate ) / time;
      //else
      //  avChUse = 0;
      if ( avChUse > 100 )
        avChUse = 100;
      return avChUse;
    }
    private void DisplayResults( IOperationResult result )
    {
      try
      {
        this.@_RXFramesCount.Text = m_Statistic.GetStRxFrameCounter.ToString();
        this.@_TXFramesCount.Text = m_Statistic.GetStTxFrameCounter.ToString();
        this.@_CRCErrorsCnt.Text = m_Statistic.GetStRxCRCErrorCounter.ToString();
        this.@_IncompleteCnt.Text = m_Statistic.GetStRxFragmentedCounter.ToString();
        this.@_TimeOutCount.Text = m_Statistic.GetStRxNoResponseCounter.ToString();
        this.@_NAKCount.Text = m_Statistic.GetStRxNAKCounter.ToString();
        this.@_MaxResponseTime.Text = m_Statistic.GetTimeMaxResponseDelay;
        this.@_MaxCharGapTime.Text = m_Statistic.GetTimeCharGap;
        this.@_stat_synchr_error_counter.Text = m_Statistic.GetStRxSynchError.ToString();
        this.@_TestTime.Text = ( result.RunTime / 1000 ).ToString();
        this._UserDataTransfered.Text = result.NumberOfBytes.ToString();
        m_Throughput.AddCounter( result.NumberOfBytes );
        this._UserDataThroughput.Text = m_Throughput.ToString();
        int _usage = Convert.ToInt32( result.OperationsRunTime * 100 / ( result.RunTime - m_PreviusRunTime ) );
        m_PreviusRunTime = result.RunTime;
        this.@_AverageChannelUsageProgres.Value = _usage;
        this.@_AverageChannelUsage.Text = _usage.ToString();
        if ( m_DebugInProgress )
          foreach ( string _msg in result.Log )
            m_DebugWindow.AppendToLogLine( _msg );
      }
      catch ( Exception ex )
      {
        string _msg = String.Format( Resources.ProgramWorkerExceptionMessage, ex.Message );
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
        throw;
      }
    }
    private void ResetCounters()
    {
      m_Statistic.ResetStatistics();
      m_PreviusRunTime = 0;
      m_Throughput = new RTLib.MinMaxAvrInTime( 5 );
      this.@_RXFramesCount.Text = String.Empty;
      this.@_TXFramesCount.Text = String.Empty;
      this.@_CRCErrorsCnt.Text = String.Empty;
      this.@_IncompleteCnt.Text = String.Empty;
      this.@_TimeOutCount.Text = String.Empty;
      this.@_NAKCount.Text = String.Empty;
      this.@_MaxResponseTime.Text = String.Empty;
      this.@_MaxCharGapTime.Text = String.Empty;
      this.@_stat_synchr_error_counter.Text = String.Empty;
      this.@_TestTime.Text = String.Empty;
      this._UserDataThroughput.Text = String.Empty;
      this._UserDataTransfered.Text = string.Empty;
      this.@_AverageChannelUsage.Text = string.Empty;
      this.@_AverageChannelUsageProgres.Value = this.@_AverageChannelUsageProgres.Minimum;
    }

    #region backgroundWorker
    private void cm_BbackgroundWorker_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
    {
      @_StartMonitoringButton.Enabled = true;
      @_OpenPortButton.Enabled = true;
      @_StopMonitoringButton.Enabled = false;
      if ( e.Cancelled )
        return;
      if ( e.Result is Operation.ConnectReqResException )
        MessageBox.Show( ( (Operation.ConnectReqResException)e.Result ).Message, "Connection request", MessageBoxButtons.OK, MessageBoxIcon.Stop );
      else
      {
        Debug.Assert( e.Result is Exception, "There must be exception returned if canceled." );
        string _msg = String.Format( "Communication process failed to continue because: {0}", ( (Exception)e.Result ).Message );
        MessageBox.Show( _msg, "Communication process failed", MessageBoxButtons.OK, MessageBoxIcon.Stop );
      }
    }
    private void cm_BbackgroundWorker_ProgressChanged( object sender, ProgressChangedEventArgs e )
    {
      DisplayResults( (IOperationResult)e.UserState );
    }
    private void cm_BbackgroundWorker_DoWork( object sender, DoWorkEventArgs eventArgs )
    {
      BackgroundWorker _worker = sender as BackgroundWorker;
      Debug.Assert( _worker != null );
      Debug.Assert( eventArgs.Argument is LocalOperation );
      LocalOperation _requestedOperation = (LocalOperation)eventArgs.Argument;
      try
      {
        // Get the BackgroundWorker that raised this event.
        Stopwatch _StopWatch = new System.Diagnostics.Stopwatch();
        _requestedOperation.ConnectReq();
        _StopWatch.Start();
        while ( !_worker.CancellationPending )
        {
          _requestedOperation.DoOperation();
          if ( _requestedOperation.Delay > 20 )
            System.Threading.Thread.Sleep( _requestedOperation.Delay );
          if ( _StopWatch.ElapsedMilliseconds > 1000 )
          {
            _worker.ReportProgress( 0, _requestedOperation.GetOperationsResult() );
            _StopWatch.Reset();
            _StopWatch.Start();
          }
        }
        _requestedOperation.DisReq();
        eventArgs.Cancel = true;
        eventArgs.Result = null;
      }
      catch ( Exception ex )
      {
        eventArgs.Result = ex;
        eventArgs.Cancel = false;
        _requestedOperation.DisReq();
      }
    }
    #endregion

    #region Event handlers

    #region Buttons
    private void m_StopMonitoringButton_Click( object sender, System.EventArgs e )
    {
      m_BbackgroundWorker.CancelAsync();
    }
    private void m_StartMonitoringButton_Click( object sender, System.EventArgs e )
    {
      try
      {
        ResetCounters();
        LocalOperation _localOperation = new LocalOperation( this, m_DataProviderID, @_CommonBusControl, m_ProtocolParent, @_ReadRadioButton.Checked );
        m_BbackgroundWorker.RunWorkerAsync( _localOperation );
        @_StartMonitoringButton.Enabled = false;
        @_OpenPortButton.Enabled = false;
        @_StopMonitoringButton.Enabled = true;
      }
      catch ( Exception ex )
      {
        string _msg = String.Format( "Operation failed because of the exception: {0}", ex.Message );
        MessageBox.Show( _msg, "StartMonitoring Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
      }
    }
    private void m_OpenPortButton_Click( object sender, EventArgs e )
    {
      using ( OKCancelForm okcan = new OKCancelForm( "Data Provider Selector" ) )
      {
        AvailableDPTree c_AvailableDPTree;
        if ( m_LastGuid.Equals( Guid.Empty ) )
          c_AvailableDPTree = new AvailableDPTree( @_CommonBusControl, okcan );
        else
          c_AvailableDPTree = new AvailableDPTree( @_CommonBusControl, okcan, m_LastSettings, m_LastGuid );
        using ( c_AvailableDPTree )
        {
          okcan.SetUserControl = c_AvailableDPTree;
          okcan.ShowDialog();
          if ( okcan.DialogResult != DialogResult.OK )
            return;
          m_DataProviderID = c_AvailableDPTree.GetSelectedDPID;
        }
      }
      using ( AddObject<IDataProviderID> _DPSettings = new AddObject<IDataProviderID>() )
      {
        _DPSettings.Object = m_DataProviderID;
        _DPSettings.ShowDialog();
        if ( _DPSettings.DialogResult != DialogResult.OK )
          return;
        m_LastGuid = m_DataProviderID.GetDataProviderDescription.Identifier;
        m_LastSettings = m_DataProviderID.GetSettings();
      }
      this.@_PortSettings.Text = "Protocol settings:\r\n" + m_DataProviderID.GetSettingsHumanReadableFormat();
      @_StartMonitoringButton.Enabled = true;
      @_ResourceComboBox.Items.Clear();
      IAddressSpaceDescriptor[] AvailiableAddressspaces = m_DataProviderID.GetAvailiableAddressspaces();
      foreach ( IAddressSpaceDescriptor item in AvailiableAddressspaces )
        @_ResourceComboBox.Items.Add( item.Name );
      if ( @_ResourceComboBox.Items.Count > 0 )
        @_ResourceComboBox.SelectedIndex = 0;
    }
    private void m_Exitbutton_Click( object sender, EventArgs e )
    {
      this.Close();
    }
    #endregion

    private void Program_Load( object sender, EventArgs e )
    {
      //initialisation of addresses
#if DEBUG
      this.@_ComlayerAddressComboBox.Items.AddRange( new object[] 
      {
           "PCD3:5050",
           "192.168.0.157:5555", "192.168.0.8:2101","127.0.0.1:502","localhost:5050",
           "83.16.57.118:10001","83.16.57.118:10002",
           "[test.xls]Sheet3"
       } );
#endif
      m_Statistic = Protocol.GetProtocolStatistics( 0 );
    }
    private void exitToolStripMenuItem_Click( object sender, EventArgs e )
    {
      this.Close();
    }
    private void debugWindow_FormClosing( object sender, FormClosingEventArgs e )
    {
      @_DebugWindowToolStripMenuItem.Checked = false;
    }
    private void Program_FormClosing( object sender, FormClosingEventArgs e )
    {
      string _msg = "You are about to close the application. Press OK to continue ?";
      DialogResult _rst = MessageBox.Show( _msg, "Window closing.", MessageBoxButtons.OKCancel, MessageBoxIcon.Question );
      if ( _rst != DialogResult.OK )
        e.Cancel = true;
    }
    private void comboBox_datatype_SelectedIndexChanged( object sender, EventArgs e )
    {
      Int16 m_ResourceSelected = ResourceSelected();
      @_DataTypeOfConversionComboBox.Items.Clear();
      @_DataTypeOfConversionComboBox.Text = "(not selected)";
      @_DataTypeOfConversionComboBox.SelectedItem = null;
      Type[] types = m_DataProviderID.GetItemDefaultSettings( m_ResourceSelected, 0 ).AvailiableTypes;
      @_DataTypeOfConversionComboBox.Items.AddRange( types );
      if ( @_DataTypeOfConversionComboBox.Items.Count > 0 )
        @_DataTypeOfConversionComboBox.SelectedIndex = 0;
    }
    private short ResourceSelected()
    {
      Int16 m_ResourceSelected = Convert.ToInt16(WrappersHelpers.GetID( m_DataProviderID.GetAvailiableAddressspaces(), @_ResourceComboBox.Text ) );
      return m_ResourceSelected;
    }
    private void radioButtonDBWrite_CheckedChanged( object sender, EventArgs e )
    {
      //m_DataBlockLengthNumericUpDown.Enabled = false;
      //m_DataBlockLengthNumericUpDown.Value = 1;
      @_Value2BeWritten.Enabled = true;
    }
    private void radioButtonDBRead_CheckedChanged( object sender, EventArgs e )
    {
      //m_DataBlockLengthNumericUpDown.Enabled = true;
      @_Value2BeWritten.Enabled = false;
    }

    #region menu
    private void aboutToolStripMenutem_Click( object sender, EventArgs e )
    {
      //new CAS.Lib.Controls.AboutLicenseForm( null, null, Assembly.GetEntryAssembly() ).ShowDialog();
      string usr = null;
      if ( m_license != null )
        usr = m_license.User.Organization + "[" + m_license.User.Email + "]";
      Assembly cMyAss = Assembly.GetEntryAssembly();
      using ( AboutForm cAboutForm = new CAS.Lib.ControlLibrary.AboutForm( null, usr, cMyAss ) )
      {
        cAboutForm.ShowDialog();
      }
    }
    private void debugWindowToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( @_DebugWindowToolStripMenuItem.Checked && m_DebugWindow != null )
      {
        m_DebugInProgress = false;
        m_DebugWindow.Close();
        @_DebugWindowToolStripMenuItem.Checked = false;
      }
      else
      {
        if ( m_DebugWindow == null || m_DebugWindow.IsDisposed )
          m_DebugWindow = new ParallelDebugWindow( this.Icon, new FormClosingEventHandler( debugWindow_FormClosing ), Settings.Default.MaxBufferCapacity );
        m_DebugWindow.Show();
        @_DebugWindowToolStripMenuItem.Checked = true;
        m_DebugInProgress = true;
      }
    }
    private void licenseInformationToolStripMenuItem_Click( object sender, EventArgs e )
    {
      //new CAS.Lib.Controls.AboutLicenseForm( null, null, Assembly.GetEntryAssembly() ).ShowDialog();
      string usr = null;
      if ( m_license != null )
        usr = m_license.User.Organization + "[" + m_license.User.Email + "]";
      Assembly _EntryAssembly = Assembly.GetEntryAssembly();
      using ( LicenseForm cAboutForm = new CAS.Lib.ControlLibrary.LicenseForm( null, usr, _EntryAssembly ) )
      {
        using (Licenses cLicDial = new Licenses() )
        {
          cAboutForm.SetAdditionalControl = cLicDial;
          cAboutForm.ShowDialog();
        }
      }
    }
    private void enterTheUnlockCodeToolStripMenuItem_Click( object sender, EventArgs e )
    {
      using (UnlockKeyDialog dialog = new UnlockKeyDialog() )
      {
        dialog.ShowDialog();
      }
    }
    #endregion

    #endregion

    #region vars
    private long m_PreviusRunTime = 0;
    private Guid m_LastGuid = Guid.Empty;
    private string m_LastSettings = string.Empty;
    private IProtocolParent m_ProtocolParent = Protocol.CreateNewProtocol( "*********", "Diagnostic", 0, "not set" );
    private IProtocol m_Statistic;
    private IContainer components;
    private LicenseFile m_license = null;
    private IDataProviderID m_DataProviderID;
    private ParallelDebugWindow m_DebugWindow = null;
    private bool m_DebugInProgress = false;
    private RTLib.MinMaxAvrInTime m_Throughput = null;
    #endregion

    #endregion

  }
}
