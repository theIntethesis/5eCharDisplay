using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using YamlDotNet.Serialization;

namespace _5eCharDisplay
{
    internal class Spell
    {
        public int level { get; set; }
        public string name { get; set; }
        public string duration { get; set; }
        public string castingTime { get; set; }
        public string range { get; set; }
        public string school { get; set; }
        public string components { get; set; }
        public string description { get; set; }
        public bool ritual { get; set; }
        public string getDescription(charClass charClass = null)
        {
            string output;
            int index = description.IndexOf("At Higher Levels.");
            if (charClass != null)
            {
                if (name == "Eldritch Blast")
                {
                    Classes.Warlock warlock = charClass as Classes.Warlock;
                    if (warlock.EldritchInvocations.Contains("Eldritch Spear"))
                    {
                        range = "300 ft";
                    }
                }
            }
            if(index != -1)
            {
                output =  $"{description.Substring(0, index)}\n\n{description.Substring(index)}";
            }
            else
                output = description;
            if (charClass != null)
            {
                if (name == "Eldritch Blast")
                {
                    output += "\n\n";
                    Classes.Warlock warlock = charClass as Classes.Warlock;
                    foreach (string s in warlock.EldritchInvocations)
                    {
                        switch (s)
                        {
                            case "Agonizing Blast":
                                output += $"   - When you cast eldritch blast, it deals an additional +{warlock.getAbilityModifiers(5)} damage on a hit.\n\n";
                                break;
                            case "Grasp of Hadar":
                                output += $"   - Once on each of your turns when you hit a creature with your eldritch blast, you can move that creature in a straight line 10 feet closer to you.\n\n";
                                break;
                            case "Lance of Lethargy":
                                output += $"   - Once on each of your turns when you hit a creature with your eldritch blast, you can reduce that creature’s speed by 10 feet until the end of your next turn.\n\n";
                                break;
                            case "Repelling Blast":
                                output += $"   - When you hit a creature with eldritch blast, you can push the creature up to 10 feet away from you in a straight line.\n\n";
                                break;
                        }
                    }
                }
            }


            return output;

        }
        public static Spell fromYAML(string name)
        {
            Spell returned = null;
            try {
                using (FileStream fin = File.OpenRead($@".\Data\Spells\{name}.yaml"))
                {
                    TextReader reader = new StreamReader(fin);

                    var deserializer = new Deserializer();
                    returned = deserializer.Deserialize<Spell>(reader);
                }
                return returned;
            }
            catch
            {
                returned = new Spell();
                returned.name = "Spell Not Found";
                returned.description = "Error. Spell File Not Found.";
                returned.duration = "Right now";
                returned.school = "Skill Issue";
                returned.range = "Within memory";
                returned.level = 12;
                returned.castingTime = "One Femtosecond";
                returned.components = "One .yaml File, One M2 down.";
                return returned;
            }

            
        }
    }
}
