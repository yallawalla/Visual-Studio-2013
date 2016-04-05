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
            this.progressBarProgram = new System.Windows.Forms.ProgressBar();
            this.progressBarErase = new System.Windows.Forms.ProgressBar();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBarProgram
            // 
            this.progressBarProgram.BackColor = System.Drawing.Color.DarkRed;
            this.progressBarProgram.ForeColor = System.Drawing.Color.DarkRed;
            this.progressBarProgram.Location = new System.Drawing.Point(64, 39);
            this.progressBarProgram.Name = "progressBarProgram";
            this.progressBarProgram.Size = new System.Drawing.Size(335, 24);
            this.progressBarProgram.Step = 1;
            this.progressBarProgram.TabIndex = 14;
            // 
            // progressBarErase
            // 
            this.progressBarErase.BackColor = System.Drawing.Color.DarkRed;
            this.progressBarErase.ForeColor = System.Drawing.Color.DarkRed;
            this.progressBarErase.Location = new System.Drawing.Point(63, 9);
            this.progressBarErase.Name = "progressBarErase";
            this.progressBarErase.Size = new System.Drawing.Size(336, 24);
            this.progressBarErase.Step = 1;
            this.progressBarErase.TabIndex = 13;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 39);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Program";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(34, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "Erase";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 89);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.progressBarProgram);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.progressBarErase);
            this.Name = "Form3";
            this.Text = "Boot";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarProgram;
        private System.Windows.Forms.ProgressBar progressBarErase;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
    }
}