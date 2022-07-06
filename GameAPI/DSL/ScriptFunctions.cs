using System.Drawing;

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
                var deltaX = gameWorld.Player.X - go.X;
                var deltaY = gameWorld.Player.Y - go.Y;

                if((deltaX * deltaX) + (deltaY * deltaY) <= squaredRadius)
                {
                    objects.Add(go);
                }
            }

            return objects;
        }

            //Math.Sqrt(Math.Pow(gameWorld.Player.X - go.X, 2) + Math.Pow(gameWorld.Player.RelativeY - go.RelativeY, 2)
    }
}
