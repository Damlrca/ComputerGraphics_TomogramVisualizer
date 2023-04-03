using OpenTK.WinForms;

namespace Tomogram_3D
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
            components = new System.ComponentModel.Container();
            glControl1 = new GLControl();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            timer1 = new System.Windows.Forms.Timer(components);
            groupBox1 = new System.Windows.Forms.GroupBox();
            radioButton_Persp = new System.Windows.Forms.RadioButton();
            radioButton_Ortho = new System.Windows.Forms.RadioButton();
            menuStrip1.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // glControl1
            // 
            glControl1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            glControl1.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            glControl1.APIVersion = new System.Version(3, 3, 0, 0);
            glControl1.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            glControl1.IsEventDriven = true;
            glControl1.Location = new System.Drawing.Point(216, 31);
            glControl1.Name = "glControl1";
            glControl1.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            glControl1.Size = new System.Drawing.Size(504, 460);
            glControl1.TabIndex = 3;
            glControl1.Text = "glControl1";
            glControl1.Load += glControl1_Load;
            glControl1.Paint += glControl1_Paint;
            glControl1.Resize += glControl1_Resize;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { открытьToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(732, 28);
            menuStrip1.TabIndex = 4;
            menuStrip1.Text = "menuStrip1";
            // 
            // открытьToolStripMenuItem
            // 
            открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            открытьToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            открытьToolStripMenuItem.Text = "Открыть";
            открытьToolStripMenuItem.Click += Open_ToolStripMenuItem_Click;
            // 
            // timer1
            // 
            timer1.Interval = 40;
            timer1.Tick += timer1_Tick;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioButton_Ortho);
            groupBox1.Controls.Add(radioButton_Persp);
            groupBox1.Location = new System.Drawing.Point(12, 31);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(198, 88);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "Projection";
            // 
            // radioButton_Persp
            // 
            radioButton_Persp.AutoSize = true;
            radioButton_Persp.Checked = true;
            radioButton_Persp.Location = new System.Drawing.Point(6, 26);
            radioButton_Persp.Name = "radioButton_Persp";
            radioButton_Persp.Size = new System.Drawing.Size(104, 24);
            radioButton_Persp.TabIndex = 0;
            radioButton_Persp.TabStop = true;
            radioButton_Persp.Text = "Perspective";
            radioButton_Persp.UseVisualStyleBackColor = true;
            radioButton_Persp.CheckedChanged += radioButton_Persp_CheckedChanged;
            // 
            // radioButton_Ortho
            // 
            radioButton_Ortho.AutoSize = true;
            radioButton_Ortho.Location = new System.Drawing.Point(6, 56);
            radioButton_Ortho.Name = "radioButton_Ortho";
            radioButton_Ortho.Size = new System.Drawing.Size(118, 24);
            radioButton_Ortho.TabIndex = 1;
            radioButton_Ortho.Text = "Orthographic";
            radioButton_Ortho.UseVisualStyleBackColor = true;
            radioButton_Ortho.CheckedChanged += radioButton_Ortho_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(732, 503);
            Controls.Add(groupBox1);
            Controls.Add(glControl1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            MinimumSize = new System.Drawing.Size(750, 550);
            Name = "Form1";
            Text = "Tomogram 3D";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GLControl glControl1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem открытьToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton_Ortho;
        private System.Windows.Forms.RadioButton radioButton_Persp;
    }
}
