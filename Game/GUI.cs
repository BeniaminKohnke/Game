using SFML.Graphics;

namespace Game
{
    public enum Controls
    {
        StatsMenu,
        CodeEditor,
    }

    public class GUI
    {
        public Dictionary<Controls, bool> States { get; } = new();
        private readonly (Controls control, Sprite sprite)[] _controls;
        private readonly Dictionary<Controls, (int x, int y)> _positions = new()
        {
            { Controls.StatsMenu, (200, 200) },
            { Controls.CodeEditor, (-119, -70) },
        };

        public GUI()
        {
            var controls = new List<(Controls, Sprite)>();
            var mainDir = $@"{Directory.GetCurrentDirectory()}\Textures\Interface";
            if (Directory.Exists(mainDir))
            {
                foreach (var file in Enum.GetValues(typeof(Controls)))
                {
                    var filePath = $@"{mainDir}\{file}.sm";
                    if (File.Exists(filePath))
                    {
                        var texture = File.ReadAllLines(filePath).Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray()).ToArray();
                        if (texture.Length > 0)
                        {
                            var grid = File.ReadAllLines(filePath).Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray()).ToArray();
                            if (grid.Length > 0 && grid[0].Length > 0)
                            {
                                var image = new Image((uint)grid[0].Length, (uint)grid.Length);

                                for (int i = 0; i < grid.Length; i++)
                                {
                                    for (int j = 0; j < grid[i].Length; j++)
                                    {
                                        image.SetPixel((uint)j, (uint)i, grid[i][j] switch
                                        {
                                            2 or 5 => Color.Black,
                                            3 or 4 => Color.White,
                                            _ => Color.Transparent,
                                        });
                                    }
                                }

                                controls.Add(((Controls)file, new()
                                {
                                    Texture = new(image),
                                }));

                                States[(Controls)file] = false;
                            }
                        }
                    }
                }
            }

            _controls = controls.ToArray();
        }

        public void Draw(RenderWindow window, int x, int y)
        {
            foreach (var (control, sprite) in _controls)
            {
                if (States[control])
                {
                    var position = _positions[control];
                    sprite.Position = new(position.x + x, position.y + y);
                    window.Draw(sprite);
                }
            }
        }
    }
}