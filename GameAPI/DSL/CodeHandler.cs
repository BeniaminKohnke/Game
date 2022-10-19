using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using MoreLinq.Extensions;
using System.Collections.Concurrent;
using System.Reflection;

namespace GameAPI.DSL
{
    public class CodeHandler
    {
        private readonly GameWorld _gameWorld;
        private readonly Thread t_update;
        private readonly ConcurrentDictionary<string, object> _dynamicObjects = new();
        private readonly Dictionary<string, PlayerScript> _activeScripts = new();
        private readonly CSharpCompilationOptions _compilationOptions = new(OutputKind.DynamicallyLinkedLibrary);
        private readonly MetadataReference[] _references = new[]
        {
            AssemblyMetadata.CreateFromFile(typeof(string).Assembly.Location).GetReference(),
            MetadataReference.CreateFromFile($@"{Directory.GetCurrentDirectory()}\GameAPI.dll"),
            MetadataReference.CreateFromFile($@"{Path.GetDirectoryName(typeof(object).Assembly.Location)}\mscorlib.dll"),
            MetadataReference.CreateFromFile($@"{Path.GetDirectoryName(typeof(object).Assembly.Location)}\System.dll"),
            MetadataReference.CreateFromFile($@"{Path.GetDirectoryName(typeof(object).Assembly.Location)}\System.Core.dll"),
            MetadataReference.CreateFromFile($@"{Path.GetDirectoryName(typeof(object).Assembly.Location)}\System.Runtime.dll"),
            MetadataReference.CreateFromFile($@"{Path.GetDirectoryName(typeof(object).Assembly.Location)}\System.Collections.dll"),
            MetadataReference.CreateFromFile($@"{Path.GetDirectoryName(typeof(object).Assembly.Location)}\System.Collections.Concurrent.dll"),
        };
        public bool AllowRunningScripts { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public bool RecompileScripts { get; set; } = false;

        public CodeHandler(GameWorld gameWorld)
        {
            _gameWorld = gameWorld;
            ScriptFunctions.GameWorld = _gameWorld;
            t_update = new(new ThreadStart(Update));
            t_update.Start();
        }

        private void Update()
        {
            while (IsActive)
            {
                if (RecompileScripts)
                {
                    CompileScripts();
                    RecompileScripts = false;
                    AllowRunningScripts = false;
                }
                if (AllowRunningScripts)
                {
                    RunScripts();
                }
            }

            AbortScripts();
        }

        private void RunScripts()
        {
            foreach (var position in CodeBuilder.CallOrder)
            {
                if (_activeScripts.TryGetValue(position, out var script) && !script.IsActive)
                {
                    script.Invoke(_gameWorld, _dynamicObjects);
                }
            }
        }

        private void CompileScripts()
        {
            AbortScripts();

            foreach (var csFile in Directory.GetFiles(CodeBuilder.ScriptsFolderPath).Where(f => f.Contains(".cs")))
            {
                var dllFileName = $"{csFile.Replace($@"{CodeBuilder.ScriptsFolderPath}\", string.Empty).Replace(".cs", string.Empty)}";
                var compilation = CSharpCompilation.Create(dllFileName, new[] { SyntaxFactory.ParseSyntaxTree(File.ReadAllText(csFile), null, string.Empty) }, _references, _compilationOptions);

                var types = new List<Type>();
                using (var dllStream = new MemoryStream())
                {
                    var result = compilation.Emit(dllStream);
                    if (result.Success)
                    {
                        var dll = Assembly.Load(dllStream.ToArray());
                        types.AddRange(dll.GetExportedTypes());
                    }
                }

                foreach (var position in CodeBuilder.CallOrder)
                {
                    var script = types.FirstOrDefault(t => t.Name.Equals(position));
                    if (script != null)
                    {
                        dynamic? instance = Activator.CreateInstance(script);
                        if (instance is PlayerScript playerScript)
                        {
                            _activeScripts[position] = playerScript;
                        }
                    }
                }
            }
        }

        public void AbortScripts()
        {
            AllowRunningScripts = false;
            _activeScripts.ForEach(s => s.Value.Abort());
        }
    }
}
