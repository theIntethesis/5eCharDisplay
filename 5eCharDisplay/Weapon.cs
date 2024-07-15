using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using System.IO;

namespace _5eCharDisplay
{
    internal class Weapon
    {
		public string Name { get; set; }
		public List<Die> DamageDie { get; set; }
		public List<string> DamageType { set; get; }
		public List<string> Properties { set; get; }
		public int MagicBonus { set; get; }
		public List<string> Effects { set; get; }

		public static Weapon fromYaml(string aName = "", string fName = "")
		{
			Weapon returned = null;
			string file;
			if (!string.IsNullOrEmpty(aName))
				file = $@"./Data/Weapons/{aName}.yaml";
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
			 
			return returned;
		}

		public Weapon()
        {

        }
	}
}
