using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.WinForms;

namespace Tomogram_Utilities
{
    public enum ProjMode
    {
        Ortho,
        Persp
    }

    public static class View3D
    {
        public static ProjMode projMode = ProjMode.Persp;
        public static int alpha_coef = 0;
        public static int number_of_slices = 300;

        public static void SetupView(int width, int height)
        {
            GL.ShadeModel(ShadingModel.Smooth);
            ChangeView(width, height);
        }

        public static void ChangeView(int width, int height)
        {
            switch (projMode)
            {
                case ProjMode.Ortho:
                    // orthographic projection
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
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadIdentity();
                    GL.Viewport(0, 0, width, height);
                    break;
                case ProjMode.Persp:
                    // perspective projection
                    float aspect_ratio = Math.Max(width, 1) / (float)Math.Max(height, 1);
                    Matrix4 perpective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect_ratio, 1, 64);
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadMatrix(ref perpective);
                    Matrix4 lookat = Matrix4.LookAt(0, 0, 3.5f, 0, 0, 0, 0, 1, 0);
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadMatrix(ref lookat);
                    GL.Viewport(0, 0, width, height);
                    break;
                default:
                    //???
                    break;
            }
        }

        private static int texture_id = 0;

        public static void Generate3DTexture()
        {
            int bytes = Bin.X * Bin.Y * Bin.Z * 4;
            byte[] texture_buffer = new byte[bytes];

            for (int i = 0, j = 0; i < bytes; i+= 4, j++)
            {
                byte t = View.TransferFunction(Bin.array[j]);
                texture_buffer[i] = t;
                texture_buffer[i + 1] = t;
                texture_buffer[i + 2] = t;
                texture_buffer[i + 3] = t;
            }

            GL.BindTexture(TextureTarget.Texture3D, texture_id);
            GL.TexImage3D(TextureTarget.Texture3D, 0, PixelInternalFormat.Rgba,
                          Bin.X, Bin.Y, Bin.Z, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                          PixelType.UnsignedByte, texture_buffer);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMinFilter,
                            (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMagFilter,
                            (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapS,
                            (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapT,
                            (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapR,
                            (int)TextureWrapMode.ClampToBorder);
        }

        static double angle = 0;

        public static void Render()
        {
            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Gequal, alpha_coef / 100f);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Texture3DExt);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (Bin.is_loaded)
            {
                GL.MatrixMode(MatrixMode.Texture);
                GL.LoadIdentity();
                GL.Translate(0.5f, 0.5f, 0.5f);
                angle += 1;
                GL.Rotate(angle, 0, 1, 0);
                GL.Translate(-0.5f, -0.5f, -0.5f);

                GL.BindTexture(TextureTarget.Texture3D, texture_id);
                /*
                GL.Begin(PrimitiveType.Quads);
                for (float z = -2.0f; z <= 2.0f; z += 4.0f / 500)
                {
                    float tex_z = z / 2 - 0.5f;
                    GL.TexCoord3(-0.5f, -0.5f, -tex_z);
                    GL.Vertex3(-2.0f, -2.0f, z);
                    GL.TexCoord3(-0.5f, 1.5f, -tex_z);
                    GL.Vertex3(-2.0f, 2.0f, z);
                    GL.TexCoord3(1.5f, 1.5f, -tex_z);
                    GL.Vertex3(2.0f, 2.0f, z);
                    GL.TexCoord3(1.5f, -0.5f, -tex_z);
                    GL.Vertex3(2.0f, -2.0f, z);
                }
                GL.End();
                */
                // optimized for rotate around Y only:
                double d = Math.Sqrt(2);
                GL.Begin(PrimitiveType.Quads);
                for (double z = -d; z <= d; z += 2 * d / number_of_slices)
                {
                    double tex_z = z / 2 - 0.5;
                    double t = Math.Sqrt(2 - z * z);
                    GL.TexCoord3(-t / 2 + 0.5, 0, -tex_z);
                    GL.Vertex3(-t, -1, z);
                    GL.TexCoord3(-t / 2 + 0.5, 1, -tex_z);
                    GL.Vertex3(-t, 1, z);
                    GL.TexCoord3(t / 2 + 0.5, 1, -tex_z);
                    GL.Vertex3(t, 1, z);
                    GL.TexCoord3(t / 2 + 0.5, 0, -tex_z);
                    GL.Vertex3(t, -1, z);
                }
                GL.End();
            }
        }
    }
}
