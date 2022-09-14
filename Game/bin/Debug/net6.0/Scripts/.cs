using GameAPI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GameAPI.DSL
{
	public class  : PlayerScript
	{
		public ()
		{
		}

		protected override void Do(GameWorld gameWorld, ConcurrentDictionary<string, object> parameters)
		{
			gameWorld.Player.EnqueueMovement((Directions)new Random().Next(0, 5));
		}
	}
}
