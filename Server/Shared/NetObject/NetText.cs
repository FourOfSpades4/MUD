using DarkRift;

namespace MUD.Net
{
    public class NetText : IDarkRiftSerializable
    {
        public string text;
        public bool append;

        public void Deserialize(DeserializeEvent e)
        {
            text = e.Reader.ReadString();
            append = e.Reader.ReadBoolean();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(text);
            e.Writer.Write(append);
        }
    }
}
