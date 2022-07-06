namespace GameAPI
{
    public class Player : GameObject
    {
        public Player(GridLoader loader, long x, long y) : base(loader, x, y, Types.Player, Grids.Player)
        {

        }
    }
}
