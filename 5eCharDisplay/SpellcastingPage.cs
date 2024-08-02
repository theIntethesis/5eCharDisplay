using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using _5eCharDisplay.Classes;
using System.Runtime.Versioning;

namespace _5eCharDisplay
{
    [SupportedOSPlatform("windows")]
    public partial class SpellcastingPage : Form
	{
		Character player;
		int numPrepared = 0, maxNumPrepared = 0;
		int classnumber;
		List<CheckBox> preparedBoxes = new List<CheckBox>();
		bool atMax = false;
		internal SpellcastingPage(Character PC, int Classnum)
		{
			InitializeComponent();
			player = PC;
			classnumber = Classnum;

			// Set Spellmod for quick referencing.
			int spellmod;
			switch (player.myClasses[Classnum].SpellcastingAbilityModifier)
			{
				case charClass.SpellMod.STR:
					spellmod = player.strength.getMod();
					break;
				case charClass.SpellMod.DEX:
					spellmod = player.dexterity.getMod();
					break;
				case charClass.SpellMod.CON:
					spellmod = player.constitution.getMod();
					break;
				case charClass.SpellMod.INT:
					spellmod = player.intelligence.getMod();
					break;
				case charClass.SpellMod.WIS:
					spellmod = player.wisdom.getMod();
					break;
				case charClass.SpellMod.CHA:
					spellmod = player.charisma.getMod();
					break;
				default:
					spellmod = 0;
					break;
			}

			// Add "Max Preprared Spells" Box 
			if (player.myClasses[Classnum].prepMethod == charClass.SpellPrepMethod.KnowSomePrepSome || player.myClasses[Classnum].prepMethod == charClass.SpellPrepMethod.KnowAllPrepSome)
			{
				GroupBox PrepNum = new GroupBox();
				PrepNum.Text = "Max Prepared Spells";
				PrepNum.Size = new Size(118, 66);
				PrepNum.Location = new Point(748, 11);
				Label label = new Label();
				maxNumPrepared = spellmod + player.myClasses[Classnum].SpellPrepLevel;
				label.Text = $"{maxNumPrepared}";
				PrepNum.Controls.Add(label);
				label.Location = new Point(35, 31);
				label.AutoSize = true;
				
				Controls.Add(PrepNum);
			}
			
			int firstY;
			int[] xVals = { 15, 50, 87, 163, 208, 256, 304, 350, 395 };

			// Set up Spell Slot CheckBoxes
			for(int i = 0; i < 9; i++)
			{
				if (player.myClasses[classnumber].spellSlotsMax[i] > 0)
				{
					firstY = 45;
					int check = player.myClasses[Classnum].spellSlots[i];
					for (int j = 0; j < player.myClasses[classnumber].spellSlotsMax[i]; j++)
					{
						CheckBox box = new CheckBox();
						box.Location = new Point(xVals[i], firstY += 20);
						box.Text = null;
						box.Size = new Size(17, 17);
                        if (player.myClasses[classnumber].classname == charClass.ClassName.Warlock)
							player.myClasses[classnumber].warlockSlotBoxes.Add(box);
						else
							player.myClasses[classnumber].spellSlotBoxes.Add(box);
						if (check > 0)
						{
							box.Checked = true;
							check--;
						}
						Controls.Add(box);
						switch (i)
						{
							case 0:
								box.CheckedChanged += ChangeFirstSlot;
								break;
							case 1:
								box.CheckedChanged += ChangeSecondSlot;
								break;
							case 2:
								box.CheckedChanged += ChangeThirdSlot;
								break;
							case 3:
								box.CheckedChanged += ChangeFourthSlot;
								break;
							case 4:
								box.CheckedChanged += ChangeFifthSlot;
								break;
							case 5:
								box.CheckedChanged += ChangeSixthSlot;
								break;
							case 6:
								box.CheckedChanged += ChangeSeventhSlot;
								break;
							case 7:
								box.CheckedChanged += ChangeEighthSlot;
								break;
							case 8:
								box.CheckedChanged += ChangeNinthSlot;
								break;
						}
					}
				}
			}

			int yDiff = 145;
			int xDiff = 20;

			charClass.SpellPrepMethod prepMethod = player.myClasses[classnumber].prepMethod;

			if(player.myClasses[Classnum].Cantrips.Count > 0)
			{
				Label FirstLabel = new Label();
				FirstLabel.Text = "Cantrips";
				FirstLabel.Size = new Size(250, 17);
				FirstLabel.Location = new Point(xDiff - 5, yDiff);
				FirstLabel.Font = new Font(FontFamily.GenericSansSerif, 9);
				Controls.Add(FirstLabel);
				yDiff += 20;
				foreach (string spell in player.myClasses[classnumber].Cantrips)
				{
					Label SpellName = new Label();
					SpellName.Text = spell;
					SpellName.Size = new Size(200, 17);
					SpellName.Location = new Point(xDiff + 20, yDiff);
					SpellName.MouseDown += DisplayOnRightClick;
					Controls.Add(SpellName);

					yDiff += 20;
					if (yDiff >= 660)
					{
						yDiff = 170;
						xDiff += 260;
					}
				}
			}
			
			if (player.myClasses[Classnum].FirstLevelSpells.Count > 0)
			{
				Label SpellLabel = new Label();
				SpellLabel.Text = "1st Level Spells";
				SpellLabel.Size = new Size(250, 17);
				SpellLabel.Location = new Point(xDiff - 5, yDiff);
				SpellLabel.Font = new Font(FontFamily.GenericSansSerif, 9);
				Controls.Add(SpellLabel);
				yDiff += 20;
				foreach (string spell in player.myClasses[classnumber].FirstLevelSpells)
				{
					Spell s = Spell.fromYAML(spell);
					if (prepMethod == charClass.SpellPrepMethod.KnowSomePrepNone)
					{
						Label prepared = new Label();
						prepared.Text = spell;
						if (s.ritual) prepared.Text += " [R]";
						prepared.Size = new Size(200, 20);
						prepared.Location = new Point(xDiff + 25, yDiff);
						prepared.MouseDown += DisplayOnRightClick;
						Controls.Add(prepared);
					}
					else
					{
						CheckBox prepared = new CheckBox();
						preparedBoxes.Add(prepared);
						prepared.Text = spell;
						if (s.ritual) prepared.Text += " [R]";
						prepared.Size = new Size(250, 20);
						prepared.AutoSize = true;
						prepared.Location = new Point(xDiff, yDiff);
						if (player.myClasses[classnumber].PreparedSpells.Contains(spell) || player.myClasses[classnumber].AlwaysPrepared.Contains(spell))
						{
							prepared.Checked = true;
							numPrepared++;
						}
						else if (player.myClasses[classnumber].AlwaysPrepared.Contains(spell))
							prepared.Checked = true;

						prepared.MouseDown += DisplayOnRightClick;
						prepared.CheckedChanged += TogglePrepared;
						Controls.Add(prepared);
					}

					yDiff += 20;
					if (yDiff >= 660)
					{
						yDiff = 170;
						xDiff += 260;
					}
				}
			}
			
			if (player.myClasses[Classnum].SecondLevelSpells.Count > 0)
			{
				Label SecondLabel = new Label();
				SecondLabel.Text = "2nd Level Spells";
				SecondLabel.Size = new Size(250, 20);
				SecondLabel.Location = new Point(xDiff - 5, yDiff);
				SecondLabel.Font = new Font(FontFamily.GenericSansSerif, 9);
				Controls.Add(SecondLabel);
				yDiff += 20;
				foreach (string spell in player.myClasses[classnumber].SecondLevelSpells)
				{
					Spell s = Spell.fromYAML(spell);
					if (prepMethod == charClass.SpellPrepMethod.KnowSomePrepNone)
					{
						Label prepared = new Label();
						prepared.Text = spell;
						if (s.ritual) prepared.Text += " [R]";
						prepared.Size = new Size(200, 20);
						prepared.Location = new Point(xDiff + 25, yDiff);
						prepared.MouseDown += DisplayOnRightClick;
						Controls.Add(prepared);
					}
					else
					{
						CheckBox prepared = new CheckBox();
						preparedBoxes.Add(prepared);
						prepared.Text = spell;
						if (s.ritual) prepared.Text += " [R]";
						prepared.Size = new Size(200, 20);
						prepared.AutoSize = true;
						prepared.Location = new Point(xDiff, yDiff);
						if (player.myClasses[classnumber].PreparedSpells.Contains(spell) || player.myClasses[classnumber].AlwaysPrepared.Contains(spell))
						{
							prepared.Checked = true;
							numPrepared++;
						}
						else if (player.myClasses[classnumber].AlwaysPrepared.Contains(spell))
							prepared.Checked = true;

						prepared.MouseDown += DisplayOnRightClick;
						prepared.CheckedChanged += TogglePrepared;
						Controls.Add(prepared);
					}

					yDiff += 20;
					if (yDiff >= 660)
					{
						yDiff = 170;
						xDiff += 260;
					}
				}
			}
			
			if(player.myClasses[Classnum].ThirdLevelSpells.Count > 0)
			{
				Label ThirdLabel = new Label();
				ThirdLabel.Text = "3rd Level Spells";
				ThirdLabel.Size = new Size(200, 17);
				ThirdLabel.Location = new Point(xDiff - 5, yDiff);
				ThirdLabel.Font = new Font(FontFamily.GenericSansSerif, 9);
				Controls.Add(ThirdLabel);
				yDiff += 20;
				foreach (string spell in player.myClasses[classnumber].ThirdLevelSpells)
				{
					Spell s = Spell.fromYAML(spell);
					if (prepMethod == charClass.SpellPrepMethod.KnowSomePrepNone)
					{
						Label prepared = new Label();
						prepared.Text = spell;
						if (s.ritual) prepared.Text += " [R]";
						prepared.Size = new Size(200, 20);
						prepared.Location = new Point(xDiff + 25, yDiff);
						prepared.MouseDown += DisplayOnRightClick;
						Controls.Add(prepared);
					}
					else
					{
						CheckBox prepared = new CheckBox();
						preparedBoxes.Add(prepared);
						prepared.Text = spell;
						if (s.ritual) prepared.Text += " [R]";
						prepared.Size = new Size(200, 20);
						prepared.AutoSize = true;
						prepared.Location = new Point(xDiff, yDiff);
						if (player.myClasses[classnumber].PreparedSpells.Contains(spell) || player.myClasses[classnumber].AlwaysPrepared.Contains(spell))
						{
							prepared.Checked = true;
							numPrepared++;
						}
						else if (player.myClasses[classnumber].AlwaysPrepared.Contains(spell))
							prepared.Checked = true;

						prepared.MouseDown += DisplayOnRightClick;
						prepared.CheckedChanged += TogglePrepared;
						Controls.Add(prepared);
					}

					yDiff += 20;
					if (yDiff >= 660)
					{
						yDiff = 170;
						xDiff += 260;
					}
				}
			}

			if (player.myClasses[Classnum].FourthLevelSpells.Count > 0)
			{
				Label FourthLabel = new Label();
				FourthLabel.Text = "4th Level Spells";
				FourthLabel.Size = new Size(250, 17);
				FourthLabel.Location = new Point(xDiff - 5, yDiff);
				FourthLabel.Font = new Font(FontFamily.GenericSansSerif, 9);
				Controls.Add(FourthLabel);
				yDiff += 20;
				foreach (string spell in player.myClasses[classnumber].FourthLevelSpells)
				{
					Spell s = Spell.fromYAML(spell);
					if (prepMethod == charClass.SpellPrepMethod.KnowSomePrepNone)
					{
						Label prepared = new Label();
						prepared.Text = spell;
						if (s.ritual) prepared.Text += " [R]";
						prepared.Size = new Size(200, 20);
						prepared.Location = new Point(xDiff + 25, yDiff);
						prepared.MouseDown += DisplayOnRightClick;
						Controls.Add(prepared);
					}
					else
					{
						CheckBox prepared = new CheckBox();
						preparedBoxes.Add(prepared);
						prepared.Text = spell;
						if (s.ritual) prepared.Text += " [R]";
						prepared.Size = new Size(200, 20);
						prepared.Location = new Point(xDiff, yDiff);
						if (player.myClasses[classnumber].PreparedSpells.Contains(spell) || player.myClasses[classnumber].AlwaysPrepared.Contains(spell))
						{
							prepared.Checked = true;
							numPrepared++;
						}
						else if (player.myClasses[classnumber].AlwaysPrepared.Contains(spell))
							prepared.Checked = true;

						prepared.MouseDown += DisplayOnRightClick;
						prepared.CheckedChanged += TogglePrepared;
						Controls.Add(prepared);
					}

					yDiff += 20;
					if (yDiff >= 660)
					{
						yDiff = 170;
						xDiff += 260;
					}
				}
			}

			if (player.myClasses[Classnum].FifthLevelSpells.Count > 0)
			{
				Label FourthLabel = new Label();
				FourthLabel.Text = "5th Level Spells";
				FourthLabel.Size = new Size(200, 17);
				FourthLabel.Location = new Point(xDiff - 5, yDiff);
				FourthLabel.Font = new Font(FontFamily.GenericSansSerif, 9);
				Controls.Add(FourthLabel);
				yDiff += 20;
				foreach (string spell in player.myClasses[classnumber].FifthLevelSpells)
				{
					Spell s = Spell.fromYAML(spell);
					if (prepMethod == charClass.SpellPrepMethod.KnowSomePrepNone)
					{
						Label prepared = new Label();
						prepared.Text = spell;
						if (s.ritual) prepared.Text += " [R]";
						prepared.Size = new Size(200, 20);
						prepared.Location = new Point(xDiff + 25, yDiff);
						prepared.MouseDown += DisplayOnRightClick;
						Controls.Add(prepared);
					}
					else
					{
						CheckBox prepared = new CheckBox();
						preparedBoxes.Add(prepared);
						prepared.Text = spell;
						if (s.ritual) prepared.Text += " [R]";
						prepared.Size = new Size(200, 20);
						prepared.Location = new Point(xDiff, yDiff);
						if (player.myClasses[classnumber].PreparedSpells.Contains(spell) || player.myClasses[classnumber].AlwaysPrepared.Contains(spell))
						{
							prepared.Checked = true;
							numPrepared++;
						}
						else if (player.myClasses[classnumber].AlwaysPrepared.Contains(spell))
							prepared.Checked = true;

						prepared.MouseDown += DisplayOnRightClick;
						prepared.CheckedChanged += TogglePrepared;
						Controls.Add(prepared);
					}

					yDiff += 20;
					if (yDiff >= 660)
					{
						yDiff = 170;
						xDiff += 260;
					}
				}
			}

			if (player.myClasses[Classnum].SixthLevelSpells.Count > 0)
			{
				Label FourthLabel = new Label();
				FourthLabel.Text = "6th Level Spells";
				FourthLabel.Size = new Size(200, 17);
				FourthLabel.Location = new Point(xDiff - 5, yDiff);
				FourthLabel.Font = new Font(FontFamily.GenericSansSerif, 9);
				Controls.Add(FourthLabel);
				yDiff += 20;
				foreach (string spell in player.myClasses[classnumber].SixthLevelSpells)
				{
					Spell s = Spell.fromYAML(spell);
					if (prepMethod == charClass.SpellPrepMethod.KnowSomePrepNone)
					{
						Label prepared = new Label();
						prepared.Text = spell;
						if (s.ritual) prepared.Text += " [R]";
						prepared.Size = new Size(200, 20);
						prepared.Location = new Point(xDiff + 25, yDiff);
						prepared.MouseDown += DisplayOnRightClick;
						Controls.Add(prepared);
					}
					else
					{
						CheckBox prepared = new CheckBox();
						preparedBoxes.Add(prepared);
						prepared.Text = spell;
						if (s.ritual) prepared.Text += " [R]";
						prepared.Size = new Size(200, 20);
						prepared.Location = new Point(xDiff, yDiff);
						if (player.myClasses[classnumber].PreparedSpells.Contains(spell) || player.myClasses[classnumber].AlwaysPrepared.Contains(spell))
						{
							prepared.Checked = true;
							numPrepared++;
						}
						else if (player.myClasses[classnumber].AlwaysPrepared.Contains(spell))
							prepared.Checked = true;

						prepared.MouseDown += DisplayOnRightClick;
						prepared.CheckedChanged += TogglePrepared;
						Controls.Add(prepared);
					}

					yDiff += 20;
					if (yDiff >= 660)
					{
						yDiff = 170;
						xDiff += 260;
					}
				}
			}

			if (player.myClasses[Classnum].SeventhLevelSpells.Count > 0)
			{
				Label FourthLabel = new Label();
				FourthLabel.Text = "7th Level Spells";
				FourthLabel.Size = new Size(200, 17);
				FourthLabel.Location = new Point(xDiff - 5, yDiff);
				FourthLabel.Font = new Font(FontFamily.GenericSansSerif, 9);
				Controls.Add(FourthLabel);
				yDiff += 20;
				foreach (string spell in player.myClasses[classnumber].SeventhLevelSpells)
				{
					Spell s = Spell.fromYAML(spell);
					if (prepMethod == charClass.SpellPrepMethod.KnowSomePrepNone)
					{
						Label prepared = new Label();
						prepared.Text = spell;
						if (s.ritual) prepared.Text += " [R]";
						prepared.Size = new Size(200, 20);
						prepared.Location = new Point(xDiff + 25, yDiff);
						prepared.MouseDown += DisplayOnRightClick;
						Controls.Add(prepared);
					}
					else
					{
						CheckBox prepared = new CheckBox();
						preparedBoxes.Add(prepared);
						prepared.Text = spell;
						if (s.ritual) prepared.Text += " [R]";
						prepared.Size = new Size(200, 20);
						prepared.Location = new Point(xDiff, yDiff);
						if (player.myClasses[classnumber].PreparedSpells.Contains(spell) || player.myClasses[classnumber].AlwaysPrepared.Contains(spell))
						{
							prepared.Checked = true;
							numPrepared++;
						}
						else if (player.myClasses[classnumber].AlwaysPrepared.Contains(spell))
							prepared.Checked = true;

						prepared.MouseDown += DisplayOnRightClick;
						prepared.CheckedChanged += TogglePrepared;
						Controls.Add(prepared);
					}

					yDiff += 20;
					if (yDiff >= 660)
					{
						yDiff = 170;
						xDiff += 260;
					}
				}
			}

			if (player.myClasses[Classnum].EighthLevelSpells.Count > 0)
			{
				Label FourthLabel = new Label();
				FourthLabel.Text = "8th Level Spells";
				FourthLabel.Size = new Size(200, 17);
				FourthLabel.Location = new Point(xDiff - 5, yDiff);
				FourthLabel.Font = new Font(FontFamily.GenericSansSerif, 9);
				Controls.Add(FourthLabel);
				yDiff += 20;
				foreach (string spell in player.myClasses[classnumber].EighthLevelSpells)
				{
					Spell s = Spell.fromYAML(spell);
					if (prepMethod == charClass.SpellPrepMethod.KnowSomePrepNone)
					{
						Label prepared = new Label();
						prepared.Text = spell;
						if (s.ritual) prepared.Text += " [R]";
						prepared.Size = new Size(200, 20);
						prepared.Location = new Point(xDiff + 25, yDiff);
						prepared.MouseDown += DisplayOnRightClick;
						Controls.Add(prepared);
					}
					else
					{
						CheckBox prepared = new CheckBox();
						preparedBoxes.Add(prepared);
						prepared.Text = spell;
						if (s.ritual) prepared.Text += " [R]";
						prepared.Size = new Size(200, 20);
						prepared.Location = new Point(xDiff, yDiff);
						if (player.myClasses[classnumber].PreparedSpells.Contains(spell) || player.myClasses[classnumber].AlwaysPrepared.Contains(spell))
						{
							prepared.Checked = true;
							numPrepared++;
						}
						else if (player.myClasses[classnumber].AlwaysPrepared.Contains(spell))
							prepared.Checked = true;

						prepared.MouseDown += DisplayOnRightClick;
						prepared.CheckedChanged += TogglePrepared;
						Controls.Add(prepared);
					}

					yDiff += 20;
					if (yDiff >= 660)
					{
						yDiff = 170;
						xDiff += 260;
					}
				}
			}

			if (player.myClasses[Classnum].NinthLevelSpells.Count > 0)
			{
				Label FourthLabel = new Label();
				FourthLabel.Text = "9th Level Spells";
				FourthLabel.Size = new Size(200, 17);
				FourthLabel.Location = new Point(xDiff - 5, yDiff);
				FourthLabel.Font = new Font(FontFamily.GenericSansSerif, 9);
				Controls.Add(FourthLabel);
				yDiff += 20;
				foreach (string spell in player.myClasses[classnumber].NinthLevelSpells)
				{
					Spell s = Spell.fromYAML(spell);
					if (prepMethod == charClass.SpellPrepMethod.KnowSomePrepNone)
					{
						Label prepared = new Label();
						prepared.Text = spell;
						if (s.ritual) prepared.Text += " [R]";
						prepared.Size = new Size(200, 20);
						prepared.Location = new Point(xDiff + 25, yDiff);
						prepared.MouseDown += DisplayOnRightClick;
						Controls.Add(prepared);
					}
					else
					{
						CheckBox prepared = new CheckBox();
						preparedBoxes.Add(prepared);
						prepared.Text = spell;
						if (s.ritual) prepared.Text += " [R]";
						prepared.Size = new Size(200, 20);
						prepared.Location = new Point(xDiff, yDiff);
						if (player.myClasses[classnumber].PreparedSpells.Contains(spell) || player.myClasses[classnumber].AlwaysPrepared.Contains(spell))
						{
							prepared.Checked = true;
							numPrepared++;
						}
						else if (player.myClasses[classnumber].AlwaysPrepared.Contains(spell))
							prepared.Checked = true;

						prepared.MouseDown += DisplayOnRightClick;
						prepared.CheckedChanged += TogglePrepared;
						Controls.Add(prepared);
					}

					yDiff += 20;
					if (yDiff >= 660)
					{
						yDiff = 170;
						xDiff += 260;
					}
				}
			}

			if (numPrepared >= maxNumPrepared && (player.myClasses[classnumber].prepMethod == charClass.SpellPrepMethod.KnowSomePrepSome || player.myClasses[classnumber].prepMethod == charClass.SpellPrepMethod.KnowAllPrepSome))
			{
				foreach (CheckBox c in preparedBoxes)
				{
					if (!c.Checked)
						c.Enabled = false;
					atMax = true;
				}
			}


			NameLabel.Text = player.name;
			Text = $"{player.name}'s Spellcasting Sheet - {player.myClasses[Classnum].classname}";
			SpellcastingAbilityLabel.Text = $"{player.myClasses[Classnum].SpellcastingAbilityModifier} (+{spellmod})";

			SpellAttackLabel.Text = $"+{spellmod + (int)Math.Ceiling(player.level.Sum() / 4.0) + 1}";
			SpellSaveLabel.Text = $"{8 + spellmod + (int)Math.Ceiling(player.level.Sum() / 4.0) + 1}";


			FormClosing += new FormClosingEventHandler(ExecOnClose);

		}

		#region ChangeNthSlot
		private void ChangeFirstSlot(object sender, EventArgs e)
		{
			int level = 0;
			CheckBox thisthing = sender as CheckBox;
			if (!thisthing.Checked)
				player.myClasses[classnumber].spellSlots[level]--;
			else
				player.myClasses[classnumber].spellSlots[level]++;
			return;
		}
		private void ChangeSecondSlot(object sender, EventArgs e)
		{
			int level = 1;
			CheckBox thisthing = sender as CheckBox;
			if (!thisthing.Checked)
				player.myClasses[classnumber].spellSlots[level]--;
			else
				player.myClasses[classnumber].spellSlots[level]++;
			return;
		}
		private void ChangeThirdSlot(object sender, EventArgs e)
		{
			int level = 2;
			CheckBox thisthing = sender as CheckBox;
			if (!thisthing.Checked)
				player.myClasses[classnumber].spellSlots[level]--;
			else
				player.myClasses[classnumber].spellSlots[level]++;
			return;
		}
		private void ChangeFourthSlot(object sender, EventArgs e)
		{
			int level = 3;
			CheckBox thisthing = sender as CheckBox;
			if (!thisthing.Checked)
				player.myClasses[classnumber].spellSlots[level]--;
			else
				player.myClasses[classnumber].spellSlots[level]++;
			return;
		}
		private void ChangeFifthSlot(object sender, EventArgs e)
		{
			int level = 4;
			CheckBox thisthing = sender as CheckBox;
			if (!thisthing.Checked)
				player.myClasses[classnumber].spellSlots[level]--;
			else
				player.myClasses[classnumber].spellSlots[level]++;
			return;
		}
		private void ChangeSixthSlot(object sender, EventArgs e)
		{
			int level = 5;
			CheckBox thisthing = sender as CheckBox;
			if (!thisthing.Checked)
				player.myClasses[classnumber].spellSlots[level]--;
			else
				player.myClasses[classnumber].spellSlots[level]++;
			return;
		}
		private void ChangeSeventhSlot(object sender, EventArgs e)
		{
			int level = 6;
			CheckBox thisthing = sender as CheckBox;
			if (!thisthing.Checked)
				player.myClasses[classnumber].spellSlots[level]--;
			else
				player.myClasses[classnumber].spellSlots[level]++;
			return;
		}
		private void ChangeEighthSlot(object sender, EventArgs e)
		{
			int level = 7;
			CheckBox thisthing = sender as CheckBox;
			if (!thisthing.Checked)
				player.myClasses[classnumber].spellSlots[level]--;
			else
				player.myClasses[classnumber].spellSlots[level]++;
			return;
		}
		private void ChangeNinthSlot(object sender, EventArgs e)
		{
			int level = 8;
			CheckBox thisthing = sender as CheckBox;
			if (!thisthing.Checked)
				player.myClasses[classnumber].spellSlots[level]--;
			else
				player.myClasses[classnumber].spellSlots[level]++;
			return;
		}
		#endregion ChangeNthSlot

		private void TogglePrepared(object sender, EventArgs e)
		{
			CheckBox thisthing = sender as CheckBox;
			string SpellName = thisthing.Text;
			if (SpellName.Contains("[R]"))
				SpellName = SpellName.Substring(0, SpellName.Length - 4);
			if (thisthing.Checked)
			{
				player.myClasses[classnumber].PreparedSpells.Add(SpellName);
				numPrepared++;
				if(numPrepared >= (maxNumPrepared + player.myClasses[classnumber].AlwaysPrepared.Count)&& (player.myClasses[classnumber].prepMethod == charClass.SpellPrepMethod.KnowSomePrepSome || player.myClasses[classnumber].prepMethod == charClass.SpellPrepMethod.KnowAllPrepSome))
				{
					foreach(CheckBox c in preparedBoxes)
					{
						if(!c.Checked)
							c.Enabled = false;
						atMax = true;
					}
				}
			}
			else
			{
				player.myClasses[classnumber].PreparedSpells.Remove(SpellName);
				numPrepared--;
				if (atMax)
				{
					foreach(var c in preparedBoxes)
					{
						c.Enabled = true;
						atMax = false;
					}
				}
			}
			return;
		}

		private void DisplayOnRightClick(object sender, EventArgs e)
		{
			MouseEventArgs mouse = e as MouseEventArgs;
			if(mouse.Button == MouseButtons.Right)
			{
				CheckBox the = sender as CheckBox;
				Spell theSpell;
				string SpellName;
				if(the == null)
				{
					Label cantrip = sender as Label;
					SpellName = cantrip.Text;
				}
				else
					SpellName = the.Text;
				theSpell = Spell.fromYAML(SpellName);
				Form from = new Form();
				from.Size = new Size(200, 600);
				//from.Icon = new Icon(@"C:\Users\Hayden\Downloads\881288450786082876.ico");
				from.Location = new Point(400, 50);
				Label SpellArgs = new Label();
				SpellArgs.Text = Controller.SPage_GetSpellDisplay(theSpell, player, classnumber);
				SpellArgs.Location = new Point(6, 12);
				SpellArgs.Size = new Size(175, 575);
				from.Controls.Add(SpellArgs);
				from.LostFocus += closeOnLostFocus;
				from.Show();
			}
		}

		private void closeOnLostFocus(object sender, EventArgs e)
		{
			Form form = sender as Form;
			form.Close();
		}

		private void ExecOnClose(object sender, CancelEventArgs e)
		{
			SaveSpellSlots();
		}

		private void SaveSpellSlots()
		{
			string[] classInfo = File.ReadAllLines($@".\Data\Characters\{player.name}\{player.name}{player.charClass[classnumber]}.yaml");
			(classInfo[0], classInfo[1]) = Controller.SPage_SaveSpellSlots(player, classnumber);
			File.WriteAllLines($@".\Data\Characters\{player.name}\{player.name}{player.charClass[classnumber]}.yaml", classInfo);
		}
	}
}
