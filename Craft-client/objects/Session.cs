using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft_client.objects
{
    class Session
    {
        public long Id { get; set; }
        public int turn { get; set; }
        public long PlayerOneId { get; set; }
        public long PlayerTwoId { get; set; }
        public bool PlayerOneReady { get; set; }
        public bool PlayerTwoReady { get; set; }
    }
}
