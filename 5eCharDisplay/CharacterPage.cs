using System;
using System.Threading;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace _5eCharDisplay
{
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

			int ylocation = 18;

			Label label = new Label();
			label.Location = new Point(6, 0);
			label.Text = "===== Class Features =====";
			FnTPanel.Controls.Add(label);
			label.AutoSize = true;
			foreach(var c in player.myClasses)
			{
				foreach (GroupBox g in c.getInfoBoxes())
				{
					FnTPanel.Controls.Add(g);
					g.Location = new Point(6, ylocation);
					ylocation += g.Height + 12;
				}
			}
			FnTText.Text = "";
			FnTText.Location = new Point(6, ylocation);
			
			Label label2 = new Label();
			label2.Location = new Point(6, ylocation);
			ylocation += 18;
			label2.Text = "===== Racial Features =====";
			FnTPanel.Controls.Add(label2);
			label2.AutoSize = true;
			foreach (GroupBox g in player.myRace.getAbilityBoxes())
			{
				FnTPanel.Controls.Add(g);
				g.Location = new Point(6, ylocation);
				ylocation += g.Height + 12;
			}
			FnTText.Text = "";
			FnTText.Location = new Point(6, ylocation);

			Armor chestArmor = null;
			Armor shield = null;
			foreach (Armor a in player.wornArmor)
			{
				if (a.aType != Armor.ArmorType.Shield && chestArmor == null)
				{
					chestArmor = a;
				}
				else if (shield == null)
				{
					shield = a;
				}
			}
			int AC = 10;
			if (chestArmor != null)
			{
				int dexMax = chestArmor.DexMax;
				if (dexMax == -1)
				{
					dexMax = 10;
				}
				AC = int.Parse(chestArmor.AC);
				if (chestArmor.aType != Armor.ArmorType.Heavy)
					AC += Math.Min(player.dexterity.getMod(), dexMax);
			}
			else
				AC += player.dexterity.getMod();
			if (shield != null)
			{
				AC += int.Parse(shield.AC.Substring(1));
			}

			ACNum.Text = $"{AC + player.myRace.getACBoost()}";

			if (chestArmor != null && chestArmor.stealthDis)
				SteLabel.Text = "Stealth [D]";
			else
				SteLabel.Text = "Stealth";

			SpeedNum.Text = $"{player.myRace.getSpeed()}";

			ProfsText.Text = "";

			for(int i = 0; i < player.languages.Count() - 1; i++)
			{
				ProfsText.Text += player.languages.ElementAt(i) + ", ";
			}
			if(player.languages.Count() != 0)
				ProfsText.Text += player.languages.ElementAt(player.languages.Count() - 1) + "\n\n";

			for(int i = 0; i < player.armorProfs.Count() - 1; i++)
			{
				ProfsText.Text += player.armorProfs.ElementAt(i) + ", ";
			}
			if(player.armorProfs.Count() != 0)
				ProfsText.Text += player.armorProfs.ElementAt(player.armorProfs.Count() - 1) + "\n\n";

			for (int i = 0; i < player.weaponProfs.Count() - 1; i++)
			{
				ProfsText.Text += player.weaponProfs.ElementAt(i) + ", ";
			}

			if (player.weaponProfs.Count() != 0)
				ProfsText.Text += player.weaponProfs.ElementAt(player.weaponProfs.Count() - 1) + "\n\n";

			for (int i = 0; i < player.toolProf.Count() - 1; i++)
			{
				ProfsText.Text += player.toolProf.ElementAt(i);
				if (player.expertise.Contains(player.toolProf.ElementAt(i)))
					ProfsText.Text += " [E]";
				ProfsText.Text += ", ";
			}
			if (player.toolProf.Count() != 0)
			{
				ProfsText.Text += player.toolProf.ElementAt(player.toolProf.Count() - 1);
				if (player.expertise.Contains(player.toolProf.ElementAt(player.toolProf.Count() - 1)))
					ProfsText.Text += "[E]";
			}
			ProfsText.Text += "\n\n";

			int low = 8;
			foreach(charClass c in player.myClasses)
			{
				Label l = new Label();
				l.Text = $"{c.getRemHD()}d{c.getHitDie().getSides()}";
				l.Location = new Point(11, low);
				l.AutoSize = true;
				l.Font = new Font(FontFamily.GenericSansSerif, 12);
				l.Click += HitDieLabel_Click;
				HDPanel.Controls.Add(l);
				low += 18;
			}

			Inventory.Lines = player.inventory.ToArray();

			FormClosing += new FormClosingEventHandler(OnFormClose);

			if (!player.Spellcasting)
				SpellcastingToggle.Hide();

			#region Saving Throws

			if (player.expertise.Contains("StrSave"))
			{
				StrSaveProf.Checked = true;
				StrSaveNum.Text = $"+{2 * player.proficiency + player.strength.getMod()}";
			}
			else if (player.skillProf.Contains("StrSave"))
			{
				StrSaveProf.Checked = true;
				StrSaveNum.Text = $"+{player.proficiency + player.strength.getMod()}";
			}
			else
			{
				if(player.strength.getMod() == 0)
				{
					StrSaveNum.Text = "0";
				}
				else if(player.strength.getMod() > 0)
				{
					StrSaveNum.Text = $"+{player.strength.getMod()}";
				}
				else
				{
					StrSaveNum.Text = $"{player.strength.getMod()}";
				}
			}

			if (player.expertise.Contains("DexSave"))
			{
				DexSaveProf.Checked = true;
				DexSaveNum.Text = $"+{2 * player.proficiency + player.dexterity.getMod()}";
			}
			else if (player.skillProf.Contains("DexSave"))
			{
				DexSaveProf.Checked = true;
				DexSaveNum.Text = $"+{player.proficiency + player.dexterity.getMod()}";
			}
			else
			{
				if (player.dexterity.getMod() == 0)
				{
					DexSaveNum.Text = "0";
				}
				else if (player.dexterity.getMod() > 0)
				{
					DexSaveNum.Text = $"+{player.dexterity.getMod()}";
				}
				else
				{
					DexSaveNum.Text = $"{player.dexterity.getMod()}";
				}
			}

			if (player.expertise.Contains("ConSave"))
			{
				ConSaveProf.Checked = true;
				ConSaveNum.Text = $"+{2 * player.proficiency + player.constitution.getMod()}";
			}
			else if (player.skillProf.Contains("ConSave"))
			{
				ConSaveProf.Checked = true;
				ConSaveNum.Text = $"+{player.proficiency + player.strength.getMod()}";
			}
			else
			{
				if (player.constitution.getMod() == 0)
				{
					ConSaveNum.Text = "0";
				}
				else if (player.constitution.getMod() > 0)
				{
					ConSaveNum.Text = $"+{player.constitution.getMod()}";
				}
				else
				{
					ConSaveNum.Text = $"{player.constitution.getMod()}";
				}
			}


			if (player.expertise.Contains("IntSave"))
			{
				IntSaveProf.Checked = true;
				IntSaveNum.Text = $"+{2 * player.proficiency + player.intelligence.getMod()}";
			}
			else if (player.skillProf.Contains("IntSave"))
			{
				IntSaveProf.Checked = true;
				IntSaveNum.Text = $"+{player.proficiency + player.intelligence.getMod()}";
			}
			else
			{
				if (player.intelligence.getMod() == 0)
				{
					IntSaveNum.Text = "0";
				}
				else if (player.intelligence.getMod() > 0)
				{
					IntSaveNum.Text = $"+{player.intelligence.getMod()}";
				}
				else
				{
					IntSaveNum.Text = $"{player.intelligence.getMod()}";
				}
			}


			if (player.expertise.Contains("WisSave"))
			{
				WisSaveProf.Checked = true;
				WisSaveNum.Text = $"+{2 * player.proficiency + player.wisdom.getMod()}";
			}
			else if (player.skillProf.Contains("WisSave"))
			{
				WisSaveProf.Checked = true;
				WisSaveNum.Text = $"+{player.proficiency + player.strength.getMod()}";
			}
			else
			{
				if (player.wisdom.getMod() == 0)
				{
					WisSaveNum.Text = "0";
				}
				else if (player.wisdom.getMod() > 0)
				{
					WisSaveNum.Text = $"+{player.wisdom.getMod()}";
				}
				else
				{
					WisSaveNum.Text = $"{player.wisdom.getMod()}";
				}
			}


			if (player.expertise.Contains("ChaSave"))
			{
				ChaSaveProf.Checked = true;
				ChaSaveNum.Text = $"+{2 * player.proficiency + player.charisma.getMod()}";
			}
			else if (player.skillProf.Contains("ChaSave"))
			{
				ChaSaveProf.Checked = true;
				ChaSaveNum.Text = $"+{player.proficiency + player.strength.getMod()}";
			}
			else
			{
				if (player.charisma.getMod() == 0)
				{
					ChaSaveNum.Text = "0";
				}
				else if (player.charisma.getMod() > 0)
				{
					ChaSaveNum.Text = $"+{player.charisma.getMod()}";
				}
				else
				{
					ChaSaveNum.Text = $"{player.charisma.getMod()}";
				}
			}
			#endregion Saving Throws

			#region Skill Checks
			if (player.expertise.Contains("Acrobatics"))
			{
				AcroProf.Checked = true;
				AcroNum.Text = $"+{2 * player.proficiency + player.dexterity.getMod()}";
			}
			else if (player.skillProf.Contains("Acrobatics"))
			{
				AcroProf.Checked = true;
				AcroNum.Text = $"+{player.proficiency + player.dexterity.getMod()}";
			}
			else
			{
				if (player.dexterity.getMod() == 0)
				{
					AcroNum.Text = "0";
				}
				else if (player.dexterity.getMod() > 0)
				{
					AcroNum.Text = $"+{player.dexterity.getMod()}";
				}
				else
				{
					AcroNum.Text = $"{player.dexterity.getMod()}";
				}
			}

			if (player.expertise.Contains("Animal Handling"))
			{
				AHandProf.Checked = true;
				AHandNum.Text = $"+{2 * player.proficiency + player.wisdom.getMod()}";
			}
			else if (player.skillProf.Contains("Animal Handling"))
			{
				AHandProf.Checked = true;
				AHandNum.Text = $"+{player.proficiency + player.wisdom.getMod()}";
			}
			else
			{
				if (player.wisdom.getMod() == 0)
				{
					AHandNum.Text = "0";
				}
				else if (player.wisdom.getMod() > 0)
				{
					AHandNum.Text = $"+{player.wisdom.getMod()}";
				}
				else
				{
					AHandNum.Text = $"{player.wisdom.getMod()}";
				}
			}

			if (player.expertise.Contains("Animal Handling"))
			{
				AHandProf.Checked = true;
				AHandNum.Text = $"+{2 * player.proficiency + player.wisdom.getMod()}";
			}
			else if (player.skillProf.Contains("Animal Handling"))
			{
				AHandProf.Checked = true;
				AHandNum.Text = $"+{player.proficiency + player.wisdom.getMod()}";
			}
			else
			{
				if (player.wisdom.getMod() == 0)
				{
					AHandNum.Text = "0";
				}
				else if (player.wisdom.getMod() > 0)
				{
					AHandNum.Text = $"+{player.wisdom.getMod()}";
				}
				else
				{
					AHandNum.Text = $"{player.wisdom.getMod()}";
				}
			}

			if (player.expertise.Contains("Arcana"))
			{
				ArcanaProf.Checked = true;
				ArcanaNum.Text = $"+{2 * player.proficiency + player.intelligence.getMod()}";
			}
			else if (player.skillProf.Contains("Arcana"))
			{
				ArcanaProf.Checked = true;
				ArcanaNum.Text = $"+{player.proficiency + player.intelligence.getMod()}";
			}
			else
			{
				if (player.intelligence.getMod() == 0)
				{
					ArcanaNum.Text = "0";
				}
				else if (player.intelligence.getMod() > 0)
				{
					ArcanaNum.Text = $"+{player.intelligence.getMod()}";
				}
				else
				{
					ArcanaNum.Text = $"{player.intelligence.getMod()}";
				}
			}

			if (player.expertise.Contains("Athletics"))
			{
				AthProf.Checked = true;
				AthNum.Text = $"+{2 * player.proficiency + player.strength.getMod()}";
			}
			else if (player.skillProf.Contains("Athletics"))
			{
				AthProf.Checked = true;
				AthNum.Text = $"+{player.proficiency + player.strength.getMod()}";
			}
			else
			{
				if (player.strength.getMod() == 0)
				{
					AthNum.Text = "0";
				}
				else if (player.strength.getMod() > 0)
				{
					AthNum.Text = $"+{player.strength.getMod()}";
				}
				else
				{
					AthNum.Text = $"{player.strength.getMod()}";
				}
			}

			if (player.expertise.Contains("Deception"))
			{
				DecepProf.Checked = true;
				DecepNum.Text = $"+{2 * player.proficiency + player.charisma.getMod()}";
			}
			else if (player.skillProf.Contains("Deception"))
			{
				DecepProf.Checked = true;
				DecepNum.Text = $"+{player.proficiency + player.charisma.getMod()}";
			}
			else
			{
				if (player.charisma.getMod() == 0)
				{
					DecepNum.Text = "0";
				}
				else if (player.charisma.getMod() > 0)
				{
					DecepNum.Text = $"+{player.charisma.getMod()}";
				}
				else
				{
					DecepNum.Text = $"{player.charisma.getMod()}";
				}
			}

			if (player.expertise.Contains("History"))
			{
				HistProf.Checked = true;
				HistNum.Text = $"+{2 * player.proficiency + player.intelligence.getMod()}";
			}
			else if (player.skillProf.Contains("History"))
			{
				HistProf.Checked = true;
				HistNum.Text = $"+{player.proficiency + player.intelligence.getMod()}";
			}
			else
			{
				if (player.intelligence.getMod() == 0)
				{
					HistNum.Text = "0";
				}
				else if (player.intelligence.getMod() > 0)
				{
					HistNum.Text = $"+{player.intelligence.getMod()}";
				}
				else
				{
					HistNum.Text = $"{player.intelligence.getMod()}";
				}
			}

			if (player.expertise.Contains("History"))
			{
				HistProf.Checked = true;
				HistNum.Text = $"+{2 * player.proficiency + player.intelligence.getMod()}";
			}
			else if (player.skillProf.Contains("History"))
			{
				HistProf.Checked = true;
				HistNum.Text = $"+{player.proficiency + player.intelligence.getMod()}";
			}
			else
			{
				if (player.intelligence.getMod() == 0)
				{
					HistNum.Text = "0";
				}
				else if (player.intelligence.getMod() > 0)
				{
					HistNum.Text = $"+{player.intelligence.getMod()}";
				}
				else
				{
					HistNum.Text = $"{player.intelligence.getMod()}";
				}
			}

			if (player.expertise.Contains("Insight"))
			{
				InsightProf.Checked = true;
				InsightNum.Text = $"+{2 * player.proficiency + player.wisdom.getMod()}";
				PassInsNum.Text = $"{10 + 2 * player.proficiency + player.wisdom.getMod()}";
			}
			else if (player.skillProf.Contains("Insight"))
			{
				InsightProf.Checked = true;
				InsightNum.Text = $"+{player.proficiency + player.wisdom.getMod()}";
				PassInsNum.Text = $"{10 + player.proficiency + player.wisdom.getMod()}";
			}
			else
			{
				if (player.wisdom.getMod() == 0)
				{
					InsightNum.Text = "0";
					PassInsNum.Text = $"10";
				}
				else if (player.wisdom.getMod() > 0)
				{
					InsightNum.Text = $"+{player.wisdom.getMod()}";
					PassInsNum.Text = $"{10 + player.wisdom.getMod()}";
				}
				else
				{
					InsightNum.Text = $"{player.wisdom.getMod()}";
					PassInsNum.Text = $"{10 + player.wisdom.getMod()}";
				}
			}

			if (player.expertise.Contains("Intimidation"))
			{
				IntimProf.Checked = true;
				IntimNum.Text = $"+{2 * player.proficiency + player.charisma.getMod()}";
			}
			else if (player.skillProf.Contains("Intimidation"))
			{
				IntimProf.Checked = true;
				IntimNum.Text = $"+{player.proficiency + player.charisma.getMod()}";
			}
			else
			{
				if (player.charisma.getMod() == 0)
				{
					IntimNum.Text = "0";
				}
				else if (player.charisma.getMod() > 0)
				{
					IntimNum.Text = $"+{player.charisma.getMod()}";
				}
				else
				{
					IntimNum.Text = $"{player.charisma.getMod()}";
				}
			}

			if (player.expertise.Contains("Investigation"))
			{
				InvestProf.Checked = true;
				InvestNum.Text = $"+{2 * player.proficiency + player.intelligence.getMod()}";
			}
			else if (player.skillProf.Contains("Investigation"))
			{
				InvestProf.Checked = true;
				InvestNum.Text = $"+{player.proficiency + player.intelligence.getMod()}";
			}
			else
			{
				if (player.intelligence.getMod() == 0)
				{
					InvestNum.Text = "0";
				}
				else if (player.intelligence.getMod() > 0)
				{
					InvestNum.Text = $"+{player.intelligence.getMod()}";
				}
				else
				{
					InvestNum.Text = $"{player.intelligence.getMod()}";
				}
			}

			if (player.expertise.Contains("Medicine"))
			{
				MedProf.Checked = true;
				MedNum.Text = $"+{2 * player.proficiency + player.wisdom.getMod()}";
			}
			else if (player.skillProf.Contains("Medicine"))
			{
				MedProf.Checked = true;
				MedNum.Text = $"+{player.proficiency + player.wisdom.getMod()}";
			}
			else
			{
				if (player.wisdom.getMod() == 0)
				{
					MedNum.Text = "0";
				}
				else if (player.wisdom.getMod() > 0)
				{
					MedNum.Text = $"+{player.wisdom.getMod()}";
				}
				else
				{
					MedNum.Text = $"{player.wisdom.getMod()}";
				}
			}

			if (player.expertise.Contains("Nature"))
			{
				NatProf.Checked = true;
				NatNum.Text = $"+{2 * player.proficiency + player.intelligence.getMod()}";
			}
			else if (player.skillProf.Contains("Nature"))
			{
				NatProf.Checked = true;
				NatNum.Text = $"+{player.proficiency + player.intelligence.getMod()}";
			}
			else
			{
				if (player.intelligence.getMod() == 0)
				{
					NatNum.Text = "0";
				}
				else if (player.intelligence.getMod() > 0)
				{
					NatNum.Text = $"+{player.intelligence.getMod()}";
				}
				else
				{
					NatNum.Text = $"{player.intelligence.getMod()}";
				}
			}

			if (player.expertise.Contains("Perception"))
			{
				PercepProf.Checked = true;
				PercepNum.Text = $"+{2 * player.proficiency + player.wisdom.getMod()}";
				PassPerNum.Text = $"{10 + 2 * player.proficiency + player.wisdom.getMod()}";
			}
			else if (player.skillProf.Contains("Perception"))
			{
				PercepProf.Checked = true;
				PercepNum.Text = $"+{player.proficiency + player.wisdom.getMod()}";
				PassPerNum.Text = $"{10 + player.proficiency + player.wisdom.getMod()}";
			}
			else
			{
				if (player.wisdom.getMod() == 0)
				{
					PercepNum.Text = "0";
					PassPerNum.Text = $"10";
				}
				else if (player.wisdom.getMod() > 0)
				{
					PercepNum.Text = $"+{player.wisdom.getMod()}";
					PassPerNum.Text = $"{10 + player.wisdom.getMod()}";
				}
				else
				{
					PercepNum.Text = $"{player.wisdom.getMod()}";
					PassPerNum.Text = $"{10 + player.wisdom.getMod()}";
				}
			}

			if (player.expertise.Contains("Performance"))
			{
				PerfProf.Checked = true;
				PerfNum.Text = $"+{2 * player.proficiency + player.charisma.getMod()}";
			}
			else if (player.skillProf.Contains("Performance"))
			{
				PerfProf.Checked = true;
				PerfNum.Text = $"+{player.proficiency + player.charisma.getMod()}";
			}
			else
			{
				if (player.charisma.getMod() == 0)
				{
					PerfNum.Text = "0";
				}
				else if (player.charisma.getMod() > 0)
				{
					PerfNum.Text = $"+{player.charisma.getMod()}";
				}
				else
				{
					PerfNum.Text = $"{player.charisma.getMod()}";
				}
			}

			if (player.expertise.Contains("Persuasion"))
			{
				PersProf.Checked = true;
				PersNum.Text = $"+{2 * player.proficiency + player.charisma.getMod()}";
			}
			else if (player.skillProf.Contains("Persuasion"))
			{
				PersProf.Checked = true;
				PersNum.Text = $"+{player.proficiency + player.charisma.getMod()}";
			}
			else
			{
				if (player.charisma.getMod() == 0)
				{
					PersNum.Text = "0";
				}
				else if (player.charisma.getMod() > 0)
				{
					PersNum.Text = $"+{player.charisma.getMod()}";
				}
				else
				{
					PersNum.Text = $"{player.charisma.getMod()}";
				}
			}

			if (player.expertise.Contains("Religion"))
			{
				RelProf.Checked = true;
				RelNum.Text = $"+{2 * player.proficiency + player.intelligence.getMod()}";
			}
			else if (player.skillProf.Contains("Religion"))
			{
				RelProf.Checked = true;
				RelNum.Text = $"+{player.proficiency + player.intelligence.getMod()}";
			}
			else
			{
				if (player.intelligence.getMod() == 0)
				{
					RelNum.Text = "0";
				}
				else if (player.intelligence.getMod() > 0)
				{
					RelNum.Text = $"+{player.intelligence.getMod()}";
				}
				else
				{
					RelNum.Text = $"{player.intelligence.getMod()}";
				}
			}

			if (player.expertise.Contains("Sleight of Hand"))
			{
				SleProf.Checked = true;
				SleNum.Text = $"+{2 * player.proficiency + player.dexterity.getMod()}";
			}
			else if (player.skillProf.Contains("Sleight of Hand"))
			{
				SleProf.Checked = true;
				SleNum.Text = $"+{player.proficiency + player.dexterity.getMod()}";
			}
			else
			{
				if (player.dexterity.getMod() == 0)
				{
					SleNum.Text = "0";
				}
				else if (player.dexterity.getMod() > 0)
				{
					SleNum.Text = $"+{player.dexterity.getMod()}";
				}
				else
				{
					SleNum.Text = $"{player.dexterity.getMod()}";
				}
			}

			if (player.expertise.Contains("Stealth"))
			{
				SteProf.Checked = true;
				SteNum.Text = $"+{2 * player.proficiency + player.dexterity.getMod()}";
			}
			else if (player.skillProf.Contains("Stealth"))
			{
				SteProf.Checked = true;
				SteNum.Text = $"+{player.proficiency + player.dexterity.getMod()}";
			}
			else
			{
				if (player.dexterity.getMod() == 0)
				{
					SteNum.Text = "0";
				}
				else if (player.dexterity.getMod() > 0)
				{
					SteNum.Text = $"+{player.dexterity.getMod()}";
				}
				else
				{
					SteNum.Text = $"{player.dexterity.getMod()}";
				}
			}

			if (player.expertise.Contains("Survival"))
			{
				SurProf.Checked = true;
				SurNum.Text = $"+{2 * player.proficiency + player.wisdom.getMod()}";
			}
			else if (player.skillProf.Contains("Survival"))
			{
				SurProf.Checked = true;
				SurNum.Text = $"+{player.proficiency + player.wisdom.getMod()}";
			}
			else
			{
				if (player.wisdom.getMod() == 0)
				{
					SurNum.Text = "0";
				}
				else if (player.wisdom.getMod() > 0)
				{
					SurNum.Text = $"+{player.wisdom.getMod()}";
				}
				else
				{
					SurNum.Text = $"{player.wisdom.getMod()}";
				}
			}

			if (player.expertise.Contains("Initiative"))
			{
				InitNum.Text = $"+{2 * player.proficiency + player.dexterity.getMod()}";
			}
			else if (player.skillProf.Contains("Initiative"))
			{
				InitNum.Text = $"+{player.proficiency + player.dexterity.getMod()}";
			}
			else
			{
				if (player.dexterity.getMod() == 0)
				{
					InitNum.Text = "0";
				}
				else
					InitNum.Text = player.dexterity.ToString();
			}
			#endregion Skill Checks

			Width -= 180;

			DiceInput.KeyUp += DiceInputPressEnter;
			XPTicker.KeyUp += EXPInputPressEnter;

			PTraitLabel.Text = player.myBackground.getPTrait();
			IdealsLabel.Text = player.myBackground.getIdeal();
			FlawLabel.Text = player.myBackground.getFlaw();
			BondLabel.Text = player.myBackground.getBond();
			FeatureLabel.Text = player.myBackground.getFeature();
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

		#region base ability checks
		private void StrMod_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			DiceResult1.Text = $"{d20.roll() + player.strength.getMod()}";
			DiceResult2.Text = $"{d20.roll() + player.strength.getMod()}";
		}

		private void DexMod_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			DiceResult1.Text = $"{d20.roll() + player.dexterity.getMod()}";
			DiceResult2.Text = $"{d20.roll() + player.dexterity.getMod()}";
		}

		private void ConMod_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			DiceResult1.Text = $"{d20.roll() + player.constitution.getMod()}";
			DiceResult2.Text = $"{d20.roll() + player.constitution.getMod()}";
		}

		private void IntMod_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			DiceResult1.Text = $"{d20.roll() + player.intelligence.getMod()}";
			DiceResult2.Text = $"{d20.roll() + player.intelligence.getMod()}";
		}

		private void WisMod_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			DiceResult1.Text = $"{d20.roll() + player.wisdom.getMod()}";
			DiceResult2.Text = $"{d20.roll() + player.wisdom.getMod()}";
		}

		private void ChaMod_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			DiceResult1.Text = $"{d20.roll() + player.charisma.getMod()}";
			DiceResult2.Text = $"{d20.roll() + player.charisma.getMod()}";
		}
		#endregion base ability checks

		#region Save Checks
		private void StrSaveNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.strength.getMod();
			if (player.expertise.Contains("StrSave"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("StrSave"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}

		private void DexSaveNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.dexterity.getMod();
			if (player.expertise.Contains("DexSave"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("DexSave"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}

		private void ConSaveNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.constitution.getMod();
			if (player.expertise.Contains("ConSave"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("ConSave"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}

		private void IntSaveNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.intelligence.getMod();
			if (player.expertise.Contains("IntSave"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("IntSave"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}

		private void WisSaveNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.wisdom.getMod();
			if (player.expertise.Contains("WisSave"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("WisSave"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}

		private void ChaSaveNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.charisma.getMod();
			if (player.expertise.Contains("ChaSave"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("ChaSave"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}
		#endregion Save Checks

		#region Ability Rolls
		private void AcroNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.dexterity.getMod();
			if (player.expertise.Contains("Acrobatics"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("Acrobatics"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}

		private void AHandNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.wisdom.getMod();
			if (player.expertise.Contains("Animal Handling"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("Animal Handling"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}

		private void ArcanaNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.intelligence.getMod();
			if (player.expertise.Contains("Arcana"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("Arcana"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}

		private void AthNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.strength.getMod();
			if (player.expertise.Contains("Athletics"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("Athletics"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}

		private void DecepNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.charisma.getMod();
			if (player.expertise.Contains("Deception"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("Deception"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}

		private void HistNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.intelligence.getMod();
			if (player.expertise.Contains("History"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("History"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}

		private void InsightNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.wisdom.getMod();
			if (player.expertise.Contains("Insight"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("Insight"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}

		private void IntimNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.charisma.getMod();
			if (player.expertise.Contains("Intimidation"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("Intimidation"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}

		private void InvestNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.intelligence.getMod();
			if (player.expertise.Contains("Investigation"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("Investigation"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}

		private void MedNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.wisdom.getMod();
			if (player.expertise.Contains("Medicine"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("Medicine"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}

		private void NatNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.intelligence.getMod();
			if (player.expertise.Contains("Nature"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("Nature"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}

		private void PercepNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.wisdom.getMod();
			if (player.expertise.Contains("Perception"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("Perception"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}

		private void PerfNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.charisma.getMod();
			if (player.expertise.Contains("Performance"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("Performance"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}

		private void PersNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.charisma.getMod();
			if (player.expertise.Contains("Persuasion"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("Persuasion"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}

		private void RelNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.intelligence.getMod();
			if (player.expertise.Contains("Religion"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("Religion"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}

		private void SleNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.dexterity.getMod();
			if (player.expertise.Contains("Sleight of Hand"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("Sleight of Hand"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}

		private void SteNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.dexterity.getMod();
			if (player.expertise.Contains("Stealth"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("Stealth"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}

		private void SurNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			int modifier = player.wisdom.getMod();
			if (player.expertise.Contains("Survival"))
			{
				modifier += 2 * player.proficiency;
			}
			else if (player.skillProf.Contains("Survival"))
			{
				modifier += player.proficiency;
			}
			DiceResult1.Text = $"{d20.roll() + modifier}";
			DiceResult2.Text = $"{d20.roll() + modifier}";
		}
		#endregion Ability Rolls

		private void OnFormClose(object sender, CancelEventArgs e)
		{
			SaveInventory();
			SaveClassFeatures();
			SaveCharFeatures();
		}

		private void SaveInventory()
		{
			File.WriteAllLines($@".\Data\Characters\{player.name}\{player.name}Inventory.txt", Inventory.Lines);
			return;
		}
		private void SaveClassFeatures()
		{
			int classnum = 0;
			foreach(charClass c in player.myClasses)
			{
				string[] classInfo = c.getClassDetails(player.name);
				File.WriteAllLines($@".\Data\Characters\{player.name}\{player.name}{player.charClass[classnum]}.yaml", classInfo);
				classnum++;
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

		private void InitNum_Click(object sender, EventArgs e)
		{
			Thread.Sleep(250);
			DiceResult1.Text = $"{d20.roll() + player.dexterity.getMod()}";
			DiceResult2.Text = $"{d20.roll() + player.dexterity.getMod()}";
		}

		private void HitDieLabel_Click(object sender, EventArgs e)
		{
			var labelText = sender as Label;
			string dieText = labelText.Text;
			int indexofd = dieText.IndexOf('d');
			int sides = Convert.ToInt32(dieText.Substring(indexofd + 1));
			Die die = new Die(sides);
			foreach(var c in player.myClasses)
			{
				if(c.getHitDie().Equals(die))
				{
					int rem = c.affectHD(-1);
					int healing = die.roll();
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
			foreach(var c in player.myClasses)
				c.longRest(player.name);
			int i = 0;
			foreach (Label l in HDPanel.Controls)
			{
				l.Text = $"{player.myClasses[i].getRemHD()}d{player.myClasses[i].getHitDie().getSides()}";
				i++;
			}
		}

		private void SRButton_Click(object sender, EventArgs e)
		{
			int i = 0;
			foreach(var c in player.myClasses)
			{
				c.shortRest(player.name, ++i);
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
			if(ParseMe != null && ParseMe != "")
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
			for(int i = 0; i < player.myClasses.Count; i++)
			{
				if (player.myClasses[i].Spellcasting)
				{
					SpellcastingPage spellcastingPage = new SpellcastingPage(player, i);
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
		protected void updateACOnClose(object sender, EventArgs e)
		{
			Armor chestArmor = null;
			Armor shield = null;
			foreach (Armor a in player.wornArmor)
			{
				if (a.aType != Armor.ArmorType.Shield && chestArmor == null)
				{
					chestArmor = a;
				}
				else if(shield == null)
                {
					shield = a;
                }
			}
			int AC = 10;
			if (chestArmor != null)
			{
				int dexMax = chestArmor.DexMax;
				if (dexMax == -1)
				{
					dexMax = 10;
				}
				AC = int.Parse(chestArmor.AC);
				if(chestArmor.aType != Armor.ArmorType.Heavy)
					AC += Math.Min(player.dexterity.getMod(), dexMax);
			}
			else
				AC += player.dexterity.getMod();
			if (shield != null)
			{
				AC += int.Parse(shield.AC.Substring(1));
			}

			ACNum.Text = $"{AC + player.myRace.getACBoost()}";

			var serializer = new YamlDotNet.Serialization.SerializerBuilder().Build();
			var yaml = serializer.Serialize(player.wornArmor);
			File.WriteAllText($@"./Data/Characters/{player.name}/{player.name}Armor.yaml", yaml);

		}
		protected void closeOnLostFocus(object sender, EventArgs e)
		{
			Form form = sender as Form;
			form.Close();
		}
	}
}
