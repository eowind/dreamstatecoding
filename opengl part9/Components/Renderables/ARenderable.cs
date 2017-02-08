using System;
using OpenTK.Graphics.OpenGL4;

namespace techdump.opengl.Components.Renderables
{
    public abstract class ARenderable : IDisposable
    {
        protected readonly int Program;
        protected readonly int VertexArray;
        protected readonly int Buffer;
        protected readonly int VerticeCount;

        protected ARenderable(int program, int vertexCount)
        {
            Program = program;
            VerticeCount = vertexCount;
            VertexArray = GL.GenVertexArray();
            Buffer = GL.GenBuffer();

            GL.BindVertexArray(VertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Buffer);
        }
        public virtual void Bind()
        {
            GL.UseProgram(Program);
            GL.BindVertexArray(VertexArray);
        }
        public virtual void Render()
        {
            GL.DrawArrays(PrimitiveType.Triangles, 0, VerticeCount);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                GL.DeleteVertexArray(VertexArray);
                GL.DeleteBuffer(Buffer);
            }
        }
    }
}