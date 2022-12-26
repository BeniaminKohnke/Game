using GameAPI;
using SFML.Graphics;
using SFML.Window;

namespace Game.Graphics.GUI
{
    internal sealed class Position : Page
    {
        private readonly Text _positionText;

        internal Position(Font font)
        {
            _positionText = new()
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
                _positionText.DisplayedString = $"[H: {world.Player.Position.x}, V: {world.Player.Position.y}]";
                _positionText.Position = new(world.Player.Position.x - 95, world.Player.Position.y - 71);
                window.Draw(_positionText);
            }
        }

        internal override bool HandleInput(KeyEventArgs args) => throw new Exception("In-game interface cannot handle input");
        internal override void Reset() => throw new Exception("In-game interface cannot be reset");
    }
}
