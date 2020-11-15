using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Craft_server.Models
{
    public class Ship
    {
        public long Id { get; set; } 
        public int Size { get; set; } // length 1-4 squares
        public List<Coordinate> Coordinate { get; set; } // 1 ship can have multiple coordinates if it's a long ship
        public string type { get; set; }
    }
}
