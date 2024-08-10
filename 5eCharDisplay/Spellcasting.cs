using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace _5eCharDisplay
{
	internal class Spellcasting
	{
		public charClass.SpellPrepMethod prepMethod { set; get; }
		public List<string> Cantrips { set; get; }
		public List<string> AlwaysPrepared = new List<string>();
		public List<string> PreparedSpells { set; get; }
		public List<string> FirstLevelSpells { set; get; }
		public List<string> SecondLevelSpells { set; get; }
		public List<string> ThirdLevelSpells { set; get; }
		public List<string> FourthLevelSpells { set; get; }
		public List<string> FifthLevelSpells { set; get; }
		public List<string> SixthLevelSpells { set; get; }
		public List<string> SeventhLevelSpells { set; get; }
		public List<string> EighthLevelSpells { set; get; }
		public List<string> NinthLevelSpells { set; get; }
		public Statistic spellcastingAbilityModifier { set; get; }
		public int[] spellSlots { get; set; }
		public int[] spellSlotsMax { set; get; }
		public int spellPrepLevel { set; get; }
		public static Spellcasting fromYAML(string fPath)
		{
			Spellcasting returned = null;
			using (FileStream fin = File.OpenRead(fPath))
			{
				TextReader reader = new StreamReader(fin);
				var deserializer = new Deserializer();
				returned = deserializer.Deserialize<Spellcasting>(reader);
			}
			return returned;
		}
	}
}
