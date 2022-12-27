namespace GameAPI
{
    public enum Types : byte
    {
        None,
        Player,
        Rock,
        Tree,
        Bush,
        Grass,
        Building,
        Item,
        Enemy,
    }

    public enum ItemTypes : byte
    {
        None,
        Melee,
        Ranged,
        Consumable,
        Material,
        Amunition,
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
        Grass1,
        Bush1,
        Building1,
        Pickaxe,
        Axe,
        Sword,
        Bow,
        HealthPotion,
        Arrow,
        ItemRock,
        ItemWood,
        ItemFruit,
        ItemString,
        ItemStick,
        ItemFiber,
        Enemy1,
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
        North,
        South,
        West,
        East,
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
        MeleeRange,
        AttackDelay,
        Healing,
    }
}
