using DarkRift;
using MUD;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUD.Characters;

namespace MUD.Net
{
    public abstract class NetCharacter : IDarkRiftSerializable
    {
        public string Name { get; protected set; }
        public NetPassive[] Passives { get; protected set; }
        public NetActive[] Actives { get; protected set; }
        public int Health { get; protected set; }

        public CharacterType type;
        public NetItem[] Armor;

        public enum CharacterType { PLAYER, ENEMY }

        public NetCharacter()
        {
            Health = 100;
        }
        public virtual void Deserialize(DeserializeEvent e)
        {
            Name = e.Reader.ReadString();
            Passives = e.Reader.ReadSerializables<NetPassive>();
            Actives = e.Reader.ReadSerializables<NetActive>();
            Armor = e.Reader.ReadSerializables<NetItem>();
        }

        public virtual void Serialize(SerializeEvent e)
        {
            e.Writer.Write(Name);
            e.Writer.Write(Passives);
            e.Writer.Write(Actives);
            e.Writer.Write(Armor);
        }
    }
}

namespace MUD.Characters
{
    public enum CharacterType { PLAYER, ENEMY }
}