using GameAPI;
using GameAPI.DSL;
using MoreLinq;
using SFML.Graphics;
using SFML.Window;

namespace Game.GUI
{
    internal sealed class CodeEditor : Page
    {
        private readonly string[] _scriptsNames = new string[20];
        private readonly CodeHandler _codeHandler;
        private readonly Dictionary<Keyboard.Key, char> _scriptsCharsNormal = new()
        {
            [Keyboard.Key.A] = 'a',
            [Keyboard.Key.B] = 'b',
            [Keyboard.Key.C] = 'c',
            [Keyboard.Key.D] = 'd',
            [Keyboard.Key.E] = 'e',
            [Keyboard.Key.F] = 'f',
            [Keyboard.Key.G] = 'g',
            [Keyboard.Key.H] = 'h',
            [Keyboard.Key.I] = 'i',
            [Keyboard.Key.J] = 'j',
            [Keyboard.Key.K] = 'k',
            [Keyboard.Key.L] = 'l',
            [Keyboard.Key.M] = 'm',
            [Keyboard.Key.N] = 'n',
            [Keyboard.Key.O] = 'o',
            [Keyboard.Key.P] = 'p',
            [Keyboard.Key.Q] = 'q',
            [Keyboard.Key.R] = 'r',
            [Keyboard.Key.S] = 's',
            [Keyboard.Key.T] = 't',
            [Keyboard.Key.U] = 'u',
            [Keyboard.Key.V] = 'v',
            [Keyboard.Key.W] = 'w',
            [Keyboard.Key.X] = 'x',
            [Keyboard.Key.Y] = 'y',
            [Keyboard.Key.Z] = 'z',
            [Keyboard.Key.Space] = ' ',
            [Keyboard.Key.Enter] = '\n',
            [Keyboard.Key.Tab] = '\t',
        };
        private readonly Dictionary<Keyboard.Key, char> _scriptsCharsShift = new()
        {
            [Keyboard.Key.A] = 'A',
            [Keyboard.Key.B] = 'B',
            [Keyboard.Key.C] = 'C',
            [Keyboard.Key.D] = 'D',
            [Keyboard.Key.E] = 'E',
            [Keyboard.Key.F] = 'F',
            [Keyboard.Key.G] = 'G',
            [Keyboard.Key.H] = 'H',
            [Keyboard.Key.I] = 'I',
            [Keyboard.Key.J] = 'J',
            [Keyboard.Key.K] = 'K',
            [Keyboard.Key.L] = 'L',
            [Keyboard.Key.M] = 'M',
            [Keyboard.Key.N] = 'N',
            [Keyboard.Key.O] = 'O',
            [Keyboard.Key.P] = 'P',
            [Keyboard.Key.Q] = 'Q',
            [Keyboard.Key.R] = 'R',
            [Keyboard.Key.S] = 'S',
            [Keyboard.Key.T] = 'T',
            [Keyboard.Key.U] = 'U',
            [Keyboard.Key.V] = 'V',
            [Keyboard.Key.W] = 'W',
            [Keyboard.Key.X] = 'X',
            [Keyboard.Key.Y] = 'Y',
            [Keyboard.Key.Z] = 'Z',
        };
        private readonly Sprite _cursorSprite = new();
        private readonly Sprite _textEditorSprite = new();
        private readonly Sprite _menuSprite = new();
        private readonly Sprite _menuCursorSprite = new();
        private readonly Text _text = new();
        private readonly Text[] _menuOptions = new Text[]
        {
            new()
            {
                DisplayedString = "Add or rename",
                CharacterSize = 200,
                Scale = new(0.01f, 0.01f),
            },
            new()
            {
                DisplayedString = "Edit",
                CharacterSize = 200,
                Scale = new(0.01f, 0.01f),
            },
            new()
            {
                DisplayedString = "Save",
                CharacterSize = 200,
                Scale = new(0.01f, 0.01f),
            },
            new()
            {
                DisplayedString = "Delete",
                CharacterSize = 200,
                Scale = new(0.01f, 0.01f),
            },
            new()
            {
                DisplayedString = "Compile",
                CharacterSize = 200,
                Scale = new(0.01f, 0.01f),
            },
        };
        private bool _isMenuActive = false;
        private sbyte _cursorCurrentPosition = 0;
        public sbyte CursorCurrentPosition
        {
            get => _cursorCurrentPosition;
            set => _cursorCurrentPosition = (sbyte)(value > 20 ? 0 : (value < 0 ? 20 : value));
        }
        private sbyte _menuCursorCurrentPosition = 0;
        public sbyte MenuCursorCurrentPosition
        {
            get => _menuCursorCurrentPosition;
            set => _menuCursorCurrentPosition = (sbyte)(value > 3 ? 0 : (value < 0 ? 3 : value));
        }
        internal string EditedText { get; set; } = string.Empty;

        internal CodeEditor(Font font, GameWorld world)
        {
            _codeHandler = new(world);

            var grid = File
                .ReadAllLines($@"{Interface.TexturesDirectory}\{Textures.CodeEditor}.sm")
                .Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray())
                .ToArray();
            if (grid.Length > 0 && grid[0].Length > 0)
            {
                _textEditorSprite.Texture = new(Engine.CreateImage(grid));
            }

            _text = new()
            {
                Font = font,
                DisplayedString = "100",
                CharacterSize = 100,
                Position = new(34, 11),
                Scale = new(0.01f, 0.01f),
            };

            grid = File
                .ReadAllLines($@"{Interface.TexturesDirectory}\{Textures.Cursor}.sm")
                .Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray())
                .ToArray();
            if (grid.Length > 0 && grid[0].Length > 0)
            {
                _cursorSprite.Texture = new(Engine.CreateImage(grid));
                _menuCursorSprite.Texture = new(Engine.CreateImage(grid));
            }

            grid = File
                .ReadAllLines($@"{Interface.TexturesDirectory}\{Textures.CodeEditorMenu}.sm")
                .Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray())
                .ToArray();
            if (grid.Length > 0 && grid[0].Length > 0)
            {
                _menuSprite.Texture = new(Engine.CreateImage(grid));
            }

            _menuOptions.ForEach(p => p.Font = font);
        }

        internal override void Draw(RenderWindow window, GameWorld world)
        {
            window.Draw(_textEditorSprite);
            _cursorSprite.Position = new(2, 13 + CursorCurrentPosition * 6);
            window.Draw(_cursorSprite);
            window.Draw(_text);

            if (_isMenuActive)
            {
                _menuSprite.Position = new(_cursorSprite.Position.X + 32, _cursorSprite.Position.Y);
                window.Draw(_menuSprite);

                _menuCursorSprite.Position = new(_menuSprite.Position.X, _menuSprite.Position.Y + MenuCursorCurrentPosition * 6);
                window.Draw(_menuCursorSprite);
                
                for (var i = 0; i < _menuOptions.Length; i++)
                {
                    _menuOptions[i].Position = new(_menuSprite.Position.X + 8, _menuSprite.Position.Y + i * 6 + 2);
                    window.Draw(_menuOptions[i]);
                }
            }
        }

        internal override void HandleInput(KeyEventArgs args)
        {
            if (_isMenuActive)
            {
                if (args.Code == Keyboard.Key.Enter)
                {
                    switch (_menuOptions[MenuCursorCurrentPosition].DisplayedString)
                    {
                        case "Add or rename":
                            RenameScript();
                            break;
                        case "Delete":
                            DeleteScript();
                            break;
                        case "Save":
                            SaveScript();
                            break;
                        case "Compile":
                            CompileScript();
                            break;
                    }
                }

                if (args.Code == Keyboard.Key.Up)
                {
                    MenuCursorCurrentPosition--;
                }

                if (args.Code == Keyboard.Key.Down)
                {
                    MenuCursorCurrentPosition++;
                }
            }
            else
            {
                if (args.Code == Keyboard.Key.Enter)
                {
                    _isMenuActive = true;
                }

                if (args.Code == Keyboard.Key.Up)
                {
                    CursorCurrentPosition--;
                }

                if (args.Code == Keyboard.Key.Down)
                {
                    CursorCurrentPosition++;
                }
            }
        }

        public void RenameScript()
        {
            if (true)
            {

            }
        }

        public void DeleteScript()
        {

        }

        public void SaveScript()
        {

        }

        public void CompileScript()
        {

        }

        internal override void Reset()
        {
            CursorCurrentPosition = 0;
        }

        internal override void Release()
        {
            throw new NotImplementedException();
        }
    }
}
