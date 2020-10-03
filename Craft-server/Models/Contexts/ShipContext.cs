using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Craft_server.Models
{
    public class ShipContext : DbContext
    {
        public ShipContext(DbContextOptions<ShipContext> options)
           : base(options)
        {
        }

        public DbSet<Ship> Ships { get; set; }
    }
}
