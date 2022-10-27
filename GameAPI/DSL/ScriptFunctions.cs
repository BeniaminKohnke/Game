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

        public static void Use(object item, GameWorld gameWorld, Dictionary<string, object> parameters, float deltaTime)
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

        public static void Move(object direction, GameWorld gameWorld, Dictionary<string, object> parameters, float deltaTime)
        {
            if (direction is string text)
            {
                switch (text)
                {
                    case "North":
                        gameWorld.Player.EnqueueMovement(Directions.Up);
                        break;
                    case "South":
                        gameWorld.Player.EnqueueMovement(Directions.Down);
                        break;
                    case "West":
                        gameWorld.Player.EnqueueMovement(Directions.Left);
                        break;
                    case "East":
                        gameWorld.Player.EnqueueMovement(Directions.Right);
                        break;
                }
            }
        }

        public static object DistanceBetween(object first, object second, GameWorld gameWorld, Dictionary<string, object> parameters, float deltaTime)
        {
            if (first is GameObject firstObject && second is GameObject secondObject)
            {
                return Math.Sqrt(Math.Pow(firstObject.Position.x - secondObject.Position.x, 2) + Math.Pow(firstObject.Position.y - secondObject.Position.y, 2)); 
            }
            return "NaN"; 
        }

        public static object RangeOf(object gameObject, GameWorld gameWorld, Dictionary<string, object> parameters, float deltaTime)
        {
            if (gameObject is GameObject go && go.ObjectParameters.TryGetValue(ObjectsParameters.ScanRadius, out var value))
            {
                return value;
            }

            return "NaN";
        }

        public static object ScanArea(GameWorld gameWorld, Dictionary<string, object> parameters, float deltaTime) => gameWorld.GetObjects(GetObjectsOptions.FromPlayer) ?? new List<GameObject>();
    }
}
