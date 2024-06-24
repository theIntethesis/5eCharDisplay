using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5eCharDisplay.Races
{
    internal class HalfOrc : charRace
    {
        public HalfOrc()
        {
            speed = 30;
            StrBoost = 2;
            ConBoost = 1;
            languages = new List<string> { "Common", "Orcish" };
            abilities = new List<string> { "Darkvision", " - You can see in dim light within 60 feet of you as if it were bright light, and in darkness as if it were dim light.", "Relentless Endurance", " - When you are reduced to 0 hit points but not killed outright, you can drop to 1 hit point instead. You can’t use this feature again until you finish a long rest.", "Savage Attacks", " - When you score a critical hit with a melee weapon attack, you can roll one of the weapon’s damage dice one additional time and add it to the extra damage of the critical hit." };
            skillProfs = new List<string> { "Intimidation" };
        }
    }
}
