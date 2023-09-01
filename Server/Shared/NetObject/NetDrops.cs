using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUD.Net
{
    public class NetDrops : IDarkRiftSerializable
    {
        public ItemDrop[] Items { get; protected set; }
        public PassiveDrop[] Passives { get; protected set; }
        public ActiveDrop[] Actives { get; protected set; }

        public static NetDrops CreateDrops(ItemDrop[] items,
            PassiveDrop[] passives, ActiveDrop[] actives)
        {
            NetDrops d = new NetDrops();

            d.Items = items;
            d.Passives = passives;
            d.Actives = actives;

            return d;
        }

        public virtual void Deserialize(DeserializeEvent e)
        {
            Items = e.Reader.ReadSerializables<ItemDrop>();
            Passives = e.Reader.ReadSerializables<PassiveDrop>();
            Actives = e.Reader.ReadSerializables<ActiveDrop>();
        }

        public virtual void Serialize(SerializeEvent e)
        {
            e.Writer.Write(Items);
            e.Writer.Write(Passives);
            e.Writer.Write(Actives);
        }

        public NetDrops GenerateDrops()
        {
            Random random = new Random();

            List<ItemDrop> items = new List<ItemDrop>();
            List<ActiveDrop> actives = new List<ActiveDrop>();
            List<PassiveDrop> passives = new List<PassiveDrop>();
            
            foreach (ItemDrop item in Items)
            {
                var n = random.NextDouble();
                if (item.Chance > n)
                    items.Add(ItemDrop.CreateItemDrop(item.Item, 1));
            }

            foreach (PassiveDrop passive in Passives)
            {
                var n = random.NextDouble();
                if (passive.Chance > n)
                    passives.Add(PassiveDrop.CreatePassiveDrop(passive.Passive, 1));
            }

            foreach (ActiveDrop active in Actives)
            {
                var n = random.NextDouble();
                if (active.Chance > n)
                    actives.Add(ActiveDrop.CreateActiveDrop(active.Active, 1));
            }

            return CreateDrops(items.ToArray(), passives.ToArray(), actives.ToArray());
        }
    }

    public class ItemDrop : IDarkRiftSerializable
    {
        public NetItem Item { get; private set; }
        public double Chance { get; private set; }

        public static ItemDrop CreateItemDrop(NetItem item, double chance)
        {
            ItemDrop d = new ItemDrop();

            d.Item = item;
            d.Chance = chance;

            return d;
        }

        public virtual void Deserialize(DeserializeEvent e)
        {
            Item = e.Reader.ReadSerializable<NetItem>();
            Chance = e.Reader.ReadDouble();
        }

        public virtual void Serialize(SerializeEvent e)
        {
            e.Writer.Write(Item);
            e.Writer.Write(Chance);
        }
    }

    public class PassiveDrop : IDarkRiftSerializable
    {
        public Passive Passive { get; private set; }
        public double Chance { get; private set; }

        public static PassiveDrop CreatePassiveDrop(Passive passive, double chance)
        {
            PassiveDrop d = new PassiveDrop();

            d.Passive = passive;
            d.Chance = chance;

            return d;
        }

        public virtual void Deserialize(DeserializeEvent e)
        {
            Passive = e.Reader.ReadSerializable<Passive>();
            Chance = e.Reader.ReadDouble();
        }

        public virtual void Serialize(SerializeEvent e)
        {
            e.Writer.Write(Passive);
            e.Writer.Write(Chance);
        }
    }

    public class ActiveDrop : IDarkRiftSerializable
    {
        public Active Active { get; private set; }
        public double Chance { get; private set; }

        public static ActiveDrop CreateActiveDrop(Active active, double chance)
        {
            ActiveDrop d = new ActiveDrop();

            d.Active = active;
            d.Chance = chance;

            return d;
        }

        public virtual void Deserialize(DeserializeEvent e)
        {
            Active = e.Reader.ReadSerializable<Active>();
            Chance = e.Reader.ReadDouble();
        }

        public virtual void Serialize(SerializeEvent e)
        {
            e.Writer.Write(Active);
            e.Writer.Write(Chance);
        }
    }
}
