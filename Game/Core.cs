using GameAPI.DSL;
using GameAPI;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Game
{
    public class Core
    {
        private readonly RenderWindow _window = new(VideoMode.FullscreenModes[1], string.Empty, Styles.Fullscreen);
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
                            _gameWorld.Player.SetSelctedItem(false);
                            break;
                        case Keyboard.Key.Num2:
                            _gameWorld.Player.SetSelctedItem(true);
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

                if (e.Code == Keyboard.Key.Escape)
                {
                    _engine.GameInterface.IsMenu = !_engine.GameInterface.IsMenu;
                }

                if (e.Code == Keyboard.Key.Tilde)
                {
                    _engine.Release();
                    _gameWorld.IsActive = false;
                    _window.Close();
                }

                //if (e.Code == Keyboard.Key.F1)
                //{
                //    _engine.GameInterface.CodeEditor.IsActive = !_engine.GameInterface.CodeEditor.IsActive;
                //}
                //
                //if (_engine.GameInterface.CodeEditor.IsActive)
                //{
                //    if (e.Shift)
                //    {
                //        if (_scriptsCharsNormal.ContainsKey(e.Code))
                //        {
                //            _engine.GameInterface.CodeEditor.EditedText += _scriptsCharsShift[e.Code];
                //        }
                //    }
                //    else
                //    {
                //        if (_scriptsCharsNormal.ContainsKey(e.Code))
                //        {
                //            _engine.GameInterface.CodeEditor.EditedText += _scriptsCharsNormal[e.Code];
                //        }
                //    }
                //}

                //if (_engine.GameInterface.Equipment.IsActive)
                //{
                //    if (e.Code == Keyboard.Key.Up)
                //    {
                //        _engine.GameInterface.Equipment.CursorCurrentPosition--;
                //    }
                //
                //    if (e.Code == Keyboard.Key.Down)
                //    {
                //        _engine.GameInterface.Equipment.CursorCurrentPosition++;
                //    }
                //}
            });

            _window.KeyReleased += new EventHandler<KeyEventArgs>((sender, e) =>
            {
                //if (!_engine.GameInterface.CodeEditor.IsActive)
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
                _gameWorld.DeltaTime = _logicClock.Restart().AsMicroseconds() / 100000d;
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
                    _window.SetTitle($"X:{_gameWorld.Player.Position.x} Y:{_gameWorld.Player.Position.y}");
                }
            }
        }
    }
}
