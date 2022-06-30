using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Game.GameCore
{
    public class Core
    {
        private readonly RenderWindow _window;
        private Time _updateTime;
        private Time _renderTime;
        private Time _updateFrameTime;
        private Time _renderFrameTime;

        private readonly GameWorldController _gameWorld = new();

        public Core()
        {
            _window = new RenderWindow(VideoMode.DesktopMode, "Thesis", Styles.Default)
            {
                Size = new Vector2u(800, 600),
            };
        }

        private void Update()
        {

        }

        private void Render()
        {
            _window.Clear();
            _gameWorld.Draw(_window, 1200);
            _window.Display();
        }

        private void HandleKeyboardInput(Keyboard.Key key, bool isPressed)
        {

        }

        public void Run()
        {
            while(_window.IsOpen)
            {
                Render();
            }
        }
    }
}
