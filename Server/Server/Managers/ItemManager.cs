using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUD.Net;

namespace MUD.Managers
{
    /* 
     * Container for all Items loaded from database.
     * Should be initialized from the Database before using.
     */
    public class ItemManager
    {
        public static ItemManager instance = new ItemManager();
        private Dictionary<int, NetItem> items = new Dictionary<int, NetItem>();

        public void AddItem(int id, NetItem item)
        {
            items[id] = item;
        }

        public NetItem GetItem(int id)
        {
            if (items.ContainsKey(id))
            {
                return items[id];
            }
            return null;
        }
    }
}
