﻿using OpenTK.WinForms;
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

        private void Open_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Bin.readBin(ofd.FileName);
                View3D.Generate3DTexture();
                timer1.Start();
            }
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            glControl1.Invalidate();
        }

        private void radioButton_Persp_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Persp.Checked)
            {
                View3D.projMode = ProjMode.Persp;
                View3D.ChangeView(glControl1.ClientSize.Width, glControl1.ClientSize.Height);
            }
        }

        private void radioButton_Ortho_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Ortho.Checked)
            {
                View3D.projMode = ProjMode.Ortho;
                View3D.ChangeView(glControl1.ClientSize.Width, glControl1.ClientSize.Height);
            }
        }

        private void trackBar_Alpha_Scroll(object sender, EventArgs e)
        {
            View3D.alpha_coef = trackBar_Alpha.Value;
        }

        private void trackBar_NoS_Scroll(object sender, EventArgs e)
        {
            View3D.number_of_slices = trackBar_NoS.Value;
        }
    }
}
