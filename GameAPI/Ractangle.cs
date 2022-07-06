namespace GameAPI
{
    public class Ractangle
    {
        private byte[][] _grid;
        private (long x, long y) _v1;

        /// <summary>
        ///Position: top-left vertex
        ///<para>This point can be set</para>
        /// </summary>
        public (long x, long y) V1 
        {
            get => _v1; 
            set
            {
                _v1 = value;
                V2 = (_v1.x + _grid[0].Length, _v1.y);
                V3 = (_v1.x, _v1.y + _grid.Length);
                V4 = (_v1.x + _grid[0].Length, _v1.y + _grid.Length);
            }
        }
        /// <summary>
        ///Position: top-right vertex
        /// </summary>
        public (long x, long y) V2 { get; private set; }
        /// <summary>
        ///Position: bottom-left vertex
        /// </summary>
        public (long x, long y) V3 { get; private set; }
        /// <summary>
        ///Position: bottom-right vertex
        /// </summary>
        public (long x, long y) V4 { get; private set; }

        public int SizeX => _grid[0].Length;
        public int SizeY => _grid.Length;
        public byte this[int x, int y] => _grid[x][y];

        public Ractangle(byte[][] grid, long x, long y)
        {
            _grid = grid;
            _v1 = (x, y);
            V2 = (x + grid[0].Length, y);
            V3 = (x, y + grid.Length);
            V4 = (x + grid[0].Length, y + grid.Length);
        }

        public void SetGrid(byte[][] grid)
        {
            _grid = grid;
            _v1 = (_v1.x, _v1.y);
            V2 = (_v1.x + _grid[0].Length, _v1.y);
            V3 = (_v1.x, _v1.y + _grid.Length);
            V4 = (_v1.x + _grid[0].Length, _v1.y + _grid.Length);
        }
    }
}
