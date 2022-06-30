using System.Text;

namespace Game.DSL
{
    public static class CodeBuilder
    {
        public static readonly string ScriptsFolderPath = $@"{Directory.GetCurrentDirectory()}\Scripts";
        public static readonly string CallOrderFilePath = $@"{Directory.GetCurrentDirectory()}\Scripts\CallOrder.txt";
        private static string AddToDynamicObjects(string name, string type, string obj, string tabs) =>
            $@"{tabs}if(!parameters.ContainsKey(name)){"\n"}{tabs}{"{"}{"\n\t"}{tabs}parameters.DynamicObjects.Add({name}, ({type}, {obj}));{"\n"}{tabs}{"}"}";

        static CodeBuilder()
        {
            if(!Directory.Exists(ScriptsFolderPath))
            {
                Directory.CreateDirectory(ScriptsFolderPath);
            }
        }

        public static string[] GetCallOrder() => File.ReadAllLines(CallOrderFilePath).Select(l => l.Replace("()", string.Empty)).ToArray();

        public static bool CompileToCSharp(string code)
        {
            var scriptName = GetScriptName(code);

            var builder = new StringBuilder();

            builder.AppendLine("using GameAPI;\n");
            builder.AppendLine("namespace Game.DSL");
            builder.AppendLine("{");
            builder.AppendLine($"\tpublic class {scriptName}Script : IPlayerScript");
            builder.AppendLine("\t{");
            builder.AppendLine($"\t\tpublic {scriptName}Script()");
            builder.AppendLine("\t\t{");
            builder.AppendLine("\t\t}\n");

            builder.AppendLine($"\t\tpublic void Invoke(GameWorld gameWorld, Parameters parameters)");
            builder.AppendLine("\t\t{");

            //test
            builder.AppendLine("\t\t\tvar name = \"testObject\";");
            builder.AppendLine("\t\t\tvar type = GameAPI.ObjectsTypes.None;");
            builder.AppendLine("\t\t\tvar obj = new GameObject();");
            builder.AppendLine($"{AddToDynamicObjects("name", "type", "obj", "\t\t\t")}");
            
            builder.AppendLine("\t\t}");
            builder.AppendLine("\t}");
            builder.AppendLine("}");

            var filePath = $@"{ScriptsFolderPath}\{scriptName}.cs";
            if(File.Exists(filePath))
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
