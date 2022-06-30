using System.Text;

namespace Game.DSL
{
    public static class CodeBuilder
    {
        public static readonly string ScriptsFolderPath = $@"{Directory.GetCurrentDirectory()}\Scripts";
        public static readonly string CallOrderFilePath = $@"{Directory.GetCurrentDirectory}\Scripts\CallOrder.txt";
        private static readonly string _addToDynamicObjectsScriptLine = "parameters.DynamicObjects.Add(name, (type, obj));";

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
            builder.AppendLine($"\tpublic class {scriptName}Class");
            builder.AppendLine("\t{");
            builder.AppendLine($"\t\tpublic {scriptName}Class()");
            builder.AppendLine("\t\t{");
            builder.AppendLine("\t\t}\n");

            builder.AppendLine($"\t\tpublic void {scriptName}()");
            builder.AppendLine("\t\t{");

            //test
            builder.AppendLine("\t\t\tvar name = \"testObject\";");
            builder.AppendLine("\t\t\tvar type = ObjectsTypes.None");
            builder.AppendLine("\t\t\tvar obj = new GameObject()");
            builder.AppendLine($"\t\t\t{_addToDynamicObjectsScriptLine}");
            
            builder.AppendLine("\t\t}");
            builder.AppendLine("\t}");
            builder.AppendLine("}");

            File.WriteAllText(@$"{ScriptsFolderPath}\{scriptName}.cs", builder.ToString());

            return true;
        }

        private static string GetScriptName(string code)
        {
            return code;
        }
    }
}
