using Game;
using GameAPI;

namespace SpriteMaker
{
    public partial class OptionsMenu : Form
    {
        private readonly Dictionary<string, string> _paths = new();
        private readonly Grid _grid;
        public OptionsMenu(Grid grid)
        {
            InitializeComponent();
            _grid = grid;

            foreach (var control in ChoiceBox.Controls)
            {
                if (control is RadioButton button)
                {
                    button.CheckedChanged += new EventHandler(ChangeGridValue);
                }
            }

            FolderPathBox.Text = $@"C:\Users\benia\Documents\GitHub\Game\Game\bin\Debug\net6.0\Textures";
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            try
            {
                ExistingTexturesBox.Items.Clear();
                if (!string.IsNullOrEmpty(FolderPathBox.Text))
                {
                    switch(OptionsBox.Text)
                    {
                        case "API":
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
                                break;
                            }
                        case "GUI":
                            {
                                var dir = $@"{FolderPathBox.Text}\Inerface";
                                if (!Directory.Exists(dir))
                                {
                                    Directory.CreateDirectory(dir);
                                }

                                foreach (var file in Enum.GetValues(typeof(Controls)))
                                {
                                    var path = $@"{dir}\{file}.sm";
                                    if (!File.Exists(path))
                                    {
                                        File.Create(path);
                                    }

                                    var name = file.ToString();
                                    if(!string.IsNullOrEmpty(name))
                                    {
                                        ExistingTexturesBox.Items.Add(name);
                                        _paths[name] = path;
                                    }
                                }
                                break;
                            }
                    }
                }
            }
            catch
            {

            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            try
            {
                var fileName = ExistingTexturesBox.SelectedItem?.ToString();
                if (!string.IsNullOrEmpty(fileName) && _paths.ContainsKey(fileName))
                {
                    var grid = File.ReadAllLines(_paths[fileName]).Where(l => !string.IsNullOrEmpty(l)).Select(l => l.Split('\t').Select(p => byte.Parse(p)).ToArray()).ToArray();
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
                var fileName = ExistingTexturesBox.SelectedItem.ToString();
                if (!string.IsNullOrEmpty(fileName) && _paths.ContainsKey(fileName))
                {
                    File.WriteAllLines(_paths[fileName], _grid.GetGrid().Select(l => string.Join('\t', l.Where(v => v != 0))).Where(l => !string.IsNullOrEmpty(l)));
                }
            }
            catch
            {

            }
        }

        private void WidthBox_ValueChanged(object sender, EventArgs e) => _grid.GridWidth = (ushort)WidthBox.Value;

        private void HeightBox_ValueChanged(object sender, EventArgs e) => _grid.GridHeight = (ushort)HeightBox.Value;

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
    }
}
