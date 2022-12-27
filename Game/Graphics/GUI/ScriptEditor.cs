using GameAPI;
using GameAPI.DSL;
using MoreLinq;
using SFML.Graphics;
using SFML.Window;

namespace Game.Graphics.GUI
{
    internal sealed class ScriptEditor : Page
    {
        private enum MenuActions : byte
        {
            None,
            Add_Rename,
            Delete,
            Save,
            Edit,
            Compile,
        }

        private readonly (Text name, string code, byte verificationId)[] _scripts = new (Text, string, byte)[21];
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
            [Keyboard.Key.Num0] = '0',
            [Keyboard.Key.Num1] = '1',
            [Keyboard.Key.Num2] = '2',
            [Keyboard.Key.Num3] = '3',
            [Keyboard.Key.Num4] = '4',
            [Keyboard.Key.Num5] = '5',
            [Keyboard.Key.Num6] = '6',
            [Keyboard.Key.Num7] = '7',
            [Keyboard.Key.Num8] = '8',
            [Keyboard.Key.Num9] = '9',
            [Keyboard.Key.Space] = ' ',
            [Keyboard.Key.Enter] = '\n',
            [Keyboard.Key.Tab] = '\t',
            [Keyboard.Key.LBracket] = '[',
            [Keyboard.Key.RBracket] = ']',
            [Keyboard.Key.Quote] = '\'',
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
            [Keyboard.Key.LBracket] = '{',
            [Keyboard.Key.RBracket] = '}',
        };
        private readonly Sprite _cursorSprite = new();
        private readonly Sprite _textEditorSprite = new();
        private readonly Sprite _menuSprite = new();
        private readonly Sprite _menuCursorSprite = new();
        private readonly Sprite[] _verificationIcons = new Sprite[3];
        private readonly Text _currentScriptText = new();
        private readonly Text[] _menuOptions;
        private bool _isMenuActive = false;
        private MenuActions _menuAction = MenuActions.None;
        private sbyte _cursorCurrentPosition = 0;
        public sbyte CursorCurrentPosition
        {
            get => _cursorCurrentPosition;
            set => _cursorCurrentPosition = (sbyte)(value > 20 ? 0 : value < 0 ? 20 : value);
        }
        private sbyte _menuCursorCurrentPosition = 0;
        public sbyte MenuCursorCurrentPosition
        {
            get => _menuCursorCurrentPosition;
            set => _menuCursorCurrentPosition = (sbyte)(value > 4 ? 0 : value < 0 ? 4 : value);
        }
        internal string EditedText { get; set; } = string.Empty;

        internal ScriptEditor(Font font)
        {
            var grid = File
                .ReadAllLines($@"{Interface._texturesDirectory}\{Textures.CodeEditor}.sm")
                .Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray())
                .ToArray();
            if (grid.Length > 0 && grid[0].Length > 0)
            {
                _textEditorSprite.Texture = new(Engine.CreateImage(grid));
            }

            var scripts = ScriptBuilder.GetExistingScripts();
            for (var i = 0; i < 21; i++)
            {
                _scripts[i].name = new()
                {
                    Font = font,
                    DisplayedString = i == 0 ? "CallOrder" : scripts.Length > i - 1 ? scripts[i - 1].name : string.Empty,
                    CharacterSize = 100,
                    Scale = new(0.01f, 0.01f),
                };
                _scripts[i].verificationId = 2;
                _scripts[i].code = i == 0 ? string.Join("\n", ScriptBuilder.CallOrder) : scripts.Length > i - 1 ? scripts[i - 1].script : string.Empty;
            }

            _currentScriptText = new()
            {
                Font = font,
                CharacterSize = 125,
                Position = new(34, 11),
                Scale = new(0.01f, 0.015f),
                LineSpacing = 1.5f
            };

            grid = File
                .ReadAllLines($@"{Interface._texturesDirectory}\{Textures.Cursor}.sm")
                .Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray())
                .ToArray();
            if (grid.Length > 0 && grid[0].Length > 0)
            {
                _cursorSprite.Texture = new(Engine.CreateImage(grid));
                _menuCursorSprite.Texture = new(Engine.CreateImage(grid));
            }

            grid = File
                .ReadAllLines($@"{Interface._texturesDirectory}\{Textures.CodeEditorMenu}.sm")
                .Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray())
                .ToArray();
            if (grid.Length > 0 && grid[0].Length > 0)
            {
                _menuSprite.Texture = new(Engine.CreateImage(grid));
            }

            var iconsTextures = new[] { Textures.ScriptRejected, Textures.ScriptApproved, Textures.ScriptNotVerified };
            for (var i = 0; i < _verificationIcons.Length; i++)
            {
                grid = File
                    .ReadAllLines($@"{Interface._texturesDirectory}\{iconsTextures[i]}.sm")
                    .Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray())
                    .ToArray();
                if (grid.Length > 0 && grid[0].Length > 0)
                {
                    _verificationIcons[i] = new()
                    {
                        Position = new(229, 133),
                        Texture = new(Engine.CreateImage(grid))
                    };
                }
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

        internal override void Draw(RenderWindow window, GameWorld? world)
        {
            window.Draw(_textEditorSprite);
            _cursorSprite.Position = new(2, 13 + CursorCurrentPosition * 6);
            window.Draw(_cursorSprite);

            for (var i = 0; i < _scripts.Length; i++)
            {
                _scripts[i].name.Position = new(4, 16 + i * 6);
                window.Draw(_scripts[i].name);
            }

            var (_, code, verificationId) = _scripts[CursorCurrentPosition];
            _currentScriptText.DisplayedString = code;
            window.Draw(_currentScriptText);
            window.Draw(_verificationIcons[verificationId]);

            if (_isMenuActive)
            {
                _menuSprite.Position = new(_cursorSprite.Position.X + 32, _cursorSprite.Position.Y);
                window.Draw(_menuSprite);

                _menuCursorSprite.Position = new(_menuSprite.Position.X, _menuSprite.Position.Y + MenuCursorCurrentPosition * 6);
                window.Draw(_menuCursorSprite);

                for (var i = CursorCurrentPosition == 0 ? 2 : 0; i < _menuOptions.Length; i++)
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
                                    if (CursorCurrentPosition != 0)
                                    {
                                        _menuAction = MenuActions.Add_Rename;
                                        _isMenuActive = false;
                                    }
                                    break;
                                case "Edit":
                                    _menuAction = MenuActions.Edit;
                                    _isMenuActive = false;
                                    break;
                                case "Delete":
                                    if (CursorCurrentPosition != 0)
                                    {
                                        ScriptBuilder.DeleteScript(_scripts[CursorCurrentPosition].name.DisplayedString);
                                        _scripts[CursorCurrentPosition].name.DisplayedString = string.Empty;
                                        _scripts[CursorCurrentPosition].code = string.Empty;
                                        _scripts[CursorCurrentPosition].verificationId = 2;
                                        _isMenuActive = false;
                                    }
                                    break;
                                case "Save":
                                    var (name, code, _) = _scripts[CursorCurrentPosition];
                                    ScriptBuilder.SaveScript(name.DisplayedString.Trim(), code.Trim());
                                    _isMenuActive = false;
                                    break;
                                case "Compile":
                                    var script = _scripts[CursorCurrentPosition];
                                    _scripts[CursorCurrentPosition].verificationId = Convert.ToByte(ScriptBuilder.CompileScript(script.name.DisplayedString.Trim(), script.code.Trim()));
                                    _menuAction = MenuActions.None;
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
                            _scripts[CursorCurrentPosition].verificationId = 2;
                        }
                    }

                    if (args.Shift)
                    {
                        if (_scriptsCharsShift.TryGetValue(args.Code, out var character))
                        {
                            _scripts[CursorCurrentPosition].code += character;
                            _scripts[CursorCurrentPosition].verificationId = 2;
                        }
                    }
                    else
                    {
                        if (_scriptsCharsNormal.TryGetValue(args.Code, out var character))
                        {
                            _scripts[CursorCurrentPosition].code += character;
                            _scripts[CursorCurrentPosition].verificationId = 2;
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
    }
}
