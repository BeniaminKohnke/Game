using System.Collections.Concurrent;

namespace GameAPI
{
    public class Player : GameObject
    {
        public ushort SelectedItem { get; set; } = 0;
        public ConcurrentBag<Item> Items { get; } = new(); 

        public Player(GridLoader loader, int x, int y) : base(loader, x, y, Types.Player, Grids.Player)
        {
        }
    }
}
