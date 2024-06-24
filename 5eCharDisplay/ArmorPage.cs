using System;
using System.Collections.Generic;
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
        internal ArmorPage(Character PC)
        {
            InitializeComponent();
            player = PC;
            List<Armor> armors = new List<Armor>();
            var armor1 = Armor.fromYaml();
        }
    }
}
