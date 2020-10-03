using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Craft_server.Models
{
    public class CoordinateContext : DbContext
    {
        public CoordinateContext(DbContextOptions<CoordinateContext> options)
           : base(options)
        {
        }

        public DbSet<Coordinate> Coordinates { get; set; }
    }
}