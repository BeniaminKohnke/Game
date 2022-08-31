using GameAPI;
using GameAPI.DSL;
using MoreLinq;
using SFML.Graphics;
using SFML.Window;

namespace Game.GUI
{
    internal sealed class CodeEditor : Page
    {
        private enum MenuActions
        {
            None,
            Add_Rename,
            Edit,
            Save,
            Delete,
            Compile,
        }

        private readonly (Text name, string code)[] _scripts = new (Text, string)[21];
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
        private readonly Text _currentScriptText = new();
        private readonly Text[] _menuOptions;
        private bool _isMenuActive = false;
        private MenuActions _menuAction = MenuActions.None;
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
            set => _menuCursorCurrentPosition = (sbyte)(value > 4 ? 0 : (value < 0 ? 4 : value));
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
            
            for (var i = 0; i < 21; i++)
            {
                _scripts[i].name = new()
                {
                    Font = font,
                    CharacterSize = 100,
                    Scale = new(0.01f, 0.01f),
                };
            }

            _currentScriptText = new()
            {
                Font = font,
                CharacterSize = 200,
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

            var options = new List<Text>();
            foreach (var position in Enum.GetValues(typeof(MenuActions)).Flatten().Skip(1))
            {
                options.Add(new()
                {
                    Font = font,
                    DisplayedString = position.ToString(),
                    CharacterSize = 200,
                    Scale = new(0.01f, 0.01f),
                });
            }
            _menuOptions = options.ToArray();
        }

        internal override void Draw(RenderWindow window, GameWorld world)
        {
            window.Draw(_textEditorSprite);
            _cursorSprite.Position = new(2, 13 + CursorCurrentPosition * 6);
            window.Draw(_cursorSprite);

            for (var i = 0; i < _scripts.Length; i++)
            {
                _scripts[i].name.Position = new(4, 16 + i * 6);
                window.Draw(_scripts[i].name);
            }

            _currentScriptText.DisplayedString = _scripts[CursorCurrentPosition].code;
            window.Draw(_currentScriptText);

            if (_isMenuActive)
            {
                _menuSprite.Position = new(_cursorSprite.Position.X + 32, _cursorSprite.Position.Y);
                window.Draw(_menuSprite);

                _menuCursorSprite.Position = new(_menuSprite.Position.X, _menuSprite.Position.Y + MenuCursorCurrentPosition * 6);
                window.Draw(_menuCursorSprite);
                
                for (var i = 0; i < _menuOptions.Length; i++)
                {
                    _menuOptions[i].Position = new(_menuSprite.Position.X + 3, _menuSprite.Position.Y + i * 6 + 2);
                    window.Draw(_menuOptions[i]);
                }
            }
        }

        internal override bool HandleInput(KeyEventArgs args)
        {
            switch (_menuAction)
            {
                case MenuActions.None:
                    if (_isMenuActive)
                    {
                        if (args.Code == Keyboard.Key.Escape)
                        {
                            _isMenuActive = false;
                            _menuAction = MenuActions.None;
                            return true;
                        }

                        if (args.Code == Keyboard.Key.Enter)
                        {
                            switch (_menuOptions[MenuCursorCurrentPosition].DisplayedString)
                            {
                                case "Add_Rename":
                                    _menuAction = MenuActions.Add_Rename;
                                    _isMenuActive = false;
                                    break;
                                case "Edit":
                                    _menuAction = MenuActions.Edit;
                                    _isMenuActive = false;
                                    break;
                                case "Delete":
                                    _menuAction = MenuActions.Delete;
                                    _isMenuActive = false;
                                    break;
                                case "Save":
                                    _menuAction = MenuActions.Save;
                                    _isMenuActive = false;
                                    break;
                                case "Compile":
                                    _menuAction = MenuActions.Compile;
                                    _isMenuActive = false;
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
                    break;
                case MenuActions.Add_Rename:
                    if (args.Code == Keyboard.Key.Escape)
                    {
                        _isMenuActive = true;
                        _menuAction = MenuActions.None;
                        return true;
                    }

                    if (args.Code == Keyboard.Key.Backspace)
                    {
                        var text = _scripts[CursorCurrentPosition].name.DisplayedString;
                        if (!string.IsNullOrEmpty(text))
                        {
                            _scripts[CursorCurrentPosition].name.DisplayedString = new(text.Take(text.Length - 1).ToArray());
                        }
                    }

                    if (args.Shift)
                    {
                        if (_scriptsCharsShift.TryGetValue(args.Code, out var character))
                        {
                            _scripts[CursorCurrentPosition].name.DisplayedString += character;
                        }
                    }
                    else
                    {
                        if (_scriptsCharsNormal.TryGetValue(args.Code, out var character))
                        {
                            _scripts[CursorCurrentPosition].name.DisplayedString += character;
                        }
                    }
                    break;
                case MenuActions.Edit:
                    if (args.Code == Keyboard.Key.Escape)
                    {
                        _isMenuActive = true;
                        _menuAction = MenuActions.None;
                        return true;
                    }

                    if (args.Code == Keyboard.Key.Backspace)
                    {
                        var text = _scripts[CursorCurrentPosition].code;
                        if (!string.IsNullOrEmpty(text))
                        {
                            _scripts[CursorCurrentPosition].code = new(text.Take(text.Length - 1).ToArray());
                        }
                    }
                    
                    if (args.Shift)
                    {
                        if (_scriptsCharsShift.TryGetValue(args.Code, out var character))
                        { 
                            _scripts[CursorCurrentPosition].code += character;
                        }
                    }
                    else
                    {
                        if (_scriptsCharsNormal.TryGetValue(args.Code, out var character))
                        {
                            _scripts[CursorCurrentPosition].code += character;
                        }
                    }
                    break;
                case MenuActions.Save:
                    if (args.Code == Keyboard.Key.Escape)
                    {
                        _isMenuActive = true;
                        _menuAction = MenuActions.None;
                        return true;
                    }
                    break;
                case MenuActions.Delete:
                    if (args.Code == Keyboard.Key.Escape)
                    {
                        _isMenuActive = true;
                        _menuAction = MenuActions.None;
                        return true;
                    }

                    {
                        var scriptName = _scripts[CursorCurrentPosition].name.DisplayedString;
                        if (!string.IsNullOrEmpty(scriptName))
                        {
                            _codeHandler.DeleteScript(scriptName);
                        }
                    }
                    break;
                case MenuActions.Compile:
                    if (args.Code == Keyboard.Key.Escape)
                    {
                        _isMenuActive = true;
                        _menuAction = MenuActions.None;
                        return true;
                    }

                    {
                        var scriptName = _scripts[CursorCurrentPosition].name.DisplayedString;
                        if (!string.IsNullOrEmpty(scriptName))
                        {
                            _codeHandler.DeleteScript(scriptName);
                        }
                    }
                    break;
            }

            return false;
        }

        internal override void Reset()
        {
            CursorCurrentPosition = 0;
            _menuAction = MenuActions.None;
            _isMenuActive = false;
        }

        internal override void Release()
        {
            _codeHandler.IsActive = false;
        }
    }
}
