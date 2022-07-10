using GameAPI;
using SFML.Graphics;
using System.Collections.Concurrent;

namespace Game
{
    public class Engine
    {
        private readonly ConcurrentDictionary<Grids, ConcurrentDictionary<States, Texture>> _textures = new();
        private readonly ConcurrentDictionary<uint, (States state, Sprite sprite)> _gameObjectsSprites = new();

        public Engine(GameWorld gameWorld)
        {
            foreach (var type in gameWorld.GetGrids())
            {
                _textures[type.Key] = new();
                foreach (var pair in type.Value)
                {
                    if (pair.Value.Count > 0 && pair.Value.Any(r => r.Any()))
                    {
                        var image = new Image((uint)pair.Value[0].Count, (uint)pair.Value.Count);
                        for (uint i = 0; i < pair.Value.Count; i++)
                        {
                            for (uint j = 0; j < pair.Value[(int)i].Count; j++)
                            {
                                image.SetPixel(j, i, pair.Value[(int)i][(int)j] switch
                                {
                                    2 or 5 => Color.Black,
                                    3 or 4 => Color.White,
                                    _ => Color.Transparent,
                                });
                            }
                        }

                        _textures[type.Key][pair.Key] = new(image);
                    }
                }
            }

            gameWorld.GetObjects().ForEach(go => _gameObjectsSprites[go.Id] = (go.State, new()
            {
                Texture = _textures[go.Grid][go.State],
                Position = new(go.Position.x, go.Position.y),
            }));
        }

        public void Draw(RenderWindow window, int drawDistance, GameWorld gameWorld)
        {
            var gameObjects = gameWorld.GetObjects(GetObjectsOptions.FromPlayer | GetObjectsOptions.Ordered, drawDistance);
            foreach (var gameObject in gameObjects)
            {
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

                window.Draw(sprite);
            }
        }
    }
}
