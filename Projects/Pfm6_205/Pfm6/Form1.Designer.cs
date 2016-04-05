namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ButtonsFrame = new System.Windows.Forms.GroupBox();
            this.aGauge2 = new AGaugeApp.AGauge();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.DAC1 = new System.Windows.Forms.TrackBar();
            this.DAC2 = new System.Windows.Forms.TrackBar();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.HV = new System.Windows.Forms.TrackBar();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.DelayPw = new System.Windows.Forms.TrackBar();
            this.DelayTime = new System.Windows.Forms.TrackBar();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.SimmerPw = new System.Windows.Forms.TrackBar();
            this.label9 = new System.Windows.Forms.Label();
            this.SimmerRate = new System.Windows.Forms.TrackBar();
            this.aGauge1 = new AGaugeApp.AGauge();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Xlap4 = new System.Windows.Forms.RadioButton();
            this.Xlap2 = new System.Windows.Forms.RadioButton();
            this.Xlap1 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Simmer2 = new System.Windows.Forms.CheckBox();
            this.Simmer1 = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.TriggerButton = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConnectMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ScopeMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.TempMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.eCEnable = new System.Windows.Forms.ToolStripMenuItem();
            this.qSPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vLPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mSPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sSPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.com = new System.IO.Ports.SerialPort(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.StatusList = new System.Windows.Forms.ListView();
            this.IgbtTemp = new System.Windows.Forms.Timer(this.components);
            this.Delay = new System.Windows.Forms.Timer(this.components);
            this.SetFrame = new System.Windows.Forms.GroupBox();
            this.trackT = new System.Windows.Forms.TrackBar();
            this.trackU = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ResetFrame = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Length = new System.Windows.Forms.TrackBar();
            this.Burst = new System.Windows.Forms.TrackBar();
            this.Period = new System.Windows.Forms.TrackBar();
            this.ResetBurst = new System.Windows.Forms.TrackBar();
            this.BootBar = new System.Windows.Forms.ProgressBar();
            this.Login = new System.Windows.Forms.Timer(this.components);
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.Pbar = new System.Windows.Forms.ProgressBar();
            this.shaped = new System.Windows.Forms.CheckBox();
            this.charger = new System.Windows.Forms.CheckBox();
            this.channel1 = new System.Windows.Forms.RadioButton();
            this.channel2 = new System.Windows.Forms.RadioButton();
            this.channel = new System.Windows.Forms.RadioButton();
            this.ButtonsFrame.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DAC1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DAC2)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HV)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DelayPw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelayTime)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SimmerPw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SimmerRate)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SetFrame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackU)).BeginInit();
            this.ResetFrame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Length)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Burst)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Period)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResetBurst)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonsFrame
            // 
            this.ButtonsFrame.Controls.Add(this.aGauge2);
            this.ButtonsFrame.Controls.Add(this.groupBox6);
            this.ButtonsFrame.Controls.Add(this.groupBox5);
            this.ButtonsFrame.Controls.Add(this.groupBox4);
            this.ButtonsFrame.Controls.Add(this.groupBox3);
            this.ButtonsFrame.Controls.Add(this.aGauge1);
            this.ButtonsFrame.Controls.Add(this.groupBox2);
            this.ButtonsFrame.Controls.Add(this.groupBox1);
            this.ButtonsFrame.Controls.Add(this.label8);
            this.ButtonsFrame.Controls.Add(this.TriggerButton);
            this.ButtonsFrame.Controls.Add(this.textBox2);
            this.ButtonsFrame.Controls.Add(this.textBox1);
            this.ButtonsFrame.Dock = System.Windows.Forms.DockStyle.Right;
            this.ButtonsFrame.Enabled = false;
            this.ButtonsFrame.Location = new System.Drawing.Point(607, 24);
            this.ButtonsFrame.Name = "ButtonsFrame";
            this.ButtonsFrame.Size = new System.Drawing.Size(263, 420);
            this.ButtonsFrame.TabIndex = 2;
            this.ButtonsFrame.TabStop = false;
            // 
            // aGauge2
            // 
            this.aGauge2.BackColor = System.Drawing.SystemColors.Control;
            this.aGauge2.BaseArcColor = System.Drawing.Color.Gray;
            this.aGauge2.BaseArcRadius = 30;
            this.aGauge2.BaseArcStart = 0;
            this.aGauge2.BaseArcSweep = -90;
            this.aGauge2.BaseArcWidth = 2;
            this.aGauge2.Cap_Idx = ((byte)(1));
            this.aGauge2.CapColors = new System.Drawing.Color[] {
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black};
            this.aGauge2.CapPosition = new System.Drawing.Point(10, 10);
            this.aGauge2.CapsPosition = new System.Drawing.Point[] {
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10)};
            this.aGauge2.CapsText = new string[] {
        "",
        "",
        "",
        "",
        ""};
            this.aGauge2.CapText = "";
            this.aGauge2.Center = new System.Drawing.Point(15, 70);
            this.aGauge2.Location = new System.Drawing.Point(6, 148);
            this.aGauge2.MaxValue = 120F;
            this.aGauge2.MinValue = 0F;
            this.aGauge2.Name = "aGauge2";
            this.aGauge2.NeedleColor1 = AGaugeApp.AGauge.NeedleColorEnum.Gray;
            this.aGauge2.NeedleColor2 = System.Drawing.Color.Maroon;
            this.aGauge2.NeedleRadius = 45;
            this.aGauge2.NeedleType = 0;
            this.aGauge2.NeedleWidth = 3;
            this.aGauge2.Range_Idx = ((byte)(2));
            this.aGauge2.RangeColor = System.Drawing.Color.SandyBrown;
            this.aGauge2.RangeEnabled = true;
            this.aGauge2.RangeEndValue = 120F;
            this.aGauge2.RangeInnerRadius = 30;
            this.aGauge2.RangeOuterRadius = 50;
            this.aGauge2.RangesColor = new System.Drawing.Color[] {
        System.Drawing.Color.YellowGreen,
        System.Drawing.Color.Gold,
        System.Drawing.Color.SandyBrown,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.aGauge2.RangesEnabled = new bool[] {
        false,
        false,
        true,
        false,
        false};
            this.aGauge2.RangesEndValue = new float[] {
        40F,
        95F,
        120F,
        0F,
        0F};
            this.aGauge2.RangesInnerRadius = new int[] {
        10,
        10,
        30,
        70,
        70};
            this.aGauge2.RangesOuterRadius = new int[] {
        50,
        50,
        50,
        80,
        80};
            this.aGauge2.RangesStartValue = new float[] {
        0F,
        40F,
        95F,
        0F,
        0F};
            this.aGauge2.RangeStartValue = 95F;
            this.aGauge2.ScaleLinesInterColor = System.Drawing.Color.Black;
            this.aGauge2.ScaleLinesInterInnerRadius = 30;
            this.aGauge2.ScaleLinesInterOuterRadius = 40;
            this.aGauge2.ScaleLinesInterWidth = 1;
            this.aGauge2.ScaleLinesMajorColor = System.Drawing.Color.Black;
            this.aGauge2.ScaleLinesMajorInnerRadius = 30;
            this.aGauge2.ScaleLinesMajorOuterRadius = 50;
            this.aGauge2.ScaleLinesMajorStepValue = 40F;
            this.aGauge2.ScaleLinesMajorWidth = 1;
            this.aGauge2.ScaleLinesMinorColor = System.Drawing.Color.Gray;
            this.aGauge2.ScaleLinesMinorInnerRadius = 30;
            this.aGauge2.ScaleLinesMinorNumOf = 9;
            this.aGauge2.ScaleLinesMinorOuterRadius = 35;
            this.aGauge2.ScaleLinesMinorWidth = 1;
            this.aGauge2.ScaleNumbersColor = System.Drawing.Color.Black;
            this.aGauge2.ScaleNumbersFormat = null;
            this.aGauge2.ScaleNumbersRadius = 60;
            this.aGauge2.ScaleNumbersRotation = 0;
            this.aGauge2.ScaleNumbersStartScaleLine = 0;
            this.aGauge2.ScaleNumbersStepScaleLines = 10;
            this.aGauge2.Size = new System.Drawing.Size(79, 87);
            this.aGauge2.TabIndex = 42;
            this.aGauge2.Text = "aGauge2";
            this.aGauge2.Value = 0F;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.DAC1);
            this.groupBox6.Controls.Add(this.DAC2);
            this.groupBox6.Location = new System.Drawing.Point(79, 134);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(81, 116);
            this.groupBox6.TabIndex = 52;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Curr.limit";
            // 
            // DAC1
            // 
            this.DAC1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DAC1.Location = new System.Drawing.Point(6, 17);
            this.DAC1.Maximum = 4095;
            this.DAC1.Name = "DAC1";
            this.DAC1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.DAC1.Size = new System.Drawing.Size(42, 92);
            this.DAC1.TabIndex = 43;
            this.DAC1.TickFrequency = 512;
            this.DAC1.Value = 4095;
            this.DAC1.Scroll += new System.EventHandler(this.SlidersScroll);
            this.DAC1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DAC_MouseUp);
            // 
            // DAC2
            // 
            this.DAC2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DAC2.Location = new System.Drawing.Point(40, 17);
            this.DAC2.Maximum = 4095;
            this.DAC2.Name = "DAC2";
            this.DAC2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.DAC2.Size = new System.Drawing.Size(42, 92);
            this.DAC2.TabIndex = 45;
            this.DAC2.TickFrequency = 512;
            this.DAC2.Value = 4095;
            this.DAC2.Scroll += new System.EventHandler(this.SlidersScroll);
            this.DAC2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DAC_MouseUp);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.HV);
            this.groupBox5.Location = new System.Drawing.Point(3, 253);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(56, 167);
            this.groupBox5.TabIndex = 52;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "HV";
            // 
            // HV
            // 
            this.HV.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HV.Location = new System.Drawing.Point(6, 19);
            this.HV.Maximum = 800;
            this.HV.Name = "HV";
            this.HV.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.HV.Size = new System.Drawing.Size(42, 124);
            this.HV.SmallChange = 50;
            this.HV.TabIndex = 30;
            this.HV.TickFrequency = 50;
            this.HV.Value = 80;
            this.HV.Scroll += new System.EventHandler(this.SlidersScroll);
            this.HV.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SetHVCommand);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.DelayPw);
            this.groupBox4.Controls.Add(this.DelayTime);
            this.groupBox4.Location = new System.Drawing.Point(159, 253);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(92, 173);
            this.groupBox4.TabIndex = 52;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Burst";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(40, 143);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 21);
            this.label10.TabIndex = 56;
            this.label10.Text = "PW";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(4, 143);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(43, 21);
            this.label12.TabIndex = 55;
            this.label12.Text = "delay";
            // 
            // DelayPw
            // 
            this.DelayPw.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DelayPw.Location = new System.Drawing.Point(41, 19);
            this.DelayPw.Minimum = 2;
            this.DelayPw.Name = "DelayPw";
            this.DelayPw.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.DelayPw.Size = new System.Drawing.Size(42, 124);
            this.DelayPw.SmallChange = 20;
            this.DelayPw.TabIndex = 50;
            this.DelayPw.Value = 5;
            this.DelayPw.Scroll += new System.EventHandler(this.SlidersScroll);
            this.DelayPw.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SetDelay);
            // 
            // DelayTime
            // 
            this.DelayTime.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DelayTime.Location = new System.Drawing.Point(0, 19);
            this.DelayTime.Maximum = 500;
            this.DelayTime.Name = "DelayTime";
            this.DelayTime.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.DelayTime.Size = new System.Drawing.Size(42, 124);
            this.DelayTime.TabIndex = 49;
            this.DelayTime.TickFrequency = 50;
            this.DelayTime.Value = 100;
            this.DelayTime.Scroll += new System.EventHandler(this.SlidersScroll);
            this.DelayTime.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SetDelay);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.SimmerPw);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.SimmerRate);
            this.groupBox3.Location = new System.Drawing.Point(62, 253);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(91, 173);
            this.groupBox3.TabIndex = 51;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Simmer";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(38, 143);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 21);
            this.label11.TabIndex = 54;
            this.label11.Text = "PW";
            // 
            // SimmerPw
            // 
            this.SimmerPw.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SimmerPw.Location = new System.Drawing.Point(41, 19);
            this.SimmerPw.Maximum = 200;
            this.SimmerPw.Minimum = 10;
            this.SimmerPw.Name = "SimmerPw";
            this.SimmerPw.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.SimmerPw.Size = new System.Drawing.Size(42, 124);
            this.SimmerPw.SmallChange = 5;
            this.SimmerPw.TabIndex = 53;
            this.SimmerPw.TickFrequency = 50;
            this.SimmerPw.Value = 100;
            this.SimmerPw.Scroll += new System.EventHandler(this.SlidersScroll);
            this.SimmerPw.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SetPFMSimmercommand);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(-1, 143);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 21);
            this.label9.TabIndex = 52;
            this.label9.Text = "rate";
            // 
            // SimmerRate
            // 
            this.SimmerRate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SimmerRate.Location = new System.Drawing.Point(2, 19);
            this.SimmerRate.Maximum = 60;
            this.SimmerRate.Minimum = 25;
            this.SimmerRate.Name = "SimmerRate";
            this.SimmerRate.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.SimmerRate.Size = new System.Drawing.Size(42, 124);
            this.SimmerRate.SmallChange = 5;
            this.SimmerRate.TabIndex = 51;
            this.SimmerRate.TickFrequency = 5;
            this.SimmerRate.Value = 50;
            this.SimmerRate.Scroll += new System.EventHandler(this.SlidersScroll);
            this.SimmerRate.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SetPFMSimmercommand);
            // 
            // aGauge1
            // 
            this.aGauge1.BackColor = System.Drawing.SystemColors.Control;
            this.aGauge1.BaseArcColor = System.Drawing.Color.Gray;
            this.aGauge1.BaseArcRadius = 80;
            this.aGauge1.BaseArcStart = 90;
            this.aGauge1.BaseArcSweep = 90;
            this.aGauge1.BaseArcWidth = 2;
            this.aGauge1.Cap_Idx = ((byte)(0));
            this.aGauge1.CapColors = new System.Drawing.Color[] {
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black};
            this.aGauge1.CapPosition = new System.Drawing.Point(10, 10);
            this.aGauge1.CapsPosition = new System.Drawing.Point[] {
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10)};
            this.aGauge1.CapsText = new string[] {
        "",
        "",
        "",
        "",
        ""};
            this.aGauge1.CapText = "";
            this.aGauge1.Center = new System.Drawing.Point(110, 20);
            this.aGauge1.Location = new System.Drawing.Point(6, 9);
            this.aGauge1.MaxValue = 1000F;
            this.aGauge1.MinValue = 0F;
            this.aGauge1.Name = "aGauge1";
            this.aGauge1.NeedleColor1 = AGaugeApp.AGauge.NeedleColorEnum.Gray;
            this.aGauge1.NeedleColor2 = System.Drawing.Color.Maroon;
            this.aGauge1.NeedleRadius = 65;
            this.aGauge1.NeedleType = 0;
            this.aGauge1.NeedleWidth = 3;
            this.aGauge1.Range_Idx = ((byte)(2));
            this.aGauge1.RangeColor = System.Drawing.Color.DarkSalmon;
            this.aGauge1.RangeEnabled = true;
            this.aGauge1.RangeEndValue = 1000F;
            this.aGauge1.RangeInnerRadius = 30;
            this.aGauge1.RangeOuterRadius = 80;
            this.aGauge1.RangesColor = new System.Drawing.Color[] {
        System.Drawing.Color.PaleGoldenrod,
        System.Drawing.Color.YellowGreen,
        System.Drawing.Color.DarkSalmon,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.aGauge1.RangesEnabled = new bool[] {
        true,
        true,
        true,
        false,
        false};
            this.aGauge1.RangesEndValue = new float[] {
        400F,
        800F,
        1000F,
        0F,
        0F};
            this.aGauge1.RangesInnerRadius = new int[] {
        30,
        30,
        30,
        70,
        70};
            this.aGauge1.RangesOuterRadius = new int[] {
        80,
        80,
        80,
        80,
        80};
            this.aGauge1.RangesStartValue = new float[] {
        0F,
        400F,
        800F,
        0F,
        0F};
            this.aGauge1.RangeStartValue = 800F;
            this.aGauge1.ScaleLinesInterColor = System.Drawing.Color.Black;
            this.aGauge1.ScaleLinesInterInnerRadius = 73;
            this.aGauge1.ScaleLinesInterOuterRadius = 80;
            this.aGauge1.ScaleLinesInterWidth = 1;
            this.aGauge1.ScaleLinesMajorColor = System.Drawing.Color.Black;
            this.aGauge1.ScaleLinesMajorInnerRadius = 70;
            this.aGauge1.ScaleLinesMajorOuterRadius = 80;
            this.aGauge1.ScaleLinesMajorStepValue = 200F;
            this.aGauge1.ScaleLinesMajorWidth = 2;
            this.aGauge1.ScaleLinesMinorColor = System.Drawing.Color.Gray;
            this.aGauge1.ScaleLinesMinorInnerRadius = 75;
            this.aGauge1.ScaleLinesMinorNumOf = 9;
            this.aGauge1.ScaleLinesMinorOuterRadius = 80;
            this.aGauge1.ScaleLinesMinorWidth = 1;
            this.aGauge1.ScaleNumbersColor = System.Drawing.Color.Black;
            this.aGauge1.ScaleNumbersFormat = null;
            this.aGauge1.ScaleNumbersRadius = 95;
            this.aGauge1.ScaleNumbersRotation = 0;
            this.aGauge1.ScaleNumbersStartScaleLine = 0;
            this.aGauge1.ScaleNumbersStepScaleLines = 100;
            this.aGauge1.Size = new System.Drawing.Size(154, 133);
            this.aGauge1.TabIndex = 34;
            this.aGauge1.Text = "aGauge1";
            this.aGauge1.Value = 0F;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Xlap4);
            this.groupBox2.Controls.Add(this.Xlap2);
            this.groupBox2.Controls.Add(this.Xlap1);
            this.groupBox2.Location = new System.Drawing.Point(160, 119);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(83, 51);
            this.groupBox2.TabIndex = 41;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Overlap _-_";
            // 
            // Xlap4
            // 
            this.Xlap4.AutoSize = true;
            this.Xlap4.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Xlap4.Checked = true;
            this.Xlap4.Location = new System.Drawing.Point(52, 15);
            this.Xlap4.Name = "Xlap4";
            this.Xlap4.Size = new System.Drawing.Size(17, 30);
            this.Xlap4.TabIndex = 47;
            this.Xlap4.TabStop = true;
            this.Xlap4.Text = "4";
            this.Xlap4.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Xlap4.UseVisualStyleBackColor = true;
            this.Xlap4.CheckedChanged += new System.EventHandler(this.XlapChanged);
            // 
            // Xlap2
            // 
            this.Xlap2.AutoSize = true;
            this.Xlap2.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Xlap2.Location = new System.Drawing.Point(29, 15);
            this.Xlap2.Name = "Xlap2";
            this.Xlap2.Size = new System.Drawing.Size(17, 30);
            this.Xlap2.TabIndex = 46;
            this.Xlap2.Text = "2";
            this.Xlap2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Xlap2.UseVisualStyleBackColor = true;
            this.Xlap2.CheckedChanged += new System.EventHandler(this.XlapChanged);
            // 
            // Xlap1
            // 
            this.Xlap1.AutoSize = true;
            this.Xlap1.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Xlap1.Location = new System.Drawing.Point(6, 15);
            this.Xlap1.Name = "Xlap1";
            this.Xlap1.Size = new System.Drawing.Size(17, 30);
            this.Xlap1.TabIndex = 45;
            this.Xlap1.Text = "1";
            this.Xlap1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Xlap1.UseVisualStyleBackColor = true;
            this.Xlap1.CheckedChanged += new System.EventHandler(this.XlapChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Simmer2);
            this.groupBox1.Controls.Add(this.Simmer1);
            this.groupBox1.Location = new System.Drawing.Point(160, 176);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(82, 52);
            this.groupBox1.TabIndex = 40;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Simmer";
            // 
            // Simmer2
            // 
            this.Simmer2.AutoSize = true;
            this.Simmer2.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Simmer2.Location = new System.Drawing.Point(52, 14);
            this.Simmer2.Name = "Simmer2";
            this.Simmer2.Size = new System.Drawing.Size(17, 31);
            this.Simmer2.TabIndex = 26;
            this.Simmer2.Text = "2";
            this.Simmer2.UseVisualStyleBackColor = true;
            this.Simmer2.CheckedChanged += new System.EventHandler(this.Simmer_CheckedChanged);
            // 
            // Simmer1
            // 
            this.Simmer1.AutoSize = true;
            this.Simmer1.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Simmer1.Location = new System.Drawing.Point(6, 14);
            this.Simmer1.Name = "Simmer1";
            this.Simmer1.Size = new System.Drawing.Size(17, 31);
            this.Simmer1.TabIndex = 25;
            this.Simmer1.Text = "1";
            this.Simmer1.UseVisualStyleBackColor = true;
            this.Simmer1.CheckedChanged += new System.EventHandler(this.Simmer_CheckedChanged);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 23);
            this.label8.TabIndex = 46;
            // 
            // TriggerButton
            // 
            this.TriggerButton.BackColor = System.Drawing.Color.DarkRed;
            this.TriggerButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.TriggerButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.TriggerButton.ForeColor = System.Drawing.Color.Gold;
            this.TriggerButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TriggerButton.Location = new System.Drawing.Point(160, 18);
            this.TriggerButton.Name = "TriggerButton";
            this.TriggerButton.Size = new System.Drawing.Size(82, 45);
            this.TriggerButton.TabIndex = 14;
            this.TriggerButton.Text = "Trigger";
            this.TriggerButton.UseVisualStyleBackColor = false;
            this.TriggerButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TriggerButton_MouseDown);
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.Window;
            this.textBox2.Location = new System.Drawing.Point(160, 69);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(82, 20);
            this.textBox2.TabIndex = 20;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox1.Location = new System.Drawing.Point(160, 94);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(82, 20);
            this.textBox1.TabIndex = 19;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.ConnectMenu,
            this.ScopeMenu,
            this.TempMenu,
            this.eCEnable,
            this.qSPToolStripMenuItem,
            this.vLPToolStripMenuItem,
            this.sPToolStripMenuItem,
            this.lPToolStripMenuItem,
            this.mSPToolStripMenuItem,
            this.sSPToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(870, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // FileMenu
            // 
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(35, 20);
            this.FileMenu.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitMenu_Click);
            // 
            // ConnectMenu
            // 
            this.ConnectMenu.Name = "ConnectMenu";
            this.ConnectMenu.Size = new System.Drawing.Size(59, 20);
            this.ConnectMenu.Text = "Connect";
            this.ConnectMenu.DropDownOpening += new System.EventHandler(this.ConnectMenu_DropDownOpening);
            this.ConnectMenu.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ConnectMenu_DropDownItemClicked);
            this.ConnectMenu.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ConnectMenu_MouseDown);
            // 
            // ScopeMenu
            // 
            this.ScopeMenu.Name = "ScopeMenu";
            this.ScopeMenu.Size = new System.Drawing.Size(48, 20);
            this.ScopeMenu.Text = "Scope";
            this.ScopeMenu.Click += new System.EventHandler(this.ScopeMenu_Click);
            // 
            // TempMenu
            // 
            this.TempMenu.Name = "TempMenu";
            this.TempMenu.Size = new System.Drawing.Size(45, 20);
            this.TempMenu.Text = "Temp";
            this.TempMenu.Click += new System.EventHandler(this.TempMenu_Click);
            // 
            // eCEnable
            // 
            this.eCEnable.Name = "eCEnable";
            this.eCEnable.Size = new System.Drawing.Size(67, 20);
            this.eCEnable.Text = "EC enable";
            this.eCEnable.Click += new System.EventHandler(this.eCEnable_Click);
            // 
            // qSPToolStripMenuItem
            // 
            this.qSPToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.qSPToolStripMenuItem.BackColor = System.Drawing.Color.Goldenrod;
            this.qSPToolStripMenuItem.Name = "qSPToolStripMenuItem";
            this.qSPToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.qSPToolStripMenuItem.Text = "QSP";
            this.qSPToolStripMenuItem.Click += new System.EventHandler(this.qSPToolStripMenuItem_Click);
            // 
            // vLPToolStripMenuItem
            // 
            this.vLPToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.vLPToolStripMenuItem.BackColor = System.Drawing.Color.Gold;
            this.vLPToolStripMenuItem.Name = "vLPToolStripMenuItem";
            this.vLPToolStripMenuItem.Size = new System.Drawing.Size(36, 20);
            this.vLPToolStripMenuItem.Text = "VLP";
            this.vLPToolStripMenuItem.Click += new System.EventHandler(this.vLPToolStripMenuItem_Click);
            // 
            // sPToolStripMenuItem
            // 
            this.sPToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.sPToolStripMenuItem.BackColor = System.Drawing.Color.LemonChiffon;
            this.sPToolStripMenuItem.Name = "sPToolStripMenuItem";
            this.sPToolStripMenuItem.Size = new System.Drawing.Size(31, 20);
            this.sPToolStripMenuItem.Text = "SP";
            this.sPToolStripMenuItem.Click += new System.EventHandler(this.sPToolStripMenuItem_Click);
            // 
            // lPToolStripMenuItem
            // 
            this.lPToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lPToolStripMenuItem.BackColor = System.Drawing.Color.Khaki;
            this.lPToolStripMenuItem.Name = "lPToolStripMenuItem";
            this.lPToolStripMenuItem.Size = new System.Drawing.Size(30, 20);
            this.lPToolStripMenuItem.Text = "LP";
            this.lPToolStripMenuItem.Click += new System.EventHandler(this.lPToolStripMenuItem_Click);
            // 
            // mSPToolStripMenuItem
            // 
            this.mSPToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.mSPToolStripMenuItem.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.mSPToolStripMenuItem.Name = "mSPToolStripMenuItem";
            this.mSPToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.mSPToolStripMenuItem.Text = "MSP";
            this.mSPToolStripMenuItem.Click += new System.EventHandler(this.mSPToolStripMenuItem_Click);
            // 
            // sSPToolStripMenuItem
            // 
            this.sSPToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.sSPToolStripMenuItem.BackColor = System.Drawing.Color.DarkKhaki;
            this.sSPToolStripMenuItem.Name = "sSPToolStripMenuItem";
            this.sSPToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.sSPToolStripMenuItem.Text = "SSP";
            this.sSPToolStripMenuItem.Click += new System.EventHandler(this.sSPToolStripMenuItem_Click);
            // 
            // com
            // 
            this.com.BaudRate = 921600;
            this.com.ReadBufferSize = 32786;
            this.com.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialRxEvent);
            // 
            // StatusList
            // 
            this.StatusList.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.StatusList.BackColor = System.Drawing.SystemColors.Window;
            this.StatusList.Dock = System.Windows.Forms.DockStyle.Left;
            this.StatusList.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusList.ForeColor = System.Drawing.SystemColors.WindowText;
            this.StatusList.GridLines = true;
            this.StatusList.Location = new System.Drawing.Point(0, 24);
            this.StatusList.Name = "StatusList";
            this.StatusList.Size = new System.Drawing.Size(316, 420);
            this.StatusList.TabIndex = 4;
            this.StatusList.UseCompatibleStateImageBehavior = false;
            this.StatusList.View = System.Windows.Forms.View.Details;
            // 
            // IgbtTemp
            // 
            this.IgbtTemp.Enabled = true;
            this.IgbtTemp.Tick += new System.EventHandler(this.IgbtTemp_Tick);
            // 
            // Delay
            // 
            this.Delay.Interval = 20;
            this.Delay.Tick += new System.EventHandler(this.Delay_Tick);
            // 
            // SetFrame
            // 
            this.SetFrame.Controls.Add(this.trackT);
            this.SetFrame.Controls.Add(this.trackU);
            this.SetFrame.Controls.Add(this.label2);
            this.SetFrame.Controls.Add(this.label1);
            this.SetFrame.Dock = System.Windows.Forms.DockStyle.Left;
            this.SetFrame.Location = new System.Drawing.Point(316, 24);
            this.SetFrame.Name = "SetFrame";
            this.SetFrame.Size = new System.Drawing.Size(118, 420);
            this.SetFrame.TabIndex = 24;
            this.SetFrame.TabStop = false;
            this.SetFrame.Text = "Parameters";
            // 
            // trackT
            // 
            this.trackT.Cursor = System.Windows.Forms.Cursors.Hand;
            this.trackT.Location = new System.Drawing.Point(66, 18);
            this.trackT.Maximum = 2000;
            this.trackT.Minimum = 20;
            this.trackT.Name = "trackT";
            this.trackT.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackT.Size = new System.Drawing.Size(42, 346);
            this.trackT.TabIndex = 22;
            this.trackT.TickFrequency = 100;
            this.trackT.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackT.Value = 100;
            this.trackT.Scroll += new System.EventHandler(this.SlidersScroll);
            this.trackT.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SetPFMcommand);
            // 
            // trackU
            // 
            this.trackU.Cursor = System.Windows.Forms.Cursors.Hand;
            this.trackU.Location = new System.Drawing.Point(18, 19);
            this.trackU.Maximum = 800;
            this.trackU.Name = "trackU";
            this.trackU.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackU.Size = new System.Drawing.Size(42, 346);
            this.trackU.TabIndex = 21;
            this.trackU.TickFrequency = 100;
            this.trackU.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackU.Scroll += new System.EventHandler(this.SlidersScroll);
            this.trackU.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SetPFMcommand);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(63, 376);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "     T";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 376);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "     U";
            // 
            // ResetFrame
            // 
            this.ResetFrame.Controls.Add(this.label5);
            this.ResetFrame.Controls.Add(this.label4);
            this.ResetFrame.Controls.Add(this.label3);
            this.ResetFrame.Controls.Add(this.Length);
            this.ResetFrame.Controls.Add(this.Burst);
            this.ResetFrame.Controls.Add(this.Period);
            this.ResetFrame.Controls.Add(this.ResetBurst);
            this.ResetFrame.Dock = System.Windows.Forms.DockStyle.Top;
            this.ResetFrame.Location = new System.Drawing.Point(434, 24);
            this.ResetFrame.Name = "ResetFrame";
            this.ResetFrame.Size = new System.Drawing.Size(173, 324);
            this.ResetFrame.TabIndex = 25;
            this.ResetFrame.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(111, 300);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Length";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(63, 300);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Burst";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 300);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Period";
            // 
            // Length
            // 
            this.Length.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Length.Location = new System.Drawing.Point(114, 19);
            this.Length.Minimum = 1;
            this.Length.Name = "Length";
            this.Length.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.Length.Size = new System.Drawing.Size(42, 278);
            this.Length.TabIndex = 9;
            this.Length.Value = 1;
            this.Length.Scroll += new System.EventHandler(this.SlidersScroll);
            this.Length.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ResetPFMcommand);
            // 
            // Burst
            // 
            this.Burst.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Burst.Location = new System.Drawing.Point(66, 18);
            this.Burst.Maximum = 20;
            this.Burst.Minimum = 1;
            this.Burst.Name = "Burst";
            this.Burst.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.Burst.Size = new System.Drawing.Size(42, 278);
            this.Burst.TabIndex = 8;
            this.Burst.Value = 1;
            this.Burst.Scroll += new System.EventHandler(this.SlidersScroll);
            this.Burst.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ResetPFMcommand);
            // 
            // Period
            // 
            this.Period.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Period.Location = new System.Drawing.Point(18, 19);
            this.Period.Maximum = 1000;
            this.Period.Minimum = 5;
            this.Period.Name = "Period";
            this.Period.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.Period.Size = new System.Drawing.Size(42, 278);
            this.Period.TabIndex = 7;
            this.Period.TickFrequency = 100;
            this.Period.Value = 10;
            this.Period.Scroll += new System.EventHandler(this.SlidersScroll);
            this.Period.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ResetPFMcommand);
            // 
            // ResetBurst
            // 
            this.ResetBurst.Location = new System.Drawing.Point(66, 19);
            this.ResetBurst.Name = "ResetBurst";
            this.ResetBurst.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.ResetBurst.Size = new System.Drawing.Size(42, 278);
            this.ResetBurst.TabIndex = 8;
            // 
            // BootBar
            // 
            this.BootBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BootBar.Location = new System.Drawing.Point(434, 348);
            this.BootBar.Name = "BootBar";
            this.BootBar.Size = new System.Drawing.Size(173, 96);
            this.BootBar.TabIndex = 33;
            // 
            // Login
            // 
            this.Login.Enabled = true;
            this.Login.Interval = 500;
            this.Login.Tick += new System.EventHandler(this.Login_Tick);
            // 
            // Pbar
            // 
            this.Pbar.Location = new System.Drawing.Point(434, 421);
            this.Pbar.Name = "Pbar";
            this.Pbar.Size = new System.Drawing.Size(173, 23);
            this.Pbar.TabIndex = 34;
            // 
            // shaped
            // 
            this.shaped.AutoSize = true;
            this.shaped.Location = new System.Drawing.Point(440, 378);
            this.shaped.Name = "shaped";
            this.shaped.Size = new System.Drawing.Size(63, 17);
            this.shaped.TabIndex = 35;
            this.shaped.Text = "Shaped";
            this.shaped.UseVisualStyleBackColor = true;
            this.shaped.CheckedChanged += new System.EventHandler(this.shaped_CheckedChanged);
            // 
            // charger
            // 
            this.charger.AutoSize = true;
            this.charger.Location = new System.Drawing.Point(440, 355);
            this.charger.Name = "charger";
            this.charger.Size = new System.Drawing.Size(63, 17);
            this.charger.TabIndex = 36;
            this.charger.Text = "Charger";
            this.charger.UseVisualStyleBackColor = true;
            this.charger.CheckedChanged += new System.EventHandler(this.charger_CheckedChanged);
            // 
            // channel1
            // 
            this.channel1.AutoSize = true;
            this.channel1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.channel1.Location = new System.Drawing.Point(570, 354);
            this.channel1.Name = "channel1";
            this.channel1.Size = new System.Drawing.Size(31, 17);
            this.channel1.TabIndex = 37;
            this.channel1.Text = "1";
            this.channel1.UseVisualStyleBackColor = true;
            this.channel1.CheckedChanged += new System.EventHandler(this.channelChanged);
            // 
            // channel2
            // 
            this.channel2.AutoSize = true;
            this.channel2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.channel2.Location = new System.Drawing.Point(570, 400);
            this.channel2.Name = "channel2";
            this.channel2.Size = new System.Drawing.Size(31, 17);
            this.channel2.TabIndex = 38;
            this.channel2.Text = "2";
            this.channel2.UseVisualStyleBackColor = true;
            this.channel2.CheckedChanged += new System.EventHandler(this.channelChanged);
            // 
            // channel
            // 
            this.channel.AutoSize = true;
            this.channel.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.channel.Checked = true;
            this.channel.Location = new System.Drawing.Point(554, 377);
            this.channel.Name = "channel";
            this.channel.Size = new System.Drawing.Size(47, 17);
            this.channel.TabIndex = 39;
            this.channel.TabStop = true;
            this.channel.Text = "Both";
            this.channel.UseVisualStyleBackColor = true;
            this.channel.CheckedChanged += new System.EventHandler(this.channelChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(870, 444);
            this.Controls.Add(this.channel);
            this.Controls.Add(this.channel2);
            this.Controls.Add(this.channel1);
            this.Controls.Add(this.charger);
            this.Controls.Add(this.shaped);
            this.Controls.Add(this.Pbar);
            this.Controls.Add(this.BootBar);
            this.Controls.Add(this.ResetFrame);
            this.Controls.Add(this.SetFrame);
            this.Controls.Add(this.StatusList);
            this.Controls.Add(this.ButtonsFrame);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "PFM 6";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.ButtonsFrame.ResumeLayout(false);
            this.ButtonsFrame.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DAC1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DAC2)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HV)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DelayPw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelayTime)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SimmerPw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SimmerRate)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.SetFrame.ResumeLayout(false);
            this.SetFrame.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackU)).EndInit();
            this.ResetFrame.ResumeLayout(false);
            this.ResetFrame.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Length)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Burst)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Period)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResetBurst)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox ButtonsFrame;
        private System.Windows.Forms.Button TriggerButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ConnectMenu;
        private System.Windows.Forms.ToolStripMenuItem ScopeMenu;
        private System.IO.Ports.SerialPort com;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ListView StatusList;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Timer IgbtTemp;
        private System.Windows.Forms.Timer Delay;
        private System.Windows.Forms.GroupBox SetFrame;
        private System.Windows.Forms.TrackBar trackT;
        private System.Windows.Forms.TrackBar trackU;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox ResetFrame;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar Length;
        private System.Windows.Forms.TrackBar Burst;
        private System.Windows.Forms.TrackBar Period;
        private System.Windows.Forms.TrackBar ResetBurst;
        private System.Windows.Forms.ToolStripMenuItem TempMenu;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ProgressBar BootBar;
        private System.Windows.Forms.Timer Login;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox Simmer2;
        private System.Windows.Forms.CheckBox Simmer1;
        private System.Windows.Forms.RadioButton Xlap4;
        private System.Windows.Forms.RadioButton Xlap2;
        private System.Windows.Forms.RadioButton Xlap1;
        private AGaugeApp.AGauge aGauge1;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private AGaugeApp.AGauge aGauge2;
        private System.Windows.Forms.ToolStripMenuItem eCEnable;
        private System.Windows.Forms.TrackBar DAC1;
        private System.Windows.Forms.TrackBar DAC2;
        private System.Windows.Forms.ProgressBar Pbar;
        private System.Windows.Forms.ToolStripMenuItem sSPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mSPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vLPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem qSPToolStripMenuItem;
        private System.Windows.Forms.CheckBox shaped;
        private System.Windows.Forms.CheckBox charger;
        private System.Windows.Forms.RadioButton channel1;
        private System.Windows.Forms.RadioButton channel2;
        private System.Windows.Forms.RadioButton channel;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TrackBar DelayPw;
        private System.Windows.Forms.TrackBar DelayTime;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TrackBar SimmerPw;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TrackBar SimmerRate;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TrackBar HV;
        private System.Windows.Forms.GroupBox groupBox6;
    }
}

