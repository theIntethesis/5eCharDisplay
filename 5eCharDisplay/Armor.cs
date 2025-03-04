﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using System.IO;
using System.Text.RegularExpressions;

namespace _5eCharDisplay
{
	public class Armor
	{
		public string Name { get; set; }
		public string AC { get; set; }
		public int DexMax { get; set; }
		public enum ArmorType { Unarmored, Light, Medium, Heavy, Shield, Other }
		public ArmorType aType { get; set; }
		public bool stealthDis { get; set; }
		public int StrReq { get; set; }
		public int ArmorClass = 0;

		public static Armor fromYaml(string aName = "", string fName = "")
		{
			Armor returned = null;
			string file = "";
			if (!string.IsNullOrEmpty(aName))
				file = $@"./Data/Armor/{aName}.yaml";
			else if (!string.IsNullOrEmpty(fName))
				file = fName;
			else
				return null;
			using (FileStream fin = File.OpenRead(file))
			{
				TextReader reader = new StreamReader(fin);

				var deserializer = new Deserializer();
				returned = deserializer.Deserialize<Armor>(reader);
			}
			return returned;
		}
		public static List<Armor> listFromYaml(string fName)
		{
			List<Armor> returned = null;
			using (FileStream fin = File.OpenRead(fName))
			{
				TextReader reader = new StreamReader(fin);

				var deserializer = new Deserializer();
				returned = deserializer.Deserialize<List<Armor>>(reader);
			}
			return returned;
		}
	}
}
