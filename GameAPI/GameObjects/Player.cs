namespace GameAPI.GameObjects
{
    public class Player : GameObject
    {
        private float _movementUpdate = 0f;
        public readonly uint[] ItemsMenu = new uint[10];
        public uint SelectedItemId { get; set; } = 0;
        public byte SelectedPosition { get; set; } = 0;
        public List<Item> Items { get; } = new();
        public bool IsMoving { get; set; } = false;
        
        public Player(GridLoader loader, int x, int y) : base(loader, x, y, Types.Player, Grids.Player)
        {
            LastDirection = Directions.Right;
        }

        public void IncreaseItemUses()
        {
            var item = Items.FirstOrDefault(i => i.Id == SelectedItemId);
            if (item != null)
            {
                item.Uses++;
                item.IsActive = true;
            }
        }

        public void SetSelctedItem(byte position)
        {
            SelectedPosition = position;
            SelectedPosition--;
            SelectedItemId = ItemsMenu[SelectedPosition];
        }

        public override void EnqueueMovement(Directions direction)
        {
            if (_movementUpdate > 0.02f)
            {
                base.EnqueueMovement(direction);
                _movementUpdate = 0f;
            }
        }

        public override void Update(float deltaTime, GridLoader loader)
        {
            _movementUpdate += deltaTime;
            foreach (var item in Items)
            {
                if (item.IsActive)
                {
                    item.LastDirection = LastDirection;
                    item.Position = Position;
                }
            }

            if (!IsMoving)
            {
                switch (LastDirection)
                {
                    case Directions.Up:
                        ResetState(Animations.MovingRight, 2);
                        break;
                    case Directions.Down:
                        ResetState(Animations.MovingLeft, 2);
                        break;
                    case Directions.Left:
                        ResetState(Animations.MovingLeft, 2);
                        break;
                    case Directions.Right:
                        ResetState(Animations.MovingRight, 2);
                        break;
                }
            }
        }
    }
}
