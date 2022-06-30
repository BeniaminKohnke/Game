using GameAPI;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Game.GameCore
{
    public class Core
    {
        private readonly RenderWindow _window = new(VideoMode.DesktopMode, "Thesis", Styles.Default)
        {
            Size = new Vector2u(800, 600),
        };
        private readonly Parameters _parameters = new();
        private readonly GameWorld _gameWorld = new();
        private readonly GameWorldController _gameWorldController;
        private Time _updateTime;
        private Time _renderTime;
        private Time _updateFrameTime;
        private Time _renderFrameTime;


        public Core()
        {
            _gameWorldController = new(_gameWorld);
        }

        private void Update()
        {

        }

        private void Render()
        {
            _window.Clear();
            _gameWorldController.Draw(_window, 1200);
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
