using System;
using System.Collections.Generic;
using OpenTK;
using techdump.opengl.Components.Renderables;

namespace techdump.opengl.Components.GameObjects
{
    public class Bullet : AGameObject
    {
        private static int _bulletCounter;
        private readonly int _bulletNumber;
        private BulletType _bulletType;
        private AGameObject _target;
        private double _life;
        public Bullet(ARenderable model, Vector4 position, Vector4 direction, Vector4 rotation, float velocity, BulletType bulletType = BulletType.Straight) 
            : base(model, position, direction, rotation, velocity)
        {
            _bulletNumber = _bulletCounter++;
            _bulletType = bulletType;
        }

        public override void Update(double time, double delta)
        {
            _life += delta;
            if (_life > 5)
                ToBeRemoved = true;
            switch (_bulletType)
            {
                case BulletType.Straight:
                    break;
                case BulletType.Wave:
                    if (_bulletNumber % 2 == 0)
                        _direction.X = (float)Math.Sin(_position.Y * 33f);
                    else
                        _direction.X = (float)Math.Cos(_position.Y * 33f);
                    break;
                case BulletType.Seeker:
                    if (_target != null && !_target.ToBeRemoved)
                    {
                        _direction = _direction + ((float)delta * (_target.Position - Position));
                    }
                    break;
                case BulletType.SeekerExtra:
                    if (_target != null && !_target.ToBeRemoved)
                    {
                        _direction =  _target.Position - Position;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _direction.Normalize();
            _rotation = _direction * _velocity;
            base.Update(time, delta);
        }

        public void SetTarget(Asteroid target)
        {
            _target = target;
            target.LockBullet(this);
        }
        public AGameObject CheckCollision(List<AGameObject> gameObjects)
        {
            foreach (var x in gameObjects)
            {
                if (x.GetType() != typeof (Asteroid))
                    continue;
                // naive first object in radius
                if ((Position - x.Position).Length < x.Scale.X)
                    return x;
            }
            return null;
        }

        public enum BulletType
        {
            Straight,
            Wave,
            Seeker,
            SeekerExtra
        }
    }
}