using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using techdump.opengl.Components.Renderables;

namespace techdump.opengl.Components
{
    public sealed class MainWindow : GameWindow
    {
        private readonly string _title;

        private double _time;
        private readonly List<ARenderable> _renderObjects = new List<ARenderable>();
        private readonly Color4 _backColor = new Color4(0.1f, 0.1f, 0.3f, 1.0f);
        private Matrix4 _projectionMatrix;
        private float _z = -2.7f;
        private float _fov = 60f;
        private ShaderProgram _texturedProgram;
        private ShaderProgram _solidProgram;
        private bool _renderAll = true;
        private bool _rotateSingle = false;

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
            Debug.WriteLine("OnLoad");
            VSync = VSyncMode.Off;
            CreateProjection();
            _solidProgram = new ShaderProgram();
            _solidProgram.AddShader(ShaderType.VertexShader, @"Components\Shaders\1Vert\simplePipeVert.c");
            _solidProgram.AddShader(ShaderType.FragmentShader, @"Components\Shaders\5Frag\simplePipeFrag.c");
            _solidProgram.Link();

            _texturedProgram = new ShaderProgram();
            _texturedProgram.AddShader(ShaderType.VertexShader, @"Components\Shaders\1Vert\simplePipeTexVert.c");
            _texturedProgram.AddShader(ShaderType.FragmentShader, @"Components\Shaders\5Frag\simplePipeTexFrag.c");
            _texturedProgram.Link();

            _renderObjects.Add(new TexturedRenderObject(ObjectFactory.CreateTexturedCube(0.2f, 256, 256), _texturedProgram.Id, @"Components\Textures\dotted2.png"));
            _renderObjects.Add(new TexturedRenderObject(ObjectFactory.CreateTexturedCube(0.2f, 256, 256), _texturedProgram.Id, @"Components\Textures\wooden.png"));
            _renderObjects.Add(new ColoredRenderObject(ObjectFactory.CreateSolidCube(0.2f, Color4.HotPink), _solidProgram.Id));
            _renderObjects.Add(new TexturedRenderObject(ObjectFactory.CreateTexturedCube(0.2f, 256, 256), _texturedProgram.Id, @"Components\Textures\dotted.png"));

            CursorVisible = true;

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
            GL.PointSize(3);
            GL.Enable(EnableCap.DepthTest);
            Closed += OnClosed;
            Debug.WriteLine("OnLoad .. done");
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
            
            base.Exit();
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
            if (keyState.IsKeyDown(Key.Number1))
            {
                _renderAll = false;
                _rotateSingle = false;
                _z = -0.35f;
            }
            if (keyState.IsKeyDown(Key.Number2))
            {
                _renderAll = false;
                _rotateSingle = true;
                _z = -0.35f;
            }
            if (keyState.IsKeyDown(Key.Number3))
            {
                _renderAll = true;
                _z = -2.7f;
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

            float c = 0f;
            if (_renderAll)
            {
                foreach (var renderObject in _renderObjects)
                {
                    renderObject.Bind();
                    GL.UniformMatrix4(20, false, ref _projectionMatrix);
                    for (int i = 0; i < 5; i++)
                    {
                        var k = i + (float) (_time*(0.05f + (0.1*c)));
                        var t2 = Matrix4.CreateTranslation(
                            (float) (Math.Sin(k*5f)*(c + 0.5f)),
                            (float) (Math.Cos(k*5f)*(c + 0.5f)),
                            _z + (c + 0.1f));
                        var r1 = Matrix4.CreateRotationX(k*13.0f + i);
                        var r2 = Matrix4.CreateRotationY(k*13.0f + i);
                        var r3 = Matrix4.CreateRotationZ(k*3.0f + i);
                        var modelView = r1*r2*r3*t2;
                        GL.UniformMatrix4(21, false, ref modelView);
                        renderObject.Render();
                    }
                    c += 0.3f;
                }
            }
            else
            {
                var renderObject = _renderObjects.Last();

                renderObject.Bind();
                GL.UniformMatrix4(20, false, ref _projectionMatrix);
                for (int i = 0; i < 1; i++)
                {
                    var k = i + (float)(_time * (0.05f + (0.1 * c)));
                    var t2 = Matrix4.CreateTranslation(
                        0f, //(float)(Math.Sin(k * 5f) * (c + 0.5f)),
                        0f, //(float)(Math.Cos(k * 5f) * (c + 0.5f)),
                        _z);
                    var r1 = Matrix4.CreateRotationX(k * 13.0f + i);
                    var r2 = Matrix4.CreateRotationY(k * 13.0f + i);
                    var r3 = Matrix4.CreateRotationZ(k * 13.0f + i);
                    var modelView = r1 * r2 * r3 * t2;
                    if(_rotateSingle)
                        GL.UniformMatrix4(21, false, ref modelView);
                    else
                        GL.UniformMatrix4(21, false, ref t2);
                    renderObject.Render();
                }
            }

            SwapBuffers();
        }
        
    }
}
