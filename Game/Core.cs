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
        private readonly GUI _gui = new();
        private readonly Clock _renderClock = new();
        private Time _lastRenderTime = Time.Zero;
        private Time _renderFrameTime = Time.FromSeconds(1.0f / 144.0f);
        private readonly Font _font;

        private readonly Dictionary<Keyboard.Key, char> _scriptsCharsNormal = new()
        {
            { Keyboard.Key.A, 'a' },
            { Keyboard.Key.B, 'b' },
            { Keyboard.Key.C, 'c' },
            { Keyboard.Key.D, 'd' },
            { Keyboard.Key.E, 'e' },
            { Keyboard.Key.F, 'f' },
            { Keyboard.Key.G, 'g' },
            { Keyboard.Key.H, 'h' },
            { Keyboard.Key.I, 'i' },
            { Keyboard.Key.J, 'j' },
            { Keyboard.Key.K, 'k' },
            { Keyboard.Key.L, 'l' },
            { Keyboard.Key.M, 'm' },
            { Keyboard.Key.N, 'n' },
            { Keyboard.Key.O, 'o' },
            { Keyboard.Key.P, 'p' },
            { Keyboard.Key.Q, 'q' },
            { Keyboard.Key.R, 'r' },
            { Keyboard.Key.S, 's' },
            { Keyboard.Key.T, 't' },
            { Keyboard.Key.U, 'u' },
            { Keyboard.Key.V, 'v' },
            { Keyboard.Key.W, 'w' },
            { Keyboard.Key.X, 'x' },
            { Keyboard.Key.Y, 'y' },
            { Keyboard.Key.Z, 'z' },
            { Keyboard.Key.Space, ' ' },
            { Keyboard.Key.Enter, '\n' },
            { Keyboard.Key.Tab, '\t' },
        };

        private readonly Dictionary<Keyboard.Key, char> _scriptsCharsShift = new()
        {

        };

        //debug
        private readonly Text _text;
        public Core()
        {
            _font = new Font($@"{Directory.GetCurrentDirectory()}\Font\PressStart2P-Regular.ttf");

            _text = new()
            {
                Font = _font,
                DisplayedString = string.Empty,
                CharacterSize = 100,
                Scale = new(0.01f, 0.01f),
            };

            _text.DisplayedString =
@"
using GameAPI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GameAPI.DSL
{
	public class MovePlayerScript : PlayerScript
	{
		public MovePlayerScript()
		{
		}

		protected override void Do(GameWorld gameWorld, 
            ConcurrentDictionary<string, 
            (Types, object)> parameters)
		{
			gameWorld.Player.EnqueueMovement(
                (Directions)new Random().Next(0, 5));
		}
	}
}

";

            _codeHandler = new(_gameWorld);
            _engine = new(_gameWorld);
            _view = new(new(_gameWorld.Player.Position.x, _gameWorld.Player.Position.y), new(240, 144));
            _window.SetView(_view);

            _window.KeyPressed += new EventHandler<KeyEventArgs>((sender, e) =>
            {
                if(!_gui.States[Controls.CodeEditor])
                {
                    switch(e.Code)
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

                if(e.Code == Keyboard.Key.F5)
                {
                    _codeHandler.CreateScript(string.Empty);
                }

                if(e.Code == Keyboard.Key.F6)
                {
                    _codeHandler.AllowRunningScripts = true;
                }

                if(e.Code == Keyboard.Key.F7)
                {
                    _codeHandler.AbortScripts();
                }

                if(e.Code == Keyboard.Key.Escape)
                {
                    _gameWorld.IsActive = false;
                    _codeHandler.IsActive = false;
                    _window.Close();
                }

                if(e.Code == Keyboard.Key.F1)
                {
                    _gui.States[Controls.CodeEditor] = !_gui.States[Controls.CodeEditor];
                }

                if(_gui.States[Controls.CodeEditor])
                {
                    if(e.Shift)
                    {

                    }
                    else
                    {
                        if(_scriptsCharsNormal.ContainsKey(e.Code))
                        {
                            _text.DisplayedString += _scriptsCharsNormal[e.Code];
                        }
                    }
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
                    _gui.Draw(_window, _gameWorld.Player.Position.x, _gameWorld.Player.Position.y);
                    _text.Position = new(_gameWorld.Player.Position.x - 117, _gameWorld.Player.Position.y - 68);
                    if (_gui.States[Controls.CodeEditor])
                    {
                        _window.Draw(_text);
                    }
                    _window.Display();
                    _window.SetTitle($"X:{_gameWorld.Player.Position.x} Y:{_gameWorld.Player.Position.y}");
                }
            }
        }
    }
}
