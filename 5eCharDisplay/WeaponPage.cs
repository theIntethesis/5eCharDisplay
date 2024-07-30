using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace _5eCharDisplay
{
	[SupportedOSPlatform("windows")]
	public partial class WeaponPage : Form
	{
		Character player;
		List<Weapon> weapons = new();
		List<CheckBox> checkBoxes = new();
		public WeaponPage(Character PC)
		{
			player = PC;
			InitializeComponent();
			
			Size = new Size((int)(Width * .66), Height);
			var weaponList = Directory.GetFiles($@"./Data/Weapons/");
			for (int i = 0; i < weaponList.Count(); i++)
			{
				var cutOffExcessFront = weaponList[i].Substring(weaponList[i].LastIndexOf('/') + 1);
				var cutOffExcessEnd = cutOffExcessFront.Length - 5;
				weaponList[i] = cutOffExcessFront.Substring(0, cutOffExcessEnd);
			}
			var CurrentlyEquippedWeapons = Weapon.listFromYaml($@"./Data/Characters/{player.name}/{player.name}Weapons.yaml");
			foreach (string s in player.inventory)
			{
				foreach (var ar in weaponList)
				{
					if (s.Contains(ar))
					{
						var newA = Weapon.fromYaml(name: ar);
						if (weapons.Count == 0)
							weapons.Add(newA);
						else if (!weapons.Exists(a => a.Name == newA.Name))
						{
							weapons.Add(newA);
						}
					}
				}
			}
			Label NameLabel = new();
			Label damage = new();
			Label ABLabel = new Label();

			Controls.Add(NameLabel);
			NameLabel.Location = new Point(12, 12);
			NameLabel.Text = "Name";

			Controls.Add(damage);
			damage.Location = new Point(275, 12);
			damage.Text = "Damage";

			Controls.Add(ABLabel);
			ABLabel.Text = "Attack Bonus";
			ABLabel.Location = new Point(175, 12);
			ABLabel.AutoSize = true;

			int yCoord = NameLabel.Size.Height + 18;
			foreach (Weapon w in weapons)
			{
				var strings = Controller.CPage_GetWeaponButtonText(w, player);
				// Item1: Name
				// Item2: Attack Bonus
				// Item3: Damage

				CheckBox equippedBox = new CheckBox();
				Controls.Add(equippedBox);
				checkBoxes.Add(equippedBox);
				equippedBox.Location = new Point(12, yCoord);
				equippedBox.Text = strings.Item1;
				equippedBox.AutoSize = true;
				equippedBox.CheckedChanged += EquipWeapon;

				Label wDamage = new Label();
				Controls.Add(wDamage);
				wDamage.Location = new Point(250, yCoord);
				wDamage.Text = strings.Item3;
				wDamage.AutoSize = true;

				Label AttackBonus = new Label();
				Controls.Add(AttackBonus);
				AttackBonus.Location = new Point(200, yCoord);
				int bonus = strings.Item2;
				if (bonus >= 0)
					AttackBonus.Text = $"+{strings.Item2}";
				else
					AttackBonus.Text = $"{strings.Item2}";
				AttackBonus.AutoSize = true;
				yCoord += equippedBox.Height + 12;

				AutoSize = true;
			}
			if (CurrentlyEquippedWeapons != null)
			{
				foreach (var a in CurrentlyEquippedWeapons)
				{
					if(checkBoxes.Find(b => b.Text.Substring(0, b.Text.IndexOf('|') - 1) == a.Name) != null)
						checkBoxes.Find(b => b.Text.Substring(0, b.Text.IndexOf('|') - 1) == a.Name).Checked = true;
				}
			}
		}
		private void EquipWeapon(object sender, EventArgs e)
		{
			var box = sender as CheckBox;
			int index = checkBoxes.FindIndex(a => box.Text == a.Text);
			Weapon relevantWeapon = weapons[index];

			if (box.Checked)
			{
				if (!player.equippedWeapons.Any(a => a.Name == relevantWeapon.Name))
					player.equippedWeapons.Add(relevantWeapon);
			}
			else
			{
				player.equippedWeapons.RemoveAll(a => a.Name == relevantWeapon.Name);
			}
		}
	}
}
