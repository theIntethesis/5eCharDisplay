using System;
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
    internal class Magus : charClass
    {
        public string Skill1 { set; get; }
        public string Skill2 { set; get; }
        public string HybridStudy { get; set; }
        public Magus()
        {
            hitDie = new Die(8);
            armorProfs = new List<string> { "Light Armor", "Medium Armor" };
            weaponProfs = new List<string> { "Simple Weapons", "Martial Weapons" };
            SavingProfs = new string[2] { "ConSave", "IntSave" };
            prepMethod = SpellPrepMethod.KnowSomePrepSome;
            Spellcasting = true;
        }
        private GroupBox AddWarcasting(int lvl)
        {
            GroupBox box = new GroupBox();
            box.Text = "Warcasting";
            Label label = new Label();
            label.Text = $"You can cast spells out of your spellbook. You can prepare {abilityModifiers[3] + lvl / 2} spells on a long rest.";
            label.MaximumSize = new Size(168, int.MaxValue);
            label.AutoSize = true;
            box.Controls.Add(label);
            label.Location = new Point(6, 12);
            box.Size = new Size(180, label.Size.Height + 18);
            label.MouseDown += DisplayOnRightClick;
            return box;
        }
        private GroupBox AddCascade()
        {
            GroupBox box = new GroupBox();
            box.Text = "Arcane Cascade";
            Label label = new Label();
            label.Text = $"As a bonus action after casting a spell or using your spellstrike, you can enter Arcane Cascade for a minute. While in Arcane Cascade, you gain a +{proficiency} bonus to melee attack damage rolls.";
            label.MaximumSize = new Size(168, int.MaxValue);
            label.AutoSize = true;
            box.Controls.Add(label);
            label.Location = new Point(6, 12);
            box.Size = new Size(180, label.Size.Height + 18);
            label.MouseDown += DisplayOnRightClick;
            return box;
        }
        private GroupBox AddSpellstrike()
        {
            GroupBox box = new GroupBox();
            box.Text = "Spellstrike";
            Label label = new Label();
            label.Text = $"As an action, you can imbue a spell that makes an attack roll against a single creature or forces a single creature to make a saving throw. Attack roll spells use your melee attack roll, and spell saves get added onto the attack.";
            label.MaximumSize = new Size(168, int.MaxValue);
            label.AutoSize = true;
            box.Controls.Add(label);
            label.Location = new Point(6, 12);
            box.Size = new Size(180, label.Size.Height + 18);
            label.MouseDown += DisplayOnRightClick;
            return box;
        }
        private GroupBox AddSubclass()
        {
            GroupBox box = new GroupBox();
            box.Text = "Hybrid Study";
            Label label = new Label();
            switch (HybridStudy)
            {
                case "Laughing Shadow":
                    label.Text += $"  - Covering Weave\n   - While in Arcane Cascade and wearing no armor, your base AC becomes {13 + abilityModifiers[1]}.\n";
                    label.Text += $"  - Spirit Sheath\n   - You've constructed a dimensional sheath within your clothes. When you make a spellstrike, you can draw a weapon stored within as a free action.\n";
                    if (level >= 6)
                    {
                        label.Text += $"  - Arcane Speed\n   - While in Arcane Cascade, you gain a 5 - foot bonus to your walking speed, or a 15 - foot bonus if you’re unarmored.\n";
                    }
                    if (level >= 10)
                    {
                        label.Text += $"  - Surrounding Stealth\n   - When a creature within 30 feet of you makes a Dexterity (Stealth) check, you can use your reaction to expend a Focus Point, granting them advantage on the roll, potentially allowing them to succeed. You may use this ability after they roll, but before you know the outcome of the roll.\n";
                    }
                    if (level >= 14)
                    {
                        label.Text += $"  - Dimensional Disappearance\n   - When you cast dimensional assault, you’re affected by the invisibility spell at the end of the teleport. You can choose to not make the attack that is normally part of dimensional assault. If you do make the attack, your invisibility ends, as is normal.\n";
                    }
                    if (level >= 18)
                    {
                        label.Text += $"  - Fast Recharge\n   - You no longer need to recharge your spellstrike.\n";
                    }
                    break;
                case "Inexorable Iron":
                    label.Text += $"  - War Mage's Mettle\n   - You can use simple and martial weapons as spellcasting focuses. In addition, you can use your spellstrike while holding a two-handed weapon.\n\n";
                    if (level >= 6)
                    {
                        label.Text += $"  - Devastating Spellstrike\n   - When you make a Spellstrike attack in your Arcane Cascade, and adjacent creatures of your choice take {proficiency} damage, and the damage type is the same as your Arcane Cascade.\n\n";
                    }
                    if (level >= 10)
                    {
                        label.Text += $"  - Focused Strike\n   - When you make a Spellstrike attack or cast a spell while in your Arcane Cascade, you can expend up to 1 Focus Point to gain a +10 bonus to damage rolls. The additional damage is of the same type as your Arcane Cascade.\n\n";
                    }
                    if (level >= 14)
                    {
                        label.Text += $"  - Mage of Opportunity\n   - When a creature provokes an attack of opportunity from you, you can use your reaction to Spellstrike them, provided that you have your Spellstrike charged.\n\n";
                    }
                    if (level >= 18)
                    {
                        label.Text += $"  - Cascading Shield\n   - When you enter your Arcane Cascade and at the start of each of your turns while your in that stance, if you're weilding a melee weapon in two hands, you gain temporary Hit Points equal to half your level (minimum 1 temporary HP).\n\n";
                    }
                    break;
                case "Starlit Span":
                    label.Text += $"  - Ranger's Spellstrike\n   - When you use Spellstrike, you can make a ranged weapon attack, as long as the target is withing the first range increment of your ranged weapon. You can deliver the spell even if its range is shorter than the range increment of your ranged attack.\n\n";
                    if (level >= 6)
                    {
                        label.Text += $"  - Expansive Spellstrike\n   - Rather than needing to use a spell that has an attack roll for a Spellstrike, you can use a harmful spell that has an area of effect or targets multiple creatures. When you cast a spell this way, it has the following restrictions:\n" +
                            $"    - If your attack critically fails, the spell is lost with no effect.\n" +
                            $"    - Creatures use their normal defenses agains the spell.\n" +
                            $"    - If the spell lets you select a number of targets, it instead targets only the creature you attacked.\n" +
                            $"    - If the spell has an area, the target must be centered in the area. A cone or line emits from your direction, and if you're not adjacent to the target, the effect emits from an adjacent to the target.\n\n";
                    }
                    if (level >= 10)
                    {
                        label.Text += $"  - Meteoric Spellstrike\n   - When you make a Spellstrike with a ranged weapon, if the spell was a leveled spell, each creature between you and the target takes damage equal to double the spells level. Determind the damage type as described in Arcane Cascade.\n\n";
                    }
                    if (level >= 14)
                    {
                        label.Text += $"  - Overwhelming Spellstrike\n   - When you make a Spellstrike attack, the damage caused by it ignores resistance to Acid, Cold, Fire, Lightning, and Thunder.\n\n";
                    }
                    if (level >= 18)
                    {
                        label.Text += $"  - Versatile Spellstrike\n   - Once per long rest when you make a Spellstrike, you ignore any restrictions of the spell, but the spell is cast at two levels lower than normal.\n\n";
                    }
                    break;
                case "Sparkling Targe":
                    label.Text += $"  - Defensive Spellcasting\n   - You gain proficiency with shields and can use them as spellcasting focuses. In addition, you can use your spellstrike while holding a shield.\n\n";
                    if (level >= 6)
                    {
                        label.Text += $"  - Steady Spellcasting\n   - As a reaction, if you were to lose concentration due to taking damage, you can choose to maintain concentration instead. You can use this feature {proficiency} times, and you regain all expended uses upon taking a long rest.\n\n";
                    }
                    if (level >= 10)
                    {
                        label.Text += $"  - Cascade Countermeasure\n   - While in your Arcane Cascade, you can use an action to expend a Focus Point, making yourself resistant to magical damage until your Arcane Cascade ends.\n\n";
                    }
                    if (level >= 14)
                    {
                        label.Text += $"  - Weave-Infused Shield\n   - While you're in Arcane Cascade, you add your shield bonus to saving throws against spells and other magical effects.\n\n";
                    }
                    if (level >= 18)
                    {
                        label.Text += $"  - Mirrored Defense\n   - When an enemy successfully lands an attack against you while you're in your Arcane Cascade, they take {level / 2} damage. The damage type is that of your Arcane Cascade.\n\n";
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
        private GroupBox AddConflux(int level)
        {
            int numPts;
            if (level > 1 && level < 9)
                numPts = 1;
            else if (level > 8 && level < 15)
                numPts = 2;
            else
                numPts = 3;
            GroupBox box = new GroupBox();
            box.Text = "Conflux Spells";
            Label label = new Label();
            label.Text = $"You have {numPts} Focus point(s).\n"; 
            switch (HybridStudy)
            {
                case "Laughing Shadow":
                    label.Text += $"  - As a bonus action, you can expend a Focus Point to teleport up to half your movement speed away. The unoccupied space must be within 5 feet of a creature, and you then must make a melee attack against a creature.\n\n";
                    break;
                case "Inexorable Iron":
                    int damage = abilityModifiers[3];
                    if (level >= 17)
                        damage *= 4;
                    else if (level >= 11)
                        damage *= 3;
                    else if (level >= 5)
                        damage *= 2;

                    label.Text += $"  - As an action, you can expend a point from your Focus Pool to cause a toppling wave of sonic vibration. Make a melee attack roll against a target withing range. Each creature in a 15-foot cone emanating from you must make a DC {8 + proficiency + abilityModifiers[3]} Constitution saving throw or take {damage} thunder damage and fall prone. On a successful save, a creature only takes half as much damage and does not fall prone.\n\n";
                    break;
                case "Sparkling Targe":
                    label.Text += $"  - As a bonus action, you can make expend a Focus Point to perform a defensive strike. Make a melee attack against a creature within range. You then can use your reaction to cast shield without expending a spell slot.\n\n";
                    break;
                case "Starlit Span":
                    label.Text += $"  - As a bonus action, you can expend a Focus Point to choose a creature that you can see within 120 feet. The next ranged attack you would make against that creature automatically hits and leaves a meteor trail in the air, which grants advantage on ranged attacks agains that creature until the beginning of your next turn.\n\n";
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
        private GroupBox StudiousSpells(int lvl)
        {
            string spellLvl = "2nd";
            if (lvl > 10)
                spellLvl = "3rd";
            if (lvl > 13)
                spellLvl = "4th";

            GroupBox box = new GroupBox();
            box.Text = "Studious Spells";
            Label label = new Label();
            label.Text = $"You have two special {spellLvl} level spell slots, which can be used to cast only certain spells.\n" +
                $"  - Invisibility\n" +
                $"  - Mirror Image\n" +
                $"  - Spider Climb\n";
            switch (HybridStudy)
            {
                case "Inexorable Iron":
                    label.Text += "  - Enlarge / Reduce (Enlarge Only)\n";
                    if (level >= 11)
                        label.Text += "  - Spirit Shroud\n";
                    if (level >= 13)
                        label.Text += "  - Fire Shield\n";
                    break;
                case "Starlit Span":
                    label.Text += "  - Darkvision\n";
                    if (level >= 11)
                        label.Text += "  - Wind Wall\n";
                    if (level >= 13)
                        label.Text += "  - Freedom of Movement\n";
                    break;
                case "Laughing Shadow":
                    label.Text += "  - Suggestion\n";
                    if (level >= 11)
                        label.Text += "  - Fly\n";
                    if (level >= 13)
                        label.Text += "  - Greater Invisibility\n";
                    break;
                case "Sparkling Targe":
                    label.Text += "  - Hold Person\n";
                    if (level >= 11)
                        label.Text += "  - Protection from Energy\n";
                    if (level >= 13)
                        label.Text += "  - Stoneskin\n";
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
        private GroupBox SpellTwin()
        {
            GroupBox box = new GroupBox();
            box.Text = "Spell-Twinned Attack";
            Label label = new Label();
            label.Text = $"You can attack twice, instead of once, whenever you take the Attack action on your turn. Moreover, you can cast one of your cantrips or make a Spellstrike in place of one of those attacks (but not both).";
            label.MaximumSize = new Size(168, int.MaxValue);
            label.AutoSize = true;
            box.Controls.Add(label);
            label.Location = new Point(6, 12);
            box.Size = new Size(180, label.Size.Height + 18);
            label.MouseDown += DisplayOnRightClick;
            return box;
        }
        private GroupBox DoubleStrike()
        {
            GroupBox box = new GroupBox();
            box.Text = "Doubled Spellstrike";
            Label label = new Label();
            label.Text = $"After you make a Spellstrike with a spell cast from a spell slot, you retain an echo of the spell, stored in your body. The next time you Spellstrike before taking a short or long rest, you can cast the same spell again without expending a spell slot. \n If you choose to cast a different spell with Spellstrike, the stored spell dissipates forcefully, dealing an amount of psychic damage equal to the level of the spell slot used to cast it to you.";
            label.MaximumSize = new Size(168, int.MaxValue);
            label.AutoSize = true;
            box.Controls.Add(label);
            label.Location = new Point(6, 12);
            box.Size = new Size(180, label.Size.Height + 18);
            label.MouseDown += DisplayOnRightClick;
            return box;
        }

        public override List<GroupBox> getInfoBoxes()
        {
            var infoBoxes = new List<GroupBox>();
            if (level >= 1)
            {
                infoBoxes.Add(AddWarcasting(level));
                infoBoxes.Add(AddCascade());
                infoBoxes.Add(AddSpellstrike());
            }
            if (level >= 2)
            {
                infoBoxes.Add(AddSubclass());
            }
            if (level >= 3)
            {
                infoBoxes.Add(AddConflux(level));
            }
            if (level >= 4)
            {
                infoBoxes.Add(ASIBox(featList[0]));
            }
            if (level >= 7)
            {
                infoBoxes.Add(StudiousSpells(level));
            }
            if (level >= 11)
            {
                infoBoxes.Add(SpellTwin());
            }
            if (level >= 20)
            {
                infoBoxes.Add(DoubleStrike());
            }
            return infoBoxes;
        }
        public static Magus fromYAML(string fName, int[] modifiers, int lvl, int prof)
        {
            Magus returned = null;
            using (FileStream fin = File.OpenRead(fName))
            {
                TextReader reader = new StreamReader(fin);

                var deserializer = new Deserializer();
                returned = deserializer.Deserialize<Magus>(reader);
            }
            returned.abilityModifiers = modifiers;
            returned.SpellcastingAbilityModifier = SpellMod.INT;
            returned.level = lvl;
            returned.proficiency = prof;
            returned.SpellPrepLevel = returned.level / 2;
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
                    returned.spellSlotsMax = new int[] { 2, 1, 0, 0, 0, 0, 0, 0, 0 };
                    break;
                case 4:
                    returned.spellSlotsMax = new int[] { 2, 2, 0, 0, 0, 0, 0, 0, 0 };
                    break;
                case 5:
                    returned.spellSlotsMax = new int[] { 0, 2, 2, 0, 0, 0, 0, 0, 0 };
                    break;
                case 6:
                    returned.spellSlotsMax = new int[] { 0, 2, 2, 0, 0, 0, 0, 0, 0 };
                    break;
                case 7:
                    returned.spellSlotsMax = new int[] { 0, 0, 2, 2, 0, 0, 0, 0, 0 };
                    break;
                case 8:
                    returned.spellSlotsMax = new int[] { 0, 0, 2, 2, 0, 0, 0, 0, 0 };
                    break;
                case 9:
                    returned.spellSlotsMax = new int[] { 0, 0, 0, 2, 2, 0, 0, 0, 0 };
                    break;
                case 10:
                    returned.spellSlotsMax = new int[] { 0, 0, 0, 2, 2, 0, 0, 0, 0 };
                    break;
                case 11:
                    returned.spellSlotsMax = new int[] { 0, 0, 0, 0, 2, 2, 0, 0, 0 };
                    break;
                case 12:
                    returned.spellSlotsMax = new int[] { 0, 0, 0, 0, 2, 2, 0, 0, 0 };
                    break;
                case 13:
                    returned.spellSlotsMax = new int[] { 0, 0, 0, 0, 0, 2, 2, 0, 0 };
                    break;
                case 14:
                    returned.spellSlotsMax = new int[] { 0, 0, 0, 0, 0, 2, 2, 0, 0 };
                    break;
                case 15:
                    returned.spellSlotsMax = new int[] { 0, 0, 0, 0, 0, 0, 2, 2, 0 };
                    break;
                case 16:
                    returned.spellSlotsMax = new int[] { 0, 0, 0, 0, 0, 0, 2, 2, 0 };
                    break;
                case 17:
                    returned.spellSlotsMax = new int[] { 0, 0, 0, 0, 0, 0, 0, 2, 2 };
                    break;
                case 18:
                    returned.spellSlotsMax = new int[] { 0, 0, 0, 0, 0, 0, 0, 2, 2 };
                    break;
                case 19:
                    returned.spellSlotsMax = new int[] { 0, 0, 0, 0, 0, 0, 0, 2, 2 };
                    break;
                case 20:
                    returned.spellSlotsMax = new int[] { 0, 0, 0, 0, 0, 0, 0, 2, 2 };
                    break;
                default:
                    returned.spellSlotsMax = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    break;
            }
            returned.getInfo();
            return returned;
        }
    }
}