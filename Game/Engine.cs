using GameAPI;
using GameAPI.GameObjects;
using SFML.Graphics;

namespace Game
{
    public class Engine
    {
        private readonly Font _font = new($@"{Directory.GetCurrentDirectory()}\Font\PressStart2P-Regular.ttf");
        private readonly Dictionary<Grids, Dictionary<States, Texture>> _textures = new();
        private readonly Dictionary<uint, (States state, Sprite sprite)> _gameObjectsSprites = new();

        public Engine(GameWorld gameWorld)
        {
            foreach (var type in gameWorld.GetGrids())
            {
                _textures[type.Key] = new();
                foreach (var pair in type.Value)
                {
                    if (pair.Value.Count > 0 && pair.Value.Any(r => r.Any()))
                    {
                        _textures[type.Key][pair.Key] = new(CreateImage(pair.Value.Select(p => p.ToArray()).ToArray()));
                    }
                }
            }

            foreach (var go in gameWorld.GetObjects())
            {
                _gameObjectsSprites[go.Id] = (go.State, new()
                {
                    Texture = _textures[go.Grid][go.State],
                    Position = new(go.Position.x, go.Position.y),
                });
            }
        }

        public void Draw(RenderWindow window, int drawDistance, GameWorld? gameWorld)
        {
            if (gameWorld != null)
            {
                {
                    if (!(gameWorld.Player.ObjectParameters.TryGetValue(ObjectsParameters.Health, out var value) && value is short health))
                    {
                        health = 100;
                    }

                    drawDistance = (int)(drawDistance * (health / 100f));
                }

                var gameObjects = gameWorld.GetObjects(GetObjectsOptions.FromPlayer | GetObjectsOptions.AddPlayerItems | GetObjectsOptions.Ordered | GetObjectsOptions.OnlyActive, drawDistance);
                foreach (var gameObject in gameObjects)
                {
                    if (!_gameObjectsSprites.ContainsKey(gameObject.Id))
                    {
                        _gameObjectsSprites[gameObject.Id] = (gameObject.State, new()
                        {
                            Texture = _textures[gameObject.Grid][gameObject.State],
                            Position = new(gameObject.Position.x, gameObject.Position.y),
                        });
                    }

                    var (state, sprite) = _gameObjectsSprites[gameObject.Id];
                    if (gameObject.State != state)
                    {
                        var texture = _textures[gameObject.Grid][gameObject.State];
                        state = gameObject.State;
                        sprite = new()
                        {
                            Texture = texture,
                            Position = new(gameObject.Position.x, gameObject.Position.y),
                        };
                        _gameObjectsSprites[gameObject.Id] = (state, sprite);
                    }
                    else
                    {
                        sprite.Position = new(gameObject.Position.x, gameObject.Position.y);
                    }

                    if (gameObject is not Player && gameObject.ObjectParameters.TryGetValue(ObjectsParameters.Health, out var value) && value is short health)
                    {
                        var text = new Text()
                        {
                            Font = _font,
                            DisplayedString = health.ToString(),
                            Position = new(gameObject.Position.x + (gameObject.SizeX / 2) - 3, gameObject.Position.y + gameObject.SizeY),
                            CharacterSize = 200,
                            Scale = new(0.01f, 0.01f),
                        };

                        window.Draw(text);
                    }

                    window.Draw(sprite);
                }
            }
        }

        internal static Image CreateImage(byte[][] pixels)
        {
            var image = new Image((uint)pixels[0].Length, (uint)pixels.Length);
            for (uint i = 0; i < pixels.Length; i++)
            {
                for (uint j = 0; j < pixels[(int)i].Length; j++)
                {
                    image.SetPixel(j, i, pixels[(int)i][(int)j] switch
                    {
                        2 or 5 => Color.Black,
                        3 or 4 => Color.White,
                        _ => Color.Transparent,
                    });
                }
            }
            return image;
        }
    }
}
