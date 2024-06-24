using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using YamlDotNet.Serialization;

namespace _5eCharDisplay
{
    internal class Background
    {
        public string Feature { get; set; }
        public string Skill1 { get; set; }
        public string Skill2 { get; set; }
        public string Tool1 { get; set; }
        public string Tool2 { get; set; }
        public string Language1 { get; set; }
        public string Language2 { get; set; }
        public string[] PersonalityTraits { get; set; }
        public string[] Ideals { get; set; }
        public string[] Bonds { get; set; }
        public string[] Flaws { get; set; }

        public static Background fromYAML(string bName)
        {
            Background returned = null;
            using (FileStream fin = File.OpenRead($@".\Data\Backgrounds\{bName}.yaml"))
            {
                TextReader reader = new StreamReader(fin);

                var deserializer = new Deserializer();
                returned = deserializer.Deserialize<Background>(reader);
            }
            
            return returned;
        }
    }
}
