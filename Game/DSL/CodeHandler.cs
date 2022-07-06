using GameAPI;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using MoreLinq.Extensions;
using System.Reflection;

namespace Game.DSL
{
    public class CodeHandler
    {
        private Type[]? _types;
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
        };

        public void InvokePlayerScripts(GameWorld gameWorld, Parameters parameters)
        {
            foreach(var position in CodeBuilder.GetCallOrder())
            {
                var script = _types?.FirstOrDefault(t => t.Name.Equals(position));
                if(script != null)
                {
                    dynamic? instance = Activator.CreateInstance(script);
                    (instance as PlayerScript)?.Invoke(gameWorld, parameters);
                }
            }
        }

        public void CompileScripts()
        {
            foreach (var csFile in Directory.GetFiles(CodeBuilder.ScriptsFolderPath).Where(f => f.Contains(".cs")))
            {
                var dllFileName = $"{csFile.Replace($@"{CodeBuilder.ScriptsFolderPath}\", string.Empty).Replace(".cs", string.Empty)}";
                var compilation = CSharpCompilation.Create(dllFileName, new[] { SyntaxFactory.ParseSyntaxTree(File.ReadAllText(csFile), null, string.Empty) }, _references, _compilationOptions);

                using var dllStream = new MemoryStream();
                using var pdbStream = new MemoryStream();
                if (compilation.Emit(dllStream, pdbStream).Success)
                {
                    var filePath = $@"{CodeBuilder.ScriptsFolderPath}\{dllFileName}.dll";
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    File.WriteAllBytes(filePath, dllStream.ToArray());
                }
            }
        }

        public void LoadScripts()
        {
            var types = new List<Type>();
            foreach(var dllFile in Directory.GetFiles(CodeBuilder.ScriptsFolderPath).Where(f => f.Contains(".dll")))
            {
                var dll = Assembly.LoadFile(dllFile);
                types.AddRange(dll.GetExportedTypes());
            }
            _types = types.ToArray();
        }

        public void CreateScript(string code)
        {

        }
    }
}
