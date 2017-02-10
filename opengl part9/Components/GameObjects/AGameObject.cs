using OpenTK;
using techdump.opengl.Components.Renderables;

namespace techdump.opengl.Components.GameObjects
{
    public abstract class AGameObject
    {
        private readonly ARenderable _model;
        private readonly float _position;
        private readonly Vector4 _direction;
        private readonly float _velocity;

        public AGameObject(ARenderable model, float position, Vector4 direction, float velocity)
        {
            _model = model;
            _position = position;
            _direction = direction;
            _velocity = velocity;
        }


    }
}