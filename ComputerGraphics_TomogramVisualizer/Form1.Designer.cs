using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.WinForms;

namespace ComputerGraphics_TomogramVisualizer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.glControl1 = new OpenTK.WinForms.GLControl();
            this.trackBarZ = new System.Windows.Forms.TrackBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton_QuadStrip = new System.Windows.Forms.RadioButton();
            this.radioButton_Texture = new System.Windows.Forms.RadioButton();
            this.radioButton_Quads = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton_Zoom = new System.Windows.Forms.RadioButton();
            this.radioButton_Stretch = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBar_Minimum = new System.Windows.Forms.TrackBar();
            this.trackBar_Width = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZ)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Minimum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Width)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открытьToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(732, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // открытьToolStripMenuItem
            // 
            this.открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            this.открытьToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.открытьToolStripMenuItem.Text = "Открыть";
            this.открытьToolStripMenuItem.Click += new System.EventHandler(this.открытьToolStripMenuItem_Click);
            // 
            // glControl1
            // 
            this.glControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl1.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            this.glControl1.APIVersion = new System.Version(3, 3, 0, 0);
            this.glControl1.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            this.glControl1.IsEventDriven = true;
            this.glControl1.Location = new System.Drawing.Point(216, 31);
            this.glControl1.Name = "glControl1";
            this.glControl1.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            this.glControl1.Size = new System.Drawing.Size(504, 400);
            this.glControl1.TabIndex = 2;
            this.glControl1.Text = "glControl1";
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            this.glControl1.Resize += new System.EventHandler(this.glControl1_Resize);
            // 
            // trackBarZ
            // 
            this.trackBarZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarZ.LargeChange = 1;
            this.trackBarZ.Location = new System.Drawing.Point(216, 437);
            this.trackBarZ.Maximum = 0;
            this.trackBarZ.Name = "trackBarZ";
            this.trackBarZ.Size = new System.Drawing.Size(504, 56);
            this.trackBarZ.TabIndex = 3;
            this.trackBarZ.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarZ.Scroll += new System.EventHandler(this.trackBarZ_Scroll);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton_QuadStrip);
            this.groupBox1.Controls.Add(this.radioButton_Texture);
            this.groupBox1.Controls.Add(this.radioButton_Quads);
            this.groupBox1.Location = new System.Drawing.Point(12, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(198, 119);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Варианты визуализации";
            // 
            // radioButton_QuadStrip
            // 
            this.radioButton_QuadStrip.AutoSize = true;
            this.radioButton_QuadStrip.Location = new System.Drawing.Point(6, 86);
            this.radioButton_QuadStrip.Name = "radioButton_QuadStrip";
            this.radioButton_QuadStrip.Size = new System.Drawing.Size(97, 24);
            this.radioButton_QuadStrip.TabIndex = 2;
            this.radioButton_QuadStrip.Text = "QuadStrip";
            this.radioButton_QuadStrip.UseVisualStyleBackColor = true;
            this.radioButton_QuadStrip.CheckedChanged += new System.EventHandler(this.radioButton_QuadStrip_CheckedChanged);
            // 
            // radioButton_Texture
            // 
            this.radioButton_Texture.AutoSize = true;
            this.radioButton_Texture.Location = new System.Drawing.Point(6, 56);
            this.radioButton_Texture.Name = "radioButton_Texture";
            this.radioButton_Texture.Size = new System.Drawing.Size(78, 24);
            this.radioButton_Texture.TabIndex = 1;
            this.radioButton_Texture.Text = "Texture";
            this.radioButton_Texture.UseVisualStyleBackColor = true;
            this.radioButton_Texture.CheckedChanged += new System.EventHandler(this.radioButton_Texture_CheckedChanged);
            // 
            // radioButton_Quads
            // 
            this.radioButton_Quads.AutoSize = true;
            this.radioButton_Quads.Checked = true;
            this.radioButton_Quads.Location = new System.Drawing.Point(6, 26);
            this.radioButton_Quads.Name = "radioButton_Quads";
            this.radioButton_Quads.Size = new System.Drawing.Size(72, 24);
            this.radioButton_Quads.TabIndex = 0;
            this.radioButton_Quads.TabStop = true;
            this.radioButton_Quads.Text = "Quads";
            this.radioButton_Quads.UseVisualStyleBackColor = true;
            this.radioButton_Quads.CheckedChanged += new System.EventHandler(this.radioButton_Quads_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton_Zoom);
            this.groupBox2.Controls.Add(this.radioButton_Stretch);
            this.groupBox2.Location = new System.Drawing.Point(12, 156);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(198, 88);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Size mode";
            // 
            // radioButton_Zoom
            // 
            this.radioButton_Zoom.AutoSize = true;
            this.radioButton_Zoom.Checked = true;
            this.radioButton_Zoom.Location = new System.Drawing.Point(6, 56);
            this.radioButton_Zoom.Name = "radioButton_Zoom";
            this.radioButton_Zoom.Size = new System.Drawing.Size(70, 24);
            this.radioButton_Zoom.TabIndex = 1;
            this.radioButton_Zoom.TabStop = true;
            this.radioButton_Zoom.Text = "Zoom";
            this.radioButton_Zoom.UseVisualStyleBackColor = true;
            this.radioButton_Zoom.CheckedChanged += new System.EventHandler(this.radioButton_Zoom_CheckedChanged);
            // 
            // radioButton_Stretch
            // 
            this.radioButton_Stretch.AutoSize = true;
            this.radioButton_Stretch.Location = new System.Drawing.Point(6, 26);
            this.radioButton_Stretch.Name = "radioButton_Stretch";
            this.radioButton_Stretch.Size = new System.Drawing.Size(76, 24);
            this.radioButton_Stretch.TabIndex = 0;
            this.radioButton_Stretch.Text = "Stretch";
            this.radioButton_Stretch.UseVisualStyleBackColor = true;
            this.radioButton_Stretch.CheckedChanged += new System.EventHandler(this.radioButton_Stretch_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(163, 450);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Слой:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 247);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "TFminimum";
            // 
            // trackBar_Minimum
            // 
            this.trackBar_Minimum.Location = new System.Drawing.Point(12, 270);
            this.trackBar_Minimum.Maximum = 255;
            this.trackBar_Minimum.Name = "trackBar_Minimum";
            this.trackBar_Minimum.Size = new System.Drawing.Size(198, 56);
            this.trackBar_Minimum.TabIndex = 8;
            this.trackBar_Minimum.Scroll += new System.EventHandler(this.trackBar_Minimum_Scroll);
            // 
            // trackBar_Width
            // 
            this.trackBar_Width.Location = new System.Drawing.Point(12, 352);
            this.trackBar_Width.Maximum = 255;
            this.trackBar_Width.Minimum = 1;
            this.trackBar_Width.Name = "trackBar_Width";
            this.trackBar_Width.Size = new System.Drawing.Size(198, 56);
            this.trackBar_Width.TabIndex = 10;
            this.trackBar_Width.Value = 1;
            this.trackBar_Width.Scroll += new System.EventHandler(this.trackBar_Width_Scroll);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 329);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "TFwidth";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 503);
            this.Controls.Add(this.trackBar_Width);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.trackBar_Minimum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.trackBarZ);
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(750, 550);
            this.Name = "Form1";
            this.Text = "Tomogram Visualizer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZ)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Minimum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Width)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private GLControl glControl1;
        private ToolStripMenuItem открытьToolStripMenuItem;
        private TrackBar trackBarZ;
        private GroupBox groupBox1;
        private RadioButton radioButton_QuadStrip;
        private RadioButton radioButton_Texture;
        private RadioButton radioButton_Quads;
        private GroupBox groupBox2;
        private RadioButton radioButton_Zoom;
        private RadioButton radioButton_Stretch;
        private Label label1;
        private Label label2;
        private TrackBar trackBar_Minimum;
        private TrackBar trackBar_Width;
        private Label label3;
    }
}