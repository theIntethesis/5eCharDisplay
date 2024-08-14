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
			feat.name = "Observant";
			feat.description = "Increase your Intelligence or Wisdom score by 1, to a maximum of 20.\n\nIf you can see a creature's mouth while it is speaking a language you understand, you can interpret what it's saying by reading its lips.\n\nYou have a +5 bonus to your passive Wisdom (Perception) and passive Intelligence (Investigation) scores.";
			feat.asiboosts = [0, 0, 0, 0, 0, 0];
			feat.SkillBonus = new();
			var i = ("Perception", 5);
			feat.SkillBonus.Add(i);
			i = ("Investigation", 5);
            feat.SkillBonus.Add(i);

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
