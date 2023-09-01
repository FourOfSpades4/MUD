using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUD
{
    public class Area
    {
        public string Name { get; private set; }
        private Room[,] rooms;

        public Area(string n, int width, int height)
        {
            Name = n;
            rooms = new Room[width, height];
        }

        public void AddRoom(int w, int h, Room room)
        {
            if (w >= 0 && w < GetWidth())
                if (h >= 0 && h < GetHeight())
                    rooms[w, h] = room;
        }

        public int GetWidth()
        {
            return rooms.GetLength(0);
        }

        public int GetHeight()
        {
            return rooms.GetLength(1);
        }

        public Room GetRoom(int w, int h)
        {
            if (w < 0)
                return null;
            if (h < 0)
                return null;
            if (w >= GetWidth())
                return null;
            if (h >= GetHeight())
                return null;
            return rooms[w, h];
        }
    }
}
