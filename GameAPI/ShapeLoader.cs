namespace GameAPI
{
    public class GridLoader
    {
        private readonly Dictionary<Grids, Dictionary<States, byte[][]>> _grids = new();

        public GridLoader()
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
                                _grids[(Grids)folder][(States)file] = state;
                            }
                        }
                    }
                }
            }
        }

        public byte[][] GetGrid(Grids grid, States state) => _grids.ContainsKey(grid) && _grids[grid].ContainsKey(state) ? _grids[grid][state] : new byte[][] { new byte[] { 1 } };
        public Dictionary<States, byte[][]> GetStates(Grids grid) => _grids.ContainsKey(grid) ? _grids[grid] : new();
        public Dictionary<Grids, Dictionary<States, byte[][]>> GetGrids() => _grids;
    }
}
