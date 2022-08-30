﻿using Game.GUI;
using GameAPI;
using SFML.Graphics;
using System.Collections.Concurrent;

namespace Game
{
    public class Engine
    {
        private readonly ConcurrentDictionary<Grids, ConcurrentDictionary<States, Texture>> _textures = new();
        private readonly ConcurrentDictionary<uint, (States state, Sprite sprite)> _gameObjectsSprites = new();
        public Interface GameInterface { get; }

        public Engine(GameWorld gameWorld)
        {
            GameInterface = new(gameWorld);

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

            gameWorld.GetObjects().ForEach(go => _gameObjectsSprites[go.Id] = (go.State, new()
            {
                Texture = _textures[go.Grid][go.State],
                Position = new(go.Position.x, go.Position.y),
            }));
        }

        public void Draw(RenderWindow window, int drawDistance, GameWorld gameWorld)
        {
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

                window.Draw(sprite);
            }

            GameInterface.Draw(window, gameWorld);
        }

        public void Release()
        {
            GameInterface.Release();
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
