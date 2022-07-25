namespace GameAPI
{
    public enum Types
    {
        None,
        Player,
        Rock,
        Tree,
        Building,
        Item,
    }

    public enum ItemTypes
    {
        None,
        Mele,
        Ranged,
    }

    public enum Grids
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

    public enum ObjectsParameters
    {
        Health,
        MovementSpeed,
        BluntDamage,
        ThrustDamage,
        SlashDamage,
        BluntDamageResistance,
        ThrustDamageResistance,
        SlashDamageResistance,
        DamageAgainstEnemiesMultiplier,
        ScanRadius,
    }

    public static class Parameters
    {
        public static readonly Dictionary<Animations, States[]> Animations = new()
        {
            [GameAPI.Animations.MovingLeft] = new[]
            {
                States.MovingLeft1,
                States.MovingLeft2,
                States.MovingLeft3,
            },
            [GameAPI.Animations.MovingRight] = new[]
            {
                States.MovingRight1,
                States.MovingRight2,
                States.MovingRight3,
            },
            [GameAPI.Animations.NoAction] = new[]
            {
                States.NoAction1,
                States.NoAction2,
                States.NoAction3,
            },
        };
    }
}
