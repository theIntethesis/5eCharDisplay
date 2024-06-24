namespace _5eCharDisplay
{
    partial class SpellcastingPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpellcastingPage));
            this.NameLabel = new System.Windows.Forms.Label();
            this.NameID = new System.Windows.Forms.Label();
            this.SpellSlotsLabel = new System.Windows.Forms.Label();
            this.SpellcastingAbilityBox = new System.Windows.Forms.GroupBox();
            this.SpellcastingAbilityLabel = new System.Windows.Forms.Label();
            this.SpellSaveBox = new System.Windows.Forms.GroupBox();
            this.SpellSaveLabel = new System.Windows.Forms.Label();
            this.SpellAttackBox = new System.Windows.Forms.GroupBox();
            this.SpellAttackLabel = new System.Windows.Forms.Label();
            this.SpellcastingAbilityBox.SuspendLayout();
            this.SpellSaveBox.SuspendLayout();
            this.SpellAttackBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameLabel.Location = new System.Drawing.Point(88, 9);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(216, 25);
            this.NameLabel.TabIndex = 3;
            this.NameLabel.Text = "{Name Displayed Here}";
            // 
            // NameID
            // 
            this.NameID.AutoSize = true;
            this.NameID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameID.Location = new System.Drawing.Point(12, 9);
            this.NameID.Name = "NameID";
            this.NameID.Size = new System.Drawing.Size(70, 25);
            this.NameID.TabIndex = 2;
            this.NameID.Text = "Name:";
            // 
            // SpellSlotsLabel
            // 
            this.SpellSlotsLabel.AutoSize = true;
            this.SpellSlotsLabel.Location = new System.Drawing.Point(15, 45);
            this.SpellSlotsLabel.Name = "SpellSlotsLabel";
            this.SpellSlotsLabel.Size = new System.Drawing.Size(335, 16);
            this.SpellSlotsLabel.TabIndex = 4;
            this.SpellSlotsLabel.Text = "1st   |   2nd   |   3rd   |   4th   |   5th   |   6th   |   7th   |   8th   |   9" +
    "th";
            // 
            // SpellcastingAbilityBox
            // 
            this.SpellcastingAbilityBox.Controls.Add(this.SpellcastingAbilityLabel);
            this.SpellcastingAbilityBox.Location = new System.Drawing.Point(455, 12);
            this.SpellcastingAbilityBox.Name = "SpellcastingAbilityBox";
            this.SpellcastingAbilityBox.Size = new System.Drawing.Size(135, 70);
            this.SpellcastingAbilityBox.TabIndex = 5;
            this.SpellcastingAbilityBox.TabStop = false;
            this.SpellcastingAbilityBox.Text = "Spellcasting Ability";
            // 
            // SpellcastingAbilityLabel
            // 
            this.SpellcastingAbilityLabel.AutoSize = true;
            this.SpellcastingAbilityLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpellcastingAbilityLabel.Location = new System.Drawing.Point(35, 31);
            this.SpellcastingAbilityLabel.Name = "SpellcastingAbilityLabel";
            this.SpellcastingAbilityLabel.Size = new System.Drawing.Size(63, 18);
            this.SpellcastingAbilityLabel.TabIndex = 0;
            this.SpellcastingAbilityLabel.Text = "ABI (+X)";
            // 
            // SpellSaveBox
            // 
            this.SpellSaveBox.Controls.Add(this.SpellSaveLabel);
            this.SpellSaveBox.Location = new System.Drawing.Point(596, 12);
            this.SpellSaveBox.Name = "SpellSaveBox";
            this.SpellSaveBox.Size = new System.Drawing.Size(111, 70);
            this.SpellSaveBox.TabIndex = 6;
            this.SpellSaveBox.TabStop = false;
            this.SpellSaveBox.Text = "Spell Save DC";
            // 
            // SpellSaveLabel
            // 
            this.SpellSaveLabel.AutoSize = true;
            this.SpellSaveLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpellSaveLabel.Location = new System.Drawing.Point(43, 31);
            this.SpellSaveLabel.Name = "SpellSaveLabel";
            this.SpellSaveLabel.Size = new System.Drawing.Size(28, 18);
            this.SpellSaveLabel.TabIndex = 0;
            this.SpellSaveLabel.Text = "XX";
            // 
            // SpellAttackBox
            // 
            this.SpellAttackBox.Controls.Add(this.SpellAttackLabel);
            this.SpellAttackBox.Location = new System.Drawing.Point(713, 12);
            this.SpellAttackBox.Name = "SpellAttackBox";
            this.SpellAttackBox.Size = new System.Drawing.Size(135, 70);
            this.SpellAttackBox.TabIndex = 7;
            this.SpellAttackBox.TabStop = false;
            this.SpellAttackBox.Text = "Spell Attack Bonus";
            // 
            // SpellAttackLabel
            // 
            this.SpellAttackLabel.AutoSize = true;
            this.SpellAttackLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpellAttackLabel.Location = new System.Drawing.Point(35, 31);
            this.SpellAttackLabel.Name = "SpellAttackLabel";
            this.SpellAttackLabel.Size = new System.Drawing.Size(27, 18);
            this.SpellAttackLabel.TabIndex = 0;
            this.SpellAttackLabel.Text = "+X";
            // 
            // SpellcastingPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1082, 818);
            this.Controls.Add(this.SpellAttackBox);
            this.Controls.Add(this.SpellSaveBox);
            this.Controls.Add(this.SpellcastingAbilityBox);
            this.Controls.Add(this.SpellSlotsLabel);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.NameID);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SpellcastingPage";
            this.Text = "SpellcastingPage";
            this.SpellcastingAbilityBox.ResumeLayout(false);
            this.SpellcastingAbilityBox.PerformLayout();
            this.SpellSaveBox.ResumeLayout(false);
            this.SpellSaveBox.PerformLayout();
            this.SpellAttackBox.ResumeLayout(false);
            this.SpellAttackBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Label NameID;
        private System.Windows.Forms.Label SpellSlotsLabel;
        private System.Windows.Forms.GroupBox SpellcastingAbilityBox;
        private System.Windows.Forms.Label SpellcastingAbilityLabel;
        private System.Windows.Forms.GroupBox SpellSaveBox;
        private System.Windows.Forms.Label SpellSaveLabel;
        private System.Windows.Forms.GroupBox SpellAttackBox;
        private System.Windows.Forms.Label SpellAttackLabel;
    }
}