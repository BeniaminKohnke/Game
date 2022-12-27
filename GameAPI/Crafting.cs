using GameAPI.GameObjects;

namespace GameAPI
{
    public enum Items
    {
        None,
        Rock,
        Wood,
        Fruit,
        HealthPotion,
        Bow,
        Sword,
        Axe,
        Pickaxe,
        Fiber,
        String,
        Stick,
        Arrow,
    }

    public static class Crafting
    {
        private static readonly Dictionary<Items, Grids> _itemsGrids = new()
        {
            [Items.HealthPotion] = Grids.HealthPotion,
            [Items.Pickaxe] = Grids.Pickaxe,
            [Items.Sword] = Grids.Sword,
            [Items.Axe] = Grids.Axe,
            [Items.Bow] = Grids.Bow,
            [Items.Arrow] = Grids.Arrow,
            [Items.Rock] = Grids.ItemRock,
            [Items.Wood] = Grids.ItemWood,
            [Items.Fruit] = Grids.ItemFruit,
            [Items.Fiber] = Grids.ItemFiber,
            [Items.String] = Grids.ItemString,
            [Items.Stick] = Grids.ItemStick,
        };
        private static readonly Dictionary<Items, Dictionary<ObjectsParameters, object>> _parameters = new()
        {
            [Items.Arrow] = new Dictionary<ObjectsParameters, object>
            {
                [ObjectsParameters.MovementSpeed] = 20,
                [ObjectsParameters.ThrustDamage] = (ushort)30,
            },
            [Items.Sword] = new Dictionary<ObjectsParameters, object>
            {
                [ObjectsParameters.CuttingDamage] = (ushort)50,
            },
            [Items.Axe] = new Dictionary<ObjectsParameters, object>
            {
                [ObjectsParameters.CuttingDamage] = (ushort)30,
            },
            [Items.Pickaxe] = new Dictionary<ObjectsParameters, object>
            {
                [ObjectsParameters.ThrustDamage] = (ushort)30,
            },
            [Items.Fruit] = new Dictionary<ObjectsParameters, object>
            {
                [ObjectsParameters.Healing] = (ushort)5,
            },
            [Items.HealthPotion] = new Dictionary<ObjectsParameters, object>
            {
                [ObjectsParameters.Healing] = (ushort)100,
            }
        };
        private static readonly Dictionary<Items, ItemTypes> _itemTypes = new()
        {
            [Items.Bow] = ItemTypes.Ranged,
            [Items.Sword] = ItemTypes.Melee,
            [Items.Pickaxe] = ItemTypes.Melee,
            [Items.Axe] = ItemTypes.Melee,
            [Items.Arrow] = ItemTypes.Amunition,
            [Items.HealthPotion] = ItemTypes.Consumable,
            [Items.Fruit] = ItemTypes.Consumable,
            [Items.Wood] = ItemTypes.Material,
            [Items.Rock] = ItemTypes.Material,
            [Items.Fiber] = ItemTypes.Material,
            [Items.Stick] = ItemTypes.Material,
            [Items.String] = ItemTypes.Material,
        };
        public static Dictionary<Items, (Items item, ushort count)[]> Recepies { get; } = new()
        {
            [Items.HealthPotion] = new[]
            {
                (Items.Fruit, (ushort)10),
            },
            [Items.Bow] = new[]
            {
                (Items.Stick, (ushort)20),
                (Items.String, (ushort)10),
            },
            [Items.Arrow] = new[]
            {
                (Items.Stick, (ushort)1),
                (Items.Rock, (ushort)1),
            },
            [Items.Stick] = new[]
            {
                (Items.Wood, (ushort)1),
            },
            [Items.String] = new[]
            {
                (Items.Fiber, (ushort)10),
            },
            [Items.Pickaxe] = new[]
            {
                (Items.Stick, (ushort)5),
                (Items.String, (ushort)3),
                (Items.Rock, (ushort)10),
            },
            [Items.Axe] = new[]
            {
                (Items.Stick, (ushort)7),
                (Items.String, (ushort)6),
                (Items.Rock, (ushort)5),
            },
            [Items.Sword] = new[]
            {
                (Items.Stick, (ushort)1),
                (Items.String, (ushort)10),
                (Items.Rock, (ushort)20),
            },
        };

        public static bool CraftItem(Player player, Items item)
        {
            if (Recepies.TryGetValue(item, out var recipe))
            {
                var isCraftable = true;
                foreach (var (name, count) in recipe)
                {
                    if (player.Items.Count(i => i.Name == name) < count)
                    {
                        isCraftable = false;
                        break;
                    }
                }

                if (isCraftable)
                {
                    foreach (var (name, count) in recipe)
                    {
                        for (var i = 0; i < count; i++)
                        {
                            player.Items.Remove(player.Items.First(i => i.Name == name));
                        }
                    }

                    var go = new Item(0, 0, Types.Item, _itemsGrids[item])
                    {
                        IsActive = false,
                        Name = item,
                        ItemType = _itemTypes[item],
                    };

                    if (_parameters.TryGetValue(item, out var parameters))
                    {
                        foreach (var parameter in parameters)
                        {
                            go.ObjectParameters[parameter.Key] = parameter.Value;
                        }
                    }

                    player.Items.Add(go);
                    return true;
                }
            }

            return false;
        }
    }
}
