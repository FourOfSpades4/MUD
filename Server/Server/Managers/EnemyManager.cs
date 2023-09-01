using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUD.Net;

namespace MUD.Managers
{
    /* 
     * Container for all Enemies loaded from database with their corresponding room.
     * Should be initialized from the Database before using.
     * To re-spawn enemies, clear the enemies and reload from database.
     */
    public class EnemyManager
    {
        public static EnemyManager instance = new EnemyManager();
        private Dictionary<int, List<NetEnemy>> enemies;


        public EnemyManager()
        {
            enemies = new Dictionary<int, List<NetEnemy>>();
        }

        public void ClearEnemies()
        {
            foreach (List<NetEnemy> roomEnemies in enemies.Values)
            {
                roomEnemies.Clear();
            }
        }

        public void AddEnemy(int roomID, NetEnemy enemy)
        {
            if (!enemies.ContainsKey(roomID))
                enemies[roomID] = new List<NetEnemy>();

            enemies[roomID].Add(enemy);
        }

        public List<NetEnemy> GetEnemies(int roomID)
        {
            if (enemies.ContainsKey(roomID))
                return enemies[roomID];

            return null;
        }
    }
}
