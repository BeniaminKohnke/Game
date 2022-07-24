namespace GameAPI
{
    public class Item : GameObject
    {
        public bool IsUsed { get; set; } = false;
        public ItemTypes ItemType { get; set; } = ItemTypes.None;

        public Item(GridLoader loader, int x, int y, Types type, Grids grid) : base(loader, x, y, type, grid)
        {
        }
    }
}
