using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUD.Ability;
using MUD.Net;


namespace MUD.Characters
{
    public class Enemy : Character
    {
        private static int currentID = 0;
        public int ID { get; private set; }
        public string Description { get; private set; }
        public string EnterCombatText { get; private set; }
        public string RoomEnterText { get; private set; }
        public NetDrops InstanceDrops { get; private set; }

        public Enemy(string name, string description,
            NetDrops drops, string enterCombatText = "", string roomEnterText = "") : base()
        {
            Name = name;
            Description = description;
            EnterCombatText = enterCombatText;
            RoomEnterText = roomEnterText;
            InstanceDrops = drops;

            Type = CharacterType.ENEMY;

            ID = currentID;
            currentID++;
        }

        public NetEnemy Net()
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

            return NetEnemy.CreateEnemy(Name, Description, InstanceDrops,
                passives.ToArray(), actives.ToArray(), ID,
                EnterCombatText, RoomEnterText);
        }
    }
}