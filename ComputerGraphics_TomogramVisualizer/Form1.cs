using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;

namespace ComputerGraphics_TomogramVisualizer
{
    public partial class Form1 : Form
    {
        int currentLayer = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void �������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Bin.readBin(ofd.FileName);
                trackBarZ.Maximum = Bin.Z - 1;
                currentLayer = trackBarZ.Value = 0;
                View.SetupView(glControl1.Width, glControl1.Height);
                glControl1.Invalidate();
            }
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (Bin.is_loaded)
            {
                View.DrawQuads(currentLayer);
                glControl1.SwapBuffers();
            }
            else
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                glControl1.SwapBuffers();
            }
        }

        private void trackBarZ_Scroll(object sender, EventArgs e)
        {
            currentLayer = trackBarZ.Value;
            glControl1.Invalidate();
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            View.ChangeView(glControl1.Width, glControl1.Height);
            glControl1.Invalidate();
        }
    }
}