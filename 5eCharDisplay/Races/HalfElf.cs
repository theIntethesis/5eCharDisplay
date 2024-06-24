using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using YamlDotNet.Serialization;

namespace _5eCharDisplay.Races
{
    internal class HalfElf : charRace
    {
        public string Boost1 { get; set; }
        public string Boost2 { get; set; }
        public string Language { get; set; }
        public string Skill1 { set; get; }
        public string Skill2 { set; get; }
        public HalfElf()
        {
            speed = 30;
            ChaBoost = 2;
            languages = new List<string> { "Common", "Elvish" };
            abilities = new List<string> { "Darkvision", " - You can see in dim light within 60 feet of you as if it were bright light, and in darkness as if it were dim light.", "Fey Ancestry", " - You have advantage on saving throws against being charmed, and magic can’t put you to sleep." };
        }
        public static HalfElf fromYAML(string fName)
        {
            HalfElf returned = null;
            using (FileStream fin = File.OpenRead(fName))
            {
                TextReader reader = new StreamReader(fin);

                var deserializer = new Deserializer();
                returned = deserializer.Deserialize<HalfElf>(reader);
            }
            returned.languages.Add(returned.Language);
            returned.skillProfs.Add(returned.Skill1);
            returned.skillProfs.Add(returned.Skill2);
            switch (returned.Boost1)
            {
                case "Strength":
                    returned.StrBoost = 1;
                    break;
                case "Dexterity":
                    returned.DexBoost = 1;
                    break;
                case "Constitution":
                    returned.ConBoost = 1;
                    break;
                case "Intelligence":
                    returned.IntBoost = 1;
                    break;
                case "Wisdom":
                    returned.WisBoost = 1;
                    break;
            }
            switch (returned.Boost2)
            {
                case "Strength":
                    returned.StrBoost = 1;
                    break;
                case "Dexterity":
                    returned.DexBoost = 1;
                    break;
                case "Constitution":
                    returned.ConBoost = 1;
                    break;
                case "Intelligence":
                    returned.IntBoost = 1;
                    break;
                case "Wisdom":
                    returned.WisBoost = 1;
                    break;
            }
            return returned;
        }
    }
}
