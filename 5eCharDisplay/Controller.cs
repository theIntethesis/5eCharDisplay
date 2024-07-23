using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5eCharDisplay
{
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
			int aBonus = 0;
			if (w.MagicBonus > 0)
				wName = $"{w.Name} | +{w.MagicBonus} {w.BaseType}";
			else
				wName = $"{w.Name} | {w.BaseType}";

			if (w.Properties.Contains("Versatile"))
				aBonus += Math.Max(player.strength.getMod(), player.dexterity.getMod());
			else
				aBonus += player.strength.getMod();
			if (player.weaponProfs.Contains(w.BaseType.ToString()))
				aBonus += player.proficiency;
			else if (player.weaponProfs.Contains("Simple Weapons") && w.PType == Weapon.ProficiencyType.Simple)
				aBonus += player.proficiency;
			else if (player.weaponProfs.Contains("Martial Weapons") && w.PType == Weapon.ProficiencyType.Martial)
				aBonus += player.proficiency;

			for (int i = 0; i < w.DamageDie.Count; i++)
			{
				if (i > 0) dRoll.Append(" + ");
				dRoll.Append($"{w.DamageDie[i]}");
				int damageMod = 0;
				if (i == 0)
				{
					if (w.Properties.Contains("Versatile"))
						damageMod += Math.Max(player.strength.getMod(), player.dexterity.getMod());
					else
						damageMod += player.strength.getMod();
					damageMod += w.MagicBonus;
				}
				if (damageMod > 0)
					dRoll.Append($" + {damageMod}");
				else if (damageMod < 0)
					dRoll.Append($" - {damageMod}");
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

		static internal (bool, string) CPage_GetSkillProfs(Character player, string prof, Character.Stat stat)
        {
			bool check = false;
			int modifier = 0;
			string text;
			if (player.expertise.Contains(prof))
			{
				check = true;
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains(prof))
			{
				check = true;
				modifier += player.proficiency;
			}
			switch (stat)
			{
				case Character.Stat.Strength:
					modifier += player.strength.getMod();
					break;
				case Character.Stat.Dexterity:
					modifier += player.dexterity.getMod();
					break;
				case Character.Stat.Constitution:
					modifier += player.constitution.getMod();
					break;
				case Character.Stat.Intelligence:
					modifier += player.intelligence.getMod();
					break;
				case Character.Stat.Wisdom:
					modifier += player.wisdom.getMod();
					break;
				case Character.Stat.Charisma:
					modifier += player.charisma.getMod();
					break;
				default:
					break;
			}
			if (modifier > 0)
				text = $"+{modifier}";
			else text = $"{modifier}";
			return (check, text);
		}
		static internal string CPage_GetPassives(Character player, string prof, Character.Stat stat)
		{
            int modifier = 0;
            if (player.expertise.Contains(prof))
            {
                modifier += 2 * player.proficiency;
            }
            else if (player.skillProf.Contains(prof))
            {
                modifier += player.proficiency;
            }
            switch (stat)
            {
                case Character.Stat.Strength:
                    modifier += player.strength.getMod();
                    break;
                case Character.Stat.Dexterity:
                    modifier += player.dexterity.getMod();
                    break;
                case Character.Stat.Constitution:
                    modifier += player.constitution.getMod();
                    break;
                case Character.Stat.Intelligence:
                    modifier += player.intelligence.getMod();
                    break;
                case Character.Stat.Wisdom:
                    modifier += player.wisdom.getMod();
                    break;
                case Character.Stat.Charisma:
                    modifier += player.charisma.getMod();
                    break;
                default:
                    break;
            }
			return $"{modifier + 10}";
        }
	}
}
