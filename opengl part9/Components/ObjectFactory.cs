using OpenTK;
using OpenTK.Graphics;
using techdump.opengl.Components.Renderables;

namespace techdump.opengl.Components
{
    public class ObjectFactory
    {
        public static ColoredVertex[] CreateSolidCube(float side, Color4 color)
        {
            side = side/2f; // half side - and other half
            ColoredVertex[] vertices =
            {
                new ColoredVertex(new Vector4(-side, -side, -side, 1.0f), color),
                new ColoredVertex(new Vector4(-side, side, -side, 1.0f), color),
                new ColoredVertex(new Vector4(-side, -side, side, 1.0f), color),
                new ColoredVertex(new Vector4(-side, -side, side, 1.0f), color),
                new ColoredVertex(new Vector4(-side, side, -side, 1.0f), color),
                new ColoredVertex(new Vector4(-side, side, side, 1.0f), color),

                new ColoredVertex(new Vector4(side, -side, -side, 1.0f), color),
                new ColoredVertex(new Vector4(side, side, -side, 1.0f), color),
                new ColoredVertex(new Vector4(side, -side, side, 1.0f), color),
                new ColoredVertex(new Vector4(side, -side, side, 1.0f), color),
                new ColoredVertex(new Vector4(side, side, -side, 1.0f), color),
                new ColoredVertex(new Vector4(side, side, side, 1.0f), color),

                new ColoredVertex(new Vector4(-side, -side, -side, 1.0f), color),
                new ColoredVertex(new Vector4(side, -side, -side, 1.0f), color),
                new ColoredVertex(new Vector4(-side, -side, side, 1.0f), color),
                new ColoredVertex(new Vector4(-side, -side, side, 1.0f), color),
                new ColoredVertex(new Vector4(side, -side, -side, 1.0f), color),
                new ColoredVertex(new Vector4(side, -side, side, 1.0f), color),

                new ColoredVertex(new Vector4(-side, side, -side, 1.0f), color),
                new ColoredVertex(new Vector4(side, side, -side, 1.0f), color),
                new ColoredVertex(new Vector4(-side, side, side, 1.0f), color),
                new ColoredVertex(new Vector4(-side, side, side, 1.0f), color),
                new ColoredVertex(new Vector4(side, side, -side, 1.0f), color),
                new ColoredVertex(new Vector4(side, side, side, 1.0f), color),

                new ColoredVertex(new Vector4(-side, -side, -side, 1.0f), color),
                new ColoredVertex(new Vector4(side, -side, -side, 1.0f), color),
                new ColoredVertex(new Vector4(-side, side, -side, 1.0f), color),
                new ColoredVertex(new Vector4(-side, side, -side, 1.0f), color),
                new ColoredVertex(new Vector4(side, -side, -side, 1.0f), color),
                new ColoredVertex(new Vector4(side, side, -side, 1.0f), color),

                new ColoredVertex(new Vector4(-side, -side, side, 1.0f), color),
                new ColoredVertex(new Vector4(side, -side, side, 1.0f), color),
                new ColoredVertex(new Vector4(-side, side, side, 1.0f), color),
                new ColoredVertex(new Vector4(-side, side, side, 1.0f), color),
                new ColoredVertex(new Vector4(side, -side, side, 1.0f), color),
                new ColoredVertex(new Vector4(side, side, side, 1.0f), color),
            };
            return vertices;
        }

        public static TexturedVertex[] CreateTexturedCube(float side)
        {
            side = side / 2f; // half side - and other half

            TexturedVertex[] vertices =
                {
                new TexturedVertex(new Vector4(-side, -side, -side, 1.0f),  new Vector2(0, 0)),
                new TexturedVertex(new Vector4(-side, side, -side, 1.0f),   new Vector2(1, 0)),
                new TexturedVertex(new Vector4(-side, -side, side, 1.0f),   new Vector2(0, 1)),
                new TexturedVertex(new Vector4(-side, -side, side, 1.0f),   new Vector2(0, 1)),
                new TexturedVertex(new Vector4(-side, side, -side, 1.0f),   new Vector2(1, 0)),
                new TexturedVertex(new Vector4(-side, side, side, 1.0f),    new Vector2(1, 1)),

                new TexturedVertex(new Vector4(side, -side, -side, 1.0f),   new Vector2(0, 0)),
                new TexturedVertex(new Vector4(side, side, -side, 1.0f),    new Vector2(1, 0)),
                new TexturedVertex(new Vector4(side, -side, side, 1.0f),    new Vector2(0, 1)),
                new TexturedVertex(new Vector4(side, -side, side, 1.0f),    new Vector2(0, 1)),
                new TexturedVertex(new Vector4(side, side, -side, 1.0f),    new Vector2(1, 0)),
                new TexturedVertex(new Vector4(side, side, side, 1.0f),     new Vector2(1, 1)),

                new TexturedVertex(new Vector4(-side, -side, -side, 1.0f),  new Vector2(0, 0)),
                new TexturedVertex(new Vector4(side, -side, -side, 1.0f),   new Vector2(1, 0)),
                new TexturedVertex(new Vector4(-side, -side, side, 1.0f),   new Vector2(0, 1)),
                new TexturedVertex(new Vector4(-side, -side, side, 1.0f),   new Vector2(0, 1)),
                new TexturedVertex(new Vector4(side, -side, -side, 1.0f),   new Vector2(1, 0)),
                new TexturedVertex(new Vector4(side, -side, side, 1.0f),    new Vector2(1, 1)),

                new TexturedVertex(new Vector4(-side, side, -side, 1.0f),   new Vector2(0, 0)),
                new TexturedVertex(new Vector4(side, side, -side, 1.0f),    new Vector2(1, 0)),
                new TexturedVertex(new Vector4(-side, side, side, 1.0f),    new Vector2(0, 1)),
                new TexturedVertex(new Vector4(-side, side, side, 1.0f),    new Vector2(0, 1)),
                new TexturedVertex(new Vector4(side, side, -side, 1.0f),    new Vector2(1, 0)),
                new TexturedVertex(new Vector4(side, side, side, 1.0f),     new Vector2(1, 1)),

                new TexturedVertex(new Vector4(-side, -side, -side, 1.0f),  new Vector2(0, 0)),
                new TexturedVertex(new Vector4(side, -side, -side, 1.0f),   new Vector2(1, 0)),
                new TexturedVertex(new Vector4(-side, side, -side, 1.0f),   new Vector2(0, 1)),
                new TexturedVertex(new Vector4(-side, side, -side, 1.0f),   new Vector2(1, 0)),
                new TexturedVertex(new Vector4(side, -side, -side, 1.0f),   new Vector2(0, 1)),
                new TexturedVertex(new Vector4(side, side, -side, 1.0f),    new Vector2(0, 0)),

                new TexturedVertex(new Vector4(-side, -side, side, 1.0f),   new Vector2(0, 0)),
                new TexturedVertex(new Vector4(side, -side, side, 1.0f),    new Vector2(1, 0)),
                new TexturedVertex(new Vector4(-side, side, side, 1.0f),    new Vector2(0, 1)),
                new TexturedVertex(new Vector4(-side, side, side, 1.0f),    new Vector2(0, 1)),
                new TexturedVertex(new Vector4(side, -side, side, 1.0f),    new Vector2(1, 0)),
                new TexturedVertex(new Vector4(side, side, side, 1.0f),     new Vector2(1, 1)),
            };
            return vertices;
        }
    }
}