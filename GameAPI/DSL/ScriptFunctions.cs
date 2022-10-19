using GameAPI.GameObjects;

namespace GameAPI.DSL
{
    public static class ScriptFunctions
    {
        private static readonly string s_playerNotesFolder = $@"{Directory.GetCurrentDirectory()}\Notes";
        public static GameWorld? GameWorld { get; set; }

        static ScriptFunctions()
        {
            if (!Directory.Exists(s_playerNotesFolder))
            {
                Directory.CreateDirectory(s_playerNotesFolder);
            }
        }

        public static void SaveNote(object name, object content)
        {
            if (name is string && content is string)
            {
                File.WriteAllText($@"{s_playerNotesFolder}\{name as string}.txt", content as string);
            }
        }

        public static string LoadNote(object name) => name is string ?
            File.Exists($@"{s_playerNotesFolder}\{name as string}.txt") ?
                File.ReadAllText($@"{s_playerNotesFolder}\{name as string}.txt")
                : string.Empty
            : string.Empty;

        public static void Use(object item)
        {
            if (item is GameObject go)
            {
                if (go.ObjectType == Types.Item)
                {
                    //GameWorld?.Player.
                }
            }

            if (item is int id)
            {

            }

            if (item is string name)
            {

            }
        }

        public static object DistanceBetween(object first, object second)
        {
            if (first is GameObject firstObject && second is GameObject secondObject)
            {
                return Math.Sqrt(Math.Pow(firstObject.Position.x - secondObject.Position.x, 2) + Math.Pow(firstObject.Position.y - secondObject.Position.y, 2)); 
            }
            return "NaN"; 
        }

        public static List<GameObject> ScanArea(GameWorld gameWorld) => gameWorld.GetObjects(GetObjectsOptions.FromPlayer);
    }
}
