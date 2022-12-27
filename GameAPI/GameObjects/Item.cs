namespace GameAPI.GameObjects
{
    public sealed class Item : GameObject
    {
        private byte _uses = 0;
        private float _nextStateCounter = 0f;
        private float _animationTime = 1f;
        public Items Name { get; set; } = Items.None;
        public byte Uses
        {
            get => _uses;
            set
            {
                if (value > _uses && ItemType != ItemTypes.Consumable)
                {
                    if (_animationTime >= 1f)
                    {
                        _animationTime = 0f;
                    }
                }
                else
                {
                    _uses = value;
                }
            }
        }
        public ItemTypes ItemType { get; set; } = ItemTypes.None;

        public Item(int x, int y, Types type, Grids grid) : base(x, y, type, grid)
        {
        }

        public override void Update(float deltaTime)
        {
            if (_animationTime < 1f)
            {
                _animationTime += deltaTime;
                _nextStateCounter += deltaTime;
                if (_nextStateCounter >= 0.25f)
                {
                    switch (LastDirection)
                    {
                        case Directions.North:
                            ChangeState(Animations.MovingRight);
                            break;
                        case Directions.South:
                            ChangeState(Animations.MovingLeft);
                            break;
                        case Directions.West:
                            ChangeState(Animations.MovingLeft);
                            break;
                        case Directions.East:
                            ChangeState(Animations.MovingRight);
                            break;
                        case Directions.None:
                            ChangeState(Animations.NoAction);
                            break;
                    }
                    _nextStateCounter = 0f;
                }

                if (_animationTime >= 0.9f)
                {
                    _uses++;
                    _animationTime = 1f;
                }
            }
            else
            {
                _nextStateCounter = 0f;
                ChangeState(Animations.NoAction);
            }

            void ChangeState(Animations animation)
            {
                SetGrid(GridLoader.GetGrid(Grid, State));
                TrySetNextState(animation);
            }
        }
    }
}
