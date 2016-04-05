namespace WindowsFormsApplication1
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.VH2O = new System.Windows.Forms.CheckBox();
            this.pAIR = new System.Windows.Forms.TrackBar();
            this.pH2O = new System.Windows.Forms.TrackBar();
            this.t2 = new System.Windows.Forms.TextBox();
            this.t1 = new System.Windows.Forms.TextBox();
            this.t3 = new System.Windows.Forms.TextBox();
            this.a3 = new AGaugeApp.AGauge();
            this.a1 = new AGaugeApp.AGauge();
            this.a2 = new AGaugeApp.AGauge();
            ((System.ComponentModel.ISupportInitialize)(this.pAIR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pH2O)).BeginInit();
            this.SuspendLayout();
            // 
            // VH2O
            // 
            this.VH2O.AutoSize = true;
            this.VH2O.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.VH2O.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.VH2O.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.VH2O.Location = new System.Drawing.Point(441, 238);
            this.VH2O.Name = "VH2O";
            this.VH2O.Size = new System.Drawing.Size(48, 17);
            this.VH2O.TabIndex = 5;
            this.VH2O.Text = "H2O";
            this.VH2O.UseVisualStyleBackColor = false;
            this.VH2O.CheckedChanged += new System.EventHandler(this.VH2O_CheckedChanged);
            // 
            // pAIR
            // 
            this.pAIR.Location = new System.Drawing.Point(520, 359);
            this.pAIR.Maximum = 65000;
            this.pAIR.Name = "pAIR";
            this.pAIR.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.pAIR.Size = new System.Drawing.Size(42, 89);
            this.pAIR.TabIndex = 8;
            this.pAIR.TickFrequency = 10;
            this.pAIR.Scroll += new System.EventHandler(this.pAIR_Scroll);
            // 
            // pH2O
            // 
            this.pH2O.Location = new System.Drawing.Point(520, 45);
            this.pH2O.Maximum = 65000;
            this.pH2O.Name = "pH2O";
            this.pH2O.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.pH2O.Size = new System.Drawing.Size(42, 89);
            this.pH2O.TabIndex = 46;
            this.pH2O.TickFrequency = 10;
            this.pH2O.Scroll += new System.EventHandler(this.pH2O_Scroll);
            // 
            // t2
            // 
            this.t2.Location = new System.Drawing.Point(422, 19);
            this.t2.Name = "t2";
            this.t2.Size = new System.Drawing.Size(92, 20);
            this.t2.TabIndex = 47;
            // 
            // t1
            // 
            this.t1.Location = new System.Drawing.Point(41, 54);
            this.t1.Name = "t1";
            this.t1.Size = new System.Drawing.Size(152, 20);
            this.t1.TabIndex = 48;
            // 
            // t3
            // 
            this.t3.Location = new System.Drawing.Point(422, 454);
            this.t3.Name = "t3";
            this.t3.Size = new System.Drawing.Size(92, 20);
            this.t3.TabIndex = 49;
            // 
            // a3
            // 
            this.a3.BackColor = System.Drawing.SystemColors.Control;
            this.a3.BaseArcColor = System.Drawing.Color.Gray;
            this.a3.BaseArcRadius = 30;
            this.a3.BaseArcStart = 0;
            this.a3.BaseArcSweep = -90;
            this.a3.BaseArcWidth = 2;
            this.a3.Cap_Idx = ((byte)(1));
            this.a3.CapColors = new System.Drawing.Color[] {
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black};
            this.a3.CapPosition = new System.Drawing.Point(10, 10);
            this.a3.CapsPosition = new System.Drawing.Point[] {
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10)};
            this.a3.CapsText = new string[] {
        "",
        "",
        "",
        "",
        ""};
            this.a3.CapText = "";
            this.a3.Center = new System.Drawing.Point(15, 70);
            this.a3.Location = new System.Drawing.Point(422, 359);
            this.a3.MaxValue = 120F;
            this.a3.MinValue = 0F;
            this.a3.Name = "a3";
            this.a3.NeedleColor1 = AGaugeApp.AGauge.NeedleColorEnum.Gray;
            this.a3.NeedleColor2 = System.Drawing.Color.Maroon;
            this.a3.NeedleRadius = 45;
            this.a3.NeedleType = 0;
            this.a3.NeedleWidth = 3;
            this.a3.Range_Idx = ((byte)(2));
            this.a3.RangeColor = System.Drawing.Color.SandyBrown;
            this.a3.RangeEnabled = true;
            this.a3.RangeEndValue = 120F;
            this.a3.RangeInnerRadius = 30;
            this.a3.RangeOuterRadius = 50;
            this.a3.RangesColor = new System.Drawing.Color[] {
        System.Drawing.Color.YellowGreen,
        System.Drawing.Color.Gold,
        System.Drawing.Color.SandyBrown,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.a3.RangesEnabled = new bool[] {
        false,
        false,
        true,
        false,
        false};
            this.a3.RangesEndValue = new float[] {
        40F,
        95F,
        120F,
        0F,
        0F};
            this.a3.RangesInnerRadius = new int[] {
        10,
        10,
        30,
        70,
        70};
            this.a3.RangesOuterRadius = new int[] {
        50,
        50,
        50,
        80,
        80};
            this.a3.RangesStartValue = new float[] {
        0F,
        40F,
        95F,
        0F,
        0F};
            this.a3.RangeStartValue = 95F;
            this.a3.ScaleLinesInterColor = System.Drawing.Color.Black;
            this.a3.ScaleLinesInterInnerRadius = 30;
            this.a3.ScaleLinesInterOuterRadius = 40;
            this.a3.ScaleLinesInterWidth = 1;
            this.a3.ScaleLinesMajorColor = System.Drawing.Color.Black;
            this.a3.ScaleLinesMajorInnerRadius = 30;
            this.a3.ScaleLinesMajorOuterRadius = 50;
            this.a3.ScaleLinesMajorStepValue = 40F;
            this.a3.ScaleLinesMajorWidth = 1;
            this.a3.ScaleLinesMinorColor = System.Drawing.Color.Gray;
            this.a3.ScaleLinesMinorInnerRadius = 30;
            this.a3.ScaleLinesMinorNumOf = 9;
            this.a3.ScaleLinesMinorOuterRadius = 35;
            this.a3.ScaleLinesMinorWidth = 1;
            this.a3.ScaleNumbersColor = System.Drawing.Color.Black;
            this.a3.ScaleNumbersFormat = null;
            this.a3.ScaleNumbersRadius = 60;
            this.a3.ScaleNumbersRotation = 0;
            this.a3.ScaleNumbersStartScaleLine = 0;
            this.a3.ScaleNumbersStepScaleLines = 10;
            this.a3.Size = new System.Drawing.Size(92, 89);
            this.a3.TabIndex = 45;
            this.a3.Text = "aGauge2";
            this.a3.Value = 0F;
            this.a3.Click += new System.EventHandler(this.a3_Click);
            // 
            // a1
            // 
            this.a1.BackColor = System.Drawing.SystemColors.Control;
            this.a1.BaseArcColor = System.Drawing.Color.Gray;
            this.a1.BaseArcRadius = 30;
            this.a1.BaseArcStart = 180;
            this.a1.BaseArcSweep = 270;
            this.a1.BaseArcWidth = 2;
            this.a1.Cap_Idx = ((byte)(1));
            this.a1.CapColors = new System.Drawing.Color[] {
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black};
            this.a1.CapPosition = new System.Drawing.Point(10, 10);
            this.a1.CapsPosition = new System.Drawing.Point[] {
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10)};
            this.a1.CapsText = new string[] {
        "",
        "",
        "",
        "",
        ""};
            this.a1.CapText = "";
            this.a1.Center = new System.Drawing.Point(70, 70);
            this.a1.Location = new System.Drawing.Point(41, 80);
            this.a1.MaxValue = 120F;
            this.a1.MinValue = 0F;
            this.a1.Name = "a1";
            this.a1.NeedleColor1 = AGaugeApp.AGauge.NeedleColorEnum.Gray;
            this.a1.NeedleColor2 = System.Drawing.Color.Maroon;
            this.a1.NeedleRadius = 45;
            this.a1.NeedleType = 0;
            this.a1.NeedleWidth = 3;
            this.a1.Range_Idx = ((byte)(2));
            this.a1.RangeColor = System.Drawing.Color.SandyBrown;
            this.a1.RangeEnabled = true;
            this.a1.RangeEndValue = 120F;
            this.a1.RangeInnerRadius = 30;
            this.a1.RangeOuterRadius = 50;
            this.a1.RangesColor = new System.Drawing.Color[] {
        System.Drawing.Color.YellowGreen,
        System.Drawing.Color.Gold,
        System.Drawing.Color.SandyBrown,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.a1.RangesEnabled = new bool[] {
        false,
        false,
        true,
        false,
        false};
            this.a1.RangesEndValue = new float[] {
        40F,
        95F,
        120F,
        0F,
        0F};
            this.a1.RangesInnerRadius = new int[] {
        10,
        10,
        30,
        70,
        70};
            this.a1.RangesOuterRadius = new int[] {
        50,
        50,
        50,
        80,
        80};
            this.a1.RangesStartValue = new float[] {
        0F,
        40F,
        95F,
        0F,
        0F};
            this.a1.RangeStartValue = 95F;
            this.a1.ScaleLinesInterColor = System.Drawing.Color.Black;
            this.a1.ScaleLinesInterInnerRadius = 30;
            this.a1.ScaleLinesInterOuterRadius = 40;
            this.a1.ScaleLinesInterWidth = 1;
            this.a1.ScaleLinesMajorColor = System.Drawing.Color.Black;
            this.a1.ScaleLinesMajorInnerRadius = 30;
            this.a1.ScaleLinesMajorOuterRadius = 50;
            this.a1.ScaleLinesMajorStepValue = 40F;
            this.a1.ScaleLinesMajorWidth = 1;
            this.a1.ScaleLinesMinorColor = System.Drawing.Color.Gray;
            this.a1.ScaleLinesMinorInnerRadius = 30;
            this.a1.ScaleLinesMinorNumOf = 9;
            this.a1.ScaleLinesMinorOuterRadius = 35;
            this.a1.ScaleLinesMinorWidth = 1;
            this.a1.ScaleNumbersColor = System.Drawing.Color.Black;
            this.a1.ScaleNumbersFormat = null;
            this.a1.ScaleNumbersRadius = 60;
            this.a1.ScaleNumbersRotation = 0;
            this.a1.ScaleNumbersStartScaleLine = 0;
            this.a1.ScaleNumbersStepScaleLines = 10;
            this.a1.Size = new System.Drawing.Size(152, 147);
            this.a1.TabIndex = 44;
            this.a1.Text = "aGauge2";
            this.a1.Value = 0F;
            this.a1.Click += new System.EventHandler(this.a1_Click);
            // 
            // a2
            // 
            this.a2.BackColor = System.Drawing.SystemColors.Control;
            this.a2.BaseArcColor = System.Drawing.Color.Gray;
            this.a2.BaseArcRadius = 30;
            this.a2.BaseArcStart = 0;
            this.a2.BaseArcSweep = -90;
            this.a2.BaseArcWidth = 2;
            this.a2.Cap_Idx = ((byte)(1));
            this.a2.CapColors = new System.Drawing.Color[] {
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black};
            this.a2.CapPosition = new System.Drawing.Point(10, 10);
            this.a2.CapsPosition = new System.Drawing.Point[] {
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10)};
            this.a2.CapsText = new string[] {
        "",
        "",
        "",
        "",
        ""};
            this.a2.CapText = "";
            this.a2.Center = new System.Drawing.Point(15, 70);
            this.a2.Location = new System.Drawing.Point(422, 45);
            this.a2.MaxValue = 120F;
            this.a2.MinValue = 0F;
            this.a2.Name = "a2";
            this.a2.NeedleColor1 = AGaugeApp.AGauge.NeedleColorEnum.Gray;
            this.a2.NeedleColor2 = System.Drawing.Color.Maroon;
            this.a2.NeedleRadius = 45;
            this.a2.NeedleType = 0;
            this.a2.NeedleWidth = 3;
            this.a2.Range_Idx = ((byte)(2));
            this.a2.RangeColor = System.Drawing.Color.SandyBrown;
            this.a2.RangeEnabled = true;
            this.a2.RangeEndValue = 120F;
            this.a2.RangeInnerRadius = 30;
            this.a2.RangeOuterRadius = 50;
            this.a2.RangesColor = new System.Drawing.Color[] {
        System.Drawing.Color.YellowGreen,
        System.Drawing.Color.Gold,
        System.Drawing.Color.SandyBrown,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.a2.RangesEnabled = new bool[] {
        false,
        false,
        true,
        false,
        false};
            this.a2.RangesEndValue = new float[] {
        40F,
        95F,
        120F,
        0F,
        0F};
            this.a2.RangesInnerRadius = new int[] {
        10,
        10,
        30,
        70,
        70};
            this.a2.RangesOuterRadius = new int[] {
        50,
        50,
        50,
        80,
        80};
            this.a2.RangesStartValue = new float[] {
        0F,
        40F,
        95F,
        0F,
        0F};
            this.a2.RangeStartValue = 95F;
            this.a2.ScaleLinesInterColor = System.Drawing.Color.Black;
            this.a2.ScaleLinesInterInnerRadius = 30;
            this.a2.ScaleLinesInterOuterRadius = 40;
            this.a2.ScaleLinesInterWidth = 1;
            this.a2.ScaleLinesMajorColor = System.Drawing.Color.Black;
            this.a2.ScaleLinesMajorInnerRadius = 30;
            this.a2.ScaleLinesMajorOuterRadius = 50;
            this.a2.ScaleLinesMajorStepValue = 40F;
            this.a2.ScaleLinesMajorWidth = 1;
            this.a2.ScaleLinesMinorColor = System.Drawing.Color.Gray;
            this.a2.ScaleLinesMinorInnerRadius = 30;
            this.a2.ScaleLinesMinorNumOf = 9;
            this.a2.ScaleLinesMinorOuterRadius = 35;
            this.a2.ScaleLinesMinorWidth = 1;
            this.a2.ScaleNumbersColor = System.Drawing.Color.Black;
            this.a2.ScaleNumbersFormat = null;
            this.a2.ScaleNumbersRadius = 60;
            this.a2.ScaleNumbersRotation = 0;
            this.a2.ScaleNumbersStartScaleLine = 0;
            this.a2.ScaleNumbersStepScaleLines = 10;
            this.a2.Size = new System.Drawing.Size(92, 89);
            this.a2.TabIndex = 43;
            this.a2.Text = "aGauge2";
            this.a2.Value = 0F;
            this.a2.Click += new System.EventHandler(this.a2_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(711, 527);
            this.Controls.Add(this.t3);
            this.Controls.Add(this.t1);
            this.Controls.Add(this.t2);
            this.Controls.Add(this.pH2O);
            this.Controls.Add(this.a3);
            this.Controls.Add(this.a1);
            this.Controls.Add(this.a2);
            this.Controls.Add(this.pAIR);
            this.Controls.Add(this.VH2O);
            this.Name = "Form3";
            this.Text = "Form3";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form3_FormClosing);
            this.Load += new System.EventHandler(this.Form3_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pAIR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pH2O)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox VH2O;
        private System.Windows.Forms.TrackBar pAIR;
        private AGaugeApp.AGauge a2;
        private AGaugeApp.AGauge a1;
        private AGaugeApp.AGauge a3;
        private System.Windows.Forms.TrackBar pH2O;
        private System.Windows.Forms.TextBox t2;
        private System.Windows.Forms.TextBox t1;
        private System.Windows.Forms.TextBox t3;
    }
}