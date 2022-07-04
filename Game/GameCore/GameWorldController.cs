using GameAPI;
using SFML.Graphics;

namespace Game.GameCore
{
    public class GameWorldController
    {
        private readonly TextureLoader _textureLoader = new();
        private readonly GameObjectPositionComparer _comparer = new();
        private Dictionary<TexturesTypes, Dictionary<States, Image>> _images = new();
        public GameWorldController(GameWorld gameWorld)
        {
            _textureLoader.LoadTextures();

            foreach(var type in _textureLoader.Textures)
            {
                _images[type.Key] = new Dictionary<States, Image>();
                foreach(var pair in type.Value)
                {
                    if(pair.Value.Length > 0 && pair.Value.Any(r => r.Any()))
                    {
                        var image = new Image((uint)pair.Value.Length, (uint)pair.Value[0].Length);
                        for (uint i = 0; i < pair.Value.Length; i++)
                        {
                            for (uint j = 0; j < pair.Value[i].Length; j++)
                            {
                                image.SetPixel(i, j, GetColor(pair.Value[i][j]));
                            }
                        }

                        image.FlipVertically();
                        _images[type.Key][pair.Key] = image;
                    }
                }
            }
        }

        public void Draw(RenderWindow window, int drawDistance, GameWorld gameWorld)
        {
            gameWorld.GameObjects.Sort(_comparer);

            foreach (var gameObject in gameWorld.GameObjects)
            {
                var difference = Math.Sqrt(Math.Pow(gameWorld.Player.X - gameObject.X, 2) + Math.Pow(gameWorld.Player.Y - gameObject.Y, 2));
                if(difference <= drawDistance)
                {
                    var image = _images[gameObject.TextureType][gameObject.State];
                    var sprite = new Sprite
                    {
                        Texture = new(image),
                        Position = new(gameObject.X, gameObject.Y),
                        Origin = new(image.Size.X / 2, image.Size.Y / 2),
                        Scale = new(10, 10),
                        Rotation = 90,
                    };

                    gameObject.OriginShiftY = (int)image.Size.Y / 2;
                    gameObject.VerticalColliderLength = _textureLoader.Textures[gameObject.TextureType][gameObject.State]
                        .Count(c => c.Any(v => v == 3));
                    gameObject.HorizontalColliderLength = _textureLoader.Textures[gameObject.TextureType][gameObject.State]
                        .Max(c => Array.LastIndexOf(c, (byte)3) - Array.IndexOf(c, (byte)3));

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
