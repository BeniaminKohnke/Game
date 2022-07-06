using GameAPI;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GameAPI.DSL
{
	public class TestScript : PlayerScript
	{
		public TestScript()
		{
		}

		protected override void Do(GameWorld gameWorld, ConcurrentDictionary<string, (Types, object)> parameters)
		{
			var name = "testObject";
			var type = Types.None;
			var obj = new GameObject();
			if(!parameters.ContainsKey(name))
			{
				parameters[name] = (type, obj);
			}
		}
	}
}
