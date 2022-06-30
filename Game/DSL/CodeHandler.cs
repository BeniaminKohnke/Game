using GameAPI;
using System.CodeDom.Compiler;
using System.Reflection;

namespace Game.DSL
{
    public class CodeHandler
    {
        private readonly CodeDomProvider _provider = CodeDomProvider.CreateProvider("CSharp");
        private Type[]? _types;

        public void InvokePlayerScripts(GameWorld gameWorld, Parameters parameters)
        {
            foreach(var position in CodeBuilder.GetCallOrder())
            {
                var script = _types?.FirstOrDefault(t => t.Name.Equals(position));
                if(script != null)
                {
                    dynamic? instance = Activator.CreateInstance(script);
                    (instance as IPlayerScript)?.Invoke(gameWorld, parameters);
                }
            }
        }

        public void CompileScripts()
        {
            foreach (var csFile in Directory.GetFiles(CodeBuilder.ScriptsFolderPath).Where(f => f.Contains(".cs")))
            {
                var dllFile = $"{csFile.Replace(".cs", string.Empty)}.dll";
                var parameters = new CompilerParameters
                {
                    OutputAssembly = dllFile,
                    GenerateInMemory = true,
                    TreatWarningsAsErrors = false,
                };

                var result = _provider.CompileAssemblyFromFile(parameters, csFile);
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
