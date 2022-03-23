using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpGL;
using System.IO.Ports;

namespace tiltblock
{
    /// <summary>
    /// The main form class.
    /// </summary>
    public partial class SharpGLForm : Form
    {
        public SerialPort com;
        public Form1 f;
        public class filter
        {
            private float k = 1e-3f;
            public float[] y;
            public filter(float f, float fo)
            {
                y = new float[] { 0, 0 };
                k = 2f*(float)Math.PI*f/fo;
            }
            public float[] eval(float x)
            {
                float d2y = (x - y[0] - 2 * y[1]);
                y[0] += y[1] * k;
                y[1] += d2y * k;
                return y;
            }
        }
        public filter[] acc=new filter[3],gyr=new filter[3];
        public float accx, accy, accz, gyrx, gyry, gyrz, accx0, accy0, accz0, gyrx0, gyry0, gyrz0;
        public float[] goff=new float[] {0,0},aoff=new float[] {0,0};

        public void offset() {
            accx0 = acc[0].y[0];
            accy0 = acc[1].y[0];
            accz0 = acc[2].y[0];
            gyrx0 = gyr[0].y[0];
            gyry0 = gyr[1].y[0];
            gyrz0 = gyr[2].y[0];
            gyrx = gyry = gyrz = 0;
        }
        private void parse(string text)
        {
            string[] n = text.Split(new Char[] { ',', '\r' });
            for (int i = 0; i < 3; ++i)
            {
                acc[i].eval(float.Parse(n[i + 1]));
                gyr[i].eval(float.Parse(n[i + 4]));
            }

            accx = acc[0].y[0] - accx0;
            accy = acc[1].y[0] - accy0;
            accz = acc[2].y[0];
            gyrx += (gyrx0-float.Parse(n[4])  ) * 0.005f;
            gyry += (gyry0-float.Parse(n[5])  ) * 0.005f;
            gyrz = 10000;

            f.plot(accx, gyrx);

            gyrz = 1000;
        }
        private void openGLControl_OpenGLDraw(object sender, RenderEventArgs e)
        {
            OpenGL gl = openGLControl.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();

            gl.Translate(0, -2, 0);
            gl.Rotate(Math.Atan2(accy, accz) / Math.PI * 180f, 0, 0, 1);
            gl.Rotate(-Math.Atan2(accx, accz) / Math.PI * 180f, 1, 0, 0);
            draw(gl,4,2,0.1f);

            gl.LoadIdentity();
            gl.Translate(0, 2, 0);
            gl.Rotate(Math.Atan2(gyry, gyrz) / Math.PI * 180f, 0, 0, 1);
            gl.Rotate(-Math.Atan2(gyrx, gyrz) / Math.PI * 180f, 1, 0, 0);
            draw(gl, 2, 1, 0.2f);
        }

        private delegate void SetTextDeleg(string text);
        public SharpGLForm()
        {
            InitializeComponent();
            com = new SerialPort("COM10", 115200, Parity.None, 8, StopBits.One);
            com.Handshake = Handshake.None;
            com.DataReceived += new SerialDataReceivedEventHandler(comDataReceived);
            com.Open();

            for (int i = 0; i < 3; ++i)
            {
                acc[i] = new filter(1, 200);
                gyr[i] = new filter(1, 200);
            }

            try
            {
                f = new Form1(this);
                f.Show();
                f.Left = Left + Width;
                f.Top = Top;
            }
            catch { }

        }
        void comDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = com.ReadLine();
            try
            {
                this.BeginInvoke(new SetTextDeleg(parse), new object[] { data });
            }
            catch { }
        }
        public void draw(OpenGL gl, float l, float w, float h)
        {
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(1f, 0, 0);
            gl.Vertex(-l, h, w);
            gl.Color(0, 1f, 0);
            gl.Vertex(l, h, w);
            gl.Color(0, 0, 1f);
            gl.Vertex(l, h, -w);
            gl.Color(1f, 1f, 1f);
            gl.Vertex(-l, h, -w);
            gl.Color(1f, 0, 0);
            gl.Vertex(-l, -h, w);
            gl.Color(0, 1f, 0);
            gl.Vertex(l, h, w);
            gl.Color(0, 0, 1f);
            gl.Vertex(l, -h, -w);
            gl.Color(1f, 1f, 1f);
            gl.Vertex(-l, -h, -w);
            gl.End();
            gl.Begin(OpenGL.GL_QUAD_STRIP);
            gl.Color(1f, 1f, 1f);
            gl.Vertex(-l, h, w);
            gl.Vertex(-l, -h, w);
            gl.Color(1f, 0, 0);
            gl.Vertex(l, h, w);
            gl.Vertex(l, -h, w);
            gl.Color(0, 1f, 0);
            gl.Vertex(l, h, -w);
            gl.Vertex(l, -h, -w);
            gl.Color(0, 0, 1f);
            gl.Vertex(-l, h, -w);
            gl.Vertex(-l, -h, -w);
            gl.End();
        }
        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            //  TODO: Initialise OpenGL here.

            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Set the clear color.
            gl.ClearColor(0, 0, 0, 0);
        }
        private void openGLControl_Resized(object sender, EventArgs e)
        {
            //  TODO: Set the projection matrix here.

            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);

            //  Load the identity.
            gl.LoadIdentity();

            //  Create a perspective transformation.
            gl.Perspective(60.0f, (double)Width / (double)Height, 0.01, 100.0);

            //  Use the 'look at' helper function to position and aim the camera.
            gl.LookAt(6, 6, 6, 0, 0, 0, 0, 1, 0);

            //  Set the modelview matrix.
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }
    }
}
