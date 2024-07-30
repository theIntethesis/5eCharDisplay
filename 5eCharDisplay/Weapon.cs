using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using System.IO;

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
		public Character.Stat stat { set; get; }
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
			Whip,
			Other = -1
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

			int num = 0;
			if(returned.Effects != null)
				num = returned.Effects.Count;
			for(int i = 0; i < num; i++)
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
			if(returned != null)
				foreach(Weapon w in returned)
				{
				    int num = 0;
				    if (w.Effects != null)
				        num = w.Effects.Count;
				    for (int i = 0; i < num; i++)
				    {
				        w.Effects[i] = w.Effects[i].Replace("~", w.Name);
					}
				}
            

            return returned;
		}

		public Weapon()
        {

        }
	}
}
