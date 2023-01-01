using GameAPI;
using SFML.Graphics;
using SFML.Window;

namespace Game.Graphics.GUI
{
    public enum Difficulty : byte
    {
        Easy = 1,
        Medium = 2,
        Hard = 3,
    }

    public enum Textures : byte
    {
        MainMenu,
        CodeEditor,
        CodeEditorMenu,
        EquipmentWindow,
        HealthBar,
        ItemsBar,
        Cursor,
        ItemsBarCursor,
        ScriptApproved,
        ScriptRejected,
        ScriptNotVerified,
        FramesCount,
        Seed,
        OptionsMenu,
        OptionsMenuCursor,
        Position,
        CraftingMenu,
        Help,
    }

    public enum Icons : byte
    {
        Pickaxe,
        Axe,
        Fruit,
        Potion,
        Sword,
        Bow,
        Arrow,
    }

    public sealed class Interface
    {
        internal static string _texturesDirectory = $@"{Directory.GetCurrentDirectory()}\Textures\Interface";
        internal static string _iconsDirectory = $@"{Directory.GetCurrentDirectory()}\Textures\Icons";
        private readonly Dictionary<Textures, Page> _pages;
        private Textures _currentPage = Textures.MainMenu;
        private readonly Textures[] _menuPostions =
        {
            Textures.MainMenu,
            Textures.EquipmentWindow,
            Textures.CodeEditor,
            Textures.CraftingMenu,
            Textures.Help,
        };
        private readonly Textures[] _inGameInterfacePositions =
        {
            Textures.HealthBar,
            Textures.ItemsBar,
            Textures.FramesCount,
            Textures.Position,
        };
        private sbyte _cursorIndex = 0;
        private bool _isMenu = true;
        private bool _isInsidePage = false;
        public EventHandler<KeyEventArgs> InterfaceHandler { get; }
        public int Seed => _pages.TryGetValue(Textures.MainMenu, out var page) && page is MainMenu mainMenu ? mainMenu.Seed : 0;
        public ushort FramesLimit => _pages.TryGetValue(Textures.MainMenu, out var page) && page is MainMenu mainMenu ? mainMenu.FramesLimit : (ushort)0;
        public bool ShowWeather => _pages.TryGetValue(Textures.MainMenu, out var page) && page is MainMenu mainMenu && mainMenu.ShowWeather;
        public bool ShowHealth => _pages.TryGetValue(Textures.MainMenu, out var page) && page is MainMenu mainMenu && mainMenu.ShowHealth;
        public ushort DrawDistance => _pages.TryGetValue(Textures.MainMenu, out var page) && page is MainMenu mainMenu ? mainMenu.DrawDistance : (ushort)0;
        public byte DifficultyLevel => (byte)(_pages.TryGetValue(Textures.MainMenu, out var page) && page is MainMenu mainMenu ? mainMenu.DifficultyLevel : Difficulty.Medium);
        public Items ItemName
        {
            get
            {
                if (_pages.TryGetValue(Textures.CraftingMenu, out var page) && page is CraftingMenu menu)
                {
                    var item = menu.ItemName;
                    menu.ItemName = Items.None;
                    return item;
                }

                return Items.None;
            }
        }
        public bool IsMenu
        {
            get => _isMenu;
            set
            {
                if (_isInsidePage)
                {
                    _isInsidePage = false;
                    if (_pages.TryGetValue(_currentPage, out var page))
                    {
                        page.Reset();
                    }
                }
                else
                {
                    _currentPage = (_isMenu = value) ? Textures.MainMenu : Textures.Cursor;
                }
            }
        }
        public MenuOptions PerformedAction
        {
            get => _pages.TryGetValue(Textures.MainMenu, out var page) && page is MainMenu mainMenu ? mainMenu.PerformedAction : MenuOptions.None;
            set
            {
                if (_pages.TryGetValue(Textures.MainMenu, out var page) && page is MainMenu mainMenu)
                {
                    mainMenu.PerformedAction = value;
                }
            }
        }

        public Interface()
        {
            var font = new Font($@"{Directory.GetCurrentDirectory()}\Font\PressStart2P-Regular.ttf");
            _pages = new()
            {
                [Textures.MainMenu] = new MainMenu(font),
                [Textures.HealthBar] = new HealthBar(font),
                [Textures.EquipmentWindow] = new Equipment(font),
                [Textures.CodeEditor] = new ScriptEditor(font),
                [Textures.CraftingMenu] = new CraftingMenu(font),
                [Textures.Help] = new Help(font),
                [Textures.ItemsBar] = new ItemsBar(),
                [Textures.FramesCount] = new FrameCount(font),
                [Textures.Position] = new Position(font),
            };

            InterfaceHandler = new((sender, e) =>
            {
                if (IsMenu)
                {
                    if (_isInsidePage)
                    {
                        if (_pages.TryGetValue(_currentPage, out var page) && page.HandleInput(e))
                        {
                            _cursorIndex = 0;
                            return;
                        }
                    }
                    else
                    {
                        if (e.Code == Keyboard.Key.Right)
                        {
                            _cursorIndex++;
                        }

                        if (e.Code == Keyboard.Key.Left)
                        {
                            _cursorIndex--;
                        }

                        if (_cursorIndex >= _menuPostions.Length)
                        {
                            _cursorIndex = 0;
                        }

                        if (_cursorIndex < 0)
                        {
                            _cursorIndex = (sbyte)(_menuPostions.Length - 1);
                        }

                        _currentPage = _menuPostions[_cursorIndex];

                        if (!_isInsidePage && e.Code == Keyboard.Key.Down)
                        {
                            _isInsidePage = true;
                        }

                        if (e.Code == Keyboard.Key.Enter)
                        {
                            _isInsidePage = true;
                        }
                    }
                }

                if (e.Code == Keyboard.Key.Escape)
                {
                    IsMenu = !IsMenu;
                }
            });
        }

        public void Draw(RenderWindow window, GameWorld? gameWorld)
        {
            if (PerformedAction == MenuOptions.Resume || PerformedAction == MenuOptions.NewGame)
            {
                _cursorIndex = 0;
                _isInsidePage = false;
                IsMenu = false;
                PerformedAction = MenuOptions.None;
            }

            if (_currentPage == Textures.Cursor)
            {
                foreach (var position in _inGameInterfacePositions)
                {
                    if (_pages.TryGetValue(position, out var page))
                    {
                        page.Draw(window, gameWorld);
                    }
                }
            }
            else
            {
                if (_pages.TryGetValue(_currentPage, out var page))
                {
                    page.Draw(window, gameWorld);
                }
                else if (_pages.TryGetValue(Textures.MainMenu, out page))
                {
                    _currentPage = Textures.MainMenu;
                    page.Draw(window, gameWorld);
                }
            }
        }
    }
}