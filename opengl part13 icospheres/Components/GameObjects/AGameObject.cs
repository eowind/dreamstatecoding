using OpenTK;
using OpenTK.Graphics.OpenGL4;
using techdump.opengl.Components.Cameras;
using techdump.opengl.Components.Renderables;

namespace techdump.opengl.Components.GameObjects
{
    public abstract class AGameObject
    {
        public ARenderable Model => _model;
        public Vector4 Position => _position;
        public Vector4 Direction => _direction;
        public Vector3 Scale => _scale;
        private static int GameObjectCounter;
        public readonly int GameObjectNumber;
        protected ARenderable _model;
        protected Vector4 _position;
        protected Vector4 _direction;
        protected Vector4 _rotation;
        protected float _velocity;
        protected Matrix4 _modelView;
        protected Vector3 _scale;
        public bool ToBeRemoved { get; set; }

        public AGameObject(ARenderable model, Vector4 position, Vector4 direction, Vector4 rotation, float velocity)
        {
            _model = model;
            _position = position;
            _direction = direction;
            _rotation = rotation;
            _velocity = velocity;
            _scale = new Vector3(1);
            GameObjectNumber = GameObjectCounter++;
        }

        public void SetScale(Vector3 scale)
        {
            _scale = scale;
        }
        public void SetPosition(Vector4 position)
        {
            _position = position;
        }
        public virtual void Update(double time, double delta)
        {
            _position += _direction*(_velocity*(float) delta);
        }


        public virtual void Render(ICamera camera)
        {
            _model.Bind();
            var t2 = Matrix4.CreateTranslation(_position.X, _position.Y, _position.Z);
            var r1 = Matrix4.CreateRotationX(_rotation.X);
            var r2 = Matrix4.CreateRotationY(_rotation.Y);
            var r3 = Matrix4.CreateRotationZ(_rotation.Z);
            var s = Matrix4.CreateScale(_scale);
            _modelView = r1*r2*r3*s*t2*camera.LookAtMatrix;
            GL.UniformMatrix4(21, false, ref _modelView);
            _model.Render();
        }
    }
}