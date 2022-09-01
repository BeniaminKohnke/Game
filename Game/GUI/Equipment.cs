using GameAPI;
using SFML.Graphics;
using SFML.Window;

namespace Game.GUI
{
    internal sealed class Equipment : Page
    {
        private readonly Sprite _equipmentSprite = new();
        private readonly Sprite _cursorSprite = new();
        private readonly Font _font;
        private readonly (int x, int y) _equipmentPosition = (0, 0);
        private readonly (int x, int y) _cursorPosition = (2, 13);
        private sbyte _cursorCurrentPosition = 0;
        public sbyte CursorCurrentPosition 
        { 
            get => _cursorCurrentPosition;
            set => _cursorCurrentPosition = (sbyte)(value > 20 ? 0 : (value < 0 ? 20 : value));
        }

        internal Equipment(Font font)
        {
            var grid = File
                .ReadAllLines($@"{Interface._texturesDirectory}\{Textures.EquipmentWindow}.sm")
                .Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray())
                .ToArray();
            if (grid.Length > 0 && grid[0].Length > 0)
            {
                _equipmentSprite.Texture = new(Engine.CreateImage(grid));
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

        internal override void Draw(RenderWindow window, GameWorld gameWorld)
        {
            _equipmentSprite.Position = new(_equipmentPosition.x, _equipmentPosition.y);
            window.Draw(_equipmentSprite);

            _cursorSprite.Position = new(_cursorPosition.x, _cursorPosition.y + CursorCurrentPosition * 6);
            window.Draw(_cursorSprite);

            var i = 0;
            foreach (var group in gameWorld.Player.Items.GroupBy(i => i.Name))
            {
                var item = group.First();
                if (i == _cursorCurrentPosition)
                {
                    var specification = new Text
                    {
                        DisplayedString = $"[{item.ItemType}]\n{string.Join("\n", item.ObjectParameters.Select(p => $"{p.Key} : {p.Value}"))}",
                        Position = new(_cursorPosition.x + 35, _cursorPosition.y),
                        Font = _font,
                        CharacterSize = 200,
                        Scale = new(0.01f, 0.01f),
                    };
                    window.Draw(specification);
                }

                var text = new Text
                {
                    DisplayedString = $"{item.Name} x {group.Count()}",
                    Position = new(_cursorPosition.x + 2, _cursorPosition.y + i * 6 + 3),
                    Font = _font,
                    CharacterSize = 120,
                    Scale = new(0.01f, 0.01f),
                };
                window.Draw(text);
                i++;
            }
        }

        internal override bool HandleInput(KeyEventArgs args)
        {
            if (args.Code == Keyboard.Key.Up)
            {
                CursorCurrentPosition--;
            }

            if (args.Code == Keyboard.Key.Down)
            {
                CursorCurrentPosition++;
            }

            return false;
        }

        internal override void Reset()
        {
            CursorCurrentPosition = 0;
        }
    }
}
