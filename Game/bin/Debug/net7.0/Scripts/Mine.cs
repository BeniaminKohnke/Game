using GameAPI;
using System;
using System.Collections.Generic;
namespace GameAPI.DSL
{
public class Mine : IPlayerScript{
public void Run(GameWorld gameWorld, Dictionary<string, object> parameters, float deltaTime){
object scannedObjects = ScriptFunctions.ScanArea(gameWorld,parameters,deltaTime);
object storedObject = (object)0;
object stObjDis = ScriptFunctions.DistanceBetween(gameWorld.Player,storedObject,gameWorld,parameters,deltaTime);
foreach( var sObject in (scannedObjects as IEnumerable<object> ?? new[]{scannedObjects}) )
{  object objDis = ScriptFunctions.DistanceBetween(gameWorld.Player,sObject,gameWorld,parameters,deltaTime);
  if( CodeBuilder.Is(sObject,Types.Rock) && CodeBuilder.LessThan(objDis,stObjDis) )
  {  storedObject = sObject;
    stObjDis = objDis;}}
if( CodeBuilder.Is(storedObject,Types.Rock) && CodeBuilder.LessThan(stObjDis,(object)5) ) 
{  foreach( var item in (gameWorld.Player.Items as IEnumerable<object> ?? new[]{gameWorld.Player.Items}) )
  {  if( CodeBuilder.Is(item,(object)"Pickaxe") )
    {  ScriptFunctions.Use(item,gameWorld,parameters,deltaTime);
      break;}}}
else
{  ScriptFunctions.GoTo(ScriptFunctions.DirectionBetween(gameWorld.Player,storedObject,gameWorld,parameters,deltaTime),gameWorld,parameters,deltaTime);}
}}}
