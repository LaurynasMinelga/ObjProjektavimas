using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Craft_server.Models
{
    public class GamePanelContext : DbContext
    {
        public GamePanelContext(DbContextOptions<GamePanelContext> options)
           : base(options)
        {
        }

        public DbSet<GamePanel> GamePanels { get; set; }
    }
}