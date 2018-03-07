using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using techdump.opengl.Components.Cameras;
using techdump.opengl.Components.GameObjects;
using techdump.opengl.Components.GameObjects.Text;
using techdump.opengl.Components.Renderables;

namespace techdump.opengl.Components
{
    public sealed class MainWindow : GameWindow
    {
        public static bool IsFullscreen { get; set; }
        private readonly string _title;
        private GameObjectFactory _gameObjectFactory;
        private readonly List<AGameObject> _gameObjects = new List<AGameObject>();
        private double _time;
        private readonly Color4 _backColor = new Color4(0.1f, 0.1f, 0.3f, 1.0f);
        private Matrix4 _projectionMatrix;
        private float _fov = 45f;
        private ShaderProgram _textProgram;
        private ShaderProgram _texturedProgram;
        private ShaderProgram _solidProgram;
        private KeyboardState _lastKeyboardState;
        private Spacecraft _player;

        private RenderText _text;
        private int _score;
        public int _clicks;
        private bool _gameOver;
        private Bullet.BulletType _bulletType;
        private Bullet _lastBullet;
        private bool _useFirstPerson = true;
        private ICamera _camera;
        private MouseState _lastMouseState;
        private Dictionary<string, ARenderable> _models;

        public MainWindow()
            : base(750, // initial width
                500, // initial height
                GraphicsMode.Default,
                "",  // initial title
                GameWindowFlags.Fullscreen,
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

        public void ToggleFullscreen()
        {
            if (IsFullscreen)
            {
                WindowBorder = WindowBorder.Resizable;
                WindowState = WindowState.Normal;
                ClientSize = new Size(750, 600);
            }
            else
            {
                WindowBorder = WindowBorder.Hidden;
                WindowState = WindowState.Fullscreen;
            }
            IsFullscreen = !IsFullscreen;
        }

        protected override void OnLoad(EventArgs e)
        {
            Debug.WriteLine("OnLoad");
            VSync = VSyncMode.Off;
            CreateProjection();

            _textProgram = new ShaderProgram();
            _textProgram.AddShader(ShaderType.VertexShader, @"Components\Shaders\1Vert\textVert.c");
            _textProgram.AddShader(ShaderType.FragmentShader, @"Components\Shaders\5Frag\textFrag.c");
            _textProgram.Link();

            _solidProgram = new ShaderProgram();
            _solidProgram.AddShader(ShaderType.VertexShader, @"Components\Shaders\1Vert\simplePipeVert.c");
            _solidProgram.AddShader(ShaderType.FragmentShader, @"Components\Shaders\5Frag\simplePipeFrag.c");
            _solidProgram.Link();

            _solidProgram = new ShaderProgram();
            _solidProgram.AddShader(ShaderType.VertexShader, @"Components\Shaders\1Vert\gouraudPipeSolidVert.c");
            _solidProgram.AddShader(ShaderType.FragmentShader, @"Components\Shaders\5Frag\gouraudPipeFrag.c");
            _solidProgram.Link();

            //_solidProgram = new ShaderProgram();
            //_solidProgram.AddShader(ShaderType.VertexShader, @"Components\Shaders\1Vert\phongPipeSolidVert.c");
            //_solidProgram.AddShader(ShaderType.FragmentShader, @"Components\Shaders\5Frag\phongPipeFrag.c");
            //_solidProgram.Link();

            _texturedProgram = new ShaderProgram();
            _texturedProgram.AddShader(ShaderType.VertexShader, @"Components\Shaders\1Vert\simplePipeTexVert.c");
            _texturedProgram.AddShader(ShaderType.FragmentShader, @"Components\Shaders\5Frag\simplePipeTexFrag.c");
            _texturedProgram.Link();

            _models = new Dictionary<string, ARenderable>();
            var textModel = new TexturedRenderObject(RenderObjectFactory.CreateTexturedCharacter(), _textProgram.Id, @"Components\Textures\font singleline.bmp");
            //textModel.SetFiltering(All.Nearest);
            _models.Add("Quad", textModel);
            _models.Add("Wooden", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, @"Components\Textures\wooden.png", 8));
            _models.Add("Golden", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, @"Components\Textures\golden.bmp", 8));
            _models.Add("Asteroid", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, @"Components\Textures\moonmap1k.jpg", 8));
            _models.Add("Spacecraft", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube6(1, 1, 1), _texturedProgram.Id, @"Components\Textures\spacecraft.png", 8));
            _models.Add("Gameover", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube6(1, 1, 1), _texturedProgram.Id, @"Components\Textures\gameover.png", 8));
            _models.Add("Bullet", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, @"Components\Textures\dotted.png", 8));

            //models.Add("TestObject", new TexturedRenderObject(RenderObjectFactory.CreateTexturedCube(1, 1, 1), _texturedProgram.Id, @"Components\Textures\asteroid texture one side.jpg"));
            //models.Add("TestObjectGen", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube(1, 1, 1), _texturedProgram.Id, @"Components\Textures\asteroid texture one side.jpg", 8));
            //models.Add("TestObjectPreGen", new MipMapManualRenderObject(RenderObjectFactory.CreateTexturedCube(1, 1, 1), _texturedProgram.Id, @"Components\Textures\asteroid texture one side mipmap levels 0 to 8.bmp", 9));

            _gameObjectFactory = new GameObjectFactory(_models);

            _player = _gameObjectFactory.CreateSpacecraft();
            //_gameObjects.Add(_player);
            //_gameObjects.Add(_gameObjectFactory.CreateAsteroid());
            //_gameObjects.Add(_gameObjectFactory.CreateGoldenAsteroid());
            //_gameObjects.Add(_gameObjectFactory.CreateWoodenAsteroid());

            var maxX = 2.5f;
            var maxY = 1.5f;
            for (float x = -maxX; x < maxX; x += 0.5f)
            {
                for (float y = -maxY; y < maxY; y += 0.5f)
                {
                    _gameObjects.Add(_gameObjectFactory.CreateSelectableSphere(new Vector4(x, y, -5f, 0)));
                }
            }
            for (float x = -maxX; x < maxX; x += 0.65f)
            {
                for (float y = -maxY; y < maxY; y += 0.65f)
                {
                    _gameObjects.Add(_gameObjectFactory.CreateSelectableSphereSecondary(new Vector4(x, y, -6f, 0)));
                }
            }


            _text = new RenderText(_models["Quad"], new Vector4(-0.2f, 0.1f, -0.4f, 1),  Color4.Red, "Score");
           
            _camera = new StaticCamera();

            CursorVisible = true;

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
            GL.PointSize(3);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            MouseUp += OnMouseUp;
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
            //_gameObjectFactory.Dispose();
            _textProgram.Dispose();
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
                0.001f,                       // near plane
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
                _gameObjects.Add(_gameObjectFactory.CreateRandomAsteroid());
            }
            for (int i = 0; i < outOfBoundsAsteroids; i++)
            {
                _gameObjects.Add(_gameObjectFactory.CreateRandomAsteroid());
            }
            if (_lastBullet == null || _lastBullet.ToBeRemoved)
            {
                _camera = new StaticCamera();
            }

            _camera.Update(_time, e.Time);
            HandleKeyboard(e.Time);
        }
        
        private void OnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (mouseButtonEventArgs.Button != MouseButton.Left)
                return;
            PickObjectOnScreen(mouseButtonEventArgs.X, mouseButtonEventArgs.Y);
        }


        private void PickObjectOnScreen(int mouseX, int mouseY)
        {
            // heavily influenced by: http://antongerdelan.net/opengl/raycasting.html
            // viewport coordinate system
            // normalized device coordinates
            var x = (2f * mouseX) / Width - 1f;
            var y = 1f - (2f * mouseY) / Height;
            var z = 1f;
            var rayNormalizedDeviceCoordinates = new Vector3(x, y, z);

            // 4D homogeneous clip coordinates
            var rayClip = new Vector4(rayNormalizedDeviceCoordinates.X, rayNormalizedDeviceCoordinates.Y, -1f, 1f);

            // 4D eye (camera) coordinates
            var rayEye = _projectionMatrix.Inverted() * rayClip;
            rayEye = new Vector4(rayEye.X, rayEye.Y, -1f, 0f);

            // 4D world coordinates
            var rayWorldCoordinates = (_camera.LookAtMatrix.Inverted() * rayEye).Xyz;
            rayWorldCoordinates.Normalize();
            FindClosestAsteroidHitByRay(rayWorldCoordinates);
        }

        private void FindClosestAsteroidHitByRay(Vector3 rayWorldCoordinates)
        {
            AGameObject bestCandidate = null;
            double? bestDistance = null;
            foreach (var gameObject in _gameObjects)
            {
                if (!(gameObject is SelectableSphere) && !(gameObject is Asteroid))
                    continue;
                var candidateDistance = gameObject.IntersectsRay(rayWorldCoordinates, _camera.Position);
                if (!candidateDistance.HasValue)
                    continue;
                if (!bestDistance.HasValue)
                {
                    bestDistance = candidateDistance;
                    bestCandidate = gameObject;
                    continue;
                }
                if (candidateDistance < bestDistance)
                {
                    bestDistance = candidateDistance;
                    bestCandidate = gameObject;
                }
            }
            if (bestCandidate != null)
            {
                _clicks++;
                switch (bestCandidate)
                {
                    case Asteroid asteroid:
                        _clicks += asteroid.Score;
                        break;
                    case SelectableSphere sphere:
                        sphere.ToggleModel();
                        break;
                }
            }
        }

        private void HandleKeyboard(double dt)
        {
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            if (keyState.IsKeyDown(Key.PageDown) && _lastKeyboardState.IsKeyUp(Key.PageDown))
            {
                ToggleFullscreen();
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
            if (keyState.IsKeyDown(Key.T))
            {
                _useFirstPerson = false;
            }
            if (keyState.IsKeyDown(Key.F))
            {
                _useFirstPerson = true;
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
                    if(_useFirstPerson)
                        _camera = new FirstPersonCamera(bullet);
                    else
                        _camera = new ThirdPersonCamera(bullet, new Vector3(-0.3f, -0.3f, 0.1f));
                }
                _lastBullet = bullet;
                _gameObjects.Add(bullet);
            }
            _lastKeyboardState = keyState;
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Title = $"{_title}: FPS:{1f / e.Time:0000.0}, obj:{_gameObjects.Count}, score:{_score}";
            if(_clicks < 10)
                _text.SetText($"Object picking by mouse");
            else if (_clicks < 50)
                _text.SetText($"This seems to work quite well");
            else if (_clicks < 75)
                _text.SetText($"Not a single false positive!");
            else if (_clicks < 100)
                _text.SetText($"dreamstatecoding.blogspot.com");
            GL.ClearColor(_backColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            int lastProgram = -1;
            foreach (var obj in _gameObjects)
            {
                lastProgram = RenderOneElement(obj, lastProgram);
            }
            // render after all opaque objects to get transparency right
            RenderOneElement(_text, lastProgram);
            SwapBuffers();
        }

        private int RenderOneElement(AGameObject obj, int lastProgram)
        {
            var program = obj.Model.Program;
            if (lastProgram != program)
                GL.UniformMatrix4(20, false, ref _projectionMatrix);
            lastProgram = obj.Model.Program;
            obj.Render(_camera);
            return lastProgram;
        }
    }
}
