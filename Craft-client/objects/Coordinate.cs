using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft_client.objects
{
    public enum Occupation
    {
        Empty,
        Ship,
        Missed,
        Destroyed
    }
    class Coordinate
    {
        public int Row { get; set; }
        public int Collumn { get; set; }
        public long ShipId { get; set; }
        public Occupation Occupation { get; set; }
        public long GameBoardId { get; set; }
    }
}
