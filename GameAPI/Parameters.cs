﻿namespace GameAPI
{
    public enum ObjectsTypes
    {
        None,
        Player,
        Rock,
        Tree
    }

    public enum TexturesTypes
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

    public class Parameters
    {
        public readonly Dictionary<string, (ObjectsTypes, object)> DynamicObjects = new();
    }

    public class GameObjectPositionComparer : IComparer<GameObject>
    {
        public int Compare(GameObject? x, GameObject? y)
        {
            if (x != null && y != null)
            {
                var differenceY = x.Y - y.Y;
                if (differenceY < 0.001)
                {
                    return -1;
                }
                if (differenceY > 0.001)
                {
                    return 1;
                }

                var differenceX = x.X - y.X;
                if (differenceX > 0.001)
                {
                    return -1;
                }
                if (differenceX < 0.001)
                {
                    return 1;
                }
            }

            return 0;
        }
    }
}
