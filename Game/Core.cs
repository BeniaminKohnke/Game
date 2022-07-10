using GameAPI.DSL;
using GameAPI;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Game
{
    public class Core
    { 
        private readonly RenderWindow _window = new(VideoMode.FullscreenModes[1], string.Empty, Styles.Default);
        private readonly View _view;
        private readonly Engine _engine;
        private readonly GameWorld _gameWorld = new();
        private readonly CodeHandler _codeHandler;
        private readonly Clock _renderClock = new();
        private Time _lastRenderTime = Time.Zero;
        private Time _renderFrameTime = Time.FromSeconds(1.0f / 144.0f);

        public Core()
        {
            _codeHandler = new(_gameWorld);
            _engine = new(_gameWorld);
            _view = new(new(_gameWorld.Player.Position.x, _gameWorld.Player.Position.y), new(128, 128));
            _window.SetView(_view);
            _window.KeyPressed += new EventHandler<KeyEventArgs>((sender, e) =>
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

                if(e.Code == Keyboard.Key.F5)
                {
                    _codeHandler.CreateScript(string.Empty);
                }

                if (e.Code == Keyboard.Key.F6)
                {
                    _codeHandler.AllowRunningScripts = true;
                }

                if (e.Code == Keyboard.Key.F7)
                {
                    _codeHandler.AbortScripts();
                }

                if (e.Code == Keyboard.Key.Escape)
                {
                    _gameWorld.IsActive = false;
                    _codeHandler.IsActive = false;
                    _window.Close();
                }
            });
        }

        public void Run()
        {
            _renderClock.Restart();
            while (_window.IsOpen)
            {
                _window.DispatchEvents();

                _lastRenderTime += _renderClock.Restart();
                if (_lastRenderTime >= _renderFrameTime)
                {
                    _lastRenderTime = Time.Zero;

                    _window.Clear();
                    _view.Center = new(_gameWorld.Player.Position.x, _gameWorld.Player.Position.y);
                    _window.SetView(_view);
                    _engine.Draw(_window, 200, _gameWorld);
                    _window.Display();
                    _window.SetTitle($"X:{_gameWorld.Player.Position.x} Y:{_gameWorld.Player.Position.y}");
                }
            }
        }
    }
}
