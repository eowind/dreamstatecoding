using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using techdump.opengl.Components.GameObjects;
using techdump.opengl.Components.Renderables;

namespace techdump.opengl.Components
{
    public sealed class MainWindow : GameWindow
    {
        private readonly string _title;
        private GameObjectFactory _gameObjectFactory;
        private readonly List<AGameObject> _gameObjects = new List<AGameObject>();
        private double _time;
        private readonly Color4 _backColor = new Color4(0.1f, 0.1f, 0.3f, 1.0f);
        private Matrix4 _projectionMatrix;
        private float _fov = 45f;
        private ShaderProgram _texturedProgram;
        private ShaderProgram _solidProgram;
        private KeyboardState _lastKeyboardState;
        private Spacecraft _player;
        private int _score;
        private bool _gameOver;
        private Bullet.BulletType _bulletType;

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
            
            var models = new Dictionary<string, ARenderable>();
            models.Add("Wooden", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube(1, 1, 1), _texturedProgram.Id, @"Components\Textures\wooden.png", 8));
            models.Add("Golden", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube(1, 1, 1), _texturedProgram.Id, @"Components\Textures\golden.bmp", 8));
            models.Add("Asteroid", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube6(1, 1, 1), _texturedProgram.Id, @"Components\Textures\asteroid.bmp", 8));
            models.Add("Spacecraft", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube6(1, 1, 1), _texturedProgram.Id, @"Components\Textures\spacecraft.png", 8));
            models.Add("Gameover", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube6(1, 1, 1), _texturedProgram.Id, @"Components\Textures\gameover.png", 8));
            models.Add("Bullet", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube(1, 1, 1), _texturedProgram.Id, @"Components\Textures\pinkframe.bmp", 8));

            //models.Add("TestObject", new TexturedRenderObject(RenderObjectFactory.CreateTexturedCube(1, 1, 1), _texturedProgram.Id, @"Components\Textures\asteroid texture one side.jpg"));
            //models.Add("TestObjectGen", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube(1, 1, 1), _texturedProgram.Id, @"Components\Textures\asteroid texture one side.jpg", 8));
            //models.Add("TestObjectPreGen", new MipMapManualRenderObject(RenderObjectFactory.CreateTexturedCube(1, 1, 1), _texturedProgram.Id, @"Components\Textures\asteroid texture one side mipmap levels 0 to 8.bmp", 9));

            _gameObjectFactory = new GameObjectFactory(models);

            _player = _gameObjectFactory.CreateSpacecraft();
            _gameObjects.Add(_player);
            _gameObjects.Add(_gameObjectFactory.CreateAsteroid());
            _gameObjects.Add(_gameObjectFactory.CreateGoldenAsteroid());
            _gameObjects.Add(_gameObjectFactory.CreateWoodenAsteroid());


            CursorVisible = true;

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
            GL.PointSize(3);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
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
            _gameObjectFactory.Dispose();
            _solidProgram.Dispose();
            _texturedProgram.Dispose();
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
            _time += e.Time;
            var remove = new HashSet<AGameObject>();
            var view = new Vector4(0, 0, -2.4f, 0);
            int removedAsteroids = 0;
            int outOfBoundsAsteroids = 0;
            foreach (var item in _gameObjects)
            {
                item.Update(_time, e.Time);
                if (item.ToBeRemoved)
                    remove.Add(item);
                //if ((view - item.Position).Length > 2)
                //{
                //    remove.Add(item);
                //    if (item.GetType() == typeof (Asteroid))
                //    {
                //        outOfBoundsAsteroids++;
                //    }
                //}
                if (item.GetType() == typeof (Bullet))
                {
                    var collide = ((Bullet) item).CheckCollision(_gameObjects);
                    if (collide != null)
                    {
                        remove.Add(item);
                        if (remove.Add(collide))
                        {
                            _score += ((Asteroid)collide).Score;
                            removedAsteroids++;
                        }
                    }
                }
                if (item.GetType() == typeof(Spacecraft))
                {
                    var collide = ((Spacecraft)item).CheckCollision(_gameObjects);
                    if (collide != null)
                    {
                        foreach (var x in _gameObjects)
                            remove.Add(x);
                        _gameObjects.Add(_gameObjectFactory.CreateGameOver());
                        _gameOver = true;
                        removedAsteroids = 0;
                        break;
                    }
                }
            }
            foreach (var r in remove)
            {
                r.ToBeRemoved = true;
                _gameObjects.Remove(r);
            }
            for (int i = 0; i < removedAsteroids; i++)
            {
                //_gameObjects.Add(_gameObjectFactory.CreateRandomAsteroid());
                _gameObjects.Add(_gameObjectFactory.CreateRandomAsteroid());
            }
            for (int i = 0; i < outOfBoundsAsteroids; i++)
            {
                _gameObjects.Add(_gameObjectFactory.CreateRandomAsteroid());
            }
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

            if(keyState.IsKeyDown(Key.Number1))
            {
                _bulletType = Bullet.BulletType.Straight;
            }
            if (keyState.IsKeyDown(Key.Number2))
            {
                _bulletType = Bullet.BulletType.Wave;
            }
            if (keyState.IsKeyDown(Key.Number3))
            {
                _bulletType = Bullet.BulletType.Seeker;
            }
            if (keyState.IsKeyDown(Key.Number4))
            {
                _bulletType = Bullet.BulletType.SeekerExtra;
            }

            if (keyState.IsKeyDown(Key.A))
            {
                _player.MoveLeft();
            }
            if (keyState.IsKeyDown(Key.D))
            {
                _player.MoveRight();
            }
            if (!_gameOver && keyState.IsKeyDown(Key.Space) && _lastKeyboardState.IsKeyUp(Key.Space))
            {
                var bullet = _gameObjectFactory.CreateBullet(_player.Position, _bulletType);
                if (_bulletType == Bullet.BulletType.Seeker || _bulletType == Bullet.BulletType.SeekerExtra)
                {
                    var asteroids = _gameObjects.Where(x => x.GetType() == typeof (Asteroid)).ToList();
                    bullet.SetTarget((Asteroid) asteroids[bullet.GameObjectNumber%asteroids.Count]);
                }
                _gameObjects.Add(bullet);
            }
            _lastKeyboardState = keyState;
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Title = $"{_title}: FPS:{1f / e.Time:0000.0}, obj:{_gameObjects.Count}, score:{_score}";
            GL.ClearColor(Color.Black);// _backColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            int lastProgram = -1;
            foreach (var obj in _gameObjects)
            {
                var program = obj.Model.Program;
                if (lastProgram != program)
                    GL.UniformMatrix4(20, false, ref _projectionMatrix);
                lastProgram = obj.Model.Program;
                obj.Render();

            }
            SwapBuffers();
        }
        
    }
}
