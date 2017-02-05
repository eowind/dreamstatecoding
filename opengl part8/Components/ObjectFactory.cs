using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;

namespace techdump.opengl.Components
{
    public class ObjectFactory
    {
        public static Vertex[] CreateSolidCube(float side, Color4 color)
        {
            side = side/2f; // halv side - and other half +
            Vertex[] vertices =
            {
                new Vertex(new Vector4(-side, -side, -side, 1.0f), color),
                new Vertex(new Vector4(-side, side, -side, 1.0f), color),
                new Vertex(new Vector4(-side, -side, side, 1.0f), color),
                new Vertex(new Vector4(-side, -side, side, 1.0f), color),
                new Vertex(new Vector4(-side, side, -side, 1.0f), color),
                new Vertex(new Vector4(-side, side, side, 1.0f), color),

                new Vertex(new Vector4(side, -side, -side, 1.0f), color),
                new Vertex(new Vector4(side, side, -side, 1.0f), color),
                new Vertex(new Vector4(side, -side, side, 1.0f), color),
                new Vertex(new Vector4(side, -side, side, 1.0f), color),
                new Vertex(new Vector4(side, side, -side, 1.0f), color),
                new Vertex(new Vector4(side, side, side, 1.0f), color),

                new Vertex(new Vector4(-side, -side, -side, 1.0f), color),
                new Vertex(new Vector4(side, -side, -side, 1.0f), color),
                new Vertex(new Vector4(-side, -side, side, 1.0f), color),
                new Vertex(new Vector4(-side, -side, side, 1.0f), color),
                new Vertex(new Vector4(side, -side, -side, 1.0f), color),
                new Vertex(new Vector4(side, -side, side, 1.0f), color),

                new Vertex(new Vector4(-side, side, -side, 1.0f), color),
                new Vertex(new Vector4(side, side, -side, 1.0f), color),
                new Vertex(new Vector4(-side, side, side, 1.0f), color),
                new Vertex(new Vector4(-side, side, side, 1.0f), color),
                new Vertex(new Vector4(side, side, -side, 1.0f), color),
                new Vertex(new Vector4(side, side, side, 1.0f), color),

                new Vertex(new Vector4(-side, -side, -side, 1.0f), color),
                new Vertex(new Vector4(side, -side, -side, 1.0f), color),
                new Vertex(new Vector4(-side, side, -side, 1.0f), color),
                new Vertex(new Vector4(-side, side, -side, 1.0f), color),
                new Vertex(new Vector4(side, -side, -side, 1.0f), color),
                new Vertex(new Vector4(side, side, -side, 1.0f), color),

                new Vertex(new Vector4(-side, -side, side, 1.0f), color),
                new Vertex(new Vector4(side, -side, side, 1.0f), color),
                new Vertex(new Vector4(-side, side, side, 1.0f), color),
                new Vertex(new Vector4(-side, side, side, 1.0f), color),
                new Vertex(new Vector4(side, -side, side, 1.0f), color),
                new Vertex(new Vector4(side, side, side, 1.0f), color),
            };
            return vertices;
        }
    }
}