using GameAPI;
using System;
using System.Collections.Generic;
namespace GameAPI.DSL
{
public class Move : IPlayerScript{
public void Run(GameWorld gameWorld, Dictionary<string, object> parameters, float deltaTime){
ScriptFunctions.Move(Directions.Up,gameWorld,parameters,deltaTime);}}}
