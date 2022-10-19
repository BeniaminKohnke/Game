using Aardvark.Base;
using GameAPI.GameObjects;

namespace GameAPI
{
    public sealed class ProceduralGeneration
    {
        private readonly PerlinNoise _noise = new();
        private readonly Dictionary<Types, Grids[]> _grids = new()
        {
            [Types.Tree] = new[] { Grids.Tree1 },
            [Types.Rock] = new[] { Grids.Rock1 },
            [Types.Building] = new[] { Grids.Building1 },
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
            }
        };
        public int Seed { get; }

        public ProceduralGeneration(int seed) => Seed = seed;

        public GameObject? CreateObject(int x, int y, GridLoader loader)
        {
            var noise = _noise.Noise(x ^ Seed, y ^ Seed);
            var value = (int)(x ^ y ^ Seed);
            var type = GetObjectType(noise, value);
            if (type != Types.None)
            {
                var grid = GetObjectGrid(type, value);
                var go = new GameObject(loader, x, y, type, grid);
                if (_parameters.TryGetValue(type, out var parameters))
                {
                    foreach (var parameter in parameters)
                    {
                        go.ObjectParameters[parameter.Key] = (parameter.Key == ObjectsParameters.Loot ? InitializeLootParameter(x, y, loader, type, value) : parameter.Value) ?? 0;
                    }
                }
                return go;
            }

            return null;
        }

        private Types GetObjectType(float noise, int value)
        {
            if (noise < 0.8f)
            {

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
            return grids[new Random(value + 8).Next(0, grids.Length - 1)];
        }

        private Item[] InitializeLootParameter(int x, int y, GridLoader loader, Types type, int value)
        {
            var items = new List<Item>();
            switch (type)
            {
                case Types.Tree:
                    items.Add(new(loader, x + new Random(value + 1).Next(-2, 2), y + new Random(value + 3).Next(-2, 2) + 20, Types.Item, Grids.ItemWood)
                    {
                        IsActive = false,
                        Name = "Wood",
                    });
                    break;
                case Types.Rock:
                    items.Add(new(loader, x + new Random(value + 2).Next(-2, 2), y + new Random(value + 4).Next(-2, 2), Types.Item, Grids.ItemRock)
                    {
                        IsActive = false,
                        Name = "Rock",
                    });
                    break;
            }

            return items.ToArray();
        }
    }
}
