namespace GameAPI
{
    public class GameWorld
    {
        public ShapeLoader ShapeLoader { get; } = new();
        public readonly List<GameObject> GameObjects;
        public readonly Player Player;

        public GameWorld()
        {
            ShapeLoader.LoadShapes();
            GameObjects = new();

            Player = new(0, 0, ShapeLoader.Shapes[Shapes.Player][States.NoAction1])
            {
                MovementSpeed = 1,
                Weight = 70,
                ObjectType = Types.Player,
                State = States.NoAction1,
                Shape = Shapes.Player,
            };

            var tree = new GameObject(20, 20, ShapeLoader.Shapes[Shapes.Tree1][States.NoAction1])
            {
                Weight = 1000,
                ObjectType = Types.Tree,
                Shape = Shapes.Tree1,
                State = States.NoAction1,
            };

            var tree2 = new GameObject(40, 40, ShapeLoader.Shapes[Shapes.Tree1][States.NoAction1])
            {
                Weight = 1000,
                ObjectType = Types.Tree,
                Shape = Shapes.Tree1,
                State = States.NoAction1,
            };

            GameObjects.Add(tree);
            GameObjects.Add(tree2);
            GameObjects.Add(Player);
        }

        public void Update()
        {
            HandleCollisions();
        }

        private void HandleCollisions()
        {
            for (int i = 0; i < GameObjects.Count; i++)
            {
                while (GameObjects[i].Movement.TryDequeue(out var direction))
                {
                    var predictedPositionX = GameObjects[i].GridPositionX;
                    var predictedPositionY = GameObjects[i].GridPositionY;
                    var mainObjectShape = GameObjects[i].ShapeData;

                    switch (direction)
                    {
                        case Directions.Up:
                            predictedPositionY -= GameObjects[i].MovementSpeed;
                            break;
                        case Directions.Down:
                            predictedPositionY += GameObjects[i].MovementSpeed;
                            break;
                        case Directions.Left:
                            predictedPositionX -= GameObjects[i].MovementSpeed;
                            break;
                        case Directions.Right:
                            predictedPositionX += GameObjects[i].MovementSpeed;
                            break;
                    }

                    var canMove = true;
                    for (int j = i + 1; j < GameObjects.Count; j++)
                    {
                        if (HandleCollision(GameObjects[i], GameObjects[j], predictedPositionX, predictedPositionY))
                        {
                            canMove = false;
                            break;
                        }
                    }

                    if (canMove)
                    {
                        GameObjects[i].GridPositionX = predictedPositionX;
                        GameObjects[i].GridPositionY = predictedPositionY;
                    }
                }
            }
        }

        private bool HandleCollision(GameObject first, GameObject second, long newX, long newY)
        {
            var isColliding = false;

            var mainX = newX + first.ShapeData[0].Length / 2;
            var mainY = newY + first.ShapeData.Length / 2;

            var otherX = second.GridPositionX + second.ShapeData[0].Length / 2;
            var otherY = second.GridPositionY + second.ShapeData.Length / 2;

            var deltaX = otherX - mainX;
            var deltaY = otherY - mainY;
            if (Math.Abs(deltaX) <= (first.ShapeData[0].Length / 2 + second.ShapeData[0].Length / 2)
                && Math.Abs(deltaY) <= (first.ShapeData.Length / 2 + second.ShapeData.Length / 2))
            {
                long x1, y1, x2, y2;

                if (deltaX > 0)
                {
                    x1 = newX;
                    x2 = second.GridPositionX + second.ShapeData[0].Length;
                }
                else
                {
                    x1 = newX + first.ShapeData[0].Length;
                    x2 = second.GridPositionX;
                }

                if (deltaY > 0)
                {
                    y1 = newY;
                    y2 = second.GridPositionY + second.ShapeData.Length;
                }
                else
                {
                    y1 = newY + first.ShapeData.Length;
                    y2 = second.GridPositionY;
                }

                var relativeDeltaX = x2 - x1;
                var relativeDeltaY = y2 - y1;

                var grid = new byte[relativeDeltaX][];
                for(int i = 0; i < grid.Length; i++)
                {
                    grid[i] = new byte[relativeDeltaY];
                    for(int j = 0; j < grid[i].Length; j++)
                    {
                        grid[i][j] = 0;
                    }
                }

                if(deltaX > 0)
                {
                    if(deltaY > 0)
                    {
                        for(int i = 0; i < first.ShapeData.Length; i++)
                        {
                            for(int j = 0; j < first.ShapeData[i].Length; j++)
                            {
                                var value = first.ShapeData[i][j];
                                if(value == 3 || value == 5 || value == 6)
                                {
                                    grid[i][j] += 1;
                                }
                            }
                        }
                        for(int i = second.ShapeData.Length - 1, x = 1; i > 0; i--, x++)
                        {
                            for(int j = second.ShapeData[i].Length - 1, y = 1; j > 0; j--, y++)
                            {
                                var value = second.ShapeData[i][j];
                                if (value == 3 || value == 5 || value == 6)
                                {
                                    grid[relativeDeltaX - x][relativeDeltaY - y] += 1;
                                }
                            }
                        }

                        File.WriteAllLines(@"C:\Users\benia\Desktop\array.txt", grid.Select(l => string.Join(' ', l)));

                        return grid.Any(r => r.Any(c => c == 2));
                    }
                    else
                    {

                    }
                }
                else
                {

                }

                if (isColliding)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
