using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace GameAPI.DSL
{
    public static class CodeBuilder
    {
        private static readonly Dictionary<string, byte> _scriptFunctions = typeof(ScriptFunctions)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .ToDictionary(m => m.Name, p => (byte)p.GetParameters().Length);
        private static readonly Regex FOR_EACH_Regex = new(@"(\t*)FOR EACH ([a-z][a-zA-Z]+) FROM ([a-z][a-zA-Z]+) REPEAT", RegexOptions.Compiled);

        public static string[] CallOrder 
        { 
            get => File.ReadAllLines(CallOrderFilePath).Select(l => l.Replace("()", "Script")).ToArray();
            private set => File.WriteAllLines(CallOrderFilePath, value);
        }
        public static string ScriptsFolderPath { get; } = $@"{Directory.GetCurrentDirectory()}\Scripts";
        public static string CallOrderFilePath { get; } = $@"{Directory.GetCurrentDirectory()}\Scripts\CallOrder.txt";

        static CodeBuilder()
        {
            if (!Directory.Exists(ScriptsFolderPath))
            {
                Directory.CreateDirectory(ScriptsFolderPath);
            }
        }
        private static string AddToDynamicObjects(string name, string type, string obj, string tabs) =>
            $@"{tabs}if(!parameters.ContainsKey(name)){"\n"}{tabs}{"{"}{"\n\t"}{tabs}parameters[{name}] = ({type}, {obj});{"\n"}{tabs}{"}"}";

        public static void SaveScript(string scriptName, string code)
        {
            if (scriptName.Equals("CallOrder"))
            {
                CallOrder = code.Split("\n");
            }
            else
            {
                File.WriteAllText($@"{ScriptsFolderPath}\{scriptName}.gs", code);
            }
        }

        public static void DeleteScript(string scriptName)
        {
            var path = $@"{ScriptsFolderPath}\{scriptName}.gs";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static bool VefifyScript(string scriptName, string code)
        {
            if (scriptName.Equals("CallOrder"))
            {
                var scriptsNames = Directory.GetFiles(ScriptsFolderPath).Where(s => s.Contains(".gs")).Select(s => s.Replace($@"{ScriptsFolderPath}\", string.Empty).Replace(".gs", string.Empty));
                return code.Split("\n").All(l => scriptsNames.Contains(l));
            }

            return CompileToCSharp(scriptName, code);
        }

        public static (string name, string code)[] GetExistingScripts() => Directory
            .GetFiles(ScriptsFolderPath)
            .Where(s => s.Contains(".gs"))
            .Select(p => (p.Replace($@"{ScriptsFolderPath}\", string.Empty).Replace(".gs", string.Empty), File.ReadAllText(p)))
            .ToArray();

        public static bool CompileToCSharp(string scriptName, string code)
        {
            code = code
                .Replace(" Player ", " gameWorld.Player ")
                .Replace("SAVE TO", "SAVE_TO")
                .Replace("FOR SINGLE", "FOR_SINGLE")
                .Replace("LESS THAN", "LESS_THAN")
                .Replace("MORE THAN", "MORE_THAN")
                .Replace("LESS OR EQUAL THAN", "LESS_OR_EQUAL_THAN")
                .Replace("MORE OR EQUAL THAN", "MORE_OR_EQUAL_THAN");

            

            var isValid = true;
            var codeWords = code.Split('\n').Select(l => l.Split(' ')).ToArray();
            var compiledCode = string.Empty;

            var builder = new StringBuilder();
            builder.AppendLine("using GameAPI;");
            builder.AppendLine("using System;");
            builder.AppendLine("using System.Collections.Concurrent;");
            builder.AppendLine("using System.Collections.Generic;\n");
            builder.AppendLine("namespace GameAPI.DSL");
            builder.AppendLine("{");
            builder.AppendLine($"\tpublic class {scriptName} : PlayerScript");
            builder.AppendLine("\t{");
            builder.AppendLine($"\t\tpublic {scriptName}()");
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
            bool Compile()
            {
                return false;
            }

            bool Detect_SAVE_TO()
            {

                return false;
            }

            bool Detect_FOR_EACH()
            {
                return false;
            }
        }
    }
}
