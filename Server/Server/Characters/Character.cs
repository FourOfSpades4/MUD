using DarkRift;
using MUD;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUD.Ability;
using MUD.Items;

namespace MUD.Characters
{
    public abstract class Character
    {
        public string Name { get; protected set; }
        public Passive[] Passives { get; protected set; }
        public Active[] Actives { get; protected set; }
        public int Health { get; protected set; }
        public Item[] Armor { get; protected set; }
        public CharacterType Type { get; protected set; }

        public Character()
        {
            Health = 100;
            Passives = new Passive[Settings.passiveSlots];
            Actives = new Active[Settings.activeSlots];
            Armor = new Item[Settings.armorSlots];
        }

        public virtual bool Equip(int index, Item item)
        {
            if (index >= 0 && index < Armor.Length)
            {
                Armor[index] = item;
                return true;
            }
            return false;
        }

        public virtual bool Equip(int index, Passive item)
        {
            if (index >= 0 && index < Passives.Length)
            {
                Passives[index] = item;
                return true;
            }
            return false;
        }

        public virtual bool Equip(int index, Active item)
        {
            if (index >= 0 && index < Actives.Length)
            {
                Actives[index] = item;
                return true;
            }
            return false;
        }

        public virtual int TakeDamage(int damage)
        {
            Health -= damage;

            return damage;
        }
    }
}
