using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5eCharDisplay.Races
{
    internal class Elf : charRace
    {
        public Elf(string subrace)
        {
            speed = 30;
            DexBoost = 2;
            languages = new List<string> { "Common", "Elvish" };
            abilities = new List<string> { "Darkvision", " - You can see in dim light within 60 feet of you as if it were bright light, and in darkness as if it were dim light.", "Fey Ancestry", " - You have advantage on saving throws against being charmed, and magic can’t put you to sleep.", "Trance", " - Elves don’t need to sleep. Instead, they meditate deeply, remaining semiconscious, for 4 hours a day. After resting in this way, you gain the same benefit that a human does from 8 hours of sleep." };
            skillProfs = new List<string> { "Perception" };
            switch (subrace)
            {
                case "High Elf":
                    IntBoost = 1;
                    weaponProfs = new List<string> { "Longsword", "Shortsword", "Longbow", "Shortbow" };
                    break;
                case "Wood Elf":
                    WisBoost = 1;
                    weaponProfs = new List<string> { "Longsword", "Shortsword", "Longbow", "Shortbow" };
                    speed = 35;
                    abilities.Add("Mask of the Wild");
                    abilities.Add(" - You can attempt to hide even when you are only lightly obscured by foliage, heavy rain, falling snow, mist, and other natural phenomena.");
                    break;
                case "Dark Elf":
                    ChaBoost = 1;
                    abilities.Add("Superior Darkvision");
                    abilities.Add(" - Your darkvision has a radius of 120 feet.");
                    abilities.Add("Sunlight Sensitivity");
                    abilities.Add(" - You have disadvantage on attack rolls and on Wisdom (Perception) checks that rely on sight when you, the target of your attack, or whatever you are trying to perceive is in direct sunlight.");
                    abilities.Add("Drow Magic");
                    abilities.Add(" - You know the dancing lights cantrip. When you reach 3rd level, you can cast the faerie fire spell once with this trait and regain the ability to do so when you finish a long rest. When you reach 5th level, you can cast the darkness spell once with this trait and regain the ability to do so when you finish a long rest. Charisma is your spellcasting ability for these spells.");
                    weaponProfs = new List<string> { "Rapier", "Shortsword", "Hand Crossbow" };
                    break;

            }
        }
    }
}
