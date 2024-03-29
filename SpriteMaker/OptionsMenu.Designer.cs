﻿namespace SpriteMaker
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
            this.XLabel = new System.Windows.Forms.Label();
            this.HeightBox = new System.Windows.Forms.NumericUpDown();
            this.WidthBox = new System.Windows.Forms.NumericUpDown();
            this.LoadButton = new System.Windows.Forms.Button();
            this.FolderPathLabel = new System.Windows.Forms.Label();
            this.FolderPathBox = new System.Windows.Forms.TextBox();
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
            this.TypeGroupBox = new System.Windows.Forms.ComboBox();
            this.ItemGroupBox = new System.Windows.Forms.ComboBox();
            this.OptionsBox = new System.Windows.Forms.ComboBox();
            this.ToolsPage = new System.Windows.Forms.TabPage();
            this.DeleteRowButton = new System.Windows.Forms.Button();
            this.DeleteColumnButton = new System.Windows.Forms.Button();
            this.InsertRowButton = new System.Windows.Forms.Button();
            this.InsertColumnButton = new System.Windows.Forms.Button();
            this.PixelSizeBox = new System.Windows.Forms.NumericUpDown();
            this.PixelSizeLabel = new System.Windows.Forms.Label();
            this.HeightLabel = new System.Windows.Forms.Label();
            this.WidthLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.HeightBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WidthBox)).BeginInit();
            this.ChoiceBox.SuspendLayout();
            this.Pages.SuspendLayout();
            this.FilePage.SuspendLayout();
            this.ToolsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PixelSizeBox)).BeginInit();
            this.SuspendLayout();
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
            // FolderPathLabel
            // 
            this.FolderPathLabel.AutoSize = true;
            this.FolderPathLabel.Location = new System.Drawing.Point(7, 68);
            this.FolderPathLabel.Name = "FolderPathLabel";
            this.FolderPathLabel.Size = new System.Drawing.Size(114, 15);
            this.FolderPathLabel.TabIndex = 19;
            this.FolderPathLabel.Text = "Textures folder path:";
            // 
            // FolderPathBox
            // 
            this.FolderPathBox.Location = new System.Drawing.Point(7, 86);
            this.FolderPathBox.Name = "FolderPathBox";
            this.FolderPathBox.Size = new System.Drawing.Size(206, 23);
            this.FolderPathBox.TabIndex = 18;
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
            this.FilePage.Controls.Add(this.TypeGroupBox);
            this.FilePage.Controls.Add(this.ItemGroupBox);
            this.FilePage.Controls.Add(this.OptionsBox);
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
            // TypeGroupBox
            // 
            this.TypeGroupBox.FormattingEnabled = true;
            this.TypeGroupBox.Location = new System.Drawing.Point(6, 144);
            this.TypeGroupBox.Name = "TypeGroupBox";
            this.TypeGroupBox.Size = new System.Drawing.Size(207, 23);
            this.TypeGroupBox.TabIndex = 29;
            // 
            // ItemGroupBox
            // 
            this.ItemGroupBox.FormattingEnabled = true;
            this.ItemGroupBox.Location = new System.Drawing.Point(7, 115);
            this.ItemGroupBox.Name = "ItemGroupBox";
            this.ItemGroupBox.Size = new System.Drawing.Size(206, 23);
            this.ItemGroupBox.TabIndex = 28;
            // 
            // OptionsBox
            // 
            this.OptionsBox.FormattingEnabled = true;
            this.OptionsBox.Items.AddRange(new object[] {
            "API",
            "GUI",
            "Icons"});
            this.OptionsBox.Location = new System.Drawing.Point(7, 42);
            this.OptionsBox.Name = "OptionsBox";
            this.OptionsBox.Size = new System.Drawing.Size(206, 23);
            this.OptionsBox.TabIndex = 27;
            this.OptionsBox.Text = "API";
            this.OptionsBox.SelectedIndexChanged += new System.EventHandler(this.OptionsBox_SelectedIndexChanged);
            // 
            // ToolsPage
            // 
            this.ToolsPage.Controls.Add(this.DeleteRowButton);
            this.ToolsPage.Controls.Add(this.DeleteColumnButton);
            this.ToolsPage.Controls.Add(this.InsertRowButton);
            this.ToolsPage.Controls.Add(this.InsertColumnButton);
            this.ToolsPage.Controls.Add(this.PixelSizeBox);
            this.ToolsPage.Controls.Add(this.PixelSizeLabel);
            this.ToolsPage.Controls.Add(this.HeightLabel);
            this.ToolsPage.Controls.Add(this.WidthLabel);
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
            // DeleteRowButton
            // 
            this.DeleteRowButton.Location = new System.Drawing.Point(112, 131);
            this.DeleteRowButton.Name = "DeleteRowButton";
            this.DeleteRowButton.Size = new System.Drawing.Size(101, 23);
            this.DeleteRowButton.TabIndex = 32;
            this.DeleteRowButton.Text = "Delete row";
            this.DeleteRowButton.UseVisualStyleBackColor = true;
            this.DeleteRowButton.Click += new System.EventHandler(this.DeleteRowButton_Click);
            // 
            // DeleteColumnButton
            // 
            this.DeleteColumnButton.Location = new System.Drawing.Point(112, 102);
            this.DeleteColumnButton.Name = "DeleteColumnButton";
            this.DeleteColumnButton.Size = new System.Drawing.Size(101, 23);
            this.DeleteColumnButton.TabIndex = 31;
            this.DeleteColumnButton.Text = "Delete column";
            this.DeleteColumnButton.UseVisualStyleBackColor = true;
            this.DeleteColumnButton.Click += new System.EventHandler(this.DeleteColumnButton_Click);
            // 
            // InsertRowButton
            // 
            this.InsertRowButton.Location = new System.Drawing.Point(6, 131);
            this.InsertRowButton.Name = "InsertRowButton";
            this.InsertRowButton.Size = new System.Drawing.Size(89, 23);
            this.InsertRowButton.TabIndex = 30;
            this.InsertRowButton.Text = "Insert row";
            this.InsertRowButton.UseVisualStyleBackColor = true;
            this.InsertRowButton.Click += new System.EventHandler(this.InsertRowButton_Click);
            // 
            // InsertColumnButton
            // 
            this.InsertColumnButton.Location = new System.Drawing.Point(6, 102);
            this.InsertColumnButton.Name = "InsertColumnButton";
            this.InsertColumnButton.Size = new System.Drawing.Size(89, 23);
            this.InsertColumnButton.TabIndex = 29;
            this.InsertColumnButton.Text = "Insert column";
            this.InsertColumnButton.UseVisualStyleBackColor = true;
            this.InsertColumnButton.Click += new System.EventHandler(this.InsertColumnButton_Click);
            // 
            // PixelSizeBox
            // 
            this.PixelSizeBox.Location = new System.Drawing.Point(122, 264);
            this.PixelSizeBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PixelSizeBox.Name = "PixelSizeBox";
            this.PixelSizeBox.Size = new System.Drawing.Size(89, 23);
            this.PixelSizeBox.TabIndex = 28;
            this.PixelSizeBox.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.PixelSizeBox.ValueChanged += new System.EventHandler(this.PixelSizeBox_ValueChanged);
            // 
            // PixelSizeLabel
            // 
            this.PixelSizeLabel.AutoSize = true;
            this.PixelSizeLabel.Location = new System.Drawing.Point(6, 266);
            this.PixelSizeLabel.Name = "PixelSizeLabel";
            this.PixelSizeLabel.Size = new System.Drawing.Size(57, 15);
            this.PixelSizeLabel.TabIndex = 27;
            this.PixelSizeLabel.Text = "Pixel size:";
            // 
            // HeightLabel
            // 
            this.HeightLabel.AutoSize = true;
            this.HeightLabel.Location = new System.Drawing.Point(122, 289);
            this.HeightLabel.Name = "HeightLabel";
            this.HeightLabel.Size = new System.Drawing.Size(46, 15);
            this.HeightLabel.TabIndex = 26;
            this.HeightLabel.Text = "Height:";
            // 
            // WidthLabel
            // 
            this.WidthLabel.AutoSize = true;
            this.WidthLabel.Location = new System.Drawing.Point(6, 289);
            this.WidthLabel.Name = "WidthLabel";
            this.WidthLabel.Size = new System.Drawing.Size(42, 15);
            this.WidthLabel.TabIndex = 25;
            this.WidthLabel.Text = "Width:";
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
            ((System.ComponentModel.ISupportInitialize)(this.PixelSizeBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Label XLabel;
        private NumericUpDown HeightBox;
        private NumericUpDown WidthBox;
        private Button LoadButton;
        private Label FolderPathLabel;
        private TextBox FolderPathBox;
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
        private Label PixelSizeLabel;
        private Label HeightLabel;
        private Label WidthLabel;
        private NumericUpDown PixelSizeBox;
        private ComboBox TypeGroupBox;
        private ComboBox ItemGroupBox;
        private Button InsertColumnButton;
        private Button InsertRowButton;
        private Button DeleteRowButton;
        private Button DeleteColumnButton;
    }
}