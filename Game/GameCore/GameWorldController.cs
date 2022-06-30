using GameAPI;
using SFML.Graphics;
using SFML.System;

namespace Game.GameCore
{
    public class GameWorldController
    {
        private readonly TextureLoader _textureLoader = new();
        private readonly GameObjectComparer _comparer = new();
        private readonly Dictionary<uint, Sprite> _sprites;

        public GameWorldController(GameWorld gameWorld)
        {
            _textureLoader.LoadTextures();
            _sprites = gameWorld.GameObjects
                .Select(go => (go.ObjectId, sprite: new Sprite { Position = new(go.PositionX, go.PositionY) }))
                .ToDictionary(k => k.ObjectId, v => v.sprite);
        }

        public void Draw(RenderWindow window, int drawDistance, GameWorld gameWorld)
        {
            gameWorld.GameObjects.Sort(_comparer);

            foreach (var gameObject in gameWorld.GameObjects)
            {
                var difference = Math.Sqrt(Math.Pow(gameWorld.Player.PositionX - gameObject.PositionX, 2) + Math.Pow(gameWorld.Player.PositionY - gameObject.PositionY, 2));
                if(difference <= drawDistance)
                {
                    gameObject.State = States.NoAction1;
                    _sprites[gameObject.ObjectId].Texture = _textureLoader.Textures[gameObject.TextureType][gameObject.State];
                    _sprites[gameObject.ObjectId].Scale = new(10, 10);
                    window.Draw(_sprites[gameObject.ObjectId]);
                }
            }
        }

        public void Update(GameWorld gameWorld)
        {

        }
    }
}
