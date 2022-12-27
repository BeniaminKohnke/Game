using Aardvark.Base;
using GameAPI.GameObjects;
using System;

namespace GameAPI
{
    public sealed class ProceduralGeneration
    {
        private readonly PerlinNoise _noise = new();
        private readonly Dictionary<Types, Grids[]> _grids = new()
        {
            [Types.Tree] = new[] { Grids.Tree1, Grids.Tree2 },
            [Types.Rock] = new[] { Grids.Rock1, Grids.Rock2 },
            [Types.Building] = new[] { Grids.Building1 },
            [Types.Enemy] = new[] { Grids.Enemy1, },
            [Types.Bush] = new[] { Grids.Bush1, },
            [Types.Grass] = new[] { Grids.Grass1, },
        };
        private readonly Dictionary<Types, Dictionary<ObjectsParameters, object?>> _parameters = new()
        {
            [Types.Tree] = new()
            {
                [ObjectsParameters.Health] = (short)100,
                [ObjectsParameters.ThrustDamageResistance] = (byte)100,
                [ObjectsParameters.Loot] = null,
            },
            [Types.Rock] = new()
            {
                [ObjectsParameters.Health] = (short)100,
                [ObjectsParameters.CuttingDamageResistance] = (byte)100,
                [ObjectsParameters.Loot] = null,
            },
            [Types.Enemy] = new()
            {
                [ObjectsParameters.Health] = (short)100,
                [ObjectsParameters.CuttingDamage] = (ushort)10,
                [ObjectsParameters.MovementSpeed] = 1,
                [ObjectsParameters.Loot] = null,
            },
            [Types.Bush] = new()
            {
                [ObjectsParameters.Health] = (short)100,
                [ObjectsParameters.Loot] = null,
            },
            [Types.Grass] = new()
            {
                [ObjectsParameters.Health] = (short)100,
                [ObjectsParameters.Loot] = null,
            }
        };
        public int Seed { get; }

        public ProceduralGeneration(int seed) => Seed = seed;

        public GameObject? CreateObject(int x, int y)
        {
            var noise = _noise.Noise(x ^ Seed, y ^ Seed);
            var value = Seed + x + y;
            var type = GetObjectType(noise, value);
            if (type != Types.None)
            {
                var grids = _grids[type];
                var grid = grids[new Random(value / 2).Next(0, grids.Length)];
                var go = type switch
                {
                    Types.Enemy => new Enemy(x, y, grid),
                    Types.Item => new Item(x, y, type, grid),
                    Types.Player => new Player(x, y),
                    _ => new GameObject(x, y, type, grid),
                };

                if (_parameters.TryGetValue(type, out var parameters))
                {
                    foreach (var parameter in parameters)
                    {
                        go.ObjectParameters[parameter.Key] = (parameter.Key == ObjectsParameters.Loot ? InitializeLootParameter(x, y, type) : parameter.Value) ?? 0;
                    }
                }
                return go;
            }

            return null;
        }

        private static Types GetObjectType(float noise, int value)
        {
            var random = new Random(value);
            if (noise < 0.65f)
            {
                return Types.None;
            }
            else if (noise < 0.7f)
            {
                return random.Next(0, 20) == 1 ? Types.Tree : Types.None;
            }
            else if (noise < 0.75f)
            {
                return random.Next(0, 20) == 1 ? Types.Rock : Types.None;
            }
            else if (noise < 0.8f)
            {
                return random.Next(0, 40) == 1 ? Types.Grass : Types.None;
            }
            else if (noise < 0.85f)
            {
                return random.Next(0, 40) == 1 ? Types.Bush : Types.None;
            }
            else if (noise < 0.95f)
            {
                return random.Next(0, 200) == 1 ? Types.Enemy : Types.None;
            }
            else if (noise < 0.96f)
            {
                return random.Next(0, 300) == 1 ? Types.Building : Types.None;
            }

            return Types.None;
        }

        private static Item[] InitializeLootParameter(int x, int y, Types type)
        {
            var items = new List<Item>();
            switch (type)
            {
                case Types.Tree:
                    items.Add(new(x, y + 20, Types.Item, Grids.ItemWood)
                    {
                        IsActive = false,
                        Name = Items.Wood,
                        ItemType = ItemTypes.Material,
                    });
                    break;
                case Types.Rock:
                    items.Add(new(x, y, Types.Item, Grids.ItemRock)
                    {
                        IsActive = false,
                        Name = Items.Rock,
                        ItemType = ItemTypes.Material,
                    });
                    break;
                case Types.Grass:
                    items.Add(new(x, y, Types.Item, Grids.ItemFiber)
                    {
                        IsActive = false,
                        Name = Items.Fiber,
                        ItemType = ItemTypes.Material,
                    });
                    break;
                case Types.Bush:
                    items.Add(new(x, y, Types.Item, Grids.ItemFruit)
                    {
                        IsActive = false,
                        Name = Items.Fruit,
                        ItemType = ItemTypes.Consumable,
                        ObjectParameters = new Dictionary<ObjectsParameters, object>
                        {
                            [ObjectsParameters.Healing] = (ushort)5,
                        }
                    });
                    break;
                case Types.Enemy:
                    for (var i = 0; i < 5; i++)
                    {
                        items.Add(new Item(x, y, Types.Item, Grids.ItemFruit)
                        {
                            IsActive = false,
                            Name = Items.Fruit,
                            ItemType = ItemTypes.Consumable,
                            ObjectParameters = new Dictionary<ObjectsParameters, object>
                            {
                                [ObjectsParameters.Healing] = (ushort)5,
                            }
                        });
                    }
                    break;
            }

            return items.ToArray();
        }
    }
}
