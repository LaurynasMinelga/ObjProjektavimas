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
    public class GameBoardsController : ControllerBase
    {
        private readonly GameBoardContext _context;

        public GameBoardsController(GameBoardContext context)
        {
            _context = context;
        }

        // GET: api/GameBoards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameBoard>>> GetGameBoards()
        {
            return await _context.GameBoards.ToListAsync();
        }

        // GET: api/GameBoards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameBoard>> GetGameBoard(long id)
        {
            var gameBoard = await _context.GameBoards.FindAsync(id);

            if (gameBoard == null)
            {
                return NotFound();
            }

            return gameBoard;
        }

        // PUT: api/GameBoards/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGameBoard(long id, GameBoard gameBoard)
        {
            if (id != gameBoard.Id)
            {
                return BadRequest();
            }

            _context.Entry(gameBoard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameBoardExists(id))
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

        // POST: api/GameBoards
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<GameBoard>> PostGameBoard(GameBoard gameBoard)
        {
            _context.GameBoards.Add(gameBoard);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGameBoard", new { id = gameBoard.Id }, gameBoard);
        }

        // DELETE: api/GameBoards/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GameBoard>> DeleteGameBoard(long id)
        {
            var gameBoard = await _context.GameBoards.FindAsync(id);
            if (gameBoard == null)
            {
                return NotFound();
            }

            _context.GameBoards.Remove(gameBoard);
            await _context.SaveChangesAsync();

            return gameBoard;
        }

        private bool GameBoardExists(long id)
        {
            return _context.GameBoards.Any(e => e.Id == id);
        }
    }
}
