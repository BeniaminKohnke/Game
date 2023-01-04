using Aardvark.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFML.Graphics;
using SFML.Window;
using System;

namespace GameTest
{
    [TestClass]
    public sealed class NoiseTest
    {
        private const ushort SIZE = 50;
        private readonly RenderWindow _window = new(VideoMode.DesktopMode, "Noise", Styles.Default)
        {
            Size = new(SIZE * 10, SIZE * 10),
        };

        public NoiseTest()
        {
            _window.SetView(new(new(0, 0), new(SIZE, SIZE)));
            _window.Closed += new((o, args) => _window.Close());
        }

        [TestMethod]
        public void GenerateRandom()
        {
            var random = new Random();
            _window.SetTitle(typeof(Random).FullName);
            Draw(GenerateSprite((x, y) => random.Next(-100, 100) / 100f));
        }

        [TestMethod]
        public void GeneratePerlin()
        {
            _window.SetTitle(typeof(PerlinNoise).FullName);
            Draw(GenerateSprite(new PerlinNoise().SmoothNoise));
        }

        private static Sprite GenerateSprite(Func<int, int, float> generator)
        {
            var image = new Image(SIZE, SIZE);
            for (var i = 0; i < SIZE; i++)
            {
                for (var j = 0; j < SIZE; j++)
                {
                    var value = (byte)(127 + 127 * generator(i, j));
                    image.SetPixel((uint)i, (uint)j, new(value, value, value));
                }
            }

            return new Sprite(new Texture(image))
            {
                Position = new(-SIZE / 2, -SIZE / 2),
            };
        }

        private void Draw(Sprite sprite)
        {
            while (_window.IsOpen)
            {
                _window.DispatchEvents();
                _window.Clear();
                _window.Draw(sprite);
                _window.Display();
            }
        }
    }
}
