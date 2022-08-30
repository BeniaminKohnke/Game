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
        Cursor,
    }

    public sealed class Interface
    {
        internal static string TexturesDirectory = $@"{Directory.GetCurrentDirectory()}\Textures\Interface";
        private readonly Dictionary<Textures, Page> _pages;
        private Textures _currentPage = Textures.MainMenu;
        private readonly Textures[] _postions = new[]
        {
            Textures.MainMenu,
            Textures.CodeEditor,
            Textures.EquipmentWindow,
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
                    _currentPage = (_isMenu = value) ? Textures.MainMenu : Textures.HealthBar;
                }
            }
        }

        public EventHandler<KeyEventArgs> InterfaceHandler { get; }

        public Interface(GameWorld world)
        {
            var font = new Font($@"{Directory.GetCurrentDirectory()}\Font\PressStart2P-Regular.ttf");
            _pages = new()
            {
                [Textures.MainMenu] = new MainMenu(),
                [Textures.HealthBar] = new HealthBar(font),
                [Textures.EquipmentWindow] = new Equipment(font),
                [Textures.CodeEditor] = new CodeEditor(font, world),
            };

            InterfaceHandler = new((sender, e) =>
            {
                if (IsMenu)
                {
                    if (_isInsidePage)
                    {
                        if (_pages.TryGetValue(_currentPage, out var page))
                        {
                            page.HandleInput(e);
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
                            _cursorIndex = (sbyte)(_postions.Length - 1);
                        }

                        _currentPage = _postions[_cursorIndex];
                        
                        if (e.Code == Keyboard.Key.Enter)
                        {
                            _isInsidePage = true;
                        }
                    }
                }
            });
        }

        public void Draw(RenderWindow window, GameWorld gameWorld)
        {
            if (_pages.TryGetValue(_currentPage, out var page) || _pages.TryGetValue(Textures.MainMenu, out page))
            {
                page.Draw(window, gameWorld);
            }
        }

        public void Release() => _pages.ForEach(p => p.Value.Release());
    }
}