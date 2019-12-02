using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Text.RegularExpressions;  

namespace ttt
{
    public partial class Form1 : Form
    {
        double tmax = 0, tmin = Double.MaxValue;
        public Form1()
        {
            List<string> id = new List<string>();

            InitializeComponent();
            Text = "";

            Axis ax = chart1.ChartAreas["ChartArea1"].AxisY;
            ax.MinorGrid.Enabled = ax.MajorGrid.Enabled = true;
            ax.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
            ax.MinorGrid.LineColor = Color.LightGray;
            ax.MinorGrid.Interval = 1;
            ax.MajorGrid.Interval = 5;
            ax.LabelStyle.Enabled = false;

            ax = chart1.ChartAreas["ChartArea1"].AxisX;
            ax.MajorGrid.Enabled = true;
            ax.MinorGrid.Enabled = true;
            ax.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
            ax.MinorGrid.LineColor = Color.LightGray;
            ax.ScaleView.Zoomable = true;

            ChartArea CA = chart1.ChartAreas["ChartArea1"];
            CA.CursorX.Interval = 0.001;
            CA.CursorX.IsUserEnabled = true;
            CA.CursorX.AutoScroll = true;
            CA.CursorX.IsUserSelectionEnabled = true;
        }

        private void Display()
        {
            try
            {
                StreamReader reader = File.OpenText(Text);
                string line;
                foreach (Series s in chart1.Series)
                    s.Points.Clear();
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.TrimStart();
                    line = Regex.Replace(line, " {2,}", " ");

                    for (int i = 0; i < chart1.Series.Count; ++i)
                    {
                        Series s = chart1.Series[i];
                        s.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        if (s.Name == line.Substring(25, 3))
                        {
                            double t = double.Parse(line.Substring(1, 16));
                            switch (s.Name)
                            {
                                case "200":
                                case "240":
                                case "241":
                                    double tt = int.Parse(line.Substring(33, 2));
                                    if (s.Points.Count > 0)
                                        s.Points.Add(new DataPoint(t, s.Points[s.Points.Count - 1].YValues[0]));
                                    s.Points.Add(new DataPoint(t, tt + i * 5));
                                    break;
                                default:
                                    if (s.Points.Count == 0)
                                        s.Points.Add(new DataPoint(t, i * 5));
                                    else
                                    {
                                        s.Points.Add(new DataPoint(t, i * 5));
                                        s.Points.Add(new DataPoint(t, i * 5 + 1));
                                        s.Points.Add(new DataPoint(t, i * 5));
                                    }
                                    break;
                            }
                        }
                    }
                }
                reader.Close();
            }
            catch
            {
                MessageBox.Show("sniffer file ???...");
            }

        }


        private void menuFile_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void menuFile_Click(object sender, EventArgs e)
        {
 
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileDialog fd = new OpenFileDialog();
            fd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            fd.FilterIndex = 1;
            fd.RestoreDirectory = true;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                Text = fd.FileName;
                try
                {
                    StreamReader fs = new StreamReader(fd.FileName);
                    string line;
                    menuSelect.DropDownItems.Clear();
                    while ((line = fs.ReadLine()) != null)
                    {
                        int i;
                        line = line.TrimStart();
                        line = Regex.Replace(line, " {2,}", " ");
                        if(Double.Parse(line.Substring(1, 16)) > tmax)
                            tmax = Double.Parse(line.Substring(1, 16));
                        if (Double.Parse(line.Substring(1, 16)) < tmin)
                            tmin = Double.Parse(line.Substring(1, 16));
                        for (i = 0; i < menuSelect.DropDownItems.Count; ++i)
                        {
                            if (menuSelect.DropDownItems[i].Text == line.Substring(25, 3))
                                break;
                        }
                        if (i == menuSelect.DropDownItems.Count)
                            menuSelect.DropDownItems.Add(line.Substring(25, 3));
                    }
                    fs.Close();

                    double dt = tmax - tmin;
                    double exp = Math.Floor(Math.Log10(dt));
                    dt = Math.Ceiling(dt / Math.Pow(10, exp));
                    if (dt > 2)
                    {
                        if (dt > 5)
                            dt = 10 * Math.Pow(10, exp);
                        else
                            dt = 5 * Math.Pow(10, exp);
                    }
                    else
                        dt = 2 * Math.Pow(10, exp);

                    tmax = tmin + dt;

                    Axis ax = chart1.ChartAreas["ChartArea1"].AxisX;
                    ax.MajorGrid.Interval = ax.MajorTickMark.Interval = ax.LabelStyle.Interval = (tmax - tmin) / 10;
                    ax.MinorGrid.Interval = ax.MajorGrid.Interval / 10;
                    ax.Minimum = tmin;
                    ax.Maximum = tmax;
                    ax.LabelStyle.IntervalOffset = Math.Truncate(tmin) - tmin;
                    
                }
                catch
                {
                    MessageBox.Show("tega fila se ne da brat...");
                }
            }
        }

        private void menuSelect_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {   
            foreach (Series s in  chart1.Series)
            {
               s.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
               if (s.Name == e.ClickedItem.Text)
               {
                   foreach(ToolStripMenuItem m in menuSelect.DropDownItems)
                   {
                       if (m.Text == e.ClickedItem.Text)
                           m.Checked = false;
                   }
                   chart1.Series.Remove(s);
                   Display();
                   return;
               }
             }
            chart1.Series.Add(e.ClickedItem.Text);
            chart1.Series[e.ClickedItem.Text].BorderWidth = 3;
            foreach (ToolStripMenuItem m in menuSelect.DropDownItems)
            {
                if (m.Text == e.ClickedItem.Text)
                    m.Checked = true;
            }
            Display();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            chart1.Width = Width;
            chart1.Height = Height - 2*menuMain.Height;
            chart1.Left = 0;
            chart1.Top = menuMain.Height;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chart1.Width = Width;
            chart1.Height = Height - 2*menuMain.Height;
            chart1.Left = 0;
            chart1.Top = menuMain.Height;
        }

        private void chart1_AxisViewChanged(object sender, ViewEventArgs e)
        {
            double dt = e.Axis.ScaleView.ViewMaximum - e.Axis.ScaleView.ViewMinimum;
            double exp = Math.Floor(Math.Log10(dt));
            dt = Math.Ceiling(dt / Math.Pow(10, exp));

            if (dt > 2)
            {
                if (dt > 5)
                    dt = 10 * Math.Pow(10, exp);
                else
                    dt = 5 * Math.Pow(10, exp);
            }
            else
                dt = 2 * Math.Pow(10, exp);

            e.Axis.MajorGrid.Interval = e.Axis.MajorTickMark.Interval = e.Axis.LabelStyle.Interval = dt / 10;
            e.Axis.MinorGrid.Interval = e.Axis.MajorGrid.Interval / 10;
            e.Axis.LabelStyle.IntervalOffset = Math.Truncate(e.Axis.ScaleView.ViewMinimum) - e.Axis.ScaleView.ViewMinimum;
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Application.MessageLoop)
                System.Windows.Forms.Application.Exit();
            else
                System.Environment.Exit(1);
        }
    }
}
