using GameAPI;

namespace SpriteMaker
{
    public partial class SpriteMaker : Form
    {
        private Dictionary<string, string> _paths = new();
        private byte[][] _pixels = new byte[32][];

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
                        Location = new(20 * i + 5, 20 * j + 5),
                        Text = string.Empty,
                        ImageAlign = ContentAlignment.TopCenter,
                        BackColor = Color.Gray,
                        FlatStyle = FlatStyle.Flat
                    };
            
                    button.Click += new(ChangePixel);
                    Controls.Add(button);
                }
            }
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

                    if (TransparentButton.Checked)
                    {
                        _pixels[i][j] = 0;
                        button.BackColor = Color.Gray;
                    }
                    else if (FillingButton.Checked)
                    {
                        _pixels[i][j] = 1;
                        button.BackColor = Color.Black;
                    }
                    else if (ColliderButton.Checked)
                    {
                        _pixels[i][j] = 2;
                        button.BackColor = Color.Yellow;
                    }
                    else if (ContourButton.Checked)
                    {
                        _pixels[i][j] = 3;
                        button.BackColor = Color.White;
                    }
                }
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(FolderPathBox.Text))
            {
                foreach (var folder in Directory.GetDirectories(FolderPathBox.Text))
                {
                    var folderName = Path.GetFileName(folder);
                    var folderId = byte.Parse(folderName);
                    foreach(var file in Directory.GetFiles(folder).Where(f => f.Contains(".sm")))
                    {
                        var fileName = Path.GetFileName(file);
                        var fileId = byte.Parse(fileName.Replace(".sm", string.Empty));
                        var position = $"{Enum.GetName(typeof(TexturesTypes), folderId)}->{Enum.GetName(typeof(States), fileId)}";
                        ExistingTexturesBox.Items.Add(position);
                        _paths[position] = file;
                    }
                }
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            var fileName = ExistingTexturesBox.SelectedItem.ToString();
            if(!string.IsNullOrEmpty(fileName) && _paths.ContainsKey(fileName))
            {
                _pixels = File.ReadAllLines(_paths[fileName]).Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray()).ToArray();

                for(int i = 0; i < _pixels.Length; i++)
                {
                    for(int j = 0; j < _pixels[i].Length; j++)
                    {
                        Controls[$"{i}-{j}"].BackColor = GetColor(_pixels[i][j]);
                    }
                }
            }

            Color GetColor(byte value) => value switch
            {
                1 => Color.Black,
                2 => Color.Yellow,
                3 => Color.White,
                _ => Color.Gray,
            };
        }
    }
}