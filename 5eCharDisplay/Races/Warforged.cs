using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using System.IO;

namespace _5eCharDisplay.Races
{
    internal class Warforged : charRace
    {
        public string Boost { set; get; }
        public string Skill { set; get; }
        public string Tool { set; get; }
        public string Language { set; get; }
        public Warforged()
        {
            speed = 30;
            ConBoost = 2;
            ACBoost = 1;
            languages = new List<string> { "Common" };
            abilities = new List<string> { "Constructed Resilience", " - Adv against being poisoned, resistance to poison\n - Don't need to eat, drink, or breathe\n - Immune to disease\n - Don't need to sleep, magic can't put you to sleep", "Sentry's Rest", " - When you long rest, you appear inert, but don't fall unconscious", "Integrated Protection", " - +1 to AC, Only don armor with proficiency, armor can't be removed from you." };
        }
        public static Warforged fromYAML(string fName)
        {
            Warforged returned = null;
            using (FileStream fin = File.OpenRead(fName))
            {
                TextReader reader = new StreamReader(fin);

                var deserializer = new Deserializer();
                returned = deserializer.Deserialize<Warforged>(reader);
            }
            switch (returned.Boost)
            {
                case "Strength":
                    returned.StrBoost = 1;
                    break;
                case "Dexterity":
                    returned.DexBoost = 1;
                    break;
                case "Intelligence":
                    returned.IntBoost = 1;
                    break;
                case "Wisdom":
                    returned.WisBoost = 1;
                    break;
                case "Charisma":
                    returned.ChaBoost = 1;
                    break;
            }
            returned.languages.Add(returned.Language);
            returned.skillProfs.Add(returned.Skill);
            returned.toolProfs.Add(returned.Tool);
            return returned;
        }
    }
}
