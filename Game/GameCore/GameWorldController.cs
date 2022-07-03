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
                        Texture = texture.texture,
                        Position = new((int)Math.Floor(gameObject.X), (int)Math.Floor(gameObject.Y)),
                        Origin = new((int)Math.Floor(texture.size.X / 2d), (int)Math.Floor(texture.size.Y / 2d)),
                    };
                    gameObject.VerticalColliderLength = 1;
                    gameObject.HorizontalColliderLength = (texture.size.X / 2);
                    gameObject.OriginShiftY = texture.size.Y / 2;
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
