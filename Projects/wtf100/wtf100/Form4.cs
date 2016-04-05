using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApplication1
{
    public partial class pShape : Form
    {
        FileDialog      fd = null;
        Form1           f1 = null;
        ToolTip         tooltip = new ToolTip();
        Point           R1,R2;
        int             count=0;
        double fdy1 = 0, fy1 = 0, fe1 = 0;
        double fdy2 = 0, fy2 = 0, fe2 = 0;
        double off1=0, off2=0;

        BackgroundWorker bw = new BackgroundWorker();
        int              undo = 0;
        ushort           crc = 0;

        public pShape(Form1 fparent)
        {
            f1 = fparent;
            InitializeComponent();
            Tgrid.Text = Properties.chart.Default.Tgrid.ToString();
            Ugrid.Text = Properties.chart.Default.Ugrid.ToString();
            Tmax.Text = Properties.chart.Default.Tmax.ToString();
            Umax.Text = Properties.chart.Default.Umax.ToString();
            pwmrate.Text = Properties.chart.Default.pwmrate.ToString();
            FlashK1.Text = Properties.chart.Default.flashK1.ToString();
            FlashK2.Text = Properties.chart.Default.flashK2.ToString();

            chart.Left = 0;
            chart.Top = toolStrip1.Height;
            chart.Width = Width;
            chart.Height = Height - toolStrip1.Height - 20;

            chart.Series["p1"].Points.Clear();
            chart.Series["p2"].Points.Clear();
            chart.Series["p3"].Points.Clear();
            chart.Series["p4"].Points.Clear();

            ContextMenu     rclick  = new ContextMenu();
            MenuItem undo = new MenuItem(),
                            redo = new MenuItem(),
                            zoomrst = new MenuItem();
    
            rclick.MenuItems.AddRange(new MenuItem[] { undo,redo,zoomrst});

            undo.Index = 0;
            undo.Text = "Undo";
            undo.Click += undo_Click;

            redo.Index = 1;
            redo.Text = "Redo";
            redo.Click += redo_Click;

            zoomrst.Index = 2;
            zoomrst.Text = "Zoom reset";
            zoomrst.Click += zoomrst_Click;
            
            chart.ContextMenu = rclick;
            
            chart.ChartAreas["1"].AxisX.ScaleView.Zoomable = true;
            chart.ChartAreas["1"].AxisY.ScaleView.Zoomable = true;

            this.MouseWheel += new MouseEventHandler(chart_MouseWheel);
            chart.ChartAreas["1"].AxisX.ScaleView.Zoom(Properties.chart.Default.Zxmin, Properties.chart.Default.Zxmax);
            chart.ChartAreas["1"].AxisY.ScaleView.Zoom(Properties.chart.Default.Zymin, Properties.chart.Default.Zymax);
            chart_AxisViewChanged(null, null);

            if (Properties.chart.Default.filename != "---")
            {
                Download(Properties.chart.Default.filename);
                chart.Refresh();
            }

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C del undo*.lw";
            process.StartInfo = startInfo;
            process.Start();
            System.Threading.Thread.Sleep(100);

        }

        private ushort Crc(ushort val, ushort tmp)
        {
            if (((val ^ tmp) & 0x0001) == 0)
                return (ushort)(((val ^ tmp) >> 1) ^ 0xA001);
            else
                return (ushort)((val ^ tmp) >> 1);
        }


        private void chart_MouseWheel(object sender, MouseEventArgs e)
        {
            double x1 = chart.ChartAreas["1"].AxisX.ScaleView.ViewMinimum;
            double x2 = chart.ChartAreas["1"].AxisX.ScaleView.ViewMaximum;
            double y1 = chart.ChartAreas["1"].AxisY.ScaleView.ViewMinimum;
            double y2 = chart.ChartAreas["1"].AxisY.ScaleView.ViewMaximum;
            if (x2 - x1 > 1000 || e.Delta<0)
            {
                chart.ChartAreas["1"].AxisX.ScaleView.Zoom(Math.Max(0, x1 + e.Delta), Math.Min(Properties.chart.Default.Tmax, x2 - e.Delta));
                chart_AxisViewChanged(null, null);
            }
         }

        void undo_Click(object sender, EventArgs e)
        {
            try
            {
                if (Download("undo" + (undo - 2).ToString() + ".lw"))
                    undo = undo - 2;
            }
            catch
            {
            }
        }

        void redo_Click(object sender, EventArgs e)
        {
            try
            {
                Download("undo" + undo.ToString() + ".lw");
            }
            catch
            {
            }
        }

        void zoomrst_Click(object sender, EventArgs e)
        {
            chart.ChartAreas["1"].AxisX.ScaleView.ZoomReset();
            chart.ChartAreas["1"].AxisY.ScaleView.ZoomReset();
            chart.ChartAreas["1"].AxisX.ScaleView.Size = Properties.chart.Default.Tmax;
            chart.ChartAreas["1"].AxisY.ScaleView.Size = Properties.chart.Default.Umax;
            chart.ChartAreas["1"].AxisX.Minimum = 0;
            chart.ChartAreas["1"].AxisY.Minimum = -100;
            chart_AxisViewChanged(null, null);
        }


        public void parse(string line)
        {
            if (line != "")
            {
                try
                {
                    char[] charSeparators = new char[] { ',', '\x9', ' ' };
                    var cols = line.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);

                    if (cols.Length == 1 && cols[0] == "?t")
                    {
                        chart.Series["p2"].Points.Clear();
                        chart.Series["p3"].Points.Clear();
                        fe1 = fe2 = 0;
                    }

                    else if (cols.Length == 3)
                    {
                        foreach (string col in cols)
                            try { Convert.ToDouble(col); }
                            catch
                            {
                                return;
                            }
                        DrawT(Convert.ToDouble(cols[0]), Convert.ToDouble(cols[1]), Convert.ToDouble(cols[2]));
                    }

                    else if (cols.Length == 8 && cols[0].Substring(0,1) == ":" && cols[2] == "03")            // pfm set
                    {
                        f1.WritePort("t\r");
                    }

                    else if (cols.Length == 4)
                    {
                        chart.Series["p1"].Points.AddY(Convert.ToDouble(cols[0]));
                        chart.Series["p2"].Points.AddY(Convert.ToDouble(cols[1]));
                        chart.Series["p3"].Points.AddY(Convert.ToDouble(cols[2]));
                        chart.Series["p4"].Points.AddY(Convert.ToDouble(cols[3]));

                    }

                    else if (cols.Length == 8)
                    {
                        foreach (string col in cols)
                            try { Convert.ToDouble(col); }
                            catch { return; }
                        chart.Series["p1"].Points.AddXY(count, Convert.ToDouble(cols[0]));
                        chart.Series["p2"].Points.AddXY(count, Convert.ToDouble(cols[1]));
                        ++count;
                    }
                }
                catch { }
            }
        }

        private void DrawT(double to, double xin1, double xin2)
        {
            int t;

            try
            {
                t=(int)chart.Series["p3"].Points.FindMaxByValue("X").XValue;
            }
            catch
            {
                t=0;
                fdy1 =fy1 =fdy2 =fy2 =0;
                off1 = 0;
                off2 = 0;
            }

            xin1 = (xin1 - off1) * (7.0 / 6.0);                                     // voltage = (U-Uo)*pwm scale 
            xin2 = (xin2 - off2) * (7.0 / 6.0);
            while(t++ <= to)
            {
                double tau = 0.05;
                int k1 = Properties.chart.Default.flashK1;
                int k2 = Properties.chart.Default.flashK2;
                fy1 += fdy1 * tau;                                                 // current = U*U/400
                fdy1 += (xin1 * xin1 / k1 / k1 - fy1 - 2 * fdy1) * tau;
                fy2 += fdy2 * tau;
                fdy2 += (xin2 * xin2 / k2 / k2 - fy2 - 2 * fdy2) * tau;

                fe1 += fy1 * xin1 * 1e-6;
                fe2 += fy2 * xin2 * 1e-6;

                chart.Series["p2"].Points.AddXY(t, fy1 * xin1 / 1000);              // kW
                chart.Series["p3"].Points.AddXY(t, fy2 * xin2 / 1000);              // kW
            }
        }

        private void Form4_Resize(object sender, EventArgs e)
        {
            chart.Left = 0;
            chart.Top = toolStrip1.Height;
            chart.Width = Width;
            chart.Height = Height - toolStrip1.Height - 20;
        }

        private void chart_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                R1.X = R2.X = (int)(Properties.chart.Default.Tgrid * Math.Round(chart.ChartAreas["1"].AxisX.PixelPositionToValue(e.X) / Properties.chart.Default.Tgrid));
                R1.Y = R2.Y = (int)(Properties.chart.Default.Ugrid * Math.Round(chart.ChartAreas["1"].AxisY.PixelPositionToValue(e.Y) / Properties.chart.Default.Ugrid));
                foreach (DataPoint p in chart.Series["p1"].Points)
                    foreach (DataPoint q in chart.Series["p4"].Points)
                        if (p.XValue == q.XValue)
                            p.YValues[0] = q.YValues[0];
                chart.Series["p4"].Points.Clear();
//                FilterButton_Click(null, null);
                chart.Refresh();
            }
        }

        private  void chart_MouseMove(object sender, MouseEventArgs e)
        {
            if (HandButton.Checked == true && chart.Series["p4"].Points.Count() != 0 && e.Button == MouseButtons.None)
            {
                int r2x = (int)(Properties.chart.Default.Tgrid * Math.Round(chart.ChartAreas["1"].AxisX.PixelPositionToValue(e.X) / Properties.chart.Default.Tgrid));
                int r2y = (int)(Properties.chart.Default.Ugrid * Math.Round(chart.ChartAreas["1"].AxisY.PixelPositionToValue(e.Y) / Properties.chart.Default.Ugrid));
                foreach (DataPoint p in chart.Series["p4"].Points)
                {
                    if (SlopeButton.Checked)
                        p.YValues[0] += (r2y - R2.Y) * (p.XValue - R1.X) / (R2.X - R1.X);
                    else
                        p.YValues[0] += (r2y - R2.Y);
                    p.XValue += r2x - R2.X; 
                }
                R1.Y += r2y - R2.Y;
                R2.Y = r2y;
                R1.X -= R2.X - r2x;
                R2.X = r2x;
            }
            if (e.Button == MouseButtons.Left)
            {
                R2.X = (int)(Properties.chart.Default.Tgrid * Math.Round(chart.ChartAreas["1"].AxisX.PixelPositionToValue(e.X) / Properties.chart.Default.Tgrid));
                R2.Y = (int)(Properties.chart.Default.Ugrid * Math.Round(chart.ChartAreas["1"].AxisY.PixelPositionToValue(e.Y) / Properties.chart.Default.Ugrid));
            }
            chart.Refresh();
        }

        private void chart_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                double x1 = Properties.chart.Default.Tgrid * Math.Round((double)Math.Min(R1.X, R2.X) / Properties.chart.Default.Tgrid);
                double x2 = Properties.chart.Default.Tgrid * Math.Round((double)Math.Max(R1.X, R2.X) / Properties.chart.Default.Tgrid);
                double y1 = Properties.chart.Default.Ugrid * Math.Round((double)Math.Min(R1.Y, R2.Y) / Properties.chart.Default.Ugrid);
                double y2 = Properties.chart.Default.Ugrid * Math.Round((double)Math.Max(R1.Y, R2.Y) / Properties.chart.Default.Ugrid);

                if (ZoomButton.Checked==true)
                {
                    if (Math.Abs(R1.X - R2.X) > Properties.chart.Default.Tgrid  && Math.Abs(R1.Y - R2.Y) > Properties.chart.Default.Ugrid)
                    {
                        chart.ChartAreas["1"].AxisX.ScaleView.Zoom(x1,x2);
                        chart.ChartAreas["1"].AxisY.ScaleView.Zoom(y1,y2);
                        chart_AxisViewChanged(null, null);
                    }
                }

                else if (HandButton.Checked==true)
                {
                    chart.Series["p4"].Points.Clear();
                    foreach (DataPoint pt in chart.Series["p1"].Points)
                    {
                        if (pt.XValue >= x1 && pt.XValue < x2 && pt.YValues[0] >= y1 && pt.YValues[0] <= y2)
                            chart.Series["p4"].Points.AddXY(pt.XValue, pt.YValues[0]);
                    }
                }

                else if (DeleteButton.Checked == true)
                {
                    foreach (DataPoint pt in chart.Series["p1"].Points)
                        if (pt.XValue >= x1 && pt.XValue < x2 && pt.YValues[0] >= y1 && pt.YValues[0] <= y2)
                        {
                            pt.YValues[0] = R2.Y;
                        }
                }
                else
                {}

                if (chart.Series["p4"].Points.Count == 0)
                {
                    R1.X = R2.X = (int)(Properties.chart.Default.Tgrid * Math.Round(chart.ChartAreas["1"].AxisX.PixelPositionToValue(e.X) / Properties.chart.Default.Tgrid));
                    R1.Y = R2.Y = (int)(Properties.chart.Default.Ugrid * Math.Round(chart.ChartAreas["1"].AxisY.PixelPositionToValue(e.Y) / Properties.chart.Default.Ugrid));
                }
//               FilterButton_Click(null, null);
                chart.Refresh();   
            }
        }

        private void chart_Paint(object sender, PaintEventArgs e)
        {
            foreach (DataPoint p in chart.Series["p1"].Points)
                if(p.YValues[0] < 0)
                    p.YValues[0]=0;


            if (R1 != R2)
            {
                int xmin = (int)chart.ChartAreas["1"].AxisX.ValueToPixelPosition((double)Math.Min(R1.X, R2.X));
                int ymin = (int)chart.ChartAreas["1"].AxisY.ValueToPixelPosition((double)Math.Max(R1.Y, R2.Y));
                int xmax = (int)chart.ChartAreas["1"].AxisX.ValueToPixelPosition((double)Math.Max(R1.X, R2.X));
                int ymax = (int)chart.ChartAreas["1"].AxisY.ValueToPixelPosition((double)Math.Min(R1.Y, R2.Y));

                Pen p = new Pen(Color.Yellow, 1);
                p.DashStyle = DashStyle.Dot;

                Rectangle rect = new Rectangle(xmin,ymin,xmax-xmin,ymax-ymin);
                e.Graphics.DrawRectangle(p, rect);
            }

            //if (FilterButton.Checked == true)
            //{
            //    double esum = 0;
            //    Font font = new Font("Arial", 16);
            //    SolidBrush brush = new SolidBrush(Color.Yellow);
            //    foreach (DataPoint p in chart.Series["p2"].Points)
            //        esum += Math.Pow(p.YValues[0], 3) / 400;
            //    e.Graphics.DrawString(String.Format("{0:0.0} J", esum * 1e-6), font, brush, Width - 100, chart.ChartAreas["1"].Position.Y + 100);
            //}

            if (fe1 > 0 || fe2 > 0)
            {
                Font font = new Font("Arial", 16);
                SolidBrush brush1 = new SolidBrush(chart.Series["p2"].Color);
                SolidBrush brush2 = new SolidBrush(chart.Series["p3"].Color);
                e.Graphics.DrawString(String.Format("{0:0.0} J", fe1), font, brush1, Width - 100, chart.ChartAreas["1"].Position.Y + 100);
                e.Graphics.DrawString(String.Format("{0:0.0} J", fe2), font, brush2, Width - 100, chart.ChartAreas["1"].Position.Y + 100 + font.Height);
            }
        }

        private void fNew_Click(object sender, EventArgs e)
        {
            fd = null; 
            chart.Series["p1"].Points.Clear();
            chart.Series["p2"].Points.Clear();
            chart.Series["p3"].Points.Clear();
            chart.Series["p4"].Points.Clear();

            for (int i = 0; i < Properties.chart.Default.Tmax; i += Properties.chart.Default.pwmrate)
                chart.Series["p1"].Points.AddXY(i, 0);

            chart.ChartAreas["1"].AxisX.Minimum = 0;
            chart.ChartAreas["1"].AxisY.Minimum = -100;
            chart.ChartAreas["1"].AxisX.Maximum = Properties.chart.Default.Tmax;
            chart.ChartAreas["1"].AxisY.Maximum = Properties.chart.Default.Umax;
            chart.Refresh();
        }

        private void SlopeButton_Click(object sender, EventArgs e)
        {
            HandButton.Checked = true;
            DeleteButton.Checked = false;
            ZoomButton.Checked = false;
        }
 
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            SlopeButton.Checked = false;
            HandButton.Checked = false;
            ZoomButton.Checked = false;
        }

        private void HandButton_Click(object sender, EventArgs e)
        {
            SlopeButton.Checked = false;
            DeleteButton.Checked = false;
            ZoomButton.Checked = false;
        }

        private void ZoomButton_Click(object sender, EventArgs e)
        {
            SlopeButton.Checked = false;
            HandButton.Checked = false;
            DeleteButton.Checked = false;
        }

        private void chart_AxisScrollBarClicked(object sender, System.Windows.Forms.DataVisualization.Charting.ScrollBarEventArgs e)
        {
            ZoomButton.Checked = false;
        }

        private void chart_AxisViewChanged(object sender, ViewEventArgs e)
        {
            chart.ChartAreas["1"].AxisX.LabelStyle.IntervalOffset = -chart.ChartAreas["1"].AxisX.ScaleView.ViewMinimum;
            chart.ChartAreas["1"].AxisY.LabelStyle.IntervalOffset = -chart.ChartAreas["1"].AxisY.ScaleView.ViewMinimum;
            chart.ChartAreas["1"].AxisX.MinorGrid.IntervalOffset = -chart.ChartAreas["1"].AxisX.ScaleView.ViewMinimum;
            chart.ChartAreas["1"].AxisY.MinorGrid.IntervalOffset = -chart.ChartAreas["1"].AxisY.ScaleView.ViewMinimum;
            chart.ChartAreas["1"].AxisX.MajorGrid.IntervalOffset = -chart.ChartAreas["1"].AxisX.ScaleView.ViewMinimum;
            chart.ChartAreas["1"].AxisY.MajorGrid.IntervalOffset = -chart.ChartAreas["1"].AxisY.ScaleView.ViewMinimum;
            Properties.chart.Default.Zxmin = chart.ChartAreas["1"].AxisX.ScaleView.ViewMinimum;
            Properties.chart.Default.Zymin = chart.ChartAreas["1"].AxisY.ScaleView.ViewMinimum;
            Properties.chart.Default.Zxmax = chart.ChartAreas["1"].AxisX.ScaleView.ViewMaximum;
            Properties.chart.Default.Zymax = chart.ChartAreas["1"].AxisY.ScaleView.ViewMaximum;
            Properties.chart.Default.Save();
        }

        private void toolStripTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Properties.chart.Default.Tgrid = Convert.ToInt32(Tgrid.Text);
                Properties.chart.Default.Ugrid = Convert.ToInt32(Ugrid.Text);
                Properties.chart.Default.Tmax = Convert.ToInt32(Tmax.Text);
                Properties.chart.Default.Umax = Convert.ToInt32(Umax.Text);
                Properties.chart.Default.pwmrate = Convert.ToInt32(pwmrate.Text);
                Properties.chart.Default.flashK1 = Convert.ToInt32(FlashK1.Text);
                Properties.chart.Default.flashK2 = Convert.ToInt32(FlashK2.Text);
                Properties.chart.Default.Save();
                SetupButton.HideDropDown();
            }
        }

        private void Upload(StreamWriter f)
        {
            if (f == null)
                f1.WritePort("\r#\r");
            else
                f.Write("\r#\r");
            int x = 0, xx = 0, y = 0;
            foreach (DataPoint p in chart.Series["p1"].Points)
            {
                xx = (int)Math.Round(p.XValue,0);
                if (Math.Round(p.YValues[0], 0) != y)
                {
                    if (f == null)
                        f1.WritePort("#" + (xx-x).ToString() + "," + y.ToString() + "\r");
                    else
                        f.Write("#" + (xx - x).ToString() + "," + y.ToString() + "\r");
                    y = (int)Math.Round(p.YValues[0], 0);
                    x = (int)Math.Round(p.XValue, 0);
                }
            }
            if (f == null)
                f1.WritePort("#" + (xx - x).ToString() + "," + y.ToString() + "\r");
            else
                f.Write("#" + (xx - x).ToString() + "," + y.ToString() + "\r");
        }



        private void FilterButton_Click(object sender, EventArgs e)
        {
            fe1 = fe2 = 0;
            f1.WritePort("?t\r");
        }

        public bool Intersect(double Ax, double Ay,
            double Bx, double By,
            double Cx, double Cy,
            double Dx, double Dy,
            ref double X, ref double Y)
        {

            double distAB, theCos, theSin, newX, ABpos;
            if (Ax == Bx && Ay == By || Cx == Dx && Cy == Dy) return false;
            
            Bx -= Ax; By -= Ay;
            Cx -= Ax; Cy -= Ay;
            Dx -= Ax; Dy -= Ay;

            distAB = Math.Sqrt(Bx * Bx + By * By);          //  dolzina A-B.
            
            theCos = Bx / distAB;                           //  zasuk....
            theSin = By / distAB;
            newX = Cx * theCos + Cy * theSin;
            Cy = Cy * theCos - Cx * theSin; Cx = newX;
            newX = Dx * theCos + Dy * theSin;
            Dy = Dy * theCos - Dx * theSin; Dx = newX;

   
            if (Cy == Dy) return false;                     // paralelno....
            
            ABpos = Dx + (Cx - Dx) * Dy / (Dy - Cy);        // skalarni prod. ... projekcija
            
            X = Ax + ABpos * theCos;                        // zasuk nazaj
            Y = Ay + ABpos * theSin;
            return true;
        }

        private void chart_Click(object sender, EventArgs e)
        {

        }


        private void fOpen_Click(object sender, EventArgs e)
        {
            fd = new OpenFileDialog();
            fd.Filter = "lw files (*.lw)|*.lw|All files (*.*)|*.*";
            fd.FilterIndex = 1;
            fd.RestoreDirectory = true;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Download(fd.FileName);
                    chart.Refresh();
                    Properties.chart.Default.filename = fd.FileName;
                    Properties.chart.Default.Save();
                }
                catch
                {
                    MessageBox.Show("file error...");
                }
            }
            else
            {
                fd = null;
            }
        }

        private bool Download(string filename)
        {
            try
            {
                char[] charSeparators = new char[] { ',', '\x9', ' ' };
                string[] ss = File.ReadAllLines(filename);
                chart.Series["p1"].Points.Clear();
                chart.Series["p2"].Points.Clear();
                chart.Series["p3"].Points.Clear();
                chart.Series["p4"].Points.Clear();
                count = 0;
                foreach (string s in ss)
                {
                    int i;
                    var cols = s.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                    for (i = 0; cols.Length == 2 && i < Convert.ToDecimal(Convert.ToDecimal(cols[0].Substring(1))); i += Properties.chart.Default.pwmrate) {
                        chart.Series["p1"].Points.AddXY(count + i, Convert.ToDecimal(cols[1]));
                        chart.Series["p2"].Points.AddXY(count + i, 0);
                        chart.Series["p3"].Points.AddXY(count + i, 0);
                    }
                    count += i;
                }
                chart.Refresh();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void fSaveAs_Click(object sender, EventArgs e)
        {
            fd = new SaveFileDialog();
            fd.Filter = "lw files (*.lw)|*.lw|All files (*.*)|*.*";
            fd.FilterIndex = 1;
            fd.RestoreDirectory = true;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamWriter fs = new StreamWriter(fd.FileName);
                    Upload(fs);
                    fs.Close();
                    Properties.chart.Default.filename = fd.FileName;
                    Properties.chart.Default.Save();
                }
                catch
                {
                    MessageBox.Show("file error...");
                }
            }  else {
                fd = null;
            }
        }

        private void fSave_Click(object sender, EventArgs e)
        {
            if (fd == null)
            {
                fd = new SaveFileDialog();
                fd.Filter = "lw files (*.lw)|*.lw|All files (*.*)|*.*";
                fd.FilterIndex = 1;
                fd.RestoreDirectory = true;
                if (fd.ShowDialog() != DialogResult.OK)
                {
                    fd = null;
                    return;
                }
            }
            try
            {
                StreamWriter fs = new StreamWriter(fd.FileName);
                Upload(fs);
                fs.Close();
                Properties.chart.Default.filename = fd.FileName;
                Properties.chart.Default.Save();
            }
            catch
            {
                MessageBox.Show("file error...");
            } 
        }

        private void fExit_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Application.MessageLoop)
                System.Windows.Forms.Application.Exit();
            else
                System.Environment.Exit(1);   
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            Upload(null);
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            fExit_Click(null, null);
        }

        private void pShape_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)25:                                  // CtrlY
                    redo_Click(null, null);
                    break;
                case (char)26:                                  // CtrlZ
                    undo_Click(null, null);
                    break;
                case (char)27:                                  // Esc
                    R1.X = R2.X = R1.Y = R2.Y = 0;
                    chart.Series["p4"].Points.Clear();
                    chart.Refresh();
                    break;
                default:
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ushort tmp = 0;
            foreach (DataPoint p in chart.Series["p1"].Points)
            {
                tmp = Crc((ushort)p.XValue, tmp);
                tmp = Crc((ushort)p.YValues[0], tmp);
            }
            if (crc != tmp)
            {
                crc = tmp;
                StreamWriter fs = new StreamWriter("undo" + undo.ToString() + ".lw");
                ++undo;
                Upload(fs);
                fs.Flush();
                fs.Close();
            }
        }
    }
}
