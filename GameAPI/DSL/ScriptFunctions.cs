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

        public static void SaveNote(object name, object content, GameWorld gameWorld, Dictionary<string, object> parameters, float deltaTime)
        {
            if (name is string && content is string)
            {
                File.WriteAllText($@"{s_playerNotesFolder}\{name as string}.txt", content as string);
            }
        }

        public static string LoadNote(object name, GameWorld gameWorld, Dictionary<string, object> parameters, float deltaTime) => name is string ?
            File.Exists($@"{s_playerNotesFolder}\{name as string}.txt") ?
                File.ReadAllText($@"{s_playerNotesFolder}\{name as string}.txt")
                : string.Empty
            : string.Empty;

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
                    return x > 0 ? Directions.Right : Directions.Left;
                }
                else
                {
                    if (y != 0)
                    {
                        return y > 0 ? Directions.Down : Directions.Up;
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
                            return (int)Math.Sqrt(Math.Pow(fo.Position.x - so.Position.x, 2) + Math.Pow(fo.Position.y - so.Position.y, 2));
                        case Tuple<int, int> sp:
                            return (int)Math.Sqrt(Math.Pow(fo.Position.x - sp.Item1, 2) + Math.Pow(fo.Position.y - sp.Item2, 2));
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

        public static object ScanArea(GameWorld gameWorld, Dictionary<string, object> parameters, float deltaTime) => gameWorld.GetObjects(GetObjectsOptions.FromPlayer).Select(o => (object)o) ?? Array.Empty<object>();
    }
}
