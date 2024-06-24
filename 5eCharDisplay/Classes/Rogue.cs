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
    internal class Rogue : charClass
    {
        public string Skill1 { set; get; }
        public string Skill2 { set; get; }
        public string Skill3 { set; get; }
        public string Skill4 { set; get; }
        public string Expertise1 { set; get; }
        public string Expertise2 { set; get; }
        public string RoguishArchetype { get; set; }

        public Rogue()
        {
            hitDie = new Die(8);
            armorProfs = new List<string> { "Light Armor" };
            weaponProfs = new List<string> { "Simple Weapons", "Hand Crossbow", "Longsword", "Rapier", "Shortsword" };
            toolProfs = new List<string> { "Thieves' Tools" };
            SavingProfs = new string[2] { "DexSave", "IntSave" };
            languages = new List<string> { "Thieves' Cant" };
        }
        public override List<GroupBox> getInfoBoxes()
        {
            var infoBoxes = new List<GroupBox>();
            if (level >= 1)
            {
                infoBoxes.Add(AddExpertiseBox());
                infoBoxes.Add(AddSneakAttackBox());
            }
            if (level >= 2)
            {
                infoBoxes.Add(AddCunningActionBox());
            }
            if (level >= 3)
            {
                infoBoxes.Add(AddSubclassBox());
            }
            if (level >= 4)
            {

            }
            if (level >= 7)
            {

            }
            if (level >= 11)
            {

            }
            if (level >= 20)
            {

            }
            return infoBoxes;
        }
        private GroupBox AddExpertiseBox()
        {
            GroupBox box = new GroupBox();
            box.Text = "Expertise";
            Label label = new Label();
            label.Text = "You gain expertise in two skills of your choice that you already have proficiency in, or one of those skills and your proficiency in thieves' tools.";
            label.MaximumSize = new Size(168, int.MaxValue);
            label.AutoSize = true;
            box.Controls.Add(label);
            label.Location = new Point(6, 12);
            box.Size = new Size(180, label.Size.Height + 18);
            label.MouseDown += DisplayOnRightClick;
            return box;
        }
        private GroupBox AddSneakAttackBox()
        {
            GroupBox box = new GroupBox();
            box.Text = "Sneak Attack";
            Label label = new Label();
            label.Text = $"You know how to strike subtly and exploit a foe’s distraction. Once per turn, you can deal an extra {Math.Ceiling(level/2.0)}d6 damage to one creature you hit with an attack if you have advantage on the attack roll. The attack must use a finesse or a ranged weapon. You don’t need advantage on the attack roll if another enemy of the target is within 5 feet of it, that enemy isn’t incapacitated, and you don’t have disadvantage on the attack roll.";
            label.MaximumSize = new Size(168, int.MaxValue);
            label.AutoSize = true;
            box.Controls.Add(label);
            label.Location = new Point(6, 12);
            box.Size = new Size(180, label.Size.Height + 18);
            label.MouseDown += DisplayOnRightClick;
            return box;
        }
        private GroupBox AddCunningActionBox()
        {
            GroupBox box = new GroupBox();
            box.Text = "Cunning Action";
            Label label = new Label();
            label.Text = $"Your quick thinking and agility allow you to move and act quickly. You can take a bonus action on each of your turns in combat. This action can be used only to take the Dash, Disengage, or Hide action.";
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
            box.Text = "Roguish Archetype";
            Label label = new Label();
            switch (RoguishArchetype)
            {
                case "Assassin":
                    toolProfs.Add("Disguise Kit");
                    toolProfs.Add("Poisoner's Kit");
                    label.Text += $"  - Assassinate\n   - You are at your deadliest when you get the drop on your enemies. You have advantage on attack rolls against any creature that hasn’t taken a turn in the combat yet. In addition, any hit you score against a creature that is surprised is a critical hit.\n";
                    if (level >= 9)
                    {
                        label.Text += $"  - Infiltration Expertise\n   -  You can unfailingly create false identities for yourself. You must spend seven days and 25 gp to establish the history, profession, and affiliations for an identity. You can’t establish an identity that belongs to someone else. For example, you might acquire appropriate clothing, letters of introduction, and official-looking certification to establish yourself as a member of a trading house from a remote city so you can insinuate yourself into the company of other wealthy merchants. Thereafter, if you adopt the new identity as a disguise, other creatures believe you to be that person until given an obvious reason not to.\n";
                    }
                    if (level >= 13)
                    {
                        label.Text += $"  - Impostor\n   - You gain the ability to unerringly mimic another person’s speech, writing, and behavior. You must spend at least three hours studying these three components of the person’s behavior, listening to speech, examining handwriting, and observing mannerisms. Your ruse is indiscernible to the casual observer. If a wary creature suspects something is amiss, you have advantage on any Charisma(Deception) check you make to avoid detection.\n";
                    }
                    if (level >= 17)
                    {
                        label.Text += $"  - Death Strike\n   - you become a master of instant death. When you attack and hit a creature that is surprised, it must make a DC {8 + abilityModifiers[1] + (int)Math.Ceiling(level / 4.0)} Constitution saving throw. On a failed save, double the damage of your attack against the creature.\n";
                    }
                    break;
                case "Arcane Trickster":
                    Spellcasting = true;
                    SpellcastingAbilityModifier = SpellMod.INT;
                    switch (level)
                    {
                        case 3:
                            spellSlotsMax = new int[] { 2, 0, 0, 0, 0, 0, 0, 0, 0 };
                            break;
                        case 4:
                            spellSlotsMax = new int[] { 3, 0, 0, 0, 0, 0, 0, 0, 0 };
                            break;
                        case 5:
                            spellSlotsMax = new int[] { 3, 0, 0, 0, 0, 0, 0, 0, 0 };
                            break;
                        case 6:
                            spellSlotsMax = new int[] { 3, 0, 0, 0, 0, 0, 0, 0, 0 };
                            break;
                        case 7:
                            spellSlotsMax = new int[] { 4, 2, 0, 0, 0, 0, 0, 0, 0 };
                            break;
                        case 8:
                            spellSlotsMax = new int[] { 4, 2, 0, 0, 0, 0, 0, 0, 0 };
                            break;
                        case 9:
                            spellSlotsMax = new int[] { 4, 2, 0, 0, 0, 0, 0, 0, 0 };
                            break;
                        case 10:
                            spellSlotsMax = new int[] { 4, 3, 0, 0, 0, 0, 0, 0, 0 };
                            break;
                        case 11:
                            spellSlotsMax = new int[] { 4, 3, 0, 0, 0, 0, 0, 0, 0 };
                            break;
                        case 12:
                            spellSlotsMax = new int[] { 4, 3, 0, 0, 0, 0, 0, 0, 0 };
                            break;
                        case 13:
                            spellSlotsMax = new int[] { 4, 3, 2, 0, 0, 0, 0, 0, 0 };
                            break;
                        case 14:
                            spellSlotsMax = new int[] { 4, 3, 2, 0, 0, 0, 0, 0, 0 };
                            break;
                        case 15:
                            spellSlotsMax = new int[] { 4, 3, 2, 0, 0, 0, 0, 0, 0 };
                            break;
                        case 16:
                            spellSlotsMax = new int[] { 4, 3, 3, 0, 0, 0, 0, 0, 0 };
                            break;
                        case 17:
                            spellSlotsMax = new int[] { 4, 3, 3, 0, 0, 0, 0, 0, 0 };
                            break;
                        case 18:
                            spellSlotsMax = new int[] { 4, 3, 3, 0, 0, 0, 0, 0, 0 };
                            break;
                        case 19:
                            spellSlotsMax = new int[] { 4, 3, 3, 1, 0, 0, 0, 0, 0 };
                            break;
                        case 20:
                            spellSlotsMax = new int[] { 4, 3, 3, 1, 0, 0, 0, 0, 0 };
                            break;
                    }
                    label.Text += $"  - Spellcasting\n   - You gain the ability to cast spells.\n";
                    label.Text += $"  - Mage Hand Legerdemain\n   - When you cast mage hand, you can make the spectral hand invisible, and you can perform the following additional tasks with it:\n" +
                        $"    - You can stow one object the hand is holding in a container worn or carried by another creature.\n" +
                        $"    - You can retrieve an object in a container worn or carried by another creature.\n" +
                        $"    - You can use thieves’ tools to pick locks and disarm traps at range.\n" +
                        $"   - You can perform one of these tasks without being noticed by a creature if you succeed on a Dexterity (Sleight of Hand) check contested by the creature’s Wisdom (Perception) check. In addition, you can use the bonus action granted by your Cunning Action to control the hand.\n";
                    if (level >= 9)
                    {
                        label.Text += $"  - Magical Ambush\n   - If you are hidden from a creature when you cast a spell on it, the creature has disadvantage on any saving throw it makes against the spell this turn.\n";
                    }
                    if (level >= 13)
                    {
                        label.Text += $"  - Versatile Trickster\n   - You gain the ability to distract targets with your mage hand. As a bonus action on your turn, you can designate a creature within 5 feet of the spectral hand created by the spell. Doing so gives you advantage on attack rolls against that creature until the end of the turn.\n";
                    }
                    if (level >= 17)
                    {
                        label.Text += $"  - Spell Thief\n   - Immediately after a creature casts a spell that targets you or includes you in its area of effect, you can use your reaction to force the creature to make a saving throw with its spellcasting ability modifier. The DC for the save is {8 + abilityModifiers[1] + (int)Math.Ceiling(level / 4.0)} your spell save DC. On a failed save, you negate the spell’s effect against you, and you steal the knowledge of the spell if it is at least 1st level and of a level you can cast (it doesn’t need to be a wizard spell). For the next 8 hours, you know the spell and can cast it using your spell slots. The creature can’t cast that spell until the 8 hours have passed. Once you use this feature, you can’t use it again until you finish a long rest.\n";
                    }
                    break;
                case "Thief":
                    label.Text += $"  - Fast Hands\n   - You can use the bonus action granted by your Cunning Action to make a Dexterity (Sleight of Hand) check, use your thieves’ tools to disarm a trap or open a lock, or take the Use an Object action.\n";
                    label.Text += $"  - Second-Story Work\n   - You gain the ability to climb faster than normal; climbing no longer costs you extra movement. In addition, when you make a running jump, the distance you cover increases by a number of feet equal to your Dexterity modifier.\n";
                    if (level >= 9)
                    {
                        label.Text += $"  - Supreme Sneak\n   - You have advantage on a Dexterity (Stealth) check if you move no more than half your speed on the same turn.\n";
                    }
                    if (level >= 13)
                    {
                        label.Text += $"  - Use Magic Device\n   - You have learned enough about the workings of magic that you can improvise the use of items even when they are not intended for you. You ignore all class, race, and level requirements on the use of magic items.\n";
                    }
                    if (level >= 17)
                    {
                        label.Text += $"  - Thief’s Reflexes\n   - You have become adept at laying ambushes and quickly escaping danger. You can take two turns during the first round of any combat. You take your first turn at your normal initiative and your second turn at your initiative minus 10. You can’t use this feature when you are surprised.\n";
                    }
                    break;
                case "Soulknife":
                    int die = 6;
                    if (level >= 17) die = 12;
                    if (level >= 11) die = 10;
                    if (level >= 5) die = 8;
                    label.Text += $"  - Psionic Power\n   - You harbor a wellspring of psionic energy within yourself. This energy is represented by your Psionic Energy dice, which are each a d{die}. You have {(int)Math.Ceiling(level / 4.0) + 1} dice, and they fuel various psionic powers you have, which are detailed below.\n" +
                        $"   - Some of your powers expend the Psionic Energy die they use, as specified in a power’s description, and you can’t use a power if it requires you to use a die when your dice are all expended. You regain all your expended Psionic Energy dice when you finish a long rest. In addition, as a bonus action, you can regain one expended Psionic Energy die, but you can’t do so again until you finish a short or long rest.\n" +
                        $"     The powers below use your Psionic Energy dice.\n\n" +
                        $"    - Psi-Bolstered Knack. When your nonpsionic training fails you, your psionic power can help: if you fail an ability check using a skill or tool with which you have proficiency, you can roll one Psionic Energy die and add the number rolled to the check, potentially turning failure into success. You expend the die only if the roll succeeds.\n" +
                        $"    - Psychic Whispers. You can establish telepathic communication between yourself and others—perfect for quiet infiltration. As an action, choose one or more creatures you can see, up to a number of creatures equal to your proficiency bonus, and then roll one Psionic Energy die. For a number of hours equal to the number rolled, the chosen creatures can speak telepathically with you, and you can speak telepathically with them. To send or receive a message (no action required), you and the other creature must be within 1 mile of each other. A creature can’t use this telepathy if it can’t speak any languages, and a creature can end the telepathic connection at any time (no action required). You and the creature don’t need to speak a common language to understand each other.\n" +
                        $"      The first time you use this power after each long rest, you don’t expend the Psionic Energy die. All other times you use the power, you expend the die.\n\n";
                    label.Text += $"  - Psychic Blades\n   - You can manifest your psionic power as shimmering blades of psychic energy. Whenever you take the Attack action, you can manifest a psychic blade from your free hand and make the attack with that blade. This magic blade is a simple melee weapon with the finesse and thrown properties. It has a normal range of 60 feet and no long range, and on a hit, it deals psychic damage equal to 1d6 plus the ability modifier you used for the attack roll. The blade vanishes immediately after it hits or misses its target, and it leaves no mark on its target if it deals damage.\n   - After you attack with the blade, you can make a melee or ranged weapon attack with a second psychic blade as a bonus action on the same turn, provided your other hand is free to create it. The damage die of this bonus attack is 1d4, instead of 1d6.";
                    if (level >= 9)
                    {
                        label.Text += $"  - Soul Blades\n   - Your Psychic Blades are now an expression of your psi-suffused soul, giving you these powers that use your Psionic Energy dice:\n\n    - Homing Strikes. If you make an attack roll with your Psychic Blades and miss the target, you can roll one Psionic Energy die and add the number rolled to the attack roll. If this causes the attack to hit, you expend the Psionic Energy die.\n    - Psychic Teleportation. As a bonus action, you manifest one of your Psychic Blades, expend one Psionic Energy die and roll it, and throw the blade at an unoccupied space you can see, up to a number of feet away equal to 10 times the number rolled. You then teleport to that space, and the blade vanishes.";
                    }
                    if (level >= 13)
                    {
                        label.Text += $"  - Psychic Veil\n   - You can weave a veil of psychic static to mask yourself. As an action, you can magically become invisible, along with anything you are wearing or carrying, for 1 hour or until you dismiss this effect (no action required). This invisibility ends early immediately after you deal damage to a creature or you force a creature to make a saving throw.\n     Once you use this feature, you can’t do so again until you finish a long rest, unless you expend a Psionic Energy die to use this feature again.";
                    }
                    if (level >= 17)
                    {
                        label.Text += $"  - Rend Mind\n   - You can sweep your Psychic Blades directly through a creature’s mind. When you use your Psychic Blades to deal Sneak Attack damage to a creature, you can force that target to make a Wisdom saving throw (DC equal to 8 + your proficiency bonus + your Dexterity modifier). If the save fails, the target is stunned for 1 minute. The stunned target can repeat the saving throw at the end of each of its turns, ending the effect on itself on a success.\n     Once you use this feature, you can’t do so again until you finish a long rest, unless you expend three Psionic Energy dice to use it again.";
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
        public static Rogue fromYAML(string fName, int[] modifiers, int lvl, int prof)
        {
            Rogue returned = null;
            using (FileStream fin = File.OpenRead(fName))
            {
                TextReader reader = new StreamReader(fin);

                var deserializer = new Deserializer();
                returned = deserializer.Deserialize<Rogue>(reader);
            }
            returned.abilityModifiers = modifiers;
            returned.proficiency = prof;
            returned.level = lvl;
            returned.HDrem = returned.level;
            returned.skillProfs.Add(returned.Skill1);
            returned.skillProfs.Add(returned.Skill2);
            returned.skillProfs.Add(returned.Skill3);
            returned.skillProfs.Add(returned.Skill4);
            returned.expertise.Add(returned.Expertise1);
            returned.expertise.Add(returned.Expertise2);
            returned.Spellcasting = false;
            returned.getInfo();
            return returned;
        }

    }
}
