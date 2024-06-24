using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using YamlDotNet.Serialization;

namespace _5eCharDisplay
{
    internal class Item
	{
		public int physicalArmor { set; get; }
		public int magicArmor { set; get; }
		public string name { set; get; }
		public string returnCode { set; get; }
		public string type { set; get; }
		public string armorSetEffect { set; get; }
		public string statisticName { set; get; }
		public int diceNum { set; get; }
		public int diceSides { set; get; }
		public int armorSetCode { set; get; }

		private Statistic stat;
		private Die damageDie;

		public static Item fromYAML(string fName)
		{
			Item returned = null;
			using (FileStream fin = File.OpenRead(fName))
			{
				TextReader reader = new StreamReader(fin);

				var deserializer = new Deserializer();
				returned = deserializer.Deserialize<Item>(reader);
			}
			returned.damageDie = new Die(returned.diceNum, returned.diceSides);
			returned.stat = new Statistic(returned.statisticName);


			return returned;
		}


		public Item(string alias, string variety, int phys, int mage, string returnable, int armorCode, string armorEffects)
		{
			List<string> lines = new List<string>
			{
				$"name: {alias}",
				$"type: {variety}",
				$"physicalArmor: {phys}",
				$"magicArmor: {mage}",
				$"returnCode: {returnable}",
				$"armorSetCode: {armorCode}",
				$"armorSetEffect: {armorEffects}"
			};

			File.WriteAllLines($@".\Data\Items\{alias}.yaml", lines);
		}

		public Item(string alias, string variety, int phys, int mage, string returnable, int armorCode)
		{
			List<string> lines = new List<string>
			{
				$"name: {alias}",
				$"type: {variety}",
				$"physicalArmor: {phys}",
				$"magicArmor: {mage}",
				$"returnCode: {returnable}",
				$"armorSetCode: {armorCode}"
			};

			File.WriteAllLines($@".\Data\Items\{alias}.yaml", lines);
		}

		public Item(string alias, string variety, int phys, int mage, string returnable)
		{
			List<string> lines = new List<string>
			{
				$"name: {alias}",
				$"type: {variety}",
				$"physicalArmor: {phys}",
				$"magicArmor: {mage}",
				$"returnCode: {returnable}",
				$"armorSetCode: 000"
			};

			File.WriteAllLines($@".\Data\Items\{alias}.yaml", lines);
		}

		public Item(string alias, string variety, string statistic, int dieNum, int die, string returnable)
		{
			List<string> lines = new List<string>
			{
				$"name: {alias}",
				$"type: {variety}",
				$"statisticName: {statistic}",
				$"returnCode: {returnable}",
				$"diceNum: {dieNum}",
				$"diceSides: {die}",
				$"returnCode: {returnable}"
			};

			File.WriteAllLines($@".\Data\Items\{alias}.yaml", lines);
		}
		public Item() { }


		public Die getDie() { return damageDie; }
		public string getDieType() { return $"{damageDie.getNum()}d{damageDie.getSides()}"; }
		public string getStat() { return stat.getType(); }
	}
}
