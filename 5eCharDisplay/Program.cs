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
			feat.name = "Healer";
			//feat.prerequisite = "";
			feat.description = "You are an able physician, allowing you to mend wounds quickly and get your allies back in the fight. You gain the following benefits:\n\t- When you use a healer's kit to stabilize a dying creature, that creature also regains 1 hit point.\n\t- As an action, you can spend one use of a healer's kit to tend to a creature and restore 1d6 + 4 hit points to it, plus additional hit points equal to the creature's maximum number of Hit Dice. The creature can't regain hit points from this feat again until it finishes a short or long rest.";
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
			WriteFeats();
			return;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new CharacterSelect());
		}
	}
}
