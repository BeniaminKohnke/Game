using GameAPI;
using SFML.Graphics;
using SFML.System;

namespace Game.GameCore
{
    public class GameWorldController
    {
        private readonly TextureLoader _textureLoader = new();
        private readonly GameObjectComparer _comparer = new();
        private readonly Dictionary<uint, Sprite> _sprites = Parameters.GameWorld.GameObjects
            .Select(go => new KeyValuePair<uint, Sprite>(go.ObjectId, new Sprite { Position = new Vector2f(go.PositionX, go.PositionY) }))
            .ToDictionary(k => k.Key, v => v.Value);

        public GameWorldController() => _textureLoader.LoadTextures();

        public void Draw(RenderWindow window, int drawDistance)
        {
            Parameters.GameWorld.GameObjects.Sort(_comparer);

            foreach (var gameObject in Parameters.GameWorld.GameObjects)
            {
                var difference = Math.Sqrt(Math.Pow(Parameters.GameWorld.Player.PositionX - gameObject.PositionX, 2) + Math.Pow(Parameters.GameWorld.Player.PositionY - gameObject.PositionY, 2));
                if(difference <= drawDistance)
                {
                    gameObject.State = States.NoAction1;
                    _sprites[gameObject.ObjectId].Texture = _textureLoader.Textures[gameObject.TextureType][gameObject.State];
                    _sprites[gameObject.ObjectId].Scale = new Vector2f(10, 10);
                    window.Draw(_sprites[gameObject.ObjectId]);
                }
            }
        }
    }
}
