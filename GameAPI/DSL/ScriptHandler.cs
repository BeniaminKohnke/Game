using Microsoft.CodeAnalysis;
using MoreLinq.Extensions;
using System.Reflection;

namespace GameAPI.DSL
{
    public sealed class ScriptHandler
    {
        private readonly Dictionary<string, object> _dynamicObjects = new();
        private readonly Dictionary<string, IPlayerScript> _compilations = new();
        public bool RunScripts { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public bool ReloadScripts { get; set; } = false;

        public void Update(GameWorld gameWorld, float deltaTime)
        {
            if (ReloadScripts)
            {
                foreach (var dllFilePath in Directory.GetFiles(ScriptBuilder.ScriptsFolderPath).Where(f => f.Contains(".dll")))
                {
                    var dll = Assembly.Load(File.ReadAllBytes(dllFilePath));
                    foreach (var position in ScriptBuilder.CallOrder)
                    {
                        var script = dll.GetExportedTypes().FirstOrDefault(t => t.Name.Equals(position));
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

                ReloadScripts = false;
            }

            if (RunScripts)
            {
                foreach (var position in ScriptBuilder.CallOrder)
                {
                    if (_compilations.TryGetValue(position, out var script))
                    {
                        script.Run(gameWorld, _dynamicObjects, deltaTime);
                    }
                }
            }
        }
    }
}
