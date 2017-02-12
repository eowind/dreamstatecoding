using System;
using System.Collections.Generic;
using OpenTK;
using techdump.opengl.Components.Renderables;

namespace techdump.opengl.Components.GameObjects
{
    public class Bullet : AGameObject
    {
        public Bullet(ARenderable model, Vector4 position, Vector4 direction, Vector4 rotation, float velocity) 
            : base(model, position, direction, rotation, velocity)
        {
        }

        public override void Update(double time, double delta)
        {
            _rotation.X = (float)Math.Sin(time * 15 + _gameObjectNumber);
            _rotation.Y = (float)Math.Cos(time * 15 + _gameObjectNumber);
            _rotation.Z = (float)Math.Cos(time * 15 + _gameObjectNumber);
            base.Update(time, delta);
        }

        public AGameObject CheckCollision(List<AGameObject> gameObjects)
        {
            foreach (var x in gameObjects)
            {
                if(x.GetType() != typeof(Asteroid))
                    continue;
                // naive first object in radius
                if ((Position - x.Position).Length < x.Scale.X)
                    return x;
            }
            return null;
        }
    }
}