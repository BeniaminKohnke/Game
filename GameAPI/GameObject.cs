namespace GameAPI
{
    public class GameObject : Position
    {
        private static uint _lastId = 0;
        public readonly uint ObjectId = _lastId++;

        public ushort Weight { get; set; } = 0;
        public int MovementSpeed { get; set; } = 0;
        public int MovementSpeedMultipiler { get; set; } = 1;
        public States State { get; set; }
        public ObjectsTypes ObjectType { get; set; }
        public TexturesTypes TextureType { get; set; }

        public GameObject(int x, int y) : base(x, y)
        {

        }

        public void HandleCollison(GameObject other)
        {
            //YOU MUST NOT CHANGE ORDER OF VARIABLES BECAUSE TRIGONOMETRIC FUNCTIONS CAN HAVE NEGATIVE VALUES
            var diagonal = GetDistance(other, true);
            var horizontal = other.ShiftedX - ShiftedX;
            var vertical = other.ShiftedY - ShiftedY;
        
            if (diagonal != 0)
            {
                var sin = (float)vertical / diagonal;
                var cos = (float)horizontal / diagonal;
        
                var differenceX = (HorizontalColliderLength + other.HorizontalColliderLength) - Math.Abs(horizontal);
                var differenceY = (VerticalColliderLength + other.VerticalColliderLength) - Math.Abs(vertical) + 10;
        
                if (0 <= differenceX && 0 <= differenceY)
                {
                    if (Weight > other.Weight)
                    {
                        if (Math.Abs(sin) > Math.Abs(cos) - 0.9)
                        {
                            if (differenceY > 0)
                            {
                                other.Y += (int)Math.Ceiling((sin * differenceY) + (sin > 0 ? 1 : -1));
                            }
                            else if (differenceX > 0)
                            {
                                other.X += (int)Math.Ceiling(cos * differenceX);
                            }
                        }
                        else
                        {
                            if (differenceX > 0)
                            {
                                other.X += (int)Math.Ceiling(cos * differenceX);
                            }
                            else if (differenceY > 0)
                            {
                                other.Y += (int)Math.Ceiling((sin * differenceY) + (sin > 0 ? 1 : -1));
                            }
                        }
                    }
                    else
                    {
                        if (Math.Abs(sin) > Math.Abs(cos) - 0.9)
                        {
                            if (differenceY > 0)
                            {
                                Y -= (int)Math.Ceiling((sin * differenceY) + (sin > 0 ? 1 : -1));
                            }
                            else if (differenceX > 0)
                            {
                                X -= (int)Math.Ceiling(cos * differenceX);
                            }
                        }
                        else
                        {
                            if (differenceX > 0)
                            {
                                X -= (int)Math.Ceiling(cos * differenceX);
                            }
                            else if (differenceY > 0)
                            {
                                Y -= (int)Math.Ceiling((sin * differenceY) + (sin > 0 ? 1 : -1));
                            }
                        }
                    }
                }
            }
        }
    }
}