using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft_client.objects
{
    abstract class Ship
    {
        public long Id;

        public abstract string getShipType();
        public abstract int getShipRow();

        public abstract int getShipCollumn();
    }
}
