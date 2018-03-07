using System;
using OpenTK;
using techdump.opengl.Components.Renderables;

namespace techdump.opengl.Components.GameObjects
{
    public class Asteroid : AGameObject
    {
        public int Score { get; set; }
        private Bullet _lockedBullet;
        private ARenderable _original;
        public Asteroid(ARenderable model, Vector4 position, Vector4 direction, Vector4 rotation, float velocity) 
            : base(model, position, direction, rotation, velocity)
        {
            _original = model;
        }

        public double? IntersectsRay(Vector3 rayDirection, Vector3 rayOrigin)
        {
            var radius = _scale.X;
            var difference = Position.Xyz - rayOrigin;
            var differenceLengthSquared = difference.LengthSquared;
            var sphereRadiusSquared = radius * radius;
            if (differenceLengthSquared < sphereRadiusSquared)
            {
                return 0d; 
            }
            var distanceAlongRay = Vector3.Dot(rayDirection, difference);
            if (distanceAlongRay < 0)
            {
                return null;
            }
            var dist = sphereRadiusSquared + distanceAlongRay * distanceAlongRay - differenceLengthSquared;
            var result = (dist < 0) ? null : distanceAlongRay - (double?)Math.Sqrt(dist);
            return result;
        }

        public override void Update(double time, double delta)
        {
            _rotation.X = (float)Math.Sin((time + GameObjectNumber) * 0.3);
            _rotation.Y = (float)Math.Cos((time + GameObjectNumber) * 0.5);
            _rotation.Z = (float)Math.Cos((time + GameObjectNumber) * 0.2);
            var d = new Vector4(_rotation.X, _rotation.Y, 0, 0);
            d.Normalize();
            _direction = d;
            if (_lockedBullet != null && _lockedBullet.ToBeRemoved)
            {
                _lockedBullet = null;
                _model = _original;
            }
            base.Update(time, delta);
        }
        
        public void LockBullet(Bullet bullet)
        {
            _lockedBullet = bullet;
            _model = bullet.Model;
        }

    }
}