namespace GameAPI
{
    public class GameWorld
    {
        public readonly List<GameObject> GameObjects;
        public readonly Player Player;

        public GameWorld()
        {
            GameObjects = new();
            Player = new()
            {
                ObjectType = ObjectsTypes.Player,
                State = States.NoAction1,
                TextureType = TexturesTypes.Player,
            };

            var tree = new GameObject
            {
                PositionX = 200,
                PositionY = 400,
                ObjectType = ObjectsTypes.Tree,
                TextureType = TexturesTypes.Tree1,
                State = States.NoAction1,
            };

            GameObjects.Add(tree);
            GameObjects.Add(Player);
        }
    }
}
