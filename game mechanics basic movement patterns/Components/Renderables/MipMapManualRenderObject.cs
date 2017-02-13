using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK.Graphics.OpenGL4;

namespace techdump.opengl.Components.Renderables
{
    public class MipMapManualRenderObject : ARenderable
    {
        private int _minMipmapLevel = 0;
        private int _maxMipmapLevel;
        private int _texture;
        public MipMapManualRenderObject(TexturedVertex[] vertices, int program, string filename, int maxMipmapLevel)
            : base(program, vertices.Length)
        {
            _maxMipmapLevel = maxMipmapLevel;
            // create first buffer: vertex
            GL.NamedBufferStorage(
                Buffer,
                TexturedVertex.Size * vertices.Length,        // the size needed by this buffer
                vertices,                           // data to initialize with
                BufferStorageFlags.MapWriteBit);    // at this point we will only write to the buffer
            
            GL.VertexArrayAttribBinding(VertexArray, 0, 0);
            GL.EnableVertexArrayAttrib(VertexArray, 0);
            GL.VertexArrayAttribFormat(
                VertexArray,
                0,                      // attribute index, from the shader location = 0
                4,                      // size of attribute, vec4
                VertexAttribType.Float, // contains floats
                false,                  // does not need to be normalized as it is already, floats ignore this flag anyway
                0);                     // relative offset, first item
            
            GL.VertexArrayAttribBinding(VertexArray, 1, 0);
            GL.EnableVertexArrayAttrib(VertexArray, 1);
            GL.VertexArrayAttribFormat(
                VertexArray,
                1,                      // attribute index, from the shader location = 1
                2,                      // size of attribute, vec2
                VertexAttribType.Float, // contains floats
                false,                  // does not need to be normalized as it is already, floats ignore this flag anyway
                16);                     // relative offset after a vec4

            // link the vertex array and buffer and provide the stride as size of Vertex
            GL.VertexArrayVertexBuffer(VertexArray, 0, Buffer, IntPtr.Zero, TexturedVertex.Size);

            InitTextures(filename);
        }

        private void InitTextures(string filename)
        {
            var data = LoadTexture(filename);
            GL.CreateTextures(TextureTarget.Texture2D, 1, out _texture);
            GL.BindTexture(TextureTarget.Texture2D, _texture);
            GL.TextureStorage2D(
                _texture,
                _maxMipmapLevel,             // levels of mipmapping
                SizedInternalFormat.Rgba32f, // format of texture
                data.First().Width,
                data.First().Height); 

            for (int m = 0; m < data.Count; m++)
            {
                var mipLevel = data[m];
                GL.TextureSubImage2D(_texture,
                    m,                  // this is level m
                    0,                  // x offset
                    0,                  // y offset
                    mipLevel.Width,
                    mipLevel.Height,
                    PixelFormat.Rgba,
                    PixelType.Float,
                    mipLevel.Data);
            }
            
            var textureMinFilter = (int)All.LinearMipmapLinear;
            GL.TextureParameterI(_texture, All.TextureMinFilter, ref textureMinFilter);
            var textureMagFilter = (int)All.Linear;
            GL.TextureParameterI(_texture, All.TextureMagFilter, ref textureMagFilter);
            // data not needed from here on, OpenGL has the data
        }
        
        private List<MipLevel> LoadTexture(string filename)
        {
            var mipmapLevels = new List<MipLevel>();
            using (var bmp = (Bitmap)Image.FromFile(filename))
            {
                int xOffset = 0;
                int width = bmp.Width;
                int height = (bmp.Height/3)*2;
                var originalHeight = height;
                for (int m = 0; m < _maxMipmapLevel; m++)
                {
                    xOffset += m == 0 || m == 1 ? 0 : width*2;
                    var yOffset = m == 0 ? 0 : originalHeight;

                    MipLevel mipLevel;
                    mipLevel.Level = m;
                    mipLevel.Width = width;
                    mipLevel.Height = height;
                    mipLevel.Data = new float[mipLevel.Width * mipLevel.Height * 4];
                    int index = 0;
                    ExtractMipmapLevel(yOffset, mipLevel, xOffset, bmp, index);
                    mipmapLevels.Add(mipLevel);

                    if (width == 1 && height == 1)
                    {
                        _maxMipmapLevel = m;
                        break;
                    }

                    width /= 2;
                    if (width < 1)
                        width = 1;
                    height /= 2;
                    if (height < 1)
                        height = 1;
                }
            }
            return mipmapLevels;
        }

        private static void ExtractMipmapLevel(int yOffset, MipLevel mipLevel, int xOffset, Bitmap bmp, int index)
        {
            var width = xOffset + mipLevel.Width;
            var height = yOffset + mipLevel.Height;
            for (int y = yOffset; y < height; y++)
            {
                for (int x = xOffset; x < width; x++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    mipLevel.Data[index++] = pixel.R/255f;
                    mipLevel.Data[index++] = pixel.G/255f;
                    mipLevel.Data[index++] = pixel.B/255f;
                    mipLevel.Data[index++] = pixel.A/255f;
                }
            }
        }

        public override void Bind()
        {
            base.Bind();
            GL.BindTexture(TextureTarget.Texture2D, _texture);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                GL.DeleteTexture(_texture);
            }
            base.Dispose(disposing);
        }

        public struct MipLevel
        {
            public int Level;
            public int Width;
            public int Height;
            public float[] Data;
        }
    }
}
