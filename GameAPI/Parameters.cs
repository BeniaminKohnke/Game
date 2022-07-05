namespace GameAPI
{
    public enum Types
    {
        None,
        Player,
        Rock,
        Tree
    }

    public enum Shapes
    {
        Player,
        Rock1,
        Rock2,
        Rock3,
        Tree1,
        Tree2,
        Tree3,
    }

    public enum States
    {
        NoAction1,
        NoAction2,
        NoAction3,
        MovingLeft1,
        MovingLeft2,
        MovingLeft3,
        MovingRight1,
        MovingRight2,
        MovingRight3,
    }

    public enum Directions
    {
        Up,
        Down,
        Left,
        Right,
    }

    public class Parameters
    {
        public readonly Dictionary<string, (Types, object)> DynamicObjects = new();
    }

    public class GameObjectPositionComparer : IComparer<GameObject>
    {
        public int Compare(GameObject? first, GameObject? second)
        {
            //if (x != null && y != null)
            //{
            //    var differenceY = x.Y - y.Y;
            //    if (differenceY < 0.001)
            //    {
            //        return -1;
            //    }
            //    if (differenceY > 0.001)
            //    {
            //        return 1;
            //    }
            //
            //    var differenceX = x.X - y.X;
            //    if (differenceX > 0.001)
            //    {
            //        return -1;
            //    }
            //    if (differenceX < 0.001)
            //    {
            //        return 1;
            //    }
            //}

            if (first != null && second != null)
            {
                var differenceY = first.GridRelativePositionY - second.GridRelativePositionY;
                if (differenceY < 0)
                {
                    return -1;
                }
                if (differenceY > 0)
                {
                    return 1;
                }

                var differenceX = first.GridPositionX - second.GridPositionX;
                if (differenceX > 0)
                {
                    return -1;
                }
                if (differenceX < 0)
                {
                    return 1;
                }
            }

            return (first != null && second != null) ? (first.Id > second.Id ? -1 : 1) : 0;
        }
    }
}
