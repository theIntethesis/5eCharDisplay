using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5eCharDisplay
{
    internal class Dragonborn : charRace
    {
        public Dragonborn(string subrace, int ConMod, int level, int proficiency)
        {
            speed = 30;
            StrBoost = 2;
            ChaBoost = 1;
            string breathWeapon = "";
            string DamageType = "";
            string DragonType = "";
            int num = 0;
            if (level < 5)
                num = 1;
            else if (level < 11)
                num = 2;
            else if (level < 17)
                num = 3;
            else
                num = 4;
            languages = new List<string> { "Common", "Draconic" };
            switch (subrace)
            {
                case "Black Dragonborn":
                    breathWeapon = $" a 5 by 30 foot line must make a DC {8 + proficiency + ConMod} Dexterity ";
                    DamageType = "acid";
                    DragonType = "Chromatic";
                    break;
                case "Blue Dragonborn":
                    breathWeapon = $" a 5 by 30 foot line must make a DC {8 + proficiency + ConMod} Dexterity ";
                    DamageType = "lightning";
                    DragonType = "Chromatic";
                    break;
                case "Brass Dragonborn":
                    breathWeapon = $" a 5 by 30 foot line must make a DC {8 + proficiency + ConMod} Dexterity ";
                    DamageType = "fire";
                    DragonType = "Metallic";
                    break;
                case "Bronze Dragonborn":
                    breathWeapon = $" a 5 by 30 foot line must make a DC {8 + proficiency + ConMod} Dexterity ";
                    DamageType = "lightning";
                    DragonType = "Metallic";
                    break;
                case "Copper Dragonborn":
                    breathWeapon = $" a 5 by 30 foot line must make a DC {8 + proficiency + ConMod} Dexterity ";
                    DamageType = "acid";
                    DragonType = "Metallic";
                    break;
                case "Gold Dragonborn":
                    breathWeapon = $" a 15 foot cone must make a DC {8 + proficiency + ConMod} Dexterity ";
                    DamageType = "fire";
                    DragonType = "Metallic";
                    break;
                case "Green Dragonborn":
                    breathWeapon = $" a 15 foot cone must make a DC {8 + proficiency + ConMod} Constitution ";
                    DamageType = "poison";
                    DragonType = "Chromatic";
                    break;
                case "Red Dragonborn":
                    breathWeapon = $" a 15 foot cone must make a DC {8 + proficiency + ConMod} Dexterity ";
                    DamageType = "fire";
                    DragonType = "Chromatic";
                    break;
                case "Silver Dragonborn":
                    breathWeapon = $" a 15 foot cone must make a DC {8 + proficiency + ConMod} Constitution ";
                    DamageType = "cold";
                    DragonType = "Metallic";
                    break;
                case "White Dragonborn":
                    breathWeapon = $" a 15 foot cone must make a DC {8 + proficiency + ConMod} Constitution ";
                    DamageType = "cold";
                    DragonType = "Chromatic";
                    break;
                case "Amethyst Dragonborn":
                    breathWeapon = $" a 15 foot cone must make a DC {8 + proficiency + ConMod} Dexterity ";
                    DamageType = "force";
                    DragonType = "Gemspark";
                    break;
                case "Crystal Dragonborn":
                    breathWeapon = $" a 15 foot cone must make a DC {8 + proficiency + ConMod} Dexterity ";
                    DamageType = "radiant";
                    DragonType = "Gemspark";
                    break;
                case "Emerald Dragonborn":
                    breathWeapon = $" a 15 foot cone must make a DC {8 + proficiency + ConMod} Dexterity ";
                    DamageType = "psychic";
                    DragonType = "Gemspark";
                    break;
                case "Sapphire Dragonborn":
                    breathWeapon = $" a 15 foot cone must make a DC {8 + proficiency + ConMod} Dexterity ";
                    DamageType = "thunder";
                    DragonType = "Gemspark";
                    break;
                case "Topaz Dragonborn":
                    breathWeapon = $" a 15 foot cone must make a DC {8 + proficiency + ConMod} Dexterity ";
                    DamageType = "necrotic";
                    DragonType = "Gemspark";
                    break;
            }
            abilities = new List<string> { $"Breath Weapon", $" - When you take the Attack action on your turn, you can replace one of your attacks with an exhalation of magical energy. When you use your breath weapon, each creature in {breathWeapon} saving throw. A creature takes {num}d10 {DamageType} damage on a failed save, and half as much damage on a successful one. You can use your Breath Weapon a number of times equal to your proficiency bonus, and you regain all expended uses when you finish a long rest.\n\n", $"Damage Resistance", $" - You have resistance to {DamageType} damage." };
            switch (DragonType)
            {
                case "Chromatic":
                    if (level >= 5)
                    {
                        abilities.Add("Chromatic Warding");
                        abilities.Add(" - Starting at 5th level, as an action you can channel your draconic energy to protect yourself. For 1 minute, you become immune to the damage type associated with your Chromatic Ancestry. Once you use this trait, you can’t do so again until you finish a long rest.");
                    }
                    break;
                case "Metallic":
                    if (level <= 5)
                    {
                        abilities.Add("Metallic Breath Weapon");
                        abilities.Add($" - At 5th level, you gain a second breath weapon. When you take the Attack action on your turn, you can replace one of your attacks with an exhalation in a 15-foot cone. The save DC for this breath is {8 + ConMod + proficiency}. Whenever you use this trait, choose one:\n  - Enervating Breath. Each creature in the cone must succeed on a Constitution saving throw or become incapacitated until the start of your next turn.\n  - Repulsion Breath. Each creature in the cone must succeed on a Strength saving throw or be pushed 20 feet away from you and be knocked prone.\n   Once you use your Metallic Breath Weapon, you can’t do so again until you finish a long rest.");
                    }
                    break;
                case "Gemspark":
                    abilities.Add("Psionic Mind");
                    abilities.Add(" - You can send telepathic messages to any creature you can see within 30 feet of you. You don’t need to share a language with the creature for it to understand these messages, but it must be able to understand at least one language to comprehend them.");
                    if (level >= 5)
                    {
                        abilities.Add("Gem Flight"); 
                        abilities.Add(" - Starting at 5th level, you can use a bonus action to manifest spectral wings on your body. These wings last for 1 minute. For the duration, you gain a flying speed equal to your walking speed and can hover. Once you use this trait, you can’t do so again until you finish a long rest.\n\n");
                    }
                    break;
            }
        }
    }
}
