namespace SpriteMaker
{
    partial class OptionsMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.KeywordLabel = new System.Windows.Forms.Label();
            this.KeywordBox = new System.Windows.Forms.TextBox();
            this.XLabel = new System.Windows.Forms.Label();
            this.HeightBox = new System.Windows.Forms.NumericUpDown();
            this.WidthBox = new System.Windows.Forms.NumericUpDown();
            this.LoadButton = new System.Windows.Forms.Button();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.FolderPathLabel = new System.Windows.Forms.Label();
            this.FolderPathBox = new System.Windows.Forms.TextBox();
            this.ExistingTexturesBox = new System.Windows.Forms.ListBox();
            this.ChoiceBox = new System.Windows.Forms.GroupBox();
            this.TransparentColliderButton = new System.Windows.Forms.RadioButton();
            this.FillingColliderButton = new System.Windows.Forms.RadioButton();
            this.ContourButton = new System.Windows.Forms.RadioButton();
            this.ColliderButton = new System.Windows.Forms.RadioButton();
            this.FillingButton = new System.Windows.Forms.RadioButton();
            this.TransparentButton = new System.Windows.Forms.RadioButton();
            this.SaveButton = new System.Windows.Forms.Button();
            this.Pages = new System.Windows.Forms.TabControl();
            this.FilePage = new System.Windows.Forms.TabPage();
            this.OptionsBox = new System.Windows.Forms.ComboBox();
            this.ToolsPage = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.HeightBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WidthBox)).BeginInit();
            this.ChoiceBox.SuspendLayout();
            this.Pages.SuspendLayout();
            this.FilePage.SuspendLayout();
            this.ToolsPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // KeywordLabel
            // 
            this.KeywordLabel.AutoSize = true;
            this.KeywordLabel.Location = new System.Drawing.Point(6, 112);
            this.KeywordLabel.Name = "KeywordLabel";
            this.KeywordLabel.Size = new System.Drawing.Size(83, 15);
            this.KeywordLabel.TabIndex = 26;
            this.KeywordLabel.Text = "Keyword filter:";
            // 
            // KeywordBox
            // 
            this.KeywordBox.Location = new System.Drawing.Point(6, 130);
            this.KeywordBox.Name = "KeywordBox";
            this.KeywordBox.Size = new System.Drawing.Size(206, 23);
            this.KeywordBox.TabIndex = 25;
            this.KeywordBox.TextChanged += new System.EventHandler(this.KeywordBox_TextChanged);
            // 
            // XLabel
            // 
            this.XLabel.AutoSize = true;
            this.XLabel.Location = new System.Drawing.Point(101, 309);
            this.XLabel.Name = "XLabel";
            this.XLabel.Size = new System.Drawing.Size(14, 15);
            this.XLabel.TabIndex = 24;
            this.XLabel.Text = "X";
            // 
            // HeightBox
            // 
            this.HeightBox.Location = new System.Drawing.Point(122, 307);
            this.HeightBox.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.HeightBox.Name = "HeightBox";
            this.HeightBox.Size = new System.Drawing.Size(89, 23);
            this.HeightBox.TabIndex = 23;
            this.HeightBox.ValueChanged += new System.EventHandler(this.HeightBox_ValueChanged);
            // 
            // WidthBox
            // 
            this.WidthBox.Location = new System.Drawing.Point(6, 307);
            this.WidthBox.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.WidthBox.Name = "WidthBox";
            this.WidthBox.Size = new System.Drawing.Size(89, 23);
            this.WidthBox.TabIndex = 22;
            this.WidthBox.ValueChanged += new System.EventHandler(this.WidthBox_ValueChanged);
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(76, 6);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(64, 23);
            this.LoadButton.TabIndex = 21;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // RefreshButton
            // 
            this.RefreshButton.Location = new System.Drawing.Point(146, 6);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(64, 23);
            this.RefreshButton.TabIndex = 20;
            this.RefreshButton.Text = "Refresh";
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // FolderPathLabel
            // 
            this.FolderPathLabel.AutoSize = true;
            this.FolderPathLabel.Location = new System.Drawing.Point(7, 68);
            this.FolderPathLabel.Name = "FolderPathLabel";
            this.FolderPathLabel.Size = new System.Drawing.Size(111, 15);
            this.FolderPathLabel.TabIndex = 19;
            this.FolderPathLabel.Text = "Textures folder path";
            // 
            // FolderPathBox
            // 
            this.FolderPathBox.Location = new System.Drawing.Point(7, 86);
            this.FolderPathBox.Name = "FolderPathBox";
            this.FolderPathBox.Size = new System.Drawing.Size(206, 23);
            this.FolderPathBox.TabIndex = 18;
            // 
            // ExistingTexturesBox
            // 
            this.ExistingTexturesBox.FormattingEnabled = true;
            this.ExistingTexturesBox.ItemHeight = 15;
            this.ExistingTexturesBox.Location = new System.Drawing.Point(6, 159);
            this.ExistingTexturesBox.Name = "ExistingTexturesBox";
            this.ExistingTexturesBox.Size = new System.Drawing.Size(206, 169);
            this.ExistingTexturesBox.TabIndex = 17;
            // 
            // ChoiceBox
            // 
            this.ChoiceBox.Controls.Add(this.TransparentColliderButton);
            this.ChoiceBox.Controls.Add(this.FillingColliderButton);
            this.ChoiceBox.Controls.Add(this.ContourButton);
            this.ChoiceBox.Controls.Add(this.ColliderButton);
            this.ChoiceBox.Controls.Add(this.FillingButton);
            this.ChoiceBox.Controls.Add(this.TransparentButton);
            this.ChoiceBox.Location = new System.Drawing.Point(6, 6);
            this.ChoiceBox.Name = "ChoiceBox";
            this.ChoiceBox.Size = new System.Drawing.Size(206, 90);
            this.ChoiceBox.TabIndex = 16;
            this.ChoiceBox.TabStop = false;
            this.ChoiceBox.Text = "Options";
            // 
            // TransparentColliderButton
            // 
            this.TransparentColliderButton.AutoSize = true;
            this.TransparentColliderButton.Location = new System.Drawing.Point(106, 65);
            this.TransparentColliderButton.Name = "TransparentColliderButton";
            this.TransparentColliderButton.Size = new System.Drawing.Size(101, 19);
            this.TransparentColliderButton.TabIndex = 5;
            this.TransparentColliderButton.TabStop = true;
            this.TransparentColliderButton.Text = "Transp collider";
            this.TransparentColliderButton.UseVisualStyleBackColor = true;
            // 
            // FillingColliderButton
            // 
            this.FillingColliderButton.AutoSize = true;
            this.FillingColliderButton.Location = new System.Drawing.Point(106, 43);
            this.FillingColliderButton.Name = "FillingColliderButton";
            this.FillingColliderButton.Size = new System.Drawing.Size(99, 19);
            this.FillingColliderButton.TabIndex = 4;
            this.FillingColliderButton.TabStop = true;
            this.FillingColliderButton.Text = "Filling collider";
            this.FillingColliderButton.UseVisualStyleBackColor = true;
            // 
            // ContourButton
            // 
            this.ContourButton.AutoSize = true;
            this.ContourButton.Checked = true;
            this.ContourButton.Location = new System.Drawing.Point(6, 22);
            this.ContourButton.Name = "ContourButton";
            this.ContourButton.Size = new System.Drawing.Size(69, 19);
            this.ContourButton.TabIndex = 3;
            this.ContourButton.TabStop = true;
            this.ContourButton.Text = "Contour";
            this.ContourButton.UseVisualStyleBackColor = true;
            // 
            // ColliderButton
            // 
            this.ColliderButton.AutoSize = true;
            this.ColliderButton.Location = new System.Drawing.Point(106, 21);
            this.ColliderButton.Name = "ColliderButton";
            this.ColliderButton.Size = new System.Drawing.Size(66, 19);
            this.ColliderButton.TabIndex = 2;
            this.ColliderButton.TabStop = true;
            this.ColliderButton.Text = "Collider";
            this.ColliderButton.UseVisualStyleBackColor = true;
            // 
            // FillingButton
            // 
            this.FillingButton.AutoSize = true;
            this.FillingButton.Location = new System.Drawing.Point(6, 43);
            this.FillingButton.Name = "FillingButton";
            this.FillingButton.Size = new System.Drawing.Size(57, 19);
            this.FillingButton.TabIndex = 1;
            this.FillingButton.TabStop = true;
            this.FillingButton.Text = "Filling";
            this.FillingButton.UseVisualStyleBackColor = true;
            // 
            // TransparentButton
            // 
            this.TransparentButton.AutoSize = true;
            this.TransparentButton.Location = new System.Drawing.Point(6, 65);
            this.TransparentButton.Name = "TransparentButton";
            this.TransparentButton.Size = new System.Drawing.Size(86, 19);
            this.TransparentButton.TabIndex = 0;
            this.TransparentButton.TabStop = true;
            this.TransparentButton.Text = "Transparent";
            this.TransparentButton.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(6, 6);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(64, 23);
            this.SaveButton.TabIndex = 15;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // Pages
            // 
            this.Pages.Controls.Add(this.FilePage);
            this.Pages.Controls.Add(this.ToolsPage);
            this.Pages.Location = new System.Drawing.Point(2, 1);
            this.Pages.Name = "Pages";
            this.Pages.SelectedIndex = 0;
            this.Pages.Size = new System.Drawing.Size(227, 364);
            this.Pages.TabIndex = 27;
            // 
            // FilePage
            // 
            this.FilePage.Controls.Add(this.OptionsBox);
            this.FilePage.Controls.Add(this.ExistingTexturesBox);
            this.FilePage.Controls.Add(this.KeywordLabel);
            this.FilePage.Controls.Add(this.RefreshButton);
            this.FilePage.Controls.Add(this.KeywordBox);
            this.FilePage.Controls.Add(this.LoadButton);
            this.FilePage.Controls.Add(this.FolderPathLabel);
            this.FilePage.Controls.Add(this.FolderPathBox);
            this.FilePage.Controls.Add(this.SaveButton);
            this.FilePage.Location = new System.Drawing.Point(4, 24);
            this.FilePage.Name = "FilePage";
            this.FilePage.Padding = new System.Windows.Forms.Padding(3);
            this.FilePage.Size = new System.Drawing.Size(219, 336);
            this.FilePage.TabIndex = 0;
            this.FilePage.Text = "Files";
            this.FilePage.UseVisualStyleBackColor = true;
            // 
            // OptionsBox
            // 
            this.OptionsBox.FormattingEnabled = true;
            this.OptionsBox.Items.AddRange(new object[] {
            "API",
            "GUI"});
            this.OptionsBox.Location = new System.Drawing.Point(7, 42);
            this.OptionsBox.Name = "OptionsBox";
            this.OptionsBox.Size = new System.Drawing.Size(206, 23);
            this.OptionsBox.TabIndex = 27;
            this.OptionsBox.Text = "API";
            // 
            // ToolsPage
            // 
            this.ToolsPage.Controls.Add(this.ChoiceBox);
            this.ToolsPage.Controls.Add(this.XLabel);
            this.ToolsPage.Controls.Add(this.WidthBox);
            this.ToolsPage.Controls.Add(this.HeightBox);
            this.ToolsPage.Location = new System.Drawing.Point(4, 24);
            this.ToolsPage.Name = "ToolsPage";
            this.ToolsPage.Padding = new System.Windows.Forms.Padding(3);
            this.ToolsPage.Size = new System.Drawing.Size(219, 336);
            this.ToolsPage.TabIndex = 1;
            this.ToolsPage.Text = "Tools";
            this.ToolsPage.UseVisualStyleBackColor = true;
            // 
            // OptionsMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(238, 395);
            this.ControlBox = false;
            this.Controls.Add(this.Pages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "OptionsMenu";
            this.Text = "OptionsMenu";
            ((System.ComponentModel.ISupportInitialize)(this.HeightBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WidthBox)).EndInit();
            this.ChoiceBox.ResumeLayout(false);
            this.ChoiceBox.PerformLayout();
            this.Pages.ResumeLayout(false);
            this.FilePage.ResumeLayout(false);
            this.FilePage.PerformLayout();
            this.ToolsPage.ResumeLayout(false);
            this.ToolsPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Label KeywordLabel;
        private TextBox KeywordBox;
        private Label XLabel;
        private NumericUpDown HeightBox;
        private NumericUpDown WidthBox;
        private Button LoadButton;
        private Button RefreshButton;
        private Label FolderPathLabel;
        private TextBox FolderPathBox;
        private ListBox ExistingTexturesBox;
        private GroupBox ChoiceBox;
        private RadioButton TransparentColliderButton;
        private RadioButton FillingColliderButton;
        private RadioButton ContourButton;
        private RadioButton ColliderButton;
        private RadioButton FillingButton;
        private RadioButton TransparentButton;
        private Button SaveButton;
        private TabControl Pages;
        private TabPage FilePage;
        private TabPage ToolsPage;
        private ComboBox OptionsBox;
    }
}