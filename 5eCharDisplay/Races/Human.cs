using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using YamlDotNet.Serialization;

namespace _5eCharDisplay.Races
{
    internal class Human : charRace
    {
        public string Language { set; get; }
        public Human()
        {
            speed = 30;
            StrBoost = 1;
            DexBoost = 1;
            ConBoost = 1;
            IntBoost = 1;
            WisBoost = 1;
            ChaBoost = 1;
            languages = new List<string> { "Common" };
        }
        public static Human fromYAML(string fName)
        {
            Human returned = null;
            using (FileStream fin = File.OpenRead(fName))
            {
                TextReader reader = new StreamReader(fin);

                var deserializer = new Deserializer();
                returned = deserializer.Deserialize<Human>(reader);
            }
            returned.languages.Add(returned.Language);
            return returned;
        }
    }
}
