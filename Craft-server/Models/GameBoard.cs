using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Craft_server.Models
{
    public class GameBoard
    {
        public long Id { get; set; }
        public List<Coordinate> Coordinate { get; set; }
        public Player player { get; set; }
        public long GamePanelId { get; set; }
        public GamePanel GamePanel { get; set; }
    }
}
