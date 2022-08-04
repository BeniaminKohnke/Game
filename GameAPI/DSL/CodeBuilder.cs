using System.Text;

namespace GameAPI.DSL
{
    public static class CodeBuilder
    {
        public static string[] CallOrder { get; private set; }
        public static readonly string ScriptsFolderPath = $@"{Directory.GetCurrentDirectory()}\Scripts";
        public static readonly string CallOrderFilePath = $@"{Directory.GetCurrentDirectory()}\Scripts\CallOrder.txt";
        private static string AddToDynamicObjects(string name, string type, string obj, string tabs) =>
            $@"{tabs}if(!parameters.ContainsKey(name)){"\n"}{tabs}{"{"}{"\n\t"}{tabs}parameters[{name}] = ({type}, {obj});{"\n"}{tabs}{"}"}";

        static CodeBuilder()
        {
            if (!Directory.Exists(ScriptsFolderPath))
            {
                Directory.CreateDirectory(ScriptsFolderPath);
            }

            CallOrder = GetCallOrder();
        }

        public static string[] GetCallOrder() => File.ReadAllLines(CallOrderFilePath).Select(l => l.Replace("()", "Script")).ToArray();

        public static bool CompileToCSharp(string code)
        {
            var scriptName = GetScriptName(code);

            var builder = new StringBuilder();

            builder.AppendLine("using GameAPI;");
            builder.AppendLine("using System;");
            builder.AppendLine("using System.Collections.Concurrent;");
            builder.AppendLine("using System.Collections.Generic;\n");
            builder.AppendLine("namespace GameAPI.DSL");
            builder.AppendLine("{");
            builder.AppendLine($"\tpublic class {scriptName}Script : PlayerScript");
            builder.AppendLine("\t{");
            builder.AppendLine($"\t\tpublic {scriptName}Script()");
            builder.AppendLine("\t\t{");
            builder.AppendLine("\t\t}\n");

            builder.AppendLine($"\t\tprotected override void Do(GameWorld gameWorld, ConcurrentDictionary<string, object> parameters)");
            builder.AppendLine("\t\t{");

            //test
            //builder.AppendLine("\t\t\tvar name = \"testObject\";");
            //builder.AppendLine("\t\t\tvar type = Types.None;");
            //builder.AppendLine("\t\t\tvar obj = new GameObject();");
            //builder.AppendLine($"{AddToDynamicObjects("name", "type", "obj", "\t\t\t")}");

            builder.AppendLine("\t\t\tgameWorld.Player.EnqueueMovement((Directions)new Random().Next(0, 5));");

            builder.AppendLine("\t\t}");
            builder.AppendLine("\t}");
            builder.AppendLine("}");

            var filePath = $@"{ScriptsFolderPath}\{scriptName}.cs";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.WriteAllText(filePath, builder.ToString());

            return true;
        }

        private static string GetScriptName(string code)
        {
            return code;
        }
    }
}
