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
            Player = new(Loader, 0, 0)
            {
                MovementSpeed = 1,
            };

            GameObjects = new()
            {
                Player,
                new GameObject(Loader, 20, 20, Types.Tree, Grids.Tree1),
                new GameObject(Loader, 40, 40, Types.Tree, Grids.Tree1),
                new GameObject(Loader, -50, -20, Types.Building, Grids.Building1),
            };
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

                        if(canMove)
                        {
                            GameObjects[i].Position = newRectangle.Position;
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
