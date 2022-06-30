using GameAPI;
using SFML.Graphics;
using SFML.System;

namespace Game.GameCore
{
    public class GameWorldController
    {
        private readonly GameWorld _gameWorld;
        private readonly TextureLoader _textureLoader = new();
        private readonly GameObjectComparer _comparer = new();
        private readonly Dictionary<uint, Sprite> _sprites;

        public GameWorldController(GameWorld gameWorld)
        {
            _gameWorld = gameWorld;
            _textureLoader.LoadTextures();
            _sprites = _gameWorld.GameObjects
                .Select(go => new KeyValuePair<uint, Sprite>(go.ObjectId, new Sprite { Position = new Vector2f(go.PositionX, go.PositionY) }))
                .ToDictionary(k => k.Key, v => v.Value);
        }

        public void Draw(RenderWindow window, int drawDistance)
        {
            _gameWorld.GameObjects.Sort(_comparer);

            foreach (var gameObject in _gameWorld.GameObjects)
            {
                var difference = Math.Sqrt(Math.Pow(_gameWorld.Player.PositionX - gameObject.PositionX, 2) + Math.Pow(_gameWorld.Player.PositionY - gameObject.PositionY, 2));
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
