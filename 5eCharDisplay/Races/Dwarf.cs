using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5eCharDisplay.Races
{
    internal class Dwarf : charRace
    {
        public Dwarf(string subrace)
        {
            speed = 25;
            ConBoost = 2;
            languages = new List<string> { "Common", "Dwarvish" };
            abilities = new List<string> { "Darkvision", " - You can see in dim light within 60 feet of you as if it were bright light, and in darkness as if it were dim light.", "Dwarven Resilience", " - You have advantage on saving throws against poison, and you have resistance against poison damage.", "Stonecunning", " - Whenever you make an Intelligence (History) check related to the origin of stonework, you are considered proficient in the History skill and add double your proficiency bonus to the check, instead of your normal proficiency bonus." };
            weaponProfs = new List<string> { "Battleaxe", "Handaxe", "Light Hammer", "Warhammer"};
            switch (subrace)
            {
                case "Hill Dwarf":
                    WisBoost = 1;
                    hpBoost = 1;
                    break;
                case "Mountain Dwarf":
                    StrBoost = 2;
                    armorProfs = new List<string> { "Light Armor", "Medium Armor" };
                    break;
            }
        }
    }
}
