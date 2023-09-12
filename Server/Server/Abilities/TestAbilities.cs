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

        public static void HealDamage(object sender, CombatInstanceEventArgs args, int health)
        {
            Passive passive = (Passive)sender;

            if (passive.Uses > 0)
            {
                Server.logger.Info("Lifesaver Passive Triggered");
                args.Character.TakeDamage(-health);
                passive.Uses -= 1;
            }
        }

        public static void SetUses(object sender, CombatInstanceEventArgs args, int uses)
        {
            Passive passive = (Passive)sender;
            passive.Uses = uses;
        }

        public void UpdatePassive(int id, Passive passive)
        {
            if (id == 1)
                passive.CombatEnter += (obj, combatInstance) => Server.logger.Info("Passive 1 Triggered");
            if (id == 2)
            {
                passive.Death += (obj, combatInstance) => HealDamage(obj, combatInstance, 25);
                passive.CombatEnter += (obj, combatInstance) => SetUses(obj, combatInstance, 2);
                passive.CombatExit += (obj, combatInstance) => SetUses(obj, combatInstance, 2);
            }
            if (id == 3)
                passive.TurnEnd += (obj, combatInstance) => Server.logger.Info(combatInstance.Character.Health.ToString());
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
