using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Versioning;

namespace _5eCharDisplay
{
    [SupportedOSPlatform("windows")]
    public partial class SpellcastingPage : Form
	{
		Character player;
        charClass cClass;
		Spellcasting spellcasting;
        int numPrepared = 0, maxNumPrepared = 0;
		List<CheckBox> preparedBoxes = new List<CheckBox>();
		bool atMax = false;
		internal SpellcastingPage(Character PC, charClass cla)
		{
			InitializeComponent();
			player = PC;
			cClass = cla;
			spellcasting = cla.spellcasting;

			// Set Spellmod for quick referencing.
			int spellmod = spellcasting.spellcastingAbilityModifier.getMod();

			// Add "Max Preprared Spells" Box 
			if (spellcasting.prepMethod == Spellcasting.SpellPrepMethod.KnowSomePrepSome || spellcasting.prepMethod == Spellcasting.SpellPrepMethod.KnowAllPrepSome)
			{
				GroupBox PrepNum = new GroupBox();
				PrepNum.Text = "Max Prepared Spells";
				PrepNum.Size = new Size(118, 66);
				PrepNum.Location = new Point(748, 11);
				Label label = new Label();
				maxNumPrepared = spellmod + spellcasting.spellPrepLevel;
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
				if (cClass.spellcasting.spellSlotsMax[i] > 0)
				{
					firstY = 45;
					int check = spellcasting.spellSlots[i];
					for (int j = 0; j < cClass.spellcasting.spellSlotsMax[i]; j++)
					{
						CheckBox box = new CheckBox();
						box.Location = new Point(xVals[i], firstY += 20);
						box.Text = null;
						box.Size = new Size(17, 17);
                        if (cClass.classname == charClass.ClassName.Warlock)
							cClass.warlockSlotBoxes.Add(box);
						else
							cClass.spellSlotBoxes.Add(box);
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


			if(cClass.spellcasting.Cantrips.Count > 0)
			{
				Label FirstLabel = new Label();
				FirstLabel.Text = "Cantrips";
				FirstLabel.Size = new Size(250, 17);
				FirstLabel.Location = new Point(xDiff - 5, yDiff);
				FirstLabel.Font = new Font(FontFamily.GenericSansSerif, 9);
				Controls.Add(FirstLabel);
				yDiff += 20;
				foreach (string spell in cClass.spellcasting.Cantrips)
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
			
			if (cClass.spellcasting.FirstLevelSpells.Count > 0)
			{
				Label SpellLabel = new Label();
				SpellLabel.Text = "1st Level Spells";
				SpellLabel.Size = new Size(250, 17);
				SpellLabel.Location = new Point(xDiff - 5, yDiff);
				SpellLabel.Font = new Font(FontFamily.GenericSansSerif, 9);
				Controls.Add(SpellLabel);
				yDiff += 20;
				foreach (string spell in cClass.spellcasting.FirstLevelSpells)
				{
					Spell s = Spell.fromYAML(spell);
					if (spellcasting.prepMethod == Spellcasting.SpellPrepMethod.KnowSomePrepNone)
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
						if (cClass.spellcasting.PreparedSpells.Contains(spell) || cClass.spellcasting.AlwaysPrepared.Contains(spell))
						{
							prepared.Checked = true;
							numPrepared++;
						}
						else if (cClass.spellcasting.AlwaysPrepared.Contains(spell))
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
			
			if (cClass.spellcasting.SecondLevelSpells.Count > 0)
			{
				Label SecondLabel = new Label();
				SecondLabel.Text = "2nd Level Spells";
				SecondLabel.Size = new Size(250, 20);
				SecondLabel.Location = new Point(xDiff - 5, yDiff);
				SecondLabel.Font = new Font(FontFamily.GenericSansSerif, 9);
				Controls.Add(SecondLabel);
				yDiff += 20;
				foreach (string spell in cClass.spellcasting.SecondLevelSpells)
				{
					Spell s = Spell.fromYAML(spell);
					if (spellcasting.prepMethod == Spellcasting.SpellPrepMethod.KnowSomePrepNone)
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
						if (cClass.spellcasting.PreparedSpells.Contains(spell) || cClass.spellcasting.AlwaysPrepared.Contains(spell))
						{
							prepared.Checked = true;
							numPrepared++;
						}
						else if (cClass.spellcasting.AlwaysPrepared.Contains(spell))
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
			
			if (cClass.spellcasting.ThirdLevelSpells.Count > 0)
			{
				Label ThirdLabel = new Label();
				ThirdLabel.Text = "3rd Level Spells";
				ThirdLabel.Size = new Size(200, 17);
				ThirdLabel.Location = new Point(xDiff - 5, yDiff);
				ThirdLabel.Font = new Font(FontFamily.GenericSansSerif, 9);
				Controls.Add(ThirdLabel);
				yDiff += 20;
				foreach (string spell in cClass.spellcasting.ThirdLevelSpells)
				{
					Spell s = Spell.fromYAML(spell);
					if (spellcasting.prepMethod == Spellcasting.SpellPrepMethod.KnowSomePrepNone)
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
						if (cClass.spellcasting.PreparedSpells.Contains(spell) || cClass.spellcasting.AlwaysPrepared.Contains(spell))
						{
							prepared.Checked = true;
							numPrepared++;
						}
						else if (cClass.spellcasting.AlwaysPrepared.Contains(spell))
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

			if (cClass.spellcasting.FourthLevelSpells.Count > 0)
			{
				Label FourthLabel = new Label();
				FourthLabel.Text = "4th Level Spells";
				FourthLabel.Size = new Size(250, 17);
				FourthLabel.Location = new Point(xDiff - 5, yDiff);
				FourthLabel.Font = new Font(FontFamily.GenericSansSerif, 9);
				Controls.Add(FourthLabel);
				yDiff += 20;
				foreach (string spell in cClass.spellcasting.FourthLevelSpells)
				{
					Spell s = Spell.fromYAML(spell);
					if (spellcasting.prepMethod == Spellcasting.SpellPrepMethod.KnowSomePrepNone)
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
						if (cClass.spellcasting.PreparedSpells.Contains(spell) || cClass.spellcasting.AlwaysPrepared.Contains(spell))
						{
							prepared.Checked = true;
							numPrepared++;
						}
						else if (cClass.spellcasting.AlwaysPrepared.Contains(spell))
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

			if (cClass.spellcasting.FifthLevelSpells.Count > 0)
			{
				Label FourthLabel = new Label();
				FourthLabel.Text = "5th Level Spells";
				FourthLabel.Size = new Size(200, 17);
				FourthLabel.Location = new Point(xDiff - 5, yDiff);
				FourthLabel.Font = new Font(FontFamily.GenericSansSerif, 9);
				Controls.Add(FourthLabel);
				yDiff += 20;
				foreach (string spell in cClass.spellcasting.FifthLevelSpells)
				{
					Spell s = Spell.fromYAML(spell);
					if (spellcasting.prepMethod == Spellcasting.SpellPrepMethod.KnowSomePrepNone)
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
						if (cClass.spellcasting.PreparedSpells.Contains(spell) || cClass.spellcasting.AlwaysPrepared.Contains(spell))
						{
							prepared.Checked = true;
							numPrepared++;
						}
						else if (cClass.spellcasting.AlwaysPrepared.Contains(spell))
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

			if (cClass.spellcasting.SixthLevelSpells.Count > 0)
			{
				Label FourthLabel = new Label();
				FourthLabel.Text = "6th Level Spells";
				FourthLabel.Size = new Size(200, 17);
				FourthLabel.Location = new Point(xDiff - 5, yDiff);
				FourthLabel.Font = new Font(FontFamily.GenericSansSerif, 9);
				Controls.Add(FourthLabel);
				yDiff += 20;
				foreach (string spell in cClass.spellcasting.SixthLevelSpells)
				{
					Spell s = Spell.fromYAML(spell);
					if (spellcasting.prepMethod == Spellcasting.SpellPrepMethod.KnowSomePrepNone)
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
						if (cClass.spellcasting.PreparedSpells.Contains(spell) || cClass.spellcasting.AlwaysPrepared.Contains(spell))
						{
							prepared.Checked = true;
							numPrepared++;
						}
						else if (cClass.spellcasting.AlwaysPrepared.Contains(spell))
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

			if (cClass.spellcasting.SeventhLevelSpells.Count > 0)
			{
				Label FourthLabel = new Label();
				FourthLabel.Text = "7th Level Spells";
				FourthLabel.Size = new Size(200, 17);
				FourthLabel.Location = new Point(xDiff - 5, yDiff);
				FourthLabel.Font = new Font(FontFamily.GenericSansSerif, 9);
				Controls.Add(FourthLabel);
				yDiff += 20;
				foreach (string spell in cClass.spellcasting.SeventhLevelSpells)
				{
					Spell s = Spell.fromYAML(spell);
					if (spellcasting.prepMethod == Spellcasting.SpellPrepMethod.KnowSomePrepNone)
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
						if (cClass.spellcasting.PreparedSpells.Contains(spell) || cClass.spellcasting.AlwaysPrepared.Contains(spell))
						{
							prepared.Checked = true;
							numPrepared++;
						}
						else if (cClass.spellcasting.AlwaysPrepared.Contains(spell))
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

			if (cClass.spellcasting.EighthLevelSpells.Count > 0)
			{
				Label FourthLabel = new Label();
				FourthLabel.Text = "8th Level Spells";
				FourthLabel.Size = new Size(200, 17);
				FourthLabel.Location = new Point(xDiff - 5, yDiff);
				FourthLabel.Font = new Font(FontFamily.GenericSansSerif, 9);
				Controls.Add(FourthLabel);
				yDiff += 20;
				foreach (string spell in cClass.spellcasting.EighthLevelSpells)
				{
					Spell s = Spell.fromYAML(spell);
					if (spellcasting.prepMethod == Spellcasting.SpellPrepMethod.KnowSomePrepNone)
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
						if (cClass.spellcasting.PreparedSpells.Contains(spell) || cClass.spellcasting.AlwaysPrepared.Contains(spell))
						{
							prepared.Checked = true;
							numPrepared++;
						}
						else if (cClass.spellcasting.AlwaysPrepared.Contains(spell))
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

			if (cClass.spellcasting.NinthLevelSpells.Count > 0)
			{
				Label FourthLabel = new Label();
				FourthLabel.Text = "9th Level Spells";
				FourthLabel.Size = new Size(200, 17);
				FourthLabel.Location = new Point(xDiff - 5, yDiff);
				FourthLabel.Font = new Font(FontFamily.GenericSansSerif, 9);
				Controls.Add(FourthLabel);
				yDiff += 20;
				foreach (string spell in cClass.spellcasting.NinthLevelSpells)
				{
					Spell s = Spell.fromYAML(spell);
					if (spellcasting.prepMethod == Spellcasting.SpellPrepMethod.KnowSomePrepNone)
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
						if (cClass.spellcasting.PreparedSpells.Contains(spell) || cClass.spellcasting.AlwaysPrepared.Contains(spell))
						{
							prepared.Checked = true;
							numPrepared++;
						}
						else if (cClass.spellcasting.AlwaysPrepared.Contains(spell))
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

			if (numPrepared >= maxNumPrepared && (spellcasting.prepMethod == Spellcasting.SpellPrepMethod.KnowSomePrepSome || spellcasting.prepMethod == Spellcasting.SpellPrepMethod.KnowAllPrepSome))
			{
				foreach (CheckBox c in preparedBoxes)
				{
					if (!c.Checked)
						c.Enabled = false;
					atMax = true;
				}
			}


			NameLabel.Text = player.name;
			Text = $"{player.name}'s Spellcasting Sheet - {cClass.classname}";
			SpellcastingAbilityLabel.Text = $"{spellcasting.spellcastingAbilityModifier.name} ({spellcasting.spellcastingAbilityModifier})";
			SpellcastingAbilityLabel.Location = new Point(59 - (SpellcastingAbilityLabel.Width / 2), 29); 

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
				spellcasting.spellSlots[level]--;
			else
				spellcasting.spellSlots[level]++;
			return;
		}
		private void ChangeSecondSlot(object sender, EventArgs e)
		{
			int level = 1;
			CheckBox thisthing = sender as CheckBox;
			if (!thisthing.Checked)
				spellcasting.spellSlots[level]--;
			else
				spellcasting.spellSlots[level]++;
			return;
		}
		private void ChangeThirdSlot(object sender, EventArgs e)
		{
			int level = 2;
			CheckBox thisthing = sender as CheckBox;
			if (!thisthing.Checked)
				spellcasting.spellSlots[level]--;
			else
				spellcasting.spellSlots[level]++;
			return;
		}
		private void ChangeFourthSlot(object sender, EventArgs e)
		{
			int level = 3;
			CheckBox thisthing = sender as CheckBox;
			if (!thisthing.Checked)
				spellcasting.spellSlots[level]--;
			else
				spellcasting.spellSlots[level]++;
			return;
		}
		private void ChangeFifthSlot(object sender, EventArgs e)
		{
			int level = 4;
			CheckBox thisthing = sender as CheckBox;
			if (!thisthing.Checked)
				spellcasting.spellSlots[level]--;
			else
				spellcasting.spellSlots[level]++;
			return;
		}
		private void ChangeSixthSlot(object sender, EventArgs e)
		{
			int level = 5;
			CheckBox thisthing = sender as CheckBox;
			if (!thisthing.Checked)
				spellcasting.spellSlots[level]--;
			else
				spellcasting.spellSlots[level]++;
			return;
		}
		private void ChangeSeventhSlot(object sender, EventArgs e)
		{
			int level = 6;
			CheckBox thisthing = sender as CheckBox;
			if (!thisthing.Checked)
				spellcasting.spellSlots[level]--;
			else
				spellcasting.spellSlots[level]++;
			return;
		}
		private void ChangeEighthSlot(object sender, EventArgs e)
		{
			int level = 7;
			CheckBox thisthing = sender as CheckBox;
			if (!thisthing.Checked)
				spellcasting.spellSlots[level]--;
			else
				spellcasting.spellSlots[level]++;
			return;
		}
		private void ChangeNinthSlot(object sender, EventArgs e)
		{
			int level = 8;
			CheckBox thisthing = sender as CheckBox;
			if (!thisthing.Checked)
				spellcasting.spellSlots[level]--;
			else
				spellcasting.spellSlots[level]++;
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
				spellcasting.PreparedSpells.Add(SpellName);
				numPrepared++;
				if(numPrepared >= (maxNumPrepared + spellcasting.AlwaysPrepared.Count)&& (spellcasting.prepMethod == Spellcasting.SpellPrepMethod.KnowSomePrepSome || spellcasting.prepMethod == Spellcasting.SpellPrepMethod.KnowAllPrepSome))
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
				cClass.spellcasting.PreparedSpells.Remove(SpellName);
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
				if (SpellName.Contains("[R]"))
					SpellName = SpellName.Substring(0, SpellName.Length - 4);

				theSpell = Spell.fromYAML(SpellName);
				Form from = new Form();
				from.MinimumSize = new Size(200, 0);
				from.AutoSize = true;
				//from.Icon = new Icon(@"C:\Users\Hayden\Downloads\881288450786082876.ico");
				from.Location = new Point(400, 50);
				Label SpellArgs = new Label();
				SpellArgs.Text = Controller.SPage_GetSpellDisplay(theSpell, cClass);
				SpellArgs.Location = new Point(6, 12);
                SpellArgs.MaximumSize = new Size(175, 0);
                SpellArgs.AutoSize = true;
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
            var serializer = new YamlDotNet.Serialization.SerializerBuilder().Build();
            var yaml = serializer.Serialize(cClass);
            File.WriteAllText($@"./Data/Characters/{player.name}/{player.name}{cClass.classname}.yaml", yaml);/*
            string[] classInfo = File.ReadAllLines($@".\Data\Characters\{player.name}\{player.name}{cClass.classname}.yaml");
			(classInfo[0], classInfo[1]) = Controller.SPage_SaveSpellSlots(player, cClass);
			File.WriteAllLines($@".\Data\Characters\{player.name}\{player.name}{cClass.classname}.yaml", classInfo);*/
		}
	}
}
