using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.Versioning;
using System.Numerics;

namespace _5eCharDisplay
{
    [SupportedOSPlatform("windows")]
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
		public ClassName classname;
		public void setCharName (string n)
		{
			charname = n;
		}
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
        public Spellcasting spellcasting { get; set; }

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
        internal virtual List<GroupBox> AddAbilityFromList(string fPath)
		{
            List<GroupBox> boxes = new List<GroupBox>();
            List<Ability> abilities = Ability.ListFromYaml(fPath);
            foreach (var ability in abilities)
            {
                if (ability.levelAt > level)
                    continue;
                GroupBox box = new GroupBox();
                box.MaximumSize = new Size(180, 0);
                box.Text = ability.Name;
                Label label = new Label();
                box.Controls.Add(label);
                label.MaximumSize = new Size(168, 0);
                label.Location = new Point(6, 12);
                label.AutoSize = true;
                label.Text = ability.Description;
                int number = 0;
                /*switch (ability.uses)
                {
                    case Ability.AbilityUses.NoAbility:
                        break;
                    case Ability.AbilityUses.AbilityMod:
                        number = abilityModifiers[(int)ability.numUsesStat];
                        break;
                    case Ability.AbilityUses.Proficiency:
                        number = proficiency;
						break;
					case Ability.AbilityUses.Number1:
						number = 1;
                        break;
                }
                int y = 12;
                for (int i = 0; i < number; i++)
                {
                    CheckBox cBox = new CheckBox();
                    cBox.AutoSize = true;
                    cBox.Location = new Point(y, label.Bottom);
                    box.Controls.Add(cBox);
                    y += cBox.Size.Width + 6;
					switch (ability.refresh)
					{
						case Ability.RefreshOn.NoRefresh:
							break;
						case Ability.RefreshOn.ShortRest:
							resetOnSR.Add(cBox);
							break;
						case Ability.RefreshOn.LongRest:
							resetOnLR.Add(cBox);
							break;
						default:
							break;
					}
                }*/
                box.AutoSize = true;
                label.MouseDown += DisplayOnRightClick;
                boxes.Add(box);
            }
            return boxes;
        }
		internal virtual GroupBox AddAbility(string fPath)
		{
            var abil = Ability.fromYaml(fPath);
            GroupBox box = new GroupBox();
            box.MaximumSize = new Size(180, 0);
            box.Text = abil.Name;
            Label label = new Label();
            box.Controls.Add(label);
            label.MaximumSize = new Size(168, 0);
            label.Location = new Point(6, 12);
            label.AutoSize = true;
            abil.Description = Regex.Replace(abil.Description, @"{(\w*)}", match => GetValue(match.Value));
            label.Text = abil.Description;
            int number = 0;
            /*switch (abil.uses)
            {
                case Ability.AbilityUses.NoAbility:
                    break;
                case Ability.AbilityUses.AbilityMod:
                    number = abilityModifiers[(int)abil.numUsesStat];
                    break;
                case Ability.AbilityUses.Proficiency:
                    number = proficiency;
                    break;
            }
            int y = 12;
            for (int i = 0; i < number; i++)
            {
                CheckBox cBox = new CheckBox();
                cBox.AutoSize = true;
                cBox.Location = new Point(y, label.Bottom - 60);
                box.Controls.Add(cBox);
                y += cBox.Size.Width + 6;
            }*/
            box.AutoSize = true;
            label.MouseDown += DisplayOnRightClick;
            return box;
        }
		internal virtual string GetValue(string variable)
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
        internal virtual void shortRest(object sender, EventArgs e)
		{

        }
        internal virtual void longRest(object sender, EventArgs e)
        {
            HDrem += (int)Math.Floor(level / 2.0);
            if (HDrem > level) HDrem = level;
        }
        public virtual string[] getClassDetails(string name)
		{
			throw new NotImplementedException();
		}
		protected string getClassFile(string charname)
		{
			return $@".\Data\Characters\{charname}\{charname}{classname}.yaml";
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
                from.MaximumSize = new Size(200, 0);
                from.AutoSize = true;
                //from.Icon = new Icon(@"C:\Users\Hayden\Downloads\881288450786082876.ico");
                from.Location = new Point(400, 50);
                Label SpellArgs = new Label();
                SpellArgs.Text = Controller.SPage_GetSpellDisplay(theSpell, this);
                SpellArgs.Location = new Point(6, 12);
                SpellArgs.MaximumSize = new Size(175, 0);
				SpellArgs.AutoSize = true;
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
	}

}
