namespace GameAPI.GameObjects
{
    public class Item : GameObject
    {
        private double _lastUpdate = 0f;
        public string Name { get; set; } = string.Empty;
        public byte Uses { get; set; } = 0;
        public ItemTypes ItemType { get; set; } = ItemTypes.None;
        public Item(GridLoader loader, int x, int y, Types type, Grids grid) : base(loader, x, y, type, grid)
        {
        }

        public override void Update(double deltaTime, GridLoader loader)
        {
            _lastUpdate += deltaTime;
            if (_lastUpdate >= 0.3f)
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
                _lastUpdate = 0f;
            }

            void ChangeState(Animations animation)
            {
                if (Uses > 0)
                {
                    SetGrid(loader.GetGrid(Grid, State));
                    TrySetNextState(animation);
                }
                else
                {
                    IsActive = false;
                }
            }
        }
    }
}
