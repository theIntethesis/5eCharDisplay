﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using YamlDotNet.Serialization;
using System.Windows.Forms;
using System.Drawing;

namespace _5eCharDisplay.Classes
{
	internal class Warlock : charClass
	{
		public string Skill1 { get; set; }
		public string Skill2 { get; set; }
		public string OtherworldlyPatron { set; get; }
		public string PactBoon { set; get; }
		public List<string> MysticArcanum { get; set; }
		public bool[] ArcanumUsed { get; set; }
		public bool EldritchMasterUsed { get; set; }
		public List<string> EldritchInvocations { set; get; }

		#region Subclass Bools

		public bool EntropicWard { set; get; }
		public bool DarkOnesOwnLuck { set; get; }
		public bool HurlThroughHell { set; get; }
		public bool FeyPresence { set; get; }
		public bool MistyEscape { set; get; }
		public bool DarkDelirium { set; get; }
		public bool HexbladesCurse { set; get; }
		public bool AccursedSpecter { set; get; }

		#endregion Subclass Bools
		public Warlock()
		{
			hitDie = new Die(8);
			armorProfs = new List<string> { "Light Armor" };
			weaponProfs = new List<string> { "Simple Weapons" };
			SavingProfs = new string[2] { "WisSave", "ChaSave" };
			prepMethod = SpellPrepMethod.KnowSomePrepNone;
			name = ClassName.Warlock;
		}
		public static Warlock fromYAML(string fName, int[] modifiers, int lvl, int prof)
		{
			Warlock returned = null;
			using (FileStream fin = File.OpenRead(fName))
			{
				TextReader reader = new StreamReader(fin);

				var deserializer = new Deserializer();
				returned = deserializer.Deserialize<Warlock>(reader);
			}
			returned.abilityModifiers = modifiers;
			returned.SpellcastingAbilityModifier = SpellMod.CHA;
			returned.Spellcasting = true;
			returned.level = lvl;
			returned.proficiency = prof;
			returned.HDrem = returned.level;
			returned.skillProfs.Add(returned.Skill1);
			returned.skillProfs.Add(returned.Skill2);
			switch (returned.level)
			{
				case 1:
					returned.spellSlotsMax = new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0 };
					break;
				case 2:
					returned.spellSlotsMax = new int[] { 2, 0, 0, 0, 0, 0, 0, 0, 0 };
					break;
				case 3:
					returned.spellSlotsMax = new int[] { 0, 2, 0, 0, 0, 0, 0, 0, 0 };
					break;
				case 4:
					returned.spellSlotsMax = new int[] { 0, 2, 0, 0, 0, 0, 0, 0, 0 };
					break;
				case 5:
					returned.spellSlotsMax = new int[] { 0, 0, 2, 0, 0, 0, 0, 0, 0 };
					break;
				case 6:
					returned.spellSlotsMax = new int[] { 0, 0, 2, 0, 0, 0, 0, 0, 0 };
					break;
				case 7:
					returned.spellSlotsMax = new int[] { 0, 0, 0, 2, 0, 0, 0, 0, 0 };
					break;
				case 8:
					returned.spellSlotsMax = new int[] { 0, 0, 0, 2, 0, 0, 0, 0, 0 };
					break;
				case 9:
					returned.spellSlotsMax = new int[] { 0, 0, 0, 0, 2, 0, 0, 0, 0 };
					break;
				case 10:
					returned.spellSlotsMax = new int[] { 0, 0, 0, 0, 2, 0, 0, 0, 0 };
					break;
				case 11:
					returned.spellSlotsMax = new int[] { 0, 0, 0, 0, 3, 0, 0, 0, 0 };
					break;
				case 12:
					returned.spellSlotsMax = new int[] { 0, 0, 0, 0, 3, 0, 0, 0, 0 };
					break;
				case 13:
					returned.spellSlotsMax = new int[] { 0, 0, 0, 0, 3, 0, 0, 0, 0 };
					break;
				case 14:
					returned.spellSlotsMax = new int[] { 0, 0, 0, 0, 3, 0, 0, 0, 0 };
					break;
				case 15:
					returned.spellSlotsMax = new int[] { 0, 0, 0, 0, 3, 0, 0, 0, 0 };
					break;
				case 16:
					returned.spellSlotsMax = new int[] { 0, 0, 0, 0, 3, 0, 0, 0, 0 };
					break;
				case 17:
					returned.spellSlotsMax = new int[] { 0, 0, 0, 0, 4, 0, 0, 0, 0 };
					break;
				case 18:
					returned.spellSlotsMax = new int[] { 0, 0, 0, 0, 4, 0, 0, 0, 0 };
					break;
				case 19:
					returned.spellSlotsMax = new int[] { 0, 0, 0, 0, 4, 0, 0, 0, 0 };
					break;
				case 20:
					returned.spellSlotsMax = new int[] { 0, 0, 0, 0, 4, 0, 0, 0, 0 };
					break;
			}
			if (returned.FeatNames != null)
			{
				if(returned.FeatNames.Count != 0)
				{
					int i = 0;
					foreach (string s in returned.FeatNames)
					{
						if(s == "Ability Score Increase")
						{
							returned.featList.Add(Feat.FromYAML(s, returned.ASIChoices[i]));
							i++;
						}
						else
							returned.featList.Add(Feat.FromYAML(s));
					}
				}
			}
			
			returned.getInfoBoxes();

			return returned;
		}
		public override List<GroupBox> getInfoBoxes()
		{
			var infoBoxes = new List<GroupBox>();
			infoBoxes.Add(AddClassDelimiter("Warlock"));
			if (level >= 1)
			{
				infoBoxes.Add(AddSubclassBox());
				infoBoxes.Add(AddPactMagicBox());
			}
			if (level >= 2)
			{
				infoBoxes.Add(AddInvocationsBox());
			}
			if (level >= 3)
			{
				infoBoxes.Add(AddPactBox());
			}
			if (level >= 4)
			{
				infoBoxes.Add(ASIBox(featList[0]));
			}
			if (level >= 8)
			{
				infoBoxes.Add(ASIBox(featList[1]));
			}
			if (level >= 11)
			{
				infoBoxes.Add(AddMysticArcanumBox());
			}
			if (level >= 12)
			{
				infoBoxes.Add(ASIBox(featList[2]));
			}
			if (level >= 16)
			{
				infoBoxes.Add(ASIBox(featList[3]));
			}
			if (level >= 19)
			{
				infoBoxes.Add(ASIBox(featList[4]));
			}
			if (level >= 20)
			{
				infoBoxes.Add(AddEldritchMasterBox());
			}
			return infoBoxes;
		}
		private GroupBox AddSubclassBox()
		{
			GroupBox box = new GroupBox();
			box.Text = "Otherworldly Patron";
			Label label = new Label();
			box.Controls.Add(label);
			label.MaximumSize = new Size(168, int.MaxValue);
			label.AutoSize = true;
			label.Location = new Point(6, 12);
			int low = 0;
			switch (OtherworldlyPatron)
			{
				case "The Great Old One":
					label.Text += $"  - Awakened Mind\n   - You can telepathically speak to any creature you can see within 30 feet of you. You don't need to share a language with the creature for it to understand your telepathic utterances, but the creature must be able to understand at least one language.\n\n";
					if (level >= 6)
					{
						// SR
						label.Text += $"  - Entropic Ward\n   - When a creature makes an attack roll against you, you can use your reaction to impose disadvantage on that roll. If the attack misses you, your next attack roll against the creature has advantage if you make it before the end of your next turn.\nOnce you use this feature, you can't use it again until you finish a short or long rest.\n\n";
					}
					if (level >= 10)
					{
						label.Text += $"  - Thought Shield\n   - Your thoughts can’t be read by telepathy or other means unless you allow it. You also have resistance to psychic damage, and whenever a creature deals psychic damage to you, that creature takes the same amount of damage that you do.\n\n";
					}
					if (level >= 14)
					{
						label.Text += $"  - Create Thrall\n   - You gain the ability to infect a humanoid’s mind with the alien magic of your patron. You can use your action to touch an incapacitated humanoid. That creature is then charmed by you until a remove curse spell is cast on it, the charmed condition is removed from it, or you use this feature again.\nYou can communicate telepathically with the charmed creature as long as the two of you are on the same plane of existence.\n\n";
					}
					low += label.Size.Height + 12;
					if(level >= 6)
					{
						CheckBox EntropicWardBox = new CheckBox();
						EntropicWardBox.Checked = EntropicWard;
						EntropicWardBox.AutoSize = true;
						EntropicWardBox.Text = "Entropic Ward / SR";
						box.Controls.Add(EntropicWardBox);
						EntropicWardBox.Location = new Point(6, low);
						EntropicWardBox.CheckedChanged += ToggleEntropicWard;
						resetOnSR.Add(EntropicWardBox);
						low += EntropicWardBox.Size.Height + 12;
					}

					break;
				case "The Fiend":
					label.Text += $"  - Dark One’s Blessing\n   - When you reduce a hostile creature to 0 hit points, you gain {abilityModifiers[5] + level} temporary hit points.\n\n";
					if (level >= 6)
					{
						// SR
						label.Text += $"  - Dark One’s Own Luck\n   - You can call on your patron to alter fate in your favor. When you make an ability check or a saving throw, you can use this feature to add a d10 to your roll. You can do so after seeing the initial roll but before any of the roll’s effects occur.\n   - Once you use this feature, you can’t use it again until you finish a short or long rest.\n\n";
					}
					if (level >= 10)
					{
						label.Text += $"  - Fiendish Resilience\n   - You can choose one damage type when you finish a short or long rest. You gain resistance to that damage type until you choose a different one with this feature. Damage from magical weapons or silver weapons ignores this resistance.\n\n";
					}
					if (level >= 14)
					{
						// LR
						label.Text += $"  - Hurl Through Hell\n   - When you hit a creature with an attack, you can use this feature to instantly transport the target through the lower planes. The creature disappears and hurtles through a nightmare landscape.\n   - At the end of your next turn, the target returns to the space it previously occupied, or the nearest unoccupied space. If the target is not a fiend, it takes 10d10 psychic damage as it reels from its horrific experience.\n   - Once you use this feature, you can’t use it again until you finish a long rest.\n\n";
					}
					low += label.Size.Height + 12;
					if(level >= 6)
					{
						CheckBox DarkOnesOwnLuckBox = new CheckBox();
						DarkOnesOwnLuckBox.Checked = DarkOnesOwnLuck;
						DarkOnesOwnLuckBox.AutoSize = true;
						DarkOnesOwnLuckBox.Text = "Dark One's Own Luck / SR";
						box.Controls.Add(DarkOnesOwnLuckBox);
						DarkOnesOwnLuckBox.Location = new Point(6, low);
						DarkOnesOwnLuckBox.CheckedChanged += ToggleDarkOnesOwnLuck;
						resetOnSR.Add(DarkOnesOwnLuckBox);
						low += DarkOnesOwnLuckBox.Size.Height + 12;
					}
					if(level >= 14)
					{
						CheckBox HurlThroughHellBox = new CheckBox();
						HurlThroughHellBox.Checked = HurlThroughHell;
						HurlThroughHellBox.AutoSize = true;
						HurlThroughHellBox.Text = "Hurl Through Hell / LR";
						box.Controls.Add(HurlThroughHellBox);
						HurlThroughHellBox.Location = new Point(6, low);
						HurlThroughHellBox.CheckedChanged += ToggleHurlThroughHell;
						resetOnLR.Add(HurlThroughHellBox);
						low += HurlThroughHellBox.Size.Height + 12;
					}
					break;
				case "The Archfey":
					// SR
					label.Text += $"  - Fey Presence\n   - Your patron bestows upon you the ability to project the beguiling and fearsome presence of the fey. As an action, you can cause each creature in a 10-foot cube originating from you to make a DC {8 + proficiency + abilityModifiers[5]} Wisdom saving throw. The creatures that fail their saving throws are all charmed or frightened by you (your choice) until the end of your next turn.\n   - Once you use this feature, you can’t use it again until you finish a short or long rest.\n\n";
					if (level >= 6)
					{
						// SR
						label.Text += "  - Misty Escape\n   - You can vanish in a puff of mist in response to harm. When you take damage, you can use your reaction to turn invisible and teleport up to 60 feet to an unoccupied space you can see. You remain invisible until the start of your next turn or until you attack or cast a spell.\n   - Once you use this feature, you can’t use it again until you finish a short or long rest.\n\n";
					}
					if (level >= 10)
					{
						label.Text += $"  - Beguiling Defenses\n   - Your patron teaches you how to turn the mind-affecting magic of your enemies against them. You are immune to being charmed, and when another creature attempts to charm you, you can use your reaction to attempt to turn the charm back on that creature. The creature must succeed on a Wisdom saving throw against your warlock spell save DC or be charmed by you for 1 minute or until the creature takes any damage.\n\n";
					}
					if (level >= 14)
					{
						// SR
						label.Text += $"  - Dark Delirium\n   - You can plunge a creature into an illusory realm. As an action, choose a creature that you can see within 60 feet of you. It must make a DC {8 + proficiency + abilityModifiers[5]} Wisdom saving throw. On a failed save, it is charmed or frightened by you (your choice) for 1 minute or until your concentration is broken (as if you are concentrating on a spell). This effect ends early if the creature takes any damage.\n   - Until this illusion ends, the creature thinks it is lost in a misty realm, the appearance of which you choose. The creature can see and hear only itself, you, and the illusion.\n   - You must finish a short or long rest before you can use this feature again.\n\n";
					}
					low += label.Size.Height + 12;
					CheckBox FeyPresenceBox = new CheckBox();
					FeyPresenceBox.Checked = FeyPresence;
					FeyPresenceBox.AutoSize = true;
					FeyPresenceBox.Text = "Fey Presence / SR";
					box.Controls.Add(FeyPresenceBox);
					FeyPresenceBox.Location = new Point(6, low);
					FeyPresenceBox.CheckedChanged += ToggleFeyPresence;
					resetOnSR.Add(FeyPresenceBox);
					low += FeyPresenceBox.Size.Height + 12;

					if(level >= 6)
					{
						CheckBox MistyEscapeBox = new CheckBox();
						MistyEscapeBox.Checked = MistyEscape;
						MistyEscapeBox.AutoSize = true;
						MistyEscapeBox.Text = "Misty Escape / SR";
						box.Controls.Add(MistyEscapeBox);
						MistyEscapeBox.Location = new Point(6, low);
						MistyEscapeBox.CheckedChanged += ToggleMistyEscape;
						resetOnSR.Add(MistyEscapeBox);
						low += MistyEscapeBox.Size.Height + 12;

					}
					if(level >= 14)
					{
						CheckBox DarkDeliriumBox = new CheckBox();
						DarkDeliriumBox.Checked = DarkDelirium;
						DarkDeliriumBox.AutoSize = true;
						DarkDeliriumBox.Text = "Dark Delirium / SR";
						box.Controls.Add(DarkDeliriumBox);
						DarkDeliriumBox.Location = new Point(6, low);
						DarkDeliriumBox.CheckedChanged += ToggleDarkDelirium;
						resetOnSR.Add(DarkDeliriumBox);
						low += DarkDeliriumBox.Size.Height + 12;
					}
					break;
				case "The Hexblade":
					// SR
					label.Text += $"  - Hexblade's Curse\n   - As a bonus action, choose one creature you can see within 30 feet of you. The target is cursed for 1 minute. The curse ends early if the target dies, you die, or you are incapacitated. Until the curse ends, you gain the following benefits:\n    - You gain a +{proficiency} bonus to damage rolls against the cursed target.\n    - Any attack roll you make against the cursed target is a critical hit on a roll of 19 or 20 on the d20.\n    - If the cursed target dies, you regain {level + abilityModifiers[5]} hit points.\n   - You can’t use this feature again until you finish a short or long rest.\n\n";
					label.Text += $"  - Hex Warrior\n  - Whenever you finish a long rest, you can touch one weapon that you are proficient with and that lacks the two-handed property. When you attack with that weapon, you can use your Charisma modifier, instead of Strength or Dexterity, for the attack and damage rolls. This benefit lasts until you finish a long rest. If you later gain the Pact of the Blade feature, this benefit extends to every pact weapon you conjure with that feature, no matter the weapon’s type.\n\n";
					armorProfs.Add("Medium Armor");
					armorProfs.Add("Shields");
					weaponProfs.Add("Martial Weapons");
					if (level >= 6)
					{
						// LR
						label.Text += $"  - Accursed Specter\n   - When you slay a humanoid, you can cause its spirit to rise from its corpse as a specter, the statistics for which are in the Monster Manual. When the specter appears, it gains {level / 2} temporary hit points. Roll initiative for the specter, which has its own turns. It obeys your verbal commands, and it gains a {abilityModifiers[5]} bonus to its attack rolls.\n   - The specter remains in your service until the end of your next long rest, at which point it vanishes to the afterlife.\n   - Once you bind a specter with this feature, you can’t use the feature again until you finish a long rest.\n\n";
					}
					if (level >= 10)
					{
						label.Text += $"  - Armor of Hexes\n   - If the target cursed by your Hexblade’s Curse hits you with an attack roll, you can use your reaction to roll a d6. On a 4 or higher, the attack instead misses you, regardless of its roll.\n\n";
					}
					if (level >= 14)
					{
						label.Text += $"  - Master of Hexes\n   - When the creature cursed by your Hexblade’s Curse dies, you can apply the curse to a different creature you can see within 30 feet of you, provided you aren’t incapacitated. When you apply the curse in this way, you don’t regain hit points from the death of the previously cursed creature.\n\n";
					}
					low += label.Size.Height + 12;

					CheckBox HexbladesCurseBox = new CheckBox();
					HexbladesCurseBox.Checked = HexbladesCurse;
					HexbladesCurseBox.AutoSize = true;
					HexbladesCurseBox.Text = "Hexblade's Curse / SR";
					box.Controls.Add(HexbladesCurseBox);
					HexbladesCurseBox.Location = new Point(6, low);
					HexbladesCurseBox.CheckedChanged += ToggleHexbladesCurse;
					resetOnSR.Add(HexbladesCurseBox);
					low += HexbladesCurseBox.Size.Height + 12;

					if(level >= 14)
					{
						CheckBox AccursedSpecterBox = new CheckBox();
						AccursedSpecterBox.Checked = AccursedSpecter;
						AccursedSpecterBox.AutoSize = true;
						AccursedSpecterBox.Text = "Accursed Specter / LR";
						box.Controls.Add(AccursedSpecterBox);
						AccursedSpecterBox.Location = new Point(6, low);
						AccursedSpecterBox.CheckedChanged += ToggleAccursedSpecter;
						resetOnLR.Add(AccursedSpecterBox);
						low += AccursedSpecterBox.Size.Height + 12;
					}

					break;
				default:
					label.Text += $"No Subclass chosen!";
					break;
			}
			
			box.Size = new Size(180, low + 6);
			label.MouseDown += DisplayOnRightClick;
			return box;
		}
		private GroupBox AddPactMagicBox()
		{
			int numCantrips = 2;
			if (level >= 4)
				numCantrips = 3;
			if (level >= 10)
				numCantrips = 4;

			int numSpells = 2;
			if (level < 10)
				numSpells = level + 1;
			else
				numSpells = 10 + ((level - 10) / 2);

			int numSpellSlots = 1;
			if (level > 1 && level <= 10)
				numSpellSlots = 2;
			else if (level > 10 && level < 17)
				numSpellSlots = 3;
			else
				numSpellSlots = 4;

			int slotLevel = 1;
			if (level > 2)
				slotLevel = 2;
			if (level > 10)
				slotLevel = 3;
			if (level > 16)
				slotLevel = 4;

			GroupBox box = new GroupBox();
			box.Text = "Pact Magic";
			Label label = new Label();
			label.Text = "Your arcane research and the magic bestowed on you by your patron have given you facility with spells. See Spells Rules for the general rules of spellcasting and the Spells Listing for the warlock spell list.\n\n";
			label.Text += $" - Cantrips\n  - You know {numCantrips} cantrips of your choice from the warlock spell list. You learn additional warlock cantrips of your choice at higher levels.\n\n";
			label.Text += $" - Spell Slots\n  - You have {numSpellSlots} level {slotLevel} spell slots. All of your spell slots are the same level. To cast one of your warlock spells of 1st level or higher, you must expend a spell slot. You regain all expended Pact Magic spell slots when you finish a short or long rest.\n\n";
			label.Text += $" - Spells Known of 1st Level and Higher\n  - You know {numSpells} spells of your choice from the warlock spell list.\n  - When you gain a level in this class, you can choose one of the warlock spells you know and replace it with another spell from the warlock spell list, which also must be of a level for which you have spell slots.\n\n";
			label.Text += $" - Spellcasting Ability\n  - Charisma is your spellcasting ability for your warlock spells, so you use your Charisma whenever a spell refers to your spellcasting ability.\n\n";
			label.Text += $" - Spellcasting Focus\n  - You can use an arcane focus as a spellcasting focus for your warlock spells.\n\n";
			Spellcasting = true;
			label.MaximumSize = new Size(168, int.MaxValue);
			label.AutoSize = true;
			box.Controls.Add(label);
			label.Location = new Point(6, 12);
			box.Size = new Size(180, label.Size.Height + 18);
			label.MouseDown += DisplayOnRightClick;
			return box;
		}
		private GroupBox AddInvocationsBox()
		{
			int number = 2;
			if (level >= 18) number = 8;
			else if (level >= 15) number = 7;
			else if (level >= 12) number = 6;
			else if (level >= 9) number = 5;
			else if (level >= 7) number = 4;
			else if (level >= 5) number = 3;
			GroupBox box = new GroupBox();
			box.Text = "Eldritch Invocations";
			Label label = new Label();
			label.Text += $" - In your study of occult lore, you have unearthed eldritch invocations, fragments of forbidden knowledge that imbue you with an abiding magical ability.\nYou know {number} eldritch invocations of your choice. Your invocation options are detailed at the end of the class description.\n - Additionally, when you gain a level in this class, you can choose one of the invocations you know and replace it with another invocation that you could learn at that level.\n - If an eldritch invocation has prerequisites, you must meet them to learn it. You can learn the invocation at the same time that you meet its prerequisites. A level prerequisite refers to your level in this class.\n\n";
			foreach (string s in EldritchInvocations)
			{
				label.Text += $"  - {s}\n";
				switch (s)
				{
					case "Agonizing Blast":
						label.Text += $"   - When you cast eldritch blast, it deals an additional +{abilityModifiers[5]} damage on a hit.\n\n";
						break;
					case "Armor of Shadows":
						label.Text += $"   - You can cast mage armor on yourself at will, without expending a spell slot or material components.\n\n";
						FirstLevelSpells.Add("Mage Armor");
						break;
					case "Ascendant Step":
						label.Text += $"   - You can cast levitate on yourself at will, without expending a spell slot or material components.\n\n";
						SecondLevelSpells.Add("Levitate");
						break;
					case "Aspect of the Moon":
						label.Text += $"   - You no longer need to sleep and can’t be forced to sleep by any means. To gain the benefits of a long rest, you can spend all 8 hours doing light activity, such as reading your Book of Shadows and keeping watch.\n\n";
						break;
					case "Beast Speech":
						label.Text += $"   - You can cast speak with animals at will, without expending a spell slot.\n\n";
						FirstLevelSpells.Add("Speak With Animals");
						break;
					case "Beguiling Influence":
						label.Text += $"   - You gain proficiency in the Deception and Persuasion skills.\n\n";
						skillProfs.Add("Deception");
						skillProfs.Add("Persuasion");
						break;
					case "Bewitching Whispers":
						label.Text += $"   - You can cast compulsion once using a warlock spell slot. You can’t do so again until you finish a long rest.\n\n";
						// 1/LR
						break;
					case "Bond of the Talisman":
						label.Text += $"   - While someone else is wearing your talisman, you can use your action to teleport to the unoccupied space closest to them, provided the two of you are on the same plane of existence. The wearer of your talisman can do the same thing, using their action to teleport to you. The teleportation can be used a number of times equal to your proficiency bonus, and all expended uses are restored when you finish a long rest.\n\n";
						// PROF/LR
						break;
					case "Book of Ancient Secrets":
						label.Text += $"   - You can now inscribe magical rituals in your Book of Shadows. Choose two 1st-level spells that have the ritual tag from any class’s spell list (the two needn’t be from the same list). The spells appear in the book and don’t count against the number of spells you know. With your Book of Shadows in hand, you can cast the chosen spells as rituals. You can’t cast the spells except as rituals, unless you’ve learned them by some other means. You can also cast a warlock spell you know as a ritual if it has the ritual tag.\n   - On your adventures, you can add other ritual spells to your Book of Shadows. When you find such a spell, you can add it to the book if the spell’s level is equal to or less than half your warlock level (rounded up) and if you can spare the time to transcribe the spell. For each level of the spell, the transcription process takes 2 hours and costs 50 gp for the rare inks needed to inscribe it.\n\n";
						break;
					case "Chains of Carceri":
						label.Text += $"   - You can cast hold monster at will — targeting a celestial, fiend, or elemental — without expending a spell slot or material components. You must finish a long rest before you can use this invocation on the same creature again.\n\n";
						// 1/Creature/LR
						break;
					case "Cloak of Flies":
						label.Text += $"   - As a bonus action, you can surround yourself with a magical aura that looks like buzzing flies. The aura extends 5 feet from you in every direction, but not through total cover. It lasts until you’re incapacitated or you dismiss it as a bonus action.\n   - The aura grants you advantage on Charisma (Intimidation) checks but disadvantage on all other Charisma checks. Any other creature that starts its turn in the aura takes {Math.Max(0, abilityModifiers[5])} poison damage.\n   - Once you use this invocation, you can’t use it again until you finish a short or long rest.\n\n";
						// 1/SR
						break;
					case "Devil’s Sight":
						label.Text += $"   - You can see normally in darkness, both magical and nonmagical, to a distance of 120 feet.\n\n";
						break;
					case "Dreadful Word":
						label.Text += $"   - You can cast confusion once using a warlock spell slot. You can’t do so again until you finish a long rest.\n\n";
						// 1/LR
						break;
					case "Eldritch Mind":
						label.Text += $"   - You have advantage on Constitution saving throws that you make to maintain your concentration on a spell.\n\n";
						break;
					case "Eldritch Sight":
						label.Text += $"   - You can cast detect magic at will, without expending a spell slot.\n\n";
						FirstLevelSpells.Add("Detect Magic");
						break;
					case "Eldritch Smite":
						label.Text += $"   - Once per turn when you hit a creature with your pact weapon, you can expend a warlock spell slot to deal an extra 1d8 force damage to the target, plus another 1d8 per level of the spell slot, and you can knock the target prone if it is Huge or smaller.\n\n";
						break;
					case "Eldritch Spear":
						label.Text += $"   - When you cast eldritch blast, its range is 300 feet.\n\n";
						break;
					case "Eyes of the Rune Keeper":
						label.Text += $"   - You can read all writing.\n\n";
						break;
					case "Far Scribe":
						label.Text += $"   - A new page appears in your Book of Shadows. With your permission, a creature can use its action to write its name on that page, which can contain a number of names equal to your proficiency bonus.\n   - You can cast the sending spell, targeting a creature whose name is on the page, without using a spell slot and without using material components. To do so, you must write the message on the page. The target hears the message in their mind, and if the target replies, their message appears on the page, rather than in your mind. The writing disappears after 1 minute.\n   - As an action, you can magically erase a name on the page by touching it.\n\n";
						break;
					case "Fiendish Vigor":
						label.Text += $"   - You can cast false life on yourself at will as a 1st-level spell, without expending a spell slot or material components.\n\n";
						break;
					case "Gaze of Two Minds":
						label.Text += $"   - You can use your action to touch a willing humanoid and perceive through its senses until the end of your next turn. As long as the creature is on the same plane of existence as you, you can use your action on subsequent turns to maintain this connection, extending the duration until the end of your next turn. While perceiving through the other creature’s senses, you benefit from any special senses possessed by that creature, and you are blinded and deafened to your own surroundings.\n\n";
						break;
					case "Ghostly Gaze":
						label.Text += $"   - As an action, you gain the ability to see through solid objects to a range of 30 feet. Within that range, you have darkvision if you don’t already have it. This special sight lasts for 1 minute or until your concentration ends (as if you were concentrating on a spell). During that time, you perceive objects as ghostly, transparent images.\n   - Once you use this invocation, you can’t use it again until you finish a short or long rest.\n\n";
						// 1/SR
						break;
					case "Gift of the Depths":
						label.Text += $"   - You can breathe underwater, and you gain a swimming speed equal to your walking speed.\n   - You can also cast water breathing once without expending a spell slot. You regain the ability to do so when you finish a long rest.\n\n";
						// WATER BREATHING 1/LR
						break;
					case "Gift of the Ever-Living Ones":
						label.Text += $"   - Whenever you regain hit points while your familiar is within 100 feet of you, treat any dice rolled to determine the hit points you regain as having rolled their maximum value for you.\n\n";
						break;
					case "Gift of the Protectors":
						label.Text += $"   - A new page appears in your Book of Shadows. With your permission, a creature can use its action to write its name on that page, which can contain a number of names equal to your proficiency bonus.\n   - When any creature whose name is on the page is reduced to 0 hit points but not killed outright, the creature magically drops to 1 hit point instead. Once this magic is triggered, no creature can benefit from it until you finish a long rest.\n   - As an action, you can magically erase a name on the page by touching it.\n\n";
						break;
					case "Grasp of Hadar":
						label.Text += $"   - Once on each of your turns when you hit a creature with your eldritch blast, you can move that creature in a straight line 10 feet closer to you.\n\n";
						break;
					case "Improved Pact Weapon":
						label.Text += $"   - You can use any weapon you summon with your Pact of the Blade feature as a spellcasting focus for your warlock spells.\n   - In addition, the weapon gains a +1 bonus to its attack and damage rolls, unless it is a magic weapon that already has a bonus to those rolls.\n   - Finally, the weapon you conjure can be a shortbow, longbow, light crossbow, or heavy crossbow.\n\n";
						break;
					case "Investment of the Chain Master":
						label.Text += $"   - When you cast find familiar, you infuse the summoned familiar with a measure of your eldritch power, granting the creature the following benefits:\n   - The familiar gains either a flying speed or a swimming speed (your choice) of 40 feet.\n   - As a bonus action, you can command the familiar to take the Attack action.\n   - The familiar’s weapon attacks are considered magical for the purpose of overcoming immunity and resistance to nonmagical attacks.\n   - The familiar’s weapon attacks are considered magical for the purpose of overcoming immunity and resistance to nonmagical attacks.\n   - When the familiar takes damage, you can use your reaction to grant it resistance against that damage.\n\n";
						break;
					case "Lance of Lethargy":
						label.Text += $"   - Once on each of your turns when you hit a creature with your eldritch blast, you can reduce that creature’s speed by 10 feet until the end of your next turn.\n\n";
						break;
					case "Lifedrinker":
						label.Text += $"   - When you hit a creature with your pact weapon, the creature takes {Math.Max(1, abilityModifiers[5])} extra necrotic damage.\n\n";
						break;
					case "Maddening Hex":
						label.Text += $"   - As a bonus action, you cause a psychic disturbance around the target cursed by your hex spell or by a warlock feature of yours, such as Hexblade’s Curse or Sign of Ill Omen. When you do so, you deal {Math.Max(1, abilityModifiers[5])} psychic damage to the cursed target and each creature of your choice that you can see within 5 feet of it. To use this invocation, you must be able to see the cursed target, and it must be within 30 feet of you.\n\n";
						break;
					case "Mask of Many Faces":
						label.Text += $"   - You can cast disguise self at will, without expending a spell slot.\n\n";
						FirstLevelSpells.Add("Disguise Self");
						break;
					case "Master of Myriad Forms":
						label.Text += $"   - You can cast alter self at will, without expending a spell slot.\n\n";
						SecondLevelSpells.Add("Alter Self");
						// ALTER SELF
						break;
					case "Minions of Chaos":
						label.Text += $"   - You can cast conjure elemental once using a warlock spell slot. You can’t do so again until you finish a long rest.\n\n";
						// CONJURE ELEMENTAL 1/LR
						break;
					case "Mire the Mind":
						label.Text += $"   - You can cast slow once using a warlock spell slot. You can’t do so again until you finish a long rest.\n\n";
						// SLOW 1/LR
						break;
					case "Misty Visions":
						label.Text += $"   - You can cast silent image at will, without expending a spell slot or material components.\n\n";
						FirstLevelSpells.Add("Silent Image");
						// SILENT IMAGE
						break;
					case "One with Shadows":
						label.Text += $"   - When you are in an area of dim light or darkness, you can use your action to become invisible until you move or take an action or a reaction.\n\n";
						break;
					case "Otherworldly Leap":
						label.Text += $"   - You can cast jump on yourself at will, without expending a spell slot or material components.\n\n";
						FirstLevelSpells.Add("Jump");
						break;
					case "Protection of the Talisman":
						label.Text += $"  - When the wearer of your talisman fails a saving throw, they can add a d4 to the roll, potentially turning the save into a success. This benefit can be used {proficiency} times, and all expended uses are restored when you finish a long rest.\n\n";
						break;
					case "Rebuke of the Talisman":
						label.Text += $"   - When the wearer of your talisman is hit by an attacker you can see within 30 feet of you, you can use your reaction to deal {proficiency} psychic damage to the attacker push it up to 10 feet away from the talisman’s wearer.\n\n";
						break;
					case "Relentless Hex":
						label.Text += $"   - Your curse creates a temporary bond between you and your target. As a bonus action, you can magically teleport up to 30 feet to an unoccupied space you can see within 5 feet of the target cursed by your hex spell or by a warlock feature of yours, such as Hexblade’s Curse or Sign of Ill Omen. To teleport in this way, you must be able to see the cursed target.\n\n";
						break;
					case "Repelling Blast":
						label.Text += $"   - When you hit a creature with eldritch blast, you can push the creature up to 10 feet away from you in a straight line.\n\n";
						break;
					case "Sculptor of Flesh":
						label.Text += $"   - You can cast polymorph once using a warlock spell slot. You can’t do so again until you finish a long rest.\n\n";
						// POLYMORPH 1/LR
						break;
					case "Shroud of Shadow":
						label.Text += $"   - You can cast invisibility at will, without expending a spell slot.\n\n";
						SecondLevelSpells.Add("Invisibility");
						break;
					case "Sign of Ill Omen":
						label.Text += $"   - You can cast bestow curse once using a warlock spell slot. You can’t do so again until you finish a long rest.\n\n";
						// BESTOW CURSE 1/LR
						break;
					case "Thief of Five Fates":
						label.Text += $"   - You can cast bane once using a warlock spell slot. You can’t do so again until you finish a long rest.\n\n";
						// BANE 1/LR
						break;
					case "Thirsting Blade":
						label.Text += $"   - You can attack with your pact weapon twice, instead of once, whenever you take the Attack action on your turn.\n\n";
						break;
					case "Tomb of Levistus":
						label.Text += $"   - As a reaction when you take damage, you can entomb yourself in ice, which melts away at the end of your next turn. You gain {10 * level} temporary hit points, which take as much of the triggering damage as possible. Immediately after you take the damage, you gain vulnerability to fire damage, your speed is reduced to 0, and you are incapacitated. These effects, including any remaining temporary hit points, all end when the ice melts.\nOnce you use this invocation, you can’t use it again until you finish a short or long rest.\n\n";
						break;
					case "Trickster's Escape":
						label.Text += $"   - You can cast freedom of movement once on yourself without expending a spell slot. You regain the ability to do so when you finish a long rest.\n\n";
						// FREEDOM OF MOVEMENT 1/LR
						break;
					case "Undying Servitude":
						label.Text += $"   - You can cast animate dead without using a spell slot. Once you do so, you can’t cast it in this way again until you finish a long rest.\n\n";
						// ANIMATE DEAD 1/LR
						break;
					case "Visions of Distant Realms":
						label.Text += $"   - You can cast arcane eye at will, without expending a spell slot.\n\n";
						FourthLevelSpells.Add("Arcane Eye");
						// ARCANE EYE
						break;
					case "Voice of the Chain Master":
						label.Text += $"   - You can communicate telepathically with your familiar and perceive through your familiar’s senses as long as you are on the same plane of existence. Additionally, while perceiving through your familiar’s senses, you can also speak through your familiar in your own voice, even if your familiar is normally incapable of speech.\n\n";
						break;
					case "Whispers of the Grave":
						label.Text += $"   - You can cast speak with dead at will, without expending a spell slot.\n\n";
						ThirdLevelSpells.Add("Speak With Dead");
						break;
					case "Witch Sight":
						label.Text += $"   - You can see the true form of any shapechanger or creature concealed by illusion or transmutation magic while the creature is within 30 feet of you and within line of sight.\n\n";
						break;
				}
			}
			label.MaximumSize = new Size(168, int.MaxValue);
			label.AutoSize = true;
			box.Controls.Add(label);
			label.Location = new Point(6, 12);
			box.Size = new Size(180, label.Size.Height + 18);
			label.MouseDown += DisplayOnRightClick;
			return box;
		}
		private GroupBox AddPactBox()
		{
			GroupBox box = new GroupBox();
			box.Text = "Pact Boon";
			Label label = new Label();
			label.Text = "Pact Weapon\n - Your otherworldly patron bestows a gift upon you for your loyal service.\n\n" +
				$"  - {PactBoon}\n";
			switch (PactBoon)
			{
				case "Pact of the Tome":
					label.Text += $"   - Your patron gives you a grimoire called a Book of Shadows. When you gain this feature, choose three cantrips from any class’s spell list (the three needn’t be from the same list). While the book is on your person, you can cast those cantrips at will. They don’t count against your number of cantrips known. If they don’t appear on the warlock spell list, they are nonetheless warlock spells for you.\n   - If you lose your Book of Shadows, you can perform a 1-hour ceremony to receive a replacement from your patron. This ceremony can be performed during a short or long rest, and it destroys the previous book. The book turns to ash when you die.\n\n";
					// BOOK OF SHADOWS SPELLS
					break;
				case "Pact of the Blade":
					label.Text += $"   - You can use your action to create a pact weapon in your empty hand. You can choose the form that this melee weapon takes each time you create it. You are proficient with it while you wield it. This weapon counts as magical for the purpose of overcoming resistance and immunity to nonmagical attacks and damage.\n   - Your pact weapon disappears if it is more than 5 feet away from you for 1 minute or more. It also disappears if you use this feature again, if you dismiss the weapon (no action required), or if you die.\n   - You can transform one magic weapon into your pact weapon by performing a special ritual while you hold the weapon. You perform the ritual over the course of 1 hour, which can be done during a short rest. You can then dismiss the weapon, shunting it into an extradimensional space, and it appears whenever you create your pact weapon thereafter. You can’t affect an artifact or a sentient weapon in this way. The weapon ceases being your pact weapon if you die, if you perform the 1-hour ritual on a different weapon, or if you use a 1-hour ritual to break your bond to it. The weapon appears at your feet if it is in the extradimensional space when the bond breaks.\n\n";
					break;
				case "Pact of the Chain":
					label.Text += $"   - You learn the find familiar spell and can cast it as a ritual. The spell doesn’t count against your number of spells known.\n   - When you cast the spell, you can choose one of the normal forms for your familiar or one of the following special forms: imp, pseudodragon, quasit, or sprite.\n   - Additionally, when you take the Attack action, you can forgo one of your own attacks to allow your familiar to make one attack with its reaction.\n\n";
					break;
				case "Pact of the Talisman":
					label.Text += $"   - Your patron gives you an amulet, a talisman that can aid the wearer when the need is great. When the wearer fails an ability check, they can add a d4 to the roll, potentially turning the roll into a success. This benefit can be used {proficiency} times, and all expended uses are restored when you finish a long rest.\n   - If you lose the talisman, you can perform a 1-hour ceremony to receive a replacement from your patron. This ceremony can be performed during a short or long rest, and it destroys the previous amulet. The talisman turns to ash when you die.\n\n";
					break;
			}
			label.MaximumSize = new Size(168, int.MaxValue);
			label.AutoSize = true;
			box.Controls.Add(label);
			label.Location = new Point(6, 12);
			box.Size = new Size(180, label.Size.Height + 18);
			label.MouseDown += DisplayOnRightClick;
			return box;
		}
		private GroupBox AddMysticArcanumBox()
		{
			GroupBox box = new GroupBox();
			box.Text = "Mystic Arcanum";
			Label label = new Label();
			label.Text = "  - Your patron bestows upon you a magical secret called an arcanum.\n" +
				"  - You can cast your arcanum spells once without expending a spell slot. You regain all uses of your Mystic Arcanum when you finish a long rest.\n";
			
			label.MaximumSize = new Size(168, int.MaxValue);
			label.AutoSize = true;
			box.Controls.Add(label);
			label.Location = new Point(6, 12);
			int lowPoint = label.Height + 16;
			for (int i = 0; i < 4; i++)
			{
				if (MysticArcanum[i] == null) break;
				else
				{
					CheckBox spellLabel = new CheckBox();
					spellLabel.Text = MysticArcanum[i];
					box.Controls.Add(spellLabel);
					spellLabel.Location = new Point(12, lowPoint);
					spellLabel.Checked = ArcanumUsed[i];
					spellLabel.CheckedChanged += ToggleArcanum;
					spellLabel.MouseDown += DisplaySpellOnRightClick;
					spellLabel.AutoSize = true;
					lowPoint += spellLabel.Height + 6;
					resetOnLR.Add(spellLabel);
				}
			}
			box.Size = new Size(180, lowPoint + 18);
			label.MouseDown += DisplayOnRightClick;
			
			return box;
		}
		private void ToggleArcanum(object sender, EventArgs e)
		{
			var box = sender as CheckBox;
			int i = MysticArcanum.IndexOf(box.Text);
			ArcanumUsed[i] = !ArcanumUsed[i];
		}
		private GroupBox AddEldritchMasterBox()
		{
			GroupBox box = new GroupBox();
			box.Text = "Eldritch Master";
			Label label = new Label();
			label.Text = " - You can draw on your inner reserve of mystical power while entreating your patron to regain expended spell slots. You can spend 1 minute entreating your patron for aid to regain all your expended spell slots from your Pact Magic feature. Once you regain spell slots with this feature, you must finish a long rest before you can do so again.";
			label.MaximumSize = new Size(168, int.MaxValue);
			label.AutoSize = true;
			box.Controls.Add(label);
			label.Location = new Point(6, 12);
			int lowPoint = label.Size.Height + 18;
			CheckBox cbox = new CheckBox();
			cbox.Checked = EldritchMasterUsed;
			cbox.Location = new Point(12, lowPoint);
			lowPoint += cbox.Height + 6;
			box.Controls.Add(cbox);
			cbox.Text = "/ Long Rest";
			box.Size = new Size(180, lowPoint + 18);
			cbox.CheckedChanged += ToggleMaster;
			resetOnLR.Add(cbox);
			label.MouseDown += DisplayOnRightClick;
			return box;
		}
		private void ToggleMaster(object sender, EventArgs e)
		{
			var box = sender as CheckBox;
			EldritchMasterUsed = box.Checked;
		}
		private void ToggleEntropicWard(object sender, EventArgs e)
		{
			var box = sender as CheckBox;
			EntropicWard = box.Checked;
		}
		private void ToggleDarkOnesOwnLuck(object sender, EventArgs e)
		{
			var box = sender as CheckBox;
			DarkOnesOwnLuck = box.Checked;
		}
		private void ToggleHurlThroughHell(object sender, EventArgs e)
		{
			var box = sender as CheckBox;
			HurlThroughHell = box.Checked;
		}
		private void ToggleFeyPresence(object sender, EventArgs e)
		{
			var c = sender as CheckBox;
			FeyPresence = c.Checked;
		}
		private void ToggleMistyEscape(object sender, EventArgs e)
		{
			var box = sender as CheckBox;
			MistyEscape = box.Checked;
		}
		private void ToggleDarkDelirium(object sender, EventArgs e)
		{
			var box = sender as CheckBox;
			DarkDelirium = box.Checked;
		}
		private void ToggleHexbladesCurse(object sender, EventArgs e)
		{
			var box = sender as CheckBox;
			HexbladesCurse = box.Checked;
		}
		private void ToggleAccursedSpecter(object sender, EventArgs e)
		{
			var box = sender as CheckBox;
			AccursedSpecter = box.Checked;
		}

		public override void shortRest(string name, int classnum)
		{
			// Get classInfo
			string[] classInfo = File.ReadAllLines($@".\Data\Characters\{name}\{name}Warlock.yaml");

			// Reset Spell Slots
			classInfo[0] = "spellSlots: [0, 0, 0, 0, 0, 0, 0, 0, 0]";
			foreach(var box in warlockSlotBoxes)
			{
				box.Checked = false;
			}
			for (int i = 0; i < 9; i++)
			{
				spellSlots[i] = 0;
			}

			// Reset Subclass Bools
			classInfo[22] = $"EntropicWard: false";
			classInfo[23] = $"DarkOnesOwnLuck: false";
			classInfo[25] = $"FeyPresence: false";
			classInfo[26] = $"MistyEscape: false";
			classInfo[27] = $"DarkDelirium: false";
			classInfo[28] = $"HexbladesCurse: false";

			// Reset SR Groupboxes
			foreach(CheckBox g in resetOnSR)
			{
				g.Checked = false;
			}

			// Write classInfo
			File.WriteAllLines($@".\Data\Characters\{name}\{name}Warlock.yaml", classInfo);
		}
		public override void longRest(string name)
		{
			// Refresh Hit Dice
			HDrem += (int)Math.Floor(level / 2.0);
			if (HDrem > level) HDrem = level;

			// Get classInfo
			string[] classInfo = File.ReadAllLines($@".\Data\Characters\{name}\{name}Warlock.yaml");

			// Reset Spell Slots
			classInfo[0] = "spellSlots: [0, 0, 0, 0, 0, 0, 0, 0, 0]";
			for(int i = 0; i < 9; i++)
			{
				spellSlots[i] = 0;
			}

			// Reset Mystic Arcanum
			classInfo[8] = "ArcanumUsed: [false, false, false, false]";
			for(int i = 0; i < 4; i++)
			{
				ArcanumUsed[i] = false;
			}

			// Reset Eldritch Master
			classInfo[9] = "EldritchMasterUsed: false";
			EldritchMasterUsed = false;

			// Reset Subclass Bools
			classInfo[22] = $"EntropicWard: false";
			classInfo[23] = $"DarkOnesOwnLuck: false";
			classInfo[24] = $"HurlThroughHell: false";
			classInfo[25] = $"FeyPresence: false";
			classInfo[26] = $"MistyEscape: false";
			classInfo[27] = $"DarkDelirium: false";
			classInfo[28] = $"HexbladesCurse: false";
			classInfo[29] = $"AccursedSpecter: false";

			// Reset Groupbox Checks
			foreach(var g in resetOnSR)
			{
				g.Checked = false;
			}
			foreach(var g in resetOnSR)
			{
				g.Checked = false;
			}


			// Write classInfo
			File.WriteAllLines($@".\Data\Characters\{name}\{name}Warlock.yaml", classInfo);
		}
		public override string[] getClassDetails(string name, string classname = "Warlock")
		{
			string[] classInfo = File.ReadAllLines($@".\Data\Characters\{name}\{name}Warlock.yaml");
			
			// Store spells
			string spells = "spellSlots: [";
			for(int i = 0; i < 8; i++)
				spells += $"{spellSlots[i]}, ";
			spells += $"{spellSlots[8]}]";
			classInfo[0] = spells;

			// Store Arcanum usage
			string arcanum = "ArcanumUsed: [";
			for(int i = 0; i < 3; i++)
				arcanum += $"{ArcanumUsed[i]}, ";
			arcanum += $"{ArcanumUsed[3]}]";
			classInfo[8] = arcanum;

			// Store Eldritch Master usage
			classInfo[9] = $"EldritchMasterUsed: {EldritchMasterUsed}";

			classInfo[22] = $"EntropicWard: {EntropicWard}";
			classInfo[23] = $"DarkOnesOwnLuck: {DarkOnesOwnLuck}";
			classInfo[24] = $"HurlThroughHell: {HurlThroughHell}";
			classInfo[25] = $"FeyPresence: {FeyPresence}";
			classInfo[26] = $"MistyEscape: {MistyEscape}";
			classInfo[27] = $"DarkDelirium: {DarkDelirium}";
			classInfo[28] = $"HexbladesCurse: {HexbladesCurse}";
			classInfo[29] = $"AccursedSpecter: {AccursedSpecter}";


			return classInfo;
		}

	}
}
