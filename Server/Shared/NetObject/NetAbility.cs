using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MUD.Net
{
    public abstract class NetAbility : IDarkRiftSerializable {
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        public void Deserialize(DeserializeEvent e)
        {
            Name = e.Reader.ReadString();
            Description = e.Reader.ReadString();

        }
        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(Name);
            e.Writer.Write(Description);
        }

    }
    public class NetActive : NetAbility
    {
        public int Cooldown { get; private set; }
        public int Uses { get; private set; }
        public static NetActive CreateActive(string name, string desc, 
            int cooldown, int uses)
        {
            NetActive a = new NetActive();

            a.Name = name;
            a.Description = desc;
            a.Cooldown = cooldown;
            a.Uses = uses;

            return a;
        }
        public void Deserialize(DeserializeEvent e)
        {
            base.Deserialize(e);
            Cooldown = e.Reader.ReadInt32();
            Uses = e.Reader.ReadInt32();
        }
        public void Serialize(SerializeEvent e)
        {
            base.Serialize(e);
            e.Writer.Write(Cooldown);
            e.Writer.Write(Uses);
        }
    }

    public class NetPassive : NetAbility
    {
        public static NetPassive CreatePassive(string name, string desc)
        {
            NetPassive p = new NetPassive();

            p.Name = name;
            p.Description = desc;

            return p;
        }

        public void Deserialize(DeserializeEvent e)
        {
            base.Deserialize(e);
        }
        public void Serialize(SerializeEvent e)
        {
            base.Serialize(e);
        }
    }
}
