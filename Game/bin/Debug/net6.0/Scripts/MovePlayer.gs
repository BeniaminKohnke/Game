using GameAPI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GameAPI.DSL
{
	public class MovePlayer : PlayerScript
	{
		public MovePlayer()
		{
		}

		protected override void Do(GameWorld gameWorld, ConcurrentDictionary<string, object> parameters)
		{
			gameWorld.Player.EnqueueMovement((Directions)new Random().Next(0, 5));
		}
	}
}
