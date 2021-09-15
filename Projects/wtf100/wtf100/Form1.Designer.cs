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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Reset = new System.Windows.Forms.ToolStripMenuItem();
            this.setupMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.lfdelayMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.fontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.baudrateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.iapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.utilsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pulseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.valveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pyroToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.net = new System.Windows.Forms.ToolStripMenuItem();
            this.whatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.com = new System.IO.Ports.SerialPort(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.TxTimer = new System.Windows.Forms.Timer(this.components);
            this.text = new System.Windows.Forms.RichTextBox();
            this.bar = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.Login = new System.Windows.Forms.Timer(this.components);
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.connectToolStripMenuItem,
            this.Reset,
            this.setupMenu,
            this.iapToolStripMenuItem,
            this.utilsToolStripMenuItem,
            this.testToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(663, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem1});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(140, 22);
            this.toolStripMenuItem1.Text = "Send to host";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.Send2Host_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.exitToolStripMenuItem.Text = "Log";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.logToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(137, 6);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(140, 22);
            this.exitToolStripMenuItem1.Text = "Exit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.connectToolStripMenuItem.Text = "Connect";
            this.connectToolStripMenuItem.DropDownOpening += new System.EventHandler(this.Connect_Opening);
            this.connectToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.Connect_Clicked);
            this.connectToolStripMenuItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Connect_MouseDown);
            // 
            // Reset
            // 
            this.Reset.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.Reset.BackColor = System.Drawing.Color.Transparent;
            this.Reset.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.Reset.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Reset.Name = "Reset";
            this.Reset.Size = new System.Drawing.Size(52, 20);
            this.Reset.Text = "Reset";
            this.Reset.Click += new System.EventHandler(this.Reset_Click);
            // 
            // setupMenu
            // 
            this.setupMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lfdelayMenu,
            this.fontToolStripMenuItem,
            this.baudrateToolStripMenuItem});
            this.setupMenu.Name = "setupMenu";
            this.setupMenu.Size = new System.Drawing.Size(49, 20);
            this.setupMenu.Text = "Setup";
            // 
            // lfdelayMenu
            // 
            this.lfdelayMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1});
            this.lfdelayMenu.Name = "lfdelayMenu";
            this.lfdelayMenu.Size = new System.Drawing.Size(121, 22);
            this.lfdelayMenu.Text = "LF delay";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 23);
            this.toolStripTextBox1.Text = "10";
            this.toolStripTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LFdelay_Enter);
            // 
            // fontToolStripMenuItem
            // 
            this.fontToolStripMenuItem.Name = "fontToolStripMenuItem";
            this.fontToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.fontToolStripMenuItem.Text = "Font";
            this.fontToolStripMenuItem.Click += new System.EventHandler(this.Font_Click);
            // 
            // baudrateToolStripMenuItem
            // 
            this.baudrateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.toolStripMenuItem7});
            this.baudrateToolStripMenuItem.Name = "baudrateToolStripMenuItem";
            this.baudrateToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.baudrateToolStripMenuItem.Text = "Baudrate";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(110, 22);
            this.toolStripMenuItem4.Text = "9600";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.Baudrate_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(110, 22);
            this.toolStripMenuItem5.Text = "19200";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.Baudrate_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(110, 22);
            this.toolStripMenuItem6.Text = "115200";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.Baudrate_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(110, 22);
            this.toolStripMenuItem7.Text = "921600";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.Baudrate_Click);
            // 
            // iapToolStripMenuItem
            // 
            this.iapToolStripMenuItem.Name = "iapToolStripMenuItem";
            this.iapToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.iapToolStripMenuItem.Text = "Iap";
            this.iapToolStripMenuItem.Click += new System.EventHandler(this.iapToolStripMenuItem_Click);
            // 
            // utilsToolStripMenuItem
            // 
            this.utilsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pulseToolStripMenuItem,
            this.valveToolStripMenuItem,
            this.pyroToolStripMenuItem,
            this.net});
            this.utilsToolStripMenuItem.Name = "utilsToolStripMenuItem";
            this.utilsToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.utilsToolStripMenuItem.Text = "Utils";
            // 
            // pulseToolStripMenuItem
            // 
            this.pulseToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pulseToolStripMenuItem.Image")));
            this.pulseToolStripMenuItem.Name = "pulseToolStripMenuItem";
            this.pulseToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.pulseToolStripMenuItem.Text = "Pulse";
            this.pulseToolStripMenuItem.Click += new System.EventHandler(this.pulseToolStripMenuItem_Click);
            // 
            // valveToolStripMenuItem
            // 
            this.valveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("valveToolStripMenuItem.Image")));
            this.valveToolStripMenuItem.Name = "valveToolStripMenuItem";
            this.valveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.valveToolStripMenuItem.Text = "Valve";
            this.valveToolStripMenuItem.Click += new System.EventHandler(this.valveToolStripMenuItem_Click);
            // 
            // pyroToolStripMenuItem
            // 
            this.pyroToolStripMenuItem.Name = "pyroToolStripMenuItem";
            this.pyroToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.pyroToolStripMenuItem.Text = "Shape";
            this.pyroToolStripMenuItem.Click += new System.EventHandler(this.pyroToolStripMenuItem_Click);
            // 
            // net
            // 
            this.net.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.whatToolStripMenuItem,
            this.pingToolStripMenuItem});
            this.net.Image = ((System.Drawing.Image)(resources.GetObject("net.Image")));
            this.net.Name = "net";
            this.net.Size = new System.Drawing.Size(152, 22);
            this.net.Text = "network";
            this.net.Click += new System.EventHandler(this.netToolStripMenuItem_Click);
            // 
            // whatToolStripMenuItem
            // 
            this.whatToolStripMenuItem.Name = "whatToolStripMenuItem";
            this.whatToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.whatToolStripMenuItem.Text = "what";
            this.whatToolStripMenuItem.Click += new System.EventHandler(this.whatToolStripMenuItem_Click);
            // 
            // pingToolStripMenuItem
            // 
            this.pingToolStripMenuItem.Name = "pingToolStripMenuItem";
            this.pingToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.pingToolStripMenuItem.Text = "ping";
            this.pingToolStripMenuItem.Click += new System.EventHandler(this.pingToolStripMenuItem_Click);
            // 
            // com
            // 
            this.com.BaudRate = 921600;
            this.com.DiscardNull = true;
            this.com.ReadBufferSize = 32786;
            this.com.WriteBufferSize = 32786;
            this.com.ErrorReceived += new System.IO.Ports.SerialErrorReceivedEventHandler(this.com_ErrorReceived);
            this.com.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.comRxEvent);
            // 
            // TxTimer
            // 
            this.TxTimer.Interval = 10;
            this.TxTimer.Tick += new System.EventHandler(this.TxTimer_Tick);
            // 
            // text
            // 
            this.text.Location = new System.Drawing.Point(0, 27);
            this.text.Name = "text";
            this.text.ReadOnly = true;
            this.text.Size = new System.Drawing.Size(99, 106);
            this.text.TabIndex = 4;
            this.text.Text = "";
            // 
            // bar
            // 
            this.bar.Enabled = false;
            this.bar.ForeColor = System.Drawing.Color.Goldenrod;
            this.bar.Location = new System.Drawing.Point(367, 0);
            this.bar.Name = "bar";
            this.bar.Size = new System.Drawing.Size(100, 20);
            this.bar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.bar.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(10, 20);
            this.button1.TabIndex = 7;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Login
            // 
            this.Login.Enabled = true;
            this.Login.Interval = 250;
            this.Login.Tick += new System.EventHandler(this.Login_Tick);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.testToolStripMenuItem.Text = "test";
            this.testToolStripMenuItem.Click += new System.EventHandler(this.testToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ClientSize = new System.Drawing.Size(663, 391);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.bar);
            this.Controls.Add(this.text);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "wtf100";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.IO.Ports.SerialPort com;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem Reset;
        private System.Windows.Forms.Timer TxTimer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem setupMenu;
        private System.Windows.Forms.ToolStripMenuItem fontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lfdelayMenu;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripMenuItem iapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem baudrateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.RichTextBox text;
        private System.Windows.Forms.ProgressBar bar;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem utilsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pulseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem valveToolStripMenuItem;
        private System.Windows.Forms.Timer Login;
        private System.Windows.Forms.ToolStripMenuItem net;
        private System.Windows.Forms.ToolStripMenuItem whatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pyroToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
    }
}

