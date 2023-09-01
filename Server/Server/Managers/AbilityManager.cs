using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUD.Net;

namespace MUD.Managers
{
    /* 
     * Container for all Active and Passive abilities loaded from database.
     * Should be initialized from the Database before using.
     */
    public class AbilityManager
    {
        public static AbilityManager instance = new AbilityManager();
        private Dictionary<int, Passive> passives = new Dictionary<int, Passive>();
        private Dictionary<int, Active> actives = new Dictionary<int, Active>();

        public void AddPassive(int id, Passive passive)
        {
            passives[id] = passive;
        }

        public Passive GetPassive(int id)
        {
            if (passives.ContainsKey(id))
            {
                return passives[id];
            }
            return null;
        }

        public void AddActive(int id, Active active)
        {
            actives[id] = active;
        }

        public Active GetActive(int id)
        {
            if (actives.ContainsKey(id))
            {
                return actives[id];
            }
            return null;
        }
    }
}
