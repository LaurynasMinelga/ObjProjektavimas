using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Craft_server.Models
{
    public class Player
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long GameBoardId { get; set; } //can be null
        public GameBoard GameBoard { get; set; } //can be null
        public Session Session { get; set; } //can be null
    }
}
