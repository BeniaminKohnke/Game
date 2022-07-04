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
                    var image = new Image((uint)texture.Length, (uint)texture[0].Length);
                    
                    for(uint i = 0; i < texture.Length; i++)
                    {
                        for(uint j = 0; j < texture[i].Length; j++)
                        {
                            image.SetPixel(i, j, GetColor(texture[i][j]));
                        }
                    }

                    image.FlipVertically();
                    var sprite = new Sprite
                    {
                        Texture = new Texture(image),
                        Position = new(gameObject.X, gameObject.Y),
                        Origin = new(image.Size.X / 2, image.Size.Y / 2),
                        Scale = new(10, 10),
                        Rotation = 90,
                    };

                    gameObject.OriginShiftY = (int)image.Size.Y / 2;
                    gameObject.VerticalColliderLength = texture.Count(c => c.Any(v => v == 3));
                    gameObject.HorizontalColliderLength = texture.Max(c => Array.LastIndexOf(c, (byte)3) - Array.IndexOf(c, (byte)3));

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

        private static Color GetColor(byte color) => color switch
        {
            2 => Color.Black,
            3 or 4 => Color.White,
            _ => Color.Transparent,
        };
    }
}
