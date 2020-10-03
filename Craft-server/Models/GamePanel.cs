using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Craft_server.Models
{
    public enum Levels
    {
        Alaska,
        Desert,
        Swamp,
        Sea,
        Space
    }
    public class GamePanel
    {
        public long Id { get; set; }
        public List<GameBoard> GameBoards { get; set; }
        public Session Session { get; set; }
        public long SessionId { get; set; }
        public Levels Levels { get; set; }
    }
}
