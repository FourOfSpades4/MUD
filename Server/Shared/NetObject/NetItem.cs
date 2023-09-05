using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MUD.Net
{
    public class NetItem : IDarkRiftSerializable
    {
        public enum ItemType
        {
            CHEST,
            LEGGINGS,
            HELM,
            BOOTS
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public ItemType Type { get; private set; }
        public bool Stackable { get; private set; }

        public NetPassive AppliesPassive { get; private set; }
        public NetActive AppliesActive { get; private set; }

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

        public static NetItem CreateItem(string name, string desc, ItemType type, bool stackable,
            NetPassive passive = null, NetActive active = null,
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
            NetItem item = new NetItem();

            item.Name = name;
            item.Description = desc;
            item.Type = type;
            item.Stackable = stackable;

            item.AppliesPassive = passive;
            item.AppliesActive = active;

            item.HealthIncrease = health;
            item.HealthOnKill = healthOnKill;
            item.PercentageHealthOnHit = percentageHealthOnHit;
            item.FlatHealthOnHit = flatHealthOnHit;

            item.CritChance = critChance;
            item.CritDamage = critDamage;

            item.Resist = resist;
            item.BleedResist = bleedResist;
            item.PoisonResist = posionResist;

            item.FlatThorns = flatThorns;

            item.AllDamage = allDamage;
            item.BaseDamage = baseDamage;
            item.SkillDamage = skillDamage;
            item.PosionDamage = poisonDamage;
            item.BleedDamage = bleedDamage;
            item.MarkDamage = markDamage;

            item.WhilePoisonDamage = whilePoisonDamage;
            item.WhileBleedDamage = whileBleedDamage;
            item.WhileMarkDamage = whileMarkDamage;

            item.HealingIncoming = healingIncoming;
            item.HealingOutgoing = healingOutgoing;

            item.DamageWhileMaxHealth = damageWhileMaxHealth;
            item.DamageWhileLowHealth = damageWhileLowHealth;

            return new NetItem();
        }
        public virtual void Deserialize(DeserializeEvent e)
        {
            Name = e.Reader.ReadString();
            Description = e.Reader.ReadString();
            Type = (ItemType)e.Reader.ReadInt32();
            Stackable = e.Reader.ReadBoolean();

            AppliesPassive = e.Reader.ReadSerializable<NetPassive>();
            AppliesActive = e.Reader.ReadSerializable<NetActive>();

            HealthIncrease = e.Reader.ReadInt32();
            HealthOnKill = e.Reader.ReadInt32();
            PercentageHealthOnHit = e.Reader.ReadDouble();
            FlatHealthOnHit = e.Reader.ReadInt32();

            CritChance = e.Reader.ReadDouble();
            CritDamage = e.Reader.ReadDouble();

            Resist = e.Reader.ReadDouble();
            BleedResist = e.Reader.ReadDouble();
            PoisonResist = e.Reader.ReadDouble();

            FlatThorns = e.Reader.ReadInt32();

            AllDamage = e.Reader.ReadDouble();
            BaseDamage = e.Reader.ReadDouble();
            SkillDamage = e.Reader.ReadDouble();
            PosionDamage = e.Reader.ReadDouble();
            BleedDamage = e.Reader.ReadDouble();
            MarkDamage = e.Reader.ReadDouble();

            WhilePoisonDamage = e.Reader.ReadDouble();
            WhileBleedDamage = e.Reader.ReadDouble();
            WhileMarkDamage = e.Reader.ReadDouble();

            HealingIncoming = e.Reader.ReadDouble();
            HealingOutgoing = e.Reader.ReadDouble();

            DamageWhileMaxHealth = e.Reader.ReadDouble();
            DamageWhileLowHealth = e.Reader.ReadDouble();
        }

        public virtual void Serialize(SerializeEvent e)
        {
            e.Writer.Write(Name);
            e.Writer.Write(Description);
            e.Writer.Write((int)Type);
            e.Writer.Write(Stackable);

            e.Writer.Write(AppliesPassive);
            e.Writer.Write(AppliesActive);

            e.Writer.Write(HealthIncrease);
            e.Writer.Write(HealthOnKill);
            e.Writer.Write(PercentageHealthOnHit);
            e.Writer.Write(FlatHealthOnHit);

            e.Writer.Write(CritChance);
            e.Writer.Write(CritDamage);

            e.Writer.Write(Resist);
            e.Writer.Write(BleedResist);
            e.Writer.Write(PoisonResist);

            e.Writer.Write(FlatThorns);

            e.Writer.Write(AllDamage);
            e.Writer.Write(BaseDamage);
            e.Writer.Write(SkillDamage);
            e.Writer.Write(PosionDamage);
            e.Writer.Write(BleedDamage);
            e.Writer.Write(MarkDamage);

            e.Writer.Write(WhilePoisonDamage);
            e.Writer.Write(WhileBleedDamage);
            e.Writer.Write(WhileMarkDamage);

            e.Writer.Write(HealingIncoming);
            e.Writer.Write(HealingOutgoing);

            e.Writer.Write(DamageWhileMaxHealth);
            e.Writer.Write(DamageWhileLowHealth);
        }
    }
}
