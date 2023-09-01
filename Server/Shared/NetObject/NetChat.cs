using DarkRift;

namespace MUD.Net
{
    public class NetChat : IDarkRiftSerializable
    {
        public string chatMessage;
        public string token;

        public void Deserialize(DeserializeEvent e)
        {
            token = e.Reader.ReadString();
            chatMessage = e.Reader.ReadString();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(token);
            e.Writer.Write(chatMessage);
        }
    }

    public class ChatResponse : IDarkRiftSerializable
    {
        public string username;
        public string title;
        public string chatMessage;

        public void Deserialize(DeserializeEvent e)
        {
            username = e.Reader.ReadString();
            title = e.Reader.ReadString();
            chatMessage = e.Reader.ReadString();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(username);
            e.Writer.Write(title);
            e.Writer.Write(chatMessage);
        }
    }
}
