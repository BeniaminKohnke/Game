namespace GameAPI
{
    public enum Types
    {
        None,
        Player,
        Rock,
        Tree,
        Building
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
        Building1,
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

    public enum Animations
    {
        NoAction,
        MovingLeft,
        MovingRight,
    }

    public enum Directions
    {
        None,
        Up,
        Down,
        Left,
        Right,
    }

    public class Parameters
    {
        public static readonly Dictionary<Animations, States[]> Animations = new()
        {
            {
                GameAPI.Animations.MovingLeft,
                new[]
                {
                    States.MovingLeft1,
                    States.MovingLeft2,
                    States.MovingLeft3,
                }
            },
            {
                GameAPI.Animations.MovingRight,
                new[]
                {
                    States.MovingRight1,
                    States.MovingRight2,
                    States.MovingRight3,
                }
            },
            {
                GameAPI.Animations.NoAction,
                new[]
                {
                    States.NoAction1,
                    States.NoAction2,
                    States.NoAction3,
                }
            },
        };
    }

    public class GameObjectComparer : IComparer<GameObject>
    {
        public int Compare(GameObject? first, GameObject? second)
        {
            if (first != null && second != null)
            {
                var differenceY = first.RelativeY - second.RelativeY;
                if (differenceY < 0)
                {
                    return -1;
                }
                if (differenceY > 0)
                {
                    return 1;
                }

                var differenceX = first.X - second.X;
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
