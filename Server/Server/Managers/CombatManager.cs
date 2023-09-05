using MUD.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUD.Ability;

namespace MUD.Managers
{
    public class CombatManager
    {
        public static CombatManager instance = new CombatManager();
        private Dictionary<int, CombatInstance> combats;


        public void TickCombats()
        {
            foreach (CombatInstance instance in combats.Values)
            {
                instance.TickTurn();
            }
        }

    }

    public class CombatInstance
    {
        int Turn { get; set; }
        int TurnTimeRemaining { get; set; }
        List<NetCharacter> Characters { get; set; }
        List<ushort> Players { get; set; }

        CombatInstance()
        {
            Turn = 0;
            Characters = new List<NetCharacter>();
        }

        public void AddCharacter(NetCharacter c)
        {
            Characters.Add(c);

            if (c.type == NetCharacter.CharacterType.PLAYER)
                Players.Add(((NetPlayer)c).ID);
        }

        public void AddCharacters(List<NetCharacter> c)
        {
            Characters.AddRange(c);

            foreach (NetCharacter character in c)
                if (character.type == NetCharacter.CharacterType.PLAYER)
                    Players.Add(((NetPlayer)character).ID);
        }

        public void TickTurn()
        {
            TurnTimeRemaining -= 1;

            // PlayerManager.instance.SendToClients(Players, Tags)
        }
    }
    public class CombatInstanceEventArgs : EventArgs
    {
        public CombatInstance Combat { get; set; }
        public NetCharacter Character { get; set; }
        public List<NetCharacter> Targets { get; set; }
        public Active Ability { get; set; }
    }
}
