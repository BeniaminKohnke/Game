using Game.Graphic;
using Game.Graphic.GUI;
using GameAPI;
using GameAPI.DSL;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Game
{
    public sealed class Core
    {
        private readonly RenderWindow _window = new(VideoMode.DesktopMode, "Thesis", Styles.Default);
        private readonly View _view;
        private readonly Interface _gameInterface = new();
        private readonly Clock _renderClock = new();
        private readonly Clock _logicClock = new();
        private readonly Time _renderFrameTime = Time.FromSeconds(1.0f / 144.0f);
        private Time _lastRenderTime = Time.Zero;
        private Keyboard.Key _pressedKey;
        private Engine? _engine;
        private readonly WorldHandler _handler = new();

        public Core()
        {
            _view = new(new(120, 72), new(240, 144));
            _window.SetView(_view);

            _window.KeyPressed += _gameInterface.InterfaceHandler;
            _window.KeyPressed += new EventHandler<KeyEventArgs>((sender, e) =>
            {
                if (!_gameInterface.IsMenu)
                {
                    _pressedKey = e.Code;
                }
            });

            _window.KeyReleased += new EventHandler<KeyEventArgs>((sender, e) =>
            {
                _pressedKey = Keyboard.Key.Unknown;
            });
        }

        public void Run()
        {
            _renderClock.Restart();
            _logicClock.Restart();
            while (_window.IsOpen)
            {
                HandleLogic();
                switch (_gameInterface.PerformedAction)
                {
                    case MenuOptions.Resume:
                        if (_handler.World == null)
                        {
                            _gameInterface.PerformedAction = MenuOptions.None;
                        }
                        break;
                    case MenuOptions.NewGame:
                        _handler.World?.Destroy();
                        _handler.World = new(_gameInterface.Seed);
                        _engine = new(_handler.World);
                        break;
                    case MenuOptions.Exit:
                        if (_handler.World != null)
                        {
                            _handler.IsActive = false;
                            _handler.World.IsActive = false;
                        }
                        _window.Close();
                        break;
                }

                _lastRenderTime += _renderClock.Restart();
                if (_lastRenderTime >= _renderFrameTime)
                {
                    _lastRenderTime = Time.Zero;
                    _window.Clear();
                    _view.Center = _gameInterface.IsMenu || _handler.World == null ? new(120, 72) : new(_handler.World.Player.Position.x, _handler.World.Player.Position.y);
                    _window.SetView(_view);
                    _engine?.Draw(_window, 200, _handler.World);
                    _gameInterface.Draw(_window, _handler.World);
                    _window.Display();
                }
            }
        }

        public void HandleLogic()
        {
            _window.DispatchEvents();
            _handler.Update(_logicClock.Restart().AsSeconds());
            if (_handler.World != null)
            {
                switch (_pressedKey)
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
                        _handler.World.Player.IncreaseItemUses();
                        break;
                    case Keyboard.Key.Num1:
                        _handler.World.Player.SetSelctedItem(1);
                        break;
                    case Keyboard.Key.Num2:
                        _handler.World.Player.SetSelctedItem(2);
                        break;
                    case Keyboard.Key.Num3:
                        _handler.World.Player.SetSelctedItem(3);
                        break;
                    case Keyboard.Key.Num4:
                        _handler.World.Player.SetSelctedItem(4);
                        break;
                    case Keyboard.Key.Num5:
                        _handler.World.Player.SetSelctedItem(5);
                        break;
                    case Keyboard.Key.Num6:
                        _handler.World.Player.SetSelctedItem(6);
                        break;
                    case Keyboard.Key.Num7:
                        _handler.World.Player.SetSelctedItem(7);
                        break;
                    case Keyboard.Key.Num8:
                        _handler.World.Player.SetSelctedItem(8);
                        break;
                    case Keyboard.Key.Num9:
                        _handler.World.Player.SetSelctedItem(9);
                        break;
                    case Keyboard.Key.Num0:
                        _handler.World.Player.SetSelctedItem(10);
                        break;
                    case Keyboard.Key.F1:
                        if (_handler != null)
                        {
                            _handler.ReloadScripts = true;
                        }
                        break;
                    case Keyboard.Key.F2:
                        if (_handler != null)
                        {
                            _handler.RunScripts = true;
                        }
                        break;
                    case Keyboard.Key.F3:
                        if (_handler != null)
                        {
                            _handler.RunScripts = false;
                        }
                        break;

                        void EnqueueMovement(Directions direction)
                        {
                            if (_handler.World.Player.ObjectParameters.TryGetValue(ObjectsParameters.MovementSpeed, out var value) && value is int movementSpeed)
                            {
                                for (var i = 0; i < movementSpeed; i++)
                                {
                                    _handler.World.Player.EnqueueMovement(direction);
                                    _handler.World.Player.IsMoving = true;
                                }
                            }
                        }
                }
            }
        }
    }
}
