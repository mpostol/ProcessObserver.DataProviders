using System.ComponentModel;
using System.Windows.Forms;
using CAS.Lib.CommonBus;
namespace CAS.DPDiagnostics
{
  partial class Program
  {


    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing )
    {
      if ( disposing && ( components != null ) )
      {
        components.Dispose();
      }
      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.Label label2;
      System.Windows.Forms.Label label3;
      System.Windows.Forms.Label label6;
      System.Windows.Forms.Label _TestTimeLab;
      System.Windows.Forms.Label _AverageChannelUsageLabel;
      System.Windows.Forms.Label _UserDataTransferedLabel;
      System.Windows.Forms.Label _UserDataThroughputLabel;
      System.Windows.Forms.TableLayoutPanel _AddressTable;
      System.Windows.Forms.Label _CharGapLab;
      System.Windows.Forms.Label _Synchronisation;
      System.Windows.Forms.Label _NAKLabel;
      System.Windows.Forms.Label _MaxResTime;
      System.Windows.Forms.Label _TimeOutCountLab;
      System.Windows.Forms.Label _lIncompleteLab;
      System.Windows.Forms.Label _CRCErrorLob;
      System.Windows.Forms.Label _RXFramesLab;
      System.Windows.Forms.Label _TXFramesLab;
      System.Windows.Forms.Label label7;
      System.Windows.Forms.Label _DBLengthLabel;
      System.Windows.Forms.Label _ResourceLabel;
      System.Windows.Forms.TableLayoutPanel _OpeartionTtable;
      System.Windows.Forms.Label label4;
      System.Windows.Forms.Label _Value2writeLabel;
      System.Windows.Forms.GroupBox _StationAddressGroupBox;
      System.Windows.Forms.GroupBox _OperationGroup;
      System.Windows.Forms.TableLayoutPanel _buttonsTable;
      System.Windows.Forms.GroupBox _ContentGroup;
      System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
      System.Windows.Forms.Label _DBStartAddrLabel;
      System.Windows.Forms.Label label8;
      System.Windows.Forms.GroupBox _StatisticsGroup;
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Program));
      this._PortSettings = new System.Windows.Forms.TextBox();
      this._ComlayerAddressComboBox = new System.Windows.Forms.ComboBox();
      this._OpenPortButton = new System.Windows.Forms.Button();
      this.m_StationAddressNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this._Value2BeWritten = new System.Windows.Forms.TextBox();
      this._ReadRadioButton = new System.Windows.Forms.RadioButton();
      this._WriteRadioButton = new System.Windows.Forms.RadioButton();
      this._Delay = new System.Windows.Forms.NumericUpDown();
      this._StartMonitoringButton = new System.Windows.Forms.Button();
      this._StopMonitoringButton = new System.Windows.Forms.Button();
      this._Exitbutton = new System.Windows.Forms.Button();
      this._DataTypeOfConversionComboBox = new System.Windows.Forms.ComboBox();
      this._ResourceComboBox = new System.Windows.Forms.ComboBox();
      this._RegisterAddressNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this._DataBlockLengthNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this._StatisticsTable = new System.Windows.Forms.TableLayoutPanel();
      this._AverageChannelUsage = new System.Windows.Forms.Label();
      this._AverageChannelUsageProgres = new System.Windows.Forms.ProgressBar();
      this._UserDataTransfered = new System.Windows.Forms.Label();
      this._RXFramesCount = new System.Windows.Forms.Label();
      this._MaxCharGapTime = new System.Windows.Forms.Label();
      this._TestTime = new System.Windows.Forms.Label();
      this._TXFramesCount = new System.Windows.Forms.Label();
      this._MaxResponseTime = new System.Windows.Forms.Label();
      this._stat_synchr_error_counter = new System.Windows.Forms.Label();
      this._CRCErrorsCnt = new System.Windows.Forms.Label();
      this._NAKCount = new System.Windows.Forms.Label();
      this._IncompleteCnt = new System.Windows.Forms.Label();
      this._TimeOutCount = new System.Windows.Forms.Label();
      this._UserDataThroughput = new System.Windows.Forms.Label();
      this._AddressToolTip = new System.Windows.Forms.ToolTip(this.components);
      this._OperationToolTip = new System.Windows.Forms.ToolTip(this.components);
      this._MasterTable = new System.Windows.Forms.TableLayoutPanel();
      this.m_BbackgroundWorker = new System.ComponentModel.BackgroundWorker();
      this._CommonBusControl = new CAS.Lib.CommonBus.CommonBusControl(this.components);
      this._MainMenu = new System.Windows.Forms.MenuStrip();
      this._SettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this._DebugWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this._HelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this._AboutToolStripMenutem = new System.Windows.Forms.ToolStripMenuItem();
      this._LicenseInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this._EnterTheUnlockCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this._StatisticsToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.m_ContentToolTip = new System.Windows.Forms.ToolTip(this.components);
      label2 = new System.Windows.Forms.Label();
      label3 = new System.Windows.Forms.Label();
      label6 = new System.Windows.Forms.Label();
      _TestTimeLab = new System.Windows.Forms.Label();
      _AverageChannelUsageLabel = new System.Windows.Forms.Label();
      _UserDataTransferedLabel = new System.Windows.Forms.Label();
      _UserDataThroughputLabel = new System.Windows.Forms.Label();
      _AddressTable = new System.Windows.Forms.TableLayoutPanel();
      _CharGapLab = new System.Windows.Forms.Label();
      _Synchronisation = new System.Windows.Forms.Label();
      _NAKLabel = new System.Windows.Forms.Label();
      _MaxResTime = new System.Windows.Forms.Label();
      _TimeOutCountLab = new System.Windows.Forms.Label();
      _lIncompleteLab = new System.Windows.Forms.Label();
      _CRCErrorLob = new System.Windows.Forms.Label();
      _RXFramesLab = new System.Windows.Forms.Label();
      _TXFramesLab = new System.Windows.Forms.Label();
      label7 = new System.Windows.Forms.Label();
      _DBLengthLabel = new System.Windows.Forms.Label();
      _ResourceLabel = new System.Windows.Forms.Label();
      _OpeartionTtable = new System.Windows.Forms.TableLayoutPanel();
      label4 = new System.Windows.Forms.Label();
      _Value2writeLabel = new System.Windows.Forms.Label();
      _StationAddressGroupBox = new System.Windows.Forms.GroupBox();
      _OperationGroup = new System.Windows.Forms.GroupBox();
      _buttonsTable = new System.Windows.Forms.TableLayoutPanel();
      _ContentGroup = new System.Windows.Forms.GroupBox();
      tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
      _DBStartAddrLabel = new System.Windows.Forms.Label();
      label8 = new System.Windows.Forms.Label();
      _StatisticsGroup = new System.Windows.Forms.GroupBox();
      _AddressTable.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.m_StationAddressNumericUpDown)).BeginInit();
      _OpeartionTtable.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this._Delay)).BeginInit();
      _StationAddressGroupBox.SuspendLayout();
      _OperationGroup.SuspendLayout();
      _buttonsTable.SuspendLayout();
      _ContentGroup.SuspendLayout();
      tableLayoutPanel2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this._RegisterAddressNumericUpDown)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this._DataBlockLengthNumericUpDown)).BeginInit();
      _StatisticsGroup.SuspendLayout();
      this._StatisticsTable.SuspendLayout();
      this._MasterTable.SuspendLayout();
      this._MainMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Dock = System.Windows.Forms.DockStyle.Fill;
      label2.Location = new System.Drawing.Point(3, 0);
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size(108, 26);
      label2.TabIndex = 46;
      label2.Text = "Application Layer";
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Dock = System.Windows.Forms.DockStyle.Fill;
      label3.Location = new System.Drawing.Point(3, 26);
      label3.Name = "label3";
      label3.Size = new System.Drawing.Size(108, 27);
      label3.TabIndex = 47;
      label3.Text = "Communication Layer";
      // 
      // label6
      // 
      label6.AutoSize = true;
      label6.Dock = System.Windows.Forms.DockStyle.Fill;
      label6.ForeColor = System.Drawing.SystemColors.MenuHighlight;
      label6.Location = new System.Drawing.Point(3, 0);
      label6.Name = "label6";
      label6.Size = new System.Drawing.Size(148, 15);
      label6.TabIndex = 19;
      label6.Text = "Counters:";
      label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // _TestTimeLab
      // 
      _TestTimeLab.AutoSize = true;
      _TestTimeLab.Dock = System.Windows.Forms.DockStyle.Fill;
      _TestTimeLab.Location = new System.Drawing.Point(3, 202);
      _TestTimeLab.Name = "_TestTimeLab";
      _TestTimeLab.Size = new System.Drawing.Size(148, 15);
      _TestTimeLab.TabIndex = 73;
      _TestTimeLab.Text = "Run time [s]";
      _TestTimeLab.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // _AverageChannelUsageLabel
      // 
      _AverageChannelUsageLabel.AutoSize = true;
      _AverageChannelUsageLabel.Dock = System.Windows.Forms.DockStyle.Fill;
      _AverageChannelUsageLabel.Location = new System.Drawing.Point(3, 180);
      _AverageChannelUsageLabel.Name = "_AverageChannelUsageLabel";
      _AverageChannelUsageLabel.Size = new System.Drawing.Size(148, 22);
      _AverageChannelUsageLabel.TabIndex = 80;
      _AverageChannelUsageLabel.Text = "Channel Usage [%]";
      _AverageChannelUsageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // _UserDataTransferedLabel
      // 
      _UserDataTransferedLabel.AutoSize = true;
      _UserDataTransferedLabel.Dock = System.Windows.Forms.DockStyle.Fill;
      _UserDataTransferedLabel.Location = new System.Drawing.Point(3, 165);
      _UserDataTransferedLabel.Name = "_UserDataTransferedLabel";
      _UserDataTransferedLabel.Size = new System.Drawing.Size(148, 15);
      _UserDataTransferedLabel.TabIndex = 78;
      _UserDataTransferedLabel.Text = "User data transfered";
      _UserDataTransferedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // _UserDataThroughputLabel
      // 
      _UserDataThroughputLabel.AutoSize = true;
      _UserDataThroughputLabel.Dock = System.Windows.Forms.DockStyle.Fill;
      _UserDataThroughputLabel.Location = new System.Drawing.Point(3, 217);
      _UserDataThroughputLabel.Name = "_UserDataThroughputLabel";
      _UserDataThroughputLabel.Size = new System.Drawing.Size(148, 44);
      _UserDataThroughputLabel.TabIndex = 78;
      _UserDataThroughputLabel.Text = "User data throughput [B/s]";
      _UserDataThroughputLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // _AddressTable
      // 
      _AddressTable.AutoSize = true;
      _AddressTable.ColumnCount = 2;
      _AddressTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      _AddressTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      _AddressTable.Controls.Add(label2, 0, 0);
      _AddressTable.Controls.Add(this._PortSettings, 1, 2);
      _AddressTable.Controls.Add(this._ComlayerAddressComboBox, 1, 1);
      _AddressTable.Controls.Add(this._OpenPortButton, 0, 2);
      _AddressTable.Controls.Add(label3, 0, 1);
      _AddressTable.Controls.Add(this.m_StationAddressNumericUpDown, 1, 0);
      _AddressTable.Dock = System.Windows.Forms.DockStyle.Fill;
      _AddressTable.ForeColor = System.Drawing.SystemColors.ControlText;
      _AddressTable.Location = new System.Drawing.Point(3, 16);
      _AddressTable.Name = "_AddressTable";
      _AddressTable.RowCount = 3;
      _AddressTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
      _AddressTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
      _AddressTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      _AddressTable.Size = new System.Drawing.Size(368, 129);
      _AddressTable.TabIndex = 83;
      // 
      // _PortSettings
      // 
      this._PortSettings.Location = new System.Drawing.Point(117, 56);
      this._PortSettings.Multiline = true;
      this._PortSettings.Name = "_PortSettings";
      this._PortSettings.ReadOnly = true;
      this._PortSettings.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this._PortSettings.Size = new System.Drawing.Size(248, 70);
      this._PortSettings.TabIndex = 82;
      this._PortSettings.TabStop = false;
      this._PortSettings.Text = "Press \'Protocol setting\' button to select DataProvider and provide communication " +
    "settings.";
      this._AddressToolTip.SetToolTip(this._PortSettings, "DataProvider description and settings.");
      // 
      // _ComlayerAddressComboBox
      // 
      this._ComlayerAddressComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this._ComlayerAddressComboBox.FormattingEnabled = true;
      this._ComlayerAddressComboBox.Location = new System.Drawing.Point(117, 29);
      this._ComlayerAddressComboBox.Name = "_ComlayerAddressComboBox";
      this._ComlayerAddressComboBox.Size = new System.Drawing.Size(248, 21);
      this._ComlayerAddressComboBox.TabIndex = 49;
      this._ComlayerAddressComboBox.Text = "Enter an address here:";
      this._AddressToolTip.SetToolTip(this._ComlayerAddressComboBox, "Communication layer addresss:\r\nIP: a.b.c.d:<port>\r\nRS:ignored.");
      // 
      // _OpenPortButton
      // 
      this._OpenPortButton.AutoSize = true;
      this._OpenPortButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this._OpenPortButton.Dock = System.Windows.Forms.DockStyle.Fill;
      this._OpenPortButton.Location = new System.Drawing.Point(3, 56);
      this._OpenPortButton.Name = "_OpenPortButton";
      this._OpenPortButton.Size = new System.Drawing.Size(108, 70);
      this._OpenPortButton.TabIndex = 0;
      this._OpenPortButton.Text = "Protocol settings";
      this._AddressToolTip.SetToolTip(this._OpenPortButton, "Select protocol and setup the configuration settings. \r\nPush to open the DataProv" +
        "ider settings window.");
      this._OpenPortButton.Click += new System.EventHandler(this.m_OpenPortButton_Click);
      // 
      // m_StationAddressNumericUpDown
      // 
      this.m_StationAddressNumericUpDown.AutoSize = true;
      this.m_StationAddressNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_StationAddressNumericUpDown.Location = new System.Drawing.Point(117, 3);
      this.m_StationAddressNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
      this.m_StationAddressNumericUpDown.Name = "m_StationAddressNumericUpDown";
      this.m_StationAddressNumericUpDown.Size = new System.Drawing.Size(248, 20);
      this.m_StationAddressNumericUpDown.TabIndex = 43;
      this.m_StationAddressNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this._AddressToolTip.SetToolTip(this.m_StationAddressNumericUpDown, "Remote station address as integer.");
      this.m_StationAddressNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // _CharGapLab
      // 
      _CharGapLab.AutoSize = true;
      _CharGapLab.Dock = System.Windows.Forms.DockStyle.Fill;
      _CharGapLab.Location = new System.Drawing.Point(3, 150);
      _CharGapLab.Name = "_CharGapLab";
      _CharGapLab.Size = new System.Drawing.Size(148, 15);
      _CharGapLab.TabIndex = 70;
      _CharGapLab.Text = "Characters gap  [us]";
      _CharGapLab.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // _Synchronisation
      // 
      _Synchronisation.AutoSize = true;
      _Synchronisation.Dock = System.Windows.Forms.DockStyle.Fill;
      _Synchronisation.Location = new System.Drawing.Point(3, 105);
      _Synchronisation.Name = "_Synchronisation";
      _Synchronisation.Size = new System.Drawing.Size(148, 15);
      _Synchronisation.TabIndex = 68;
      _Synchronisation.Text = "Synchr. errors";
      _Synchronisation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // _NAKLabel
      // 
      _NAKLabel.AutoSize = true;
      _NAKLabel.Dock = System.Windows.Forms.DockStyle.Fill;
      _NAKLabel.Location = new System.Drawing.Point(3, 90);
      _NAKLabel.Name = "_NAKLabel";
      _NAKLabel.Size = new System.Drawing.Size(148, 15);
      _NAKLabel.TabIndex = 68;
      _NAKLabel.Text = "NAK";
      _NAKLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // _MaxResTime
      // 
      _MaxResTime.AutoSize = true;
      _MaxResTime.Dock = System.Windows.Forms.DockStyle.Fill;
      _MaxResTime.Location = new System.Drawing.Point(3, 135);
      _MaxResTime.Name = "_MaxResTime";
      _MaxResTime.Size = new System.Drawing.Size(148, 15);
      _MaxResTime.TabIndex = 66;
      _MaxResTime.Text = "Response time [ms]";
      _MaxResTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // _TimeOutCountLab
      // 
      _TimeOutCountLab.AutoSize = true;
      _TimeOutCountLab.Dock = System.Windows.Forms.DockStyle.Fill;
      _TimeOutCountLab.Location = new System.Drawing.Point(3, 75);
      _TimeOutCountLab.Name = "_TimeOutCountLab";
      _TimeOutCountLab.Size = new System.Drawing.Size(148, 15);
      _TimeOutCountLab.TabIndex = 64;
      _TimeOutCountLab.Text = "Time-outs";
      _TimeOutCountLab.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // _lIncompleteLab
      // 
      _lIncompleteLab.AutoSize = true;
      _lIncompleteLab.Dock = System.Windows.Forms.DockStyle.Fill;
      _lIncompleteLab.Location = new System.Drawing.Point(3, 60);
      _lIncompleteLab.Name = "_lIncompleteLab";
      _lIncompleteLab.Size = new System.Drawing.Size(148, 15);
      _lIncompleteLab.TabIndex = 62;
      _lIncompleteLab.Text = "Incomplete frames";
      _lIncompleteLab.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // _CRCErrorLob
      // 
      _CRCErrorLob.AutoSize = true;
      _CRCErrorLob.Dock = System.Windows.Forms.DockStyle.Fill;
      _CRCErrorLob.Location = new System.Drawing.Point(3, 45);
      _CRCErrorLob.Name = "_CRCErrorLob";
      _CRCErrorLob.Size = new System.Drawing.Size(148, 15);
      _CRCErrorLob.TabIndex = 60;
      _CRCErrorLob.Text = "CRC errors";
      _CRCErrorLob.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // _RXFramesLab
      // 
      _RXFramesLab.AutoSize = true;
      _RXFramesLab.Dock = System.Windows.Forms.DockStyle.Fill;
      _RXFramesLab.Location = new System.Drawing.Point(3, 15);
      _RXFramesLab.Name = "_RXFramesLab";
      _RXFramesLab.Size = new System.Drawing.Size(148, 15);
      _RXFramesLab.TabIndex = 19;
      _RXFramesLab.Text = "RX frames";
      _RXFramesLab.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // _TXFramesLab
      // 
      _TXFramesLab.AutoSize = true;
      _TXFramesLab.Dock = System.Windows.Forms.DockStyle.Fill;
      _TXFramesLab.Location = new System.Drawing.Point(3, 30);
      _TXFramesLab.Name = "_TXFramesLab";
      _TXFramesLab.Size = new System.Drawing.Size(148, 15);
      _TXFramesLab.TabIndex = 18;
      _TXFramesLab.Text = "TX frames";
      _TXFramesLab.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label7
      // 
      label7.AutoSize = true;
      label7.Dock = System.Windows.Forms.DockStyle.Fill;
      label7.ForeColor = System.Drawing.SystemColors.MenuHighlight;
      label7.Location = new System.Drawing.Point(3, 120);
      label7.Name = "label7";
      label7.Size = new System.Drawing.Size(148, 15);
      label7.TabIndex = 19;
      label7.Text = "Time Statistics:";
      label7.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
      // 
      // _DBLengthLabel
      // 
      _DBLengthLabel.AutoSize = true;
      _DBLengthLabel.Dock = System.Windows.Forms.DockStyle.Fill;
      _DBLengthLabel.Location = new System.Drawing.Point(3, 81);
      _DBLengthLabel.Name = "_DBLengthLabel";
      _DBLengthLabel.Size = new System.Drawing.Size(69, 27);
      _DBLengthLabel.TabIndex = 3;
      _DBLengthLabel.Text = "Length";
      _DBLengthLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // _ResourceLabel
      // 
      _ResourceLabel.AutoSize = true;
      _ResourceLabel.Dock = System.Windows.Forms.DockStyle.Fill;
      _ResourceLabel.Location = new System.Drawing.Point(3, 0);
      _ResourceLabel.Name = "_ResourceLabel";
      _ResourceLabel.Size = new System.Drawing.Size(69, 27);
      _ResourceLabel.TabIndex = 11;
      _ResourceLabel.Text = "Resource";
      _ResourceLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // _OpeartionTtable
      // 
      _OpeartionTtable.AutoSize = true;
      _OpeartionTtable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      _OpeartionTtable.ColumnCount = 2;
      _OpeartionTtable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      _OpeartionTtable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      _OpeartionTtable.Controls.Add(this._Value2BeWritten, 1, 1);
      _OpeartionTtable.Controls.Add(label4, 0, 2);
      _OpeartionTtable.Controls.Add(_Value2writeLabel, 0, 1);
      _OpeartionTtable.Controls.Add(this._ReadRadioButton, 0, 0);
      _OpeartionTtable.Controls.Add(this._WriteRadioButton, 1, 0);
      _OpeartionTtable.Controls.Add(this._Delay, 1, 2);
      _OpeartionTtable.Dock = System.Windows.Forms.DockStyle.Fill;
      _OpeartionTtable.ForeColor = System.Drawing.SystemColors.ControlText;
      _OpeartionTtable.Location = new System.Drawing.Point(3, 16);
      _OpeartionTtable.Name = "_OpeartionTtable";
      _OpeartionTtable.RowCount = 3;
      _OpeartionTtable.RowStyles.Add(new System.Windows.Forms.RowStyle());
      _OpeartionTtable.RowStyles.Add(new System.Windows.Forms.RowStyle());
      _OpeartionTtable.RowStyles.Add(new System.Windows.Forms.RowStyle());
      _OpeartionTtable.Size = new System.Drawing.Size(368, 91);
      _OpeartionTtable.TabIndex = 87;
      // 
      // _Value2BeWritten
      // 
      this._Value2BeWritten.Dock = System.Windows.Forms.DockStyle.Fill;
      this._Value2BeWritten.Enabled = false;
      this._Value2BeWritten.Location = new System.Drawing.Point(93, 33);
      this._Value2BeWritten.Name = "_Value2BeWritten";
      this._Value2BeWritten.Size = new System.Drawing.Size(272, 20);
      this._Value2BeWritten.TabIndex = 7;
      this._Value2BeWritten.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this._OperationToolTip.SetToolTip(this._Value2BeWritten, "Enter value to write - text that is converted \r\nto object of selected type.");
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Dock = System.Windows.Forms.DockStyle.Fill;
      label4.Location = new System.Drawing.Point(3, 56);
      label4.Name = "label4";
      label4.Size = new System.Drawing.Size(84, 35);
      label4.TabIndex = 84;
      label4.Text = "Loop delay [ms]";
      this._OperationToolTip.SetToolTip(label4, "Delay [ms] between two consequent operations.");
      // 
      // _Value2writeLabel
      // 
      _Value2writeLabel.Location = new System.Drawing.Point(3, 30);
      _Value2writeLabel.Name = "_Value2writeLabel";
      _Value2writeLabel.Size = new System.Drawing.Size(80, 16);
      _Value2writeLabel.TabIndex = 6;
      _Value2writeLabel.Text = "Value to write";
      _Value2writeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
      this._OperationToolTip.SetToolTip(_Value2writeLabel, "Value to write");
      // 
      // _ReadRadioButton
      // 
      this._ReadRadioButton.Checked = true;
      this._ReadRadioButton.Location = new System.Drawing.Point(3, 3);
      this._ReadRadioButton.Name = "_ReadRadioButton";
      this._ReadRadioButton.Size = new System.Drawing.Size(84, 24);
      this._ReadRadioButton.TabIndex = 0;
      this._ReadRadioButton.TabStop = true;
      this._ReadRadioButton.Text = "Read";
      this._OperationToolTip.SetToolTip(this._ReadRadioButton, "Select to read from PLC");
      this._ReadRadioButton.CheckedChanged += new System.EventHandler(this.radioButtonDBRead_CheckedChanged);
      // 
      // _WriteRadioButton
      // 
      this._WriteRadioButton.Location = new System.Drawing.Point(93, 3);
      this._WriteRadioButton.Name = "_WriteRadioButton";
      this._WriteRadioButton.Size = new System.Drawing.Size(84, 24);
      this._WriteRadioButton.TabIndex = 1;
      this._WriteRadioButton.Text = "Write";
      this._OperationToolTip.SetToolTip(this._WriteRadioButton, "Select to write to PLC");
      this._WriteRadioButton.CheckedChanged += new System.EventHandler(this.radioButtonDBWrite_CheckedChanged);
      // 
      // _Delay
      // 
      this._Delay.AutoSize = true;
      this._Delay.Dock = System.Windows.Forms.DockStyle.Fill;
      this._Delay.Location = new System.Drawing.Point(93, 59);
      this._Delay.Maximum = 99999999;
      this._Delay.Minimum = 100;
      this._Delay.Value = 500;
      this._Delay.Name = "_Delay";
      this._Delay.Size = new System.Drawing.Size(272, 20);
      this._Delay.TabIndex = 85;
      this._Delay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this._OperationToolTip.SetToolTip(this._Delay, "Set delay [ms] between two consequent operations.");
      // 
      // _StationAddressGroupBox
      // 
      _StationAddressGroupBox.AutoSize = true;
      _StationAddressGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      _StationAddressGroupBox.Controls.Add(_AddressTable);
      _StationAddressGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
      _StationAddressGroupBox.ForeColor = System.Drawing.SystemColors.MenuHighlight;
      _StationAddressGroupBox.Location = new System.Drawing.Point(3, 3);
      _StationAddressGroupBox.Name = "_StationAddressGroupBox";
      _StationAddressGroupBox.Size = new System.Drawing.Size(374, 148);
      _StationAddressGroupBox.TabIndex = 55;
      _StationAddressGroupBox.TabStop = false;
      _StationAddressGroupBox.Text = "Station address";
      // 
      // _OperationGroup
      // 
      _OperationGroup.AutoSize = true;
      _OperationGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      _OperationGroup.Controls.Add(_OpeartionTtable);
      _OperationGroup.Dock = System.Windows.Forms.DockStyle.Fill;
      _OperationGroup.ForeColor = System.Drawing.SystemColors.MenuHighlight;
      _OperationGroup.Location = new System.Drawing.Point(3, 290);
      _OperationGroup.Name = "_OperationGroup";
      _OperationGroup.Size = new System.Drawing.Size(374, 110);
      _OperationGroup.TabIndex = 5;
      _OperationGroup.TabStop = false;
      _OperationGroup.Text = "Operation";
      // 
      // _buttonsTable
      // 
      _buttonsTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      _buttonsTable.ColumnCount = 2;
      _buttonsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      _buttonsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      _buttonsTable.Controls.Add(this._StartMonitoringButton, 0, 0);
      _buttonsTable.Controls.Add(this._StopMonitoringButton, 1, 0);
      _buttonsTable.Controls.Add(this._Exitbutton, 1, 1);
      _buttonsTable.Dock = System.Windows.Forms.DockStyle.Fill;
      _buttonsTable.Location = new System.Drawing.Point(383, 290);
      _buttonsTable.Name = "_buttonsTable";
      _buttonsTable.RowCount = 2;
      _buttonsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      _buttonsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      _buttonsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      _buttonsTable.Size = new System.Drawing.Size(398, 110);
      _buttonsTable.TabIndex = 87;
      // 
      // _StartMonitoringButton
      // 
      this._StartMonitoringButton.AutoSize = true;
      this._StartMonitoringButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this._StartMonitoringButton.Dock = System.Windows.Forms.DockStyle.Fill;
      this._StartMonitoringButton.Enabled = false;
      this._StartMonitoringButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
      this._StartMonitoringButton.Location = new System.Drawing.Point(9, 9);
      this._StartMonitoringButton.Margin = new System.Windows.Forms.Padding(9);
      this._StartMonitoringButton.Name = "_StartMonitoringButton";
      this._StartMonitoringButton.Size = new System.Drawing.Size(181, 37);
      this._StartMonitoringButton.TabIndex = 4;
      this._StartMonitoringButton.Text = "Start monitoring";
      this._StartMonitoringButton.Click += new System.EventHandler(this.m_StartMonitoringButton_Click);
      // 
      // _StopMonitoringButton
      // 
      this._StopMonitoringButton.AutoSize = true;
      this._StopMonitoringButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this._StopMonitoringButton.Dock = System.Windows.Forms.DockStyle.Fill;
      this._StopMonitoringButton.Enabled = false;
      this._StopMonitoringButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
      this._StopMonitoringButton.Location = new System.Drawing.Point(208, 9);
      this._StopMonitoringButton.Margin = new System.Windows.Forms.Padding(9);
      this._StopMonitoringButton.Name = "_StopMonitoringButton";
      this._StopMonitoringButton.Size = new System.Drawing.Size(181, 37);
      this._StopMonitoringButton.TabIndex = 49;
      this._StopMonitoringButton.Text = "Stop monitoring";
      this._StopMonitoringButton.Click += new System.EventHandler(this.m_StopMonitoringButton_Click);
      // 
      // _Exitbutton
      // 
      this._Exitbutton.AutoSize = true;
      this._Exitbutton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this._Exitbutton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this._Exitbutton.Dock = System.Windows.Forms.DockStyle.Fill;
      this._Exitbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
      this._Exitbutton.Location = new System.Drawing.Point(208, 64);
      this._Exitbutton.Margin = new System.Windows.Forms.Padding(9);
      this._Exitbutton.Name = "_Exitbutton";
      this._Exitbutton.Size = new System.Drawing.Size(181, 37);
      this._Exitbutton.TabIndex = 86;
      this._Exitbutton.Text = "Exit";
      this._Exitbutton.UseVisualStyleBackColor = true;
      this._Exitbutton.Click += new System.EventHandler(this.m_Exitbutton_Click);
      // 
      // _ContentGroup
      // 
      _ContentGroup.AutoSize = true;
      _ContentGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      _ContentGroup.Controls.Add(tableLayoutPanel2);
      _ContentGroup.Dock = System.Windows.Forms.DockStyle.Fill;
      _ContentGroup.ForeColor = System.Drawing.SystemColors.MenuHighlight;
      _ContentGroup.Location = new System.Drawing.Point(3, 157);
      _ContentGroup.Name = "_ContentGroup";
      _ContentGroup.Size = new System.Drawing.Size(374, 127);
      _ContentGroup.TabIndex = 0;
      _ContentGroup.TabStop = false;
      _ContentGroup.Text = "Content";
      // 
      // tableLayoutPanel2
      // 
      tableLayoutPanel2.AutoSize = true;
      tableLayoutPanel2.ColumnCount = 2;
      tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      tableLayoutPanel2.Controls.Add(this._DataTypeOfConversionComboBox, 1, 2);
      tableLayoutPanel2.Controls.Add(this._ResourceComboBox, 1, 0);
      tableLayoutPanel2.Controls.Add(this._RegisterAddressNumericUpDown, 1, 1);
      tableLayoutPanel2.Controls.Add(this._DataBlockLengthNumericUpDown, 1, 3);
      tableLayoutPanel2.Controls.Add(_DBStartAddrLabel, 0, 1);
      tableLayoutPanel2.Controls.Add(label8, 0, 2);
      tableLayoutPanel2.Controls.Add(_DBLengthLabel, 0, 3);
      tableLayoutPanel2.Controls.Add(_ResourceLabel, 0, 0);
      tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
      tableLayoutPanel2.ForeColor = System.Drawing.SystemColors.ControlText;
      tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
      tableLayoutPanel2.Name = "tableLayoutPanel2";
      tableLayoutPanel2.RowCount = 4;
      tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
      tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
      tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
      tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
      tableLayoutPanel2.Size = new System.Drawing.Size(368, 108);
      tableLayoutPanel2.TabIndex = 87;
      // 
      // _DataTypeOfConversionComboBox
      // 
      this._DataTypeOfConversionComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this._DataTypeOfConversionComboBox.FormattingEnabled = true;
      this._DataTypeOfConversionComboBox.Location = new System.Drawing.Point(78, 57);
      this._DataTypeOfConversionComboBox.Name = "_DataTypeOfConversionComboBox";
      this._DataTypeOfConversionComboBox.Size = new System.Drawing.Size(287, 21);
      this._DataTypeOfConversionComboBox.Sorted = true;
      this._DataTypeOfConversionComboBox.TabIndex = 10;
      this.m_ContentToolTip.SetToolTip(this._DataTypeOfConversionComboBox, "Data type to be used for conversion (casting) \r\nfrom/to internal PLC representati" +
        "on.");
      // 
      // _ResourceComboBox
      // 
      this._ResourceComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this._ResourceComboBox.FormattingEnabled = true;
      this._ResourceComboBox.Location = new System.Drawing.Point(78, 3);
      this._ResourceComboBox.Name = "_ResourceComboBox";
      this._ResourceComboBox.Size = new System.Drawing.Size(287, 21);
      this._ResourceComboBox.TabIndex = 10;
      this.m_ContentToolTip.SetToolTip(this._ResourceComboBox, "Select PLC resource (address space) \r\nfor communication testing.");
      this._ResourceComboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox_datatype_SelectedIndexChanged);
      // 
      // _RegisterAddressNumericUpDown
      // 
      this._RegisterAddressNumericUpDown.AutoSize = true;
      this._RegisterAddressNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
      this._RegisterAddressNumericUpDown.Location = new System.Drawing.Point(78, 30);
      this._RegisterAddressNumericUpDown.Maximum = new decimal(new int[] {
            64000,
            0,
            0,
            0});
      this._RegisterAddressNumericUpDown.Name = "_RegisterAddressNumericUpDown";
      this._RegisterAddressNumericUpDown.Size = new System.Drawing.Size(287, 20);
      this._RegisterAddressNumericUpDown.TabIndex = 1;
      this._RegisterAddressNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.m_ContentToolTip.SetToolTip(this._RegisterAddressNumericUpDown, "First address from the selected address \r\nspace (PLC resources).");
      this._RegisterAddressNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // _DataBlockLengthNumericUpDown
      // 
      this._DataBlockLengthNumericUpDown.AutoSize = true;
      this._DataBlockLengthNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
      this._DataBlockLengthNumericUpDown.Location = new System.Drawing.Point(78, 84);
      this._DataBlockLengthNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
      this._DataBlockLengthNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._DataBlockLengthNumericUpDown.Name = "_DataBlockLengthNumericUpDown";
      this._DataBlockLengthNumericUpDown.Size = new System.Drawing.Size(287, 20);
      this._DataBlockLengthNumericUpDown.TabIndex = 4;
      this._DataBlockLengthNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.m_ContentToolTip.SetToolTip(this._DataBlockLengthNumericUpDown, "Number of values (resources) to be read/write. \r\nFor text it is length of the str" +
        "ing.");
      this._DataBlockLengthNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // _DBStartAddrLabel
      // 
      _DBStartAddrLabel.AutoSize = true;
      _DBStartAddrLabel.Dock = System.Windows.Forms.DockStyle.Fill;
      _DBStartAddrLabel.Location = new System.Drawing.Point(3, 27);
      _DBStartAddrLabel.Name = "_DBStartAddrLabel";
      _DBStartAddrLabel.Size = new System.Drawing.Size(69, 27);
      _DBStartAddrLabel.TabIndex = 2;
      _DBStartAddrLabel.Text = "Start address";
      _DBStartAddrLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // label8
      // 
      label8.AutoSize = true;
      label8.Dock = System.Windows.Forms.DockStyle.Fill;
      label8.Location = new System.Drawing.Point(3, 54);
      label8.Name = "label8";
      label8.Size = new System.Drawing.Size(69, 27);
      label8.TabIndex = 3;
      label8.Text = "DataType";
      label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // _StatisticsGroup
      // 
      _StatisticsGroup.AutoSize = true;
      _StatisticsGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      _StatisticsGroup.Controls.Add(this._StatisticsTable);
      _StatisticsGroup.Dock = System.Windows.Forms.DockStyle.Fill;
      _StatisticsGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
      _StatisticsGroup.ForeColor = System.Drawing.SystemColors.MenuHighlight;
      _StatisticsGroup.Location = new System.Drawing.Point(383, 3);
      _StatisticsGroup.Name = "_StatisticsGroup";
      this._MasterTable.SetRowSpan(_StatisticsGroup, 2);
      _StatisticsGroup.Size = new System.Drawing.Size(398, 281);
      _StatisticsGroup.TabIndex = 59;
      _StatisticsGroup.TabStop = false;
      _StatisticsGroup.Text = "DataProvider statistics";
      // 
      // _StatisticsTable
      // 
      this._StatisticsTable.AutoSize = true;
      this._StatisticsTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this._StatisticsTable.ColumnCount = 3;
      this._StatisticsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this._StatisticsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
      this._StatisticsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this._StatisticsTable.Controls.Add(this._AverageChannelUsage, 1, 13);
      this._StatisticsTable.Controls.Add(this._AverageChannelUsageProgres, 2, 13);
      this._StatisticsTable.Controls.Add(label6, 0, 0);
      this._StatisticsTable.Controls.Add(this._UserDataTransfered, 1, 12);
      this._StatisticsTable.Controls.Add(_UserDataThroughputLabel, 0, 15);
      this._StatisticsTable.Controls.Add(_RXFramesLab, 0, 2);
      this._StatisticsTable.Controls.Add(_AverageChannelUsageLabel, 0, 13);
      this._StatisticsTable.Controls.Add(this._RXFramesCount, 1, 2);
      this._StatisticsTable.Controls.Add(this._MaxCharGapTime, 1, 11);
      this._StatisticsTable.Controls.Add(this._TestTime, 1, 14);
      this._StatisticsTable.Controls.Add(_TXFramesLab, 0, 3);
      this._StatisticsTable.Controls.Add(_TestTimeLab, 0, 14);
      this._StatisticsTable.Controls.Add(_CharGapLab, 0, 11);
      this._StatisticsTable.Controls.Add(this._TXFramesCount, 1, 3);
      this._StatisticsTable.Controls.Add(this._MaxResponseTime, 1, 10);
      this._StatisticsTable.Controls.Add(this._stat_synchr_error_counter, 1, 8);
      this._StatisticsTable.Controls.Add(_MaxResTime, 0, 10);
      this._StatisticsTable.Controls.Add(_CRCErrorLob, 0, 4);
      this._StatisticsTable.Controls.Add(label7, 0, 9);
      this._StatisticsTable.Controls.Add(_Synchronisation, 0, 8);
      this._StatisticsTable.Controls.Add(this._CRCErrorsCnt, 1, 4);
      this._StatisticsTable.Controls.Add(this._NAKCount, 1, 7);
      this._StatisticsTable.Controls.Add(_lIncompleteLab, 0, 5);
      this._StatisticsTable.Controls.Add(_NAKLabel, 0, 7);
      this._StatisticsTable.Controls.Add(this._IncompleteCnt, 1, 5);
      this._StatisticsTable.Controls.Add(_TimeOutCountLab, 0, 6);
      this._StatisticsTable.Controls.Add(this._TimeOutCount, 1, 6);
      this._StatisticsTable.Controls.Add(_UserDataTransferedLabel, 0, 12);
      this._StatisticsTable.Controls.Add(this._UserDataThroughput, 1, 15);
      this._StatisticsTable.Dock = System.Windows.Forms.DockStyle.Fill;
      this._StatisticsTable.ForeColor = System.Drawing.SystemColors.ControlText;
      this._StatisticsTable.Location = new System.Drawing.Point(3, 17);
      this._StatisticsTable.Name = "_StatisticsTable";
      this._StatisticsTable.RowCount = 16;
      this._StatisticsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this._StatisticsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this._StatisticsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this._StatisticsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this._StatisticsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this._StatisticsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this._StatisticsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this._StatisticsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this._StatisticsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this._StatisticsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this._StatisticsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this._StatisticsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this._StatisticsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this._StatisticsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this._StatisticsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this._StatisticsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this._StatisticsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this._StatisticsTable.Size = new System.Drawing.Size(392, 261);
      this._StatisticsTable.TabIndex = 87;
      // 
      // _AverageChannelUsage
      // 
      this._AverageChannelUsage.AutoSize = true;
      this._AverageChannelUsage.BackColor = System.Drawing.SystemColors.Window;
      this._AverageChannelUsage.Dock = System.Windows.Forms.DockStyle.Fill;
      this._AverageChannelUsage.Location = new System.Drawing.Point(157, 180);
      this._AverageChannelUsage.Name = "_AverageChannelUsage";
      this._AverageChannelUsage.Size = new System.Drawing.Size(54, 22);
      this._AverageChannelUsage.TabIndex = 82;
      this._AverageChannelUsage.Text = "0";
      this._AverageChannelUsage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._StatisticsToolTip.SetToolTip(this._AverageChannelUsage, "Average channel usage\r\noperation run time / total run time [%]\r\n");
      // 
      // _AverageChannelUsageProgres
      // 
      this._AverageChannelUsageProgres.Dock = System.Windows.Forms.DockStyle.Fill;
      this._AverageChannelUsageProgres.Location = new System.Drawing.Point(217, 183);
      this._AverageChannelUsageProgres.Name = "_AverageChannelUsageProgres";
      this._AverageChannelUsageProgres.Size = new System.Drawing.Size(172, 16);
      this._AverageChannelUsageProgres.Step = 1;
      this._AverageChannelUsageProgres.TabIndex = 81;
      // 
      // _UserDataTransfered
      // 
      this._UserDataTransfered.AutoSize = true;
      this._UserDataTransfered.BackColor = System.Drawing.SystemColors.Window;
      this._StatisticsTable.SetColumnSpan(this._UserDataTransfered, 2);
      this._UserDataTransfered.Dock = System.Windows.Forms.DockStyle.Fill;
      this._UserDataTransfered.Location = new System.Drawing.Point(157, 165);
      this._UserDataTransfered.Name = "_UserDataTransfered";
      this._UserDataTransfered.Size = new System.Drawing.Size(232, 15);
      this._UserDataTransfered.TabIndex = 80;
      this._UserDataTransfered.Text = "0";
      this._UserDataTransfered.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._StatisticsToolTip.SetToolTip(this._UserDataTransfered, "number of sent or received user data bytes.");
      // 
      // _RXFramesCount
      // 
      this._RXFramesCount.AutoSize = true;
      this._RXFramesCount.BackColor = System.Drawing.SystemColors.Window;
      this._StatisticsTable.SetColumnSpan(this._RXFramesCount, 2);
      this._RXFramesCount.Dock = System.Windows.Forms.DockStyle.Fill;
      this._RXFramesCount.ForeColor = System.Drawing.SystemColors.InfoText;
      this._RXFramesCount.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._RXFramesCount.Location = new System.Drawing.Point(157, 15);
      this._RXFramesCount.Name = "_RXFramesCount";
      this._RXFramesCount.Size = new System.Drawing.Size(232, 15);
      this._RXFramesCount.TabIndex = 58;
      this._RXFramesCount.Text = "0";
      this._RXFramesCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._StatisticsToolTip.SetToolTip(this._RXFramesCount, "Number of received frames.");
      // 
      // _MaxCharGapTime
      // 
      this._MaxCharGapTime.AutoSize = true;
      this._MaxCharGapTime.BackColor = System.Drawing.SystemColors.Window;
      this._StatisticsTable.SetColumnSpan(this._MaxCharGapTime, 2);
      this._MaxCharGapTime.Dock = System.Windows.Forms.DockStyle.Fill;
      this._MaxCharGapTime.ForeColor = System.Drawing.SystemColors.InfoText;
      this._MaxCharGapTime.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._MaxCharGapTime.Location = new System.Drawing.Point(157, 150);
      this._MaxCharGapTime.Name = "_MaxCharGapTime";
      this._MaxCharGapTime.Size = new System.Drawing.Size(232, 15);
      this._MaxCharGapTime.TabIndex = 71;
      this._MaxCharGapTime.Text = "0";
      this._MaxCharGapTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._StatisticsToolTip.SetToolTip(this._MaxCharGapTime, "waiting time of a character in response frame\r\nminimum/average/maximum [us]");
      // 
      // _TestTime
      // 
      this._TestTime.AutoSize = true;
      this._TestTime.BackColor = System.Drawing.SystemColors.Window;
      this._StatisticsTable.SetColumnSpan(this._TestTime, 2);
      this._TestTime.Dock = System.Windows.Forms.DockStyle.Fill;
      this._TestTime.ForeColor = System.Drawing.SystemColors.InfoText;
      this._TestTime.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._TestTime.Location = new System.Drawing.Point(157, 202);
      this._TestTime.Name = "_TestTime";
      this._TestTime.Size = new System.Drawing.Size(232, 15);
      this._TestTime.TabIndex = 74;
      this._TestTime.Text = "0";
      this._TestTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._StatisticsToolTip.SetToolTip(this._TestTime, "Run time of the test.");
      // 
      // _TXFramesCount
      // 
      this._TXFramesCount.AutoSize = true;
      this._TXFramesCount.BackColor = System.Drawing.SystemColors.Window;
      this._StatisticsTable.SetColumnSpan(this._TXFramesCount, 2);
      this._TXFramesCount.Dock = System.Windows.Forms.DockStyle.Fill;
      this._TXFramesCount.ForeColor = System.Drawing.SystemColors.InfoText;
      this._TXFramesCount.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._TXFramesCount.Location = new System.Drawing.Point(157, 30);
      this._TXFramesCount.Name = "_TXFramesCount";
      this._TXFramesCount.Size = new System.Drawing.Size(232, 15);
      this._TXFramesCount.TabIndex = 59;
      this._TXFramesCount.Text = "0";
      this._TXFramesCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._StatisticsToolTip.SetToolTip(this._TXFramesCount, "Number of sent frames.");
      // 
      // _MaxResponseTime
      // 
      this._MaxResponseTime.AutoSize = true;
      this._MaxResponseTime.BackColor = System.Drawing.SystemColors.Window;
      this._StatisticsTable.SetColumnSpan(this._MaxResponseTime, 2);
      this._MaxResponseTime.Dock = System.Windows.Forms.DockStyle.Fill;
      this._MaxResponseTime.ForeColor = System.Drawing.SystemColors.InfoText;
      this._MaxResponseTime.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._MaxResponseTime.Location = new System.Drawing.Point(157, 135);
      this._MaxResponseTime.Name = "_MaxResponseTime";
      this._MaxResponseTime.Size = new System.Drawing.Size(232, 15);
      this._MaxResponseTime.TabIndex = 67;
      this._MaxResponseTime.Text = "0";
      this._MaxResponseTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._StatisticsToolTip.SetToolTip(this._MaxResponseTime, "waiting time of the first character in response \r\nminimum/average/maximum [ms]");
      // 
      // _stat_synchr_error_counter
      // 
      this._stat_synchr_error_counter.AutoSize = true;
      this._stat_synchr_error_counter.BackColor = System.Drawing.SystemColors.Window;
      this._StatisticsTable.SetColumnSpan(this._stat_synchr_error_counter, 2);
      this._stat_synchr_error_counter.Dock = System.Windows.Forms.DockStyle.Fill;
      this._stat_synchr_error_counter.ForeColor = System.Drawing.SystemColors.InfoText;
      this._stat_synchr_error_counter.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._stat_synchr_error_counter.Location = new System.Drawing.Point(157, 105);
      this._stat_synchr_error_counter.Name = "_stat_synchr_error_counter";
      this._stat_synchr_error_counter.Size = new System.Drawing.Size(232, 15);
      this._stat_synchr_error_counter.TabIndex = 69;
      this._stat_synchr_error_counter.Text = "0";
      this._stat_synchr_error_counter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._StatisticsToolTip.SetToolTip(this._stat_synchr_error_counter, "Number of synchronization errors");
      // 
      // _CRCErrorsCnt
      // 
      this._CRCErrorsCnt.AutoSize = true;
      this._CRCErrorsCnt.BackColor = System.Drawing.SystemColors.Window;
      this._StatisticsTable.SetColumnSpan(this._CRCErrorsCnt, 2);
      this._CRCErrorsCnt.Dock = System.Windows.Forms.DockStyle.Fill;
      this._CRCErrorsCnt.ForeColor = System.Drawing.SystemColors.InfoText;
      this._CRCErrorsCnt.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._CRCErrorsCnt.Location = new System.Drawing.Point(157, 45);
      this._CRCErrorsCnt.Name = "_CRCErrorsCnt";
      this._CRCErrorsCnt.Size = new System.Drawing.Size(232, 15);
      this._CRCErrorsCnt.TabIndex = 61;
      this._CRCErrorsCnt.Text = "0";
      this._CRCErrorsCnt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._StatisticsToolTip.SetToolTip(this._CRCErrorsCnt, "Number of CRC errors.");
      // 
      // _NAKCount
      // 
      this._NAKCount.AutoSize = true;
      this._NAKCount.BackColor = System.Drawing.SystemColors.Window;
      this._StatisticsTable.SetColumnSpan(this._NAKCount, 2);
      this._NAKCount.Dock = System.Windows.Forms.DockStyle.Fill;
      this._NAKCount.ForeColor = System.Drawing.SystemColors.InfoText;
      this._NAKCount.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._NAKCount.Location = new System.Drawing.Point(157, 90);
      this._NAKCount.Name = "_NAKCount";
      this._NAKCount.Size = new System.Drawing.Size(232, 15);
      this._NAKCount.TabIndex = 69;
      this._NAKCount.Text = "0";
      this._NAKCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._StatisticsToolTip.SetToolTip(this._NAKCount, "number of received NAK \r\n(negative acknowledge)");
      // 
      // _IncompleteCnt
      // 
      this._IncompleteCnt.AutoSize = true;
      this._IncompleteCnt.BackColor = System.Drawing.SystemColors.Window;
      this._StatisticsTable.SetColumnSpan(this._IncompleteCnt, 2);
      this._IncompleteCnt.Dock = System.Windows.Forms.DockStyle.Fill;
      this._IncompleteCnt.ForeColor = System.Drawing.SystemColors.InfoText;
      this._IncompleteCnt.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._IncompleteCnt.Location = new System.Drawing.Point(157, 60);
      this._IncompleteCnt.Name = "_IncompleteCnt";
      this._IncompleteCnt.Size = new System.Drawing.Size(232, 15);
      this._IncompleteCnt.TabIndex = 63;
      this._IncompleteCnt.Text = "0";
      this._IncompleteCnt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._StatisticsToolTip.SetToolTip(this._IncompleteCnt, "Number of incomplete frames.");
      // 
      // _TimeOutCount
      // 
      this._TimeOutCount.AutoSize = true;
      this._TimeOutCount.BackColor = System.Drawing.SystemColors.Window;
      this._StatisticsTable.SetColumnSpan(this._TimeOutCount, 2);
      this._TimeOutCount.Dock = System.Windows.Forms.DockStyle.Fill;
      this._TimeOutCount.ForeColor = System.Drawing.SystemColors.InfoText;
      this._TimeOutCount.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._TimeOutCount.Location = new System.Drawing.Point(157, 75);
      this._TimeOutCount.Name = "_TimeOutCount";
      this._TimeOutCount.Size = new System.Drawing.Size(232, 15);
      this._TimeOutCount.TabIndex = 65;
      this._TimeOutCount.Text = "0";
      this._TimeOutCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._StatisticsToolTip.SetToolTip(this._TimeOutCount, "Number of timeouts.");
      // 
      // _UserDataThroughput
      // 
      this._UserDataThroughput.AutoSize = true;
      this._UserDataThroughput.BackColor = System.Drawing.SystemColors.Window;
      this._StatisticsTable.SetColumnSpan(this._UserDataThroughput, 2);
      this._UserDataThroughput.Dock = System.Windows.Forms.DockStyle.Fill;
      this._UserDataThroughput.Location = new System.Drawing.Point(157, 217);
      this._UserDataThroughput.Name = "_UserDataThroughput";
      this._UserDataThroughput.Size = new System.Drawing.Size(232, 44);
      this._UserDataThroughput.TabIndex = 79;
      this._UserDataThroughput.Text = "0";
      this._UserDataThroughput.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._StatisticsToolTip.SetToolTip(this._UserDataThroughput, "Current user data throughput\r\nuser data / total run time [Bytes/s]");
      // 
      // _AddressToolTip
      // 
      this._AddressToolTip.AutoPopDelay = 5000;
      this._AddressToolTip.InitialDelay = 500;
      this._AddressToolTip.IsBalloon = true;
      this._AddressToolTip.ReshowDelay = 200;
      this._AddressToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
      this._AddressToolTip.ToolTipTitle = "Remote Unit Address";
      // 
      // _OperationToolTip
      // 
      this._OperationToolTip.IsBalloon = true;
      this._OperationToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
      this._OperationToolTip.ToolTipTitle = "Operation settings";
      // 
      // _MasterTable
      // 
      this._MasterTable.AutoSize = true;
      this._MasterTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this._MasterTable.ColumnCount = 2;
      this._MasterTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this._MasterTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this._MasterTable.Controls.Add(_StationAddressGroupBox, 0, 0);
      this._MasterTable.Controls.Add(_OperationGroup, 0, 2);
      this._MasterTable.Controls.Add(_buttonsTable, 1, 2);
      this._MasterTable.Controls.Add(_ContentGroup, 0, 1);
      this._MasterTable.Controls.Add(_StatisticsGroup, 1, 0);
      this._MasterTable.Dock = System.Windows.Forms.DockStyle.Fill;
      this._MasterTable.Location = new System.Drawing.Point(0, 24);
      this._MasterTable.Name = "_MasterTable";
      this._MasterTable.RowCount = 3;
      this._MasterTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this._MasterTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this._MasterTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this._MasterTable.Size = new System.Drawing.Size(775, 396);
      this._MasterTable.TabIndex = 88;
      // 
      // m_BbackgroundWorker
      // 
      this.m_BbackgroundWorker.WorkerReportsProgress = true;
      this.m_BbackgroundWorker.WorkerSupportsCancellation = true;
      this.m_BbackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.cm_BbackgroundWorker_DoWork);
      this.m_BbackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.cm_BbackgroundWorker_ProgressChanged);
      this.m_BbackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.cm_BbackgroundWorker_RunWorkerCompleted);
      // 
      // _MainMenu
      // 
      this._MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._SettingsToolStripMenuItem,
            this._HelpToolStripMenuItem});
      this._MainMenu.Location = new System.Drawing.Point(0, 0);
      this._MainMenu.Name = "_MainMenu";
      this._MainMenu.Size = new System.Drawing.Size(775, 24);
      this._MainMenu.TabIndex = 83;
      this._MainMenu.Text = "menuStrip_MainMenu";
      // 
      // _SettingsToolStripMenuItem
      // 
      this._SettingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._DebugWindowToolStripMenuItem,
            this.exitToolStripMenuItem});
      this._SettingsToolStripMenuItem.Name = "_SettingsToolStripMenuItem";
      this._SettingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
      this._SettingsToolStripMenuItem.Text = "Settings";
      // 
      // _DebugWindowToolStripMenuItem
      // 
      this._DebugWindowToolStripMenuItem.Name = "_DebugWindowToolStripMenuItem";
      this._DebugWindowToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
      this._DebugWindowToolStripMenuItem.Text = "Debug Window";
      this._DebugWindowToolStripMenuItem.Click += new System.EventHandler(this.debugWindowToolStripMenuItem_Click);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
      this.exitToolStripMenuItem.Text = "Exit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // _HelpToolStripMenuItem
      // 
      this._HelpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._AboutToolStripMenutem,
            this._LicenseInformationToolStripMenuItem,
            this._EnterTheUnlockCodeToolStripMenuItem});
      this._HelpToolStripMenuItem.Name = "_HelpToolStripMenuItem";
      this._HelpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
      this._HelpToolStripMenuItem.Text = "Help";
      // 
      // _AboutToolStripMenutem
      // 
      this._AboutToolStripMenutem.Image = ((System.Drawing.Image)(resources.GetObject("_AboutToolStripMenutem.Image")));
      this._AboutToolStripMenutem.Name = "_AboutToolStripMenutem";
      this._AboutToolStripMenutem.Size = new System.Drawing.Size(226, 22);
      this._AboutToolStripMenutem.Text = "About DPDiagnostic";
      this._AboutToolStripMenutem.Click += new System.EventHandler(this.aboutToolStripMenutem_Click);
      // 
      // _LicenseInformationToolStripMenuItem
      // 
      this._LicenseInformationToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_LicenseInformationToolStripMenuItem.Image")));
      this._LicenseInformationToolStripMenuItem.Name = "_LicenseInformationToolStripMenuItem";
      this._LicenseInformationToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
      this._LicenseInformationToolStripMenuItem.Text = "License information";
      this._LicenseInformationToolStripMenuItem.Click += new System.EventHandler(this.licenseInformationToolStripMenuItem_Click);
      // 
      // _EnterTheUnlockCodeToolStripMenuItem
      // 
      this._EnterTheUnlockCodeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_EnterTheUnlockCodeToolStripMenuItem.Image")));
      this._EnterTheUnlockCodeToolStripMenuItem.Name = "_EnterTheUnlockCodeToolStripMenuItem";
      this._EnterTheUnlockCodeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.K)));
      this._EnterTheUnlockCodeToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
      this._EnterTheUnlockCodeToolStripMenuItem.Text = "Enter the unlock code";
      this._EnterTheUnlockCodeToolStripMenuItem.Click += new System.EventHandler(this.enterTheUnlockCodeToolStripMenuItem_Click);
      // 
      // _StatisticsToolTip
      // 
      this._StatisticsToolTip.IsBalloon = true;
      this._StatisticsToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
      this._StatisticsToolTip.ToolTipTitle = "DataProvider statistics";
      // 
      // m_ContentToolTip
      // 
      this.m_ContentToolTip.IsBalloon = true;
      // 
      // Program
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.ClientSize = new System.Drawing.Size(775, 420);
      this.Controls.Add(this._MasterTable);
      this.Controls.Add(this._MainMenu);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this._MainMenu;
      this.MaximizeBox = false;
      this.Name = "Program";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Data Provider Diagnostics Tool";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Program_FormClosing);
      this.Load += new System.EventHandler(this.Program_Load);
      _AddressTable.ResumeLayout(false);
      _AddressTable.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.m_StationAddressNumericUpDown)).EndInit();
      _OpeartionTtable.ResumeLayout(false);
      _OpeartionTtable.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this._Delay)).EndInit();
      _StationAddressGroupBox.ResumeLayout(false);
      _StationAddressGroupBox.PerformLayout();
      _OperationGroup.ResumeLayout(false);
      _OperationGroup.PerformLayout();
      _buttonsTable.ResumeLayout(false);
      _buttonsTable.PerformLayout();
      _ContentGroup.ResumeLayout(false);
      _ContentGroup.PerformLayout();
      tableLayoutPanel2.ResumeLayout(false);
      tableLayoutPanel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this._RegisterAddressNumericUpDown)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this._DataBlockLengthNumericUpDown)).EndInit();
      _StatisticsGroup.ResumeLayout(false);
      _StatisticsGroup.PerformLayout();
      this._StatisticsTable.ResumeLayout(false);
      this._StatisticsTable.PerformLayout();
      this._MasterTable.ResumeLayout(false);
      this._MasterTable.PerformLayout();
      this._MainMenu.ResumeLayout(false);
      this._MainMenu.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.Windows.Forms.Button @_OpenPortButton;
    private System.Windows.Forms.Button @_StartMonitoringButton;
    private System.Windows.Forms.Button @_StopMonitoringButton;
    private System.Windows.Forms.NumericUpDown @_RegisterAddressNumericUpDown;
    private System.Windows.Forms.NumericUpDown @_DataBlockLengthNumericUpDown;
    private System.Windows.Forms.RadioButton @_ReadRadioButton;
    private System.Windows.Forms.RadioButton @_WriteRadioButton;
    private System.Windows.Forms.NumericUpDown m_StationAddressNumericUpDown;
    private System.Windows.Forms.Label @_RXFramesCount;
    private System.Windows.Forms.Label @_TXFramesCount;
    private System.Windows.Forms.Label @_CRCErrorsCnt;
    private System.Windows.Forms.Label @_IncompleteCnt;
    private System.Windows.Forms.Label @_TimeOutCount;
    private System.Windows.Forms.Label @_MaxResponseTime;
    private System.Windows.Forms.Label @_NAKCount;
    private System.Windows.Forms.Label @_MaxCharGapTime;
    private System.Windows.Forms.Label @_TestTime;
    private System.Windows.Forms.TextBox @_Value2BeWritten;
    private System.Windows.Forms.Label _UserDataThroughput;
    private System.Windows.Forms.Label @_AverageChannelUsage;
    private System.Windows.Forms.ProgressBar @_AverageChannelUsageProgres;
    private System.Windows.Forms.Label _UserDataTransfered;
    private CommonBusControl @_CommonBusControl;
    private BackgroundWorker m_BbackgroundWorker;
    private TextBox @_PortSettings;
    private MenuStrip @_MainMenu;
    private ToolStripMenuItem @_HelpToolStripMenuItem;
    private ToolStripMenuItem @_AboutToolStripMenutem;
    private ComboBox @_ComlayerAddressComboBox;
    private ComboBox @_ResourceComboBox;
    private Label @_stat_synchr_error_counter;
    private ToolStripMenuItem @_SettingsToolStripMenuItem;
    private ToolStripMenuItem @_DebugWindowToolStripMenuItem;
    private ComboBox @_DataTypeOfConversionComboBox;
    private ToolStripMenuItem @_LicenseInformationToolStripMenuItem;
    private ToolStripMenuItem @_EnterTheUnlockCodeToolStripMenuItem;
    private NumericUpDown @_Delay;
    private ToolTip _AddressToolTip;
    private Button _Exitbutton;
    private ToolTip _OperationToolTip;
    private TableLayoutPanel _MasterTable;
    private TableLayoutPanel _StatisticsTable;
    private ToolTip m_ContentToolTip;
    private ToolTip _StatisticsToolTip;
    private ToolStripMenuItem exitToolStripMenuItem;

  }
}