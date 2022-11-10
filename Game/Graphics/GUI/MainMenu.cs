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
        private readonly Sprite _cursorSprite = new();
        private readonly Sprite _menuSprite = new();
        private readonly Text[] _menuOptions;
        private sbyte _cursorCurrentPosition = 0;
        public string Seed { get; private set; } = "333";
        public sbyte CursorCurrentPosition
        {
            get => _cursorCurrentPosition;
            set => _cursorCurrentPosition = (sbyte)(value > 20 ? 0 : value < 0 ? 20 : value);
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

            if (args.Code == Keyboard.Key.Enter)
            {
                switch (_menuOptions[CursorCurrentPosition].DisplayedString)
                {
                    case "NewGame":
                        PerformedAction = MenuOptions.NewGame;
                        Reset();
                        break;
                    case "Resume":
                        PerformedAction = MenuOptions.Resume;
                        Reset();
                        break;
                    case "Options":
                        PerformedAction = MenuOptions.Options;
                        break;
                    case "Exit":
                        PerformedAction = MenuOptions.Exit;
                        break;
                    default:
                        PerformedAction = MenuOptions.None;
                        break;
                }
            }

            return false;
        }

        internal override void Reset()
        {
            CursorCurrentPosition = 0;
        }
    }
}
