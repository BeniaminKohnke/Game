using System.Collections.Concurrent;

namespace GameAPI.GameObjects
{
    public class Player : GameObject
    {
        public readonly uint[] ItemsMenu = new uint[10];
        public uint SelectedItemId { get; set; } = 0;
        public byte SelectedPosition { get; set; } = 0;
        public ConcurrentBag<Item> Items { get; } = new();
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

        public void SetItemState(bool isUsed)
        {
            //var item = Items.FirstOrDefault(i => i.Id == SelectedItemId);
            //if (item != null)
            //{
            //    item.IsUsed = isUsed;
            //}
        }

        public override void Update(double deltaTime, GridLoader loader)
        {
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
