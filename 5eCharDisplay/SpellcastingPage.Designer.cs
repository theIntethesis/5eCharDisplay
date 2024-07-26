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
            NameLabel = new System.Windows.Forms.Label();
            NameID = new System.Windows.Forms.Label();
            SpellSlotsLabel = new System.Windows.Forms.Label();
            SpellcastingAbilityBox = new System.Windows.Forms.GroupBox();
            SpellcastingAbilityLabel = new System.Windows.Forms.Label();
            SpellSaveBox = new System.Windows.Forms.GroupBox();
            SpellSaveLabel = new System.Windows.Forms.Label();
            SpellAttackBox = new System.Windows.Forms.GroupBox();
            SpellAttackLabel = new System.Windows.Forms.Label();
            SpellcastingAbilityBox.SuspendLayout();
            SpellSaveBox.SuspendLayout();
            SpellAttackBox.SuspendLayout();
            SuspendLayout();
            // 
            // NameLabel
            // 
            NameLabel.AutoSize = true;
            NameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            NameLabel.Location = new System.Drawing.Point(77, 8);
            NameLabel.Name = "NameLabel";
            NameLabel.Size = new System.Drawing.Size(173, 20);
            NameLabel.TabIndex = 3;
            NameLabel.Text = "{Name Displayed Here}";
            // 
            // NameID
            // 
            NameID.AutoSize = true;
            NameID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            NameID.Location = new System.Drawing.Point(10, 8);
            NameID.Name = "NameID";
            NameID.Size = new System.Drawing.Size(55, 20);
            NameID.TabIndex = 2;
            NameID.Text = "Name:";
            // 
            // SpellSlotsLabel
            // 
            SpellSlotsLabel.AutoSize = true;
            SpellSlotsLabel.Location = new System.Drawing.Point(13, 42);
            SpellSlotsLabel.Name = "SpellSlotsLabel";
            SpellSlotsLabel.Size = new System.Drawing.Size(329, 15);
            SpellSlotsLabel.TabIndex = 4;
            SpellSlotsLabel.Text = "1st   |   2nd   |   3rd   |   4th   |   5th   |   6th   |   7th   |   8th   |   9th";
            // 
            // SpellcastingAbilityBox
            // 
            SpellcastingAbilityBox.Controls.Add(SpellcastingAbilityLabel);
            SpellcastingAbilityBox.Location = new System.Drawing.Point(398, 11);
            SpellcastingAbilityBox.Name = "SpellcastingAbilityBox";
            SpellcastingAbilityBox.Size = new System.Drawing.Size(118, 66);
            SpellcastingAbilityBox.TabIndex = 5;
            SpellcastingAbilityBox.TabStop = false;
            SpellcastingAbilityBox.Text = "Spellcasting Ability";
            // 
            // SpellcastingAbilityLabel
            // 
            SpellcastingAbilityLabel.AutoSize = true;
            SpellcastingAbilityLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            SpellcastingAbilityLabel.Location = new System.Drawing.Point(31, 29);
            SpellcastingAbilityLabel.Name = "SpellcastingAbilityLabel";
            SpellcastingAbilityLabel.Size = new System.Drawing.Size(51, 15);
            SpellcastingAbilityLabel.TabIndex = 0;
            SpellcastingAbilityLabel.Text = "ABI (+X)";
            // 
            // SpellSaveBox
            // 
            SpellSaveBox.Controls.Add(SpellSaveLabel);
            SpellSaveBox.Location = new System.Drawing.Point(522, 11);
            SpellSaveBox.Name = "SpellSaveBox";
            SpellSaveBox.Size = new System.Drawing.Size(97, 66);
            SpellSaveBox.TabIndex = 6;
            SpellSaveBox.TabStop = false;
            SpellSaveBox.Text = "Spell Save DC";
            // 
            // SpellSaveLabel
            // 
            SpellSaveLabel.AutoSize = true;
            SpellSaveLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            SpellSaveLabel.Location = new System.Drawing.Point(38, 29);
            SpellSaveLabel.Name = "SpellSaveLabel";
            SpellSaveLabel.Size = new System.Drawing.Size(23, 15);
            SpellSaveLabel.TabIndex = 0;
            SpellSaveLabel.Text = "XX";
            // 
            // SpellAttackBox
            // 
            SpellAttackBox.Controls.Add(SpellAttackLabel);
            SpellAttackBox.Location = new System.Drawing.Point(624, 11);
            SpellAttackBox.Name = "SpellAttackBox";
            SpellAttackBox.Size = new System.Drawing.Size(118, 66);
            SpellAttackBox.TabIndex = 7;
            SpellAttackBox.TabStop = false;
            SpellAttackBox.Text = "Spell Attack Bonus";
            // 
            // SpellAttackLabel
            // 
            SpellAttackLabel.AutoSize = true;
            SpellAttackLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            SpellAttackLabel.Location = new System.Drawing.Point(31, 29);
            SpellAttackLabel.Name = "SpellAttackLabel";
            SpellAttackLabel.Size = new System.Drawing.Size(22, 15);
            SpellAttackLabel.TabIndex = 0;
            SpellAttackLabel.Text = "+X";
            // 
            // SpellcastingPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(947, 686);
            Controls.Add(SpellAttackBox);
            Controls.Add(SpellSaveBox);
            Controls.Add(SpellcastingAbilityBox);
            Controls.Add(SpellSlotsLabel);
            Controls.Add(NameLabel);
            Controls.Add(NameID);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "SpellcastingPage";
            Text = "SpellcastingPage";
            SpellcastingAbilityBox.ResumeLayout(false);
            SpellcastingAbilityBox.PerformLayout();
            SpellSaveBox.ResumeLayout(false);
            SpellSaveBox.PerformLayout();
            SpellAttackBox.ResumeLayout(false);
            SpellAttackBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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