using GameAPI;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Game.Graphics.GUI
{
    internal sealed class FrameCount : Page
    {
        private readonly Clock _clock = new();
        private readonly Time _changeTime = Time.FromSeconds(1f);
        private Time _lastChange = Time.Zero;
        private ushort _framesCount = 1;
        private readonly Text _framesCountText;

        internal FrameCount(Font font)
        {
            _framesCountText = new()
            {
                Font = font,
                CharacterSize = 200,
                Scale = new(0.01f, 0.01f),
            };
        }

        internal override void Draw(RenderWindow window, GameWorld? world)
        {
            if (world != null)
            {
                _framesCount++;
                _lastChange += _clock.Restart();
                if (_lastChange >= _changeTime)
                {
                    _lastChange = Time.Zero;
                    _framesCountText.DisplayedString = $"[FPS: {_framesCount}]";
                    _framesCount = 1;
                }

                _framesCountText.Position = new(world.Player.Position.x - 119, world.Player.Position.y - 71);
                window.Draw(_framesCountText);
            }
        }

        internal override bool HandleInput(KeyEventArgs args) => throw new Exception("In-game interface cannot handle input");
        internal override void Reset() => throw new Exception("In-game interface cannot be reset");
    }
}
