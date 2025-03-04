﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace _5eCharDisplay
{
	[SupportedOSPlatform("windows")]
	public class Controller
	{
		static internal int CPage_GetArmorClass(Character player)
		{
			Armor chestArmor = null;
			Armor shield = null;
			foreach (Armor a in player.wornArmor)
			{
				if (a.aType != Armor.ArmorType.Shield && chestArmor == null)
				{
					chestArmor = a;
				}
				else if (shield == null)
				{
					shield = a;
				}
			}
			int AC = 10;
			if (chestArmor != null)
			{
				int dexMax = chestArmor.DexMax;
				if (dexMax == -1)
				{
					dexMax = 10;
				}
				AC = int.Parse(chestArmor.AC);
				if (chestArmor.aType == Armor.ArmorType.Medium && player.hasFeat("Medium Armor Master"))
				{
					if (chestArmor.DexMax == 2)
						chestArmor.DexMax = 3;
				}
				if (chestArmor.aType != Armor.ArmorType.Heavy)
					AC += Math.Min(player.dexterity.getMod(), dexMax);
			}
			else
				AC += player.dexterity.getMod();
			if (shield != null)
			{
				AC += int.Parse(shield.AC.Substring(1));
			}
			return AC;
		}
		static internal string CPage_GetStealthLabel(Character player)
		{
			Armor chestArmor = null;

			foreach (Armor a in player.wornArmor)
			{
				if (a.aType != Armor.ArmorType.Shield && chestArmor == null)
				{
					chestArmor = a;
					break;
				}
			}
			if (chestArmor != null && chestArmor.stealthDis)
				return "Stealth [D]";
			else
				return "Stealth";
		}
		static internal (string, int, string) CPage_GetWeaponButtonText(Weapon w, Character player)
		{
			StringBuilder dRoll = new StringBuilder();
			string wName;
			int statBonus = 0;
			int aBonus = 0;
			if (w.MagicBonus > 0)
				wName = $"{w.Name} | +{w.MagicBonus} {w.BaseType}";
			else
				wName = $"{w.Name} | {w.BaseType}";

			if(w.stat != 0)
			{
				switch (w.stat)
				{
					case Character.Stat.Dexterity:
						statBonus += player.dexterity.getMod();
						break;
					case Character.Stat.Constitution:
						statBonus += player.constitution.getMod();
						break;
					case Character.Stat.Intelligence:
						statBonus += player.intelligence.getMod();
						break;
					case Character.Stat.Wisdom:
						statBonus += player.wisdom.getMod();
						break;
					case Character.Stat.Charisma:
						statBonus += player.charisma.getMod();
						break;
					default:
						break;
				}
			}
			else if (w.Properties.Contains("Versatile"))
				statBonus += Math.Max(player.strength.getMod(), player.dexterity.getMod());
			else
				statBonus += player.strength.getMod();

			if (player.weaponProfs.Contains(w.BaseType.ToString()))
				aBonus += player.proficiency;
			else if (player.weaponProfs.Contains("Simple Weapons") && w.PType == Weapon.ProficiencyType.Simple)
				aBonus += player.proficiency;
			else if (player.weaponProfs.Contains("Martial Weapons") && w.PType == Weapon.ProficiencyType.Martial)
				aBonus += player.proficiency;
			aBonus += w.MagicBonus + statBonus;

			for (int i = 0; i < w.DamageDie.Count; i++)
			{
				if (i > 0) dRoll.Append(" + ");
				dRoll.Append($"{w.DamageDie[i]}");
				int damageMod = 0;
				if (i == 0)
					damageMod += w.MagicBonus + statBonus;

				if (damageMod > 0)
					dRoll.Append($" + {damageMod}");
				else if (damageMod < 0)
					dRoll.Append($" - {Math.Abs(damageMod)}");
				dRoll.Append($" {w.DamageType[i]}");
			}

			return (wName, aBonus, dRoll.ToString());
		}
		static internal string CPage_GetProfList(Character player)
		{
			StringBuilder s = new StringBuilder();
			for (int i = 0; i < player.languages.Count() - 1; i++)
			{
				s.Append($"{player.languages[i]}, ");
			}
			if (player.languages.Count() != 0)
				s.Append($"{player.languages[player.languages.Count() - 1]}\n\n");

			for (int i = 0; i < player.armorProfs.Count() - 1; i++)
			{
				s.Append($"{player.armorProfs[i]}, ");
			}
			if (player.armorProfs.Count() != 0)
				s.Append($"{player.armorProfs[player.armorProfs.Count() - 1]}\n\n");

			for (int i = 0; i < player.weaponProfs.Count() - 1; i++)
			{
				s.Append($"{player.weaponProfs[i]}, ");
			}

			if (player.weaponProfs.Count() != 0)
				s.Append($"{player.weaponProfs[player.weaponProfs.Count() - 1]}\n\n");

			for (int i = 0; i < player.toolProf.Count() - 1; i++)
			{
				s.Append(player.toolProf[i]);
				if (player.expertise.Contains(player.toolProf.ElementAt(i)))
					s.Append(" [E]");
				s.Append(", ");
			}
			if (player.toolProf.Count() != 0)
			{
				s.Append(player.toolProf.ElementAt(player.toolProf.Count() - 1));
				if (player.expertise.Contains(player.toolProf.ElementAt(player.toolProf.Count() - 1)))
					s.Append("[E]");
			}
			s.Append("\n\n");
			return s.ToString();
		}
		static internal (bool, string) CPage_GetSkillProfs(Character player, string prof, Statistic stat)
		{
			bool check = false;
			string text;
			if (player.expertise.Contains(prof) || player.skillProf.Contains(prof))
			{
				check = true;
			}

			int modifier = player.GetSkillModifier(prof, stat);
			if (modifier > 0)
				text = $"+{modifier}";
			else text = $"{modifier}";
			return (check, text);
		}
		static internal string CPage_GetPassives(Character player, string prof, Statistic stat)
		{
            int modifier = player.GetSkillModifier(prof, stat);
			/*
            if ((prof == "Investigation" || prof == "Perception") && player.hasFeat("Observant"))
				modifier += 5;
			*/

			return $"{modifier + 10}";
		}
		static internal string CPage_DamageRoll(string input, bool critical)
		{
			Regex pattern = new Regex(@"((\d+)d(\d+))( \+ \d+)? (\w+)");
			/*
			 * Match 0: 1d6 + 3 Piercing
			 * Group 1: 1d6
			 * Group 2: 1
			 * Group 3: 6
			 * Group 4:  + 3
			 * Group 5: Piercing
			 */
			StringBuilder sb = new StringBuilder();
			MatchCollection matches = pattern.Matches(input);
			for(int i = 0; i < matches.Count; i++)
			{
				if(i > 0)
				{
					sb.AppendLine();
				}
				Die d = new Die(int.Parse(matches[i].Groups[2].Value), int.Parse(matches[i].Groups[3].Value));
				int total = d.roll();
				if (matches[i].Groups[4].Success)
					total += int.Parse(matches[i].Groups[4].Value.Substring(matches[i].Groups[4].Value.Length - 1));
				if (critical)
					total += d.roll();
				sb.Append($"{total} {matches[i].Groups[5].Value}");
			}
			return sb.ToString();
		}
		static internal string CPage_GetWeaponDisplay(Weapon w)
		{
			StringBuilder sb = new();
			if (w.MagicBonus > 0)
				sb.AppendLine($"{w.Name} | +{w.MagicBonus} {w.BaseType}\n");
			else
				sb.AppendLine($"{w.Name} | {w.BaseType}\n");
			sb.Append("Damage: ");
			for (int i = 0; i < w.DamageDie.Count; i++)
			{
				if (i > 0) sb.Append(" + ");
				sb.Append(w.DamageDie[i].ToString());
				if (w.MagicBonus > 0 && i == 0)
					sb.Append($" + {w.MagicBonus}");
				sb.Append($" {w.DamageType[i]}");
			}
			sb.AppendLine("\n");

			sb.Append("Properties: ");
			for (int i = 0; i < w.Properties.Count; i++)
			{
				if (i > 0)
					sb.Append(", ");
				sb.Append(w.Properties[i]);
			}
			sb.AppendLine("\n");

			sb.Append("Effects: \n");
			foreach (var p in w.Effects)
			{
				sb.AppendLine($"   - {p}");
				sb.AppendLine();
			}

			return sb.ToString();
		}
		static internal string SPage_GetSpellDisplay(Spell s, charClass cClass)
		{
			StringBuilder sb = new();

            sb.Append($"{s.name}:\n\n");
            if (s.level == 0)
                sb.Append($"{s.school.name} Cantrip\n\n");
            else
                sb.Append($"Level {s.level} {s.school.name}\n\n");
            sb.Append($"Casting Time: {s.casting_time}\n\n");
            sb.Append($"Range: {s.range}\n\n");
			sb.Append($"Components: ");
            for (int i = 0; i < s.components.Count; i++)
            {
                if (i > 0) sb.Append(", ");
                sb.Append(s.components[i]);
            }
            if (s.material != null)
            {
                sb.Append($" ({s.material})");
            }
            sb.Append($"\n\n");
            if (s.concentration)
                sb.Append($"Duration: {s.duration} [C]\n\n\n\n");
            else
                sb.Append($"Duration: {s.duration}\n\n\n\n");
            if (cClass.classname == charClass.ClassName.Warlock)
			{
				sb.Append($"{s.getDescription(cClass)}");
			}
			else
				sb.Append($"{s.getDescription()}");


			return sb.ToString();
		}
		static internal List<EventHandler> getLongRest(Character player)
		{
			List<EventHandler> list = new();
			foreach(charClass c in  player.myClasses)
				list.Add(c.longRest);
			return list;
		}
		static internal List<EventHandler> getShortRest(Character player)
		{
			List<EventHandler> list = new();
			foreach (charClass c in player.myClasses)
				list.Add(c.shortRest);
			return list;
        }
    }
}
