namespace SpriteMaker
{
    partial class SpriteMaker : Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SaveButton = new System.Windows.Forms.Button();
            this.ChoiceBox = new System.Windows.Forms.GroupBox();
            this.ContourButton = new System.Windows.Forms.RadioButton();
            this.ColliderButton = new System.Windows.Forms.RadioButton();
            this.FillingButton = new System.Windows.Forms.RadioButton();
            this.TransparentButton = new System.Windows.Forms.RadioButton();
            this.ExistingTexturesBox = new System.Windows.Forms.ListBox();
            this.FolderPathBox = new System.Windows.Forms.TextBox();
            this.FolderPathLabel = new System.Windows.Forms.Label();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.LoadButton = new System.Windows.Forms.Button();
            this.ChoiceBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(653, 93);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(89, 23);
            this.SaveButton.TabIndex = 2;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            // 
            // ChoiceBox
            // 
            this.ChoiceBox.Controls.Add(this.ContourButton);
            this.ChoiceBox.Controls.Add(this.ColliderButton);
            this.ChoiceBox.Controls.Add(this.FillingButton);
            this.ChoiceBox.Controls.Add(this.TransparentButton);
            this.ChoiceBox.Location = new System.Drawing.Point(653, 12);
            this.ChoiceBox.Name = "ChoiceBox";
            this.ChoiceBox.Size = new System.Drawing.Size(206, 75);
            this.ChoiceBox.TabIndex = 4;
            this.ChoiceBox.TabStop = false;
            this.ChoiceBox.Text = "Options";
            // 
            // ContourButton
            // 
            this.ContourButton.AutoSize = true;
            this.ContourButton.Checked = true;
            this.ContourButton.Location = new System.Drawing.Point(106, 47);
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
            this.ColliderButton.Location = new System.Drawing.Point(106, 22);
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
            this.FillingButton.Location = new System.Drawing.Point(6, 46);
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
            this.TransparentButton.Location = new System.Drawing.Point(6, 22);
            this.TransparentButton.Name = "TransparentButton";
            this.TransparentButton.Size = new System.Drawing.Size(86, 19);
            this.TransparentButton.TabIndex = 0;
            this.TransparentButton.TabStop = true;
            this.TransparentButton.Text = "Transparent";
            this.TransparentButton.UseVisualStyleBackColor = true;
            // 
            // ExistingTexturesBox
            // 
            this.ExistingTexturesBox.FormattingEnabled = true;
            this.ExistingTexturesBox.ItemHeight = 15;
            this.ExistingTexturesBox.Location = new System.Drawing.Point(653, 258);
            this.ExistingTexturesBox.Name = "ExistingTexturesBox";
            this.ExistingTexturesBox.Size = new System.Drawing.Size(206, 379);
            this.ExistingTexturesBox.TabIndex = 5;
            // 
            // FolderPathBox
            // 
            this.FolderPathBox.Location = new System.Drawing.Point(653, 229);
            this.FolderPathBox.Name = "FolderPathBox";
            this.FolderPathBox.Size = new System.Drawing.Size(206, 23);
            this.FolderPathBox.TabIndex = 6;
            this.FolderPathBox.Text = "C:\\Users\\benia\\Documents\\GitHub\\Game\\Game\\Textures";
            // 
            // FolderPathLabel
            // 
            this.FolderPathLabel.AutoSize = true;
            this.FolderPathLabel.Location = new System.Drawing.Point(653, 204);
            this.FolderPathLabel.Name = "FolderPathLabel";
            this.FolderPathLabel.Size = new System.Drawing.Size(111, 15);
            this.FolderPathLabel.TabIndex = 7;
            this.FolderPathLabel.Text = "Textures folder path";
            // 
            // RefreshButton
            // 
            this.RefreshButton.Location = new System.Drawing.Point(770, 200);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(89, 23);
            this.RefreshButton.TabIndex = 8;
            this.RefreshButton.Text = "Refresh";
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(770, 93);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(89, 23);
            this.LoadButton.TabIndex = 9;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // SpriteMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 649);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.FolderPathLabel);
            this.Controls.Add(this.FolderPathBox);
            this.Controls.Add(this.ExistingTexturesBox);
            this.Controls.Add(this.ChoiceBox);
            this.Controls.Add(this.SaveButton);
            this.Name = "SpriteMaker";
            this.Text = "SpriteMaker";
            this.ChoiceBox.ResumeLayout(false);
            this.ChoiceBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button SaveButton;
        private GroupBox ChoiceBox;
        private RadioButton ContourButton;
        private RadioButton ColliderButton;
        private RadioButton FillingButton;
        private RadioButton TransparentButton;
        private ListBox ExistingTexturesBox;
        private TextBox FolderPathBox;
        private Label FolderPathLabel;
        private Button RefreshButton;
        private Button LoadButton;
    }
}