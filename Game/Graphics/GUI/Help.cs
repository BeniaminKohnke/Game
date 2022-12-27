using GameAPI;
using SFML.Graphics;
using SFML.Window;

namespace Game.Graphics.GUI
{
    internal sealed class Help : Page
    {
        private readonly Sprite _helpSprite = new();
        private readonly Sprite _cursorSprite = new();
        private readonly Font _font;

        internal Help(Font font)
        {
            var grid = File
                .ReadAllLines($@"{Interface._texturesDirectory}\{Textures.Help}.sm")
                .Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray())
                .ToArray();
            if (grid.Length > 0 && grid[0].Length > 0)
            {
                _helpSprite.Texture = new(Engine.CreateImage(grid));
            }

            grid = File
                .ReadAllLines($@"{Interface._texturesDirectory}\{Textures.Cursor}.sm")
                .Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray())
                .ToArray();
            if (grid.Length > 0 && grid[0].Length > 0)
            {
                _cursorSprite.Texture = new(Engine.CreateImage(grid));
            }

            _font = font;
        }

        internal override void Draw(RenderWindow window, GameWorld? world)
        {
            window.Draw(_helpSprite);
        }

        internal override bool HandleInput(KeyEventArgs args)
        {
            return false;
        }

        internal override void Reset()
        {
            
        }
    }
}
