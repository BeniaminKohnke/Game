using GameAPI;
using SFML.Graphics;

namespace Game.GameCore
{
    public class GameWorldController
    {
        private readonly TextureLoader _textureLoader = new();
        private readonly GameObjectPositionComparer _comparer = new();

        public GameWorldController(GameWorld gameWorld)
        {
            _textureLoader.LoadTextures();
        }

        public void Draw(RenderWindow window, int drawDistance, GameWorld gameWorld)
        {
            gameWorld.GameObjects.Sort(_comparer);

            foreach (var gameObject in gameWorld.GameObjects)
            {
                var difference = Math.Sqrt(Math.Pow(gameWorld.Player.X - gameObject.X, 2) + Math.Pow(gameWorld.Player.Y - gameObject.Y, 2));
                if(difference <= drawDistance)
                {
                    var texture = _textureLoader.Textures[gameObject.TextureType][gameObject.State];
                    var sprite = new Sprite
                    {
                        Texture = texture,
                        Position = new(gameObject.X, gameObject.Y),
                        Origin = new(texture.Size.X / 2, texture.Size.Y / 2),
                        Scale = new (10, 10),
                    };
                    gameObject.VerticalColliderLength = 10;
                    gameObject.HorizontalColliderLength = (int)texture.Size.X * 5;
                    gameObject.OriginShiftY = (int)texture.Size.Y * 5;
                    window.Draw(sprite);
                }
            }

            window.SetTitle($"X:{gameWorld.Player.X} Y:{gameWorld.Player.Y}");
        }

        public void Update(GameWorld gameWorld)
        {
            for (int i = 0; i < gameWorld.GameObjects.Count; i++)
            {
                for (int j = i + 1; j < gameWorld.GameObjects.Count; j++)
                {
                    gameWorld.GameObjects[i].HandleCollison(gameWorld.GameObjects[j]);
                }
            }
        }
    }
}
