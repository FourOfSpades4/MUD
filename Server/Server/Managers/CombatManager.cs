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

            public CombatManager()
            {
                combats = new Dictionary<int, CombatInstance>();
            }


            public void TickCombats()
            {
                foreach (KeyValuePair<int, CombatInstance> instance in combats)
                {
                    if (instance.Value.isActive == -1)
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

                    List<Enemy> enemies = EnemyManager.instance.GetEnemies(roomID);

                    foreach (Enemy enemy in enemies)
                        instance.AddCharacter(enemy);
                }

                player.UpdateCombatStatus(true);
                instance.AddCharacter(player);
                instance.StartCombat();
            }

            public void EndCombat(Player player, int roomID)
            {
                if (!combats.ContainsKey(roomID))
                    return;

                foreach (Passive p in player.Passives)
                {
                    if (p != null)
                    {
                        CombatInstanceEventArgs combat = new CombatInstanceEventArgs();
                        combat.Combat = combats[roomID];
                        combat.Character = player;
                        p.OnCombatExit(combat);
                    }
                }

                combats[roomID].RemoveCharacter(player);

                player.UpdateCombatStatus(false);
            }

            public void EndCombat(int roomID)
            {
                if (!combats.ContainsKey(roomID))
                    return;

                combats[roomID].EndCombat();
                combats.Remove(roomID);
            }

            public void ForceEndCombat(Player player)
            {
                foreach (CombatInstance combatInstance in combats.Values)
                {
                    combatInstance.RemoveCharacter(player);

                    foreach (Passive p in player.Passives)
                    {
                        if (p != null)
                        {
                            CombatInstanceEventArgs combat = new CombatInstanceEventArgs();
                            combat.Combat = combatInstance;
                            combat.Character = player;
                            p.OnCombatExit(combat);
                        }
                    }
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

            public int isActive { get; private set; }



            public CombatInstance()
            {
                Turn = 0;
                Characters = new List<Character>();
                Players = new List<ushort>();
                isActive = 0;
            }

            public void StartCombat()
            {
                isActive = 1;
                TurnTimeRemaining = Settings.turnTicks;
                StartTurn();
            }

            public void AddCharacter(Character c)
            {
                Characters.Add(c);

                if (c.Type == CharacterType.PLAYER)
                {
                    Players.Add(((Player)c).ID);
                    foreach (Passive p in c.Passives)
                    {
                        if (p != null)
                        {
                            CombatInstanceEventArgs combat = new CombatInstanceEventArgs();
                            combat.Combat = this;
                            combat.Character = c;
                            p.OnCombatEnter(combat);
                        }
                    }
                }
            }

            public void RemoveCharacter(Character c)
            {
                Characters.Remove(c);

                if (c.Type == CharacterType.PLAYER)
                {
                    Players.Remove(((Player)c).ID);

                    foreach (Passive p in c.Passives)
                    {
                        if (p != null)
                        {
                            CombatInstanceEventArgs combat = new CombatInstanceEventArgs();
                            combat.Combat = this;
                            combat.Character = c;
                            p.OnCombatExit(combat);
                        }
                    }
                }
            }

            public void AddCharacters(List<Character> c)
            {
                Characters.AddRange(c);

                foreach (Character character in c)
                {
                    if (character.Type == CharacterType.PLAYER)
                    {
                        Players.Add(((Player)character).ID);

                        foreach (Passive p in character.Passives)
                        {
                            if (p != null)
                            {
                                CombatInstanceEventArgs combat = new CombatInstanceEventArgs();
                                combat.Combat = this;
                                combat.Character = character;
                                p.OnCombatEnter(combat);
                            }
                        }
                    }
                }
            }

            public void EndCombat()
            {
                foreach (Character character in Characters)
                {
                    if (character.Type == CharacterType.PLAYER)
                    {
                        ((Player)character).UpdateCombatStatus(false);

                        foreach (Passive p in character.Passives)
                        {
                            if (p != null)
                            {
                                CombatInstanceEventArgs combat = new CombatInstanceEventArgs();
                                combat.Combat = this;
                                combat.Character = character;
                                p.OnCombatExit(combat);
                            }
                        }
                    }
                }

                isActive = -1;
            }

            public void TickTurn()
            {
                TurnTimeRemaining -= 1;

                if (TurnTimeRemaining <= 0)
                {
                    EndTurn();
                    NextTurn();
                }

                // Enemy Turn
                if (TurnTimeRemaining <= (Settings.turnTicks - Settings.enemyTurnTime))
                {
                    Character character = Characters[Turn];
                    if (character.Type == CharacterType.ENEMY)
                    {
                        // TODO

                        CombatInstanceEventArgs combat = new CombatInstanceEventArgs();

                        combat.Combat = this;

                        combat.Character = character;

                        combat.Ability = character.Actives[0];

                        combat.Targets = new List<Character>();
                        foreach (Character c in Characters)
                        {
                            if (c != character)
                            {
                                combat.Targets.Add(c);
                            }
                        }

                        character.Actives[0].OnCast(combat);

                        EndTurn();
                        NextTurn();
                    }
                }

                // PlayerManager.instance.SendToClients(Players, Tags)
            }

            public void NextTurn()
            {
                Turn = (Turn + 1) % Characters.Count;
                TurnTimeRemaining = Settings.turnTicks;

                StartTurn();
            }

            public void StartTurn()
            {
                Character character = Characters[Turn];
                Server.logger.Info("Turn Starting: " + character.Name);

                foreach (Passive p in character.Passives)
                {
                    if (p != null)
                    {
                        CombatInstanceEventArgs combat = new CombatInstanceEventArgs();
                        combat.Combat = this;
                        combat.Character = character;
                        p.OnTurnStart(combat);
                    }
                }
            }

            public void EndTurn()
            {
                Character character = Characters[Turn];

                foreach (Passive p in character.Passives)
                {
                    if (p != null)
                    {
                        CombatInstanceEventArgs combat = new CombatInstanceEventArgs();
                        combat.Combat = this;
                        combat.Character = character;
                        p.OnTurnEnd(combat);
                    }
                }

                for (int i = 0; i < Characters.Count; i++)
                {
                    character = Characters[i];
                    if (character.Health <= 0)
                    {
                        foreach (Passive p in character.Passives)
                        {
                            if (p != null)
                            {
                                CombatInstanceEventArgs combat = new CombatInstanceEventArgs();
                                combat.Combat = this;
                                combat.Character = character;
                                p.OnDeath(combat);
                            }
                        }

                        if (Characters[i].Health <= 0)
                        {
                            Characters[i].Die();
                            RemoveCharacter(Characters[i]);
                        }
                    }
                }
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