using GameAPI;
using SFML.Graphics;
using SFML.Window;

namespace Game.Graphics.GUI
{
    internal sealed class Equipment : Page
    {
        private readonly Sprite _equipmentSprite = new();
        private readonly Sprite _cursorSprite = new();
        private readonly Font _font;
        private readonly (int x, int y) _equipmentPosition = (0, 0);
        private readonly (int x, int y) _cursorPosition = (2, 13);
        private sbyte _itemMenuPosition = 0;
        private readonly ItemTypes[] _allowedTypes = new[]
        {
            ItemTypes.Melee,
            ItemTypes.Ranged,
            ItemTypes.Consumable,
            ItemTypes.Amunition
        };
        private sbyte _cursorCurrentPosition = 0;
        public sbyte CursorCurrentPosition
        {
            get => _cursorCurrentPosition;
            set => _cursorCurrentPosition = (sbyte)(value > 20 ? 0 : value < 0 ? 20 : value);
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

        internal override void Draw(RenderWindow window, GameWorld? gameWorld)
        {
            if (gameWorld != null)
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
                        window.Draw(new Text
                        {
                            DisplayedString = $"[{item.ItemType}]\n{string.Join("\n", item.ObjectParameters.Select(p => $"{p.Key} : {p.Value}"))}",
                            Position = new(_cursorPosition.x + 35, _cursorPosition.y),
                            Font = _font,
                            CharacterSize = 200,
                            Scale = new(0.01f, 0.01f),
                        });

                        if (_itemMenuPosition != -1 && _allowedTypes.Contains(item.ItemType))
                        {
                            var id = group.Last().Id;
                            for (var x = 0; x < gameWorld.Player.ItemsMenu.Length; x++)
                            {
                                if (gameWorld.Player.ItemsMenu[x] == id)
                                {
                                    gameWorld.Player.ItemsMenu[x] = 0;
                                }
                            }
                            gameWorld.Player.ItemsMenu[_itemMenuPosition] = id;
                            _itemMenuPosition = -1;
                        }
                    }

                    window.Draw(new Text
                    {
                        DisplayedString = $"{item.Name} x {group.Count()}",
                        Position = new(_cursorPosition.x + 2, _cursorPosition.y + i * 6 + 3),
                        Font = _font,
                        CharacterSize = 120,
                        Scale = new(0.01f, 0.01f),
                    });

                    i++;
                }
            }
        }

        internal override bool HandleInput(KeyEventArgs args)
        {
            switch (args.Code)
            {
                case Keyboard.Key.Up:
                    CursorCurrentPosition--;
                    break;
                case Keyboard.Key.Down:
                    CursorCurrentPosition++;
                    break;
                case Keyboard.Key.Num0:
                    _itemMenuPosition = 9;
                    break;
                case Keyboard.Key.Num1:
                    _itemMenuPosition = 0;
                    break;
                case Keyboard.Key.Num2:
                    _itemMenuPosition = 1;
                    break;
                case Keyboard.Key.Num3:
                    _itemMenuPosition = 2;
                    break;
                case Keyboard.Key.Num4:
                    _itemMenuPosition = 3;
                    break;
                case Keyboard.Key.Num5:
                    _itemMenuPosition = 4;
                    break;
                case Keyboard.Key.Num6:
                    _itemMenuPosition = 5;
                    break;
                case Keyboard.Key.Num7:
                    _itemMenuPosition = 6;
                    break;
                case Keyboard.Key.Num8:
                    _itemMenuPosition = 7;
                    break;
                case Keyboard.Key.Num9:
                    _itemMenuPosition = 8;
                    break;
            }

            return false;
        }

        internal override void Reset()
        {
            CursorCurrentPosition = 0;
        }
    }
}
