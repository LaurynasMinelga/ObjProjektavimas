using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft_client.objects
{
    class GamePanel
    {
        public enum Levels
        {
            Desert,
            Swamp,
            Sea,
            Space
        }

        public long Id { get; set; }
        public long SessionId { get; set; }
        public Levels Level { get; set; }
    }
}
