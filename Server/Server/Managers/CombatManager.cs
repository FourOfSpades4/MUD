using MUD.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUD.Ability;
using MUD.Combat;
using MUD.Characters;

namespace MUD
{
    namespace Managers
    {
        public class CombatManager
        {
            public static CombatManager instance = new CombatManager();
            private Dictionary<int, CombatInstance> combats;


            public void TickCombats()
            {
                foreach (KeyValuePair<int, CombatInstance> instance in combats)
                {
                    if (!instance.Value.isActive)
                        combats.Remove(instance.Key);

                    instance.Value.TickTurn();
                }
            }

            public void StartCombat(Player player, int roomID)
            {
                CombatInstance instance;

                if (combats.ContainsKey(roomID))
                {
                    instance = combats[roomID];
                }
                else
                {
                    instance = new CombatInstance();
                    combats[roomID] = instance;
                }

                player.UpdateCombatStatus(true);
                instance.AddCharacter(player);
            }

            public void EndCombat(Player player, int roomID)
            {
                if (!combats.ContainsKey(roomID))
                    return;

                combats[roomID].RemoveCharacter(player);
                player.UpdateCombatStatus(false);
            }

            public void ForceEndCombat(Player player)
            {
                foreach (CombatInstance combatInstance in combats.Values)
                {
                    combatInstance.RemoveCharacter(player);
                }
                player.UpdateCombatStatus(false);
            }
        }
    }
    namespace Combat
    {
        public class CombatInstance
        {
            int Turn { get; set; }
            int TurnTimeRemaining { get; set; }
            List<Character> Characters { get; set; }
            List<ushort> Players { get; set; }

            public bool isActive { get; }



            public CombatInstance()
            {
                Turn = 0;
                Characters = new List<Character>();
                isActive = true;
            }

            public void AddCharacter(Character c)
            {
                Characters.Add(c);

                if (c.Type == CharacterType.PLAYER)
                    Players.Add(((Player)c).ID);
            }

            public void RemoveCharacter(Character c)
            {
                Characters.Remove(c);

                if (c.Type == CharacterType.PLAYER)
                    Players.Remove(((Player)c).ID);
            }

            public void AddCharacters(List<Character> c)
            {
                Characters.AddRange(c);

                foreach (Character character in c)
                    if (character.Type == CharacterType.PLAYER)
                        Players.Add(((Player)character).ID);
            }

            public void EndCombat()
            {
                foreach (Character character in Characters)
                    if (character.Type == CharacterType.PLAYER)
                        ((Player)character).UpdateCombatStatus(false);
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
            public Character Character { get; set; }
            public List<Character> Targets { get; set; }
            public Active Ability { get; set; }

            public CombatInstanceEventArgs ()
            {
                Combat = null;
                Character = null;
                Targets = null;
                Ability = null;
            }
        }
    }
}