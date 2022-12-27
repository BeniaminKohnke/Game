using Aardvark.Base;
using GameAPI.GameObjects;

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
            var value = x ^ y ^ Seed;
            var type = GetObjectType(noise, value);
            if (type != Types.None)
            {
                var grid = GetObjectGrid(type, value);
                var go = CreateObject(x, y, type, grid);
                if (_parameters.TryGetValue(type, out var parameters))
                {
                    foreach (var parameter in parameters)
                    {
                        go.ObjectParameters[parameter.Key] = (parameter.Key == ObjectsParameters.Loot ? InitializeLootParameter(x, y, type, value) : parameter.Value) ?? 0;
                    }
                }
                return go;
            }

            return null;
        }

        private static Types GetObjectType(float noise, int value)
        {
            if (noise < 0.76f)
            {
                return Types.None;
            }
            else if (noise < 0.77f && new Random(value + 11).Next(0, 50) == 0)
            {
                return Types.Enemy;
            }
            else if (noise < 0.78f && new Random(value + 13).Next(0, 100) == 0)
            {
                return Types.Grass;
            }
            else if (noise < 0.8f && new Random(value + 12).Next(0, 30) == 0)
            {
                return Types.Bush;
            }
            else if (noise < 0.85f && new Random(value + 5).Next(0, 35) == 0)
            {
                return Types.Rock;
            }
            else if (noise < 0.90f && new Random(value + 6).Next(0, 50) == 0)
            {
                return Types.Tree;
            }
            else if (0.99f < noise && new Random(value + 7).Next(0, 500) == 150)
            {
                return Types.Building;
            }

            return Types.None;
        }

        private Grids GetObjectGrid(Types type, int value)
        {
            var grids = _grids[type];
            return grids[new Random(value + 8).Next(0, grids.Length)];
        }

        private static Item[] InitializeLootParameter(int x, int y, Types type, int value)
        {
            var items = new List<Item>();
            switch (type)
            {
                case Types.Tree:
                    items.Add(new(x + new Random(value + 1).Next(-2, 2), y + new Random(value + 3).Next(-2, 2) + 20, Types.Item, Grids.ItemWood)
                    {
                        IsActive = false,
                        Name = Items.Wood,
                        ItemType = ItemTypes.Material,
                    });
                    break;
                case Types.Rock:
                    items.Add(new(x + new Random(value + 2).Next(-2, 2), y + new Random(value + 4).Next(-2, 2), Types.Item, Grids.ItemRock)
                    {
                        IsActive = false,
                        Name = Items.Rock,
                        ItemType = ItemTypes.Material,
                    });
                    break;
                case Types.Grass:
                    items.Add(new(x + new Random(value + 2).Next(-2, 2), y + new Random(value + 4).Next(-2, 2), Types.Item, Grids.ItemFiber)
                    {
                        IsActive = false,
                        Name = Items.Fiber,
                        ItemType = ItemTypes.Material,
                    });
                    break;
                case Types.Bush:
                    items.Add(new(x + new Random(value + 2).Next(-2, 2), y + new Random(value + 4).Next(-2, 2), Types.Item, Grids.ItemFruit)
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
            }

            return items.ToArray();
        }

        private static GameObject CreateObject(int x, int y, Types type, Grids grid) => type switch
        {
            Types.Enemy => new Enemy(x, y, grid),
            Types.Item => new Item(x, y, type, grid),
            Types.Player => new Player(x, y),
            _ => new GameObject(x, y, type, grid),
        };
    }
}
