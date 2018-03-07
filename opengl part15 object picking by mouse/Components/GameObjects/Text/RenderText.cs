using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using techdump.opengl.Components.Cameras;
using techdump.opengl.Components.Renderables;

namespace techdump.opengl.Components.GameObjects.Text
{
    // valve, chris green: http://www.valvesoftware.com/publications/2007/SIGGRAPH2007_AlphaTestedMagnification.pdf
    // mapbox: https://www.mapbox.com/blog/text-signed-distance-fields/
    public class RenderText : AGameObject
    {
        private readonly Vector4 _color;
        public const string Characters = @"qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789µ§½!""#¤%&/()=?^*@£€${[]}\~¨'-_.:,;<>|°©®±¥";
        private static readonly Dictionary<char, int> Lookup;
        public static readonly float CharacterWidthNormalized;
        // 21x48 per char, 
        public readonly List<RenderCharacter> Text;
        static RenderText()
        {
            Lookup = new Dictionary<char, int>();
            for (int i = 0; i < Characters.Length; i++)
            {
                if (!Lookup.ContainsKey(Characters[i]))
                    Lookup.Add(Characters[i], i);
            }
            CharacterWidthNormalized = 1f / Characters.Length;
        }
        public RenderText(ARenderable model, Vector4 position, Color4 color, string value)
            : base(model, position, Vector4.Zero, Vector4.Zero, 0)
        {
            _color = new Vector4(color.R, color.G, color.B, color.A);
            Text = new List<RenderCharacter>(value.Length);
            _scale = new Vector3(0.02f);
            SetText(value);
        }
        public void SetText(string value)
        {
            Text.Clear();
            for (int i = 0; i < value.Length; i++)
            {
                int offset;
                if (Lookup.TryGetValue(value[i], out offset))
                {
                    var c = new RenderCharacter(Model,
                        new Vector4(_position.X + (i * 0.015f),
                            _position.Y,
                            _position.Z,
                            _position.W),
                        (offset*CharacterWidthNormalized));
                    c.SetScale(_scale);
                    Text.Add(c);
                }
            }
        }
        public override void Render(ICamera camera)
        {
            _model.Bind();
            GL.VertexAttrib4(3, _color);
            for (int i = 0; i < Text.Count; i++)
            {
                var c = Text[i];
                c.Render(camera);
            }
        }
    }
}