using GameAPI.GameObjects;
using MoreLinq;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace GameAPI
{
    public enum GetObjectsOptions
    {
        None = 0,
        FromPlayer = 2,
        Ordered = 8,
        OnlyActive = 16,
        AddPlayerItems = 32,
    }

    public class GameWorld
    {
        private readonly Thread t_update;
        private ConcurrentBag<GameObject> _gameObjects;
        private readonly PositionComparer _comparer = new();
        private readonly GridLoader _loader = new();
        public Player Player { get; private set; }
        public bool IsActive { get; set; } = true;
        public double DeltaTime { get; set; } = 0f;

        public GameWorld()
        {
            var pickaxe = new Item(_loader, 0, 0, Types.Item, Grids.Pickaxe)
            {
                ItemType = ItemTypes.Mele,
                Name = "Pickaxe",
                ObjectParameters = new Dictionary<ObjectsParameters, object>
                {
                    [ObjectsParameters.ThrustDamage] = (ushort)30,
                },
            };
            var axe = new Item(_loader, 0, 0, Types.Item, Grids.Axe)
            {
                ItemType = ItemTypes.Mele,
                Name = "Axe",
                ObjectParameters = new Dictionary<ObjectsParameters, object>
                {
                    [ObjectsParameters.CuttingDamage] = (ushort)30,
                }
            };

            Player = new(_loader, 0, 0)
            {
                ObjectParameters = new()
                {
                    [ObjectsParameters.MovementSpeed] = 1,
                    [ObjectsParameters.Health] = (short)100,
                    [ObjectsParameters.ScanRadius] = 100,
                }
            };

            Player.Items.Add(axe);
            Player.Items.Add(pickaxe);
            Player.SelectedItemId = axe.Id;

            _gameObjects = new()
            {
                axe,
                pickaxe,
                Player,
                //new(_loader, 20, 20, Types.Tree, Grids.Tree1),
                //new(_loader, 40, 40, Types.Tree, Grids.Tree1),
                new(_loader, -50, -20, Types.Building, Grids.Building1),
                //new(_loader, 40, -20, Types.Rock, Grids.Rock1),
            };

            var random = new Random();
            for (int i = 0; i < 30; i++)
            {
                var x = random.Next(-200, 200);
                var y = random.Next(-200, 200);
                _gameObjects.Add(new(_loader, x, y, Types.Tree, Grids.Tree1)
                {
                    ObjectParameters = new()
                    {
                        [ObjectsParameters.Health] = (short)100,
                        [ObjectsParameters.ThrustDamageResistance] = (byte)100,
                        [ObjectsParameters.Loot] = new Item[]
                        {
                            new(_loader, x + random.Next(-2, 2), y + random.Next(-2, 2) + 20, Types.Item, Grids.ItemWood)
                            {
                                IsActive = false,
                                IsUsed = false,
                                Name = "Wood",
                            }
                        }
                    }
                });
            }

            for (int i = 0; i < 30; i++)
            {
                var x = random.Next(-200, 200);
                var y = random.Next(-200, 200);
                _gameObjects.Add(new(_loader, x, y, Types.Rock, Grids.Rock1)
                {
                    ObjectParameters = new()
                    {
                        [ObjectsParameters.Health] = (short)100,
                        [ObjectsParameters.CuttingDamageResistance] = (byte)100,
                        [ObjectsParameters.Loot] = new Item[]
                        {
                            new(_loader, x + random.Next(-2, 2), y + random.Next(-2, 2), Types.Item, Grids.ItemRock)
                            {
                                IsActive = false,
                                IsUsed = false,
                                Name = "Rock",
                            }
                        }
                    }
                });
            }

            t_update = new(new ThreadStart(Update));
            t_update.Start();
        }

        public ConcurrentDictionary<Grids, ConcurrentDictionary<States, ReadOnlyCollection<ReadOnlyCollection<byte>>>> GetGrids() => _loader.GetGrids();

        public List<GameObject> GetObjects(GetObjectsOptions options = GetObjectsOptions.None, int? radius = null)
        {
            var objects = _gameObjects.ToList();
            if (options.HasFlag(GetObjectsOptions.FromPlayer))
            {
                var squaredRadius = Math.Pow(radius ?? Player.ObjectParameters[ObjectsParameters.ScanRadius] as int? ?? 0, 2);
                foreach (var go in _gameObjects)
                {
                    var deltaX = Player.Position.x - go.Position.x;
                    var deltaY = Player.Position.y - go.Position.y;

                    if ((deltaX * deltaX) + (deltaY * deltaY) > squaredRadius)
                    {
                        objects.Remove(go);
                    }
                };
            }

            if (options.HasFlag(GetObjectsOptions.AddPlayerItems))
            {
                objects = objects.Concat(Player.Items).ToList();
            }

            if (options.HasFlag(GetObjectsOptions.OnlyActive))
            {
                objects = objects.Where(go => go.IsActive).ToList();
            }

            if (options.HasFlag(GetObjectsOptions.Ordered))
            {
                objects = objects.OrderBy(go => go, _comparer).ToList();
            }

            return objects;
        }

        private void Update()
        {
            while (IsActive)
            {
                var objectsToRemove = new List<uint>();
                foreach (var go in _gameObjects)
                {
                    if (go.ObjectParameters.TryGetValue(ObjectsParameters.Health, out var value) && value is short health)
                    {
                        if (health <= 0)
                        {
                            go.IsActive = false;
                            objectsToRemove.Add(go.Id);
                            if (go.ObjectParameters.TryGetValue(ObjectsParameters.Loot, out var v) && v is Item[] items)
                            {
                                foreach (var i in items)
                                {
                                    i.IsActive = true;
                                    _gameObjects.Add(i);
                                }
                            }
                        }
                    }

                    if (go.IsActive)
                    {
                        go.Update(DeltaTime, _loader);
                    }
                }
                HandleCollisions();

                var item = Player.Items.FirstOrDefault(i => i.Id == Player.SelectedItemId);
                if (item != null)
                {
                    if (item.Uses > 0)
                    {
                        item.Uses--;
                        HandleItemActions(item, Player.Id);
                    }
                }

                _gameObjects = new ConcurrentBag<GameObject>(_gameObjects.Where(go => !objectsToRemove.Contains(go.Id)));
            }
        }

        private void HandleCollisions()
        {
            foreach (var main in _gameObjects.Where(go => go.IsActive && go.ObjectType != Types.Item))
            {
                if (main.ObjectParameters.ContainsKey(ObjectsParameters.MovementSpeed))
                {
                    var direction = main.DequeueMovement(_loader);
                    while (direction != Directions.None)
                    {
                        var newRectangle = direction switch
                        {
                            Directions.Up => main.CopyWithShift(0, -1),
                            Directions.Down => main.CopyWithShift(0, 1),
                            Directions.Left => main.CopyWithShift(-1, 0),
                            Directions.Right => main.CopyWithShift(1, 0),
                            _ => null,
                        };

                        if (newRectangle != null)
                        {
                            var canMove = true;
                            foreach (var other in _gameObjects.Where(go => go.IsActive && !Player.Items.Contains(go)))
                            {
                                if (main.Id != other.Id && newRectangle.CheckCollision(other))
                                {
                                    if (other is not Item)
                                    {
                                        canMove = false;
                                    }
                                    else if(main is Player && other is Item item)
                                    {
                                        other.IsActive = false;
                                        Player.Items.Add(item);
                                    }
                                    break;
                                }
                            }

                            if (canMove)
                            {
                                main.Position = newRectangle.Position;
                            }
                        }

                        direction = main.DequeueMovement(_loader);
                    }
                }
            }
        }

        public void HandleItemActions(Item item, uint usingObjectId)
        {
            if (item != null && item.IsActive && item.IsUsed)
            {
                switch (item.ItemType)
                {
                    case ItemTypes.Mele:
                        {
                            foreach (var gameObject in _gameObjects)
                            {
                                if (gameObject.Id != item.Id && gameObject.Id != usingObjectId && item.CheckCollision(gameObject))
                                {
                                    if (gameObject.ObjectParameters.TryGetValue(ObjectsParameters.Health, out var value) && value is short health)
                                    {
                                        DealDamage(ObjectsParameters.CuttingDamage, ObjectsParameters.CuttingDamageResistance);
                                        DealDamage(ObjectsParameters.BluntDamage, ObjectsParameters.BluntDamageResistance);
                                        DealDamage(ObjectsParameters.ThrustDamage, ObjectsParameters.ThrustDamageResistance);
        
                                        gameObject.ObjectParameters[ObjectsParameters.Health] = health;
                                        void DealDamage(ObjectsParameters damageType, ObjectsParameters resistanceType)
                                        {
                                            if (item.ObjectParameters.TryGetValue(damageType, out value) && value is ushort damage)
                                            {
                                                if (!(gameObject.ObjectParameters.TryGetValue(resistanceType, out value) && value is byte resistance))
                                                {
                                                    resistance = 0;
                                                }
        
                                                health -= (short)(((100 - resistance) * 0.01f) * damage);
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                }
        
            }
        }

        private class PositionComparer : IComparer<GameObject>
        {
            public int Compare(GameObject? first, GameObject? second)
            {
                if (first != null && second != null)
                {
                    var differenceY = (first.Position.y + first.SizeY) - (second.Position.y + second.SizeY);
                    if (differenceY < 0)
                    {
                        return -1;
                    }
                    if (differenceY > 0)
                    {
                        return 1;
                    }

                    var differenceX = first.Position.x - second.Position.x;
                    if (differenceX > 0)
                    {
                        return -1;
                    }
                    if (differenceX < 0)
                    {
                        return 1;
                    }
                }

                return (first != null && second != null) ? (first.Id > second.Id ? -1 : 1) : 0;
            }
        }
    }
}
