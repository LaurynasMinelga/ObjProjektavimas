using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Craft_server.Logger
{
    [Route("[controller]")]
    [ApiController]
    public class LoggerModelsController : ControllerBase
    {
        private readonly LoggerContext _context;

        public LoggerModelsController(LoggerContext context)
        {
            _context = context;
        }

        // GET: api/LoggerModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoggerModel>>> GetCoordinates()
        {
            return await _context.Coordinates.ToListAsync();
        }

        // GET: api/LoggerModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoggerModel>> GetLoggerModel(long id)
        {
            var loggerModel = await _context.Coordinates.FindAsync(id);

            if (loggerModel == null)
            {
                return NotFound();
            }

            return loggerModel;
        }

        // PUT: api/LoggerModels/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoggerModel(long id, LoggerModel loggerModel)
        {
            if (id != loggerModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(loggerModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoggerModelExists(id))
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

        // POST: api/LoggerModels
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<LoggerModel>> PostLoggerModel(LoggerModel loggerModel)
        {
            _context.Coordinates.Add(loggerModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLoggerModel", new { id = loggerModel.Id }, loggerModel);
        }

        // DELETE: api/LoggerModels/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<LoggerModel>> DeleteLoggerModel(long id)
        {
            var loggerModel = await _context.Coordinates.FindAsync(id);
            if (loggerModel == null)
            {
                return NotFound();
            }

            _context.Coordinates.Remove(loggerModel);
            await _context.SaveChangesAsync();

            return loggerModel;
        }

        private bool LoggerModelExists(long id)
        {
            return _context.Coordinates.Any(e => e.Id == id);
        }
    }
}
