using GameAPI;
using SFML.Graphics;
using SFML.Window;

namespace Game.GUI
{
    internal sealed class MainMenu : Page
    {
        private readonly Sprite _cursorSprite = new();
        private readonly Sprite _menuSprite = new();
        private readonly (int x, int y) _cursorPosition = (2, 13);
        private sbyte _cursorCurrentPosition = 0;
        public sbyte CursorCurrentPosition
        {
            get => _cursorCurrentPosition;
            set => _cursorCurrentPosition = (sbyte)(value > 20 ? 0 : (value < 0 ? 20 : value));
        }

        internal MainMenu()
        {
            var grid = File
                .ReadAllLines($@"{Interface.TexturesDirectory}\{Textures.MainMenu}.sm")
                .Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray())
                .ToArray();
            if (grid.Length > 0 && grid[0].Length > 0)
            {
                _menuSprite.Texture = new(Engine.CreateImage(grid));
            }

            grid = File
                .ReadAllLines($@"{Interface.TexturesDirectory}\{Textures.Cursor}.sm")
                .Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray())
                .ToArray();
            if (grid.Length > 0 && grid[0].Length > 0)
            {
                _cursorSprite.Texture = new(Engine.CreateImage(grid));
            }
        }

        internal override void Draw(RenderWindow window, GameWorld world)
        {
            window.Draw(_menuSprite);
            _cursorSprite.Position = new(_cursorPosition.x, _cursorPosition.y + CursorCurrentPosition * 6);
            window.Draw(_cursorSprite);
        }

        internal override void HandleInput(KeyEventArgs args)
        {
            if (args.Code == Keyboard.Key.Up)
            {
                CursorCurrentPosition--;
            }

            if (args.Code == Keyboard.Key.Down)
            {
                CursorCurrentPosition++;
            }
        }

        internal override void Reset()
        {
            CursorCurrentPosition = 0;
        }

        internal override void Release()
        {
            
        }
    }
}
