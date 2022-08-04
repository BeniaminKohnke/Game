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

        public static void Use(Player player, object item)
        {
            if (item is GameObject go)
            {
                if (go.ObjectType == Types.Item)
                {

                }
            }

            if (item is int id)
            {

            }

            if (item is string name)
            {

            }
        }

        public static List<GameObject> ScanArea(GameWorld gameWorld) => gameWorld.GetObjects(GetObjectsOptions.FromPlayer);
    }
}
