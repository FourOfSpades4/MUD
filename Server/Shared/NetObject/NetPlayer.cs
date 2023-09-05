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
        public ushort ID { get; private set; }
        public string Title { get; private set; }
        public string Token { get; private set; }
        public Boolean InCombat { get; private set; }

        public static NetPlayer CreatePlayer(string username, ushort id)
        {
            NetPlayer p = new NetPlayer();

            p.Name = username;
            p.ID = id;
            p.Title = "";
            p.InCombat = false;
            p.type = CharacterType.PLAYER;

            return p;
        }

        public void SetTitle(string title)
        {
            Title = title;
        }

        public void SetToken(string t)
        {
            Token = t;
        }

        public bool VerifyToken(string t)
        {
            return t == Token;
        }

        public override string ToString()
        {
            return Name + " (" + Title + ")" + "     " +
                Passives[0].ID + " | " + Passives[1].ID + " | " +
                Passives[2].ID + " | " + Passives[3].ID + " | " +
                Passives[4].ID + "    " +
                Actives[0].ID + " | " + Actives[1].ID + " | " +
                Actives[2].ID + " | " + Actives[3].ID + " | " +
                Actives[4].ID;
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

        public void UpdateCombatStatus(bool inCombat)
        {
            InCombat = inCombat;
        }
    }
}