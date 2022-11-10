using Aardvark.Base;
using SFML.Graphics;
using SFML.System;

namespace Game.Graphic
{
    internal class Filter
    {
        private enum FilterType : byte
        {
            None = 0,
            Rain = 1,
            Snow = 2,
        }

        private (int x, int y) _waypoint = (0,0);
        private readonly int _scale = 2;
        private readonly ushort _generationDistance = 500;
        private readonly Clock _clock = new();
        private readonly Time _drawTime = Time.FromSeconds(1.0f / 4f);
        private Time _lastDraw = Time.Zero;
        private byte _frame = 0;
        private FilterType _type = FilterType.None;
        private readonly Dictionary<FilterType, Sprite[]> _textures = new();
        private readonly Dictionary<FilterType, (byte offset, byte count, byte[][] frame)> _patterns = new()
        {
            [FilterType.Rain] = (200, 3, new[]
            {
                new [] { (byte)3 },
                new [] { (byte)3 },
            }),
            [FilterType.Snow] = (200, 5, new[]
            {
                new [] { (byte)0, (byte)3, (byte)0 },
                new [] { (byte)3, (byte)3, (byte)3 },
                new [] { (byte)0, (byte)3, (byte)0 },
            }),
        };
        private readonly (sbyte x, sbyte y)[] _directions =
        {
            (-1, 1),  (0, 1),  (1, 1),
            (-1, 0),  (0, 0),  (1, 0),
            (-1, -1), (0, -1), (1, -1),
        };

        internal Filter()
        {
            foreach (var pattern in _patterns)
            {
                _textures[pattern.Key] = Enumerable.Range(0, pattern.Value.count).Select(v => CreateTexture(pattern.Value.frame, pattern.Value.offset)).ToArray();
            }

            Sprite CreateTexture(byte[][] frame, byte offset)
            {
                var range = _generationDistance * _scale;
                var random = new Random();
                var grid = Enumerable.Range(0, range).Select(c => Enumerable.Range(0, range).Select(r => (byte)0).ToArray()).ToArray();
                for (var i = 0; i < range; i++)
                {
                    for (var j = 0; j < range; j++)
                    {
                        if (random.Next(0, offset) == 0)
                        {
                            for (var y = 0; y < frame.Length; y++)
                            {
                                for (var x = 0; x < frame[y].Length; x++)
                                {
                                    var a = i + y;
                                    var b = j + x;
                                    if (a < grid.Length && b < grid.Length && frame[y][x] != 0)
                                    {
                                        grid[a][b] = frame[y][x];
                                    }
                                }
                            }
                        }
                    }
                }

                return new Sprite(new Texture(Engine.CreateImage(grid.Select(l => l.ToArray()).ToArray()))) { Scale = new(1f / _scale, 1f / _scale) };
            }
        }

        internal void Draw(RenderWindow window, int x, int y)
        {
            if (_type != FilterType.None)
            {
                HandleWaypoint(x, y);

                var frames = _textures[_type];

                foreach (var (a, b) in _directions)
                {
                    frames[_frame].Position = new(_waypoint.x + (_generationDistance * a), _waypoint.y + (_generationDistance * b));
                    window.Draw(frames[_frame]);
                }

                _lastDraw += _clock.Restart();
                if (_drawTime <= _lastDraw)
                {
                    _lastDraw = Time.Zero;
                    if (++_frame == frames.Length)
                    {
                        _frame = 0;
                    }
                }
            }
        }

        internal void SetNextFilter()
        {
            var types = Enum.GetValues(typeof(FilterType)).ToArrayOfT<FilterType>();
            _type = types[new Random().Next(0, types.Length)];
            _frame = 0;
        }

        private void HandleWaypoint(int x, int y)
        {
            var directionX = 0;
            var directionY = 0;

            if (_waypoint.x + _generationDistance / 2 < x)
            {
                directionX = 1;
            }

            if (_waypoint.x - _generationDistance / 2 > x)
            {
                directionX = -1;
            }

            if (_waypoint.y + _generationDistance / 2 < y)
            {
                directionY = 1;
            }

            if (_waypoint.y - _generationDistance / 2 > y)
            {
                directionY = -1;
            }

            if (directionX != 0 || directionY != 0)
            {
                _waypoint = (_waypoint.x + (_generationDistance * directionX / 2), _waypoint.y + (_generationDistance * directionY / 2));
            }
        }
    }
}
