using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using System.IO;
using System.Reflection.Metadata.Ecma335;

namespace _5eCharDisplay
{
    public class Weapon
    {
		public string Name { get; set; }
		public List<Die> DamageDie { get; set; }
		public List<string> DamageType { set; get; }
		public List<string> Properties { set; get; }
		public int MagicBonus { set; get; }
		public List<string> Effects { set; get; }
		public WeaponType BaseType { set; get; }
		public ProficiencyType PType { set; get; }
		public enum WeaponType
		{
			Battleaxe,
			Blowgun,
			Club,
			Dagger,
			Dart,
			Flail,
			Glaive,
			Greataxe,
			Greatclub,
			Greatsword,
			Halberd,
			Hand_Crossbow,
			Handaxe,
			Heavy_Crossbow,
			Javelin,
			Lance,
			Light_Crossbow,
			Light_Hammer,
			Longbow,
			Longsword,
			Mace,
			Maul,
			Morningstar,
			Pike,
			Quarterstaff,
			Rapier,
			Scimitar,
			Shortbow,
			Shortsword,
			Sickle,
			Sling,
			Spear,
			Trident,
			War_Pick,
			Warhammer,
			Whip
		}
		public enum ProficiencyType
        {
			Unarmed, Simple, Martial, Exotic, Improvised
        }
		public static Weapon fromYaml(string name = "", string fName = "")
		{
			Weapon returned = null;
			string file;
			if (!string.IsNullOrEmpty(name))
				file = $@"./Data/Weapons/{name}.yaml";
			else if (!string.IsNullOrEmpty(fName))
				file = fName;
			else
				return null;
			using (FileStream fin = File.OpenRead(file))
			{
				TextReader reader = new StreamReader(fin);

				var deserializer = new Deserializer();
				returned = deserializer.Deserialize<Weapon>(reader);
			}

			for(int i = 0; i < returned.Effects.Count; i++)
			{
				returned.Effects[i] = returned.Effects[i].Replace("~", returned.Name);
            }
			 
			return returned;
		}

		public static List<Weapon> listFromYaml(string fName)
		{
			List<Weapon> returned = null;
			using (FileStream fin = File.OpenRead(fName))
			{
				TextReader reader = new StreamReader(fin);

				var deserializer = new Deserializer();
				returned = deserializer.Deserialize<List<Weapon>>(reader);
			}
			return returned;
		}

		public Weapon()
        {

        }
	}
}
