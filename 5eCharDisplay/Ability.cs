using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace _5eCharDisplay
{
    internal class Ability
    {
        public string Name { set; get; }
        public string Description { set; get; }
        public AbilityUses uses { set; get; }
        public Character.Stat numUsesStat { set; get; }
        public RefreshOn refresh { set; get; }
        public int levelAt { set; get; }
        internal enum AbilityUses
        {
            NoAbility,
            Proficiency,
            AbilityMod,
            Number1
        }
        internal enum RefreshOn
        {
            NoRefresh,
            ShortRest,
            LongRest
        }

        public static Ability fromYaml(string fPath)
        {
            Ability returned = null;
            using (FileStream fin = File.OpenRead(fPath))
            {
                TextReader reader = new StreamReader(fin);

                var deserializer = new Deserializer();
                returned = deserializer.Deserialize<Ability>(reader);
            }
            returned.Description = returned.Description.Replace("\\n", "\n");
            returned.Description = returned.Description.Replace("\\t", "   ");
            return returned;
        }
        public static List<Ability> ListFromYaml(string fPath)
        {
            List<Ability> returned = null;
            using (FileStream fin = File.OpenRead(fPath))
            {
                TextReader reader = new StreamReader(fin);

                var deserializer = new Deserializer();
                returned = deserializer.Deserialize<List<Ability>>(reader);
            }
            foreach(var a in returned)
            {
                a.Description = a.Description.Replace("\\n", "\n");
                a.Description = a.Description.Replace("\\t", "\t");
            }
            return returned;
        }
    }
}
