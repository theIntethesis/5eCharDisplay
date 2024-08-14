using System;
using System.Threading;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.Versioning;
using System.Runtime.CompilerServices;
using System.Text;

namespace _5eCharDisplay
{
	[SupportedOSPlatform("windows")]
	public partial class CharacterPage : Form
	{
		Die d20 = new Die(20);
		Character player;
		bool details = false;
		int defaultWidth = 828;
		public CharacterPage(string charName)
		{
			player = Character.fromYAML(charName);

			InitializeComponent();
			NameLabel.Text = player.name;

			StrScore.Text = player.strength.getValue().ToString();
			StrMod.Text = player.strength.ToString();

			DexScore.Text = player.dexterity.getValue().ToString();
			DexMod.Text = player.dexterity.ToString();

			ConScore.Text = player.constitution.getValue().ToString();
			ConMod.Text = player.constitution.ToString();

			IntScore.Text = player.intelligence.getValue().ToString();
			IntMod.Text = player.intelligence.ToString();

			WisScore.Text = player.wisdom.getValue().ToString();
			WisMod.Text = player.wisdom.ToString();

			ChaScore.Text = player.charisma.getValue().ToString();
			ChaMod.Text = player.charisma.ToString();

			Text = $"{player.name}'s Character Sheet";
			ClassBox.Text = player.getClassLevelEXP();
			BackLabel.Text = player.background;
			if (player.subrace == null)
				RaceLabel.Text = player.race;
			else
				RaceLabel.Text = player.subrace;
			AlignLabel.Text = player.alignment;
			ProfNum.Text = $"+{player.proficiency}";

			HPMax.Text = player.maxHitPoints.ToString();
			HPCurrent.Text = player.hitPoints.ToString();
			TempHP.Text = "0";

			int ylocation = 6;
			foreach(var cla in player.myClasses)
			{
				TabPage tab = new TabPage();
				ClassTabControl.Controls.Add(tab);
				tab.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
				tab.Text = cla.classname.ToString();
				tab.Size = new Size(279, 598);
				tab.Padding = new Padding(3);
				tab.AutoScroll = true;
				foreach (var g in cla.getInfoBoxes())
				{
					tab.Controls.Add(g);
					g.Location = new Point(6, ylocation);
					ylocation += g.Height + 12;
				}

				ylocation = 6;
			}

			TabPage RTab = new TabPage();
			ClassTabControl.Controls.Add(RTab);
			RTab.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
			RTab.Text = "Racial Features";
			RTab.Size = new Size(279, 598);
			RTab.Padding = new Padding(3);
			RTab.AutoScroll = true;
			foreach (GroupBox g in player.myRace.getAbilityBoxes())
			{
				RTab.Controls.Add(g);
				g.Location = new Point(6, ylocation);
				ylocation += g.Height + 12;
			}

			ACNum.Text = $"{Controller.CPage_GetArmorClass(player) + player.myRace.getACBoost()}";

			SteLabel.Text = Controller.CPage_GetStealthLabel(player);

			int WeaponYValues = 6;
			foreach (Weapon w in player.equippedWeapons)
			{
				GroupBox box = new();
				Label wName = new Label();
				Button aBonus = new Button();
				Button dRoll = new Button();
				WeaponsTab.Controls.Add(box);

				box.Size = new Size(284, 60);
				box.Location = new Point(0, WeaponYValues);

				box.Controls.Add(wName);
				box.Controls.Add(aBonus);
				box.Controls.Add(dRoll);

				var strings = Controller.CPage_GetWeaponButtonText(w, player);

				wName.Text = strings.Item1;
				wName.AutoSize = true;
				wName.Location = new Point(9, 12);

				wName.Click += ShowWeaponDetails;

				if (strings.Item2 >= 0) aBonus.Text = $"+{strings.Item2}";
				else aBonus.Text = $"{strings.Item2}";
				aBonus.Size = new Size(40, 23);
				aBonus.Location = new Point(box.Width - aBonus.Size.Width - 3, 9);
				aBonus.Click += AttackRoll;

				dRoll.Text = strings.Item3;
				dRoll.AutoSize = true;
				dRoll.Location = new Point(box.Width - dRoll.Size.Width - 3, 33);
				dRoll.MouseDown += DamageRoll;

				WeaponYValues += 56;
			}
			weaponEquipButton.Click += equipWeapons;

			SpeedNum.Text = $"{player.myRace.getSpeed()}";

			ProfsText.Text = Controller.CPage_GetProfList(player);

			int low = 8;
			foreach (charClass c in player.myClasses)
			{
				Label l = new Label();
				l.Text = $"{c.getRemHD()}d{c.getHitDie().getSides()}";
				l.Location = new Point(6, low);
				l.AutoSize = true;
				l.Font = new Font(FontFamily.GenericSansSerif, 12);
				l.Click += HitDieLabel_Click;
				HDPanel.Controls.Add(l);
				low += 18;
			}

			Inventory.Lines = player.inventory.ToArray();
			Inventory.KeyDown += UpdateInventory;

			foreach(EventHandler e in Controller.getLongRest(player))
			{
				LRButton.Click += e;
            }
            foreach (EventHandler e in Controller.getShortRest(player))
            {
                SRButton.Click += e;
            }

            if (!player.Spellcasting)
				SpellcastingToggle.Hide();

			(StrSaveProf.Checked, StrSaveNum.Text) = Controller.CPage_GetSkillProfs(player, "StrSave", player.strength);
			(DexSaveProf.Checked, DexSaveNum.Text) = Controller.CPage_GetSkillProfs(player, "DexSave", player.dexterity);
			(ConSaveProf.Checked, ConSaveNum.Text) = Controller.CPage_GetSkillProfs(player, "ConSave", player.constitution);
			(IntSaveProf.Checked, IntSaveNum.Text) = Controller.CPage_GetSkillProfs(player, "IntSave", player.intelligence);
			(WisSaveProf.Checked, WisSaveNum.Text) = Controller.CPage_GetSkillProfs(player, "WisSave", player.wisdom);
			(ChaSaveProf.Checked, ChaSaveNum.Text) = Controller.CPage_GetSkillProfs(player, "ChaSave", player.charisma);


			(AcroProf.Checked, AcroNum.Text) = Controller.CPage_GetSkillProfs(player, "Acrobatics", player.dexterity);
			(AHandProf.Checked, AHandNum.Text) = Controller.CPage_GetSkillProfs(player, "Animal Handling", player.wisdom);
			(ArcanaProf.Checked, ArcanaNum.Text) = Controller.CPage_GetSkillProfs(player, "Arcana", player.intelligence);
			(AthProf.Checked, AthNum.Text) = Controller.CPage_GetSkillProfs(player, "Athletics", player.strength);
			(DecepProf.Checked, DecepNum.Text) = Controller.CPage_GetSkillProfs(player, "Deception", player.charisma);
			(HistProf.Checked, HistNum.Text) = Controller.CPage_GetSkillProfs(player, "History", player.intelligence);
			(InsightProf.Checked, InsightNum.Text) = Controller.CPage_GetSkillProfs(player, "Insight", player.wisdom);
			PassInsNum.Text = Controller.CPage_GetPassives(player, "Insight", player.wisdom);
			(IntimProf.Checked, IntimNum.Text) = Controller.CPage_GetSkillProfs(player, "Intimidation", player.charisma);
			(InvestProf.Checked, InvestNum.Text) = Controller.CPage_GetSkillProfs(player, "Investigation", player.intelligence);
			(MedProf.Checked, MedNum.Text) = Controller.CPage_GetSkillProfs(player, "Medicine", player.wisdom);
			(NatProf.Checked, NatNum.Text) = Controller.CPage_GetSkillProfs(player, "Nature", player.intelligence);
			(PercepProf.Checked, PercepNum.Text) = Controller.CPage_GetSkillProfs(player, "Perception", player.wisdom);
			PassPerNum.Text = Controller.CPage_GetPassives(player, "Perception", player.wisdom);
			(PerfProf.Checked, PerfNum.Text) = Controller.CPage_GetSkillProfs(player, "Performance", player.charisma);
			(PersProf.Checked, PersNum.Text) = Controller.CPage_GetSkillProfs(player, "Persuasion", player.charisma);
			(RelProf.Checked, RelNum.Text) = Controller.CPage_GetSkillProfs(player, "Religion", player.intelligence);
			(SleProf.Checked, SleNum.Text) = Controller.CPage_GetSkillProfs(player, "Sleight of Hand", player.dexterity);
			(SteProf.Checked, SteNum.Text) = Controller.CPage_GetSkillProfs(player, "Stealth", player.dexterity);
			(SurProf.Checked, SurNum.Text) = Controller.CPage_GetSkillProfs(player, "Survival", player.wisdom);
			InitNum.Text = Controller.CPage_GetSkillProfs(player, "Initiative", player.dexterity).Item2;


			Width -= 180;
            FormClosing += new FormClosingEventHandler(OnFormClose);

            DiceInput.KeyUp += DiceInputPressEnter;
			XPTicker.KeyUp += EXPInputPressEnter;
			XPTicker.Maximum = 355000;

			PTraitLabel.Text = player.myBackground.getPTrait();
			IdealsLabel.Text = player.myBackground.getIdeal();
			FlawLabel.Text = player.myBackground.getFlaw();
			BondLabel.Text = player.myBackground.getBond();
			FeatureLabel.Text = player.myBackground.getFeature();
		}

		private void DamageRoll(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			bool crit = false;
			if ((e as MouseEventArgs).Button == MouseButtons.Right)
				crit = true;
			
			DiceResult1.Text = $"{Controller.CPage_DamageRoll((sender as Button).Text, crit)}";
			DiceResult2.Text = "";
		}
		private void AttackRoll(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = int.Parse((sender as Button).Text.Substring(1));
			int roll1 = d20.roll();
			int roll2 = d20.roll();

			DiceResult1.Text = $"{roll1 + modifier}";
			if (roll1 == 20)
				DiceResult1.Text += " (CRIT!)";
			else if (roll1 == 1)
				DiceResult1.Text += " (FAIL!)";

			DiceResult2.Text = $"{roll2 + modifier}";
			if (roll2 == 20)
				DiceResult2.Text += " (CRIT!)";
			else if (roll2 == 1)
				DiceResult2.Text += " (FAIL!)";
		}
		private void UpdateInventory(object sender, EventArgs e)
		{
			var k = (e as KeyEventArgs).KeyCode;
			if (k == Keys.Enter || k == Keys.Back || k == Keys.Delete)
			{
				player.inventory.Clear();
				foreach (var item in Inventory.Lines)
				{
					player.inventory.Add(item);
				}
			}
		}
		private void DiceInputPressEnter(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Return)
				MiscRollButton_Click(sender, e);
		}
		private void EXPInputPressEnter(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Return || e.KeyValue == (int)Keys.Enter)
				XPButton_Click(sender, e);
		}
		private void AbilityCheckRoll(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int mod;
			string modText = (sender as Label).Text;
			if (modText.Contains("-") || !modText.Contains("+"))
				mod = int.Parse(modText);
			else
				mod = int.Parse(modText.Substring(1));
			DiceResult1.Text = $"{d20.roll() + mod}";
			DiceResult2.Text = $"{d20.roll() + mod}";
		}
		private void OnFormClose(object sender, CancelEventArgs e)
		{
			SaveInventory();
			SaveClassFeatures();
			SaveCharFeatures();
		}
		private void SaveInventory()
		{
			player.inventory.Clear();
			foreach (var item in Inventory.Lines)
			{
				player.inventory.Add(item);
			}
			File.WriteAllLines($@".\Data\Characters\{player.name}\{player.name}Inventory.txt", player.inventory.ToArray());
			return;
		}
		private void SaveClassFeatures()
		{
			//int classnum = 0;
			foreach (charClass c in player.myClasses)
			{
                var serializer = new YamlDotNet.Serialization.SerializerBuilder().Build();
                var yaml = serializer.Serialize(c);
                File.WriteAllText($@"./Data/Characters/{player.name}/{player.name}{c.classname.ToString()}.yaml", yaml);
/*
                string[] classInfo = c.getClassDetails(player.name);
				File.WriteAllLines($@".\Data\Characters\{player.name}\{player.name}{player.charClass[classnum]}.yaml", classInfo);
				classnum++;*/
			}
		}
		private void SaveCharFeatures()
		{
			string[] charInfo = File.ReadAllLines($@".\Data\Characters\{player.name}\{player.name}.yaml");
			charInfo[13] = $"experience: {player.experience}";
			File.WriteAllLines($@".\Data\Characters\{player.name}\{player.name}.yaml", charInfo);
		}
		private void HPUp_Click(object sender, EventArgs e)
		{
			player.affectHitPoints((int)HPModify.Value);
			HPCurrent.Text = player.hitPoints.ToString();
			TempHP.Text = player.tempHP.ToString();
			HPModify.Value = 0;
		}
		private void HPDown_Click(object sender, EventArgs e)
		{
			player.affectHitPoints(-(int)HPModify.Value);
			HPCurrent.Text = player.hitPoints.ToString();
			TempHP.Text = player.tempHP.ToString();
			HPModify.Value = 0;
		}
		private void HitDieLabel_Click(object sender, EventArgs e)
		{
			var labelText = sender as Label;
			string dieText = labelText.Text;
			int indexofd = dieText.IndexOf('d');
			int sides = Convert.ToInt32(dieText.Substring(indexofd + 1));
			Die die = new Die(sides);
			foreach (var c in player.myClasses)
			{
				if (c.getHitDie().Equals(die))
				{
					int rem = c.affectHD(-1);
					int healing = die.roll();
					if (player.hasFeat("Durable"))
						if (healing == 1)
							healing = 2;
					player.affectHitPoints(healing + player.constitution.getMod());
					HPCurrent.Text = player.hitPoints.ToString();
					labelText.Text = $"{rem}d{die.getSides()}";
					break;
				}
			}
		}
		private void LRButton_Click(object sender, EventArgs e)
		{
			player.hitPoints = player.maxHitPoints;
			HPCurrent.Text = player.hitPoints.ToString();
			int i = 0;
			foreach (Label l in HDPanel.Controls)
			{
				l.Text = $"{player.myClasses[i].getRemHD()}d{player.myClasses[i].getHitDie().getSides()}";
				i++;
			}
		}
		private void SetTHP_Click(object sender, EventArgs e)
		{
			player.tempHP = (int)HPModify.Value;
			TempHP.Text = player.tempHP.ToString();
			HPModify.Value = 0;
		}
		private void MiscRollButton_Click(object sender, EventArgs e)
		{
			string ParseMe = DiceInput.Text;
			if (ParseMe == "0")
				LastRoll.Text = "Last Roll";
			if (ParseMe != null && ParseMe != "")
			{
				LastRoll.Text = ParseMe;

				DiceResult1.Text = $"{RollDice(ParseMe)}";
				DiceResult2.Text = "";
				DiceInput.Text = "";
			}
		}
		private static int RollDice(string input)
		{
			// Define a regular expression pattern to match individual dice rolls
			Regex pattern = new Regex(@"(\d+)d(\d+)");

			// Use Regex.Matches to find all matching patterns in the input string
			MatchCollection matches = pattern.Matches(input);

			int totalResult = 0;
			Die die;
			foreach (Match match in matches)
			{
				// Extract the values from the matched groups
				int numberOfDice = int.Parse(match.Groups[1].Value);
				int numberOfSides = int.Parse(match.Groups[2].Value);

				// Simulate the dice rolls and calculate the result
				die = new Die(numberOfDice, numberOfSides);

				// Add the result to the total
				totalResult += die.roll();
			}

			// Add any modifiers found in the original input
			totalResult += GetModifier(input);

			return totalResult;
		}
		private static int GetModifier(string input)
		{
			// Define a regular expression pattern to match modifiers (e.g., "+ 2")
			Regex positive = new Regex(@"[+]\s*(\d+)");
			Regex negative = new Regex(@"[-]\s*(\d+)");

			// Use Regex.Matches to find all matching modifiers in the input string
			MatchCollection matchesPositive = positive.Matches(input);
			MatchCollection matchNegative = negative.Matches(input);

			// Sum up all the modifiers
			int modifier = matchesPositive.Cast<Match>().Select(m => int.Parse(m.Groups[1].Value)).Sum();
			modifier -= matchNegative.Cast<Match>().Select(m => int.Parse(m.Groups[1].Value)).Sum();

			return modifier;
		}
		private void LastRoll_Click(object sender, EventArgs e)
		{
			string ParseMe = LastRoll.Text;
			if (ParseMe != null && ParseMe != "Last Roll" && ParseMe != "")
			{
				DiceResult1.Text = $"{RollDice(ParseMe)}";
				DiceResult2.Text = "";
			}
		}
		private void SpellcastingToggle_Click(object sender, EventArgs e)
		{
			foreach(charClass cClass in player.myClasses)
            {
                if (cClass.spellcasting != null)
                {
                    SpellcastingPage spellcastingPage = new SpellcastingPage(player, cClass);
                    spellcastingPage.Show();
                }
            }
		}
		private void detailsButton_Click(object sender, EventArgs e)
		{
			details = !details;
			if (details)
			{
				Width += 180;
			}
			else
			{
				Width = defaultWidth;
			}
		}
		private void XPButton_Click(object sender, EventArgs e)
		{
			player.experience += (int)XPTicker.Value;
			XPTicker.Value = 0;

			ClassBox.Text = player.getClassLevelEXP();
		}
		private void ACNum_Click(object sender, EventArgs e)
		{
			MouseEventArgs mouse = e as MouseEventArgs;
			if (mouse.Button == MouseButtons.Right)
			{
				Form from = new ArmorPage(player);
				//from.Icon = new Icon(@"C:\Users\Hayden\Downloads\881288450786082876.ico");
				from.Location = new Point(400, 50);
				from.AutoSize = true;
				from.LostFocus += closeOnLostFocus;
				from.FormClosed += updateACOnClose;
				from.Show();
			}
		}
		private void equipWeapons(object sender, EventArgs e)
		{
			Form from = new WeaponPage(player);
			from.Location = new Point(400, 50);
			from.AutoSize = true;
			from.LostFocus += closeOnLostFocus;
			from.FormClosing += updateWeaponsOnClose;
			from.Show();
		}
		private void ShowWeaponDetails(object sender, EventArgs e)
		{
			if((e as MouseEventArgs).Button == MouseButtons.Right)
			{
				Weapon weapon = Weapon.fromYaml(name: (sender as Label).Text.Substring(0, (sender as Label).Text.IndexOf('|') - 1));
				Form from = new();
				from.Size = new Size(300, 600);
				from.AutoSize = true;
				from.Location = new Point(400, 50);
				Label WeaponProperties = new();
				from.Controls.Add(WeaponProperties);
				WeaponProperties.Location = new Point(6, 12);
				WeaponProperties.MaximumSize = new Size(276, 0);
				WeaponProperties.AutoSize = true;
				WeaponProperties.Text = Controller.CPage_GetWeaponDisplay(weapon);


				from.Show();
				from.LostFocus += (f, o) => (f as Form).Close();
			}
		}
		protected void updateACOnClose(object sender, EventArgs e)
		{
			ACNum.Text = $"{Controller.CPage_GetArmorClass(player) + player.myRace.getACBoost()}";

			var serializer = new YamlDotNet.Serialization.SerializerBuilder().Build();
			var yaml = serializer.Serialize(player.wornArmor);
			File.WriteAllText($@"./Data/Characters/{player.name}/{player.name}Armor.yaml", yaml);
		}
		protected void updateWeaponsOnClose(object sender, EventArgs e)
		{
			while(WeaponsTab.Controls.Count > 0)
            {
                foreach (Control c in WeaponsTab.Controls)
                {
                    c.Dispose();
                }
            }
            int WeaponYValues = 6;
            foreach (Weapon w in player.equippedWeapons)
            {
                GroupBox box = new();
                Label wName = new Label();
                Button aBonus = new Button();
                Button dRoll = new Button();
                WeaponsTab.Controls.Add(box);

                box.Size = new Size(284, 60);
                box.Location = new Point(0, WeaponYValues);

                box.Controls.Add(wName);
                box.Controls.Add(aBonus);
                box.Controls.Add(dRoll);

                var strings = Controller.CPage_GetWeaponButtonText(w, player);

                wName.Text = strings.Item1;
                wName.AutoSize = true;
                wName.Location = new Point(9, 12);

                wName.Click += ShowWeaponDetails;

                if (strings.Item2 >= 0) aBonus.Text = $"+{strings.Item2}";
                else aBonus.Text = $"{strings.Item2}";
                aBonus.Size = new Size(40, 23);
                aBonus.Location = new Point(box.Width - aBonus.Size.Width - 3, 9);
                aBonus.Click += AttackRoll;

                dRoll.Text = strings.Item3;
                dRoll.AutoSize = true;
                dRoll.Location = new Point(box.Width - dRoll.Size.Width - 3, 33);
                dRoll.MouseDown += DamageRoll;

                WeaponYValues += 56;
            }

            var serializer = new YamlDotNet.Serialization.SerializerBuilder().Build();
			var yaml = serializer.Serialize(player.equippedWeapons);
			File.WriteAllText($@"./Data/Characters/{player.name}/{player.name}Weapons.yaml", yaml);
		}
		protected void closeOnLostFocus(object sender, EventArgs e)
		{
			(sender as Form).Close();
		}
	}
}
