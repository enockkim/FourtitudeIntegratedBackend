using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FourtitudeIntegrated.Models;
using FourtitudeIntegrated.DbContexts;
using FourtitudeIntegrated.Cache;
using FourtitudeIntegrated.AutoMapper;
using AutoMapper;

namespace FourtitudeIntegrated.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly FourtitudeIntegratedContext _context;

        public LoansController(FourtitudeIntegratedContext context)
        {
            _context = context;
        }

        private static Mapper mapper = MapperConfig.InitializeAutomapper();

        // GET: api/Loans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loans>>> GetLoans()
        {
            if (_context.Loans == null)
            {
                return NotFound();
            }
            else if (LocalCache.LoansCache.Count == 0)
            {
                var Loans = await _context.Loans.ToListAsync();
                foreach(var loan in Loans)
                {
                    LocalCache.LoansCache.Add(loan);
                }
            }

            return LocalCache.LoansCache;
        }

        // GET: api/Loans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Loans>> GetLoans(int id)
        {
          if (_context.Loans == null)
          {
              return NotFound();
          }
            var Loans = await _context.Loans.FindAsync(id);

            if (Loans == null)
            {
                return NotFound();
            }

            return Loans;
        }

        // PUT: api/Loans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoans(int id, Loans Loans)
        {
            if (id != Loans.LoanId)
            {
                return BadRequest();
            }

            _context.Entry(Loans).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoansExists(id))
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

        // POST: api/Loans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Loans>> PostLoans(Loans Loans)
        {
          if (_context.Loans == null)
          {
              return Problem("Entity set 'FourtitudeIntegratedContext.Loans'  is null.");
          }
            _context.Loans.Add(Loans);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLoans", new { id = Loans.LoanId }, Loans);
        }

        // DELETE: api/Loans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoans(int id)
        {
            if (_context.Loans == null)
            {
                return NotFound();
            }
            var Loans = await _context.Loans.FindAsync(id);
            if (Loans == null)
            {
                return NotFound();
            }

            _context.Loans.Remove(Loans);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoansExists(int id)
        {
            return (_context.Loans?.Any(e => e.LoanId == id)).GetValueOrDefault();
        }
    }
}
