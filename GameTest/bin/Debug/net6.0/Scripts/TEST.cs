using GameAPI;

namespace Game.DSL
{
	public class TestScript : IPlayerScript
	{
		public TestScript()
		{
		}

		public void Invoke(GameWorld gameWorld, Parameters parameters)
		{
			var name = "testObject";
			var type = GameAPI.ObjectsTypes.None;
			var obj = new GameObject();
			if(!parameters.ContainsKey(name))
			{
				parameters.DynamicObjects.Add(name, (type, obj));
			}
		}
	}
}
