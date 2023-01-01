using GameAPI;
using MoreLinq;
using SFML.Graphics;
using SFML.Window;

namespace Game.Graphics.GUI
{
    public enum MenuOptions : byte
    {
        None,
        NewGame,
        Resume,
        Options,
        Exit,
    }

    internal sealed class MainMenu : Page
    {
        private bool _isSeedCheck = false;
        private bool _isOptions = false;
        private readonly Sprite _cursorSprite = new();
        private readonly Sprite _menuSprite = new();
        private readonly Sprite _seedSprite = new();
        private readonly Sprite _optionsSprite = new();
        private readonly Sprite _optionsCursorSprite = new();
        private readonly Text _frameLimitText;
        private readonly Text _showWeatherText;
        private readonly Text _drawDistanceText;
        private readonly Text _seedText;
        private readonly Text _difficultyText;
        private readonly Text _showHealthText;
        private readonly Text[] _menuOptions;
        private sbyte _cursorCurrentPosition = 0;
        private sbyte _optionsCursorCurrentPosition = 0;
        public int Seed { get; private set; }
        public ushort FramesLimit { get; private set; } = 240;
        public bool ShowWeather { get; private set; } = true;
        public bool ShowHealth { get; private set; } = true;
        public ushort DrawDistance { get; private set; } = 200;
        public Difficulty DifficultyLevel { get; private set; } = Difficulty.Easy;
        public sbyte CursorCurrentPosition
        {
            get => _cursorCurrentPosition;
            set => _cursorCurrentPosition = (sbyte)(value > 20 ? 0 : value < 0 ? 20 : value);
        }
        public sbyte OptionsCursorCurrentPosition
        {
            get => _optionsCursorCurrentPosition;
            set => _optionsCursorCurrentPosition = (sbyte)(value > 12 ? 0 : value < 0 ? 12 : value);
        }
        public MenuOptions PerformedAction { get; set; } = MenuOptions.None;

        internal MainMenu(Font font)
        {
            var grid = File
                .ReadAllLines($@"{Interface._texturesDirectory}\{Textures.MainMenu}.sm")
                .Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray())
                .ToArray();
            if (grid.Length > 0 && grid[0].Length > 0)
            {
                _menuSprite.Texture = new(Engine.CreateImage(grid));
            }

            grid = File
                .ReadAllLines($@"{Interface._texturesDirectory}\{Textures.Cursor}.sm")
                .Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray())
                .ToArray();
            if (grid.Length > 0 && grid[0].Length > 0)
            {
                _cursorSprite.Texture = new(Engine.CreateImage(grid));
            }

            grid = File
                .ReadAllLines($@"{Interface._texturesDirectory}\{Textures.Seed}.sm")
                .Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray())
                .ToArray();
            if (grid.Length > 0 && grid[0].Length > 0)
            {
                _seedSprite.Texture = new(Engine.CreateImage(grid));
                _seedSprite.Position = new(101, 60);
            }

            grid = File
                .ReadAllLines($@"{Interface._texturesDirectory}\{Textures.OptionsMenuCursor}.sm")
                .Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray())
                .ToArray();
            if (grid.Length > 0 && grid[0].Length > 0)
            {
                _optionsCursorSprite.Texture = new(Engine.CreateImage(grid));
                _optionsCursorSprite.Position = new(134, 52);
            }

            grid = File
                .ReadAllLines($@"{Interface._texturesDirectory}\{Textures.OptionsMenu}.sm")
                .Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray())
                .ToArray();
            if (grid.Length > 0 && grid[0].Length > 0)
            {
                _optionsSprite.Texture = new(Engine.CreateImage(grid));
                _optionsSprite.Position = new(93, 50);
            }

            var options = new List<Text>();
            foreach (var position in Enum.GetValues(typeof(MenuOptions)).Flatten().Skip(1))
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
            _seedText = new()
            {
                Font = font,
                DisplayedString = string.Empty,
                CharacterSize = 200,
                Scale = new(0.01f, 0.01f),
                Position = new(112, 76),
            };
            _frameLimitText = new()
            {
                Font = font,
                CharacterSize = 200,
                Scale = new(0.01f, 0.01f),
                Position = new(96, 53),
            };
            _drawDistanceText = new()
            {
                Font = font,
                CharacterSize = 200,
                Scale = new(0.01f, 0.01f),
                Position = new(96, 57),
            };
            _showWeatherText = new()
            {
                Font = font,
                CharacterSize = 200,
                Scale = new(0.01f, 0.01f),
                Position = new(96, 61),
            };
            _difficultyText = new()
            {
                Font = font,
                CharacterSize = 200,
                Scale = new(0.01f, 0.01f),
                Position = new(96, 65),
            };
            _showHealthText = new()
            {
                Font = font,
                CharacterSize = 200,
                Scale = new(0.01f, 0.01f),
                Position = new(96, 69),
            };
        }

        internal override void Draw(RenderWindow window, GameWorld? world)
        {
            window.Draw(_menuSprite);
            _cursorSprite.Position = new(2, 13 + CursorCurrentPosition * 6);
            window.Draw(_cursorSprite);

            for (var i = 0; i < _menuOptions.Length; i++)
            {
                _menuOptions[i].Position = new(4, i * 6 + 16);
                window.Draw(_menuOptions[i]);
            }

            if (_isSeedCheck)
            {
                window.Draw(_seedSprite);
                _seedText.DisplayedString = Seed.ToString();
                window.Draw(_seedText);
            }

            if (_isOptions)
            {
                window.Draw(_optionsSprite);
                _optionsCursorSprite.Position = new(134, 52 + OptionsCursorCurrentPosition * 4);
                window.Draw(_optionsCursorSprite);
                _frameLimitText.DisplayedString = $"Frames limit: {FramesLimit}";
                window.Draw(_frameLimitText);
                _drawDistanceText.DisplayedString = $"Draw distance: {DrawDistance}";
                window.Draw(_drawDistanceText);
                _showWeatherText.DisplayedString = $"Show weather: {ShowWeather}";
                window.Draw(_showWeatherText);
                _difficultyText.DisplayedString = $"Difficulty: {DifficultyLevel}";
                window.Draw(_difficultyText);
                _showHealthText.DisplayedString = $"Show health: {ShowHealth}";
                window.Draw(_showHealthText);
            }
        }

        internal override bool HandleInput(KeyEventArgs args)
        {
            if (_isSeedCheck)
            {
                if (args.Code == Keyboard.Key.Enter)
                {
                    PerformedAction = MenuOptions.NewGame;
                    _isSeedCheck = false;
                }
                else
                {
                    switch (args.Code)
                    {
                        case Keyboard.Key.Num0:
                            Seed *= 10;
                            LimitSeed();
                            return true;
                        case Keyboard.Key.Num1:
                            Seed = Seed * 10 + 1;
                            LimitSeed();
                            return true;
                        case Keyboard.Key.Num2:
                            Seed = Seed * 10 + 2;
                            LimitSeed();
                            return true;
                        case Keyboard.Key.Num3:
                            Seed = Seed * 10 + 3;
                            LimitSeed();
                            return true;
                        case Keyboard.Key.Num4:
                            Seed = Seed * 10 + 4;
                            LimitSeed();
                            return true;
                        case Keyboard.Key.Num5:
                            Seed = Seed * 10 + 5;
                            LimitSeed();
                            return true;
                        case Keyboard.Key.Num6:
                            Seed = Seed * 10 + 6;
                            LimitSeed();
                            return true;
                        case Keyboard.Key.Num7:
                            Seed = Seed * 10 + 7;
                            LimitSeed();
                            return true;
                        case Keyboard.Key.Num8:
                            Seed = Seed * 10 + 8;
                            LimitSeed();
                            return true;
                        case Keyboard.Key.Num9:
                            Seed = Seed * 10 + 9;
                            LimitSeed();
                            return true;
                        case Keyboard.Key.Backspace:
                            Seed /= 10;
                            LimitSeed();
                            return true;
                        case Keyboard.Key.Escape:
                            _isSeedCheck = false;
                            return true;
                    }
                }

                void LimitSeed()
                {
                    if (Seed > 99999999)
                    {
                        Seed /= 10;
                    }
                }
            }
            else if (_isOptions)
            {
                if (args.Code == Keyboard.Key.Escape)
                {
                    PerformedAction = MenuOptions.None;
                    _isOptions = false;
                }
                else
                {
                    if (args.Code == Keyboard.Key.Up)
                    {
                        OptionsCursorCurrentPosition--;
                    }

                    if (args.Code == Keyboard.Key.Down)
                    {
                        OptionsCursorCurrentPosition++;
                    }

                    switch (args.Code)
                    {
                        case Keyboard.Key.Num0:
                            ChangeOption(0);
                            break;
                        case Keyboard.Key.Num1:
                            ChangeOption(1);
                            break;
                        case Keyboard.Key.Num2:
                            ChangeOption(2);
                            break;
                        case Keyboard.Key.Num3:
                            ChangeOption(3);
                            break;
                        case Keyboard.Key.Num4:
                            ChangeOption(4);
                            break;
                        case Keyboard.Key.Num5:
                            ChangeOption(5);
                            break;
                        case Keyboard.Key.Num6:
                            ChangeOption(6);
                            break;
                        case Keyboard.Key.Num7:
                            ChangeOption(7);
                            break;
                        case Keyboard.Key.Num8:
                            ChangeOption(8);
                            break;
                        case Keyboard.Key.Num9:
                            ChangeOption(9);
                            break;
                        case Keyboard.Key.Backspace:
                            ChangeOption(-1);
                            break;
                        case Keyboard.Key.Enter:
                            ChangeOption(-2);
                            break;
                    }

                    void ChangeOption(sbyte value)
                    {
                        switch (OptionsCursorCurrentPosition)
                        {
                            case 0:
                                var output = (long)FramesLimit;
                                if (value == -1)
                                {
                                    output /= 10;
                                }
                                else
                                {
                                    output = output * 10 + value;
                                }
                                
                                FramesLimit = output > ushort.MaxValue ? ushort.MaxValue : (ushort)output;
                                break;
                            case 1:
                                output = (long)DrawDistance;
                                if (value == -1)
                                {
                                    output /= 10;
                                }
                                else
                                {
                                    output = output * 10 + value;
                                }

                                DrawDistance = output > ushort.MaxValue ? ushort.MaxValue : (ushort)output;
                                break;
                            case 2:
                                if (value == -2)
                                {
                                    ShowWeather = !ShowWeather;
                                }
                                break;
                            case 3:
                                if (value == -2)
                                {
                                    if (DifficultyLevel == Difficulty.Hard)
                                    {
                                        DifficultyLevel = Difficulty.Easy;
                                    }
                                    else
                                    {
                                        DifficultyLevel++;
                                    }
                                }
                                break;
                            case 4:
                                if (value == -2)
                                {
                                    ShowHealth = !ShowHealth;
                                }
                                break;
                        }
                    }
                }

                return true;
            }
            else
            {
                if (args.Code == Keyboard.Key.Up)
                {
                    CursorCurrentPosition--;
                }

                if (args.Code == Keyboard.Key.Down)
                {
                    CursorCurrentPosition++;
                }

                if (args.Code == Keyboard.Key.Enter)
                {
                    switch (_menuOptions[CursorCurrentPosition].DisplayedString)
                    {
                        case "NewGame":
                            _isSeedCheck = true;
                            Reset();
                            return true;
                        case "Resume":
                            PerformedAction = MenuOptions.Resume;
                            Reset();
                            break;
                        case "Options":
                            PerformedAction = MenuOptions.Options;
                            _isOptions = true;
                            return true;
                        case "Exit":
                            PerformedAction = MenuOptions.Exit;
                            break;
                        default:
                            PerformedAction = MenuOptions.None;
                            break;
                    }
                }
            }

            return false;
        }

        internal override void Reset()
        {
            CursorCurrentPosition = 0;
            OptionsCursorCurrentPosition = 0;
        }
    }
}
