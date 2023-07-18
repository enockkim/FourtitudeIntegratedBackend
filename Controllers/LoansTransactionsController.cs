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
    public class LoanTransactionsController : ControllerBase
    {
        private readonly FourtitudeIntegratedContext _context;

        public LoanTransactionsController(FourtitudeIntegratedContext context)
        {
            _context = context;
        }

        private static Mapper mapper = MapperConfig.InitializeAutomapper();

        // GET: api/LoanTransactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanTransactions>>> GetLoanTransactions()
        {
            if (_context.LoanTransactions == null)
            {
                return NotFound();
            }
            else if (LocalCache.LoanTransactionsCache.Count == 0)
            {
                var LoanTransactions = await _context.LoanTransactions.ToListAsync();
                foreach (var loanTransaction in LoanTransactions)
                {
                    LocalCache.LoanTransactionsCache.Add(loanTransaction);
                }
            }

            return LocalCache.LoanTransactionsCache;
        }

        // GET: api/LoanTransactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoanTransactions>> GetLoanTransactions(int id)
        {
            if (_context.LoanTransactions == null)
            {
                return NotFound();
            }
            var LoanTransactions = await _context.LoanTransactions.FindAsync(id);

            if (LoanTransactions == null)
            {
                return NotFound();
            }

            return LoanTransactions;
        }

        // PUT: api/LoanTransactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoanTransactions(int id, LoanTransactions LoanTransactions)
        {
            if (id != LoanTransactions.LoanId)
            {
                return BadRequest();
            }

            _context.Entry(LoanTransactions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanTransactionsExists(id))
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

        // POST: api/LoanTransactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LoanTransactions>> PostLoanTransactions(LoanTransactions LoanTransactions)
        {
            if (_context.LoanTransactions == null)
            {
                return Problem("Entity set 'FourtitudeIntegratedContext.LoanTransactions'  is null.");
            }
            _context.LoanTransactions.Add(LoanTransactions);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLoanTransactions", new { id = LoanTransactions.LoanId }, LoanTransactions);
        }

        // DELETE: api/LoanTransactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoanTransactions(int id)
        {
            if (_context.LoanTransactions == null)
            {
                return NotFound();
            }
            var LoanTransactions = await _context.LoanTransactions.FindAsync(id);
            if (LoanTransactions == null)
            {
                return NotFound();
            }

            _context.LoanTransactions.Remove(LoanTransactions);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoanTransactionsExists(int id)
        {
            return (_context.LoanTransactions?.Any(e => e.LoanId == id)).GetValueOrDefault();
        }
    }
}
