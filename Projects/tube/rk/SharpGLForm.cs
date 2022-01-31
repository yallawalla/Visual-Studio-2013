using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SharpGL;

namespace rk
{
    /// <summary>
    /// The main form class.
    /// </summary>
    public partial class SharpGLForm : Form
    {
        private const int   N =5000;
        private int mouseX = 0, mouseY = 0;
        private float rotX = 0.0f, rotY = 0.0f;
        private Form1 f;

        struct coord {public double x=0,y=0,z=0;};
        struct cam {
          coord norm,up;
          double hRadians = 0, vRadians = 0;
          public void SetView(float h, float v) {
              norm.x = Math.Cos(vRadians) * Math.Sin(hRadians); 
              norm.y = -Math.Sin(vRadians);
              norm.z = Math.Cos(vRadians) * Math.Sin(hRadians); 

              up.x = Math.Sin(vRadians) * Math.Sin(hRadians);
              up.y = Math.Cos(vRadians);
              up.z = Math.Sin(vRadians) * Math.Cos(hRadians);
          }

          public void View(cam c)
          {
              OpenGL gl = openGLControl.OpenGL;
              gl.LookAt(c.pos.x, c.pos.y, c.pos.z,
            cam_pos.x + cam_norm.x, cam + pos.y + cam_norm.y, camp_pos.z + cam_norm.z,
            cam_up.x, cam_up.y, cam_up.z); 
          }
        }
        




        [DllImport("..\\..\\..\\rkdll\\Debug\\rkdll.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void rk4open(int n);
        [DllImport("..\\..\\..\\rkdll\\Debug\\rkdll.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void rk4close();
        [DllImport("..\\..\\..\\rkdll\\Debug\\rkdll.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern float rk4x(int n);
        [DllImport("..\\..\\..\\rkdll\\Debug\\rkdll.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern float rk4y(int n);
        [DllImport("..\\..\\..\\rkdll\\Debug\\rkdll.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern float rk4z(int n);
        [DllImport("..\\..\\..\\rkdll\\Debug\\rkdll.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int rk4(float xo, float yo, float zo, int n);
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpGLForm"/> class.
        /// </summary>
        public SharpGLForm()
        {
            InitializeComponent();
            openGLControl.MouseUp += new System.Windows.Forms.MouseEventHandler(glMouseUp);
            openGLControl.MouseDown += new System.Windows.Forms.MouseEventHandler(glMouseDown);
            openGLControl.MouseMove += new System.Windows.Forms.MouseEventHandler(glMouseMove);
            openGLControl.MouseWheel += new System.Windows.Forms.MouseEventHandler(glMouseWheel);

            rk4open(N);
            int k=rk4(100,0,0, N);
            try
            {
                f = new Form1();
                f.Show();
            }
                catch {}

            if (k > 0)
            {
                f.hit(rk4z(k), rk4y(k));
            }
 
        }

        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RenderEventArgs"/> instance containing the event data.</param>
        void RotateCamera(float h, float v)
        { 
          hRadians += h; 
          vRadians += v; 

          cam_norm.x = cos(vRadians) * sin(hRadians); 
          cam_norm.y = -sin(vRadians);
          cam_norm.z = cos(vRadians) * sin(hRadians); 

          cam_up.x = sin(vRadians) * sin(hRadians);
          cam_up.y = cos(vRadians);
          cam_up.z = sin(vRadians) * cos(hRadians);
        } 

        private void openGLControl_OpenGLDraw(object sender, RenderEventArgs e)
        {
            OpenGL gl = openGLControl.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();

            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Ortho(-Width / 2, Width / 2, -Height/2, Height/2, -1000, 1000);

            gl.LookAt(10,10, 10, 0, 0, 0, 0, 1, 0);
            gl.Rotate(hr,vr, 0);

            gl.Begin(OpenGL.GL_LINES);
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(-1.0f, 0.0f, 0.0f);
            gl.Vertex(100.0f, 0.0f, 0.0f);
            gl.End();
            gl.Begin(OpenGL.GL_LINES);
            gl.Color(0.0f, 1.0f, 0.0f);
            gl.Vertex(0.0f, -1.0f, 0.0f);
            gl.Vertex(0.0f, 100.0f, 0.0f);
            gl.End();
            gl.Begin(OpenGL.GL_LINES);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(0.0f, 0.0f, -1.0f);
            gl.Vertex(0.0f, 0.0f, 100.0f);
            gl.End();

            gl.Begin(OpenGL.GL_LINE_STRIP);
            gl.Color(1.0f, 1.0f, 0.0f);
            gl.Vertex(0.0f, 0.0f, 0.0f);

            for (int i = 0; i < N-1; ++i)
            {
                gl.Vertex(rk4x(i), rk4y(i), rk4z(i));
            }
            gl.End();
        }

        private void glMouseUp(object sender, MouseEventArgs e)
        {
            mouseX = mouseY = 0;
        }
        private void glMouseDown(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;
        }
        private void glMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Math.Abs(e.X - mouseX) > Math.Abs(e.Y - mouseY))
                {
                    vr -= e.X - mouseX;
                }
                else
                {
                 hr += e.Y - mouseY;
                }
                mouseX = e.X;
                mouseY = e.Y;
            }
            if (e.Button == MouseButtons.Right)
            {
                rotX -= (e.X - mouseX);
                mouseX = e.X;
                rotY += (e.Y - mouseY);
                mouseY = e.Y;
            }
        }
        private void glMouseWheel(object sender, MouseEventArgs e)
        {

        }

        /// <summary>
        /// Handles the OpenGLInitialized event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            //  TODO: Initialise OpenGL here.

            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Set the clear color.
            gl.ClearColor(0, 0, 0, 0);
        }

        /// <summary>
        /// Handles the Resized event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
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
            gl.LookAt(5, 5, 5, 0, 0, 0, 0, 1, 0);

            //  Set the modelview matrix.
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        /// <summary>
        /// The current rotation.
        /// </summary>
    }
}
