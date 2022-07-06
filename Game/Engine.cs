using GameAPI;
using SFML.Graphics;

namespace Game
{
    public class Engine
    {
        private readonly Dictionary<Grids, Dictionary<States, Texture>> _textures = new();
        private readonly Dictionary<uint, (States state, Sprite sprite)> _gameObjectsSprites = new();

        public Engine(GameWorld gameWorld)
        {
            foreach (var type in gameWorld.Loader.GetGrids())
            {
                _textures[type.Key] = new();
                foreach (var pair in type.Value)
                {
                    if (pair.Value.Length > 0 && pair.Value.Any(r => r.Any()))
                    {
                        var image = new Image((uint)pair.Value[0].Length, (uint)pair.Value.Length);
                        for (uint i = 0; i < pair.Value.Length; i++)
                        {
                            for (uint j = 0; j < pair.Value[i].Length; j++)
                            {
                                image.SetPixel(j, i, GetColor(pair.Value[i][j]));
                            }
                        }

                        _textures[type.Key][pair.Key] = new(image);
                    }
                }
            }

            gameWorld.GameObjects.ForEach(go => _gameObjectsSprites[go.Id] = (go.State, new()
            {
                Texture = _textures[go.Grid][go.State],
                Position = new(go.V1.x, go.V1.y),
            }));

        }

        public void Draw(RenderWindow window, int drawDistance, GameWorld gameWorld)
        {
            gameWorld.Sort();
            foreach (var gameObject in GameAPI.DSL.ScriptFunctions.ScanArea(gameWorld, drawDistance))
            {
                var (state, sprite) = _gameObjectsSprites[gameObject.Id];
                if (gameObject.State != state)
                {
                    var texture = _textures[gameObject.Grid][gameObject.State];
                    state = gameObject.State;
                    sprite = new()
                    {
                        Texture = texture,
                        Position = new(gameObject.V1.x, gameObject.V1.y),
                    };
                    _gameObjectsSprites[gameObject.Id] = (state, sprite);
                }
                else
                {
                    sprite.Position = new(gameObject.V1.x, gameObject.V1.y);
                }

                window.Draw(sprite);
            }

            window.SetTitle($"X:{gameWorld.Player.V1.x} Y:{gameWorld.Player.V1.y}");
        }

        private static Color GetColor(byte color) => color switch
        {
            2 or 5 => Color.Black,
            3 or 4 => Color.White,
            _ => Color.Transparent,
        };
    }
}
