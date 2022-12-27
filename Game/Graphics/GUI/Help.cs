using GameAPI;
using SFML.Graphics;
using SFML.Window;

namespace Game.Graphics.GUI
{
    internal sealed class Help : Page
    {
        private readonly Dictionary<string, (string name, string definition)[]> _dictionary = new()
        {
            ["Scripts - rules"] = new[]
            {
                ("CallOrder", "defines order of scripts' execution"),
                ("[variable]", "we declare variables in brackets"),
                ("tabulations", "we use them to separate segments of the code"),
                ("IF condition THEN", "conditional statement, which will execute code in new line"),
                ("ELSE THEN", "part of IF statement, executes other instructions"),
                ("FOR SINGLE collection FROM collection DO", "executes code for each element in colleciton"),
                ("value SAVE TO [variable]", "asigns value to variable"),
                ("[value1] LESS THAN [value2]", "compares values"),
                ("[value1] MORE THAN [value2]", "compares values"),
                ("[value1] LESS OR EQUAL THAN [value2]", "compares values"),
                ("[value1] MORE OR EQUAL THAN [value2]", "compares values"),
                ("[value1] EQUALS [value2]", "compares values"),
                ("FINISH", "finish the FOR SINGLE loop"),

            },
            ["Scripts - functions"] = new[]
            {
                ("Use [item]", "uses item, which can be position in the menu, item's name or object"),
                ("DirectionBetween [first] [second]", "returns largest direction between two objects"),
                ("GoTo [direction]", "leads to chosen direction"),
                ("DistanceBetween [first] [second]", "returns distance between two objects"),
                ("RangeOf [gameObject]", "returns range of game object"),
                ("CraftItem [item]", "crafts item, which can be text of item name or item object"),
                ("Count [collection] [element]", "returns count of similar elements"),
                ("CallScript [name]", "calls player's script"),
                ("ScanArea", "returns collection of the objects which surounds player")
            },
            ["Controls"] = new[]
            {
                ("Arrow Up", "move character up\\navigate in menu"),
                ("Arrow Down", "move character down\\navigate in menu"),
                ("Arrow Left", "move character left\\navigate in menu"),
                ("Arrow Right", "move character right\\navigate in menu"),
                ("space", "use item"),
                ("num 1 - 9", "change used item\\change item assignment in Items"),
                ("F1", "reload scripts"),
                ("F2", "run scripts"),
                ("F3", "stop script"),
                ("Enter", "accept action"),
                ("Esc", "escape page"),
            },
        };
        private readonly Sprite _helpSprite = new();
        private readonly Sprite _cursorSprite = new();
        private readonly Font _font;
        private readonly (int x, int y) _cursorPosition = (2, 13);
        private sbyte _cursorCurrentPosition = 0;
        public sbyte CursorCurrentPosition
        {
            get => _cursorCurrentPosition;
            set => _cursorCurrentPosition = (sbyte)(value > 20 ? 0 : value < 0 ? 20 : value);
        }

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
            _cursorSprite.Position = new(_cursorPosition.x, _cursorPosition.y + CursorCurrentPosition * 6);
            window.Draw(_cursorSprite);

            var i = 0;
            foreach (var position in _dictionary)
            {
                if (i == _cursorCurrentPosition)
                {
                    var description = new Text
                    {
                        DisplayedString = string.Join("\n", position.Value.Select(d => $"{d.name} - {d.definition}")),
                        Position = new(_cursorPosition.x + 35, _cursorPosition.y),
                        Font = _font,
                        CharacterSize = 200,
                        Scale = new(0.01f, 0.01f),
                    };
                    window.Draw(description);
                }

                var text = new Text
                {
                    DisplayedString = position.Key,
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
            if (args.Code == Keyboard.Key.Escape)
            {
                return false;
            }

            if (args.Code == Keyboard.Key.Up)
            {
                CursorCurrentPosition--;
            }

            if (args.Code == Keyboard.Key.Down)
            {
                CursorCurrentPosition++;
            }

            return true;
        }

        internal override void Reset()
        {
            _cursorCurrentPosition = 0;
        }
    }
}
