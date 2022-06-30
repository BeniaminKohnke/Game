namespace GameAPI
{
    public class GameWorld
    {
        public readonly List<GameObject> GameObjects;
        public readonly Player Player;

        public GameWorld()
        {
            GameObjects = new();
            Player = new();

            GameObjects.Add(Player);
        }
    }
}
