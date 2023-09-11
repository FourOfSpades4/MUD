using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUD.Ability;
using MUD.Combat;
using MUD.Net;
using MUD.Characters;

namespace MUD.Ability
{
    class TestAbilities : AbilitySet
    {
        private int[] passiveIDs = { 1, 2, 3 };
        private int[] activeIDs = { 1, 2, 3 };

        public static void DealDamage(object sender, CombatInstanceEventArgs args, int damage)
        {
            foreach (Character c in args.Targets)
                c.TakeDamage(damage);
        }

        public void UpdatePassive(int id, Passive passive)
        {
            if (id == 1)
                passive.CombatEnter += (obj, combatInstance) => Server.logger.Info("Passive 1 Triggered");
            if (id == 2)
                passive.TurnStart += (obj, combatInstance) => Server.logger.Info("Passive 2 Triggered");
            if (id == 3)
                passive.TurnEnd += (obj, combatInstance) => Server.logger.Info("Passive 3 Triggered");
        }

        public void UpdateActive(int id, Active active)
        {
            if (id == 1)
                active.Cast += (obj, combatInstance) => DealDamage(obj, combatInstance, 10);
            if (id == 2)
                active.Cast += (obj, combatInstance) => DealDamage(obj, combatInstance, -10);
            if (id == 2)
                active.Cast += (obj, combatInstance) => DealDamage(obj, combatInstance, 100);
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
