using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUD.Net
{
    public class NetArea : IDarkRiftSerializable
    {
        public string Name { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public static NetArea CreateArea(string name, int width, int height)
        {
            NetArea a = new NetArea();
            a.Name = name;
            a.Width = width;
            a.Height = height;
            return a;
        }

        public virtual void Deserialize(DeserializeEvent e)
        {
            Name = e.Reader.ReadString();
            Width = e.Reader.ReadInt32();
            Height = e.Reader.ReadInt32();
        }

        public virtual void Serialize(SerializeEvent e)
        {
            e.Writer.Write(Name);
            e.Writer.Write(Width);
            e.Writer.Write(Height);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
