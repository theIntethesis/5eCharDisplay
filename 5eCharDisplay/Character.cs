﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using _5eCharDisplay.Races;
using _5eCharDisplay.Classes;
using YamlDotNet.Serialization;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace _5eCharDisplay
{
    [SupportedOSPlatform("windows")]
	public class SkillModifierArgs
	{
		public SkillModifierArgs(string text, Statistic s = null, int b = 0)
        {
            skill = text;
            Stat = s;
            bonus = b;
        }
        public string skill { get; }
		public Statistic Stat { get; }
		public int bonus { get; }
		public int ret = 0;
    }
    [SupportedOSPlatform("windows")]
    public class Character
    {
		#region Set / Get Constructors
		public string name { get; set; }
		public List<string> inventory = new List<string>();
		public List<string> skillProf = new List<string>();
		public List<string> expertise  = new List<string>();
		public List<string> toolProf = new List<string>();
		public List<string> armorProfs = new List<string>();
		public List<string> weaponProfs = new List<string>();
		public List<string> languages = new List<string>();
		public string race { get; set; }
		public string subrace { get; set; }
		public List<string> charClass { get; set; }
		public string background { get; set; }
		public string alignment { get; set; }
		public int[] level { get; set; }
		public int hitPoints { get; set; }
		public int baseStr { get; set; }
		public int baseDex { get; set; }
		public int baseCon { get; set; }
		public int baseInt { get; set; }
		public int baseWis { get; set; }
		public int baseCha { get; set; }
		public int maxHitPoints { get; set; }
		public List<string> SpellsKnown { get; set; }
		public int experience { set; get; }
		#endregion Set / Get Constructors

		internal Statistic strength = new Statistic("Strength");
		internal Statistic dexterity = new Statistic("Dexterity");
		internal Statistic constitution = new Statistic("Constitution");
		internal Statistic intelligence = new Statistic("Intelligence");
		internal Statistic wisdom = new Statistic("Wisdom");
		internal Statistic charisma = new Statistic("Charisma");
		public int proficiency;
		public int tempHP = 0;
		public List<charClass> myClasses { get; set; }
		internal charRace myRace;
		internal charBackground myBackground;
		public List<Armor> wornArmor;
		public List<Weapon> equippedWeapons;
		public delegate void SkillModifierHandler(object sender, SkillModifierArgs e);
		public event SkillModifierHandler GetSkillMod;
		internal int GetSkillModifier(string skillName, Statistic stat)
		{
			var e = new SkillModifierArgs(skillName, stat);
            GetSkillMod.Invoke(this, e);
			return e.ret;
		}

		public bool Spellcasting = false;
		public string getRace()
		{
			if (subrace != null)
				return subrace;
			else
				return race;
		}
		public string getClassLevelEXP()
		{
			string retme = "";
			foreach(charClass c in myClasses)
			{
				retme += $"{c.classname} {c.getLevel()}\n";
			}
			retme += $"Exp: {experience}";
			return retme;
		}
		public int getRemHD()
		{
			int returnMe = 0;
			foreach(var c in myClasses)
			{
				returnMe += c.getRemHD();
			}
			return returnMe;
		}
		public enum Stat
        {
			Strength, Dexterity, Constitution, Intelligence, Wisdom, Charisma
        }
		public int affectHitPoints(int hps)
		{
			if(tempHP > 0 && hps < 0)
			{
				if (-hps > tempHP)
				{
					hps += tempHP;
					tempHP = 0;
				}
				else
				{
					tempHP += hps;
					hps = 0;
				}
			}
			hitPoints += hps;
			if (hitPoints > maxHitPoints) hitPoints = maxHitPoints;
			else if (hitPoints < 0) hitPoints = 0;
			return hitPoints;
        }
        public bool hasFeat(string feat)
        {
			if (myClasses.Any(c => c.getFeats().Any(f => f.name == feat)))
				return true;
			return false;
        }
		private static void addClass(string classString) {
            
        }

        public static Character fromYAML(string charName)
		{
			Character returned = null;
			using (FileStream fin = File.OpenRead($@".\Data\Characters\{charName}\{charName}.yaml"))
			{
				TextReader reader = new StreamReader(fin);

				var deserializer = new Deserializer();
				returned = deserializer.Deserialize<Character>(reader);
			}


			foreach (string line in File.ReadAllLines($@".\Data\Characters\{returned.name}\{returned.name}Inventory.txt"))
			{ returned.inventory.Add(line); }

			int[] stats = { returned.strength.getMod(), returned.dexterity.getMod(), returned.constitution.getMod(), returned.intelligence.getMod(), returned.wisdom.getMod(), returned.charisma.getMod() };

			returned.proficiency = (int)Math.Ceiling(returned.level.Sum() / 4.0) + 1;

			if (returned.myClasses == null)
            {
                returned.myClasses = new();
            }
			for (int i = 0; i < returned.charClass.Count; i++)
			{
				if(!returned.myClasses.Any(c => c.classname.ToString() == returned.charClass[i]))
				{
                    switch (returned.charClass[i])
                    {
                        case "Artificer":
                            break;
                        case "Bard":
                            break;
                        case "Barbarian":
                            break;
                        case "Cleric":
                            break;
                        case "Druid":
                            break;
                        case "Fighter":
                            break;
                        case "Magus":
                            returned.myClasses.Add(Magus.fromYAML($@".\Data\Characters\{returned.name}\{returned.name}Magus.yaml", stats, returned.level[i], returned.proficiency));
                            break;
                        case "Monk":
                            break;
                        case "Paladin":
                            returned.myClasses.Add(Paladin.fromYAML($@".\Data\Characters\{returned.name}\{returned.name}Paladin.yaml", stats, returned.level[i], returned.proficiency));
                            break;
                        case "Rogue":
                            returned.myClasses.Add(Rogue.fromYAML($@".\Data\Characters\{returned.name}\{returned.name}Rogue.yaml", stats, returned.level[i], returned.proficiency));
                            break;
                        case "Sorcerer":
                            break;
                        case "Warlock":
                            returned.myClasses.Add(Warlock.fromYAML($@"./Data\Characters\{returned.name}\{returned.name}Warlock.yaml", stats, returned.level[i], returned.proficiency));
                            break;
                        case "Wizard":
                            returned.myClasses.Add(Wizard.fromYAML($@"./Data\Characters\{returned.name}\{returned.name}Wizard.yaml", stats, returned.level[i], returned.proficiency, returned.name));
                            break;
                    }
                }
			}

			foreach(charClass c in returned.myClasses)
			{
				c.setCharName(returned.name);
			}

			switch (returned.race)
			{
				case "Warforged":
					returned.myRace = Warforged.fromYAML($@".\Data\Characters\{returned.name}\{returned.name}Race.yaml");
					break;
				case "Human":
					returned.myRace = new Human();
					break;
				case "Elf":
					returned.myRace = new Elf(returned.subrace);
					break;
				case "Half-Orc":
					returned.myRace = new HalfOrc();
					break;
				case "Dragonborn":
					int tLvl = returned.level.Sum();
					returned.myRace = new Dragonborn(returned.subrace, returned.constitution.getMod(), tLvl, returned.proficiency);
					break;
				case "Gnome":
					returned.myRace = new Gnome(returned.subrace);
					break;
				case "Halfling":
					returned.myRace = new Halfling(returned.subrace);
					break;
				case "Tiefling":
					returned.myRace = new Tiefling();
					break;
				case "Half-Elf":
					returned.myRace = HalfElf.fromYAML($@".\Data\Characters\{returned.name}\{returned.name}Race.yaml");
					break;
			}

			returned.myBackground = charBackground.fromYAML(returned.name, returned.background);

			foreach (charClass c in returned.myClasses)
			{
				if (returned.myRace.Spellcasting)
				{
					returned.Spellcasting = true;
					break;
				}
				if (c.spellcasting != null)
				{
					returned.Spellcasting = true;
					break;
				}
			}

			foreach (charClass c in returned.myClasses)
			{
				foreach (string s in c.getArmorProfs())
				{
					if (!returned.armorProfs.Contains(s) && s != null) returned.armorProfs.Add(s);
				}
				foreach (string s in c.getWeaponProfs())
				{
					if (!returned.weaponProfs.Contains(s) && s != null) returned.weaponProfs.Add(s);
				}
				foreach (string s in c.getLanguages())
				{
					if (!returned.languages.Contains(s) && s != null) returned.languages.Add(s);
				}
				foreach (string s in c.getSkillProfs())
				{
					if (!returned.skillProf.Contains(s) && s != null) returned.skillProf.Add(s);
				}
				foreach (string s in c.getExpertise())
				{
					if (!returned.expertise.Contains(s) && s != null) returned.expertise.Add(s);
				}
				foreach (string s in c.getToolProfs())
				{
					if (!returned.toolProf.Contains(s) && s != null) returned.toolProf.Add(s);
				}
			}
			foreach (string s in returned.myRace.getArmorProfs())
			{
				if (!returned.armorProfs.Contains(s) && s != null) returned.armorProfs.Add(s);
			}
			foreach (string s in returned.myRace.getWeaponProfs())
			{
				if (!returned.weaponProfs.Contains(s) && s != null) returned.weaponProfs.Add(s);
			}
			foreach (string s in returned.myRace.getLanguages())
			{
				if (!returned.languages.Contains(s) && s != null) returned.languages.Add(s);
			}
			foreach(string s in returned.myBackground.getLanguages())
			{
				if (!returned.languages.Contains(s) && s != null) returned.languages.Add(s);
			}

			foreach (string s in returned.myClasses[0].getSaves().Except(returned.skillProf))
			{
				returned.skillProf.Add(s);
			}

			foreach(string s in returned.myRace.getSkillProfs())
			{
				if (!returned.skillProf.Contains(s) && s != null) returned.skillProf.Add(s);
			}
			foreach (string s in returned.myBackground.getSkillProfs())
			{
				if (!returned.skillProf.Contains(s) && s != null) returned.skillProf.Add(s);
			}

			foreach (string s in returned.myRace.getToolProfs())
			{
				if (!returned.toolProf.Contains(s) && s != null) returned.toolProf.Add(s);
			}
			foreach (string s in returned.myBackground.getToolProfs())
			{
				if (!returned.toolProf.Contains(s) && s != null) returned.toolProf.Add(s);
			}

			// Clean Weapon Profs
			if(returned.weaponProfs.Contains("Simple Weapons"))
			{
				if (returned.weaponProfs.Contains("Boomerang"))
					returned.weaponProfs.Remove("Boomerang");
				if (returned.weaponProfs.Contains("Club"))
					returned.weaponProfs.Remove("Club");
				if (returned.weaponProfs.Contains("Light Crossbow"))
					returned.weaponProfs.Remove("Light Crossbow");
				if (returned.weaponProfs.Contains("Dagger"))
					returned.weaponProfs.Remove("Dagger");
				if (returned.weaponProfs.Contains("Dart"))
					returned.weaponProfs.Remove("Dart");
				if (returned.weaponProfs.Contains("Greatclub"))
					returned.weaponProfs.Remove("Greatclub");
				if (returned.weaponProfs.Contains("Handaxe"))
					returned.weaponProfs.Remove("Handaxe");
				if (returned.weaponProfs.Contains("Javelin"))
					returned.weaponProfs.Remove("Javelin");
				if (returned.weaponProfs.Contains("Light Hammer"))
					returned.weaponProfs.Remove("Light Hammer");
				if (returned.weaponProfs.Contains("Mace"))
					returned.weaponProfs.Remove("Mace");
				if (returned.weaponProfs.Contains("Quarterstaff"))
					returned.weaponProfs.Remove("Quarterstaff");
				if (returned.weaponProfs.Contains("Shortbow"))
					returned.weaponProfs.Remove("Shortbow");
				if (returned.weaponProfs.Contains("Sickle"))
					returned.weaponProfs.Remove("Sickle");
				if (returned.weaponProfs.Contains("Sling"))
					returned.weaponProfs.Remove("Sling");
				if (returned.weaponProfs.Contains("Spear"))
					returned.weaponProfs.Remove("Spear");
				if (returned.weaponProfs.Contains("Yklwa"))
					returned.weaponProfs.Remove("Yklwa");
			}
			if(returned.weaponProfs.Contains("Martial Weapons"))
			{
				if (returned.weaponProfs.Contains("Battleaxe"))
					returned.weaponProfs.Remove("Battleaxe");
				if (returned.weaponProfs.Contains("Blowgun"))
					returned.weaponProfs.Remove("Blowgun");
				if (returned.weaponProfs.Contains("Hand Crossbow"))
					returned.weaponProfs.Remove("Hand Crossbow");
				if (returned.weaponProfs.Contains("Heavy Crossbow"))
					returned.weaponProfs.Remove("Heavy Crossbow");
				if (returned.weaponProfs.Contains("Double-Bladed Scimitar"))
					returned.weaponProfs.Remove("Double-Bladed Scimitar");
				if (returned.weaponProfs.Contains("Flail"))
					returned.weaponProfs.Remove("Flail");
				if (returned.weaponProfs.Contains("Glaive"))
					returned.weaponProfs.Remove("Glaive");
				if (returned.weaponProfs.Contains("Greataxe"))
					returned.weaponProfs.Remove("Greataxe");
				if (returned.weaponProfs.Contains("Greatsword"))
					returned.weaponProfs.Remove("Greatsword");
				if (returned.weaponProfs.Contains("Halberd"))
					returned.weaponProfs.Remove("Halberd");
				if (returned.weaponProfs.Contains("Hoopak"))
					returned.weaponProfs.Remove("Hoopak");
				if (returned.weaponProfs.Contains("Lance"))
					returned.weaponProfs.Remove("Lance");
				if (returned.weaponProfs.Contains("Longbow"))
					returned.weaponProfs.Remove("Longbow");
				if (returned.weaponProfs.Contains("Longsword"))
					returned.weaponProfs.Remove("Longsword");
				if (returned.weaponProfs.Contains("Maul"))
					returned.weaponProfs.Remove("Maul");
				if (returned.weaponProfs.Contains("Morningstar"))
					returned.weaponProfs.Remove("Morningstar");
				if (returned.weaponProfs.Contains("Net"))
					returned.weaponProfs.Remove("Net");
				if (returned.weaponProfs.Contains("Pike"))
					returned.weaponProfs.Remove("Pike");
				if (returned.weaponProfs.Contains("Rapier"))
					returned.weaponProfs.Remove("Rapier");
				if (returned.weaponProfs.Contains("Scimitar"))
					returned.weaponProfs.Remove("Scimitar");
				if (returned.weaponProfs.Contains("Shortsword"))
					returned.weaponProfs.Remove("Shortsword");
				if (returned.weaponProfs.Contains("Trident"))
					returned.weaponProfs.Remove("Trident");
				if (returned.weaponProfs.Contains("War Pick"))
					returned.weaponProfs.Remove("War Pick");
				if (returned.weaponProfs.Contains("Warhammer"))
					returned.weaponProfs.Remove("Warhammer");
				if (returned.weaponProfs.Contains("Whip"))
					returned.weaponProfs.Remove("Whip");
			}

			int[] statBoosts = { 0, 0, 0, 0, 0, 0 };
			foreach (charClass c in returned.myClasses)
			{
				foreach(Feat f in c.getFeats())
				{
					for(int i = 0; i < 6; i++)
					{
						statBoosts[i] += f.asiboosts[i];
					}
					returned.GetSkillMod += f.SkillModifier;
				}
			}
			returned.strength.setValue(returned.baseStr + returned.myRace.getStrBoost() + statBoosts[0]);
			returned.dexterity.setValue(returned.baseDex + returned.myRace.getDexBoost() + statBoosts[1]);
			returned.constitution.setValue(returned.baseCon + returned.myRace.getConBoost() + statBoosts[2]);
			returned.intelligence.setValue(returned.baseInt + returned.myRace.getIntBoost() + statBoosts[3]);
			returned.wisdom.setValue(returned.baseWis + returned.myRace.getWisBoost() + statBoosts[4]);
			returned.charisma.setValue(returned.baseCha + returned.myRace.getChaBoost() + statBoosts[5]);

			stats[0] = returned.strength.getMod();
			stats[1] = returned.dexterity.getMod();
			stats[2] = returned.constitution.getMod();
			stats[3] = returned.intelligence.getMod();
			stats[4] = returned.wisdom.getMod();
			stats[5] = returned.charisma.getMod();
			
			foreach(var c in returned.myClasses)
			{
				c.updateAbilities(stats);
			}
			int hp = 0;
			int classHPNum = 0;
			foreach(var c in returned.myClasses)
			{
				hp += c.getHitDie().getSides() + returned.constitution.getMod();
				hp += (returned.level[classHPNum++] - 1) * (c.getHitDie().getAverage() + returned.myRace.getHPBoost() + returned.constitution.getMod());
			}
			if (returned.hasFeat("Tough"))
				hp += returned.level.Sum() * 2;
			
			returned.maxHitPoints = hp;

			returned.hitPoints = returned.maxHitPoints;

			returned.proficiency = (int)Math.Ceiling(returned.level.Sum() / 4.0) + 1;
			foreach(var c in returned.myClasses)
			{
				if (c.spellcasting.Cantrips != null)
					c.spellcasting.Cantrips.Sort();
				else
					c.spellcasting.Cantrips = new List<string>();

				if (c.spellcasting.FirstLevelSpells != null)
					c.spellcasting.FirstLevelSpells.Sort();
				else
					c.spellcasting.FirstLevelSpells = new List<string>();

				if (c.spellcasting.SecondLevelSpells != null)
					c.spellcasting.SecondLevelSpells.Sort();
				else
					c.spellcasting.SecondLevelSpells = new List<string>();

				if (c.spellcasting.ThirdLevelSpells != null)
					c.spellcasting.ThirdLevelSpells.Sort();
				else
					c.spellcasting.ThirdLevelSpells = new List<string>();

				if (c.spellcasting.FourthLevelSpells != null)
					c.spellcasting.FourthLevelSpells.Sort();
				else
					c.spellcasting.FourthLevelSpells = new List<string>();

				if (c.spellcasting.FifthLevelSpells != null)
					c.spellcasting.FifthLevelSpells.Sort();
				else
					c.spellcasting.FifthLevelSpells = new List<string>();

				if (c.spellcasting.SixthLevelSpells != null)
					c.spellcasting.SixthLevelSpells.Sort();
				else
					c.spellcasting.SixthLevelSpells = new List<string>();

				if (c.spellcasting.SeventhLevelSpells != null)
					c.spellcasting.SeventhLevelSpells.Sort();
				else
					c.spellcasting.SeventhLevelSpells = new List<string>();

				if (c.spellcasting.EighthLevelSpells != null)
					c.spellcasting.EighthLevelSpells.Sort();
				else
					c.spellcasting.EighthLevelSpells = new List<string>();

				if (c.spellcasting.NinthLevelSpells != null)
					c.spellcasting.NinthLevelSpells.Sort();
				else
					c.spellcasting.NinthLevelSpells = new List<string>();

			}

			returned.wornArmor = Armor.listFromYaml($@"./Data/Characters/{returned.name}/{returned.name}Armor.yaml");
			returned.equippedWeapons = Weapon.listFromYaml($@"./Data/Characters/{returned.name}/{returned.name}Weapons.yaml");

			returned.GetSkillMod += (s, e) =>
			{
				int total = 0;
				var C = s as Character;
				if (C.expertise.Contains(e.skill))
				{
					total += C.proficiency * 2;
				}
				else if(C.skillProf.Contains(e.skill))
				{
					total += C.proficiency;
				}
				total += e.Stat.getMod();
				e.ret += total;
			};

			return returned;
		}

		public Character() {}
		

	}
}