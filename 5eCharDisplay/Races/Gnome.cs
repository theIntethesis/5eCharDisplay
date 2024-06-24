using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5eCharDisplay
{
    internal class Gnome : charRace
    {
        public Gnome(string subrace)
        {
            speed = 25;
            IntBoost = 2;
            languages = new List<string> { "Common", "Gnomish" };
            abilities = new List<string> { "Darkvision", " - You can see in dim light within 60 feet of you as if it were bright light, and in darkness as if it were dim light.\n\n", "Gnome Cunning\n - You have advantage on all Intelligence, Wisdom, and Charisma saving throws against magic.\n\n" };
            switch (subrace)
            {
                case "Deep Gnome":
                    DexBoost = 1;
                    abilities.Add("Superior Darkvision");
                    abilities.Add(" - Your darkvision has a radius of 120 feet.");
                    abilities.Add("Stone Camouflage");
                    abilities.Add(" - You have advantage on Dexterity (Stealth) checks to hide in rocky terrain.");
                    languages.Add("Undercommon");
                    break;
                case "Forest Gnome":
                    DexBoost = 1;
                    abilities.Add("Natural Illusionist");
                    abilities.Add(" - You know the minor illusion cantrip. Intelligence is your spellcasting ability for it.");
                    abilities.Add("Speak with Small Beasts");
                    abilities.Add(" - Through sounds and gestures, you can communicate simple ideas with Small or smaller beasts. Forest gnomes love animals and often keep squirrels, badgers, rabbits, moles, woodpeckers, and other creatures as beloved pets.");
                    break;
                case "Rock Gnome":
                    ConBoost = 1;
                    abilities.Add("Artificer's Lore");
                    abilities.Add(" - Whenever you make an Intelligence (History) check related to magic items, alchemical objects, or technological devices, you can add twice your proficiency bonus, instead of any proficiency bonus you normally apply.");
                    abilities.Add("Tinker");
                    abilities.Add(" - You have proficiency with artisan’s tools (tinker’s tools). Using those tools, you can spend 1 hour and 10 gp worth of materials to construct a Tiny clockwork device (AC 5, 1 hp). The device ceases to function after 24 hours (unless you spend 1 hour repairing it to keep the device functioning), or when you use your action to dismantle it; at that time, you can reclaim the materials used to create it. You can have up to three such devices active at a time.\n   When you create a device, choose one of the following options:\n   Clockwork Toy. This toy is a clockwork animal, monster, or person, such as a frog, mouse, bird, dragon, or soldier. When placed on the ground, the toy moves 5 feet across the ground on each of your turns in a random direction. It makes noises as appropriate to the creature it represents.\n   Fire Starter. The device produces a miniature flame, which you can use to light a candle, torch, or campfire. Using the device requires your action.\n   Music Box. When opened, this music box plays a single song at a moderate volume. The box stops playing when it reaches the song’s end or when it is closed.");
                    toolProfs.Add("Tinker's Tools");
                    break;
            }
        }
    }
}
