using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft_client.objects
{
    class Ship
    {
        public long Id { get; set; }
        public int[] Row { get; set; }
        public int[] Collumn { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }
    }
}
