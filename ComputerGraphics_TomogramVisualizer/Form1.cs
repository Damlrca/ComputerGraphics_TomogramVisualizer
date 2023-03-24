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

        bool needReload = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void îòêðûòüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Bin.readBin(ofd.FileName);
                trackBarZ.Maximum = Bin.Z - 1;
                currentLayer = trackBarZ.Value = 0;
                trackBarMin.Maximum = trackBarS.Maximum = 255;
                trackBarS.Minimum = 1;
                View.SetupView(glControl1.Width, glControl1.Height);
                glControl1.Invalidate();
            }
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            Console.WriteLine("quads");
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

        private void glControl1_Paint_Texture(object sender, PaintEventArgs e)
        {
            Console.WriteLine("texture");
            if (Bin.is_loaded)
            {
                if (needReload)
                {
                    View.generateTextureImage(currentLayer);
                    View.Load2DTexture();
                    needReload = false;
                }
                View.DrawTexture();
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
            needReload = true;
            glControl1.Invalidate();
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            View.ChangeView(glControl1.Width, glControl1.Height);
            glControl1.Invalidate();
        }

        private void trackBarMin_Scroll(object sender, EventArgs e)
        {
            View.min = trackBarMin.Value;
            needReload = true;
            glControl1.Invalidate();
        }

        private void trackBarS_Scroll(object sender, EventArgs e)
        {
            View.sr = trackBarS.Value;
            needReload = true;
            glControl1.Invalidate();
        }

        private void radioButtonTexture_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonTexture.Checked)
            {
                glControl1.Paint -= glControl1_Paint;
                glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint_Texture);
            }
        }

        private void radioButtonQuads_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonQuads.Checked)
            {
                glControl1.Paint -= glControl1_Paint_Texture;
                glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                View.zoom_mode = false;
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                View.zoom_mode = true;
            }
        }
    }
}