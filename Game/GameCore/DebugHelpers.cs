using SFML.Graphics;

namespace Game.GameCore
{
    internal class DebugHelpers
    {
        internal static void DrawRectangleVertexes(RenderWindow window)
        {
            var i = new Image(8, 14, Color.Transparent);
            i.SetPixel(0, 0, Color.Red);
            i.SetPixel(7, 13, Color.Blue);
            i.SetPixel(0, 13, Color.Yellow);
            i.SetPixel(7, 0, Color.Green);

            var s = new Sprite(new Texture(i))
            {
                Position = new SFML.System.Vector2f(0, 0)
            };
            window.Draw(s);
        }
    }
}
