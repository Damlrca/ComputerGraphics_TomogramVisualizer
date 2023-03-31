using OpenTK.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Tomogram_Utilities;

namespace Tomogram_3D
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            glControl1_Resize(glControl1, EventArgs.Empty);
            glControl1.MakeCurrent();
            View3D.SetupView(glControl1.ClientSize.Width, glControl1.ClientSize.Height);
        }


        private void glControl1_Resize(object sender, EventArgs e)
        {
            glControl1.MakeCurrent();
            View3D.ChangeView(glControl1.ClientSize.Width, glControl1.ClientSize.Height);
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            glControl1.MakeCurrent();

            View3D.Render();

            glControl1.SwapBuffers();
        }
    }
}
