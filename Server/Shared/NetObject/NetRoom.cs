using DarkRift;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace MUD.Net
{
    public class NetRoom : IDarkRiftSerializable
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Up { get; private set; }
        public int Down { get; private set; }
        public int Left { get; private set; }
        public int Right { get; private set; }
        public Modifier[] Modifiers { get; private set; }

        public static NetRoom CreateRoom(int id, string name, string desc, int up, int down, int left, int right, Modifier[] modifiers)
        {
            NetRoom r = new NetRoom();
            r.ID = id;
            r.Name = name;
            r.Description = desc;
            r.Up = up;
            r.Down = down;
            r.Left = left;
            r.Right = right;
            r.Modifiers = modifiers;
            return r;
        }

        public virtual void Deserialize(DeserializeEvent e)
        {
            Name = e.Reader.ReadString();
            Description = e.Reader.ReadString();
            Up = e.Reader.ReadInt32();
            Down = e.Reader.ReadInt32();
            Left = e.Reader.ReadInt32();
            Right = e.Reader.ReadInt32();
            Modifiers = e.Reader.ReadSerializables<Modifier>();
        }

        public virtual void Serialize(SerializeEvent e)
        {
            e.Writer.Write(Name);
            e.Writer.Write(Description);
            e.Writer.Write(Up);
            e.Writer.Write(Down);
            e.Writer.Write(Left);
            e.Writer.Write(Right);
            e.Writer.Write(Modifiers);
        }

        public override string ToString()
        {
            string str = Description + "\n\n";

            foreach (Modifier modifier in Modifiers)
            {
                str += modifier.ToString() + "\n\n";
            }

            return str;
        }
    }

    public class Modifier : IDarkRiftSerializable
    {
        public string Text { get; private set; }

        public static Modifier CreateModifier(string text)
        {
            Modifier m = new Modifier();
            m.Text = text;
            return m;
        }

        public virtual void Deserialize(DeserializeEvent e)
        {
            Text = e.Reader.ReadString();
        }

        public virtual void Serialize(SerializeEvent e)
        {
            e.Writer.Write(Text);
        }
        public override string ToString()
        {
            return Text;
        }
    }
}
