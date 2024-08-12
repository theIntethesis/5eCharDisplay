using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using YamlDotNet.Serialization;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.Versioning;

namespace _5eCharDisplay.Classes
{
    [SupportedOSPlatform("windows")]
    internal class Paladin : charClass
    {
        public string Skill1 { get; set; }
        public string Skill2 { get; set; }
        public string FightingStyle { get; set; }
        public string SacredOath { get; set; }
        public Paladin()
        {
            hitDie = new Die(10);
            armorProfs = new List<string> { "Light Armor", "Medium Armor", "Heavy Armor", "Shields" };
            weaponProfs = new List<string> { "Simple Weapons", "Martial Weapons" };
            SavingProfs = new string[2] { "WisSave", "ChaSave" };
        }

        public static Paladin fromYAML(string fName, int[] modifiers, int lvl, int prof)
        {
            Paladin returned = null;
            using (FileStream fin = File.OpenRead(fName))
            {
                TextReader reader = new StreamReader(fin);

                var deserializer = new Deserializer();
                returned = deserializer.Deserialize<Paladin>(reader);
            }
            returned.abilityModifiers = modifiers;
            returned.level = lvl;
            returned.proficiency = prof;
            returned.HDrem = returned.level;
            returned.skillProfs.Add(returned.Skill1);
            returned.skillProfs.Add(returned.Skill2);

            switch (returned.level)
            {
                case 2:
                    returned.spellcasting.spellSlotsMax = new int[] { 2, 0, 0, 0, 0, 0, 0, 0, 0 };
                    break;
                case 3:
                    returned.spellcasting.spellSlotsMax = new int[] { 3, 0, 0, 0, 0, 0, 0, 0, 0 };
                    break;
                case 4:
                    returned.spellcasting.spellSlotsMax = new int[] { 3, 0, 0, 0, 0, 0, 0, 0, 0 };
                    break;
                case 5:
                    returned.spellcasting.spellSlotsMax = new int[] { 4, 2, 0, 0, 0, 0, 0, 0, 0 };
                    break;
                case 6:
                    returned.spellcasting.spellSlotsMax = new int[] { 4, 2, 0, 0, 0, 0, 0, 0, 0 };
                    break;
                case 7:
                    returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 0, 0, 0, 0, 0, 0, 0 };
                    break;
                case 8:
                    returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 0, 0, 0, 0, 0, 0, 0 };
                    break;
                case 9:
                    returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 2, 0, 0, 0, 0, 0, 0 };
                    break;
                case 10:
                    returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 2, 0, 0, 0, 0, 0, 0 };
                    break;
                case 11:
                    returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 0, 0, 0, 0, 0, 0 };
                    break;
                case 12:
                    returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 0, 0, 0, 0, 0, 0 };
                    break;
                case 13:
                    returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 1, 0, 0, 0, 0, 0 };
                    break;
                case 14:
                    returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 1, 0, 0, 0, 0, 0 };
                    break;
                case 15:
                    returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 2, 0, 0, 0, 0, 0 };
                    break;
                case 16:
                    returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 2, 0, 0, 0, 0, 0 };
                    break;
                case 17:
                    returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 3, 1, 0, 0, 0, 0 };
                    break;
                case 18:
                    returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 3, 1, 0, 0, 0, 0 };
                    break;
                case 19:
                    returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 0, 0, 0, 0 };
                    break;
                case 20:
                    returned.spellcasting.spellSlotsMax = new int[] { 4, 3, 3, 3, 2, 0, 0, 0, 0 };
                    break;
            }
            returned.getInfo();
            return returned;
        }

        public override List<GroupBox> getInfoBoxes()
        {
            var infoBoxes = new List<GroupBox>();
            if (level >= 1)
            {
                infoBoxes.Add(AddDivineSenseBox());
                infoBoxes.Add(AddLayOnHandsBox());
            }
            if (level >= 2)
            {
                infoBoxes.Add(AddFightingStyleBox());
                infoBoxes.Add(AddSpellcastingBox());
                infoBoxes.Add(AddDivineSmiteBox());
                spellcasting.FirstLevelSpells = new List<string> { "Bless", "Ceremony", "Command", "Compelled Duel", "Cure Wounds", "Detect Evil and Good", "Detect Magic", "Detect Poison and Disease", "Divine Favor", "Heroism", "Protection from Evil and Good", "Purify Food and Drink", "Searing Smite", "Shield of Faith", "Thunderous Smite", "Wrathful Smite" };
            }
            if (level >= 3)
            {
                infoBoxes.Add(AddDivineHealthBox());
                infoBoxes.Add(AddSubclassBox());
            }
            if (level >= 4)
            {
                infoBoxes.Add(ASIBox(featList[0]));
            }
            if (level >= 5)
            {
                infoBoxes.Add(ExtraAttackBox());
            }
            if (level >= 6)
            {
                infoBoxes.Add(AuraofProtectionBox());
            }
            if (level >= 8)
            {
                infoBoxes.Add(ASIBox(featList[1]));
            }
            if (level >= 10)
            {
                infoBoxes.Add(AuraofCourageBox());
            }
            if (level >= 11)
            {
                infoBoxes.Add(ImprovedDivineSmiteBox());
            }
            if (level >= 12)
            {
                infoBoxes.Add(ASIBox(featList[2]));
            }
            if (level >= 14)
            {
                infoBoxes.Add(CleansingTouchBox());
            }
            if (level >= 16)
            {
                infoBoxes.Add(ASIBox(featList[3]));
            }
            if (level >= 19)
            {
                infoBoxes.Add(ASIBox(featList[4]));
            }

            return infoBoxes;
        }
        private GroupBox AddDivineSenseBox()
        {
            GroupBox box = new GroupBox();
            box.Text = "Divine Sense";
            Label label = new Label();
            label.Text = $" - The presence of strong evil registers on your senses like a noxious odor, and powerful good rings like heavenly music in your ears. As an action, you can open your awareness to detect such forces. Until the end of your next turn, you know the location of any celestial, fiend, or undead within 60 feet of you that is not behind total cover. You know the type of any being whose presence you sense, but not its identity. Within the same radius, you also detect the presence of any place or object that has been consecrated or desecrated, as with the hallow spell.\n - You can use this feature {1 + abilityModifiers[5]} times. When you finish a long rest, you regain all expended uses.";
            label.MaximumSize = new Size(168, int.MaxValue);
            label.AutoSize = true;
            box.Controls.Add(label);
            label.Location = new Point(6, 12);
            box.Size = new Size(180, label.Size.Height + 18);
            label.MouseDown += DisplayOnRightClick;
            return box;
        }
        private GroupBox AddLayOnHandsBox()
        {
            GroupBox box = new GroupBox();
            box.Text = "Lay on Hands";
            Label label = new Label();
            label.Text = $" - Your blessed touch can heal wounds. You have a pool of healing power that replenishes when you take a long rest. With that pool, you can restore a total {level * 5} hit points.";
            label.MaximumSize = new Size(168, int.MaxValue);
            label.AutoSize = true;
            box.Controls.Add(label);
            label.Location = new Point(6, 12);
            box.Size = new Size(180, label.Size.Height + 18);
            label.MouseDown += DisplayOnRightClick;
            return box;
        }
        private GroupBox AddFightingStyleBox()
        {
            GroupBox box = new GroupBox();
            box.Text = "Fighting Style";
            Label label = new Label();
            label.Text = $" - {FightingStyle}\n";
            switch (FightingStyle)
            {
                case "Defense":
                    label.Text += "  - While you are wearing armor, you gain a +1 bonus to AC.\n\n";
                    break;
                case "Dueling":
                    label.Text += "  - When you are wielding a melee weapon in one hand and no other weapons, you gain a +2 bonus to damage rolls with that weapon.\n\n";
                    break;
                case "Great Weapon Fighting":
                    label.Text += "  - When you roll a 1 or 2 on a damage die for an attack you make with a melee weapon that you are wielding with two hands, you can reroll the die and must use the new roll. The weapon must have the two-handed or versatile property for you to gain this benefit.\n\n";
                    break;
                case "Protection":
                    label.Text += "  - When a creature you can see attacks a target other than you that is within 5 feet of you, you can use your reaction to impose disadvantage on the attack roll. You must be wielding a shield.\n\n";
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
        private GroupBox AddSpellcastingBox()
        {
            GroupBox box = new GroupBox();
            box.Text = "Spellcasting";
            Label label = new Label();
            label.Text = $" - You have learned to draw on divine magic through meditation and prayer to cast spells as a cleric does.\n - The Spellcasting page shows how many spell slots you have to cast your paladin spells. To cast one of your paladin spells of 1st level or higher, you must expend a slot of the spell’s level or higher. You regain all expended spell slots when you finish a long rest.\n - You prepare the list of paladin spells that are available for you to cast on a long rest, choosing from the paladin spell list. When you do so, choose {(level / 2) + abilityModifiers[5]} spells from the paladin list. The spells must be of a level for which you have spell slots.\n - Charisma is your spellcasting ability for your paladin spells, since their power derives from the strength of your convictions.\n - You can use a holy symbol as a spellcasting focus for your paladin spells.";
            label.MaximumSize = new Size(168, int.MaxValue);
            label.AutoSize = true;
            box.Controls.Add(label);
            label.Location = new Point(6, 12);
            box.Size = new Size(180, label.Size.Height + 18);
            label.MouseDown += DisplayOnRightClick;
            return box;
        }
        private GroupBox AddDivineSmiteBox()
        {
            GroupBox box = new GroupBox();
            box.Text = "Divine Smite";
            Label label = new Label();
            label.Text = $" - When you hit a creature with a melee weapon attack, you can expend one spell slot to deal radiant damage to the target, in addition to the weapon’s damage. The extra damage is 1d8 plus 1d8 for each spell level, to a maximum of 5d8 (4th level spell slot). The damage increases by 1d8 if the target is an undead or a fiend, to a maximum of 6d8.";
            label.MaximumSize = new Size(168, int.MaxValue);
            label.AutoSize = true;
            box.Controls.Add(label);
            label.Location = new Point(6, 12);
            box.Size = new Size(180, label.Size.Height + 18);
            label.MouseDown += DisplayOnRightClick;
            return box;
        }
        private GroupBox AddDivineHealthBox()
        {
            GroupBox box = new GroupBox();
            box.Text = "Divine Health";
            Label label = new Label();
            label.Text = $" - The divine magic flowing through you makes you immune to disease.";
            label.MaximumSize = new Size(168, int.MaxValue);
            label.AutoSize = true;
            box.Controls.Add(label);
            label.Location = new Point(6, 12);
            box.Size = new Size(180, label.Size.Height + 18);
            label.MouseDown += DisplayOnRightClick;
            return box;
        }
        private GroupBox AddSubclassBox()
        {
            GroupBox box = new GroupBox();
            box.Text = "Sacred Oath";
            Label label = new Label();
            label.Text = $" - {SacredOath}\n";
            switch (SacredOath)
            {
                case "Oath of Devotion":
                    label.Text += $" - Channel Divinity\n  - You gain the following two Channel Divinity Options:\n   - Sacred Weapon. As an action, you can imbue one weapon that you are holding with positive energy, using your Channel Divinity. For 1 minute, you add +{Math.Max(abilityModifiers[5], 1)} to attack rolls made with that weapon. The weapon also emits bright light in a 20-foot radius and dim light 20 feet beyond that. If the weapon is not already magical, it becomes magical for the duration.\n   - You can end this effect on your turn as part of any other action. If you are no longer holding or carrying this weapon, or if you fall unconscious, this effect ends.\n   - Turn the Unholy. As an action, you present your holy symbol and speak a prayer censuring fiends and undead, using your Channel Divinity. Each fiend or undead that can see or hear you within 30 feet of you must make a DC {proficiency + 8 + abilityModifiers[5]} Wisdom saving throw. If the creature fails its saving throw, it is turned for 1 minute or until it takes damage.\n   - A turned creature must spend its turns trying to move as far away from you as it can, and it can’t willingly move to a space within 30 feet of you. It also can’t take reactions. For its action, it can use only the Dash action or try to escape from an effect that prevents it from moving. If there’s nowhere to move, the creature can use the Dodge action.\n\n";
                    spellcasting.AlwaysPrepared.Add("Protection from Evil and Good");
                    spellcasting.FirstLevelSpells.Add("Sanctuary");
                    spellcasting.AlwaysPrepared.Add("Sanctuary");
                    if (level >= 7)
                    {
                        int range = 10;
                        if (level >= 18)
                            range = 30;
                        label.Text += $" - Aura of Devotion\n  - You and friendly creatures within {range} feet of you can’t be charmed while you are conscious.";
                    }
                    if (level >= 15)
                    {
                        label.Text += $" - Purity of Spirit\n  - You are always under the effects of a protection from evil and good spell.\n\n";
                    }
                    if (level >= 20)
                    {
                        label.Text += $" - Holy Nimbus\n  - As an action, you can emanate an aura of sunlight. For 1 minute, bright light shines from you in a 30-foot radius, and dim light shines 30 feet beyond that.\n  - Whenever an enemy creature starts its turn in the bright light, the creature takes 10 radiant damage.\n  - In addition, for the duration, you have advantage on saving throws against spells cast by fiends or undead.\n  - Once you use this feature, you can’t use it again until you finish a long rest.\n\n";
                    }
                    break;
                case "Oath of Vengeance":
                    label.Text += $" - Channel Divinity\n  - You gain the following two Channel Divinity Options:\n   - Abjure Enemy. As an action, you present your holy symbol and speak a prayer of denunciation, using your Channel Divinity. Choose one creature within 60 feet of you that you can see. That creature must make a DC {proficiency + 8 + abilityModifiers[5]} Wisdom saving throw unless it is immune to being frightened. Fiends and undead have disadvantage on this saving throw.\n    On a failed save, the creature is frightened for 1 minute or until it takes any damage. While frightened, the creature's speed is 0, and it can't benefit from any bonus to its speed.\n    On a successful save, the creature's speed is halved for 1 minute or until the creature takes any damage.\n   - Vow of Enmity. As a bonus action you can utter a vow of enmity against a creature you can see within 10 feet of you, using your Channel Divinity. You gain advantate on attack rolls against the creature for 1 minute or until it drops to 0 hit points or falls unconscious.";
                    spellcasting.FirstLevelSpells.Add("Bane");
                    spellcasting.AlwaysPrepared.Add("Bane");
                    spellcasting.FirstLevelSpells.Add("Hunter's Mark");
                    spellcasting.AlwaysPrepared.Add("Hunter's Mark");
                    if (level >= 7)
                    {
                        label.Text += $" - Relentless Avenger\n  - When you hit a creature with an opportunity attack, you wan move up to half your speed immediately after the attack and as part of the same reaction. This movement doesn't provoke opportunity attacks.\n\n";
                    }
                    if (level >= 15)
                    {
                        label.Text += $" - Soul of Vengeance\n  - When a creature under the effect of your Vow of Enmity makes an attack, you can use your reaction to make a melee weapon attack against that creature if it is within range.\n\n";
                    }
                    if (level >= 20)
                    {
                        label.Text += $" - Avenging Angel\n  - Using your action, you undergo a transformation. For 1 hour, you gain the following benefits:\n   - Wings sprout from your back and grant you a flying speed of 60 feet.\n   - You emanate an aura of menace in a 30-foot radius. The first time any enemy creature enters the aura or starts its turn there during a battle, the creature must make a DC {proficiency + 8 + abilityModifiers[5]} Wisdom saving throw or become frightened of you for 1 minute or until it takes any damage. Attack rolls against the frightened creature have advantage.\n  - Once you use this feature, you can't use it again until you finish a long rest.\n\n";
                    }
                    break;
                case "Subclass 3":
                    label.Text += $"";
                    if (level >= 6)
                    {
                        label.Text += $"";
                    }
                    if (level >= 10)
                    {
                        label.Text += $"";
                    }
                    if (level >= 14)
                    {
                        label.Text += $"";
                    }
                    break;
                default:
                    label.Text += $"No Subclass chosen!";
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
        private GroupBox ExtraAttackBox()
        {
            GroupBox box = new GroupBox();
            box.Text = "Extra Attack";
            Label label = new Label();
            label.Text = $" - You can attack twice, instead of once, whenever you take the attack action on your turn.";
            label.MaximumSize = new Size(168, int.MaxValue);
            label.AutoSize = true;
            box.Controls.Add(label);
            label.Location = new Point(6, 12);
            box.Size = new Size(180, label.Size.Height + 18);
            label.MouseDown += DisplayOnRightClick;
            return box;
        }
        private GroupBox AuraofProtectionBox()
        {
            int range = 10;
            if (level >= 18) range = 30;
            GroupBox box = new GroupBox();
            box.Text = "Aura of Protection";
            Label label = new Label();
            label.Text = $" - Whenever you or a friendly creature within {range} feet of you makes a saving throw, the creature gains a +{Math.Max(1, abilityModifiers[5])} bonus to the saving throw. You must be conscious to grant this bonus.";
            label.MaximumSize = new Size(168, int.MaxValue);
            label.AutoSize = true;
            box.Controls.Add(label);
            label.Location = new Point(6, 12);
            box.Size = new Size(180, label.Size.Height + 18);
            label.MouseDown += DisplayOnRightClick;
            return box;
        }
        private GroupBox AuraofCourageBox()
        {
            int range = 10;
            if (level >= 18) range = 30;
            GroupBox box = new GroupBox();
            box.Text = "Aura of Courage";
            Label label = new Label();
            label.Text = $" - You and friendly creatures within {range} feet of you can't be frightened while you are conscious.";
            label.MaximumSize = new Size(168, int.MaxValue);
            label.AutoSize = true;
            box.Controls.Add(label);
            label.Location = new Point(6, 12);
            box.Size = new Size(180, label.Size.Height + 18);
            label.MouseDown += DisplayOnRightClick;
            return box;
        }
        private GroupBox ImprovedDivineSmiteBox()
        {
            GroupBox box = new GroupBox();
            box.Text = "Improved Divine Smite";
            Label label = new Label();
            label.Text = $" - Whenever you hit a creature with a melee attack, the creature takes an additional 1d8 radiant damage.";
            label.MaximumSize = new Size(168, int.MaxValue);
            label.AutoSize = true;
            box.Controls.Add(label);
            label.Location = new Point(6, 12);
            box.Size = new Size(180, label.Size.Height + 18);
            label.MouseDown += DisplayOnRightClick;
            return box;
        }
        private GroupBox CleansingTouchBox()
        {
            GroupBox box = new GroupBox();
            box.Text = "Cleansing Touch";
            Label label = new Label();
            label.Text = $" - You can use an action to end one spell on yourself or on one willing creature that you touch.\n  - You can use this feature {Math.Max(1, abilityModifiers[5])}, and you regain all expended uses when you finish a long rest.";
            label.MaximumSize = new Size(168, int.MaxValue);
            label.AutoSize = true;
            box.Controls.Add(label);
            label.Location = new Point(6, 12);
            box.Size = new Size(180, label.Size.Height + 18);
            label.MouseDown += DisplayOnRightClick;
            return box;
        }

    }
}
