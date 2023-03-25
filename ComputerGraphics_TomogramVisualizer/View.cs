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
    public enum VisMode
    {
        Quads,
        Texture,
        QuadStrip
    }

    public static class View
    {
        public static int min { get; set; } = 0;
        public static int sr { get; set; } = 255;
        public static bool zoom_mode { get; set; } = true;
        public static VisMode vis_mode { get; set; } = VisMode.Quads;

        private static Bitmap? textureImage = null;
        private static byte[] buffer = null;
        private static int VBOtexture = 0;
        public static bool need_reload { get; set; } = true;

        public static void SetupView(int width, int height)
        {
            GL.ShadeModel(ShadingModel.Smooth);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, Bin.X, 0, Bin.Y, -1, 1);
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

        public static byte TransferFunction(short value)
        {
            int max = min + sr;
            byte newVal = (byte)Math.Clamp((value - min) * 255 / (max - min), 0, 255);
            return newVal;
        }

        public static void Draw(int layerNumber)
        {
            switch (vis_mode)
            {
                case VisMode.Quads:
                    DrawQuads(layerNumber);
                    break;
                case VisMode.Texture:
                    if (need_reload)
                    {
                        //generateTextureImage(layerNumber);
                        //Load2DTexture();
                        generateAndLoad2DTexture(layerNumber);
                        need_reload = false;
                    }
                    DrawTexture();
                    break;
                case VisMode.QuadStrip:

                    break;
                default:
                    break;
            }
        }

        private static void DrawQuads(int layerNumber)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Begin(PrimitiveType.Quads);
            for (int x_coord = 0; x_coord < Bin.X - 1; x_coord++)
                for (int y_coord = 0; y_coord < Bin.Y - 1; y_coord++)
                {
                    int offset = layerNumber * Bin.X * Bin.Y + y_coord * Bin.X;
                    byte value;
                    value = TransferFunction(Bin.array[x_coord + offset]);
                    GL.Color3(value, value, value);
                    GL.Vertex2(x_coord, y_coord);
                    value = TransferFunction(Bin.array[x_coord + Bin.X + offset]);
                    GL.Color3(value, value, value);
                    GL.Vertex2(x_coord, y_coord + 1);
                    value = TransferFunction(Bin.array[(x_coord + 1) + Bin.X + offset]);
                    GL.Color3(value, value, value);
                    GL.Vertex2(x_coord + 1, y_coord + 1);
                    value = TransferFunction(Bin.array[(x_coord + 1) + offset]);
                    GL.Color3(value, value, value);
                    GL.Vertex2(x_coord + 1, y_coord);
                }
            GL.End();
        }

        private static void Load2DTexture()
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

        private static void generateTextureImage(int layerNumber)
        {
            if (textureImage != null)
                textureImage.Dispose();
            textureImage = new Bitmap(Bin.X, Bin.Y);
            for (int i = 0; i < Bin.X; i++)
                for (int j = 0; j < Bin.Y; j++)
                {
                    int pixelNumber = i + j * Bin.X + layerNumber * Bin.X * Bin.Y;
                    byte color = TransferFunction(Bin.array[pixelNumber]);
                    textureImage.SetPixel(i, j, Color.FromArgb(color, color, color));
                }
        }

        private static void generateAndLoad2DTexture(int layerNumber)
        {
            int bytes = Bin.X * Bin.Y * 4;
            if (buffer == null || buffer.Count() != bytes)
                buffer = new byte[bytes];

            int offset = layerNumber * Bin.X * Bin.Y;
            for (int i = 0; i < bytes; i += 4, offset++)
            {
                byte t = TransferFunction(Bin.array[offset]);
                buffer[i] = t;
                buffer[i + 1] = t;
                buffer[i + 2] = t;
                buffer[i + 3] = 255;
            }

            GL.BindTexture(TextureTarget.Texture2D, VBOtexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                          Bin.X, Bin.Y, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                          PixelType.UnsignedByte, buffer);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                            (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                            (int)TextureMagFilter.Linear);
        }

        private static void DrawTexture()
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
