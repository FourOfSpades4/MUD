using MUD.Managers;
using MUD.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MUD.Ability;

namespace MUD.Combat
{
    public class Status : Passive
    {
        public int Turns { get; private set; }

        public event EventHandler<CombatInstanceEventArgs> Apply;
        public event EventHandler<CombatInstanceEventArgs> Remove;

        public Status(int id, string name, string desc, int turns)
            : base(id, name, desc)
        {
            Turns = turns;
        }

        public NetStatus Net()
        {
            return NetStatus.CreateStatus(ID, Name, Description, Turns);
        }

        public void OnApply(CombatInstanceEventArgs args)
        {
            EventHandler<CombatInstanceEventArgs> handler = Remove;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public void OnRemove(CombatInstanceEventArgs args)
        {
            EventHandler<CombatInstanceEventArgs> handler = Apply;
            if (handler != null)
            {
                handler(this, args);
            }
        }
    }
}

    
