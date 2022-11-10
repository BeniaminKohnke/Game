using Microsoft.CodeAnalysis;
using MoreLinq.Extensions;
using System.Reflection;

namespace GameAPI.DSL
{
    public sealed class WorldHandler
    {
        private readonly Dictionary<string, object> _dynamicObjects = new();
        private readonly Dictionary<string, IPlayerScript> _compilations = new();
        public GameWorld? World { get; set; }
        public bool RunScripts { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public bool ReloadScripts { get; set; } = false;

        public void Update(float deltaTime)
        {
            if (World != null)
            {
                if (ReloadScripts)
                {
                    foreach (var dllFilePath in Directory.GetFiles(CodeBuilder.ScriptsFolderPath).Where(f => f.Contains(".dll")))
                    {
                        var dll = Assembly.LoadFile(dllFilePath);
                        foreach (var position in CodeBuilder.CallOrder)
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
    }
}
