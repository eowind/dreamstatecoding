using OpenTK;
using techdump.opengl.Components.GameObjects;

namespace techdump.opengl.Components.Cameras
{
    public class ThirdPersonCamera : ICamera
    {
        public Vector3 Position { get; private set; }
        public Matrix4 LookAtMatrix { get; private set; }
        private readonly AGameObject _target;
        private readonly Vector3 _offset;

        public ThirdPersonCamera(AGameObject target)
            : this(target, Vector3.Zero)
        {}
        public ThirdPersonCamera(AGameObject target, Vector3 offset)
        {
            _target = target;
            _offset = offset;
        }

        public void Update(double time, double delta)
        {
            var position = new Vector3(_target.Position) + (_offset * new Vector3(_target.Direction));
            Position = position;
            LookAtMatrix = Matrix4.LookAt(
                position,  
                new Vector3(_target.Position), 
                Vector3.UnitY);
        }
    }
}