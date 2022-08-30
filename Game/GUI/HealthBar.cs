using GameAPI;
using SFML.Graphics;
using SFML.Window;

namespace Game.GUI
{
    internal sealed class HealthBar : Page
    {
        private readonly Sprite _helthBarSprite = new();
        private readonly Text _healthValueText;
        private readonly (int x, int y) _barPosition = (87, 53);
        private readonly (int x, int y) _valuePosition = (95, 55);

        internal HealthBar(Font font)
        {
            var grid = File
                .ReadAllLines($@"{Interface.TexturesDirectory}\{Textures.HealthBar}.sm")
                .Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray())
                .ToArray();
            if (grid.Length > 0 && grid[0].Length > 0)
            {
                _helthBarSprite.Texture = new(Engine.CreateImage(grid));
            }

            _healthValueText = new()
            {
                Font = font,
                DisplayedString = "100",
                CharacterSize = 600,
                Scale = new(0.01f, 0.01f),
            };
        }

        internal override void Draw(RenderWindow window, GameWorld world)
        {
            if (!(world.Player.ObjectParameters.TryGetValue(ObjectsParameters.Health, out var value) && value is short health))
            {
                health = -1;
            }

            _healthValueText.DisplayedString = health.ToString();
            _healthValueText.Position = new(_valuePosition.x + world.Player.Position.x, _valuePosition.y + world.Player.Position.y);
            window.Draw(_healthValueText);

            _helthBarSprite.Position = new(_barPosition.x + world.Player.Position.x, _barPosition.y + world.Player.Position.y);
            window.Draw(_helthBarSprite);
        }

        internal override void HandleInput(KeyEventArgs args) => throw new Exception("In-game interface cannot handle input");

        internal override void Release()
        {
            throw new NotImplementedException();
        }

        internal override void Reset() => throw new Exception("In-game interface cannot be reset");
    }
}
