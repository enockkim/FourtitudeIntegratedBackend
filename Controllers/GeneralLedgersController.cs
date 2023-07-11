using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FourtitudeIntegrated.Models;
using FourtitudeIntegrated.DbContexts;

namespace FourtitudeIntegrated.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralLedgersController : ControllerBase
    {
        private readonly FourtitudeIntegratedContext _context;

        public GeneralLedgersController(FourtitudeIntegratedContext context)
        {
            _context = context;
        }

        // GET: api/GeneralLedgers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeneralLedger>>> GetGeneralLedger()
        {
          if (_context.GeneralLedger == null)
          {
              return NotFound();
          }
            return await _context.GeneralLedger.ToListAsync();
        }

        // GET: api/GeneralLedgers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GeneralLedger>> GetGeneralLedger(int id)
        {
          if (_context.GeneralLedger == null)
          {
              return NotFound();
          }
            var GeneralLedger = await _context.GeneralLedger.FindAsync(id);

            if (GeneralLedger == null)
            {
                return NotFound();
            }

            return GeneralLedger;
        }

        // PUT: api/GeneralLedgers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGeneralLedger(int id, GeneralLedger GeneralLedger)
        {
            if (id != GeneralLedger.EntryNo)
            {
                return BadRequest();
            }

            _context.Entry(GeneralLedger).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GeneralLedgerExists(id))
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

        // POST: api/GeneralLedgers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GeneralLedger>> PostGeneralLedger(GeneralLedger GeneralLedger)
        {
          if (_context.GeneralLedger == null)
          {
              return Problem("Entity set 'FourtitudeIntegratedContext.GeneralLedger'  is null.");
          }
            _context.GeneralLedger.Add(GeneralLedger);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeneralLedger", new { id = GeneralLedger.EntryNo }, GeneralLedger);
        }

        // DELETE: api/GeneralLedgers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGeneralLedger(int id)
        {
            if (_context.GeneralLedger == null)
            {
                return NotFound();
            }
            var GeneralLedger = await _context.GeneralLedger.FindAsync(id);
            if (GeneralLedger == null)
            {
                return NotFound();
            }

            _context.GeneralLedger.Remove(GeneralLedger);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GeneralLedgerExists(int id)
        {
            return (_context.GeneralLedger?.Any(e => e.EntryNo == id)).GetValueOrDefault();
        }
    }
}
