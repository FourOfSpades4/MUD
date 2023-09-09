using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DarkRift;
using MUD;

namespace MUD.Net
{
    public class NetPlayer : NetCharacter
    {
        public string Title { get; private set; }

        public static NetPlayer CreatePlayer(string username, 
            NetPassive[] passives, NetActive[] actives)
        {
            NetPlayer p = new NetPlayer();

            p.Name = username;
            p.Title = "";
            p.type = CharacterType.PLAYER;

            p.Passives = passives;
            p.Actives = actives;

            return p;
        }

        public override string ToString()
        {
            return Name + " (" + Title + ")";
        }

        public override void Deserialize(DeserializeEvent e)
        {
            base.Deserialize(e);

            Title = e.Reader.ReadString();
        }
        public override void Serialize(SerializeEvent e)
        {
            base.Serialize(e);

            e.Writer.Write(Title);
        }
    }
}