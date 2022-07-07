namespace GameAPI
{
    public class GameWorld
    {
        private readonly PositionComparer _comparer = new();
        public GridLoader Loader { get; } = new();
        public List<GameObject> GameObjects { get; } = new();
        public Player Player { get; private set; }

        public GameWorld()
        {
            GameObjects = new();

            Player = new(Loader, 0, 0)
            {
                MovementSpeed = 1,
                Weight = 70,
            };

            var tree = new GameObject(Loader, 20, 20, Types.Tree, Grids.Tree1)
            {
                Weight = 1000,
            };

            var tree2 = new GameObject(Loader, 40, 40, Types.Tree, Grids.Tree1)
            {
                Weight = 1000,
            };

            var building1 = new GameObject(Loader, -50, -20, Types.Building, Grids.Building1)
            {
                Weight = 10000,
            };

            GameObjects.Add(tree);
            GameObjects.Add(tree2);
            GameObjects.Add(building1);
            GameObjects.Add(Player);
        }

        public void Sort() => GameObjects.Sort(_comparer);

        public void Update()
        {
            HandleCollisions();
        }

        private void HandleCollisions()
        {
            for (int i = 0; i < GameObjects.Count; i++)
            {
                var direction = GameObjects[i].DequeueMovement(Loader);
                while (direction != Directions.None)
                {
                    var newRectangle = direction switch
                    {
                        Directions.Up => GameObjects[i].CopyWithShift(0, -GameObjects[i].MovementSpeed),
                        Directions.Down => GameObjects[i].CopyWithShift(0, GameObjects[i].MovementSpeed),
                        Directions.Left => GameObjects[i].CopyWithShift(-GameObjects[i].MovementSpeed, 0),
                        Directions.Right => GameObjects[i].CopyWithShift(GameObjects[i].MovementSpeed, 0),
                        _ => null,
                    };

                    if(newRectangle != null)
                    {
                        var canMove = true;
                        for (int j = 0; j < GameObjects.Count; j++)
                        {
                            if (i != j && newRectangle.CheckCollision(GameObjects[j]))
                            {
                                canMove = false;
                                break;
                            }
                        }

                        if (canMove)
                        {
                            GameObjects[i].V1 = newRectangle.V1;
                        }
                    }

                    direction = GameObjects[i].DequeueMovement(Loader);
                }
            }
        }

        private class PositionComparer : IComparer<GameObject>
        {
            public int Compare(GameObject? first, GameObject? second)
            {
                if (first != null && second != null)
                {
                    var differenceY = (first.V1.y + first.SizeY) - (second.V1.y + second.SizeY);
                    if (differenceY < 0)
                    {
                        return -1;
                    }
                    if (differenceY > 0)
                    {
                        return 1;
                    }

                    var differenceX = first.V1.x - second.V1.x;
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
