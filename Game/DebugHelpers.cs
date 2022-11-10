using SFML.Graphics;

namespace Game
{
    internal static class DebugHelpers
    {
        public static void DrawRectangleVertexes(RenderWindow window)
        {
            var i = new Image(8, 14, Color.Transparent);
            i.SetPixel(0, 0, Color.Red);
            i.SetPixel(7, 13, Color.Blue);
            i.SetPixel(0, 13, Color.Yellow);
            i.SetPixel(7, 0, Color.Green);

            var s = new Sprite(new Texture(i))
            {
                Position = new(-50, -50),
                Scale = new(0.1f,0.1f)
            };
            window.Draw(s);
        }
    }
}
