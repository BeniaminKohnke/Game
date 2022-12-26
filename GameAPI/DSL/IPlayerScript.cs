namespace GameAPI.DSL
{
    public interface IPlayerScript
    {
        public void Run(GameWorld gameWorld, Dictionary<string, object> parameters, float deltaTime);
    }
}
