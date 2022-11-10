using Game.Graphics;
using Game.Graphics.GUI;
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
        private GameWorld? _gameWorld;
        private Engine? _engine;
        private readonly ScriptHandler _handler = new();

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
                HandleInterface();

                _lastRenderTime += _renderClock.Restart();
                if (_lastRenderTime >= _renderFrameTime)
                {
                    _lastRenderTime = Time.Zero;
                    _window.Clear();
                    _view.Center = _gameInterface.IsMenu || _gameWorld == null ? new(120, 72) : new(_gameWorld.Player.Position.x, _gameWorld.Player.Position.y);
                    _window.SetView(_view);
                    _engine?.Draw(_window, 200, _gameWorld);
                    _gameInterface.Draw(_window, _gameWorld);
                    _window.Display();
                }
            }
        }

        public void HandleInterface()
        {
            switch (_gameInterface.PerformedAction)
            {
                case MenuOptions.Resume:
                    if (_gameWorld == null)
                    {
                        _gameInterface.PerformedAction = MenuOptions.None;
                    }
                    break;
                case MenuOptions.NewGame:
                    _gameWorld?.Destroy();
                    _gameWorld = new(_gameInterface.Seed);
                    _engine = new(_gameWorld);
                    break;
                case MenuOptions.Exit:
                    if (_gameWorld != null)
                    {
                        _handler.IsActive = false;
                        _gameWorld.IsActive = false;
                    }
                    _window.Close();
                    break;
            }
        }

        public void HandleLogic()
        {
            _window.DispatchEvents();
            var deltaTime = _logicClock.Restart().AsSeconds();
            if (_gameWorld != null)
            {
                _handler.Update(_gameWorld, deltaTime);
                _gameWorld.Update(deltaTime);
                switch (_pressedKey)
                {
                    case Keyboard.Key.Up:
                        EnqueueMovement(Directions.North);
                        break;
                    case Keyboard.Key.Down:
                        EnqueueMovement(Directions.South);
                        break;
                    case Keyboard.Key.Right:
                        EnqueueMovement(Directions.East);
                        break;
                    case Keyboard.Key.Left:
                        EnqueueMovement(Directions.West);
                        break;
                    case Keyboard.Key.Space:
                        _gameWorld.Player.IncreaseItemUses();
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
                            if (_gameWorld.Player.ObjectParameters.TryGetValue(ObjectsParameters.MovementSpeed, out var value) && value is int movementSpeed)
                            {
                                for (var i = 0; i < movementSpeed; i++)
                                {
                                    _gameWorld.Player.EnqueueMovement(direction);
                                    _gameWorld.Player.IsMoving = true;
                                }
                            }
                        }
                }
            }
        }
    }
}
