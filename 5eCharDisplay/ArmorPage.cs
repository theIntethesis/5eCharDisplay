using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _5eCharDisplay
{
	public partial class ArmorPage : Form
	{
		Character player;
		List<CheckBox> checkBoxes = new List<CheckBox>();
		List<Armor> armors = new List<Armor>();
		internal ArmorPage(Character PC)
		{
			InitializeComponent();
			player = PC;
			var armorList = Directory.GetFiles($@"./Data/Armor/");
			for(int i = 0; i < armorList.Count(); i++)
			{
				var cutOffExcessFront = armorList[i].Substring(armorList[i].LastIndexOf('/') + 1);
				var cutOffExcessEnd = cutOffExcessFront.Length - 5;
				armorList[i] = cutOffExcessFront.Substring(0, cutOffExcessEnd);
			}
			var CurrentlyWornArmor = Armor.listFromYaml($@"./Data/Characters/{player.name}/{player.name}Armor.yaml");
			foreach(string s in player.inventory)
			{
				if (armorList.Contains(s))
				{
					var newA = Armor.fromYaml(aName: s);
					if (armors.Count == 0)
						armors.Add(newA);
					else if (!armors.Exists(a => a.Name == newA.Name))
					{
						armors.Add(newA);
					}
				}
			}
			if(player.myClasses.Any(cClass => cClass.FirstLevelSpells.Contains("Mage Armor")))
            {
				armors.Add(Armor.fromYaml(aName: "Mage Armor"));
            }
			ACLabel.Location = new Point(125, 12);
			NameLabel.Location = new Point(12, 12);
			Label ATLabel = new Label();
			Controls.Add(ATLabel);
			ATLabel.Text = "Armor Type";
			ATLabel.Location = new Point(175, 12);
			ATLabel.AutoSize = true;

			int yCoord = NameLabel.Size.Height + 18;
			foreach (Armor a in armors)
			{
				CheckBox equippedBox = new CheckBox();
				Controls.Add(equippedBox);
				checkBoxes.Add(equippedBox);
				equippedBox.Location = new Point(12, yCoord);
				equippedBox.Text = a.Name;
				equippedBox.AutoSize = true;
				equippedBox.CheckedChanged += EquipArmor;

				Label armorBonus = new Label();
				Controls.Add(armorBonus);
				armorBonus.Location = new Point(125, yCoord);
				armorBonus.Text = a.AC;
				armorBonus.AutoSize = true;

				Label armorType = new Label();
				Controls.Add(armorType);
				armorType.Location = new Point(175, yCoord);
				armorType.Text = a.aType.ToString();
				armorType.AutoSize = true;
				yCoord += equippedBox.Height + 12;

				AutoSize = true;
			}
			if(CurrentlyWornArmor != null)
			{
				foreach (var a in CurrentlyWornArmor)
				{
					checkBoxes.Find(b => b.Text == a.Name).Checked = true;
				}
			}
		}
		private void EquipArmor(object sender, EventArgs e)
		{
			var box = sender as CheckBox;
			int index = checkBoxes.FindIndex(a => box.Text == a.Text);
			Armor releventArmor = armors[index];

			if (box.Checked)
			{
				if(!player.wornArmor.Any(a => a.Name == releventArmor.Name))
					player.wornArmor.Add(releventArmor);
				if(releventArmor.aType != Armor.ArmorType.Shield)
				{
					for(int i = 0; i < armors.Count; i++)
					{
						if(armors[i].aType != Armor.ArmorType.Shield && checkBoxes[i] != sender)
						{
							checkBoxes[i].Enabled = false;
						}
					}
				}
				else
				{
					for (int i = 0; i < armors.Count; i++)
					{
						if (armors[i].aType == Armor.ArmorType.Shield && checkBoxes[i] != sender)
						{
							checkBoxes[i].Enabled = false;
						}
					}
				}
			}
			else
			{
				player.wornArmor.RemoveAll(a => a.Name == releventArmor.Name);
				if (releventArmor.aType != Armor.ArmorType.Shield)
				{
					for (int i = 0; i < armors.Count; i++)
					{
						if (armors[i].aType != Armor.ArmorType.Shield)
						{
							checkBoxes[i].Enabled = true;
						}
					}
				}
				else
				{
					for (int i = 0; i < armors.Count; i++)
					{
						if (armors[i].aType == Armor.ArmorType.Shield)
						{
							checkBoxes[i].Enabled = true;
						}
					}
				}
			}
		}
	}
}
