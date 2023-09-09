using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DarkRift;
using MUD.Ability;
using MUD.Net;

namespace MUD.Characters
{
    public class Player : Character
    {
        public ushort ID { get; private set; }
        public string Title { get; private set; }
        public string Token { get; private set; }
        public Boolean InCombat { get; private set; }
        public int PassiveSlots { get; private set; }
        public int ActiveSlots { get; private set; }

        public Player(string username, ushort id, int passive, int active)
        {
            Name = username;
            ID = id;
            Title = "";
            InCombat = false;
            Type = CharacterType.PLAYER;
            PassiveSlots = passive;
            ActiveSlots = active;

            Passives = new Passive[PassiveSlots];
            Actives = new Active[ActiveSlots];
        }

        public void SetPassiveSlots(int passive)
        {
            PassiveSlots = passive;
        }

        public void SetActiveSlots(int active)
        {
            ActiveSlots = active;
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

        public NetPlayer Net()
        {
            List<NetPassive> passives = new List<NetPassive>();
            List<NetActive> actives = new List<NetActive>();

            foreach (Passive passive in Passives)
            {
                passives.Add(passive.Net());
            }

            foreach (Active active in Actives)
            {
                actives.Add(active.Net());
            }

            return NetPlayer.CreatePlayer(Name, passives.ToArray(), actives.ToArray());
        }
        public void UpdateCombatStatus(bool inCombat)
        {
            InCombat = inCombat;
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
    }
}