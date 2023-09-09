using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MUD.Net
{
    public class NetStatus : IDarkRiftSerializable {
        public int ID { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public int Turns { get; protected set; }

        public static NetStatus CreateStatus(int id, string name, string desc, int turns)
        {
            NetStatus s = new NetStatus();

            s.ID = id;
            s.Name = name;
            s.Description = desc;
            s.Turns = turns;

            return s;
        }

        public void Deserialize(DeserializeEvent e)
        {
            ID = e.Reader.ReadInt32();
            Name = e.Reader.ReadString();
            Description = e.Reader.ReadString();
            Turns = e.Reader.ReadInt32();
        }
        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(ID);
            e.Writer.Write(Name);
            e.Writer.Write(Description);
            e.Writer.Write(Turns);
        }

    }
}
