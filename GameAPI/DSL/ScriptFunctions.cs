using Aardvark.Base;
using GameAPI.GameObjects;

namespace GameAPI.DSL
{
    public static class ScriptFunctions
    {
        private static readonly string s_playerNotesFolder = $@"{Directory.GetCurrentDirectory()}\Notes";

        static ScriptFunctions()
        {
            if (!Directory.Exists(s_playerNotesFolder))
            {
                Directory.CreateDirectory(s_playerNotesFolder);
            }
        }

        public static object Use(object item, GameWorld gameWorld, Dictionary<string, object> parameters, float deltaTime)
        {
            if (item is Item go)
            {
                gameWorld.Player.SetSelctedItem((byte)(gameWorld.Player.Items.IndexOf(go) + 1));
                gameWorld.Player.IncreaseItemUses();
            }

            if (item is byte id)
            {
                gameWorld.Player.SetSelctedItem(id);
                gameWorld.Player.IncreaseItemUses();
            }

            if (item is string name)
            {
                var i = gameWorld.Player.Items.FirstOrDefault(i => i.Name.Equals(name));
                if (i != null)
                {
                    gameWorld.Player.SetSelctedItem((byte)gameWorld.Player.Items.IndexOf(i));
                    gameWorld.Player.IncreaseItemUses();
                }
            }

            return 0;
        }

        public static object DirectionBetween(object first, object second, GameWorld gameWorld, Dictionary<string, object> parameters, float deltaTime)
        {
            if (first is GameObject fo && second is GameObject so)
            {
                var x = so.Position.x - fo.Position.x;
                var y = so.Position.y - fo.Position.y;

                if (Math.Abs(x) > Math.Abs(y))
                {
                    return x > 0 ? Directions.East : Directions.West;
                }
                else
                {
                    if (y != 0)
                    {
                        return y > 0 ? Directions.South : Directions.North;
                    }
                }
            }

            return Directions.None;
        }

        public static object GoTo(object direction, GameWorld gameWorld, Dictionary<string, object> parameters, float deltaTime)
        {
            if (direction is Directions dir)
            {
                gameWorld.Player.EnqueueMovement(dir);
            }

            return 0;
        }

        public static object DistanceBetween(object first, object second, GameWorld gameWorld, Dictionary<string, object> parameters, float deltaTime)
        {
            switch (first)
            {
                case GameObject fo:
                    switch (second)
                    {
                        case GameObject so:
                            var x = (int)Math.Sqrt(Math.Pow(fo.Position.x - so.Position.x, 2) + Math.Pow(fo.Position.y - so.Position.y, 2));
                            return x;
                        case Tuple<int, int> sp:
                            x = (int)Math.Sqrt(Math.Pow(fo.Position.x - sp.Item1, 2) + Math.Pow(fo.Position.y - sp.Item2, 2));
                            return x;
                    }
                    break;
                case Tuple<int, int> fp:
                    switch (second)
                    {
                        case GameObject so:
                            return (int)Math.Sqrt(Math.Pow(fp.Item1 - so.Position.x, 2) + Math.Pow(fp.Item2 - so.Position.y, 2));
                        case Tuple<int, int> sp:
                            return (int)Math.Sqrt(Math.Pow(fp.Item1 - sp.Item1, 2) + Math.Pow(fp.Item2 - sp.Item2, 2));
                    }
                    break;
            }

            return int.MaxValue; 
        }

        public static object RangeOf(object gameObject, GameWorld gameWorld, Dictionary<string, object> parameters, float deltaTime)
        {
            if (gameObject is GameObject go && go.ObjectParameters.TryGetValue(ObjectsParameters.MeleeRange, out var value))
            {
                return value;
            }

            return 0;
        }

        public static object CraftItem(object item, GameWorld gameWorld, Dictionary<string, object> parameters, float deltaTime)
        {
            var wasCreated = false;
            if (item is string name)
            {
                var t = Enum.GetValues(typeof(Items)).Cast<Items>().FirstOrDefault(i => i.ToString().Equals(name));
                wasCreated = Crafting.CraftItem(gameWorld, t);
            }
            else if (item is Items type)
            {
                wasCreated = Crafting.CraftItem(gameWorld, type);
            }

            return wasCreated ? "Success" : "Failure";
        }

        public static object Count(object collection, object element, GameWorld gameWorld, Dictionary<string, object> parameters, float deltaTime)
        {
            var count = 0;
            if (collection is IEnumerable<object> coll)
            {
                foreach (var el in coll)
                {
                    switch (el)
                    {
                        case Item fi:
                            switch (element)
                            {
                                case Items sii:
                                    if (fi.Name == sii)
                                    {
                                        count++;
                                    }
                                    break;
                                case Types st:
                                    if (fi.ObjectType == st)
                                    {
                                        count++;
                                    }
                                    break;
                                case Item si:
                                    if (fi.ItemType == si.ItemType)
                                    {
                                        count++;
                                    }
                                    break;
                                case GameObject so:
                                    if (fi.ObjectType == so.ObjectType)
                                    {
                                        count++;
                                    }
                                    break;
                            }
                            break;
                        case GameObject fo:
                            switch (element)
                            {
                                case Types st:
                                    if (fo.ObjectType == st)
                                    {
                                        count++;
                                    }
                                    break;
                                case Item si:
                                    if (fo.ObjectType == si.ObjectType)
                                    {
                                        count++;
                                    }
                                    break;
                                case GameObject so:
                                    if (fo.ObjectType == so.ObjectType)
                                    {
                                        count++;
                                    }
                                    break;
                            }
                            break;
                    } 
                }
            }

            return count;
        }

        public static object CallScript(object name, GameWorld gameWorld, Dictionary<string, object> parameters, float deltaTime)
        {
            if (name is string scriptName)
            {
                if (parameters.TryGetValue($"Scripts.{scriptName}", out var script) && script is IPlayerScript playerScript)
                {
                    playerScript.Run(gameWorld, parameters, deltaTime);
                }
            }
            return 0;
        }

        public static object ScanArea(GameWorld gameWorld, Dictionary<string, object> parameters, float deltaTime) 
            => gameWorld.GetObjects(GetObjectsOptions.FromPlayer | GetObjectsOptions.OnlyActive | GetObjectsOptions.RemovePlayer).Select(o => (object)o) ?? Array.Empty<object>();
    }
}
