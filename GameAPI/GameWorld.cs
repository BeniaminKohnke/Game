using GameAPI.GameObjects;
using MoreLinq;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace GameAPI
{
    public enum GetObjectsOptions : byte
    {
        None = 0,
        FromPlayer = 2,
        Ordered = 8,
        OnlyActive = 16,
        AddPlayerItems = 32,
    }

    public sealed class GameWorld
    {
        private (int x, int y) _waypoint = (0, 0);
        private readonly ushort _generationDistance = 500;
        private ConcurrentBag<GameObject> _gameObjects;
        private readonly ProceduralGeneration _procedure;
        private readonly PositionComparer _comparer = new();
        public Player Player { get; private set; }
        public bool IsActive { get; set; } = true;
        public ushort EnemyRange { get; set; } = 25;

        public GameWorld(int seed)
        {
            Player = new(-400, -400)
            {
                ObjectParameters = 
                {
                    [ObjectsParameters.MovementSpeed] = 1,
                    [ObjectsParameters.Health] = (short)100,
                    [ObjectsParameters.ScanRadius] = 200,
                    [ObjectsParameters.MeleeRange] = 5,
                }
            };

            _procedure = new(seed == 0 ? 1000000 : seed);
            var pickaxe = new Item(0, 0, Types.Item, Grids.Pickaxe)
            {
                ItemType = ItemTypes.Melee,
                Name = "Pickaxe",
                ObjectParameters = new Dictionary<ObjectsParameters, object>
                {
                    [ObjectsParameters.ThrustDamage] = (ushort)30,
                },
                IsActive = true,
            };
            var axe = new Item(0, 0, Types.Item, Grids.Axe)
            {
                ItemType = ItemTypes.Melee,
                Name = "Axe",
                ObjectParameters = new Dictionary<ObjectsParameters, object>
                {
                    [ObjectsParameters.CuttingDamage] = (ushort)30,
                },
                IsActive = true,
            };

            Player.Items.Add(axe);
            Player.Items.Add(pickaxe);
            Player.SelectedItemId = axe.Id;
            Player.ItemsMenu[0] = axe.Id;
            Player.ItemsMenu[1] = pickaxe.Id;

            _gameObjects = new()
            {
                axe,
                pickaxe,
                Player,
            };

            for (var x = _waypoint.x - _generationDistance; x <= _waypoint.x + _generationDistance; x++)
            {
                for (var y = _waypoint.y - _generationDistance; y <= _waypoint.y + _generationDistance; y++)
                {
                    var go = _procedure.CreateObject(x, y);
                    if (go != null)
                    {
                        _gameObjects.Add(go);
                    }
                }
            }
        }

        public static Dictionary<Grids, Dictionary<States, ReadOnlyCollection<ReadOnlyCollection<byte>>>> GetGrids() => GridLoader.GetGrids();

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

        public void Update(float deltaTime)
        {
            if (IsActive)
            {
                var item = Player.Items.FirstOrDefault(i => i.Id == Player.SelectedItemId);
                if (item != null)
                {
                    if (item.Uses > 0)
                    {
                        HandleItemActions(item, Player.Id);
                        item.Uses = 0;
                    }
                }

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
                        go.Update(deltaTime);
                    }
                }
                HandleEnemies(deltaTime);
                HandleCollisions();
                _gameObjects = new ConcurrentBag<GameObject>(_gameObjects.Where(go => !objectsToRemove.Contains(go.Id)));

                HandleObjectGeneration();
            }
        }

        private void HandleCollisions()
        {
            foreach (var main in _gameObjects.Where(go => go.IsActive && go.ObjectType != Types.Item))
            {
                if (main.ObjectParameters.ContainsKey(ObjectsParameters.MovementSpeed))
                {
                    var direction = main.DequeueMovement();
                    while (direction != Directions.None)
                    {
                        var newRectangle = direction switch
                        {
                            Directions.North => main.CopyWithShift(0, -1),
                            Directions.South => main.CopyWithShift(0, 1),
                            Directions.West => main.CopyWithShift(-1, 0),
                            Directions.East => main.CopyWithShift(1, 0),
                            _ => null,
                        };

                        if (newRectangle != null)
                        {
                            var canMove = true;
                            if (main.ObjectType != Types.Enemy)
                            {
                                foreach (var other in _gameObjects.Where(go => go.IsActive && !Player.Items.Contains(go) && go.ObjectType != Types.Enemy))
                                {
                                    if (main.Id != other.Id && newRectangle.CheckCollision(other))
                                    {
                                        if (other is not Item)
                                        {
                                            canMove = false;
                                        }
                                        else if (main is Player && other is Item item)
                                        {
                                            other.IsActive = false;
                                            Player.Items.Add(item);
                                        }
                                        break;
                                    }
                                }
                            }

                            if (canMove)
                            {
                                main.Position = newRectangle.Position;
                            }
                        }

                        direction = main.DequeueMovement();
                    }
                }
            }
        }

        private void HandleItemActions(Item item, uint usingObjectId)
        {
            if (item != null)
            {
                switch (item.ItemType)
                {
                    case ItemTypes.Melee:
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
        
                                                health -= (short)((100 - resistance) * 0.01f * damage);
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

        private void HandleEnemies(float deltaTime)
        {
            foreach (var go in _gameObjects)
            {
                if (go.ObjectType == Types.Enemy)
                {
                    if (go.ObjectParameters.TryGetValue(ObjectsParameters.AttackDelay, out var value) && value is float delay)
                    {
                        go.ObjectParameters[ObjectsParameters.AttackDelay] = delay + deltaTime;
                    }
                    else
                    {
                        delay = deltaTime;
                        go.ObjectParameters[ObjectsParameters.AttackDelay] = deltaTime;
                    }

                    var x = Player.Position.x - go.Position.x;
                    var y = Player.Position.y - go.Position.y;
                    var distanceX = Math.Abs(x);
                    var distanceY = Math.Abs(y);
                    if (distanceX < EnemyRange && distanceY < EnemyRange)
                    {
                        if (delay > 0.3f && distanceX < 5 && distanceY < 5)
                        {
                            if (go.ObjectParameters.TryGetValue(ObjectsParameters.CuttingDamage, out value) && value is ushort damage)
                            {
                                if (Player.ObjectParameters.TryGetValue(ObjectsParameters.Health, out value) && value is short health && health > 0)
                                {
                                    health -= (short)damage;
                                    if (health <= 0)
                                    {
                                        health = 0;
                                        Player.IsActive = false;
                                    }
                                    Player.ObjectParameters[ObjectsParameters.Health] = health;
                                }
                                go.ObjectParameters[ObjectsParameters.AttackDelay] = 0f;
                            }
                        }

                        if (distanceX > distanceY)
                        {
                            go.EnqueueMovement(x > 0 ? Directions.East : Directions.West);
                        }
                        else
                        {
                            if (y != 0)
                            {
                                go.EnqueueMovement(y > 0 ? Directions.South : Directions.North);
                            }
                        }
                    }
                }
            }
        }

        private void HandleObjectGeneration()
        {
            var directionX = 0;
            var directionY = 0;

            if (_waypoint.x + _generationDistance / 2 < Player.Position.x)
            {
                directionX = 1;
            }

            if (_waypoint.x - _generationDistance / 2 > Player.Position.x)
            {
                directionX = -1;
            }

            if (_waypoint.y + _generationDistance / 2 < Player.Position.y)
            {
                directionY = 1;
            }

            if (_waypoint.y - _generationDistance / 2 > Player.Position.y)
            {
                directionY = -1;
            }

            if (directionX != 0 || directionY != 0)
            {
                _waypoint = (_waypoint.x + (_generationDistance * directionX / 2), _waypoint.y + (_generationDistance * directionY / 2));
                _gameObjects = new(_gameObjects.Where(go => _waypoint.x - _generationDistance <= go.Position.x && go.Position.x <= _waypoint.x + _generationDistance
                                                         && _waypoint.y - _generationDistance <= go.Position.y && go.Position.y <= _waypoint.y + _generationDistance));
                new Thread(new ThreadStart(() =>
                {
                    Parallel.For(_waypoint.x - _generationDistance, _waypoint.x + _generationDistance, (x) =>
                    {
                        for (var y = _waypoint.y - _generationDistance; y <= _waypoint.y + _generationDistance; y++)
                        {
                            if (!_gameObjects.Any(go => go.Position.x == x && go.Position.y == y))
                            {
                                var go = _procedure.CreateObject(x, y);
                                if (go != null)
                                {
                                    _gameObjects.Add(go);
                                }
                            }
                        }
                    });
                })).Start();
            }
        }

        private sealed class PositionComparer : IComparer<GameObject>
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
