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
    public class View
    {
        public static int min= 0;
        public static int sr = 255;
        public static void SetupView(int width, int height)
        {
            GL.ShadeModel(ShadingModel.Smooth);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, Bin.X, 0, Bin.Y, -1, 1);
            //GL.Viewport(0, 0, width, height);
            ChangeView(width, height);
        }

        public static void ChangeView(int GLwidth, int GLheight)
        {
            //GL.Viewport(0, 0, GLwidth, GLheight);
            // Bin.X / Bin.Y == glControl1.Width / glControl1.Height
            // Bin.X / Bin.Y * glControl1.Height == glControl1.Width 
            // glControl1.Width * Bin.Y / Bin.X == glControl1.Height
            if (Bin.is_loaded)
            {
                if (GLheight * Bin.X / Bin.Y <= GLwidth)
                {
                    int width = GLheight * Bin.X / Bin.Y;
                    GL.Viewport((GLwidth - width) / 2, 0, width, GLheight);
                }
                else
                {
                    int height = GLwidth * Bin.Y / Bin.X;
                    GL.Viewport(0, (GLheight - height) / 2, GLwidth, height);
                }
            }
        }

        public static Color TransferFunction(short value)
        {
            int max = min + sr;
            int newVal = Math.Clamp((value - min) * 255 / (max - min), 0, 255);
            return Color.FromArgb(newVal, newVal, newVal);
        }

        public static void DrawQuads(int layerNumber, int min = 0, int sr = 255)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Begin(PrimitiveType.Quads);
            for (int x_coord = 0; x_coord < Bin.X - 1; x_coord++)
                for (int y_coord = 0; y_coord < Bin.Y - 1; y_coord++)
                {
                    int offset = layerNumber * Bin.X * Bin.Y + y_coord * Bin.X;
                    short value;
                    value = Bin.array[x_coord + offset];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x_coord, y_coord);
                    value = Bin.array[x_coord + Bin.X + offset];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x_coord, y_coord + 1);
                    value = Bin.array[(x_coord + 1) + Bin.X + offset];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x_coord + 1, y_coord + 1);
                    value = Bin.array[(x_coord + 1) + offset];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x_coord + 1, y_coord);
                }
            GL.End();
        }
    }
}
