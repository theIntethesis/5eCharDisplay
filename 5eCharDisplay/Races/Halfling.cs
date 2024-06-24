using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5eCharDisplay
{
    internal class Halfling : charRace
    {
        public Halfling(string subrace)
        {
            speed = 25;
            DexBoost = 2;
            languages = new List<string> { "Common", "Halfling" };
            abilities = new List<string> { "Lucky", " - When you roll a 1 on the d20 for an attack roll, ability check, or saving throw, you can reroll the die and must use the new roll.\n\n", "Brave\n - You have advantage on saving throws against being frightened.\n\n", "Halfling Nimbleness\n - You can move through the space of any creature that is of a size larger than yours.\n\n" };
            switch (subrace)
            {
                case "Lightfoot":
                    ChaBoost = 1;
                    abilities.Add("Naturally Stealthy");
                    abilities.Add(" - You can attempt to hide even when you are obscured only by a creature that is at least one size larger than you.\n\n");
                    break;
                case "Stout":
                    ConBoost = 1;
                    abilities.Add("Stout Resilience");
                    abilities.Add(" - You have advantage on saving throws against poison, and you have resistance against poison damage.\n\n");
                    break;
            }
        }
    }
}
