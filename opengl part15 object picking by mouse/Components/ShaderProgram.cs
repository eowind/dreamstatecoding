using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace techdump.opengl.Components
{
    public class ShaderProgram : IDisposable
    {
        public int Id => _program;
        private readonly int _program;
        private readonly List<int> _shaders = new List<int>(); 
        public ShaderProgram()
        {
            _program = GL.CreateProgram();

        }

        public void Link()
        {
            foreach (var shader in _shaders)
                GL.AttachShader(_program, shader);
            GL.LinkProgram(_program);
            var info = GL.GetProgramInfoLog(_program);
            if (!string.IsNullOrWhiteSpace(info))
                Debug.WriteLine($"GL.LinkProgram had info log: {info}");

            foreach (var shader in _shaders)
            {
                GL.DetachShader(_program, shader);
                GL.DeleteShader(shader);
            }
        }

        public void AddShader(ShaderType type, string path)
        {
            var shader = GL.CreateShader(type);
            var src = File.ReadAllText(path);
            GL.ShaderSource(shader, src);
            GL.CompileShader(shader);
            var info = GL.GetShaderInfoLog(shader);
            if (!string.IsNullOrWhiteSpace(info))
                Debug.WriteLine($"GL.CompileShader [{type}] ({path}) had info log: {info}");
            _shaders.Add(shader);
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
                GL.DeleteProgram(_program);
            }
        }
    }
}