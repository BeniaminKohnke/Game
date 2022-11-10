using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using MoreLinq.Extensions;
using System.Reflection;

namespace GameAPI.DSL
{
    public sealed class WorldHandler
    {
        private readonly Dictionary<string, object> _dynamicObjects = new();
        private readonly Dictionary<string, IPlayerScript> _compilations = new();
        private readonly CSharpCompilationOptions _compilationOptions = new(OutputKind.DynamicallyLinkedLibrary);
        private readonly MetadataReference[] _references = 
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
        public GameWorld? World { get; set; }
        public bool RunScripts { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public bool RecompileScripts { get; set; } = false;

        public void Update(float deltaTime)
        {
            if (World != null)
            {
                if (RecompileScripts)
                {
                    CompileScripts();
                    RecompileScripts = false;
                }

                if (RunScripts)
                {
                    foreach (var position in CodeBuilder.CallOrder)
                    {
                        if (_compilations.TryGetValue(position, out var script))
                        {
                            script.Run(World, _dynamicObjects, deltaTime);
                        }
                    }
                }

                World.Update(deltaTime);
            }
        }

        private void CompileScripts()
        {
            foreach (var csFile in Directory.GetFiles(CodeBuilder.ScriptsFolderPath).Where(f => f.Contains(".cs")))
            {
                var dllFileName = $"{csFile.Replace($@"{CodeBuilder.ScriptsFolderPath}\", string.Empty).Replace(".cs", string.Empty)}";
                var compilation = CSharpCompilation.Create
                (
                    dllFileName, 
                    new[] { SyntaxFactory.ParseSyntaxTree(File.ReadAllText(csFile), null, string.Empty) }, 
                    _references, 
                    _compilationOptions
                );

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
                        if (instance is IPlayerScript playerScript)
                        {
                            _compilations[position] = playerScript;
                        }
                    }
                }
            }
        }
    }
}
