using OpenTK;

namespace techdump.opengl.Components.Cameras
{
    public interface ICamera
    {
        Matrix4 LookAtMatrix { get; }
        void Update(double time, double delta);
    }
}