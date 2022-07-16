using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace GameAPI
{
    public enum GetObjectsOptions
    {
        None = 0,
        FromPlayer = 2,
        Ordered = 8,
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
                MovementSpeed = 1,
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
            if (options.HasFlag(GetObjectsOptions.FromPlayer))
            {
                var squaredRadius = Math.Pow(radius ?? Player.ScanRadius, 2);
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

            if (options.HasFlag(GetObjectsOptions.Ordered))
            {
                return objects.OrderBy(go => go, _comparer).ToList();
            }

            return objects;
        }

        private void Update()
        {
            while(IsActive)
            {
                HandleCollisions();
            }
        }

        private void HandleCollisions()
        {
            foreach(var main in _gameObjects)
            {
                var direction = main.DequeueMovement(_loader);
                while (direction != Directions.None)
                {
                    var newRectangle = direction switch
                    {
                        Directions.Up => main.CopyWithShift(0, -main.MovementSpeed),
                        Directions.Down => main.CopyWithShift(0, main.MovementSpeed),
                        Directions.Left => main.CopyWithShift(-main.MovementSpeed, 0),
                        Directions.Right => main.CopyWithShift(main.MovementSpeed, 0),
                        _ => null,
                    };

                    if (newRectangle != null)
                    {
                        var canMove = true;
                        foreach(var other in _gameObjects)
                        {
                            if(main.Id != other.Id && newRectangle.CheckCollision(other))
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
