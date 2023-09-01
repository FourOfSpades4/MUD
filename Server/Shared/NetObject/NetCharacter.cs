using DarkRift;
using MUD;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUD.Net
{
    public abstract class NetCharacter : IDarkRiftSerializable
    {
        public string Name { get; protected set; }
        public Passive[] Passives { get; protected set; }
        public Active[] Actives { get; protected set; }

        public NetCharacter()
        {
            Passives = new Passive[Settings.passiveSlots];
            Actives = new Active[Settings.activeSlots];

            for (int i = 0; i < 5; i++)
            {
                Passives[i] = Passive.CreatePassive(-1, "", "");
            }

            for (int i = 0; i < 5; i++)
            {
                Actives[i] = Active.CreateActive(-1, "", "", 0, 0);
            }
        }
        public virtual void Deserialize(DeserializeEvent e)
        {
            Name = e.Reader.ReadString();
            Passives = e.Reader.ReadSerializables<Passive>();
            Actives = e.Reader.ReadSerializables<Active>();
        }

        public virtual void Serialize(SerializeEvent e)
        {
            e.Writer.Write(Name);
            e.Writer.Write(Passives);
            e.Writer.Write(Actives);
        }
    }
}
