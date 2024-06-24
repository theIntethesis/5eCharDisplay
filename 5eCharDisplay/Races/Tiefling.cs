using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5eCharDisplay
{
    internal class Tiefling : charRace
    {
        public Tiefling()
        {
            speed = 30;
            ChaBoost = 2;
            IntBoost = 1;
            languages = new List<string> { "Common", "Infernal" };
            abilities = new List<string> { "Darkvision", " - You can see in dim light within 60 feet of you as if it were bright light, and in darkness as if it were dim light.", "Hellish Resistance", " - You have resistance to fire damage.", "Infernal Legacy", " - You know the thaumaturgy cantrip. When you reach 3rd level, you can cast the hellish rebuke spell as a 2nd-level spell once with this trait and regain the ability to do so when you finish a long rest. When you reach 5th level, you can cast the darkness spell once with this trait and regain the ability to do so when you finish a long rest. Charisma is your spellcasting ability for these spells." };
        }
    }
}
