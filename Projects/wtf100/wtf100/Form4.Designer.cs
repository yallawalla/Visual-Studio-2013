namespace WindowsFormsApplication1
{
    partial class pShape
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(pShape));
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.fNew = new System.Windows.Forms.ToolStripMenuItem();
            this.fOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.fSave = new System.Windows.Forms.ToolStripMenuItem();
            this.fSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.fExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.SetupButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.tgridmenu = new System.Windows.Forms.ToolStripMenuItem();
            this.Tgrid = new System.Windows.Forms.ToolStripTextBox();
            this.ugridmenu = new System.Windows.Forms.ToolStripMenuItem();
            this.Ugrid = new System.Windows.Forms.ToolStripTextBox();
            this.maxtmenu = new System.Windows.Forms.ToolStripMenuItem();
            this.Tmax = new System.Windows.Forms.ToolStripTextBox();
            this.maxumenu = new System.Windows.Forms.ToolStripMenuItem();
            this.Umax = new System.Windows.Forms.ToolStripTextBox();
            this.pwmratemenu = new System.Windows.Forms.ToolStripMenuItem();
            this.pwmrate = new System.Windows.Forms.ToolStripTextBox();
            this.flash1Coeff = new System.Windows.Forms.ToolStripMenuItem();
            this.FlashK1 = new System.Windows.Forms.ToolStripTextBox();
            this.flash2Coeff = new System.Windows.Forms.ToolStripMenuItem();
            this.FlashK2 = new System.Windows.Forms.ToolStripTextBox();
            this.UploadButton = new System.Windows.Forms.ToolStripButton();
            this.ZoomButton = new System.Windows.Forms.ToolStripButton();
            this.SlopeButton = new System.Windows.Forms.ToolStripButton();
            this.HandButton = new System.Windows.Forms.ToolStripButton();
            this.DeleteButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.FilterButton = new System.Windows.Forms.ToolStripButton();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker4 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker5 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker6 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker7 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker8 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker9 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker10 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker11 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker12 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker13 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker14 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker15 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker16 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker17 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker18 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker19 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker20 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker21 = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart
            // 
            this.chart.AccessibleName = "chart1";
            this.chart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisX.InterlacedColor = System.Drawing.Color.Gray;
            chartArea1.AxisX.IsMarginVisible = false;
            chartArea1.AxisX.IsStartedFromZero = false;
            chartArea1.AxisX.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            chartArea1.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            chartArea1.AxisX.MajorGrid.Interval = 0D;
            chartArea1.AxisX.MajorGrid.IntervalOffset = 0D;
            chartArea1.AxisX.MajorGrid.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea1.AxisX.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisX.MinorGrid.Enabled = true;
            chartArea1.AxisX.MinorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea1.AxisX.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisX.ScrollBar.BackColor = System.Drawing.Color.Khaki;
            chartArea1.AxisX.ScrollBar.ButtonColor = System.Drawing.Color.Goldenrod;
            chartArea1.AxisX.ScrollBar.LineColor = System.Drawing.Color.LemonChiffon;
            chartArea1.AxisX.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            chartArea1.AxisY.InterlacedColor = System.Drawing.Color.Gray;
            chartArea1.AxisY.IsMarginVisible = false;
            chartArea1.AxisY.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            chartArea1.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            chartArea1.AxisY.MajorGrid.Interval = 0D;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea1.AxisY.MinorGrid.Enabled = true;
            chartArea1.AxisY.MinorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea1.AxisY.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisY.ScrollBar.BackColor = System.Drawing.Color.Khaki;
            chartArea1.AxisY.ScrollBar.ButtonColor = System.Drawing.Color.Goldenrod;
            chartArea1.AxisY.ScrollBar.LineColor = System.Drawing.Color.LemonChiffon;
            chartArea1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.BorderColor = System.Drawing.Color.White;
            chartArea1.Name = "1";
            this.chart.ChartAreas.Add(chartArea1);
            legend1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            legend1.Name = "Legend1";
            this.chart.Legends.Add(legend1);
            this.chart.Location = new System.Drawing.Point(23, 64);
            this.chart.Name = "chart";
            series1.BorderWidth = 2;
            series1.ChartArea = "1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
            series1.Color = System.Drawing.Color.GreenYellow;
            series1.Legend = "Legend1";
            series1.Name = "p1";
            series2.BorderWidth = 2;
            series2.ChartArea = "1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.Aqua;
            series2.Legend = "Legend1";
            series2.Name = "p2";
            series3.BorderWidth = 2;
            series3.ChartArea = "1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Color = System.Drawing.Color.Gold;
            series3.Legend = "Legend1";
            series3.Name = "p3";
            series4.BorderWidth = 2;
            series4.ChartArea = "1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
            series4.Color = System.Drawing.Color.Crimson;
            series4.Legend = "Legend1";
            series4.Name = "p4";
            this.chart.Series.Add(series1);
            this.chart.Series.Add(series2);
            this.chart.Series.Add(series3);
            this.chart.Series.Add(series4);
            this.chart.Size = new System.Drawing.Size(268, 249);
            this.chart.TabIndex = 1;
            this.chart.Text = "chart1";
            this.chart.AxisViewChanged += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ViewEventArgs>(this.chart_AxisViewChanged);
            this.chart.AxisScrollBarClicked += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ScrollBarEventArgs>(this.chart_AxisScrollBarClicked);
            this.chart.Click += new System.EventHandler(this.chart_Click);
            this.chart.Paint += new System.Windows.Forms.PaintEventHandler(this.chart_Paint);
            this.chart.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.pShape_KeyPress);
            this.chart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chart_MouseDown);
            this.chart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart_MouseMove);
            this.chart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chart_MouseUp);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripSeparator2,
            this.SetupButton,
            this.UploadButton,
            this.ZoomButton,
            this.SlopeButton,
            this.HandButton,
            this.DeleteButton,
            this.toolStripSeparator6,
            this.FilterButton});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(550, 50);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.fNew,
            this.fOpen,
            this.toolStripSeparator3,
            this.fSave,
            this.fSaveAs,
            this.toolStripSeparator4,
            this.fExit});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(45, 47);
            this.toolStripDropDownButton1.Text = "File";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AutoSize = false;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(110, 25);
            // 
            // fNew
            // 
            this.fNew.Name = "fNew";
            this.fNew.Size = new System.Drawing.Size(113, 22);
            this.fNew.Text = "New";
            this.fNew.Click += new System.EventHandler(this.fNew_Click);
            // 
            // fOpen
            // 
            this.fOpen.Name = "fOpen";
            this.fOpen.Size = new System.Drawing.Size(113, 22);
            this.fOpen.Text = "Open";
            this.fOpen.Click += new System.EventHandler(this.fOpen_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(110, 6);
            // 
            // fSave
            // 
            this.fSave.Name = "fSave";
            this.fSave.Size = new System.Drawing.Size(113, 22);
            this.fSave.Text = "Save";
            this.fSave.Click += new System.EventHandler(this.fSave_Click);
            // 
            // fSaveAs
            // 
            this.fSaveAs.Name = "fSaveAs";
            this.fSaveAs.Size = new System.Drawing.Size(113, 22);
            this.fSaveAs.Text = "Save As";
            this.fSaveAs.Click += new System.EventHandler(this.fSaveAs_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(110, 6);
            // 
            // fExit
            // 
            this.fExit.Name = "fExit";
            this.fExit.Size = new System.Drawing.Size(113, 22);
            this.fExit.Text = "Exit";
            this.fExit.Click += new System.EventHandler(this.fExit_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AutoSize = false;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(60, 50);
            // 
            // SetupButton
            // 
            this.SetupButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SetupButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tgridmenu,
            this.ugridmenu,
            this.maxtmenu,
            this.maxumenu,
            this.pwmratemenu,
            this.flash1Coeff,
            this.flash2Coeff});
            this.SetupButton.Image = ((System.Drawing.Image)(resources.GetObject("SetupButton.Image")));
            this.SetupButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SetupButton.Name = "SetupButton";
            this.SetupButton.Size = new System.Drawing.Size(45, 47);
            this.SetupButton.Text = "toolStripDropDownButton1";
            this.SetupButton.ToolTipText = "Setup";
            // 
            // tgridmenu
            // 
            this.tgridmenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Tgrid});
            this.tgridmenu.Name = "tgridmenu";
            this.tgridmenu.Size = new System.Drawing.Size(183, 22);
            this.tgridmenu.Text = "Time grid               (uS)";
            // 
            // Tgrid
            // 
            this.Tgrid.Name = "Tgrid";
            this.Tgrid.Size = new System.Drawing.Size(100, 21);
            this.Tgrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBox_KeyDown);
            // 
            // ugridmenu
            // 
            this.ugridmenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Ugrid});
            this.ugridmenu.Name = "ugridmenu";
            this.ugridmenu.Size = new System.Drawing.Size(183, 22);
            this.ugridmenu.Text = "Voltage grid           (V)";
            // 
            // Ugrid
            // 
            this.Ugrid.Name = "Ugrid";
            this.Ugrid.Size = new System.Drawing.Size(100, 21);
            this.Ugrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBox_KeyDown);
            // 
            // maxtmenu
            // 
            this.maxtmenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Tmax});
            this.maxtmenu.Name = "maxtmenu";
            this.maxtmenu.Size = new System.Drawing.Size(183, 22);
            this.maxtmenu.Text = "Max. time scale     (uS)";
            // 
            // Tmax
            // 
            this.Tmax.Name = "Tmax";
            this.Tmax.Size = new System.Drawing.Size(100, 21);
            this.Tmax.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBox_KeyDown);
            // 
            // maxumenu
            // 
            this.maxumenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Umax});
            this.maxumenu.Name = "maxumenu";
            this.maxumenu.Size = new System.Drawing.Size(183, 22);
            this.maxumenu.Text = "Max. voltage scale (V)";
            // 
            // Umax
            // 
            this.Umax.Name = "Umax";
            this.Umax.Size = new System.Drawing.Size(100, 21);
            this.Umax.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBox_KeyDown);
            // 
            // pwmratemenu
            // 
            this.pwmratemenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pwmrate});
            this.pwmratemenu.Enabled = false;
            this.pwmratemenu.Name = "pwmratemenu";
            this.pwmratemenu.Size = new System.Drawing.Size(183, 22);
            this.pwmratemenu.Text = "PWM rate              (uS)";
            // 
            // pwmrate
            // 
            this.pwmrate.Name = "pwmrate";
            this.pwmrate.Size = new System.Drawing.Size(100, 21);
            this.pwmrate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBox_KeyDown);
            // 
            // flash1Coeff
            // 
            this.flash1Coeff.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FlashK1});
            this.flash1Coeff.Enabled = false;
            this.flash1Coeff.Name = "flash1Coeff";
            this.flash1Coeff.Size = new System.Drawing.Size(183, 22);
            this.flash1Coeff.Text = "Flash 1 coeff.";
            // 
            // FlashK1
            // 
            this.FlashK1.Name = "FlashK1";
            this.FlashK1.Size = new System.Drawing.Size(100, 21);
            this.FlashK1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBox_KeyDown);
            // 
            // flash2Coeff
            // 
            this.flash2Coeff.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FlashK2});
            this.flash2Coeff.Enabled = false;
            this.flash2Coeff.Name = "flash2Coeff";
            this.flash2Coeff.Size = new System.Drawing.Size(183, 22);
            this.flash2Coeff.Text = "Flash 2. coeff.";
            // 
            // FlashK2
            // 
            this.FlashK2.Name = "FlashK2";
            this.FlashK2.Size = new System.Drawing.Size(100, 21);
            this.FlashK2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBox_KeyDown);
            // 
            // UploadButton
            // 
            this.UploadButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.UploadButton.Image = ((System.Drawing.Image)(resources.GetObject("UploadButton.Image")));
            this.UploadButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.UploadButton.Name = "UploadButton";
            this.UploadButton.Size = new System.Drawing.Size(36, 47);
            this.UploadButton.Text = "toolStripButton1";
            this.UploadButton.ToolTipText = "Host upload";
            this.UploadButton.Click += new System.EventHandler(this.UploadButton_Click);
            // 
            // ZoomButton
            // 
            this.ZoomButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ZoomButton.CheckOnClick = true;
            this.ZoomButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ZoomButton.Image = ((System.Drawing.Image)(resources.GetObject("ZoomButton.Image")));
            this.ZoomButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ZoomButton.Name = "ZoomButton";
            this.ZoomButton.Size = new System.Drawing.Size(36, 47);
            this.ZoomButton.Text = "toolStripButton1";
            this.ZoomButton.ToolTipText = "Zoom";
            this.ZoomButton.Click += new System.EventHandler(this.ZoomButton_Click);
            // 
            // SlopeButton
            // 
            this.SlopeButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.SlopeButton.CheckOnClick = true;
            this.SlopeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SlopeButton.Image = ((System.Drawing.Image)(resources.GetObject("SlopeButton.Image")));
            this.SlopeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SlopeButton.Name = "SlopeButton";
            this.SlopeButton.Size = new System.Drawing.Size(36, 47);
            this.SlopeButton.Text = "toolStripButton2";
            this.SlopeButton.ToolTipText = "Slope";
            this.SlopeButton.Click += new System.EventHandler(this.SlopeButton_Click);
            // 
            // HandButton
            // 
            this.HandButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.HandButton.CheckOnClick = true;
            this.HandButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.HandButton.Image = ((System.Drawing.Image)(resources.GetObject("HandButton.Image")));
            this.HandButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.HandButton.Name = "HandButton";
            this.HandButton.Size = new System.Drawing.Size(36, 47);
            this.HandButton.Text = "toolStripButton2";
            this.HandButton.ToolTipText = "Copy";
            this.HandButton.Click += new System.EventHandler(this.HandButton_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.DeleteButton.CheckOnClick = true;
            this.DeleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DeleteButton.Image = ((System.Drawing.Image)(resources.GetObject("DeleteButton.Image")));
            this.DeleteButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(36, 47);
            this.DeleteButton.Text = "toolStripButton2";
            this.DeleteButton.ToolTipText = "Cut";
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator6.AutoSize = false;
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(60, 50);
            // 
            // FilterButton
            // 
            this.FilterButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.FilterButton.CheckOnClick = true;
            this.FilterButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.FilterButton.Image = ((System.Drawing.Image)(resources.GetObject("FilterButton.Image")));
            this.FilterButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FilterButton.Name = "FilterButton";
            this.FilterButton.Size = new System.Drawing.Size(36, 47);
            this.FilterButton.Text = "toolStripButton1";
            this.FilterButton.ToolTipText = "Filter";
            this.FilterButton.Click += new System.EventHandler(this.FilterButton_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pShape
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 493);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.chart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "pShape";
            this.Text = "... ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form4_FormClosing);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.pShape_KeyPress);
            this.Resize += new System.EventHandler(this.Form4_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.ComponentModel.BackgroundWorker backgroundWorker3;
        private System.ComponentModel.BackgroundWorker backgroundWorker4;
        private System.ComponentModel.BackgroundWorker backgroundWorker5;
        private System.ComponentModel.BackgroundWorker backgroundWorker6;
        private System.ComponentModel.BackgroundWorker backgroundWorker7;
        private System.ComponentModel.BackgroundWorker backgroundWorker8;
        private System.ComponentModel.BackgroundWorker backgroundWorker9;
        private System.ComponentModel.BackgroundWorker backgroundWorker10;
        private System.ComponentModel.BackgroundWorker backgroundWorker11;
        private System.ComponentModel.BackgroundWorker backgroundWorker12;
        private System.ComponentModel.BackgroundWorker backgroundWorker13;
        private System.ComponentModel.BackgroundWorker backgroundWorker14;
        private System.ComponentModel.BackgroundWorker backgroundWorker15;
        private System.ComponentModel.BackgroundWorker backgroundWorker16;
        private System.ComponentModel.BackgroundWorker backgroundWorker17;
        private System.ComponentModel.BackgroundWorker backgroundWorker18;
        private System.ComponentModel.BackgroundWorker backgroundWorker19;
        private System.ComponentModel.BackgroundWorker backgroundWorker20;
        private System.ComponentModel.BackgroundWorker backgroundWorker21;
        private System.Windows.Forms.ToolStripButton UploadButton;
        private System.Windows.Forms.ToolStripButton ZoomButton;
        private System.Windows.Forms.ToolStripButton FilterButton;
        private System.Windows.Forms.ToolStripButton DeleteButton;
        private System.Windows.Forms.ToolStripButton HandButton;
        private System.Windows.Forms.ToolStripDropDownButton SetupButton;
        private System.Windows.Forms.ToolStripMenuItem tgridmenu;
        private System.Windows.Forms.ToolStripMenuItem ugridmenu;
        private System.Windows.Forms.ToolStripMenuItem maxtmenu;
        private System.Windows.Forms.ToolStripMenuItem maxumenu;
        private System.Windows.Forms.ToolStripMenuItem pwmratemenu;
        private System.Windows.Forms.ToolStripButton SlopeButton;
        private System.Windows.Forms.ToolStripTextBox Tgrid;
        private System.Windows.Forms.ToolStripTextBox Ugrid;
        private System.Windows.Forms.ToolStripTextBox Tmax;
        private System.Windows.Forms.ToolStripTextBox Umax;
        private System.Windows.Forms.ToolStripTextBox pwmrate;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem fNew;
        private System.Windows.Forms.ToolStripMenuItem fOpen;
        private System.Windows.Forms.ToolStripMenuItem fSave;
        private System.Windows.Forms.ToolStripMenuItem fExit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem fSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem flash1Coeff;
        private System.Windows.Forms.ToolStripTextBox FlashK1;
        private System.Windows.Forms.ToolStripMenuItem flash2Coeff;
        private System.Windows.Forms.ToolStripTextBox FlashK2;
    }
}