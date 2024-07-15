using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace _5eCharDisplay
{
	internal class charClass
	{
		protected Die hitDie;
		protected List<string> armorProfs;
		protected List<string> weaponProfs;
		protected List<string> toolProfs = new List<string>();
		protected List<string> languages = new List<string>();
		protected List<string> skillProfs = new List<string>();
		protected List<string> expertise = new List<string>();
		protected List<Feat> featList = new List<Feat>();
		protected List<CheckBox> resetOnSR = new List<CheckBox>();
		protected List<CheckBox> resetOnLR = new List<CheckBox>();
		public List<CheckBox> spellSlotBoxes = new List<CheckBox>();
		public List<CheckBox> warlockSlotBoxes = new List<CheckBox>();
		protected string[] SavingProfs;
		protected int[] abilityModifiers;
		protected int level;
		protected int HDrem;
		protected int proficiency;
		protected string charname;
		public enum SpellPrepMethod
		{
			KnowAllPrepSome,
			KnowSomePrepSome,
			KnowSomePrepNone
		}
		public enum ClassName
		{
			Artificer,
			Barbarian,
			Bard,
			Cleric,
			Druid,
			Fighter,
			Magus,
			Monk,
			Paladin,
			Rogue,
			Sorcerer,
			Warlock,
			Wizard

		}
		public SpellPrepMethod prepMethod;
		public ClassName name;
		public List<string> Cantrips { set; get; }
		public List<string> AlwaysPrepared = new List<string>();
		public List<string> PreparedSpells { set; get; }
		public List<string> FirstLevelSpells { set; get; }
		public List<string> SecondLevelSpells { set; get; }
		public List<string> ThirdLevelSpells { set; get; }
		public List<string> FourthLevelSpells { set; get; }
		public List<string> FifthLevelSpells { set; get; }
		public List<string> SixthLevelSpells { set; get; }
		public List<string> SeventhLevelSpells { set; get; }
		public List<string> EighthLevelSpells { set; get; }
		public List<string> NinthLevelSpells { set; get; }
		public List<string> FeatNames { set; get; }
		public List<string> ASIChoices { set; get; }

		public bool Spellcasting = false;
		public enum SpellMod
		{
			STR,
			DEX,
			CON, 
			INT, 
			WIS, 
			CHA
		};
		public SpellMod SpellcastingAbilityModifier;
		public int SpellPrepLevel;
		
		// spellSlots in .yaml file represents how many spell slots have been expended.
		public int[] spellSlots { get; set; }
		public int[] spellSlotsMax { protected set; get; }
		public void updateAbilities(int[] modifiers)
		{
			abilityModifiers = modifiers;
		}

		public virtual List<string> getInfo()
		{
			return null;
		}
		public virtual List<GroupBox> getInfoBoxes()
		{
			return null;
		}

		public List<string> getArmorProfs() { return armorProfs; }
		public List<string> getWeaponProfs() { return weaponProfs; }
		public List<string> getSkillProfs() { return skillProfs; }
		public List<string> getToolProfs() { return toolProfs; }
		public List<string> getLanguages() { return languages; }
		public List<string> getExpertise() { return expertise; }
		public List<Feat> getFeats() { return featList; }
		public string[] getSaves() { return SavingProfs; }
		public Die getHitDie() { return hitDie; }
		public int getRemHD() { return HDrem; }
		public int affectHD(int mod) { HDrem += mod; return HDrem; }
		public int getLevel() { return level; }
		public int getAbilityModifiers(int i) { return abilityModifiers[i]; }
		public virtual void longRest(string name)
		{
			HDrem += (int)Math.Floor(level/2.0);
			if(HDrem > level) HDrem = level;
		}
		public virtual void shortRest(string name, int classnum)
		{
			
		}
		public virtual string[] getClassDetails(string name, string className = "")
		{
			throw new NotImplementedException();
		}
		protected string getClassFile(string charname)
		{
			return $@".\Data\Characters\{charname}\{charname}{name}.yaml";
		}

		protected GroupBox AddClassDelimiter(string cName)
		{
			GroupBox box = new GroupBox();
			Label label = new Label();
			label.Text = $"========= {cName} =========";
			label.MaximumSize = new Size(168, int.MaxValue);
			label.AutoSize = true;
			box.Controls.Add(label);
			label.Location = new Point(6, 12);
			box.Size = new Size(180, label.Size.Height + 18);
			return box;
		}
		public virtual GroupBox ASIBox(Feat feature)
		{
			GroupBox box = new GroupBox();
			box.Text = feature.name;
			Label label = new Label();
			label.Text = feature.description;
			
			if(feature.name == "Ability Score Increase")
			{
				label.Text += "\n";
				if (feature.asiboosts[0] > 0)
				{
					label.Text += $" - Strength; + {feature.asiboosts[0]}\n";
				}
				if (feature.asiboosts[1] > 0)
				{
					label.Text += $" - Dexterity; + {feature.asiboosts[1]}\n";
				}
				if (feature.asiboosts[2] > 0)
				{
					label.Text += $" - Constitution; + {feature.asiboosts[2]}\n";
				}
				if (feature.asiboosts[3] > 0)
				{
					label.Text += $" - Intelligence; + {feature.asiboosts[3]}\n";
				}
				if (feature.asiboosts[4] > 0)
				{
					label.Text += $" - Wisdom; + {feature.asiboosts[4]}\n";
				}
				if (feature.asiboosts[5] > 0)
				{
					label.Text += $" - Charisma; + {feature.asiboosts[5]}\n";
				}
			}

			label.MaximumSize = new Size(168, int.MaxValue);
			label.AutoSize = true;
			box.Controls.Add(label);
			label.Location = new Point(6, 12);
			box.Size = new Size(180, label.Size.Height + 18);
			label.MouseDown += DisplayOnRightClick;
			return box;
		}

		public charClass() { }

		protected void DisplayOnRightClick(object sender, EventArgs e)
		{
			Label label = sender as Label;
			var box = label.Parent as GroupBox;
			MouseEventArgs mouse = e as MouseEventArgs;
			if (mouse.Button == MouseButtons.Right)
			{
				Form from = new Form();
				//from.Icon = new Icon(@"C:\Users\Hayden\Downloads\881288450786082876.ico");
				from.Location = new Point(400, 50);
				Label label1 = new Label();
				label1.Location = new Point(6, 6);
				label1.AutoSize = true;
				label1.MaximumSize = new Size(500, 575);
				from.Controls.Add(label1);
				label1.Text = label.Text;
				from.Size = new Size(600, label1.Height + 50);
				from.LostFocus += closeOnLostFocus;
				from.Text = box.Text;
				from.Show();
			}
		}
		protected void DisplaySpellOnRightClick(object sender, EventArgs e)
		{
			MouseEventArgs mouse = e as MouseEventArgs;
			if (mouse.Button == MouseButtons.Right)
			{
				CheckBox the = sender as CheckBox;
				Spell theSpell;
				string SpellName;
				if (the == null)
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
				SpellArgs.Text = $"{theSpell.name}:\n\n";
				if (theSpell.level == 0)
					SpellArgs.Text += $"{theSpell.school} Cantrip\n\n";
				else
					SpellArgs.Text += $"Level {theSpell.level} {theSpell.school}\n\n";
				SpellArgs.Text += $"Casting Time: {theSpell.castingTime}\n\nRange: {theSpell.range}\n\nComponents: {theSpell.components}\n\nDuration: {theSpell.duration}\n\n\n\n{theSpell.getDescription()}";
				SpellArgs.Location = new Point(6, 12);
				SpellArgs.Size = new Size(175, 575);
				from.Controls.Add(SpellArgs);
				from.LostFocus += closeOnLostFocus;
				from.Show();
			}
		}

		protected void closeOnLostFocus(object sender, EventArgs e)
		{
			Form form = sender as Form;
			form.Close();
		}
		public bool isClass(Type t)
		{
			if (GetType() == t) return true;
			else return false;
		}
	}

}
