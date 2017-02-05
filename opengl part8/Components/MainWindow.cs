using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace techdump.opengl.Components
{
    public sealed class MainWindow : GameWindow
    {
        private readonly string _title;
        private int _program;
        private double _time;
        private List<RenderObject> _renderObjects = new List<RenderObject>();
        private Color4 _backColor = new Color4(0.1f, 0.1f, 0.3f, 1.0f);
        private Matrix4 _projectionMatrix;
        private float _z = -2.7f;
        private float _fov = 60f;
        public MainWindow()
            : base(750, // initial width
                500, // initial height
                GraphicsMode.Default,
                "",  // initial title
                GameWindowFlags.Default,
                DisplayDevice.Default,
                4, // OpenGL major version
                5, // OpenGL minor version
                GraphicsContextFlags.ForwardCompatible)
        {
            _title += "dreamstatecoding.blogspot.com: OpenGL Version: " + GL.GetString(StringName.Version);
        }
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            CreateProjection();
        }


        protected override void OnLoad(EventArgs e)
        {
            VSync = VSyncMode.Off;
            CreateProjection();
            _renderObjects.Add(new RenderObject(ObjectFactory.CreateSolidCube(0.2f, Color4.HotPink)));
            _renderObjects.Add(new RenderObject(ObjectFactory.CreateSolidCube(0.2f, Color4.BlueViolet)));
            _renderObjects.Add(new RenderObject(ObjectFactory.CreateSolidCube(0.2f, Color4.Red)));
            _renderObjects.Add(new RenderObject(ObjectFactory.CreateSolidCube(0.2f, Color4.LimeGreen)));

            CursorVisible = true;

            _program = CreateProgram();
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
            GL.Enable(EnableCap.DepthTest);
            Closed += OnClosed;
        }
        
        private void OnClosed(object sender, EventArgs eventArgs)
        {
            Exit();
        }

        public override void Exit()
        {
            Debug.WriteLine("Exit called");
            foreach(var obj in _renderObjects)
                obj.Dispose();
            GL.DeleteProgram(_program);
            base.Exit();
        }
        
        private int CreateProgram()
        {
            var program = GL.CreateProgram();
            var shaders = new List<int>();
            shaders.Add(CompileShader(ShaderType.VertexShader, @"Components\Shaders\1Vert\simplePipeVert.c"));
            shaders.Add(CompileShader(ShaderType.FragmentShader, @"Components\Shaders\5Frag\simplePipeFrag.c"));

            foreach (var shader in shaders)
                GL.AttachShader(program, shader);
            GL.LinkProgram(program);
            var info = GL.GetProgramInfoLog(program);
            if (!string.IsNullOrWhiteSpace(info))
                Debug.WriteLine($"GL.LinkProgram had info log: {info}");

            foreach (var shader in shaders)
            {
                GL.DetachShader(program, shader);
                GL.DeleteShader(shader);
            }
            return program;
        }

        private int CompileShader(ShaderType type, string path)
        {
            var shader = GL.CreateShader(type);
            var src = File.ReadAllText(path);
            GL.ShaderSource(shader, src);
            GL.CompileShader(shader);
            var info = GL.GetShaderInfoLog(shader);
            if (!string.IsNullOrWhiteSpace(info))
                Debug.WriteLine($"GL.CompileShader [{type}] had info log: {info}");
            return shader;
        }

        private void CreateProjection()
        {
            
            var aspectRatio = (float)Width/Height;
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                _fov*((float) Math.PI/180f), // field of view angle, in radians
                aspectRatio,                // current window aspect ratio
                0.1f,                       // near plane
                4000f);                     // far plane
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {


            HandleKeyboard(e.Time);
        }
        private void HandleKeyboard(double dt)
        {
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            if (keyState.IsKeyDown(Key.M))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Point);
            }
            if (keyState.IsKeyDown(Key.Comma))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            }
            if (keyState.IsKeyDown(Key.Period))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }


            if (keyState.IsKeyDown(Key.J))
            {
                _fov = 40f;
                CreateProjection();
            }
            if (keyState.IsKeyDown(Key.K))
            {
                _fov = 50f;
                CreateProjection();
            }
            if (keyState.IsKeyDown(Key.L))
            {
                _fov = 60f;
                CreateProjection();
            }

            if (keyState.IsKeyDown(Key.W))
            {
                _z += 0.2f*(float)dt;
            }
            if (keyState.IsKeyDown(Key.S))
            {
                _z -= 0.2f * (float)dt;
            }
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            _time += e.Time;
            Title = $"{_title}: (Vsync: {VSync}) FPS: {1f / e.Time:0}, z:{_z}";
            GL.ClearColor(_backColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(_program);
            GL.UniformMatrix4(20, false, ref _projectionMatrix);
            float c = 0f;
            foreach (var renderObject in _renderObjects)
            {
                renderObject.Bind();
                for (int i = 0; i < 5; i++)
                {
                    var k = i + (float)(_time * (0.05f + (0.1 * c)));
                    var t2 = Matrix4.CreateTranslation(
                        (float)(Math.Sin(k * 5f) * (c + 0.5f)),
                        (float)(Math.Cos(k * 5f) * (c + 0.5f)),
                        _z);
                    var r1 = Matrix4.CreateRotationX(k * 13.0f + i);
                    var r2 = Matrix4.CreateRotationY(k * 13.0f + i);
                    var r3 = Matrix4.CreateRotationZ(k * 3.0f + i);
                    var modelView = r1 * r2 * r3 * t2;
                    GL.UniformMatrix4(21, false, ref modelView);
                    renderObject.Render();
                }
                c += 0.3f;
            }
            GL.PointSize(10);
            SwapBuffers();
        }
        
    }
}
