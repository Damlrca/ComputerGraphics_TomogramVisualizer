using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace Tomogram_Utilities
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
        public static int width { get; set; } = 255;
        public static bool zoom_mode { get; set; } = true;
        public static VisMode vis_mode { get; set; } = VisMode.Quads;

        public static void SetupView(int width, int height)
        {
            GL.ShadeModel(ShadingModel.Smooth);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, Bin.X - 1, 0, Bin.Y - 1, -1, 1);
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

        public static byte TransferFunction(short value)
        {
            int max = min + width;
            byte newVal = (byte)Math.Clamp((value - min) * 255 / (max - min), 0, 255);
            return newVal;
        }

        public static bool need_reload { get; set; } = true;

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
                        // Медленные функции из методички:
                        // generateTextureImage(layerNumber);
                        // Load2DTexture();
                        // Более быстрая функция:
                        generateAndLoad2DTexture(layerNumber);
                        need_reload = false;
                    }
                    DrawTexture();
                    break;
                case VisMode.QuadStrip:
                    DrawQuadStrip(layerNumber);
                    break;
                default:
                    //???
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
                    // 1 вершина
                    value = TransferFunction(Bin.array[x_coord + offset]);
                    GL.Color3(value, value, value);
                    GL.Vertex2(x_coord, y_coord);
                    // 2 вершина
                    value = TransferFunction(Bin.array[x_coord + Bin.X + offset]);
                    GL.Color3(value, value, value);
                    GL.Vertex2(x_coord, y_coord + 1);
                    // 3 вершина
                    value = TransferFunction(Bin.array[(x_coord + 1) + Bin.X + offset]);
                    GL.Color3(value, value, value);
                    GL.Vertex2(x_coord + 1, y_coord + 1);
                    // 4 вершина
                    value = TransferFunction(Bin.array[(x_coord + 1) + offset]);
                    GL.Color3(value, value, value);
                    GL.Vertex2(x_coord + 1, y_coord);
                }
            GL.End();
        }

        private static void DrawQuadStrip(int layerNumber)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            int offset = layerNumber * Bin.X * Bin.Y;
            for (int y_coord = 0; y_coord < Bin.Y - 1; y_coord++)
            {
                GL.Begin(PrimitiveType.QuadStrip);
                for (int x_coord = 0; x_coord < Bin.X; x_coord++)
                {
                    byte value;
                    // 1 вершина
                    value = TransferFunction(Bin.array[x_coord + y_coord * Bin.X + offset]);
                    GL.Color3(value,value,value);
                    GL.Vertex2(x_coord, y_coord);
                    // 2 вершина
                    value = TransferFunction(Bin.array[x_coord + (y_coord + 1) * Bin.X + offset]);
                    GL.Color3(value,value,value);
                    GL.Vertex2(x_coord, y_coord + 1);
                }
                GL.End();
            }

        }

        private static byte[]? texture_buffer = null;

        private static void generateAndLoad2DTexture(int layerNumber)
        {
            int bytes = Bin.X * Bin.Y * 4;
            if (texture_buffer == null || texture_buffer.Length != bytes)
                texture_buffer = new byte[bytes];

            int offset = layerNumber * Bin.X * Bin.Y;
            for (int i = 0; i < bytes; i += 4, offset++)
            {
                byte t = TransferFunction(Bin.array[offset]);
                texture_buffer[i] = t;
                texture_buffer[i + 1] = t;
                texture_buffer[i + 2] = t;
                texture_buffer[i + 3] = 255;
            }

            GL.BindTexture(TextureTarget.Texture2D, VBOtexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                          Bin.X, Bin.Y, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                          PixelType.UnsignedByte, texture_buffer);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                            (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                            (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                            (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                            (int)TextureWrapMode.ClampToEdge);
        }

        private static void DrawTexture()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, VBOtexture);

            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.White);
            // 1 вершина
            GL.TexCoord2(0f, 0f);
            GL.Vertex2(-0.5f, -0.5f);
            // 2 вершина
            GL.TexCoord2(0f, 1f);
            GL.Vertex2(-0.5f, Bin.Y - 0.5f);
            // 3 вершина
            GL.TexCoord2(1f, 1f);
            GL.Vertex2(Bin.X - 0.5f, Bin.Y - 0.5f);
            // 4 вершина
            GL.TexCoord2(1f, 0f);
            GL.Vertex2(Bin.X - 0.5f, -0.5f);
            GL.End();

            GL.Disable(EnableCap.Texture2D);
        }

        // Медленные функции из методички:

        private static Bitmap? textureImage = null;
        private static int VBOtexture = 0;

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
    }
}
