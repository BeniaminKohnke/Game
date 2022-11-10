using Game.Graphic.GUI;
using GameAPI;

namespace SpriteMaker
{
    public partial class OptionsMenu : Form
    {
        private readonly Grid _grid;

        public OptionsMenu(Grid grid)
        {
            InitializeComponent();
            _grid = grid;

            foreach (var control in ChoiceBox.Controls)
            {
                if (control is RadioButton button)
                {
                    button.CheckedChanged += new(ChangeGridValue);
                }
            }

            FolderPathBox.Text = $@"C:\Users\benia\Documents\GitHub\Game\Game\bin\Debug\net7.0\Textures";
            OptionsBox_SelectedIndexChanged(null, null);
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            try
            {
                var dir = OptionsBox.Text switch
                {
                    "API" => $@"{FolderPathBox.Text}\{ItemGroupBox.Text}",
                    "GUI" => $@"{FolderPathBox.Text}\Interface",
                    "Icons" => $@"{FolderPathBox.Text}\Icons",
                    _ => string.Empty,
                };

                var filePath = $@"{dir}\{TypeGroupBox.Text}.sm";
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    var grid = File.ReadAllLines(filePath).Where(l => !string.IsNullOrEmpty(l)).Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray()).ToArray();
                    _grid.SetGrid(grid);
                    HeightBox.Value = _grid.GridHeight;
                    WidthBox.Value = _grid.GridWidth;
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
                var dir = OptionsBox.Text switch
                {
                    "API" => $@"{FolderPathBox.Text}\{ItemGroupBox.Text}",
                    "GUI" => $@"{FolderPathBox.Text}\Interface",
                    "Icons" => $@"{FolderPathBox.Text}\Icons",
                    _ => string.Empty,
                };

                if (!string.IsNullOrEmpty(dir))
                {
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    File.WriteAllLines($@"{dir}\{TypeGroupBox.Text}.sm", _grid.GetGrid().Select(l => string.Join('\t', l.Where(v => v != 0))).Where(l => !string.IsNullOrEmpty(l)));
                }
            }
            catch
            {

            }
        }

        private void WidthBox_ValueChanged(object sender, EventArgs e) => _grid.GridWidth = (ushort)WidthBox.Value;

        private void HeightBox_ValueChanged(object sender, EventArgs e) => _grid.GridHeight = (ushort)HeightBox.Value;

        private void PixelSizeBox_ValueChanged(object sender, EventArgs e) => _grid.PixelSize = (byte)PixelSizeBox.Value;

        private void ChangeGridValue(object? sender, EventArgs e)
        {
            if (TransparentButton.Checked)
            {
                _grid.Value = 7;
            }
            else if (FillingButton.Checked)
            {
                _grid.Value = 5;
            }
            else if (ColliderButton.Checked)
            {
                _grid.Value = 3;
            }
            else if (ContourButton.Checked)
            {
                _grid.Value = 4;
            }
            else if (FillingColliderButton.Checked)
            {
                _grid.Value = 2;
            }
            else if (TransparentColliderButton.Checked)
            {
                _grid.Value = 6;
            }
        }

        private void OptionsBox_SelectedIndexChanged(object? sender, EventArgs? e)
        {
            ItemGroupBox.Text = string.Empty;
            TypeGroupBox.Text = string.Empty;

            switch (OptionsBox.Text)
            {
                case "API":
                    {
                        ItemGroupBox.Items.Clear();
                        foreach (var item in Enum.GetValues(typeof(Grids)))
                        {
                            ItemGroupBox.Items.Add(item);
                        }

                        TypeGroupBox.Items.Clear();
                        foreach (var item in Enum.GetValues(typeof(States)))
                        {
                            TypeGroupBox.Items.Add(item);
                        }
                        break;
                    }
                case "GUI":
                    {
                        ItemGroupBox.Items.Clear();
                        TypeGroupBox.Items.Clear();
                        foreach (var item in Enum.GetValues(typeof(Textures)))
                        {
                            TypeGroupBox.Items.Add(item);
                        }
                        break;
                    }
                case "Icons":
                    {
                        ItemGroupBox.Items.Clear();
                        TypeGroupBox.Items.Clear();
                        foreach (var item in Enum.GetValues(typeof(Icons)))
                        {
                            TypeGroupBox.Items.Add(item);
                        }
                        break;
                    }
            }
        }

        private void InsertColumnButton_Click(object sender, EventArgs e)
        {
            if (_grid.TryAddColumn())
            {
                WidthBox.Value++;
            }
        }

        private void InsertRowButton_Click(object sender, EventArgs e)
        {
            if (_grid.TryAddRow())
            {
                HeightBox.Value++;
            }
        }

        private void DeleteColumnButton_Click(object sender, EventArgs e)
        {
            if (_grid.TryDeleteColumn())
            {
                WidthBox.Value--;
            }
        }

        private void DeleteRowButton_Click(object sender, EventArgs e)
        {
            if (_grid.TryDeleteRow())
            {
                HeightBox.Value--;
            }
        }
    }
}
