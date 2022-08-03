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
        private readonly ConcurrentBag<GameObject> _gameObjects;
        private readonly PositionComparer _comparer = new();
        private readonly GridLoader _loader = new();
        public Player Player { get; private set; }
        public bool IsActive { get; set; } = true;

        public GameWorld()
        {
            Player = new(_loader, 0, 0)
            {
                ObjectParameters = new()
                {
                    [ObjectsParameters.MovementSpeed] = 1,
                    [ObjectsParameters.Health] = 100,
                    [ObjectsParameters.ScanRadius] = 100,
                }
            };

            _gameObjects = new()
            {
                Player,
                //new(_loader, 20, 20, Types.Tree, Grids.Tree1),
                //new(_loader, 40, 40, Types.Tree, Grids.Tree1),
                new(_loader, -50, -20, Types.Building, Grids.Building1),
                //new(_loader, 40, -20, Types.Rock, Grids.Rock1),
            };

            var random = new Random();
            for(int i = 0; i < 30; i++)
            {
                _gameObjects.Add(new(_loader, random.Next(-200, 200), random.Next(-200, 200), Types.Tree, Grids.Tree1));
            }

            for (int i = 0; i < 30; i++)
            {
                _gameObjects.Add(new(_loader, random.Next(-200, 200), random.Next(-200, 200), Types.Rock, Grids.Rock1));
            }

            t_update = new(new ThreadStart(Update));
            t_update.Start();
        }

        public ConcurrentDictionary<Grids, ConcurrentDictionary<States, ReadOnlyCollection<ReadOnlyCollection<byte>>>> GetGrids() => _loader.GetGrids();

        public List<GameObject> GetObjects(GetObjectsOptions options = GetObjectsOptions.None, int? radius = null)
        {
            var objects = _gameObjects.ToList();
            if(options.HasFlag(GetObjectsOptions.FromPlayer))
            {
                var squaredRadius = Math.Pow(radius ?? Player.ObjectParameters[ObjectsParameters.ScanRadius] as int? ?? 0, 2);
                foreach(var go in _gameObjects)
                {
                    var deltaX = Player.Position.x - go.Position.x;
                    var deltaY = Player.Position.y - go.Position.y;

                    if((deltaX * deltaX) + (deltaY * deltaY) > squaredRadius)
                    {
                        objects.Remove(go);
                    }
                };
            }

            if(options.HasFlag(GetObjectsOptions.AddPlayerItems))
            {
                objects = objects.Concat(Player.Items).ToList();
            }

            if(options.HasFlag(GetObjectsOptions.OnlyActive))
            {
                objects = objects.Where(go => go.IsActive).ToList();
            }

            if(options.HasFlag(GetObjectsOptions.Ordered))
            {
                objects = objects.OrderBy(go => go, _comparer).ToList();
            }

            return objects;
        }

        private void Update()
        {
            while(IsActive)
            {
                HandleCollisions();
                HandleItemsActions();
            }
        }

        private void HandleCollisions()
        {
            foreach(var main in _gameObjects)
            {
                if(main.ObjectParameters.ContainsKey(ObjectsParameters.MovementSpeed))
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
                            foreach (var other in _gameObjects)
                            {
                                if (main.Id != other.Id && newRectangle.CheckCollision(other))
                                {
                                    canMove = false;
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

        public void HandleItemsActions()
        {
            var item = Player.Items.ElementAtOrDefault(Player.SelectedItem - 1);
            if(item != null && item.IsActive && item.IsUsed)
            {
                switch(item.ItemType)
                {
                    case ItemTypes.Mele:
                        {
                            foreach(var gameObject in _gameObjects)
                            {
                                if(item.CheckCollision(gameObject))
                                {
                                    if(gameObject.ObjectParameters.TryGetValue(ObjectsParameters.Health, out var value) && value is ushort health)
                                    {
                                        DealDamage(ObjectsParameters.SlashDamage, ObjectsParameters.SlashDamageResistance);
                                        DealDamage(ObjectsParameters.BluntDamage, ObjectsParameters.BluntDamageResistance);
                                        DealDamage(ObjectsParameters.ThrustDamage, ObjectsParameters.ThrustDamageResistance);

                                        gameObject.ObjectParameters[ObjectsParameters.Health] = health;
                                        void DealDamage(ObjectsParameters damageType, ObjectsParameters resistanceType)
                                        {
                                            if (item.ObjectParameters.TryGetValue(damageType, out value) && value is ushort damage)
                                            {
                                                if (!(gameObject.ObjectParameters.TryGetValue(resistanceType, out value) && value is ushort resistance))
                                                {
                                                    resistance = 0;
                                                }

                                                health -= (ushort)((1 - resistance) * damage);
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
