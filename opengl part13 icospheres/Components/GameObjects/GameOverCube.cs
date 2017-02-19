using System;
using OpenTK;
using techdump.opengl.Components.Renderables;

namespace techdump.opengl.Components.GameObjects
{
    public class GameOverCube : AGameObject
    {
        public GameOverCube(ARenderable model, Vector4 position, Vector4 direction, Vector4 rotation, float velocity) 
            : base(model, position, direction, rotation, velocity)
        {
        }

        public override void Update(double time, double delta)
        {
            var k = (float)time * 0.4f;
            _rotation.X = k * 0.7f;
            _rotation.Y = k * 0.5f;
            _rotation.Z = (float)Math.Sin(k * 0.4f);
            base.Update(time, delta);
        }
        
    }
}