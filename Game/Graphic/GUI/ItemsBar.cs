using GameAPI;
using SFML.Graphics;
using SFML.Window;

namespace Game.Graphic.GUI
{
    internal sealed class ItemsBar : Page
    {
        private readonly Sprite _itemsBarSprite = new();
        private readonly Sprite _cursorSprite = new();
        private readonly Dictionary<Grids, Icons> _gridsIcons = new()
        {
            [Grids.Axe] = Icons.Axe,
            [Grids.Pickaxe] = Icons.Pickaxe,
        };
        private readonly Dictionary<Icons, Sprite> _iconsSprites = new()
        {
            [Icons.Axe] = new(),
            [Icons.Pickaxe] = new(),
        };
        internal ItemsBar()
        {
            var grid = File
                .ReadAllLines($@"{Interface._texturesDirectory}\{Textures.ItemsBar}.sm")
                .Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray())
                .ToArray();
            if (grid.Length > 0 && grid[0].Length > 0)
            {
                _itemsBarSprite.Texture = new(Engine.CreateImage(grid));
            }

            grid = File
                .ReadAllLines($@"{Interface._texturesDirectory}\{Textures.ItemsBarCursor}.sm")
                .Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray())
                .ToArray();
            if (grid.Length > 0 && grid[0].Length > 0)
            {
                _cursorSprite.Texture = new(Engine.CreateImage(grid));
            }

            foreach (var icon in Enum.GetValues(typeof(Icons)))
            {
                grid = File
                    .ReadAllLines($@"{Interface._iconsDirectory}\{icon}.sm")
                    .Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray())
                    .ToArray();
                if (grid.Length > 0 && grid[0].Length > 0)
                {
                    _iconsSprites[(Icons)icon].Texture = new(Engine.CreateImage(grid));
                }
            }
        }

        internal override void Draw(RenderWindow window, GameWorld? world)
        {
            if (world != null)
            {
                _itemsBarSprite.Position = new(world.Player.Position.x - 33, 59 + world.Player.Position.y);
                window.Draw(_itemsBarSprite);

                _cursorSprite.Position = new(_itemsBarSprite.Position.X + world.Player.SelectedPosition * 8 + 1, _itemsBarSprite.Position.Y + 1);
                window.Draw(_cursorSprite);

                for (var i = 0; i < world.Player.ItemsMenu.Length; i++)
                {
                    var item = world.Player.Items.FirstOrDefault(it => it.Id == world.Player.ItemsMenu[i]);
                    if (item != null)
                    {
                        var iconType = _gridsIcons[item.Grid];
                        if (_iconsSprites.TryGetValue(iconType, out var sprite))
                        {
                            sprite.Position = new(_itemsBarSprite.Position.X + i * 8 + 2, _itemsBarSprite.Position.Y + 2);
                            window.Draw(sprite);
                        }
                    }
                }
            }
        }

        internal override bool HandleInput(KeyEventArgs args)
        {
            throw new NotImplementedException();
        }

        internal override void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
