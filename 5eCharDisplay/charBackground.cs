using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using YamlDotNet.Serialization;

namespace _5eCharDisplay
{
    internal class charBackground
    {
        public int pTrait { set; get; }
        public int Ideal { set; get; }
        public int Flaw { set; get; }
        public int Bond { set; get; }
        public Background back;
        public string[] getLanguages()
        {
            return new[] { back.Language1, back.Language2 };
        }
        public string[] getSkillProfs()
        {
            return new[] { back.Skill2, back.Skill1 };
        }
        public string[] getToolProfs()
        {
            return new[] { back.Tool1, back.Tool2 };
        }
        public string getPTrait()
        {
            return back.PersonalityTraits[pTrait];
        }
        public string getBond()
        {
            return back.Bonds[Bond];
        }
        public string getFlaw()
        {
            return back.Flaws[Flaw];
        }
        public string getIdeal()
        {
            return back.Ideals[Ideal];
        }
        public string getFeature()
        {
            return back.Feature;
        }
        public static charBackground fromYAML(string playerName, string bName)
        {
            charBackground returned = null;
            using (FileStream fin = File.OpenRead($@".\Data\Characters\{playerName}\{playerName}Background.yaml"))
            {
                TextReader reader = new StreamReader(fin);

                var deserializer = new Deserializer();
                returned = deserializer.Deserialize<charBackground>(reader);
            }
            returned.back = Background.fromYAML(bName);

            return returned;
        }

    }
}
