namespace GameAPI.GameObjects
{
    public sealed class Player : GameObject
    {
        private float _movementUpdate = 0f;
        public readonly uint[] ItemsMenu = new uint[10];
        public uint SelectedItemId { get; set; } = 0;
        public byte SelectedPosition { get; set; } = 0;
        public List<Item> Items { get; } = new();
        public bool IsMoving { get; set; } = false;
        
        public Player(int x, int y) : base(x, y, Types.Player, Grids.Player)
        {
            LastDirection = Directions.East;

            for (var i = 0; i < 15; i++)
            {
                Items.Add(new(x, y, Types.Item, Grids.ItemRock)
                {
                    IsActive = false,
                    Name = GameAPI.Items.Rock,
                    ItemType = ItemTypes.Material,
                });
            }

            for (var i = 0; i < 9 ; i++)
            {
                Items.Add(new(x, y, Types.Item, Grids.ItemString)
                {
                    IsActive = false,
                    Name = GameAPI.Items.String,
                    ItemType = ItemTypes.Material,
                });
            }

            for (var i = 0; i < 12; i++)
            {
                Items.Add(new(x, y, Types.Item, Grids.ItemStick)
                {
                    IsActive = false,
                    Name = GameAPI.Items.Stick,
                    ItemType = ItemTypes.Material,
                });
            }
        }

        public void IncreaseItemUses()
        {
            var item = Items.FirstOrDefault(i => i.Id == SelectedItemId);
            if (item != null)
            {
                item.Uses++;
                if (item.ItemType == ItemTypes.Melee || item.ItemType == ItemTypes.Ranged)
                {
                    item.IsActive = true;
                }
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
            if (IsActive && _movementUpdate > 0.02f)
            {
                base.EnqueueMovement(direction);
                _movementUpdate = 0f;
            }
        }

        public override void Update(float deltaTime)
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
                    case Directions.North:
                        ResetState(Animations.MovingRight, 2);
                        break;
                    case Directions.South:
                        ResetState(Animations.MovingLeft, 2);
                        break;
                    case Directions.West:
                        ResetState(Animations.MovingLeft, 2);
                        break;
                    case Directions.East:
                        ResetState(Animations.MovingRight, 2);
                        break;
                }
            }
        }
    }
}
