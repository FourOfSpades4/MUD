using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUD.Ability;
using MUD.Combat;
using MUD.Net;

namespace MUD.Ability
{
    class TestAbilities : AbilitySet
    {
        private int[] passiveIDs = { 1, 2, 3 };
        private int[] activeIDs = { 1, 2, 3 };

        public static void DealDamage(object sender, CombatInstanceEventArgs args, int damage)
        {
        }

        public void UpdatePassive(int id, Passive passive)
        {
            if (id == 1)
            {

            }
        }

        public void UpdatActive(int id, Active active)
        {
            if (id == 1)
            {
                active.Cast += (obj, combatInstance) => DealDamage(obj, combatInstance, 10);
            }
        }

        public bool HasActive(int id)
        {
            return activeIDs.Contains(id);
        }
        public bool HasPassive(int id)
        {
            return passiveIDs.Contains(id);
        }
    }
}
