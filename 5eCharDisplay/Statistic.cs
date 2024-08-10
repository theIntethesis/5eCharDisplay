using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace _5eCharDisplay
{
	internal class Statistic
	{
		public double value { set; get; }
		public string name { set; get; }
        public int getMod() { return (int)Math.Floor((value - 10) / 2); }
		public int getValue() { return (int)value; }
		public string getType() { return name; }
		public void setValue(int val)
		{
			value = val;
		}
		public Statistic(string alias)
		{
			name = alias;
			value = 10;
		}
		public Statistic()
		{

		}
		public override string ToString()
		{
			if(getMod() < 0)
			{
				return $"{getMod()}";
			}
			else if (getMod() == 0)
			{
				return "0";
			}
			else
			{
				return $"+{getMod()}";
			}
		}
		public static Statistic FromYAML(string fPath)
		{
            Statistic returned = null;
            using (FileStream fin = File.OpenRead(fPath))
            {
                TextReader reader = new StreamReader(fin);
                var deserializer = new Deserializer();
                returned = deserializer.Deserialize<Statistic>(reader);
            }
            return returned;
        }
	}
}
