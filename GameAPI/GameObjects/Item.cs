namespace GameAPI.GameObjects
{
    public class Item : GameObject
    {
        private double _nextStateCounter = 0d;
        private double _animationTime = 1d;
        private byte _uses = 0;
        public string Name { get; set; } = string.Empty;
        public byte Uses
        {
            get => _uses;
            set
            {
                if (value > _uses)
                {
                    if (_animationTime >= 1d)
                    {
                        _animationTime = 0d;
                    }
                }
                else
                {
                    _uses = value;
                }
            }
        }
        public ItemTypes ItemType { get; set; } = ItemTypes.None;
        public Item(GridLoader loader, int x, int y, Types type, Grids grid) : base(loader, x, y, type, grid)
        {
        }

        public override void Update(double deltaTime, GridLoader loader)
        {
            if (_animationTime < 1d)
            {
                _animationTime += deltaTime;
                _nextStateCounter += deltaTime;
                if (_nextStateCounter >= 0.25d)
                {
                    switch (LastDirection)
                    {
                        case Directions.Up:
                            ChangeState(Animations.MovingRight);
                            break;
                        case Directions.Down:
                            ChangeState(Animations.MovingLeft);
                            break;
                        case Directions.Left:
                            ChangeState(Animations.MovingLeft);
                            break;
                        case Directions.Right:
                            ChangeState(Animations.MovingRight);
                            break;
                        case Directions.None:
                            ChangeState(Animations.NoAction);
                            break;
                    }
                    _nextStateCounter = 0d;
                }

                if (_animationTime >= 0.9d)
                {
                    _uses++;
                    _animationTime = 1d;
                }
            }
            else
            {
                _nextStateCounter = 0d;
                ChangeState(Animations.NoAction);
            }

            void ChangeState(Animations animation)
            {
                SetGrid(loader.GetGrid(Grid, State));
                TrySetNextState(animation);
            }
        }
    }
}
