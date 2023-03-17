using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputerGraphics_TomogramVisualizer
{
    public partial class Form1 : Form
    {
        private bool is_loaded = false;
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
                string str = ofd.FileName;
                Bin.readBin(ofd.FileName);
                View.SetupView(glControl1.Width, glControl1.Height);
                is_loaded = true;
                glControl1.Invalidate();
                trackBar1.Maximum = Bin.Z - 1;
            }
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (is_loaded)
            {
                View.DrawQuads(currentLayer);
                glControl1.SwapBuffers();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            currentLayer = trackBar1.Value;
            if (is_loaded)
            {
                View.DrawQuads(currentLayer);
                glControl1.SwapBuffers();
            }
        }
    }
}