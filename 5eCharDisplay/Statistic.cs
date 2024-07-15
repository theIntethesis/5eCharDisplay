using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5eCharDisplay
{
	internal class Statistic
	{
		private double value;
		private string name;
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
	}
}
