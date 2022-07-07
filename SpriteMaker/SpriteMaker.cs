using GameAPI;

namespace SpriteMaker
{
    public partial class SpriteMaker : Form
    {
        private readonly Dictionary<string, string> _paths = new();
        private readonly byte[][] _pixels = new byte[64][];

        public SpriteMaker()
        {
            InitializeComponent();

            for(int i = 0; i < 64; i++)
            {
                _pixels[i] = new byte[64];
                for(int j = 0; j < 64; j++)
                {
                    _pixels[i][j] = 0;
                    var button = new Button
                    {
                        Name = $"{i}-{j}",
                        Size = new(10, 10),
                        Location = new(10 * j + 5, 10 * i + 5),
                        Text = string.Empty,
                        ImageAlign = ContentAlignment.TopCenter,
                        BackColor = Color.Orange,
                        FlatStyle = FlatStyle.Flat,
                        Enabled = false,
                        Visible = false
                    };
            
                    button.Click += new(ChangePixel);
                    TexturePanel.Controls.Add(button);
                }
            }


            //FolderPathBox.Text = $@"{Directory.GetCurrentDirectory()}\Textures";
            //if (!Directory.Exists(FolderPathBox.Text))
            //{
            //    Directory.CreateDirectory(FolderPathBox.Text);
            //}
            FolderPathBox.Text = @"C:\Users\benia\Documents\GitHub\Game\Game\bin\Debug\net6.0\Textures";
            SetGrid();
            RefreshButton_Click(new object(), new EventArgs());
        }

        private void ChangePixel(object? sender, EventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    if (sender is Button button)
                    {
                        var splitedName = button.Name.Split('-');
                        var i = byte.Parse(splitedName[0]);
                        var j = byte.Parse(splitedName[1]);

                        if (button.BackColor != Color.Orange)
                        {
                            if (TransparentButton.Checked)
                            {
                                _pixels[i][j] = 7;
                                button.BackColor = Color.BlueViolet;
                            }
                            else if (FillingButton.Checked)
                            {
                                _pixels[i][j] = 5;
                                button.BackColor = Color.Gray;
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
                            else if (FillingColliderButton.Checked)
                            {
                                _pixels[i][j] = 2;
                                button.BackColor = Color.LightSteelBlue;
                            }
                            else if (TransparentColliderButton.Checked)
                            {
                                _pixels[i][j] = 6;
                                button.BackColor = Color.Fuchsia;
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            try
            {
                ExistingTexturesBox.Items.Clear();
                if (!string.IsNullOrEmpty(FolderPathBox.Text))
                {
                    foreach (var folder in Enum.GetValues(typeof(Grids)))
                    {
                        var dir = $@"{FolderPathBox.Text}\{folder}";
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        foreach (var file in Enum.GetValues(typeof(States)))
                        {
                            var path = $@"{dir}\{file}.sm";
                            if (!File.Exists(path))
                            {
                                File.Create(path);
                            }

                            var position = $"{Enum.GetName(typeof(Grids), folder)}->{Enum.GetName(typeof(States), file)}";
                            ExistingTexturesBox.Items.Add(position);
                            _paths[position] = path;
                        }
                    }
                }
            }
            catch
            {

            }
        }
        private static Color GetColor(byte value) => value switch
        {
            5 => Color.Gray,
            3 => Color.Yellow,
            4 => Color.White,
            2 => Color.LightSteelBlue,
            6 => Color.Fuchsia,
            _ => Color.BlueViolet,
        };

        private void LoadButton_Click(object sender, EventArgs e)
        {
            try
            {
                var fileName = ExistingTexturesBox.SelectedItem?.ToString();
                if(!string.IsNullOrEmpty(fileName) && _paths.ContainsKey(fileName))
                {
                    var pixels = File.ReadAllLines(_paths[fileName]).Where(l => !string.IsNullOrEmpty(l)).Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray()).ToArray();

                    WidthBox.Value = pixels.Length;
                    HeightBox.Value = pixels.Any(c => c.Any()) ? pixels.Max(p => p.Length) : 0;

                    for (int i = 0; i < pixels.Length; i++)
                    {
                        for (int j = 0; j < pixels[i].Length; j++)
                        {
                            _pixels[i][j] = pixels[i][j];
                            TexturePanel.Controls[$"{i}-{j}"].BackColor = GetColor(_pixels[i][j]);
                            TexturePanel.Controls[$"{i}-{j}"].Enabled = true;
                            TexturePanel.Controls[$"{i}-{j}"].Visible = true;
                        }
                    }

                    for (int i = 0; i < _pixels.Length; i++)
                    {
                        for (int j = 0; j < _pixels[i].Length; j++)
                        {
                            if (_pixels[i][j] == 0)
                            {
                                TexturePanel.Controls[$"{i}-{j}"].BackColor = Color.Orange;
                                TexturePanel.Controls[$"{i}-{j}"].Enabled = false;
                                TexturePanel.Controls[$"{i}-{j}"].Visible = false;
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                var fileName = ExistingTexturesBox.SelectedItem.ToString();
                if (!string.IsNullOrEmpty(fileName) && _paths.ContainsKey(fileName))
                {
                    File.WriteAllLines(_paths[fileName], _pixels.Select(l => string.Join('\t', l.Where(v => v != 0))).Where(l => !string.IsNullOrEmpty(l)));
                }
            }
            catch
            {

            }
        }

        private void WidthBox_ValueChanged(object sender, EventArgs e) => SetGrid();

        private void HeightBox_ValueChanged(object sender, EventArgs e) => SetGrid();

        private void SetGrid()
        {
            try
            {
                var height = (byte)HeightBox.Value;
                var width = (byte)WidthBox.Value;
                for (int i = 0; i < _pixels.Length; i++)
                {
                    for (int j = 0; j < _pixels[0].Length; j++)
                    {
                        if (i < width && j < height)
                        {
                            if (TexturePanel.Controls[$"{i}-{j}"].BackColor == Color.Orange)
                            {
                                _pixels[i][j] = 1;
                                TexturePanel.Controls[$"{i}-{j}"].BackColor = Color.BlueViolet;
                                TexturePanel.Controls[$"{i}-{j}"].Enabled = true;
                                TexturePanel.Controls[$"{i}-{j}"].Visible = true;
                            }
                        }
                        else
                        {
                            _pixels[i][j] = 0;
                            TexturePanel.Controls[$"{i}-{j}"].BackColor = Color.Orange;
                            TexturePanel.Controls[$"{i}-{j}"].Enabled = false;
                            TexturePanel.Controls[$"{i}-{j}"].Visible = false;
                        }
                    }
                }
            }
            catch
            {

            }
        }

        private void KeywordBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ExistingTexturesBox.Items.Clear();
                _paths.Keys.Where(k => k.Contains(KeywordBox.Text))?.ToList().ForEach(e => ExistingTexturesBox.Items.Add(e));
            }
            catch
            {

            }
        }
    }
}