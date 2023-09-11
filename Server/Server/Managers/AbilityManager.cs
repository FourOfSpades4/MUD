using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUD.Ability;
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
        private Dictionary<int, Passive> passives;
        private Dictionary<int, Active> actives;
        private List<AbilitySet> abilities;

        public AbilityManager()
        {
            passives = new Dictionary<int, Passive>();
            actives = new Dictionary<int, Active>();
            passives.Add(0, new Passive(0, "", ""));
            actives.Add(0, new Active(0, "", "", 0, 0));

            CreateAbilitySet();
        }

        public void CreateAbilitySet()
        {
            abilities = new List<AbilitySet>();
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in asm.GetTypes())
                {
                    if (type.GetInterface("AbilitySet") == typeof(AbilitySet))
                    {
                        Server.logger.Info("Loaded Abilities: " + type.Name);
                        abilities.Add((AbilitySet)Activator.CreateInstance(type, new object[] {  }));
                    }
                }
            }
        }

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

namespace MUD.Ability {
    public interface AbilitySet
    {
        public void UpdatePassive(int id, Passive passive);
        public bool HasPassive(int id);
    }
}
