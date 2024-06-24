using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace _5eCharDisplay
{
    public partial class CharacterSelect : Form
    {

        public CharacterSelect()
        {
            int y = 100;
            foreach (string dir in Directory.GetDirectories($@".\Data\Characters"))
            {
                int index = dir.LastIndexOf('\\');
                string charName = dir.Substring(index);
                Label lbl = new Label();
                Font CharFont = new Font(FontFamily.GenericSansSerif, 10);
                lbl.Font = new Font(FontFamily.GenericSansSerif, 10);
                lbl.Location = new Point(100, y += 25);
                lbl.Width = 800;
                lbl.Click += lbl_Click;

                Character chara = Character.fromYAML(charName);

                lbl.Text = $"{chara.name}: {chara.getRace()} ";
                for (int i = 0; i < chara.charClass.Count - 1; i++)
                {
                    lbl.Text += $"{chara.charClass[i]} {chara.level[i]}, ";
                }
                lbl.Text += $"{chara.charClass[chara.charClass.Count - 1]} {chara.level[chara.charClass.Count - 1]}";


                Controls.Add(lbl);
            }

            InitializeComponent();
        }
        private void showthis(object sender, EventArgs e) { Show(); }
        private void lbl_Click(object sender, EventArgs e)
        {
            Label label = sender as Label;
            string substring = label.Text;
            int location = substring.IndexOf(':');
            Hide();
            CharacterPage CharPage = new CharacterPage(substring.Substring(0, location));
            CharPage.Show();
            CharPage.FormClosed += showthis;
        }
    }
}
