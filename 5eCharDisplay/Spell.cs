using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using YamlDotNet.Serialization;

namespace _5eCharDisplay
{
	internal class School
	{
		public string index { set; get; }
		public string name { set; get; }
		public string url { set; get; }
		public School() { }
	}
	internal class Damage
	{
		Dictionary<int, string> damage_at_slot_level { set; get; }
		DamageType damage_type { set; get; }
	}
	internal class DamageType
	{
		string index { set; get; }
		string name { set; get; }
		string url { set; get; }
	}
	internal class Spell
	{
		
		public string index { get; set; }
		public string name { get; set; }
		public List<string> desc { get; set; }
		public List<string> higher_level { get; set; }
		public string range { get; set; }
		public List<string> components { get; set; }
		public string material { get; set; }
		public string duration { get; set; }
		public bool concentration { get; set; }
		public int level { get; set; }
		public Damage damage { get; set; }
		public School school { get; set; }
		public string url { get; set; }
		
		public string casting_time { get; set; }
		public bool ritual { get; set; }
		public string getDescription(charClass charClass = null)
		{
			StringBuilder output = new();
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
			foreach (string d in desc)
			{
				output.Append($"{d} ");
			}
			if(higher_level != null && higher_level.Count > 0)
			{
				output.Append("\n\nAt Higher Levels.   ");
				foreach(string h in higher_level)
				{
					output.AppendLine($"{h}");
				}
			}
			if (charClass != null)
			{
				if (name == "Eldritch Blast")
				{
					output.Append("\n\n");
					Classes.Warlock warlock = charClass as Classes.Warlock;
					foreach (string s in warlock.EldritchInvocations)
					{
						switch (s)
						{
							case "Agonizing Blast":
								output.Append($"   - When you cast eldritch blast, it deals an additional +{warlock.getAbilityModifiers(5)} damage on a hit.\n\n");
								break;
							case "Grasp of Hadar":
								output.Append($"   - Once on each of your turns when you hit a creature with your eldritch blast, you can move that creature in a straight line 10 feet closer to you.\n\n");
								break;
							case "Lance of Lethargy":
								output.Append($"   - Once on each of your turns when you hit a creature with your eldritch blast, you can reduce that creature’s speed by 10 feet until the end of your next turn.\n\n");
								break;
							case "Repelling Blast":
								output.Append($"   - When you hit a creature with eldritch blast, you can push the creature up to 10 feet away from you in a straight line.\n\n");
								break;
						}
					}
				}
			}
			return output.ToString();
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
				returned.desc = new();
				returned.desc.Add("Error. Spell File Not Found.");
				returned.duration = "Right now";
				returned.school = new();
				returned.school.name = "Skill Issue";
				returned.range = "Within memory";
				returned.level = 12;
				returned.casting_time = "One Femtosecond";
				returned.components = new();
				returned.components.Add("One .yaml File, One M2 down.");
				return returned;
			}

			
		}
	}
}
