using GameAPI.DSL;
using GameAPI;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Game
{
    public class Core
    {
        private readonly RenderWindow _window = new(VideoMode.FullscreenModes[1], "Thesis", Styles.Fullscreen);
        private readonly View _view;
        private readonly GameWorld _gameWorld = new();
        private readonly Engine _gameWorldController;
        private readonly CodeHandler _codeHandler = new();
        private Time _lastUpdateTime = Time.Zero;
        private Time _lastRenderTime = Time.Zero;
        private Time _updateFrameTime = Time.FromSeconds(1.0f / 60.0f);
        private Time _renderFrameTime = Time.FromSeconds(1.0f / 60.0f);
        private readonly Clock _updateClock = new();
        private readonly Clock _renderClock = new();

        public Core()
        {
            _gameWorldController = new(_gameWorld);
            _window.KeyPressed += new EventHandler<KeyEventArgs>(HandleKeyboardInput);
            _view = new(new(_gameWorld.Player.X, _gameWorld.Player.Y), new(128, 128));
            _window.SetView(_view);
        }

        private void Update()
        {
            _window.DispatchEvents();
            _gameWorld.Update();
            _codeHandler.InvokePlayerScripts(_gameWorld);
        }

        private void Render()
        {
            _window.Clear();
            _view.Center = new(_gameWorld.Player.X, _gameWorld.Player.Y);
            _window.SetView(_view);
            _gameWorldController.Draw(_window, 300, _gameWorld);
            _window.Display();
        }

        private void HandleKeyboardInput(object? sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Up)
            {
                _gameWorld.Player.EnqueueMovement(Directions.Up);
            }

            if (e.Code == Keyboard.Key.Down)
            {
                _gameWorld.Player.EnqueueMovement(Directions.Down);
            }

            if (e.Code == Keyboard.Key.Right)
            {
                _gameWorld.Player.EnqueueMovement(Directions.Right);
            }

            if (e.Code == Keyboard.Key.Left)
            {
                _gameWorld.Player.EnqueueMovement(Directions.Left);
            }

            if(e.Code == Keyboard.Key.Escape)
            {
                _window.Close();
            }
        }

        public void Run()
        {
            _updateClock.Restart();
            _renderClock.Restart();

            while (_window.IsOpen)
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
