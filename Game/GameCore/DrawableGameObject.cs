using GameAPI;
using SFML.Graphics;
using SFML.System;

namespace Game.GameCore
{
    internal class DrawableGameObject : GameObject
    {
        protected Sprite _objectSprite = new();
        protected Dictionary<States, Texture> _textures = new();

        internal void GetTextures(TextureLoader loader)
        {
            TextureType = TexturesTypes.Player;
            _textures = loader.Textures[TextureType];
        }

        internal virtual void Draw(RenderWindow window)
        {
            State = States.NoAction1;
            _objectSprite.Texture = _textures[State];
            _objectSprite.Scale = new Vector2f(10, 10);
            window.Draw(_objectSprite);
        }
    }
}
