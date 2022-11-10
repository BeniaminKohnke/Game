using GameAPI.GameObjects;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace GameAPI.DSL
{
    public static partial class CodeBuilder
    {
        private static readonly Dictionary<string, byte> _scriptFunctions = typeof(ScriptFunctions)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .ToDictionary(m => m.Name, p => (byte)(p.GetParameters().Length - 3));
        private static readonly Dictionary<string, string> _globalVariablesPaths = new()
        {
            ["Player"] = "gameWorld.Player",
            ["Rock"] = "Types.Rock",
            ["Melee"] = "ItemTypes.Melee",
            ["Items"] = "gameWorld.Player.Items",
            ["None"] = "(object)0",
            ["North"] = "Directions.Up",
            ["South"] = "Directions.Down",
            ["West"] = "Directions.Left",
            ["East"] = "Directions.Right",
        };

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

        public static bool Is(object first, object second)
        {
            if (!first.Equals(second))
            {
                switch (first)
                {
                    case Item fi:
                        switch (second)
                        {
                            case Item si:
                                return fi.ItemType == si.ItemType;
                            case ItemTypes sit:
                                return fi.ItemType == sit;
                            case string ss:
                                return fi.Name.Equals(ss);
                        }
                        break;
                    case GameObject fo:
                        switch (second)
                        {
                            case GameObject so:
                                return fo.ObjectType == so.ObjectType;
                            case Types st:
                                return fo.ObjectType == st;
                        }
                        break;
                    case Types ft:
                        switch (second)
                        {
                            case GameObject s:
                                return ft == s.ObjectType;
                            case Types st:
                                return ft == st;
                        }
                        break;
                    case double fd:
                        switch (second)
                        {
                            case double sd:
                                return fd == sd;
                        }
                        break;
                    case ItemTypes fit:
                        switch (second)
                        {
                            case Item si:
                                return fit == si.ItemType;
                            case ItemTypes sit:
                                return fit == sit;
                        }
                        break;
                    case string fs:
                        switch (second)
                        {
                            case string ss:
                                return fs.Equals(ss);
                        }
                        break;
                }
            }

            return false;
        }

        public static bool LessThan(object first, object second) => first is int fd && second is int sd && fd < sd;
        public static bool MoreThan(object first, object second) => first is int fd && second is int sd && fd > sd;
        public static bool LessOrEqualThan(object first, object second) => first is int fd && second is int sd && fd <= sd;
        public static bool MoreOrEqualThan(object first, object second) => first is int fd && second is int sd && fd >= sd;

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

            path = $@"{ScriptsFolderPath}\{scriptName}.cs";
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
            code = string.Join('\n', code.Split('\n', StringSplitOptions.RemoveEmptyEntries).Where(l => !l.Equals("\r"))) + "\n";

            code = PrepareCodeToCompilation(code);
            code = ChangeFunctionsToCSharpMethods(code);
            code = TranslateKeywords(code);
            code = ChangeTabsToBrackets(code);
            code = AddSemicolons(code);
            code = TranslateVariables(code);
            code = TranslateOtherWords(code);

            code = code.Replace("\r", string.Empty);

            if (string.IsNullOrEmpty(code))
            {
                return string.Empty;
            }

            var builder = new StringBuilder();
            builder.AppendLine("using GameAPI;");
            builder.AppendLine("using System;");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine("namespace GameAPI.DSL");
            builder.AppendLine("{");
            builder.AppendLine($"public class {scriptName} : IPlayerScript" + "{");
            builder.AppendLine("public void Run(GameWorld gameWorld, Dictionary<string, object> parameters, float deltaTime){");
            builder.Append(code);
            builder.AppendLine("}}}");

            return builder.ToString();
        }

        private static string PrepareCodeToCompilation(string code) => code
            .Replace("SAVE TO", "SAVE_TO")
            .Replace("FOR SINGLE", "FOR_SINGLE")
            .Replace("LESS THAN", "LESS_THAN")
            .Replace("MORE THAN", "MORE_THAN")
            .Replace("LESS OR EQUAL THAN", "LESS_OR_EQUAL_THAN")
            .Replace("MORE OR EQUAL THAN", "MORE_OR_EQUAL_THAN");

        private static string TranslateOtherWords(string code)
        {
            code = code.Replace(" OR ", " || ")
            .Replace(" AND ", " && ")
            .Replace("EQUALS", " == ");

            foreach (var match in ComparationRegex().Matches(code).Cast<Match>())
            {
                var method = string.Join(string.Empty, match.Groups[2].Value.ToLower().Split('_').Select(s => s.Replace(s[0].ToString(), s[0].ToString().ToUpper())));
                code = code.Replace(match.Value, $"CodeBuilder.{method}({match.Groups[1].Value},{match.Groups[3].Value})");
            }

            return code;
        }

        private static string TranslateVariables(string code)
        {
            foreach (var match in VariableRegex().Matches(code).Cast<Match>())
            {
                if (char.IsUpper(match.Groups[1].Value[0]))
                {
                    if (_globalVariablesPaths.TryGetValue(match.Groups[1].Value, out var path))
                    {
                        code = code.Replace(match.Value, path);
                    }
                }
                else
                {
                    code = code.Replace(match.Value, match.Value.Replace("[", string.Empty).Replace("]", string.Empty));
                }
            }

            foreach (var match in TupleRegex().Matches(code).Cast<Match>())
            {
                var numbers = match.Groups[1].Value.Split(',');
                if (numbers.Length == 1)
                {
                    code = code.Replace(match.Value, $"(object){numbers[0]}");
                }
                else
                {
                    code = code.Replace(match.Value, $"(object)({string.Join(',', numbers)})");
                }
            }

            foreach (var match in TextRegex().Matches(code).Cast<Match>())
            {
                code = code.Replace(match.Value, "(object)" + '\"' + match.Groups[1].Value + '\"');
            }

            foreach (var match in CollectionRegex().Matches(code).Cast<Match>())
            {
                code = code.Replace(match.Value, $"(object)Enumerable.Range({match.Groups[1].Value},{match.Groups[2].Value})");
            }

            return code;
        }

        private static string ChangeFunctionsToCSharpMethods(string code)
        {
            var codeLines = code.Replace("\t", "\t ").Split('\n');
            for (var i = 0; i < codeLines.Length; i++)
            {
                var parts = codeLines[i].Split(' ');
                var index = Array.IndexOf(parts, parts.LastOrDefault(p => p.Length > 0 && char.IsUpper(p[0]) && p.Any(c => char.IsLower(c))));
                while (index != -1)
                {
                    var functionName = parts[index];
                    if (_scriptFunctions.TryGetValue(functionName.Trim(), out var paramCount))
                    {
                        var lastIndex = index + paramCount;
                        if (parts.Length > lastIndex)
                        {
                            var parameters = $"{string.Join(',', parts.Skip(index + 1).Take(paramCount))}";
                            var function = @$"ScriptFunctions.{functionName}({(string.IsNullOrEmpty(parameters) ? string.Empty : $"{parameters},")}gameWorld,parameters,deltaTime)";
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
            var codeGrid = code.Split('\n').Select(l => l.Replace("\t", "\t ").Split(' ')).ToArray();
            while (codeGrid.Any(l => l.Any(p => p.Contains('\t'))))
            {
                var firstGridIndex = Array.IndexOf(codeGrid, codeGrid.FirstOrDefault(l => l.Any(p => p.Equals("\t"))));
                var indexOfTab = Array.IndexOf(codeGrid[firstGridIndex], "\t");
                var secondGridIndex = firstGridIndex;
                for (var i = firstGridIndex + 1; i < codeGrid.Length; i++)
                {
                    if (codeGrid[i].Length <= indexOfTab || !codeGrid[i][indexOfTab].Equals("\t"))
                    {
                        break;
                    }
                    secondGridIndex++;
                }

                if (firstGridIndex != secondGridIndex)
                {
                    codeGrid[firstGridIndex][indexOfTab] = "{";
                    for (var i = firstGridIndex + 1; i <= secondGridIndex; i++)
                    {
                        codeGrid[i][indexOfTab] = string.Empty;
                    }
                    codeGrid[secondGridIndex][^1] = codeGrid[secondGridIndex][^1] + "}";
                }
                else
                {
                    codeGrid[firstGridIndex][indexOfTab] = "{";
                    codeGrid[firstGridIndex][^1] = codeGrid[firstGridIndex][^1] + "}";
                }
            }

            return string.Join('\n', codeGrid.Select(l => string.Join(' ', l)));
        }

        private static string TranslateKeywords(string code)
        {
            code = code.Replace("FINISH", "break");

            foreach (var match in SaveToRegex().Matches(code).Cast<Match>().GroupBy(m => m.Groups[2].Value))
            {
                var first = match.First();
                code = code.Replace(first.Value, $"object {first.Groups[2].Value} = {first.Groups[1].Value}");
                foreach (var m in match.Skip(1))
                {
                    code = code.Replace(m.Value, $"{m.Groups[2].Value} = {m.Groups[1].Value}");
                }
            }

            foreach (var match in IsRegex().Matches(code).Cast<Match>())
            {
                code = code.Replace(match.Value, $"CodeBuilder.Is({match.Groups[1].Value},{match.Groups[2].Value})");
            }

            foreach (var match in ForSingleRegex().Matches(code).Cast<Match>())
            {
                var g1 = match.Groups[1].Value;
                var g2 = match.Groups[2].Value;
                code = code.Replace(match.Value, $"foreach( var {g1} in ({g2} as IEnumerable<object> ?? new[]{"{" + g2 + "}"}) )");
            }

            foreach (var match in IfRegex().Matches(code).Cast<Match>())
            {
                code = code.Replace(match.Value, $"if( {match.Groups[1].Value} )");
            }

            code = code.Replace("ELSE THEN", "else");

            return code;
        }

        private static string AddSemicolons(string code)
        {
            var codeLines = code.Split('\n');
            for (var i = 0; i < codeLines.Length; i++)
            {
                if (!codeLines[i].Contains("if") && !codeLines[i].Contains("foreach") && !codeLines[i].Contains("else") && codeLines[i].Any(c => char.IsLetter(c)))
                {
                    if (codeLines[i].Contains('}'))
                    {
                        codeLines[i] = codeLines[i].Insert(codeLines[i].IndexOf("}"), ";");
                    }
                    else
                    {
                        codeLines[i] = codeLines[i] + ";";
                    }
                }
            }

            return string.Join('\n', codeLines);
        }

        [GeneratedRegex(@"\[([A-Za-z]+)\]", RegexOptions.NonBacktracking)]
        private static partial Regex VariableRegex();
        [GeneratedRegex("([A-Za-z(),.:'\\d]+) (LESS_THAN|MORE_THAN|LESS_OR_EQUAL_THAN|MORE_OR_EQUAL_THAN) ([A-Za-z(),.:'\\d]+)", RegexOptions.NonBacktracking)]
        private static partial Regex ComparationRegex();
        [GeneratedRegex(@"\['([\S ]+)'\]", RegexOptions.NonBacktracking)]
        private static partial Regex TextRegex();
        [GeneratedRegex(@"\[((?:\d+(?:,\d+)?)+)\]", RegexOptions.NonBacktracking)]
        private static partial Regex TupleRegex();
        [GeneratedRegex(@"\[(\d+):(\d+)]", RegexOptions.NonBacktracking)]
        private static partial Regex CollectionRegex();
        [GeneratedRegex(@"FOR_SINGLE ([A-Za-z\[\]':\d]+) FROM ([A-Za-z\[\]':\d]+) DO", RegexOptions.Compiled | RegexOptions.NonBacktracking)]
        private static partial Regex ForSingleRegex();
        [GeneratedRegex(@"([A-Za-z\)\(,\[\].':\d]+) SAVE_TO \[([A-Za-z\)\(]+)\]", RegexOptions.NonBacktracking)]
        private static partial Regex SaveToRegex();
        [GeneratedRegex(@"([A-Za-z\)\(,\[\].':\d]+) IS ([A-Za-z\)\(,\[\]':\d]+)", RegexOptions.Compiled | RegexOptions.NonBacktracking)]
        private static partial Regex IsRegex();
        [GeneratedRegex(@"IF ([\S ]+) THEN", RegexOptions.Compiled | RegexOptions.NonBacktracking)]
        private static partial Regex IfRegex();
    }
}