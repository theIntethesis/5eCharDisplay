using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Numerics;
using System.Net.Http;
using System.Xml.Linq;
using System.Runtime.Versioning;
using _5eCharDisplay.Classes;

namespace _5eCharDisplay
{
	[SupportedOSPlatform("windows")]
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void WriteFeats()
		{
			Feat feat = new Feat();
			feat.name = "Polearm Master";
			feat.WeaponAdd = new();
			feat.WeaponAdd.Name = "Polearm Master Attack";
			feat.WeaponAdd.DamageDie = new();
			feat.WeaponAdd.DamageDie.Add(new Die(1, 4));
			feat.WeaponAdd.DamageType = new();
			feat.WeaponAdd.DamageType.Add("Bludgeoning");
			feat.WeaponAdd.Properties = new();
			feat.WeaponAdd.Properties.Add("Versatile");
			feat.description = "When you take the Attack action and attack with only a glaive, halberd, quarterstaff, or spear, you can use a bonus action to make a melee attack with the opposite end of the weapon. This attack uses the same ability modifier as the primary attack. The weapon's damage die for this attack is a d4, and it deals bludgeoning damage.\n\nWhile you are wielding a glaive, halberd, pike, quarterstaff, or spear, other creatures provoke an opportunity attack from you when they enter the reach you have with that weapon.";
			feat.asiboosts = [0, 0, 0, 0, 0, 0];


			var serializer = new YamlDotNet.Serialization.SerializerBuilder().Build();
			var yaml = serializer.Serialize(feat);
			File.WriteAllText($@"./Data/Feats/{feat.name}.yaml", yaml);
		}
		static void MoveToNewSpellcasting()
		{
			Character Ferrous = Character.fromYAML("Arkheth Phac");
			Spellcasting newlock = new Spellcasting();
			charClass oldlock = Ferrous.myClasses.Find(c => c.classname == charClass.ClassName.Warlock);

			Ferrous.myClasses.Find(c => c.classname == charClass.ClassName.Warlock).spellcasting = newlock;
			var war = Ferrous.myClasses.Find(c => c.classname == charClass.ClassName.Warlock);
			var serializer = new YamlDotNet.Serialization.SerializerBuilder().Build();
			var yaml = serializer.Serialize(war);
			File.WriteAllText($@"./Data/Characters/{Ferrous.name}/{Ferrous.name}Warlock.yaml", yaml);
		}
		static void miscFunction()
		{
			Character Ferrous = Character.fromYAML("Ferrous Immard");

            var serializer = new YamlDotNet.Serialization.SerializerBuilder().Build();
            var yaml = serializer.Serialize(Ferrous);
            File.WriteAllText($@"./Data/Characters/{Ferrous.name}/{Ferrous.name}FullCharacter.yaml", yaml);
        }
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new CharacterSelect());
		}
	}
}
