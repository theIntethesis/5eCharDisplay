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
			feat.name = "Inspiring Leader";
			feat.prerequisite = "Charisma 13 or higher";
			feat.description = "You can spend 10 minutes inspiring your companions, shoring up their resolve to fight. When you do, choose up to six friendly creatures (which can include yourself) within 30 feet of you who can see or hear you and who can understand you. Each creature can gain temporary hit points equal to your level plus your Charisma modifier. A creature can't gain temporary hit points from this feat again until it has finished a short or long rest.";
			feat.asiboosts = [0, 0, 0, 0, 0, 0];

            var serializer = new YamlDotNet.Serialization.SerializerBuilder().Build();
			var yaml = serializer.Serialize(feat);
			File.WriteAllText($@"./Data/Feats/{feat.name}.yaml", yaml);
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
