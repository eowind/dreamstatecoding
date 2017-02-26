using OpenTK;
using OpenTK.Graphics.OpenGL4;
using techdump.opengl.Components.Cameras;
using techdump.opengl.Components.Renderables;

namespace techdump.opengl.Components.GameObjects.Text
{
    public class RenderCharacter : AGameObject
    {
        private float _offset;

        public RenderCharacter(ARenderable model, Vector4 position, float charOffset)
            : base(model, position, Vector4.Zero, Vector4.Zero, 0)
        {
            _offset = charOffset;
            _scale = new Vector3(0.2f);
        }

        public void SetChar(float charOffset)
        {
            _offset = charOffset;
        }

        public override void Render(ICamera camera)
        {
            GL.VertexAttrib2(2, new Vector2(_offset, 0));
            var t2 = Matrix4.CreateTranslation(
                _position.X,
                _position.Y,
                _position.Z);
            var s = Matrix4.CreateScale(_scale);
            _modelView = s * t2 * camera.LookAtMatrix;
            GL.UniformMatrix4(21, false, ref _modelView);
            _model.Render();
        }
    }
}