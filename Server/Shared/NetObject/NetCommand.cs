using DarkRift;

namespace MUD.Net
{
    public class NetCommand : IDarkRiftSerializable
    {
        public string command;
        public string token;

        public void Deserialize(DeserializeEvent e)
        {
            token = e.Reader.ReadString();
            command = e.Reader.ReadString();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(token);
            e.Writer.Write(command);
        }
    }
}
