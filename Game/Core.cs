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
        private readonly Engine _engine;
        private readonly GameWorld _gameWorld = new();
        private readonly Clock _renderClock = new();
        private readonly Clock _logicClock = new();
        private Time _lastRenderTime = Time.Zero;
        private Time _renderFrameTime = Time.FromSeconds(1.0f / 144.0f);

        public Core()
        {
            _engine = new(_gameWorld);
            _view = new(new(_gameWorld.Player.Position.x, _gameWorld.Player.Position.y), new(240, 144));
            _window.SetView(_view);

            _window.KeyPressed += _engine.GameInterface.InterfaceHandler;
            _window.KeyPressed += new EventHandler<KeyEventArgs>((sender, e) =>
            {
                if (!_engine.GameInterface.IsMenu)
                {
                    switch (e.Code)
                    {
                        case Keyboard.Key.Up:
                            EnqueueMovement(Directions.Up);
                            break;
                        case Keyboard.Key.Down:
                            EnqueueMovement(Directions.Down);
                            break;
                        case Keyboard.Key.Right:
                            EnqueueMovement(Directions.Right);
                            break;
                        case Keyboard.Key.Left:
                            EnqueueMovement(Directions.Left);
                            break;
                        case Keyboard.Key.Space:
                            _gameWorld.Player.SetItemState(true);
                            break;
                        case Keyboard.Key.Num1:
                            _gameWorld.Player.SetSelctedItem(1);
                            break;
                        case Keyboard.Key.Num2:
                            _gameWorld.Player.SetSelctedItem(2);
                            break;
                        case Keyboard.Key.Num3:
                            _gameWorld.Player.SetSelctedItem(3);
                            break;
                        case Keyboard.Key.Num4:
                            _gameWorld.Player.SetSelctedItem(4);
                            break;
                        case Keyboard.Key.Num5:
                            _gameWorld.Player.SetSelctedItem(5);
                            break;
                        case Keyboard.Key.Num6:
                            _gameWorld.Player.SetSelctedItem(6);
                            break;
                        case Keyboard.Key.Num7:
                            _gameWorld.Player.SetSelctedItem(7);
                            break;
                        case Keyboard.Key.Num8:
                            _gameWorld.Player.SetSelctedItem(8);
                            break;
                        case Keyboard.Key.Num9:
                            _gameWorld.Player.SetSelctedItem(9);
                            break;
                        case Keyboard.Key.Num0:
                            _gameWorld.Player.SetSelctedItem(10);
                            break;

                            void EnqueueMovement(Directions direction)
                            {
                                if (_gameWorld.Player.ObjectParameters.TryGetValue(ObjectsParameters.MovementSpeed, out var value) && value is int movementSpeed)
                                {
                                    for (int i = 0; i < movementSpeed; i++)
                                    {
                                        _gameWorld.Player.EnqueueMovement(direction);
                                    }
                                }
                            }
                    }
                }

                if (e.Code == Keyboard.Key.Tilde)
                {
                    _engine.Release();
                    _gameWorld.IsActive = false;
                    _window.Close();
                }
            });

            _window.KeyReleased += new EventHandler<KeyEventArgs>((sender, e) =>
            {
                if (!_engine.GameInterface.IsMenu)
                {
                    switch (e.Code)
                    {
                        case Keyboard.Key.Up:
                        case Keyboard.Key.Down:
                        case Keyboard.Key.Right:
                        case Keyboard.Key.Left:
                            _gameWorld.Player.IsMoving = false;
                            break;
                        case Keyboard.Key.Space:
                            _gameWorld.Player.SetItemState(false);
                            break;
                    }
                }
            });
        }

        public void Run()
        {
            _renderClock.Restart();
            _logicClock.Restart();
            while (_window.IsOpen)
            {
                var deltaTime = _logicClock.Restart().AsMilliseconds();
                if (_gameWorld.IsActive)
                {
                    _gameWorld.Update(deltaTime);
                }

                _window.DispatchEvents();

                _lastRenderTime += _renderClock.Restart();
                if (_lastRenderTime >= _renderFrameTime)
                {
                    _lastRenderTime = Time.Zero;

                    _window.Clear();
                    _view.Center = _engine.GameInterface.IsMenu ? new(120, 72) : new(_gameWorld.Player.Position.x, _gameWorld.Player.Position.y);
                    _window.SetView(_view);
                    _engine.Draw(_window, 200, _gameWorld);
                    _window.Display();
                }

                if (_engine.GameInterface.PerformedAction == GUI.MenuOptions.Exit)
                {
                    _engine.Release();
                    _gameWorld.IsActive = false;
                    _window.Close();
                }
            }
        }
    }
}
