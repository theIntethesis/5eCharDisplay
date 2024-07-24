using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace _5eCharDisplay
{
    internal class charRace
    {
        protected int speed;
        protected List<string> abilities = new List<string>();
        protected List<string> skillProfs = new List<string>();
        protected List<string> languages = new List<string>();
        protected List<string> weaponProfs = new List<string>();
        protected List<string> armorProfs = new List<string>();
        protected List<string> toolProfs = new List<string>();
        protected int hpBoost = 0;
        protected int StrBoost = 0;
        protected int DexBoost = 0;
        protected int ConBoost = 0;
        protected int IntBoost = 0;
        protected int WisBoost = 0;
        protected int ChaBoost = 0;
        protected int ACBoost = 0;
        public bool Spellcasting = false;
        public int getStrBoost() { return StrBoost; }
        public int getDexBoost() { return DexBoost; }
        public int getConBoost() { return ConBoost; }
        public int getIntBoost() { return IntBoost; }
        public int getWisBoost() { return WisBoost; }
        public int getChaBoost() { return ChaBoost; }
        public int getSpeed() { return speed; }
        public int getACBoost() { return ACBoost; }
        public int getHPBoost() { return hpBoost; }
        public List<string> getLanguages() { return languages; }
        public List<string> getAbilities() { return abilities; }
        public List<GroupBox> getAbilityBoxes()
        {
            var boxes = new List<GroupBox>();
            for (int i = 0; i < abilities.Count; i += 2)
            {
                GroupBox box = new GroupBox();
                box.Text = $"{abilities[i]}";
                Label label = new Label();
                label.Text = $"{abilities[i+1]}";
                label.MaximumSize = new Size(168, int.MaxValue);
                label.AutoSize = true;
                box.Controls.Add(label);
                label.Location = new Point(6, 12);
                box.MaximumSize = new Size(180, int.MaxValue);
                box.AutoSize = true;
                label.MouseDown += DisplayOnRightClick;
                boxes.Add(box);
            }
            
            return boxes;
        }

        protected void DisplayOnRightClick(object sender, EventArgs e)
        {
            Label label = sender as Label;
            MouseEventArgs mouse = e as MouseEventArgs;
            if (mouse.Button == MouseButtons.Right)
            {
                Form from = new Form();
                //from.Icon = new Icon(@"C:\Users\Hayden\Downloads\881288450786082876.ico");
                from.Location = new Point(400, 50);
                Label label1 = new Label();
                label1.Location = new Point(6, 6);
                label1.AutoSize = true;
                label1.MaximumSize = new Size(175, 575);
                from.Controls.Add(label1);
                label1.Text = label.Text;
                from.Size = new Size(200, label1.Height + 50);
                from.LostFocus += closeOnLostFocus;
                from.Show();
            }
        }

        private void closeOnLostFocus(object sender, EventArgs e)
        {
            Form form = sender as Form;
            form.Close();
        }
        public List<string> getSkillProfs() { return skillProfs; }
        public List<string> getWeaponProfs() { return weaponProfs; }
        public List<string> getToolProfs() { return toolProfs; }
        public List<string> getArmorProfs() { return armorProfs; }

    }
}
