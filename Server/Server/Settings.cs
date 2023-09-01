using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUD
{
    static class CommandSettings
    {
        public static HashSet<string> leftCommands = new HashSet<string>(new string[] {"left", "west", "l", "a" });
        public static HashSet<string> rightCommands = new HashSet<string>(new string[] { "right", "east", "r", "d" });
        public static HashSet<string> upCommands = new HashSet<string>(new string[] { "up", "north", "u", "w" });
        public static HashSet<string> downCommands = new HashSet<string>(new string[] { "down", "south", "d", "s" });
    }
}
