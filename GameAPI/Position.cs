namespace GameAPI
{
    public class Position
    {
        public float X { get; set; } = 0f;

        public float Y { get; set; } = 0f;
        public float OriginShiftX { get; set; } = 0f;
        public float OriginShiftY { get; set; } = 0f;
        public float HorizontalColliderLength { get; set; } = 0f;
        public float VerticalColliderLength { get; set; } = 0f;
        public float ShiftedX => X + OriginShiftX;
        public float ShiftedY => Y + OriginShiftY;
        public Position(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float GetDistance(float x, float y) => (float)Math.Sqrt(Math.Pow(X - x, 2) + Math.Pow(Y - y, 2));
        public float GetDistance(Position other, bool shift = false) => !shift ? (float)Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2))
            : (float)Math.Sqrt(Math.Pow(ShiftedX - other.ShiftedX, 2) + Math.Pow(ShiftedY - other.ShiftedY, 2));
    }
}
