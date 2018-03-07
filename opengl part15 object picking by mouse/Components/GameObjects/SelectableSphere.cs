using System;
using OpenTK;
using techdump.opengl.Components.Renderables;

namespace techdump.opengl.Components.GameObjects
{
    public class SelectableSphere : AGameObject
    {
        private ARenderable _original;
        private ARenderable _secondaryModel;
        public SelectableSphere(ARenderable model, ARenderable secondaryModel, Vector4 position, Vector4 direction, Vector4 rotation)
            : base(model, position, direction, rotation, 0)
        {
            _original = model;
            _secondaryModel = secondaryModel;
        }


        public override void Update(double time, double delta)
        {
            _rotation.Y = (float) ((time + GameObjectNumber) * 0.5);
            var d = new Vector4(_rotation.X, _rotation.Y, 0, 0);
            d.Normalize();
            _direction = d;
            base.Update(time, delta);
        }

        public void ToggleModel()
        {
            if (_model == _original)
                _model = _secondaryModel;
            else
                _model = _original;
        }
    }
}