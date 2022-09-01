using GameAPI;
using MoreLinq;
using SFML.Graphics;
using SFML.Window;

namespace Game.GUI
{
    public enum Textures
    {
        MainMenu,
        CodeEditor,
        CodeEditorMenu,
        EquipmentWindow,
        HealthBar,
        ItemsBar,
        Cursor,
        ItemsBarCursor,
    }

    public enum Icons
    {
        Pickaxe,
        Axe,
    }

    public sealed class Interface
    {
        internal static string _texturesDirectory = $@"{Directory.GetCurrentDirectory()}\Textures\Interface";
        internal static string _iconsDirectory = $@"{Directory.GetCurrentDirectory()}\Textures\Icons";
        private readonly Dictionary<Textures, Page> _pages;
        private Textures _currentPage = Textures.MainMenu;
        private readonly Textures[] _menuPostions = new[]
        {
            Textures.MainMenu,
            Textures.CodeEditor,
            Textures.EquipmentWindow,
        };
        private readonly Textures[] _inGameInterfacePositions = new[]
        {
            Textures.HealthBar,
            Textures.ItemsBar,
        };
        private sbyte _cursorIndex = 0; 
        private bool _isMenu = true;
        private bool _isInsidePage = false;
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
            get => (_pages.TryGetValue(Textures.MainMenu, out var page) && page is MainMenu mainMenu) ? mainMenu.PerformedAction : MenuOptions.None;
            set
            {
                if (_pages.TryGetValue(Textures.MainMenu, out var page) && page is MainMenu mainMenu)
                {
                    mainMenu.PerformedAction = value;
                }
            }
        }

        public EventHandler<KeyEventArgs> InterfaceHandler { get; }

        public Interface(GameWorld world)
        {
            var font = new Font($@"{Directory.GetCurrentDirectory()}\Font\PressStart2P-Regular.ttf");
            _pages = new()
            {
                [Textures.MainMenu] = new MainMenu(font),
                [Textures.HealthBar] = new HealthBar(font),
                [Textures.EquipmentWindow] = new Equipment(font),
                [Textures.CodeEditor] = new CodeEditor(font, world),
                [Textures.ItemsBar] = new ItemsBar(),
            };

            InterfaceHandler = new((sender, e) =>
            {
                if (IsMenu)
                {
                    if (_isInsidePage)
                    {
                        if (_pages.TryGetValue(_currentPage, out var page) && page.HandleInput(e))
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (e.Code == Keyboard.Key.Right)
                        {
                            _cursorIndex--;
                        }

                        if (e.Code == Keyboard.Key.Left)
                        {
                            _cursorIndex++;
                        }

                        if (_cursorIndex > 2)
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

        public void Draw(RenderWindow window, GameWorld gameWorld)
        {
            if (PerformedAction == MenuOptions.Resume)
            {
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

        public void Release() => _pages.ForEach(p => p.Value.Release());
    }
}