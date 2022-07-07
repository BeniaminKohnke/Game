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
                    var newRactangle = direction switch
                    {
                        Directions.Up => GameObjects[i].CopyWithShift(0, -GameObjects[i].MovementSpeed),
                        Directions.Down => GameObjects[i].CopyWithShift(0, GameObjects[i].MovementSpeed),
                        Directions.Left => GameObjects[i].CopyWithShift(-GameObjects[i].MovementSpeed, 0),
                        Directions.Right => GameObjects[i].CopyWithShift(GameObjects[i].MovementSpeed, 0),
                        _ => null,
                    };

                    if(newRactangle != null)
                    {
                        var canMove = true;
                        for (int j = i + 1; j < GameObjects.Count; j++)
                        {
                            if (newRactangle.CheckCollision(GameObjects[j]))
                            {
                                canMove = false;
                                break;
                            }
                        }

                        if (canMove)
                        {
                            GameObjects[i].V1 = newRactangle.V1;
                        }
                    }

                    direction = GameObjects[i].DequeueMovement(Loader);
                }
            }
        }

        //private bool HandleCollision(GameObject first, GameObject second, long newX, long newY)
        //{
        //    var isColliding = false;
        //
        //    var mainX = newX + first.SizeX / 2;
        //    var mainY = newY + first.SizeY / 2;
        //
        //    var otherX = second.V1.x + second.SizeX / 2;
        //    var otherY = second.V1.y + second.SizeY / 2;
        //
        //    var deltaX = otherX - mainX;
        //    var deltaY = otherY - mainY;
        //    if (Math.Abs(deltaX) <= (first.SizeX / 2 + second.SizeX / 2) && Math.Abs(deltaY) <= (first.SizeY / 2 + second.SizeY / 2))
        //    {
        //        long x1, y1, x2, y2;
        //
        //        if (deltaX > 0)
        //        {
        //            x1 = newX;
        //            x2 = second.V1.x + second.SizeX;
        //        }
        //        else if (deltaX < 0)
        //        {
        //            x1 = newX + first.SizeX;
        //            x2 = second.V1.x;
        //        }
        //        else
        //        {
        //            if (first.SizeX > second.SizeX)
        //            {
        //                x1 = newX;
        //                x2 = newX + first.SizeX;
        //            }
        //            else
        //            {
        //                x1 = second.V1.x;
        //                x2 = second.V1.x + second.SizeX;
        //            }
        //        }
        //
        //        if (deltaY > 0)
        //        {
        //            y1 = newY;
        //            y2 = second.V1.y + second.SizeY;
        //        }
        //        else if (deltaY < 0)
        //        {
        //            y1 = newY + first.SizeY;
        //            y2 = second.V1.y;
        //        }
        //        else
        //        {
        //            if (first.SizeY > second.SizeY)
        //            {
        //                y1 = newY;
        //                y2 = newY + first.SizeY;
        //            }
        //            else
        //            {
        //                y1 = second.V1.y;
        //                y2 = second.V1.y + second.SizeY;
        //            }
        //        }
        //
        //        var relativeDeltaX = x2 - x1;
        //        var relativeDeltaY = y2 - y1;
        //
        //        var grid = new byte[relativeDeltaX][];
        //        for(int i = 0; i < grid.Length; i++)
        //        {
        //            grid[i] = new byte[relativeDeltaY];
        //            for(int j = 0; j < grid[i].Length; j++)
        //            {
        //                grid[i][j] = 0;
        //            }
        //        }
        //
        //        if(deltaX > 0)
        //        {
        //            if(deltaY > 0)
        //            {
        //                for(int i = 0; i < first.SizeY; i++)
        //                {
        //                    for(int j = 0; j < first.SizeX; j++)
        //                    {
        //                        var value = first[i,j];
        //                        if(value == 3 || value == 5 || value == 6)
        //                        {
        //                            grid[i][j] += 1;
        //                        }
        //                    }
        //                }
        //                for(int i = second.SizeY - 1, x = 1; i > 0; i--, x++)
        //                {
        //                    for(int j = second.SizeX - 1, y = 1; j > 0; j--, y++)
        //                    {
        //                        var value = second[i,j];
        //                        if (value == 3 || value == 5 || value == 6)
        //                        {
        //                            grid[relativeDeltaX - x][relativeDeltaY - y] += 1;
        //                        }
        //                    }
        //                }
        //
        //                //File.WriteAllLines(@"C:\Users\benia\Desktop\array.txt", grid.Select(l => string.Join(' ', l)));
        //
        //                return grid.Any(r => r.Any(c => c == 2));
        //            }
        //            else
        //            {
        //
        //            }
        //        }
        //        else
        //        {
        //
        //        }
        //
        //        if (isColliding)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

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
