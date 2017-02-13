using OpenTK;
using OpenTK.Graphics;

namespace techdump.opengl.Components.Renderables
{
    public struct TexturedVertex
    {
        public const int Size = (4 + 2) * 4; // size of struct in bytes

        private readonly Vector4 _position;
        private readonly Vector2 _textureCoordinate;

        public TexturedVertex(Vector4 position, Vector2 textureCoordinate)
        {
            _position = position;
            _textureCoordinate = textureCoordinate;
        }
    }
}