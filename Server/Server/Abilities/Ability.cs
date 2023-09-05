using MUD.Managers;
using MUD.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MUD.Ability
{
    public class Active
    {
        public int ID { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public int Cooldown { get; private set; }
        public int Uses { get; private set; }

        public Active(int id, string name, string desc,
            int cooldown, int uses)
        {
            ID = id;
            Name = name;
            Description = desc;
            Cooldown = cooldown;
            Uses = uses;
        }

        public NetActive GetNetActive()
        {
            return NetActive.CreateActive(ID, Name, Description, Cooldown, Uses);
        }
    }
    public class Passive
    {
        public int ID { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }


        public event EventHandler<CombatInstanceEventArgs> RoomEnter;

        public event EventHandler<CombatInstanceEventArgs> CombatEnter;

        public event EventHandler<CombatInstanceEventArgs> BeforeAttack;
        public event EventHandler<CombatInstanceEventArgs> AfterAttack;

        public event EventHandler<CombatInstanceEventArgs> BeforeAttackRecieved;
        public event EventHandler<CombatInstanceEventArgs> AfterAttackRecieved;

        public event EventHandler<CombatInstanceEventArgs> BeforeAbilityCast;
        public event EventHandler<CombatInstanceEventArgs> AfterAbilityCast;

        public event EventHandler<CombatInstanceEventArgs> TurnStart;
        public event EventHandler<CombatInstanceEventArgs> TurnEnd;

        public event EventHandler<CombatInstanceEventArgs> Death;


        public Passive(int id, string name, string desc)
        {
            ID = id;
            Name = name;
            Description = desc;
        }

        public NetPassive GetNetPassive()
        {
            return NetPassive.CreatePassive(ID, Name, Description);
        }

        public void OnRoomEnter(CombatInstanceEventArgs args)
        {
            EventHandler<CombatInstanceEventArgs> handler = RoomEnter;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public void OnCombatEnter(CombatInstanceEventArgs args)
        {
            EventHandler<CombatInstanceEventArgs> handler = CombatEnter;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public void OnBeforeAttack(CombatInstanceEventArgs args)
        {
            EventHandler<CombatInstanceEventArgs> handler = BeforeAttack;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public void OnAfterAttack(CombatInstanceEventArgs args)
        {
            EventHandler<CombatInstanceEventArgs> handler = AfterAttack;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public void OnBeforeAttackRecieved(CombatInstanceEventArgs args)
        {
            EventHandler<CombatInstanceEventArgs> handler = BeforeAttackRecieved;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public void OnAfterAttackRecieved(CombatInstanceEventArgs args)
        {
            EventHandler<CombatInstanceEventArgs> handler = AfterAttackRecieved;
            if (handler != null)
            {
                handler(this, args);
            }
        }


        public void OnBeforeAbilityCast(CombatInstanceEventArgs args)
        {
            EventHandler<CombatInstanceEventArgs> handler = BeforeAbilityCast;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public void OnAfterAbilityCast(CombatInstanceEventArgs args)
        {
            EventHandler<CombatInstanceEventArgs> handler = AfterAbilityCast;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public void OnTurnStart(CombatInstanceEventArgs args)
        {
            EventHandler<CombatInstanceEventArgs> handler = TurnStart;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public void OnTurnEnd(CombatInstanceEventArgs args)
        {
            EventHandler<CombatInstanceEventArgs> handler = TurnEnd;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public void OnDeath(CombatInstanceEventArgs args)
        {
            EventHandler<CombatInstanceEventArgs> handler = Death;
            if (handler != null)
            {
                handler(this, args);
            }
        }
    }
}

    
