using Game.DSL;
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
            Size = new(800, 600),
        };
        private readonly Parameters _parameters = new();
        private readonly GameWorld _gameWorld = new();
        private readonly GameWorldController _gameWorldController = new();
        private readonly CodeHandler _codeHandler = new();
        private Time _lastUpdateTime = Time.Zero;
        private Time _lastRenderTime = Time.Zero;
        private Time _updateFrameTime = Time.FromSeconds(1.0f / 60.0f);
        private Time _renderFrameTime = Time.FromSeconds(1.0f / 60.0f);
        private readonly Clock _updateClock = new();
        private readonly Clock _renderClock = new();

        public Core()
        {
            _window.KeyPressed += new EventHandler<KeyEventArgs>(HandleKeyboardInput);
        }

        private void Update()
        {
            _window.DispatchEvents();
            _gameWorldController.Update(_gameWorld);
            _codeHandler.InvokePlayerScripts(_gameWorld, _parameters);
        }

        private void Render()
        {
            _window.Clear();
            _window.SetView(new(new(_gameWorld.Player.X, _gameWorld.Player.Y), new(480, 480)));
            _gameWorldController.Draw(_window, 1200, _gameWorld);
            _window.Display();
        }

        private void HandleKeyboardInput(object? sender, KeyEventArgs e)
        {
            if(e.Code == Keyboard.Key.Up)
            {
                _gameWorld.Player.Y -= _gameWorld.Player.MovementSpeed;
            }

            if(e.Code == Keyboard.Key.Down)
            {
                _gameWorld.Player.Y += _gameWorld.Player.MovementSpeed;
            }

            if (e.Code == Keyboard.Key.Right)
            {
                _gameWorld.Player.X += _gameWorld.Player.MovementSpeed;
            }

            if (e.Code == Keyboard.Key.Left)
            {
                _gameWorld.Player.X -= _gameWorld.Player.MovementSpeed;
            }
        }

        public void Run()
        {
            _updateClock.Restart();
            _renderClock.Restart();

            while(_window.IsOpen)
            {
                _lastUpdateTime += _updateClock.Restart();
                while (_lastUpdateTime > _updateFrameTime)
                {
                    _lastUpdateTime -= _updateFrameTime;
                    Update();
                }

                _lastRenderTime += _renderClock.Restart();
                if (_lastRenderTime > _renderFrameTime)
                {
                    _lastRenderTime = Time.Zero;
                    Render();
                }
            }
        }
    }
}
