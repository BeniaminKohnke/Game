namespace GameAPI
{
    public class Player : GameObject
    {
        public Player(GridLoader loader, int x, int y) : base(loader, x, y, Types.Player, Grids.Player)
        {

        }
    }
}
