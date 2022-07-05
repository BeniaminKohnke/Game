﻿using GameAPI;
using SFML.Graphics;

namespace Game.GameCore
{
    public class GameWorldController
    {
        private readonly GameObjectPositionComparer _comparer = new();
        private readonly Dictionary<Shapes, Dictionary<States, Texture>> _textures = new();
        private readonly Dictionary<uint, (States state, Sprite sprite)> _gameObjectsSprites = new();

        public GameWorldController(GameWorld gameWorld)
        {
            gameWorld.ShapeLoader.LoadShapes();
            foreach(var type in gameWorld.ShapeLoader.Shapes)
            {
                _textures[type.Key] = new();
                foreach(var pair in type.Value)
                {
                    if(pair.Value.Length > 0 && pair.Value.Any(r => r.Any()))
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
                Texture = _textures[go.Shape][go.State],
                Position = new(go.GridPositionX, go.GridPositionY),
                Origin = new(_textures[go.Shape][go.State].Size.X / 2, _textures[go.Shape][go.State].Size.Y / 2),
            }));

        }

        public void Draw(RenderWindow window, int drawDistance, GameWorld gameWorld)
        {
            gameWorld.GameObjects.Sort(_comparer);

            foreach (var gameObject in gameWorld.GameObjects)
            {
                var difference = Math.Sqrt(Math.Pow(gameWorld.Player.GridPositionX - gameObject.GridPositionX, 2) + Math.Pow(gameWorld.Player.GridRelativePositionY - gameObject.GridRelativePositionY, 2));
                if (difference <= drawDistance)
                {
                    var (state, sprite) = _gameObjectsSprites[gameObject.Id];
                    if(gameObject.State != state)
                    {
                        var texture = _textures[gameObject.Shape][gameObject.State];
                        state = gameObject.State;
                        sprite = new()
                        {
                            Texture = texture,
                            Position = new(gameObject.GridPositionX, gameObject.GridPositionY),
                            Origin = new(texture.Size.X / 2, texture.Size.Y / 2),
                        };
                        _gameObjectsSprites[gameObject.Id] = (state, sprite);
                    }
                    else
                    {
                        sprite.Position = new(gameObject.GridPositionX, gameObject.GridPositionY);
                    }

                    window.Draw(sprite);
                }
            }

            window.SetTitle($"X:{gameWorld.Player.GridPositionX} Y:{gameWorld.Player.GridRelativePositionY}");
        }

        private static Color GetColor(byte color) => color switch
        {
            2 or 5 => Color.Black,
            3 or 4 => Color.White,
            _ => Color.Transparent,
        };
    }
}
