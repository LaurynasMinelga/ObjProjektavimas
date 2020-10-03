using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Craft_server.Models
{
    public class Session
    {
        public long Id { get; set; }
        public int turn { get; set; }
        public GamePanel GamePanel { get; set; }
        //public long GamePanelId { get; set; }
    }
}
