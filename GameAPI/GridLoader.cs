﻿using System.Collections.ObjectModel;

namespace GameAPI
{
    public static class GridLoader
    {
        private static readonly Dictionary<Grids, Dictionary<States, ReadOnlyCollection<ReadOnlyCollection<byte>>>> _grids = new();

        static GridLoader()
        {
            var mainDir = $@"{Directory.GetCurrentDirectory()}\Textures";
            foreach (var folder in Enum.GetValues(typeof(Grids)))
            {
                var folderPath = $@"{mainDir}\{folder}";
                if (Directory.Exists(folderPath))
                {
                    _grids[(Grids)folder] = new();
                    foreach (var file in Enum.GetValues(typeof(States)))
                    {
                        var filePath = $@"{folderPath}\{file}.sm";
                        if (File.Exists(filePath))
                        {
                            var state = File.ReadAllLines(filePath).Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray()).ToArray();
                            if (state.Length > 0)
                            {
                                _grids[(Grids)folder][(States)file] = Array.AsReadOnly(state.Select(a => Array.AsReadOnly(a)).ToArray());
                            }
                        }
                    }
                }
            }
        }

        public static ReadOnlyCollection<ReadOnlyCollection<byte>>? GetGrid(Grids grid, States state)
            => _grids.TryGetValue(grid, out var g) && g.TryGetValue(state, out var s) ? s : null;
        public static Dictionary<States, ReadOnlyCollection<ReadOnlyCollection<byte>>>? GetStates(Grids grid) => _grids.TryGetValue(grid, out var g) ? g : null;
        public static Dictionary<Grids, Dictionary<States, ReadOnlyCollection<ReadOnlyCollection<byte>>>> GetGrids() => _grids;
    }
}
