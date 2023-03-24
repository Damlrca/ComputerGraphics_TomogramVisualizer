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

        private void îòêðûòüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Bin.readBin(ofd.FileName);
                trackBarZ.Maximum = Bin.Z - 1;
                currentLayer = trackBarZ.Value = 0;
                View.need_reload = true;
                View.SetupView(glControl1.Width, glControl1.Height);
                glControl1.Invalidate();
            }
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (Bin.is_loaded)
            {
                View.Draw(currentLayer);
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
            View.need_reload = true;
            glControl1.Invalidate();
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            View.ChangeView(glControl1.Width, glControl1.Height);
            glControl1.Invalidate();
        }

        private void radioButton_Zoom_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Zoom.Checked)
            {
                View.zoom_mode = true;
                glControl1_Resize(glControl1, EventArgs.Empty);
            }
        }

        private void radioButton_Stretch_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Stretch.Checked)
            {
                View.zoom_mode = false;
                glControl1_Resize(glControl1, EventArgs.Empty);
            }
        }

        private void radioButton_Quads_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Quads.Checked)
            {
                View.vis_mode = VisMode.Quads;
                glControl1.Invalidate();
            }
        }

        private void radioButton_Texture_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Texture.Checked)
            {
                View.need_reload = true;
                View.vis_mode = VisMode.Texture;
                glControl1.Invalidate();
            }
        }

        private void radioButton_QuadStrip_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_QuadStrip.Checked)
            {
                View.vis_mode = VisMode.QuadStrip;
                glControl1.Invalidate();
            }
        }

        private void trackBar_Minimum_Scroll(object sender, EventArgs e)
        {
            View.min = trackBar_Minimum.Value;
            View.need_reload = true;
            glControl1.Invalidate();
        }

        private void trackBar_Width_Scroll(object sender, EventArgs e)
        {
            View.sr = trackBar_Width.Value;
            View.need_reload = true;
            glControl1.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            trackBar_Minimum.Value = View.min;
            trackBar_Width.Value = View.sr;
        }
    }
}