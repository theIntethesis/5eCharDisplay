using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;

namespace _5eCharDisplay.Classes
{
	[SupportedOSPlatform("windows")]
	internal class Wizard : charClass
	{
		public string Skill1 { set; get; }
		public string Skill2 { set; get; }
		public bool ArcaneRecovery { set; get; }
		public string ArcaneTradition { set; get; }
		public List<string> SpellMastery { set; get; }
		public List<string> SignatureSpells { set; get; }
		public int BloodChannelerUsed { set; get; }

		private List<Label> SpellMasterLabels = new();

		public Wizard()
		{
			hitDie = new Die(6);
			armorProfs = new List<string> {  };
			weaponProfs = new List<string> { "Dagger", "Dart", "Sling", "Quarterstaff", "Light Crossbow" };
			SavingProfs = [ "IntSave", "WisSave" ];
			classname = ClassName.Wizard;
		}
		public static Wizard fromYAML(string fName, int[] modifiers, int lvl, int prof, string cname)
		{
			Wizard returned = null;
			using (FileStream fin = File.OpenRead(fName))
			{
				TextReader reader = new StreamReader(fin);

				var deserializer = new Deserializer();
				returned = deserializer.Deserialize<Wizard>(reader);
			}
			returned.abilityModifiers = modifiers;
			returned.charname = cname;
			returned.level = lvl;
			returned.proficiency = prof;
			returned.HDrem = returned.level;
			returned.skillProfs.Add(returned.Skill1);
			returned.skillProfs.Add(returned.Skill2);
			returned.spellcasting.spellPrepLevel = returned.level;

			switch (returned.level)
			{
				case 1:
					returned.spellcasting.spellSlotsMax = new int[] { 2, 0, 0, 0, 0, 0, 0, 0, 0 };
					break;
				case 2:
					returned.spellcasting.spellSlotsMax = new int[] { 3, 0, 0, 0, 0, 0, 0, 0, 0 };
					break;
				case 3:
					returned.spellcasting.spellSlotsMax = new int[] { 4, 2, 0, 0, 0, 0, 0, 0, 0 };
					break;
				case 4:
					returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 0, 0, 0, 0, 0, 0, 0 };
					break;
				case 5:
					returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 2, 0, 0, 0, 0, 0, 0 };
					break;
				case 6:
					returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 0, 0, 0, 0, 0, 0 };
					break;
				case 7:
					returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 2, 1, 0, 0, 0, 0, 0 };
					break;
				case 8:
					returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 2, 0, 0, 0, 0, 0 };
					break;
				case 9:
					returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 3, 1, 0, 0, 0, 0 };
					break;
				case 10:
					returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 0, 0, 0, 0 };
					break;
				case 11:
					returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 1, 0, 0, 0 };
					break;
				case 12:
					returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 1, 0, 0, 0 };
					break;
				case 13:
					returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 1, 1, 0, 0 };
					break;
				case 14:
					returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 1, 1, 0, 0 };
					break;
				case 15:
					returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 1, 1, 1, 0 };
					break;
				case 16:
					returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 1, 1, 1, 0 };
					break;
				case 17:
					returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 1, 1, 1, 1 };
					break;
				case 18:
					returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 1, 1, 1, 1 };
					break;
				case 19:
					returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 2, 1, 1, 1 };
					break;
				case 20:
					returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 2, 2, 1, 1 };
					break;
			}
			if (returned.FeatNames != null)
			{
				if (returned.FeatNames.Count != 0)
				{
					int i = 0;
					foreach (string s in returned.FeatNames)
					{
						if (s == "Ability Score Increase")
						{
							returned.featList.Add(Feat.FromYAML(s, returned.ASIChoices[i]));
							i++;
						}
						else
							returned.featList.Add(Feat.FromYAML(s));
					}
				}
			}

			returned.getInfoBoxes();

			return returned;
		}
		internal override void shortRest(object sender, EventArgs e)
		{
			// Reset Arcane Recovery
			ArcaneRecovery = false;

			foreach(var g in resetOnSR)
			{
				g.Checked = false;
			}
			if(ArcaneTradition == "Hematurgy")
			{
				HDrem += 1;
				if (HDrem > level) HDrem = level;
			}

            // Write classInfo
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(this);
            File.WriteAllText($@"./Data/Characters/{charname}/{charname}Wizard.yaml", yaml);
        }
		internal override void longRest(object sender, EventArgs e)
		{
			// Refresh Hit Dice
			HDrem += (int)Math.Floor(level / 2.0);
			if (ArcaneTradition == "Hematurgy") HDrem += proficiency;
			if (HDrem > level) HDrem = level;

			// Reset Spell Slots
			foreach (var box in spellSlotBoxes)
			{
				box.Checked = false;
			}
			for (int i = 0; i < 9; i++)
			{
				spellcasting.spellSlots[i] = 0;
			}

			// Reset Arcane Recovery
			ArcaneRecovery = false;

			// Reset CheckBoxes
			foreach (var g in resetOnSR)
			{
				g.Checked = false;
			}
			foreach (var g in resetOnLR)
			{
				g.Checked = false;
			}

			// Write classInfo
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(this);
            File.WriteAllText($@"./Data/Characters/{charname}/{charname}Wizard.yaml", yaml);
        }
		public override List<GroupBox> getInfoBoxes()
		{
			var infoBoxes = new List<GroupBox>();
			if (level >= 1)
			{
				// Spellcasting
				infoBoxes.Add(AddAbility($@".\Data\Classes\Wizard\Spellcasting.yaml"));
				// Arcane Recovery
				if(ArcaneTradition != "Hematurgy")
					//infoBoxes.Add(AddAbility($@".\Data\Classes\Wizard\Arcane Recovery.yaml"));
				infoBoxes.Add(AddArcaneRecovery());
			}
			if (level >= 2)
			{
				// Arcane Tradition (Subclass)
				infoBoxes.Add(AddSubclass());
				//AddAbilityFromList($@".\Data\Classes\Wizard\{ArcaneTradition}.yaml").ForEach(box => infoBoxes.Add(box));
			}
			if (level >= 4)
			{
				infoBoxes.Add(ASIBox(featList[0]));
			}
			if (level >= 6)
			{
				// Subclass Feature
			}
			if (level >= 8)
			{
				infoBoxes.Add(ASIBox(featList[1]));
			}
			if (level >= 10)
			{
				// Subclass Feature
			}
			if (level >= 12)
			{
				infoBoxes.Add(ASIBox(featList[2]));
			}
			if (level >= 14)
			{
				// Subclass Feature
			}
			if (level >= 16)
			{
				infoBoxes.Add(ASIBox(featList[3]));
			}
			if (level >= 18)
			{
				// Spell Mastery
				infoBoxes.Add(AddAbility($@".\Data\Classes\Wizard\Spell Mastery.yaml"));
			}
			if (level >= 19)
			{
				infoBoxes.Add(ASIBox(featList[4]));
			}
			if (level >= 20)
			{
				// Signature Spells
				infoBoxes.Add(AddAbility($@".\Data\Classes\Wizard\Signature Spells.yaml"));
			}
			return infoBoxes;
		}
		private GroupBox AddArcaneRecovery()
		{
			GroupBox box = new GroupBox();
			box.Text = "Arcane Recovery";
			Label label = new Label();
			label.Text = $"You have learned to regain some of your magical energy by studying your spellbook. Once per day when you finish a short rest, you can choose expended spell slots to recover. The spell slots can have a combined level that is equal to or less than {Math.Ceiling(level / 2.0)}, and none of the slots can be 6th level or higher.\n\n";
			label.MaximumSize = new Size(168, int.MaxValue);
			label.AutoSize = true;
			box.Controls.Add(label);
			label.Location = new Point(6, 12);

			CheckBox ArcaneRecoveryBox = new CheckBox();
			ArcaneRecoveryBox.Checked = ArcaneRecovery;
			ArcaneRecoveryBox.AutoSize = true;
			ArcaneRecoveryBox.Text = "Arcane Recovery / LR";
			resetOnLR.Add(ArcaneRecoveryBox);
			box.Controls.Add(ArcaneRecoveryBox);

			ArcaneRecoveryBox.Location = new Point(6, label.Bottom - 115);
			ArcaneRecoveryBox.CheckedChanged += ToggleArcaneRecovery;

			box.MaximumSize = new Size(180, int.MaxValue);
			box.AutoSize = true;
			label.MouseDown += DisplayOnRightClick;
			return box;
		}
		internal override string GetValue(string variable)
		{
			string retMe = "";
			switch (variable)
			{
				case "{numCantrips}":
					retMe = "3";
					if (level >= 4)
						retMe = "4";
					if (level >= 10)
						retMe = "5";
					break;
				case "{numSpells}":
					retMe = $"{4 + 2 * level}";
					break;
				case "{numPreparedSpells}":
					retMe = $"{abilityModifiers[3] + level}";
					break;
				case "{RecoverySlots}":
					retMe = $"{Math.Ceiling(level / 2.0)}";
						break;
				case "{ArcaneWardHP}":
					retMe = $"{2 * level + abilityModifiers[3]}";
                    break;
				default:
					retMe = variable;
					break;
			}
			return retMe;
		}
		private GroupBox AddSubclass()
		{
			GroupBox box = new GroupBox();
			box.MaximumSize = new Size(180, int.MaxValue);
			box.Text = $"Arcane Tradition";
			Label label = new Label();
			box.Controls.Add(label);
			label.MaximumSize = new Size(168, 0);
			label.Location = new Point(6, 12);
			int low = 0;

            var abilities = Ability.ListFromYaml(@$".\Data\Classes\Wizard\{ArcaneTradition}.yaml", GetValue);

            switch (ArcaneTradition)
			{
				case "School of Abjuration":
                    foreach (Ability a in abilities)
                    {
                        if (a.levelAt <= level)
                        {
                            label.Text += $"   - {a.Name}\n";
                            label.Text += a.Description;
                        }
                    }
                    low += label.Size.Height + 12;
					break;
				case "Hematurgy":
					foreach(Ability a in abilities)
					{
						if(a.levelAt <= level)
						{
							label.Text += $"   - {a.Name}\n";
							label.Text += a.Description;
						}
					}
					label.AutoSize = true;
					low += label.Size.Height - 115;
					Label BloodChannelerLabel = new Label();
					BloodChannelerLabel.AutoSize = true;
					BloodChannelerLabel.Location = new Point(6, low);
					BloodChannelerLabel.Text = $"Blood Channeler";
					box.Controls.Add(BloodChannelerLabel);
					low += BloodChannelerLabel.Size.Height + 6;
					int y = 12;
					for(int i = 0; i < proficiency; i++)
					{
						CheckBox cBox = new CheckBox();
						cBox.AutoSize = true;
						cBox.Location = new Point(y, BloodChannelerLabel.Bottom);
						if (BloodChannelerUsed > i) cBox.Checked = true;
						cBox.CheckedChanged += BloodChannelerCheck;
						resetOnLR.Add(cBox);
						box.Controls.Add(cBox);
						y += cBox.Size.Width + 6;
					}
					low += 32;
					break;
				case "":
					label.Text += $"  - \n   - \n\n";
					if (level >= 6)
					{
						label.Text += $"  - \n   - \n\n";
					}
					if (level >= 10)
					{
						label.Text += $"  - \n   - \n\n";
					}
					if (level >= 14)
					{
						label.Text += $"  - \n   - \n\n";
					}
					break;
				default:
					label.Text += $"No Subclass chosen!";
					break;
			}
			box.AutoSize = true;
			label.MouseDown += DisplayOnRightClick;
			return box;
		}

		private GroupBox AddSpellMastery()
		{
			int low = 12;
			GroupBox box = new GroupBox();
			box.Text = "Spell Mastery";

			Label label = new Label();
			label.Text = $"You have achieved such mastery over certain spells that you can cast them at will. Choose a 1st-level wizard spell and a 2nd-level wizard spell that are in your spellbook. You can cast those spells at their lowest level without expending a spell slot when you have them prepared. If you want to cast either spell at a higher level, you must expend a spell slot as normal.\nBy spending 8 hours in study, you can exchange one or both of the spells you chose for different spells of the same levels.\n\n";
			box.Controls.Add(label);
			label.MaximumSize = new Size(168, int.MaxValue);
			label.AutoSize = true;
			label.Location = new Point(6, low);
			low += label.Size.Height;

			SpellMasterLabels.Clear();

			foreach(string s in SpellMastery)
			{
				Label spell = new Label();
				spell.Text = $"{s}";
				box.Controls.Add(spell);
				spell.MaximumSize = new Size(168, int.MaxValue);
				spell.AutoSize = true;
				spell.Location = new Point(6, low);
				SpellMasterLabels.Add(spell);
				low += spell.Size.Height + 6;
				spell.MouseDown += DisplaySpellOnRightClick;
			}
			box.MaximumSize = new Size(180, int.MaxValue);
			box.AutoSize = true;
			label.MouseDown += ChangeSpellMastery;
			return box;
		}

		private GroupBox AddSignatureSpells()
		{
			int low = 12;
			GroupBox box = new GroupBox();
			box.Text = "Signature Spells";

			Label label = new Label();
			label.Text = $"You gain mastery over two powerful spells and can cast them with little effort. Choose two 3rd-level wizard spells in your spellbook as your signature spells. You always have these spells prepared, they don’t count against the number of spells you have prepared, and you can cast each of them once at 3rd level without expending a spell slot. When you do so, you can’t do so again until you finish a short or long rest.\nIf you want to cast either spell at a higher level, you must expend a spell slot as normal.\n\n";
			box.Controls.Add(label);
			label.MaximumSize = new Size(168, int.MaxValue);
			label.AutoSize = true;
			label.Location = new Point(6, low);
			low += label.Size.Height;

			foreach (string s in SignatureSpells)
			{
				Label spell = new Label();
				spell.Text = $"{s}";
				box.Controls.Add(spell);
				spell.MaximumSize = new Size(168, int.MaxValue);
				spell.AutoSize = true;
				spell.Location = new Point(6, low);
				SpellMasterLabels.Add(spell);
				low += spell.Size.Height + 6;
				spell.MouseDown += DisplaySpellOnRightClick;
				spellcasting.AlwaysPrepared.Add(s);
			}
			label.MaximumSize = new Size(168, int.MaxValue);
			box.MaximumSize = new Size(180, int.MaxValue);
			box.AutoSize = true;
			label.MouseDown += DisplayOnRightClick;
			return box;
		}

		private List<CheckBox> spellMasteryBoxes1 = new List<CheckBox>();
		private List<CheckBox> spellMasteryBoxes2 = new List<CheckBox>();

		private void ChangeSpellMastery(object sender, EventArgs e)
		{
			Label label = sender as Label;
			var box = label.Parent as GroupBox;

			MouseEventArgs mouse = e as MouseEventArgs;
			if (mouse.Button == MouseButtons.Right)
			{
				int size = 0;
				Form from = new Form();
				//from.Icon = new Icon(@"C:\Users\Hayden\Downloads\881288450786082876.ico");
				from.Location = new Point(400, 50);
				Label label1 = new Label();
				label1.Location = new Point(6, 6);
				label1.AutoSize = true;
				label1.MaximumSize = new Size(500, 575);
				from.Controls.Add(label1);
				label1.Text = label.Text;
				size = label1.Height + 50;
				int firstColumn = size;
				int secondColumn = size;
				bool firstOff = false;
				bool secondOff = false;
				foreach (string s in spellcasting.FirstLevelSpells)
				{
					CheckBox c = new CheckBox();
					c.Text = s;
					if (SpellMastery.Contains(s))
					{
						c.Checked = true;
						firstOff = true;
					}
					c.CheckedChanged += ChangeActiveSpellMastery;
					c.MouseDown += DisplaySpellOnRightClick;
					spellMasteryBoxes1.Add(c);
					from.Controls.Add(c);
					c.AutoSize = true;
					c.Location = new Point(6, firstColumn);
					firstColumn += c.Size.Height + 6;
				}
				foreach (string s in spellcasting.SecondLevelSpells)
				{
					CheckBox c = new CheckBox();
					c.Text = s;
					if (SpellMastery.Contains(s))
					{
						c.Checked = true;
						secondOff = true;
					}
					c.CheckedChanged += ChangeActiveSpellMastery;
					c.MouseDown += DisplaySpellOnRightClick;
					spellMasteryBoxes2.Add(c);
					from.Controls.Add(c);
					c.AutoSize = true;
					c.Location = new Point(300, secondColumn);
					secondColumn += c.Size.Height + 6;
				}
				if (firstOff)
				{
					foreach (var spellBox in spellMasteryBoxes1)
					{
						if (!spellBox.Checked) spellBox.Enabled = false;
					}
				}
				if (secondOff)
				{
					foreach (var spellBox in spellMasteryBoxes2)
					{
						if (!spellBox.Checked) spellBox.Enabled = false;
					}
				}

				from.Size = new Size(600, Math.Max(firstColumn, secondColumn) + 18);
				from.AutoSize = true;
				from.LostFocus += closeOnLostFocus;
				from.FormClosing += SaveSpellMastery;
				from.FormClosing += ChangeLabels;
				from.Text = box.Text;
				from.Show();
			}
		}
		private void ChangeLabels(object sender, EventArgs e)
		{
			if (SpellMastery[1] == "") SpellMastery.RemoveAt(1);
			if (SpellMastery[0] == "") SpellMastery.RemoveAt(0);
			if(SpellMasterLabels.Count >= SpellMastery.Count)
			{
				for (int i = 0; i < SpellMasterLabels.Count; i++)
				{
					string s;
					try
					{
						s = SpellMastery[i];
					}
					catch
					{
						s = "";
					}
					SpellMasterLabels[i].Text = s;
				}
			}
			else
			{
				throw new Exception();
			}
		}
		private void ChangeActiveSpellMastery(object sender, EventArgs e)
		{
			CheckBox c = sender as CheckBox;
			if (c.Checked)
			{
				SpellMastery.Add(c.Text);
				if (spellcasting.FirstLevelSpells.Contains(c.Text))
				{
					foreach(var box in spellMasteryBoxes1)
					{
						if (!box.Checked) box.Enabled = false;
					}
				}
				else if (spellcasting.SecondLevelSpells.Contains(c.Text))
				{
					foreach(var box in spellMasteryBoxes2)
					{
						if (!box.Checked) box.Enabled = false;
					}
				}
			}
			else
			{
				SpellMastery.Remove(c.Text);

				if (spellcasting.FirstLevelSpells.Contains(c.Text)) foreach (var box in spellMasteryBoxes1) box.Enabled = true;

				else if (spellcasting.SecondLevelSpells.Contains(c.Text)) foreach (var box in spellMasteryBoxes2) box.Enabled = true;

			}

		}
		private void ToggleArcaneRecovery(object sender, EventArgs e)
		{
			var c = sender as CheckBox;
			ArcaneRecovery = c.Checked;
		}
		private void BloodChannelerCheck(object sender, EventArgs e)
		{
			var c = sender as CheckBox;
			if (c.Checked) BloodChannelerUsed++;
			else BloodChannelerUsed--;
		}
		private void SaveSpellMastery(object sender, CancelEventArgs e)
		{
			string[] classInfo = File.ReadAllLines($@".\Data\Characters\{charname}\{charname}Wizard.yaml");
			StringBuilder spells = new StringBuilder();
			try { spells.Append($"SpellMastery: [\"{SpellMastery[0]}\", \"{SpellMastery[1]}\"]"); }
			catch { spells.Append($"SpellMastery: [\"\", \"\"]"); }
			classInfo[18] = spells.ToString();
			File.WriteAllLines($@".\Data\Characters\{charname}\{charname}Wizard.yaml", classInfo);
			}
	}
}
