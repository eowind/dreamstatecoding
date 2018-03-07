using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using OpenTK;
using techdump.opengl.Components.GameObjects;
using techdump.opengl.Components.Renderables;

namespace techdump.opengl.Components
{
    public class GameObjectFactory : IDisposable
    {
        private const float Z = -2.7f;
        private readonly Random _random = new Random();
        private readonly Dictionary<string, ARenderable> _models;
        public GameObjectFactory(Dictionary<string, ARenderable> models)
        {
            _models = models;
        }

        public Spacecraft CreateSpacecraft()
        {
            var spacecraft = new Spacecraft(_models["Spacecraft"], new Vector4(0, -1f, Z, 0), Vector4.Zero, Vector4.Zero, 0);
            spacecraft.SetScale(new Vector3(0.2f, 0.2f, 0.001f));
            return spacecraft;
        }

        public SelectableSphere CreateSelectableSphere(Vector4 position)
        {
            var sphere = new SelectableSphere(_models["Wooden"], _models["Bullet"], position, Vector4.Zero, Vector4.Zero);
            sphere.SetScale(new Vector3(0.2f));
            return sphere;
        }
        public SelectableSphere CreateSelectableSphereSecondary(Vector4 position)
        {
            var sphere = new SelectableSphere(_models["Asteroid"], _models["Bullet"], position, Vector4.Zero, Vector4.Zero);
            sphere.SetScale(new Vector3(0.2f));
            return sphere;
        }

        public Asteroid CreateAsteroid(string model, Vector4 position)
        {
            var obj = new Asteroid(_models[model], position, Vector4.Zero, Vector4.Zero, 0.2f);
            obj.SetScale(new Vector3(0.2f));
            switch (model)
            {
                case "Asteroid":
                    obj.Score = 1;
                    break;
                case "Wooden":
                    obj.Score = 10;
                    break;
                case "Golden":
                    obj.Score = 50;
                    break;
            }
            return obj;
        }
        public AGameObject CreateRandomAsteroid()
        {
            var rnd = _random.NextDouble();
            var position = GetRandomPosition();
            if (rnd < 0.01)
                return CreateAsteroid("Golden", position);
            if (rnd < 0.2)
                return CreateAsteroid("Wooden", position);
            return CreateAsteroid("Asteroid", position);
        }
        public Asteroid CreateAsteroid()
        {
            return CreateAsteroid("Asteroid", GetRandomPosition());
        }
        public Asteroid CreateGoldenAsteroid()
        {
            var obj =  CreateAsteroid("Golden", GetRandomPosition());
            obj.SetScale(new Vector3(0.22f));
            return obj;
        }
        public Asteroid CreateWoodenAsteroid()
        {
            return CreateAsteroid("Wooden", GetRandomPosition());
        }
        public Bullet CreateBullet(Vector4 position, Bullet.BulletType bulletType)
        {
            var bullet = new Bullet(_models["Bullet"], position + new Vector4(0, 0.1f, 0, 0), Vector4.UnitY, Vector4.Zero, 0.8f, bulletType);
            bullet.SetScale(new Vector3(0.05f));
            return bullet;
        }
        public AGameObject CreateGameOver(string s = "Gameover")
        {
            var obj = new GameOverCube(_models[s], new Vector4(0, 0, Z, 0), Vector4.Zero, Vector4.Zero, 0.0f);
            obj.SetScale(new Vector3(0.8f));
            return obj;
        }

        public AGameObject CreateTestObject(string s)
        {
            var obj = new TestObject(_models[s], new Vector4(0, 0, Z, 0), -Vector4.UnitZ, Vector4.Zero, 0.3f);
            obj.SetScale(new Vector3(0.8f));
            return obj;
        }

        private Vector4 GetRandomPosition()
        {
            var position = new Vector4(
                ((float) _random.NextDouble() - 0.5f),
                ((float) _random.NextDouble() - 0.5f),
                Z,
                0);
            return position;
        }
        public void Dispose()
        {
            foreach (var obj in _models)
                obj.Value.Dispose();
        }

    }
}