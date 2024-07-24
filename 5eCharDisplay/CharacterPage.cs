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

            for (int i = 0; i < player.myClasses.Count; i++)
            {
                TabPage tab = new TabPage();
                ClassTabControl.Controls.Add(tab);
                tab.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                tab.Text = player.myClasses[i].name.ToString();
                tab.Size = new Size(279, 598);
                tab.Padding = new Padding(3);
                tab.AutoScroll = true;

                foreach (var g in player.myClasses[i].getInfoBoxes())
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
                Label wName = new Label();
                Button aBonus = new Button();
                Button dRoll = new Button();
                WeaponsTab.Controls.Add(wName);
                WeaponsTab.Controls.Add(aBonus);
                WeaponsTab.Controls.Add(dRoll);

                var strings = Controller.CPage_GetWeaponButtonText(w, player);

                wName.Text = strings.Item1;
                wName.AutoSize = true;
                wName.Location = new Point(9, WeaponYValues + 3);

                if (strings.Item2 >= 0) aBonus.Text = $"+{strings.Item2}";
                else aBonus.Text = $"{strings.Item2}";
                aBonus.Size = new Size(32, 23);
                aBonus.Location = new Point(250 - aBonus.Size.Width, WeaponYValues);
                aBonus.Click += AttackRoll;

                dRoll.Text = strings.Item3;
                dRoll.AutoSize = true;
                dRoll.Location = new Point(250 - dRoll.Size.Width, WeaponYValues + 24);
                dRoll.MouseDown += DamageRoll;

                WeaponYValues += 36 + dRoll.Size.Height;
            }

            SpeedNum.Text = $"{player.myRace.getSpeed()}";

            ProfsText.Text = Controller.CPage_GetProfList(player);

            int low = 8;
            foreach (charClass c in player.myClasses)
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
            Inventory.KeyDown += UpdateInventory;

            FormClosing += new FormClosingEventHandler(OnFormClose);

            if (!player.Spellcasting)
                SpellcastingToggle.Hide();

            (StrSaveProf.Checked, StrSaveNum.Text) = Controller.CPage_GetSkillProfs(player, "StrSave", Character.Stat.Strength);
            (DexSaveProf.Checked, DexSaveNum.Text) = Controller.CPage_GetSkillProfs(player, "DexSave", Character.Stat.Dexterity);
            (ConSaveProf.Checked, ConSaveNum.Text) = Controller.CPage_GetSkillProfs(player, "ConSave", Character.Stat.Constitution);
            (IntSaveProf.Checked, IntSaveNum.Text) = Controller.CPage_GetSkillProfs(player, "IntSave", Character.Stat.Intelligence);
            (WisSaveProf.Checked, WisSaveNum.Text) = Controller.CPage_GetSkillProfs(player, "WisSave", Character.Stat.Wisdom);
            (ChaSaveProf.Checked, ChaSaveNum.Text) = Controller.CPage_GetSkillProfs(player, "ChaSave", Character.Stat.Charisma);


            (AcroProf.Checked, AcroNum.Text) = Controller.CPage_GetSkillProfs(player, "Acrobatics", Character.Stat.Dexterity);
            (AHandProf.Checked, AHandNum.Text) = Controller.CPage_GetSkillProfs(player, "Animal Handling", Character.Stat.Wisdom);
            (ArcanaProf.Checked, ArcanaNum.Text) = Controller.CPage_GetSkillProfs(player, "Arcana", Character.Stat.Intelligence);
            (AthProf.Checked, AthNum.Text) = Controller.CPage_GetSkillProfs(player, "Athletics", Character.Stat.Strength);
            (DecepProf.Checked, DecepNum.Text) = Controller.CPage_GetSkillProfs(player, "Deception", Character.Stat.Charisma);
            (HistProf.Checked, HistNum.Text) = Controller.CPage_GetSkillProfs(player, "History", Character.Stat.Intelligence);
            (InsightProf.Checked, InsightNum.Text) = Controller.CPage_GetSkillProfs(player, "Insight", Character.Stat.Wisdom);
            PassInsNum.Text = Controller.CPage_GetPassives(player, "Insight", Character.Stat.Wisdom);
            (IntimProf.Checked, IntimNum.Text) = Controller.CPage_GetSkillProfs(player, "Intimidation", Character.Stat.Charisma);
            (InvestProf.Checked, InvestNum.Text) = Controller.CPage_GetSkillProfs(player, "Investigation", Character.Stat.Intelligence);
            (MedProf.Checked, MedNum.Text) = Controller.CPage_GetSkillProfs(player, "Medicine", Character.Stat.Wisdom);
            (NatProf.Checked, NatNum.Text) = Controller.CPage_GetSkillProfs(player, "Nature", Character.Stat.Intelligence);
            (PercepProf.Checked, PercepNum.Text) = Controller.CPage_GetSkillProfs(player, "Perception", Character.Stat.Wisdom);
            PassPerNum.Text = Controller.CPage_GetPassives(player, "Perception", Character.Stat.Wisdom);
            (PerfProf.Checked, PerfNum.Text) = Controller.CPage_GetSkillProfs(player, "Performance", Character.Stat.Charisma);
            (PersProf.Checked, PersNum.Text) = Controller.CPage_GetSkillProfs(player, "Persuasion", Character.Stat.Charisma);
            (RelProf.Checked, RelNum.Text) = Controller.CPage_GetSkillProfs(player, "Religion", Character.Stat.Intelligence);
            (SleProf.Checked, SleNum.Text) = Controller.CPage_GetSkillProfs(player, "Sleight of Hand", Character.Stat.Dexterity);
            (SteProf.Checked, SteNum.Text) = Controller.CPage_GetSkillProfs(player, "Stealth", Character.Stat.Dexterity);
            (SurProf.Checked, SurNum.Text) = Controller.CPage_GetSkillProfs(player, "Survival", Character.Stat.Wisdom);
            InitNum.Text = Controller.CPage_GetSkillProfs(player, "Initiative", Character.Stat.Dexterity).Item2;


            Width -= 180;

            DiceInput.KeyUp += DiceInputPressEnter;
            XPTicker.KeyUp += EXPInputPressEnter;

            PTraitLabel.Text = player.myBackground.getPTrait();
            IdealsLabel.Text = player.myBackground.getIdeal();
            FlawLabel.Text = player.myBackground.getFlaw();
            BondLabel.Text = player.myBackground.getBond();
            FeatureLabel.Text = player.myBackground.getFeature();
        }

        private void DamageRoll(object sender, EventArgs e)
        {
            Thread.Sleep(250);
            Regex pattern = new Regex(@"((\d+)d(\d+))( \+ \d+)? (\w+)");
            /*
			 * Match 0: 1d6 + 3 Piercing
			 * Group 1: 1d6
			 * Group 2: 1
			 * Group 3: 6
			 * Group 4:  + 3
			 * Group 5: Piercing
			 */
            DiceResult1.Text = "";
            MatchCollection matches = pattern.Matches((sender as Button).Text);
            for (int i = 0; i < matches.Count; i++)
            {
                if (i > 0)
                {
                    DiceResult1.Text += "\n";
                }
                Die d = new Die(int.Parse(matches[i].Groups[2].Value), int.Parse(matches[i].Groups[3].Value));
                int mod = 0;
                if (matches[i].Groups[4].Success)
                    mod = int.Parse(matches[i].Groups[4].Value.Substring(matches[i].Groups[4].Value.Length - 2));
                int roll = d.roll() + mod;
                if ((e as MouseEventArgs).Button == MouseButtons.Right)
                    // Right Click = Critical Damage
                    roll += d.roll();
                DiceResult1.Text += $"{roll} {matches[i].Groups[5].Value}";
            }
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
            DiceResult2.Text = $"{roll2 + modifier}";
            if (roll2 == 20)
                DiceResult2.Text += " (CRIT!)";
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
            int classnum = 0;
            foreach (charClass c in player.myClasses)
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
            foreach (var c in player.myClasses)
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
            foreach (var c in player.myClasses)
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
            for (int i = 0; i < player.myClasses.Count; i++)
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
