using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MUD.Ability;
using MUD.Net;

namespace MUD.Items
{
    public class Item
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ItemType Type { get; private set; }
        public bool Stackable { get; private set; }

        public Passive AppliesPassive { get; private set; }
        public Active AppliesActive { get; private set; }

        public int HealthIncrease { get; private set; }
        public int HealthOnKill { get; private set; }
        public double PercentageHealthOnHit { get; private set; }
        public int FlatHealthOnHit { get; private set; }

        public double CritChance { get; private set; }
        public double CritDamage { get; private set; }

        public double Resist { get; private set; }
        public double BleedResist { get; private set; }
        public double PoisonResist { get; private set; }

        public int FlatThorns { get; private set; }

        public double AllDamage { get; private set; }
        public double BaseDamage { get; private set; }
        public double SkillDamage { get; private set; }
        public double PosionDamage { get; private set; }
        public double BleedDamage { get; private set; }
        public double MarkDamage { get; private set; }

        public double WhilePoisonDamage { get; private set; }
        public double WhileBleedDamage { get; private set; }
        public double WhileMarkDamage { get; private set; }

        public double HealingOutgoing { get; private set; }
        public double HealingIncoming { get; private set; }

        public double DamageWhileMaxHealth { get; private set; }
        public double DamageWhileLowHealth { get; private set; }

        public Item(string name, string desc, ItemType type, bool stackable,
            Passive passive = null, Active active = null,
            int health = 0, int healthOnKill = 0, double percentageHealthOnHit = 0, int flatHealthOnHit = 0, 
            double critChance = 0, double critDamage = 0, 
            double resist = 0, double bleedResist = 0, double posionResist = 0, 
            int flatThorns = 0,
            double allDamage = 0, double baseDamage = 0, double skillDamage = 0, 
            double poisonDamage = 0, double bleedDamage = 0, double markDamage = 0,
            double whilePoisonDamage = 0, double whileBleedDamage = 0, double whileMarkDamage = 0,
            double healingOutgoing = 0, double healingIncoming = 0,
            double damageWhileMaxHealth = 0, double damageWhileLowHealth = 0)
        {
            Name = name;
            Description = desc;
            Type = type;
            Stackable = stackable;

            AppliesPassive = passive;
            AppliesActive = active;

            HealthIncrease = health;
            HealthOnKill = healthOnKill;
            PercentageHealthOnHit = percentageHealthOnHit;
            FlatHealthOnHit = flatHealthOnHit;

            CritChance = critChance;
            CritDamage = critDamage;

            Resist = resist;
            BleedResist = bleedResist;
            PoisonResist = posionResist;

            FlatThorns = flatThorns;

            AllDamage = allDamage;
            BaseDamage = baseDamage;
            SkillDamage = skillDamage;
            PosionDamage = poisonDamage;
            BleedDamage = bleedDamage;
            MarkDamage = markDamage;

            WhilePoisonDamage = whilePoisonDamage;
            WhileBleedDamage = whileBleedDamage;
            WhileMarkDamage = whileMarkDamage;

            HealingIncoming = healingIncoming;
            HealingOutgoing = healingOutgoing;

            DamageWhileMaxHealth = damageWhileMaxHealth;
            DamageWhileLowHealth = damageWhileLowHealth;
        }

        public NetItem Net()
        {
            return NetItem.CreateItem(Name, Description, Type, Stackable, 
                AppliesPassive.Net(), AppliesActive.Net(), 
                HealthIncrease, HealthOnKill, PercentageHealthOnHit, FlatHealthOnHit, 
                CritChance, CritDamage, 
                Resist, BleedResist, PoisonResist, FlatThorns, 
                AllDamage, BaseDamage, SkillDamage, PosionDamage, BleedDamage, MarkDamage, 
                WhilePoisonDamage, WhileBleedDamage, WhileMarkDamage, 
                HealingOutgoing, HealingIncoming, 
                DamageWhileMaxHealth, DamageWhileLowHealth);
        }
    }
}
