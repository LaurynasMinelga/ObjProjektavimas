using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Craft_server.Models
{
    public class GameBoardContext : DbContext
    {
        public GameBoardContext(DbContextOptions<GameBoardContext> options)
           : base(options)
        {
        }

        public DbSet<GameBoard> GameBoards { get; set; }
    }
}
