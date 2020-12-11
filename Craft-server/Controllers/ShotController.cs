using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Craft_server.Models;

namespace Craft_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShotController : ControllerBase
    {
        private readonly CoordinateContext _context;

        public ShotController(CoordinateContext context)
        {
            _context = context;
        }

        // GET: api/Sessions
        [HttpGet("{gameboardId}")]
        public async Task<ActionResult<IEnumerable<Coordinate>>> GetCoordinates(long gameboardId)
        {
            var coord_sequence = from coord in _context.Coordinates
                                 where coord.GameBoardId.Equals(gameboardId)
                                 select coord;

            //var coord_pure = coord_sequence.ToListAsync();
            return await coord_sequence.ToListAsync();
        }

        // POST: api/Shot
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("{gameboardId}")]
        public async Task<IActionResult> PostCoordinate(Coordinate coordinate, long gameboardId)
        {
            var coordinate_sequence = from coord in _context.Coordinates
                                      where coord.GameBoardId.Equals(gameboardId) &&
                                            coord.Collumn.Equals(coordinate.Collumn) &&
                                            coord.Row.Equals(coordinate.Row)
                                      select coord;

            if (coordinate_sequence.Count() > 0) // if ship exists
            {
                var coordinate_exist = coordinate_sequence.First(); //assign it to variable

                //destroy the ship
                coordinate_exist.Occupation = Occupation.Destroyed;
                _context.Entry(coordinate_exist).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                //check how long is the ship
                var ship_size_sequence = from coord in _context.Coordinates
                                where coord.GameBoardId.Equals(gameboardId) && coord.ShipId.Equals(coordinate_exist.ShipId)
                                select coord;
                var ship_size = ship_size_sequence.Count();

                //assign all coordinates around the ship as observers
                Coordinate c = new Coordinate();
                Battlefield battlefield = new Battlefield(_context);
                /*
                for (int i=0;i < ship_size; i++)
                {
                    c = ship_size_sequence.ToArray()[i];
                    //battlefield.Attach(c);
                    battlefield = DisableNearbyCoordinates(battlefield, c.Row, c.Collumn, gameboardId);
                }*/

                //check if the ship is destroyed
                bool all_destroyed = true;
                for (int i = 0; i < ship_size; i++)
                {
                    c = ship_size_sequence.ToArray()[i];
                    if (c.Occupation == Occupation.Ship)
                    {
                        all_destroyed = false;
                    }
                }
                if (all_destroyed) // if whole ship destroyed - notify
                {
                    //assign all coordinates around the ship as observers
                    for (int i = 0; i < ship_size; i++)
                    {
                        c = ship_size_sequence.ToArray()[i];
                        battlefield = DisableNearbyCoordinates(battlefield, c.Row, c.Collumn, gameboardId);
                    }
                    //notify
                    battlefield.Notify();
                }
                return Ok();
            }
            else // register new empty coordinate
            {
                Coordinate coordinate_new = new Coordinate
                {
                    Row = coordinate.Row,
                    Collumn = coordinate.Collumn,
                    GameBoardId = gameboardId,
                    Occupation = Occupation.Missed
                };
                _context.Coordinates.Add(coordinate_new); //register coordinate as missed
                await _context.SaveChangesAsync();
            }

            return NoContent();//CreatedAtAction("GetCoordinate", new { id = coordinate.Id }, coordinate);
        }

        public Battlefield DisableNearbyCoordinates(Battlefield battlefield, int x, int y, long gameboardId)
        {
            battlefield = CheckCoord(x+1, y, battlefield, gameboardId);
            battlefield = CheckCoord(x-1, y, battlefield, gameboardId);
            battlefield = CheckCoord(x, y+1, battlefield, gameboardId);
            battlefield = CheckCoord(x, y-1, battlefield, gameboardId);

            battlefield = CheckCoord(x+1, y+1, battlefield, gameboardId);
            battlefield = CheckCoord(x-1, y-1, battlefield, gameboardId);
            battlefield = CheckCoord(x+1, y-1, battlefield, gameboardId);
            battlefield = CheckCoord(x-1, y+1, battlefield, gameboardId);

            return battlefield;
        }

        public Battlefield CheckCoord(int x, int y, Battlefield battlefield, long gameboardId)
        {
            var sequence = from coord in _context.Coordinates
                           where coord.Row.Equals(20) && coord.Collumn.Equals(20)
                           select coord;
            bool in_range = false;
            if (x < 10 && x > -1 && y > -1 && y < 10)
            {
                sequence = from coord in _context.Coordinates
                           where coord.Row.Equals(x) && coord.Collumn.Equals(y)
                           select coord;
                in_range = true;
            }
            if (sequence.Count() > 0)
            {
                if (sequence.First().Occupation == Occupation.Empty)
                {
                    battlefield.Attach(sequence.First());
                }
            } else if (in_range)
            {
                Coordinate coordinate_new = new Coordinate
                {
                    Row = x,
                    Collumn = y,
                    GameBoardId = gameboardId,
                    Occupation = Occupation.Missed
                };
                _context.Coordinates.Add(coordinate_new); //register coordinate as missed
                _context.SaveChanges();
                Console.WriteLine("\n\n New coord added as missed: " + coordinate_new.Row + " " + coordinate_new.Collumn);
            }
            return battlefield;
        }
        
    }
}
