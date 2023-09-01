using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUD
{
    public class Room
    {
        Stopwatch stopwatch;
        Area area;
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Dictionary<int, string> Decay { get; private set; }
        public List<RoomModifier> modifiers;

        public int left, right, up, down;

        public Room(Area a, string n, string d, Dictionary<int, string> decay)
        {
            stopwatch = new Stopwatch();
            modifiers = new List<RoomModifier>();
            stopwatch.Start();
            area = a;
            Name = n;
            Description = d;
            Decay = decay;
        }

        public virtual void EnterRoom()
        {
            stopwatch.Restart();
        }

        public string GetRoomDescription()
        {
            string finalDesc = "";

            finalDesc += Description;

            foreach (RoomModifier mod in modifiers)
            {
                if (mod.isActive()) {
                    finalDesc += mod.Description;
                }
            }

            finalDesc += "\n\n";



            return finalDesc;
        }
             
        public string GetDecayText()
        {
            long secondsSinceUpdate = stopwatch.ElapsedMilliseconds / 1000;

            foreach (int timeDifference in Decay.Keys.Reverse<int>())
            {
                if (secondsSinceUpdate >= timeDifference)
                    return Decay[timeDifference];
            }

            return "";
        }
    }

    public class RoomModifier
    {
        public string Description { get; private set; }
        private bool active;
        public RoomModifier(string descriptionAddition)
        {
            Description = descriptionAddition;
        }

        public bool isActive()
        {
            return active;
        }

        public void isActive(bool a)
        {
            active = a;
        }
    }
}
