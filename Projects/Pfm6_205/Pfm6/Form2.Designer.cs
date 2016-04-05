namespace WindowsFormsApplication1
{
    partial class Form2
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Simmer1 = new System.Windows.Forms.ToolStripMenuItem();
            this.Simmer2 = new System.Windows.Forms.ToolStripMenuItem();
            this.chart1 = new ZedGraph.ZedGraphControl();
            this.fbar = new System.Windows.Forms.VScrollBar();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.Simmer1,
            this.Simmer2});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(571, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // Simmer1
            // 
            this.Simmer1.Name = "Simmer1";
            this.Simmer1.Size = new System.Drawing.Size(59, 20);
            this.Simmer1.Text = "Simmer1";
            this.Simmer1.Click += new System.EventHandler(this.Simmer1_Click);
            // 
            // Simmer2
            // 
            this.Simmer2.Name = "Simmer2";
            this.Simmer2.Size = new System.Drawing.Size(59, 20);
            this.Simmer2.Text = "Simmer2";
            this.Simmer2.Click += new System.EventHandler(this.Simmer2_Click);
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.chart1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.chart1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.chart1.Location = new System.Drawing.Point(25, 43);
            this.chart1.Name = "chart1";
            this.chart1.ScrollGrace = 0D;
            this.chart1.ScrollMaxX = 0D;
            this.chart1.ScrollMaxY = 0D;
            this.chart1.ScrollMaxY2 = 0D;
            this.chart1.ScrollMinX = 0D;
            this.chart1.ScrollMinY = 0D;
            this.chart1.ScrollMinY2 = 0D;
            this.chart1.Size = new System.Drawing.Size(148, 105);
            this.chart1.TabIndex = 2;
            // 
            // fbar
            // 
            this.fbar.Location = new System.Drawing.Point(427, 67);
            this.fbar.Minimum = 1;
            this.fbar.Name = "fbar";
            this.fbar.Size = new System.Drawing.Size(16, 80);
            this.fbar.TabIndex = 3;
            this.fbar.Value = 1;
            this.fbar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.fbar_Scroll);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 347);
            this.Controls.Add(this.fbar);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form2";
            this.Text = "Form2";
            this.TransparencyKey = System.Drawing.Color.Yellow;
            this.Load += new System.EventHandler(this.Form2_Resize);
            this.Resize += new System.EventHandler(this.Form2_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private ZedGraph.ZedGraphControl chart1;
        private System.Windows.Forms.ToolStripMenuItem Simmer1;
        private System.Windows.Forms.ToolStripMenuItem Simmer2;
        private System.Windows.Forms.VScrollBar fbar;
    }
}