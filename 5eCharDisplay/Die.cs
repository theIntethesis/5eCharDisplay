﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5eCharDisplay
{
    internal class Die
	{

		private static Random rand = new Random();
		private int sides, num;

		public Die(int num, int sides)
		{
			this.sides = sides;
			this.num = num;
		}
		public Die(int sides)
		{
			this.num = 1;
			this.sides = sides;
		}
		public int roll()
		{
			int total = 0, numDice = num;
			while (numDice > 0)
			{
				total += rand.Next(1, sides + 1);
				numDice--;
			}
			return total;
		}
		public int getAverage()
        {
			return (int)Math.Ceiling((sides + 1) / 2.0);
        }
		public int getSides() { return sides; }
		public int getNum() { return num; }
        public override string ToString()
        {
            return $"{num}d{sides}";
        }
        public override bool Equals(object obj)
        {
			Die secondDie = obj as Die;
			if (sides == secondDie.sides && num == secondDie.num) return true;
			else return false;
        }
    }
}
