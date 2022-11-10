using GameAPI.GameObjects;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Aardvark.Base;

namespace GameAPI.DSL
{
    public static partial class ScriptBuilder
    {
        private const string EXTENSION = ".script";
        private static readonly Dictionary<string, byte> _scriptFunctions = typeof(ScriptFunctions)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .ToDictionary(m => m.Name, p => (byte)(p.GetParameters().Length - 3));
        private static readonly CSharpCompilationOptions _compilationOptions = new(OutputKind.DynamicallyLinkedLibrary);
        private static readonly MetadataReference[] _references =
        {
            MetadataReference.CreateFromFile($@"{Directory.GetCurrentDirectory()}\GameAPI.dll"),
            MetadataReference.CreateFromFile($@"{Directory.GetCurrentDirectory()}\mscorlib.dll"),
            MetadataReference.CreateFromFile($@"{Directory.GetCurrentDirectory()}\System.dll"),
            MetadataReference.CreateFromFile($@"{Directory.GetCurrentDirectory()}\System.Core.dll"),
            MetadataReference.CreateFromFile($@"{Directory.GetCurrentDirectory()}\System.Runtime.dll"),
            MetadataReference.CreateFromFile($@"{Directory.GetCurrentDirectory()}\System.Collections.dll"),
            MetadataReference.CreateFromFile($@"{Directory.GetCurrentDirectory()}\System.Collections.Concurrent.dll"),
            MetadataReference.CreateFromFile($@"{Directory.GetCurrentDirectory()}\System.Private.CoreLib.dll"),
        };
        private static readonly Dictionary<string, string> _globalVariablesPaths = new()
        {
            ["Player"] = "gameWorld.Player",
            ["Items"] = "gameWorld.Player.Items",
            ["None"] = "(object)0",
        };

        public static string[] CallOrder 
        { 
            get => File.ReadAllLines(CallOrderFilePath).Select(l => l.Replace("()", "Script")).ToArray();
            private set => File.WriteAllLines(CallOrderFilePath, value);
        }
        public static string ScriptsFolderPath { get; } = $@"{Directory.GetCurrentDirectory()}\Scripts";
        public static string CallOrderFilePath { get; } = $@"{Directory.GetCurrentDirectory()}\Scripts\CallOrder.txt";

        static ScriptBuilder()
        {
            foreach (var type in new[] { typeof(Types), typeof(ItemTypes), typeof(Directions) })
            {
                foreach (var name in Enum.GetNames(type))
                {
                    if (!_globalVariablesPaths.ContainsKey(name))
                    {
                        _globalVariablesPaths[name] = $"{type.Name}.{name}";
                    }
                }
            }

            if (!Directory.Exists(ScriptsFolderPath))
            {
                Directory.CreateDirectory(ScriptsFolderPath);
            }
        }

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

        public static void SaveScript(string scriptName, string script)
        {
            if (scriptName.Equals("CallOrder"))
            {
                CallOrder = script.Split("\n");
            }
            else
            {
                File.WriteAllText($@"{ScriptsFolderPath}\{scriptName}{EXTENSION}", script);
            }
        }

        public static void DeleteScript(string scriptName)
        {
            var path = $@"{ScriptsFolderPath}\{scriptName}{EXTENSION}";
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            path = $@"{ScriptsFolderPath}\{scriptName}.dll";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static bool CompileScript(string scriptName, string script)
        {
            if (scriptName.Equals("CallOrder"))
            {
                var scriptsNames = Directory.GetFiles(ScriptsFolderPath).Where(s => s.Contains(EXTENSION)).Select(s => s.Replace($@"{ScriptsFolderPath}\", string.Empty).Replace(EXTENSION, string.Empty));
                return script.Split("\n").All(l => scriptsNames.Contains(l));
            }

            return CompileToCSharp(scriptName, script);
        }

        public static (string name, string script)[] GetExistingScripts() => Directory
            .GetFiles(ScriptsFolderPath)
            .Where(s => s.Contains(EXTENSION))
            .Select(p => (p.Replace($@"{ScriptsFolderPath}\", string.Empty).Replace(EXTENSION, string.Empty), File.ReadAllText(p)))
            .ToArray();

        public static bool CompileToCSharp(string scriptName, string script)
        {
            var success = false;
            var compiledScript = TranslateToCSharp(scriptName, script);
            if (!string.IsNullOrEmpty(compiledScript))
            {
                var compilation = CSharpCompilation.Create
                (
                    scriptName,
                    new[] { SyntaxFactory.ParseSyntaxTree(compiledScript, null, string.Empty) },
                    _references,
                    _compilationOptions
                );

                using var dllStream = new MemoryStream();
                var result = compilation.Emit(dllStream);
                if (result.Success)
                {
                    var filePath = $@"{ScriptsFolderPath}\{scriptName}.dll";
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    File.WriteAllBytes(filePath, dllStream.ToArray());
                    success = true;
                }
            }

            return success;
        }

        private static string TranslateToCSharp(string scriptName, string script)
        {
            script = string.Join('\n', script.Split('\n', StringSplitOptions.RemoveEmptyEntries).Where(l => !l.Equals("\r"))) + "\n";

            script = PrepareScriptToCompilation(script);
            script = ChangeFunctionsToCSharpMethods(script);
            script = TranslateKeywords(script);
            script = ChangeTabsToBrackets(script);
            script = AddSemicolons(script);
            script = TranslateVariables(script);
            script = TranslateOtherWords(script);

            script = script.Replace("\r", string.Empty);

            if (string.IsNullOrEmpty(script))
            {
                return string.Empty;
            }

            var builder = new StringBuilder();
            builder.AppendLine("using GameAPI;");
            builder.AppendLine("using System;");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine("namespace GameAPI.DSL");
            builder.AppendLine("{");
            builder.AppendLine($"public class {scriptName} : {nameof(IPlayerScript)}" + "{");
            builder.AppendLine("public void Run(GameWorld gameWorld, Dictionary<string, object> parameters, float deltaTime){");
            builder.Append(script);
            builder.AppendLine("}}}");

            return builder.ToString();
        }

        private static string PrepareScriptToCompilation(string script) => script
            .Replace("SAVE TO", "SAVE_TO")
            .Replace("FOR SINGLE", "FOR_SINGLE")
            .Replace("LESS THAN", "LESS_THAN")
            .Replace("MORE THAN", "MORE_THAN")
            .Replace("LESS OR EQUAL THAN", "LESS_OR_EQUAL_THAN")
            .Replace("MORE OR EQUAL THAN", "MORE_OR_EQUAL_THAN");

        private static string TranslateOtherWords(string script)
        {
            script = script.Replace(" OR ", " || ")
            .Replace(" AND ", " && ")
            .Replace("EQUALS", " == ");

            foreach (var match in ComparationRegex().Matches(script).Cast<Match>())
            {
                var method = string.Join(string.Empty, match.Groups[2].Value.ToLower().Split('_').Select(s => s.Replace(s[0].ToString(), s[0].ToString().ToUpper())));
                script = script.Replace(match.Value, $"{nameof(ScriptBuilder)}.{method}({match.Groups[1].Value},{match.Groups[3].Value})");
            }

            return script;
        }

        private static string TranslateVariables(string script)
        {
            foreach (var match in VariableRegex().Matches(script).Cast<Match>())
            {
                if (char.IsUpper(match.Groups[1].Value[0]))
                {
                    if (_globalVariablesPaths.TryGetValue(match.Groups[1].Value, out var path))
                    {
                        script = script.Replace(match.Value, path);
                    }
                }
                else
                {
                    script = script.Replace(match.Value, match.Value.Replace("[", string.Empty).Replace("]", string.Empty));
                }
            }

            foreach (var match in TupleRegex().Matches(script).Cast<Match>())
            {
                var numbers = match.Groups[1].Value.Split(',');
                if (numbers.Length == 1)
                {
                    script = script.Replace(match.Value, $"(object){numbers[0]}");
                }
                else
                {
                    script = script.Replace(match.Value, $"(object)({string.Join(',', numbers)})");
                }
            }

            foreach (var match in TextRegex().Matches(script).Cast<Match>())
            {
                script = script.Replace(match.Value, "(object)" + '\"' + match.Groups[1].Value + '\"');
            }

            foreach (var match in CollectionRegex().Matches(script).Cast<Match>())
            {
                script = script.Replace(match.Value, $"(object)Enumerable.Range({match.Groups[1].Value},{match.Groups[2].Value})");
            }

            return script;
        }

        private static string ChangeFunctionsToCSharpMethods(string script)
        {
            var scriptLines = script.Replace("\t", "\t ").Split('\n');
            for (var i = 0; i < scriptLines.Length; i++)
            {
                var parts = scriptLines[i].Split(' ');
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
                    scriptLines[i] = string.Join(" ", parts);
                }
            }

            return string.Join('\n', scriptLines);
        }

        private static string ChangeTabsToBrackets(string script)
        {
            var scriptGrid = script.Split('\n').Select(l => l.Replace("\t", "\t ").Split(' ')).ToArray();
            while (scriptGrid.Any(l => l.Any(p => p.Contains('\t'))))
            {
                var firstGridIndex = Array.IndexOf(scriptGrid, scriptGrid.FirstOrDefault(l => l.Any(p => p.Equals("\t"))));
                var indexOfTab = Array.IndexOf(scriptGrid[firstGridIndex], "\t");
                var secondGridIndex = firstGridIndex;
                for (var i = firstGridIndex + 1; i < scriptGrid.Length; i++)
                {
                    if (scriptGrid[i].Length <= indexOfTab || !scriptGrid[i][indexOfTab].Equals("\t"))
                    {
                        break;
                    }
                    secondGridIndex++;
                }

                if (firstGridIndex != secondGridIndex)
                {
                    scriptGrid[firstGridIndex][indexOfTab] = "{";
                    for (var i = firstGridIndex + 1; i <= secondGridIndex; i++)
                    {
                        scriptGrid[i][indexOfTab] = string.Empty;
                    }
                    scriptGrid[secondGridIndex][^1] = scriptGrid[secondGridIndex][^1] + "}";
                }
                else
                {
                    scriptGrid[firstGridIndex][indexOfTab] = "{";
                    scriptGrid[firstGridIndex][^1] = scriptGrid[firstGridIndex][^1] + "}";
                }
            }

            return string.Join('\n', scriptGrid.Select(l => string.Join(' ', l)));
        }

        private static string TranslateKeywords(string script)
        {
            script = script.Replace("FINISH", "break");

            foreach (var match in SaveToRegex().Matches(script).Cast<Match>().GroupBy(m => m.Groups[2].Value))
            {
                var first = match.First();
                script = script.Replace(first.Value, $"object {first.Groups[2].Value} = {first.Groups[1].Value}");
                foreach (var m in match.Skip(1))
                {
                    script = script.Replace(m.Value, $"{m.Groups[2].Value} = {m.Groups[1].Value}");
                }
            }

            foreach (var match in IsRegex().Matches(script).Cast<Match>())
            {
                script = script.Replace(match.Value, $"{nameof(ScriptBuilder)}.Is({match.Groups[1].Value},{match.Groups[2].Value})");
            }

            foreach (var match in ForSingleRegex().Matches(script).Cast<Match>())
            {
                var g1 = match.Groups[1].Value;
                var g2 = match.Groups[2].Value;
                script = script.Replace(match.Value, $"foreach( var {g1} in ({g2} as IEnumerable<object> ?? new[]{"{" + g2 + "}"}) )");
            }

            foreach (var match in IfRegex().Matches(script).Cast<Match>())
            {
                script = script.Replace(match.Value, $"if( {match.Groups[1].Value} )");
            }

            script = script.Replace("ELSE THEN", "else");

            return script;
        }

        private static string AddSemicolons(string script)
        {
            var scriptLines = script.Split('\n');
            for (var i = 0; i < scriptLines.Length; i++)
            {
                if (!scriptLines[i].Contains("if") && !scriptLines[i].Contains("foreach") && !scriptLines[i].Contains("else") && scriptLines[i].Any(c => char.IsLetter(c)))
                {
                    if (scriptLines[i].Contains('}'))
                    {
                        scriptLines[i] = scriptLines[i].Insert(scriptLines[i].IndexOf("}"), ";");
                    }
                    else
                    {
                        scriptLines[i] = scriptLines[i] + ";";
                    }
                }
            }

            return string.Join('\n', scriptLines);
        }

        [GeneratedRegex(@"\[([A-Za-z]+)\]", RegexOptions.Compiled)]
        private static partial Regex VariableRegex();
        [GeneratedRegex("([A-Za-z(),.:'\\d]+) (LESS_THAN|MORE_THAN|LESS_OR_EQUAL_THAN|MORE_OR_EQUAL_THAN) ([A-Za-z(),.:'\\d]+)", RegexOptions.Compiled)]
        private static partial Regex ComparationRegex();
        [GeneratedRegex(@"\['([\S ]+)'\]", RegexOptions.Compiled)]
        private static partial Regex TextRegex();
        [GeneratedRegex(@"\[((?:\d+(?:,\d+)?)+)\]", RegexOptions.Compiled)]
        private static partial Regex TupleRegex();
        [GeneratedRegex(@"\[(\d+):(\d+)]", RegexOptions.Compiled)]
        private static partial Regex CollectionRegex();
        [GeneratedRegex(@"FOR_SINGLE ([A-Za-z\[\]':\d]+) FROM ([A-Za-z\[\]':\d]+) DO", RegexOptions.Compiled)]
        private static partial Regex ForSingleRegex();
        [GeneratedRegex(@"([A-Za-z\)\(,\[\].':\d]+) SAVE_TO \[([A-Za-z\)\(]+)\]", RegexOptions.Compiled)]
        private static partial Regex SaveToRegex();
        [GeneratedRegex(@"([A-Za-z\)\(,\[\].':\d]+) IS ([A-Za-z\)\(,\[\]':\d]+)", RegexOptions.Compiled)]
        private static partial Regex IsRegex();
        [GeneratedRegex(@"IF ([\S ]+) THEN", RegexOptions.Compiled)]
        private static partial Regex IfRegex();
    }
}