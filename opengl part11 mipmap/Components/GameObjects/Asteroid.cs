using System;
using OpenTK;
using techdump.opengl.Components.Renderables;

namespace techdump.opengl.Components.GameObjects
{
    public class Asteroid : AGameObject
    {
        public int Score { get; set; }
        public Asteroid(ARenderable model, Vector4 position, Vector4 direction, Vector4 rotation, float velocity) 
            : base(model, position, direction, rotation, velocity)
        {
        }


        public override void Update(double time, double delta)
        {
            _rotation.X = (float)Math.Sin((time + _gameObjectNumber) * 0.3);
            _rotation.Y = (float)Math.Cos((time + _gameObjectNumber) * 0.5);
            _rotation.Z = (float)Math.Cos((time + _gameObjectNumber) * 0.2);
            var d = new Vector4(_rotation.X, _rotation.Y, 0, 0);
            d.Normalize();
            _direction = d;
            base.Update(time, delta);
        }
    }
}