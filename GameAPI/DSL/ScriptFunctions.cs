namespace GameAPI.DSL
{
    public static class ScriptFunctions
    {
        private static readonly string s_playerNotesFolder = $@"{Directory.GetCurrentDirectory()}\Notes";

        static ScriptFunctions()
        {
            if(!Directory.Exists(s_playerNotesFolder))
            {
                Directory.CreateDirectory(s_playerNotesFolder);
            }
        }

        public static void SaveNote(string name, string content) => File.WriteAllText($@"{s_playerNotesFolder}\{name}.txt", content);
        public static string LoadNote(string name) => File.Exists($@"{s_playerNotesFolder}\{name}.txt") ? File.ReadAllText($@"{s_playerNotesFolder}\{name}.txt") : string.Empty;

        public static List<GameObject> ScanArea(GameWorld gameWorld, int radius)
        {
            var objects = new List<GameObject>();
            var squaredRadius = radius * radius;
            foreach(var go in gameWorld.GameObjects)
            {
                var deltaX = gameWorld.Player.Position.x - go.Position.x;
                var deltaY = gameWorld.Player.Position.y - go.Position.y;

                if((deltaX * deltaX) + (deltaY * deltaY) <= squaredRadius)
                {
                    objects.Add(go);
                }
            }

            return objects;
        }
    }
}
