namespace WindowsFormsApplication1
{
    partial class Form4
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
            this.FanEnter = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // FanEnter
            // 
            this.FanEnter.Location = new System.Drawing.Point(417, 221);
            this.FanEnter.Name = "FanEnter";
            this.FanEnter.Size = new System.Drawing.Size(60, 35);
            this.FanEnter.TabIndex = 0;
            this.FanEnter.Text = "Close";
            this.FanEnter.UseVisualStyleBackColor = true;
            this.FanEnter.Click += new System.EventHandler(this.FanEnter_Click);
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 266);
            this.Controls.Add(this.FanEnter);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form4";
            this.Text = "...";
            this.Load += new System.EventHandler(this.Form4_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button FanEnter;
    }
}