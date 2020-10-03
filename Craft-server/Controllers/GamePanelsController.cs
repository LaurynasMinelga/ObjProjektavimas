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
    public class GamePanelsController : ControllerBase
    {
        private readonly GamePanelContext _context;

        public GamePanelsController(GamePanelContext context)
        {
            _context = context;
        }

        // GET: api/GamePanels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GamePanel>>> GetGamePanels()
        {
            return await _context.GamePanels.ToListAsync();
        }

        // GET: api/GamePanels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GamePanel>> GetGamePanel(long id)
        {
            var gamePanel = await _context.GamePanels.FindAsync(id);

            if (gamePanel == null)
            {
                return NotFound();
            }

            return gamePanel;
        }

        // PUT: api/GamePanels/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGamePanel(long id, GamePanel gamePanel)
        {
            if (id != gamePanel.Id)
            {
                return BadRequest();
            }

            _context.Entry(gamePanel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GamePanelExists(id))
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

        // POST: api/GamePanels
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<GamePanel>> PostGamePanel(GamePanel gamePanel)
        {
            _context.GamePanels.Add(gamePanel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGamePanel", new { id = gamePanel.Id }, gamePanel);
        }

        // DELETE: api/GamePanels/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GamePanel>> DeleteGamePanel(long id)
        {
            var gamePanel = await _context.GamePanels.FindAsync(id);
            if (gamePanel == null)
            {
                return NotFound();
            }

            _context.GamePanels.Remove(gamePanel);
            await _context.SaveChangesAsync();

            return gamePanel;
        }

        private bool GamePanelExists(long id)
        {
            return _context.GamePanels.Any(e => e.Id == id);
        }
    }
}
