using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Craft_server.Models
{
    public enum Levels
    {
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
        public Levels Level { get; set; }
        public string Gun1 { get; set; }
        public string Gun2 { get; set; }

    }
}
