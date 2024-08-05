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
			prepMethod = SpellPrepMethod.KnowSomePrepSome;
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
			returned.SpellcastingAbilityModifier = SpellMod.INT;
			returned.Spellcasting = true;
			returned.SpellPrepLevel = lvl;
			returned.level = lvl;
			returned.proficiency = prof;
			returned.HDrem = returned.level;
			returned.skillProfs.Add(returned.Skill1);
			returned.skillProfs.Add(returned.Skill2);


			switch (returned.level)
			{
				case 1:
					returned.spellSlotsMax = new int[] { 2, 0, 0, 0, 0, 0, 0, 0, 0 };
					break;
				case 2:
					returned.spellSlotsMax = new int[] { 3, 0, 0, 0, 0, 0, 0, 0, 0 };
					break;
				case 3:
					returned.spellSlotsMax = new int[] { 4, 2, 0, 0, 0, 0, 0, 0, 0 };
					break;
				case 4:
					returned.spellSlotsMax = new int[] { 4, 3, 0, 0, 0, 0, 0, 0, 0 };
					break;
				case 5:
					returned.spellSlotsMax = new int[] { 4, 3, 2, 0, 0, 0, 0, 0, 0 };
					break;
				case 6:
					returned.spellSlotsMax = new int[] { 4, 3, 3, 0, 0, 0, 0, 0, 0 };
					break;
				case 7:
					returned.spellSlotsMax = new int[] { 4, 3, 2, 1, 0, 0, 0, 0, 0 };
					break;
				case 8:
					returned.spellSlotsMax = new int[] { 4, 3, 3, 2, 0, 0, 0, 0, 0 };
					break;
				case 9:
					returned.spellSlotsMax = new int[] { 4, 3, 3, 3, 1, 0, 0, 0, 0 };
					break;
				case 10:
					returned.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 0, 0, 0, 0 };
					break;
				case 11:
					returned.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 1, 0, 0, 0 };
					break;
				case 12:
					returned.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 1, 0, 0, 0 };
					break;
				case 13:
					returned.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 1, 1, 0, 0 };
					break;
				case 14:
					returned.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 1, 1, 0, 0 };
					break;
				case 15:
					returned.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 1, 1, 1, 0 };
					break;
				case 16:
					returned.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 1, 1, 1, 0 };
					break;
				case 17:
					returned.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 1, 1, 1, 1 };
					break;
				case 18:
					returned.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 1, 1, 1, 1 };
					break;
				case 19:
					returned.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 2, 1, 1, 1 };
					break;
				case 20:
					returned.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 2, 2, 1, 1 };
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
		public override string[] getClassDetails(string name)
		{
			string[] classInfo = File.ReadAllLines($@".\Data\Characters\{name}\{name}Wizard.yaml");

			// Store spells
			string spells = "spellSlots: [";
			for (int i = 0; i < 8; i++)
				spells += $"{spellSlots[i]}, ";
			spells += $"{spellSlots[8]}]";
			classInfo[0] = spells;
			classInfo[20] = $"BloodChannelerUsed: {BloodChannelerUsed}";

			return classInfo;
		}
		internal override void shortRest(object sender, EventArgs e)
		{
			// Get classInfo
			string[] classInfo = File.ReadAllLines($@".\Data\Characters\{charname}\{charname}Wizard.yaml");

			// Reset Arcane Recovery
			classInfo[17] = "ArcaneRecovery: false";
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
			File.WriteAllLines($@".\Data\Characters\{charname}\{charname}Wizard.yaml", classInfo);
		}
		internal override void longRest(object sender, EventArgs e)
		{
			// Refresh Hit Dice
			HDrem += (int)Math.Floor(level / 2.0);
			if (ArcaneTradition == "Hematurgy") HDrem += proficiency;
			if (HDrem > level) HDrem = level;

			// Get classInfo
			string[] classInfo = File.ReadAllLines($@".\Data\Characters\{charname}\{charname}Wizard.yaml");

			// Reset Spell Slots
			classInfo[0] = "spellSlots: [0, 0, 0, 0, 0, 0, 0, 0, 0]";
			foreach (var box in spellSlotBoxes)
			{
				box.Checked = false;
			}
			for (int i = 0; i < 9; i++)
			{
				spellSlots[i] = 0;
			}

			// Reset Arcane Recovery
			classInfo[17] = "ArcaneRecovery: false";

			foreach (var g in resetOnSR)
			{
				g.Checked = false;
			}
			foreach (var g in resetOnLR)
			{
				g.Checked = false;
			}

			// Write classInfo
			File.WriteAllLines($@".\Data\Characters\{charname}\{charname}Wizard.yaml", classInfo);
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
			switch (ArcaneTradition)
			{
				case "School of Abjuration":
					label.Text += $"  - Abjuration Savant\n   - The gold and time you must spend to copy an abjuration spell into your spellbook is halved.\n\n";
					label.Text += $"  - Arcane Ward\n   - When you cast an abjuration spell of 1st level or higher, you can simultaneously use a strand of the spell’s magic to create a magical ward on yourself that lasts until you finish a long rest. The ward has {2*level + abilityModifiers[3]} maximum hit points. Whenever you take damage, the ward takes the damage instead. If this damage reduces the ward to 0 hit points, you take any remaining damage.\n   - While the ward has 0 hit points, it can’t absorb damage, but its magic remains. Whenever you cast an abjuration spell of 1st level or higher, the ward regains a number of hit points equal to twice the level of the spell.\n   - Once you create the ward, you can’t create it again until you finish a long rest.\n\n";
					if (level >= 6)
					{
						label.Text += $"  - Protected Ward\n   - When a creature that you can see within 30 feet of you takes damage, you can use your reaction to cause your Arcane Ward to absorb that damage. If this damage reduces the ward to 0 hit points, the warded creature takes any remaining damage.\n\n";
					}
					if (level >= 10)
					{
						label.Text += $"  - Improved Abjuration\n   - When you cast an abjuration spell that requires you to make an ability check as a part of casting that spell (as in counterspell and dispel magic), you add your proficiency bonus to that ability check.\n\n";
					}
					if (level >= 14)
					{
						label.Text += $"  - Spell Resistance\n   - You have advantage on saving throws against spells, and resistance to the damage of spells.\n\n";
					}
					low += label.Size.Height + 12;
					break;
				case "Hematurgy":
					label.Text += $"  - Blood Channeler\n   - Once per turn, you can choose to inflict a wound upon yourself to cast a spell without expending a spell slot. To cast a spell this way, you expend a hit die and roll a number of d6 equal to the level of spell slot you're replacing. You lose hit points equal to the total amount rolled. If you are reduced to 0 hit points this way, the spell you cast takes effect before you fall unconcious. You cannot cast a spell at 6th level or higher this way.\nYou can use this feature a number of times equal to your proficiency bonus, and you regain all expended uses of it when you finish a long rest.\n\n";
					label.Text += $"  - Quickened Recovery\n   - Starting at second level, your wounds recover at expedited speed. At the end of a short rest, you regain an expended Hit Die. At the end of a long rest, you regain an additional amount of Hit Dice equal to your proficiency bonus.\n\n";
					if (level >= 6)
					{
						label.Text += $"  - Siphon Blood\n   - Starting at sixth level, your control over your blood and the blood of others increases. You can use an action to create simple items as long as you have requisite blood to manifest them. For example, a small pool of blood might become a dagger, a small length of rope, or a stone.\n\n";
					}
					if (level >= 10)
					{
						label.Text += $"  - Blood Ties\n   - Starting at 10th level, spells you cast are empowered against creatures whose blood you fuse into the magic. Whenever you cast a spell, it has the following effects:\n    - The spell save DC is increased by 4 against creatures whose blood you possess.\n    - You have advantage on the spell attack roll against creatures whose blood you possess.\n    - If the spell restores hit points, it restores the maximum amount.\n\n";
					}
					if (level >= 14)
					{
						label.Text += $"  - Bloody Marionette\n   - Starting at 14th level, your control over blood extends to amounts of blood within people's bodies. As long as you have a creature's blood, you can cast Dominate Monster targeting that creature without expending a spell slot. A creature that succeeds on the saving throw can't be targeted again in this way for 24 hours.\n\n";
					}
					label.AutoSize = true;
					low += label.Bottom - 115;
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
				AlwaysPrepared.Add(s);
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
				foreach (string s in FirstLevelSpells)
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
				foreach (string s in SecondLevelSpells)
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
				if (FirstLevelSpells.Contains(c.Text))
				{
					foreach(var box in spellMasteryBoxes1)
					{
						if (!box.Checked) box.Enabled = false;
					}
				}
				else if (SecondLevelSpells.Contains(c.Text))
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

				if (FirstLevelSpells.Contains(c.Text)) foreach (var box in spellMasteryBoxes1) box.Enabled = true;

				else if (SecondLevelSpells.Contains(c.Text)) foreach (var box in spellMasteryBoxes2) box.Enabled = true;

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
