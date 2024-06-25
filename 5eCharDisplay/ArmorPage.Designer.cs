namespace _5eCharDisplay
{
    partial class ArmorPage
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
            this.NameLabel = new System.Windows.Forms.Label();
            this.ACLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(100, 12);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(44, 16);
            this.NameLabel.TabIndex = 0;
            this.NameLabel.Text = "Name";
            // 
            // ACLabel
            // 
            this.ACLabel.AutoSize = true;
            this.ACLabel.Location = new System.Drawing.Point(200, 12);
            this.ACLabel.Name = "ACLabel";
            this.ACLabel.Size = new System.Drawing.Size(25, 16);
            this.ACLabel.TabIndex = 1;
            this.ACLabel.Text = "AC";
            // 
            // ArmorPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(207, 818);
            this.Controls.Add(this.ACLabel);
            this.Controls.Add(this.NameLabel);
            this.Name = "ArmorPage";
            this.Text = "ArmorPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Label ACLabel;
    }
}