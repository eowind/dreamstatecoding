using OpenTK;

namespace techdump.opengl.Components.Cameras
{
    public class StaticCamera : ICamera
    {
        public Vector3 Position { get;  }
        public Matrix4 LookAtMatrix { get; }
        public StaticCamera()
        {
            Vector3 position;
            position.X = 0;
            position.Y = 0;
            position.Z = 0;
            Position = position;
            LookAtMatrix = Matrix4.LookAt(position, -Vector3.UnitZ, Vector3.UnitY);
        }
        public StaticCamera(Vector3 position, Vector3 target)
        {
            Position = position;
            LookAtMatrix = Matrix4.LookAt(position, target, Vector3.UnitY);
        }
        public void Update(double time, double delta)
        {}
    }
}