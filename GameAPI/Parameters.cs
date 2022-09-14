namespace GameAPI
{
    public enum Types : byte
    {
        None,
        Player,
        Rock,
        Tree,
        Building,
        Item,
    }

    public enum ItemTypes : byte
    {
        None,
        Mele,
        Ranged,
    }

    public enum Grids : byte
    {
        Player,
        Rock1,
        Rock2,
        Rock3,
        Tree1,
        Tree2,
        Tree3,
        Building1,
        Pickaxe,
        Axe,
        Sword,
        ItemRock,
        ItemWood,
    }

    public enum States : byte
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

    public enum Animations : byte
    {
        NoAction,
        MovingLeft,
        MovingRight,
    }

    public enum Directions : byte
    {
        None,
        Up,
        Down,
        Left,
        Right,
    }

    public enum ObjectsParameters : byte
    {
        None,
        Health,
        MovementSpeed,
        BluntDamage,
        ThrustDamage,
        CuttingDamage,
        BluntDamageResistance,
        ThrustDamageResistance,
        CuttingDamageResistance,
        ScanRadius,
        Loot,
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
