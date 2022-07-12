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
            this.TexturePanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // TexturePanel
            // 
            this.TexturePanel.AutoScroll = true;
            this.TexturePanel.AutoSize = true;
            this.TexturePanel.Location = new System.Drawing.Point(0, 0);
            this.TexturePanel.Name = "TexturePanel";
            this.TexturePanel.Size = new System.Drawing.Size(2048, 2048);
            this.TexturePanel.TabIndex = 15;
            // 
            // SpriteMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 661);
            this.Controls.Add(this.TexturePanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.IsMdiContainer = true;
            this.Name = "SpriteMaker";
            this.Text = "SpriteMaker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Panel TexturePanel;
    }
}