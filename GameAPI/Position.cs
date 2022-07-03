namespace GameAPI
{
    public class Position
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int OriginShiftX { get; set; } = 0;
        public int OriginShiftY { get; set; } = 0;
        public int HorizontalColliderLength { get; set; } = 0;
        public int VerticalColliderLength { get; set; } = 0;
        public int ShiftedX => X + OriginShiftX;
        public int ShiftedY => Y + OriginShiftY;
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int GetDistance(int x, int y) => (int)Math.Sqrt(Math.Pow(X - x, 2) + Math.Pow(Y - y, 2));
        public int GetDistance(Position other, bool shift = false) => !shift ? (int)Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2))
            : (int)Math.Sqrt(Math.Pow(ShiftedX - other.ShiftedX, 2) + Math.Pow(ShiftedY - other.ShiftedY, 2));
    }
}
