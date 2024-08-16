using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace _5eCharDisplay
{
    public class Feat
    {
        public string name { set; get; }
        public string prerequisite { set; get; }
        public string description { set; get; }
        public int[] asiboosts { get; set; }
        public int addSpeed { get; set; }
        public List<string> SkillProfAdd { get; set; }
        public List<string> WeaponProfAdd { get; set; }
        public Armor ArmorAdd { get; set; }
        public Weapon WeaponAdd { get; set; }
        public Dictionary<string, int> SkillBonus { get; set; }
        public void SkillModifier(object sender, SkillModifierArgs e){
            if (SkillBonus == null)
                return;
            foreach(var sb in SkillBonus)
            {
                if(e.skill == sb.Key)
                    e.ret += sb.Value;
            }
            return;
        }

        public static Feat FromYAML(string fName, string asipick = "")
        {
            Feat returned = null;
            using (FileStream fin = File.OpenRead($@".\Data\Feats\{fName}.yaml"))
            {
                TextReader reader = new StreamReader(fin);

                var deserializer = new Deserializer();
                returned = deserializer.Deserialize<Feat>(reader);
            }

            if(returned.name == "Ability Score Increase")
            {
                Regex pattern = new Regex(@"(STR|DEX|CON|INT|WIS|CHA)(\d+)");
                MatchCollection matches = pattern.Matches(asipick);
                foreach (Match match in matches)
                {
                    string matched = match.ToString();
                    int boost = int.Parse(match.Groups[2].Value);
                    if (matched.Contains("STR"))
                        returned.asiboosts[0] += boost;
                    if (matched.Contains("DEX"))
                        returned.asiboosts[1] += boost;
                    if (matched.Contains("CON"))
                        returned.asiboosts[2] += boost;
                    if (matched.Contains("INT"))
                        returned.asiboosts[3] += boost;
                    if (matched.Contains("WIS"))
                        returned.asiboosts[4] += boost;
                    if (matched.Contains("CHA"))
                        returned.asiboosts[5] += boost;
                }
            }
            return returned;
        }

    }
}
