using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;

namespace ComputerGraphics_TomogramVisualizer
{
    public static class View
    {
        //public static int min = 0;
        //public static int sr = 255;

        public static int min { get; set; } = 0;
        public static int sr { get; set; } = 255;

        public static bool zoom_mode { get; set; } = true;

        private static Bitmap? textureImage = null;
        private static int VBOtexture = 10;

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
            // GL.Viewport(0, 0, GLwidth, GLheight);
            // Bin.X / Bin.Y == glControl1.Width / glControl1.Height
            // Bin.X / Bin.Y * glControl1.Height == glControl1.Width 
            // glControl1.Width * Bin.Y / Bin.X == glControl1.Height
            if (Bin.is_loaded)
            {
                if (zoom_mode)
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
                else
                {
                    GL.Viewport(0, 0, GLwidth, GLheight);
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

        public static void Load2DTexture()
        {
            GL.BindTexture(TextureTarget.Texture2D, VBOtexture);
            BitmapData data = textureImage.LockBits(new System.Drawing.Rectangle(0, 0, textureImage.Width, textureImage.Height),
                                                    ImageLockMode.ReadOnly,
                                                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                          data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                          PixelType.UnsignedByte, data.Scan0);
            textureImage.UnlockBits(data);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                            (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                            (int)TextureMagFilter.Linear);
            //ErrorCode Er = GL.GetError();
            //string str = Er.ToString();
        }

        public static void generateTextureImage(int layerNumber)
        {
            if (textureImage != null)
                textureImage.Dispose();
            textureImage = new Bitmap(Bin.X, Bin.Y);
            for (int i=0; i< Bin.X; i++)
                for(int j=0; j<Bin.Y; j++)
                {
                    int pixelNumber = i + j * Bin.X + layerNumber * Bin.X * Bin.Y;
                    textureImage.SetPixel(i, j, TransferFunction(Bin.array[pixelNumber]));
                }
        }
        public static void DrawTexture()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, VBOtexture);

            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.White);
            GL.TexCoord2(0f, 0f);
            GL.Vertex2(0, 0);
            GL.TexCoord2(0f, 1f);
            GL.Vertex2(0, Bin.Y);
            GL.TexCoord2(1f, 1f);
            GL.Vertex2(Bin.X, Bin.Y);
            GL.TexCoord2(1f, 0f);
            GL.Vertex2(Bin.X, 0);
            GL.End();

            GL.Disable(EnableCap.Texture2D);
        }
    }
}
