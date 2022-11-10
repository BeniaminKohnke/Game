using GameAPI;
using SFML.Graphics;
using SFML.Window;

namespace Game.Graphic.GUI
{
    internal sealed class HealthBar : Page
    {
        private readonly Sprite _healthBarSprite = new();
        private readonly Text _healthValueText;

        internal HealthBar(Font font)
        {
            var grid = File
                .ReadAllLines($@"{Interface._texturesDirectory}\{Textures.HealthBar}.sm")
                .Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray())
                .ToArray();
            if (grid.Length > 0 && grid[0].Length > 0)
            {
                _healthBarSprite.Texture = new(Engine.CreateImage(grid));
            }

            _healthValueText = new()
            {
                Font = font,
                DisplayedString = "100",
                CharacterSize = 600,
                Scale = new(0.01f, 0.01f),
            };
        }

        internal override void Draw(RenderWindow window, GameWorld? world)
        {
            if (world != null)
            {
                if (!(world.Player.ObjectParameters.TryGetValue(ObjectsParameters.Health, out var value) && value is short health))
                {
                    health = -1;
                }

                _healthValueText.DisplayedString = health.ToString();
                _healthValueText.Position = new(95 + world.Player.Position.x, 60 + world.Player.Position.y);
                window.Draw(_healthValueText);

                _healthBarSprite.Position = new(87 + world.Player.Position.x, 58 + world.Player.Position.y);
                window.Draw(_healthBarSprite);
            }
        }

        internal override bool HandleInput(KeyEventArgs args) => throw new Exception("In-game interface cannot handle input");
        internal override void Reset() => throw new Exception("In-game interface cannot be reset");
    }
}
