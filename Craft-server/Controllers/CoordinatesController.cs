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
    public class CoordinatesController : ControllerBase
    {
        private readonly CoordinateContext _context;

        public CoordinatesController(CoordinateContext context)
        {
            _context = context;
        }

        // GET: api/Coordinates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Coordinate>>> GetCoordinates()
        {
            return await _context.Coordinates.ToListAsync();
        }

        // GET: api/Coordinates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Coordinate>> GetCoordinate(long id)
        {
            var coordinate = await _context.Coordinates.FindAsync(id);

            if (coordinate == null)
            {
                return NotFound();
            }

            return coordinate;
        }

        // PUT: api/Coordinates/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCoordinate(long id, Coordinate coordinate)
        {
            if (id != coordinate.Id)
            {
                return BadRequest();
            }

            _context.Entry(coordinate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoordinateExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Coordinates
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Coordinate>> PostCoordinate(Coordinate coordinate)
        {
            _context.Coordinates.Add(coordinate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCoordinate", new { id = coordinate.Id }, coordinate);
        }

        // DELETE: api/Coordinates/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Coordinate>> DeleteCoordinate(long id)
        {
            var coordinate = await _context.Coordinates.FindAsync(id);
            if (coordinate == null)
            {
                return NotFound();
            }

            _context.Coordinates.Remove(coordinate);
            await _context.SaveChangesAsync();

            return coordinate;
        }

        private bool CoordinateExists(long id)
        {
            return _context.Coordinates.Any(e => e.Id == id);
        }
    }
}
