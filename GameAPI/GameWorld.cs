namespace GameAPI
{
    public class GameWorld
    {
        public readonly List<GameObject> GameObjects;
        public readonly Player Player;

        public GameWorld()
        {
            GameObjects = new();
            Player = new(0, 0)
            {
                MovementSpeed = 1f,
                Weight = 70,
                ObjectType = ObjectsTypes.Player,
                State = States.NoAction1,
                TextureType = TexturesTypes.Player,
            };

            var tree = new GameObject(10, 20)
            {
                Weight = 1000,
                ObjectType = ObjectsTypes.Tree,
                TextureType = TexturesTypes.Tree1,
                State = States.NoAction1,
            };

            var tree2 = new GameObject(30, 50)
            {
                Weight = 1000,
                ObjectType = ObjectsTypes.Tree,
                TextureType = TexturesTypes.Tree1,
                State = States.NoAction1,
            };

            var rock1 = new GameObject(20, -30)
            {
                Weight = 30,
                ObjectType = ObjectsTypes.Rock,
                TextureType = TexturesTypes.Rock1,
                State = States.NoAction1,
            };
            GameObjects.Add(tree);
            GameObjects.Add(tree2);
            GameObjects.Add(rock1);
            GameObjects.Add(Player);
        }
    }
}
