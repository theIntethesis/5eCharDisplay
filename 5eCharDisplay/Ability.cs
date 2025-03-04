﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public static Ability fromYaml(string fPath, Func<string, string> GetValue = null)
        {
            Ability returned = null;
            using (FileStream fin = File.OpenRead(fPath))
            {
                TextReader reader = new StreamReader(fin);

                var deserializer = new Deserializer();
                returned = deserializer.Deserialize<Ability>(reader);
            }
            if (GetValue != null)
            {
                returned.Description = Regex.Replace(returned.Description, @"{(\w*)}", match => GetValue(match.Value));
            }
            returned.Description = returned.Description.Replace("\\n", "\n");
            returned.Description = returned.Description.Replace("\\t", "   ");
            return returned;
        }
        public static List<Ability> ListFromYaml(string fPath, Func<string, string> GetValue = null)
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
                if (GetValue != null)
                {
                    a.Description = Regex.Replace(a.Description, @"{(\w*)}", match => GetValue(match.Value));
                }
                a.Description = a.Description.Replace("\\n", "\n");
                a.Description = a.Description.Replace("\\t", "   ");
            }
            return returned;
        }
    }
}
