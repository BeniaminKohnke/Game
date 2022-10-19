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
        private static readonly string[] _typesNames = Enum.GetNames(typeof(Types));
        private static readonly Regex FOR_SINGLE_Regex = new(@"FOR_SINGLE ([a-z][a-zA-Z]+) FROM ([a-z][a-zA-Z]+) REPEAT", RegexOptions.Compiled);
        private static readonly Regex IS_Regex = new(@"([A-Za-z]+) IS ([A-Za-z]+)", RegexOptions.Compiled);

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
            var filePath = $@"{ScriptsFolderPath}\{scriptName}.cs";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.WriteAllText(filePath, TranslateToCSharp(scriptName, code));
            return true;
        }

        private static string TranslateToCSharp(string scriptName, string code)
        {
            code = PrepareCodeToCompilation(code);
            code = ChangeFunctionsToCSharpMethods(code);
            code = TranslateKeywords(code);
            code = ChangeTabsToBrackets(code);

            if (string.IsNullOrEmpty(code))
            {
                return string.Empty;
            }

            var builder = new StringBuilder();
            builder.AppendLine("using GameAPI;");
            builder.AppendLine("using System;");
            builder.AppendLine("using System.Collections.Concurrent;");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine("namespace GameAPI.DSL");
            builder.AppendLine("{");
            builder.AppendLine($"public class {scriptName} : PlayerScript" + "{");
            builder.AppendLine("protected override void Do(GameWorld gameWorld, ConcurrentDictionary<string, object> parameters){");
            builder.Append(code);
            builder.AppendLine("}}}");

            return builder.ToString();
        }

        private static string PrepareCodeToCompilation(string code) => code
            .Replace(" OR ", " || ")
            .Replace(" AND ", " && ")
            .Replace(" Player ", " gameWorld.Player ")
            .Replace("SAVE TO", "SAVE_TO")
            .Replace("FOR SINGLE", "FOR_SINGLE")
            .Replace("LESS THAN", "LESS_THAN")
            .Replace("MORE THAN", "MORE_THAN")
            .Replace("LESS OR EQUAL THAN", "LESS_OR_EQUAL_THAN")
            .Replace("MORE OR EQUAL THAN", "MORE_OR_EQUAL_THAN");

        private static string ChangeFunctionsToCSharpMethods(string code)
        {
            var codeLines = code.Split('\n');
            for (var i = 0; i < codeLines.Length; i++)
            {
                var parts = codeLines[i].Split(' ');
                var index = Array.IndexOf(parts, parts.LastOrDefault(p => char.IsUpper(p[0]) && p.Any(c => char.IsLower(c))));
                while (index != -1)
                {
                    var functionName = parts[index];
                    if (_scriptFunctions.TryGetValue(functionName, out var paramCount))
                    {
                        var lastIndex = index + paramCount;
                        if (parts.Length > lastIndex)
                        {
                            var function = @$"{functionName}({string.Join(',', parts.Skip(index + 1).Take(paramCount))})";
                            parts[index] = function;
                            for (var j = index + 1; j <= lastIndex; j++)
                            {
                                parts[j] = string.Empty;
                            }
                            parts = parts.Where(p => !string.IsNullOrEmpty(p)).ToArray();
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                    index = Array.IndexOf(parts, parts.LastOrDefault(p => !p.Contains('(') && char.IsUpper(p[0]) && p.Any(c => char.IsLower(c))));
                    codeLines[i] = string.Join(" ", parts);
                }
            }
            return string.Join('\n', codeLines);
        }

        private static string ChangeTabsToBrackets(string code)
        {
            var codeLines = code.Split('\n');
            while (codeLines.Any(l => l.Contains('\t')))
            {
                var firstIndex = Array.IndexOf(codeLines, codeLines.FirstOrDefault(l => l.StartsWith('\t')));
                if (firstIndex != -1)
                {
                    var lastIndex = Array.IndexOf(codeLines, codeLines.LastOrDefault(l => l.StartsWith('\t') && Array.IndexOf(codeLines, l) >= firstIndex));
                    codeLines[firstIndex] = '{' + new string(codeLines[firstIndex].Skip(1).ToArray());
                    codeLines[lastIndex] = codeLines[lastIndex] + '}';
                }
            }
            return string.Join('\n', codeLines);
        }

        private static string TranslateKeywords(string code)
        {
            code = code.Replace("FINISH", "break;");

            foreach (var match in IS_Regex.Matches(code).Cast<Match>())
            {
                code = code.Replace(match.Value, _typesNames.Contains(match.Groups[2].Value) 
                    ? $"(({match.Groups[1].Value} as GameObject)?.ObjectType == Types.{match.Groups[2].Value} ?? false)" 
                    : "false");
            }

            foreach (var match in FOR_SINGLE_Regex.Matches(code).Cast<Match>())
            {
                var g1 = match.Groups[1].Value;
                var g2 = match.Groups[2].Value;
                code = code.Replace(match.Value, $"foreach(var {g1} in (({g2} as IEnumerable collection) != null ? collection : new[]{"{" + g2 + "}"}))");
            }

            return code;
        }
    }
}
