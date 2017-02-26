using OpenTK;
using OpenTK.Graphics;

namespace techdump.opengl.Components.Renderables
{
    public struct ColoredVertex
    {
        public const int Size = (4 + 4) * 4; // size of struct in bytes

        private readonly Vector4 _position;
        private readonly Color4 _color;

        public ColoredVertex(Vector4 position, Color4 color)
        {
            _position = position;
            _color = color;
        }
    }

}