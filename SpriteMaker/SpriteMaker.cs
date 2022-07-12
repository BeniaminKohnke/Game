namespace SpriteMaker
{
    public partial class SpriteMaker : Form
    {
        private readonly OptionsMenu _menu;
        private readonly Grid _grid = new()
        {
            Location = new(5, 5),
        };

        public SpriteMaker()
        {
            InitializeComponent();

            this.IsMdiContainer = true;
            _menu = new(_grid)
            {
                MdiParent = this,
                Location = new(0,0),
            };
            _menu.Show();
            this.TexturePanel.Controls.Add(_menu);
            this.TexturePanel.Controls.Add(_grid);
            this.AutoScroll = true;
        }
    }
}