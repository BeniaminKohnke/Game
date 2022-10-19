using Game.GUI;
using GameAPI;
using GameAPI.DSL;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Game
{
    public class Core
    {
        private readonly RenderWindow _window = new(VideoMode.DesktopMode, "Thesis", Styles.Default);
        private readonly Thread t_logicUpdate;
        private readonly View _view;
        private readonly Interface _gameInterface = new();
        private readonly Clock _renderClock = new();
        private readonly Clock _logicClock = new();
        private readonly Time _renderFrameTime = Time.FromSeconds(1.0f / 144.0f);
        private readonly Time _logicUpdateTime = Time.FromSeconds(1.0f / 60.0f);
        private Time _lastRenderTime = Time.Zero;
        private Time _lastLogicUpdateTime = Time.Zero;
        private Keyboard.Key _pressedKey;
        private Engine? _engine;
        private GameWorld? _gameWorld;
        private CodeHandler? _codeHandler;

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

            t_logicUpdate = new Thread(new ThreadStart(() =>
            {
                while (_window.IsOpen)
                {
                    var deltaTime = _logicClock.Restart();
                    _lastLogicUpdateTime += deltaTime;
                    if (_lastLogicUpdateTime >= _logicUpdateTime)
                    {
                        _lastLogicUpdateTime = Time.Zero;
                        if (_gameWorld != null)
                        {
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
                                        _gameWorld.Player.IncreaseItemUses();
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
                                    case Keyboard.Key.F1:
                                        if (_codeHandler != null)
                                        {
                                            _codeHandler.RecompileScripts = true;
                                        }
                                        break;
                                    case Keyboard.Key.F2:
                                        if (_codeHandler != null)
                                        {
                                            _codeHandler.AllowRunningScripts = true;
                                        }
                                        break;
                                    case Keyboard.Key.F3:
                                        if (_codeHandler != null)
                                        {
                                            _codeHandler.AbortScripts();
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
                            _gameWorld.Update(_logicUpdateTime.AsSeconds());
                        }
                    }
                }
            }));
        }

        public void Run()
        {
            _renderClock.Restart();
            _logicClock.Restart();
            t_logicUpdate.Start();
            while (_window.IsOpen)
            {
                _window.DispatchEvents();
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
                        _codeHandler = new(_gameWorld);
                        _engine = new(_gameWorld);
                        break;
                    case MenuOptions.Exit:
                        if (_codeHandler != null && _gameWorld != null)
                        {
                            _codeHandler.IsActive = false;
                            _gameWorld.IsActive = false;
                        }
                        _window.Close();
                        break;
                }

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
    }
}
