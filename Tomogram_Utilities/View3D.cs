using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Tomogram_Utilities
{
    public static class View3D
    {
        public static void SetupView(int width, int height)
        {
            GL.ShadeModel(ShadingModel.Smooth);
            ChangeView(width, height);
        }

        public static void ChangeView(int width, int height)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            if (height <= width)
            {
                float t = 1.25f * width / height;
                GL.Ortho(-t, t, -1.25f, 1.25f, -2.0f, 2.0f);
            }
            else
            {
                float t = 1.25f * height / width;
                GL.Ortho(-1.25f, 1.25f, -t, t, -2.0f, 2.0f);
            }
            GL.Viewport(0, 0, width, height);
        }

        public static void Render()
        {
            GL.Enable(EnableCap.DepthTest); // for test
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Begin(PrimitiveType.Quads);

            GL.Color4(Color4.Aqua);
            double a = Math.Sqrt(2);
            GL.Vertex3(a, 0, 1);
            GL.Vertex3(0, a, 1);
            GL.Vertex3(-a, 0, 1);
            GL.Vertex3(-0, -a, 1);

            GL.Color4(Color4.Red);
            GL.Vertex3(1, -1, -1);
            GL.Vertex3(1, 1, -1);
            GL.Vertex3(-1, 1, -1);
            GL.Vertex3(-1, -1, -1);

            GL.End();
        }
    }
}
