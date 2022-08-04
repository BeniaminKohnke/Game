namespace GameAPI.GameObjects
{
    public class Item : GameObject
    {
        private byte _frameCounter = 0;
        private Directions _lastDirection;
        private double _lastChange = 0f;

        public byte Uses { get; set; } = 0;
        public bool IsUsed { get; set; } = false;
        public ItemTypes ItemType { get; set; } = ItemTypes.None;

        public override Directions LastDirection
        {
            get => _lastDirection;
            set
            {
                _lastDirection = value;
                switch (LastDirection)
                {
                    case Directions.Up:
                        ResetState(Animations.MovingRight);
                        break;
                    case Directions.Down:
                        ResetState(Animations.MovingLeft);
                        break;
                    case Directions.Left:
                        ResetState(Animations.MovingLeft);
                        break;
                    case Directions.Right:
                        ResetState(Animations.MovingRight);
                        break;
                }
            }
        }

        public Item(GridLoader loader, int x, int y, Types type, Grids grid) : base(loader, x, y, type, grid)
        {
        }

        public override void Update(double deltaTime)
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
            }

            void ChangeState(Animations animation)
            {
                _lastChange += deltaTime;
                if (_lastChange > 0.3f)
                {
                    if (IsUsed)
                    {
                        if (TrySetNextState(animation))
                        {
                            _frameCounter++;
                        }
                    }
                    else
                    {
                        ResetState(animation);
                        _frameCounter = 0;
                    }
                    _lastChange = 0;
                }
            }

            if (_frameCounter == 3)
            {
                _frameCounter = 0;
                Uses++;
            }
        }
    }
}
