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
        private const int   N =206;
        private int mouseX = 0, mouseY = 0, lr = 0, bt = 0;
        private float rotX = 0.0f, rotY = 0.0f;
        private Form1 f;

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
            rk4((float)(322.0 / Math.Sqrt(2.0)), (float)(322.0 / Math.Sqrt(2.0)), 0, N);
            try
            {
                f = new Form1();
                f.Show();
            }
                catch {}
 
        }

        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RenderEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLDraw(object sender, RenderEventArgs e)
        {
            OpenGL gl = openGLControl.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();

            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
        //    gl.Rotate(rotY,rotX, 0);
            gl.Ortho(-Width / 2, Width / 2, -Height/2, Height/2, -1000, 1000);

            gl.LookAt(lr/100.0f, bt/100.0f, 1, 0, 0, 0, 0, 1, 0);

            gl.Begin(OpenGL.GL_LINES);
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(-1.0f, 0.0f, 0.0f);
            gl.Vertex(1000.0f, 0.0f, 0.0f);
            gl.End();
            gl.Begin(OpenGL.GL_LINES);
            gl.Color(0.0f, 1.0f, 0.0f);
            gl.Vertex(0.0f, -1.0f, 0.0f);
            gl.Vertex(0.0f, 1000.0f, 0.0f);
            gl.End();
            gl.Begin(OpenGL.GL_LINES);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(0.0f, 0.0f, -1.0f);
            gl.Vertex(0.0f, 0.0f, 1000.0f);
            gl.End();

            gl.Begin(OpenGL.GL_LINE_STRIP);
            gl.Color(1.0f, 1.0f, 0.0f);
            gl.Vertex(0.0f, 0.0f, 0.0f);

            for (int i = 0; i < N-1; ++i)
            {
                gl.Vertex(rk4x(i) / 10, rk4y(i) / 10, rk4z(i) / 10);
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
                lr -= e.X - mouseX;
                mouseX = e.X;
                bt += e.Y - mouseY;
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
            gl.LookAt(-5, 5, -5, 0, 0, 0, 0, 1, 0);

            //  Set the modelview matrix.
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        /// <summary>
        /// The current rotation.
        /// </summary>
    }
}
