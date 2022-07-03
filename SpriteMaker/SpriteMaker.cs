using GameAPI;

namespace SpriteMaker
{
    public partial class SpriteMaker : Form
    {
        private Dictionary<string, string> _paths = new();
        private readonly byte[][] _pixels = new byte[32][];

        public SpriteMaker()
        {
            InitializeComponent();

            for(int i = 0; i < 32; i++)
            {
                _pixels[i] = new byte[32];
                for(int j = 0; j < 32; j++)
                {
                    _pixels[i][j] = 0;
                    var button = new Button
                    {
                        Name = $"{i}-{j}",
                        Size = new(20, 20),
                        Location = new(20 * j + 5, 20 * i + 5),
                        Text = string.Empty,
                        ImageAlign = ContentAlignment.TopCenter,
                        BackColor = Color.Gray,
                        FlatStyle = FlatStyle.Flat
                    };
            
                    button.Click += new(ChangePixel);
                    Controls.Add(button);
                }
            }

            SetGrid();
        }

        private void ChangePixel(object? sender, EventArgs e)
        {
            if(sender != null)
            {
                if (sender is Button button)
                {
                    var splitedName = button.Name.Split('-');
                    var i = byte.Parse(splitedName[0]);
                    var j = byte.Parse(splitedName[1]);

                    if(button.BackColor != Color.Gray)
                    {
                        if (TransparentButton.Checked)
                        {
                            _pixels[i][j] = 1;
                            button.BackColor = Color.LightYellow;
                        }
                        else if (FillingButton.Checked)
                        {
                            _pixels[i][j] = 2;
                            button.BackColor = Color.Black;
                        }
                        else if (ColliderButton.Checked)
                        {
                            _pixels[i][j] = 3;
                            button.BackColor = Color.Yellow;
                        }
                        else if (ContourButton.Checked)
                        {
                            _pixels[i][j] = 4;
                            button.BackColor = Color.White;
                        }
                    }
                }
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            ExistingTexturesBox.Items.Clear();
            if(!string.IsNullOrEmpty(FolderPathBox.Text))
            {
                foreach (var folder in Enum.GetValues(typeof(TexturesTypes)))
                {
                    var dir = $@"{FolderPathBox.Text}\{folder}";
                    if(!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    foreach(var file in Enum.GetValues(typeof(States)))
                    {
                        var path = $@"{dir}\{file}.sm";
                        if(!File.Exists(path))
                        {
                            File.Create(path);
                        }

                        var position = $"{Enum.GetName(typeof(TexturesTypes), folder)}->{Enum.GetName(typeof(States), file)}";
                        ExistingTexturesBox.Items.Add(position);
                        _paths[position] = path;
                    }
                }
            }
        }
        private static Color GetColor(byte value) => value switch
        {
            2 => Color.Black,
            3 => Color.Yellow,
            4 => Color.White,
            _ => Color.LightYellow,
        };

        private void LoadButton_Click(object sender, EventArgs e)
        {
            var fileName = ExistingTexturesBox.SelectedItem.ToString();
            if(!string.IsNullOrEmpty(fileName) && _paths.ContainsKey(fileName))
            {
                var pixels = File.ReadAllLines(_paths[fileName]).Where(l => !string.IsNullOrEmpty(l)).Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray()).ToArray();

                WidthBox.Value = pixels.Length;
                HeightBox.Value = pixels.Max(p => p.Length);

                for(int i = 0; i < pixels.Length; i++)
                {
                    for(int j = 0; j < pixels[i].Length; j++)
                    {
                        _pixels[i][j] = pixels[i][j];
                        Controls[$"{i}-{j}"].BackColor = GetColor(_pixels[i][j]);
                    }
                }

                for(int i = 0; i < _pixels.Length; i++)
                {
                    for(int j = 0; j < _pixels[i].Length; j++)
                    {
                        if (_pixels[i][j] == 0)
                        {
                            Controls[$"{i}-{j}"].BackColor = Color.Gray;
                        }
                    }
                }
            }

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var fileName = ExistingTexturesBox.SelectedItem.ToString();
            if (!string.IsNullOrEmpty(fileName) && _paths.ContainsKey(fileName))
            {
                File.WriteAllLines(_paths[fileName], _pixels.Select(l => string.Join('\t', l.Where(v => v != 0))).Where(l => !string.IsNullOrEmpty(l)));
            }
        }

        private void WidthBox_ValueChanged(object sender, EventArgs e) => SetGrid();

        private void HeightBox_ValueChanged(object sender, EventArgs e) => SetGrid();

        private void SetGrid()
        {
            var height = (byte)HeightBox.Value;
            var width = (byte)WidthBox.Value;
            for (int i = 0; i < _pixels.Length; i++)
            {
                for (int j = 0; j < _pixels[0].Length; j++)
                {
                    if (i < width && j < height)
                    {
                        if (Controls[$"{i}-{j}"].BackColor == Color.Gray)
                        {
                            _pixels[i][j] = 1;
                            Controls[$"{i}-{j}"].BackColor = Color.LightYellow;
                        }
                    }
                    else
                    {
                        _pixels[i][j] = 0;
                        Controls[$"{i}-{j}"].BackColor = Color.Gray;
                    }
                }
            }
        }

        private void KeywordBox_TextChanged(object sender, EventArgs e)
        {
            ExistingTexturesBox.Items.Clear();
            _paths.Keys.Where(k => k.Contains(KeywordBox.Text))?.ToList().ForEach(e => ExistingTexturesBox.Items.Add(e));
        }
    }
}