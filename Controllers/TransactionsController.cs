using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FourtitudeIntegrated.Models;
using FourtitudeIntegrated.DbContexts;
using Microsoft.CodeAnalysis.Differencing;
using System.Diagnostics.Eventing.Reader;
using FourtitudeIntegrated.Services;

namespace FourtitudeIntegrated.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly FourtitudeIntegratedContext _context;
        private readonly TransactionService transactionService;

        public TransactionsController(FourtitudeIntegratedContext context, TransactionService transactionService)
        {
            _context = context;
            this.transactionService = transactionService;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ViewTransactionsDTO>>> GetTransactions()
        {
          if (_context.Transactions == null)
          {
              return NotFound();
          }
            //return await _context.Transactions.Join;

            var res = await _context.Transactions
                        .Join(
                            _context.GeneralLedger,
                            t => t.TransactionId,
                            gl => gl.TransactionId,
                            (t, gl) => new ViewTransactionsDTO
                            {
                                TransactionId = t.TransactionId,
                                TransactionDate = t.TransactionDate,
                                TransactionRef = t.TransactionRef,
                                Description = t.Description,
                                TransactionType = t.TransactionType.ToString(),
                                Amount = gl.Amount,
                                AccountId = gl.AccountId
                            })
                        .ToListAsync();

            return res;
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transactions>> GetTransactions(long id)
        {
          if (_context.Transactions == null)
          {
              return NotFound();
          }
            var Transactions = await _context.Transactions.FindAsync(id);

            if (Transactions == null)
            {
                return NotFound();
            }

            return Transactions;
        }

        // PUT: api/Transactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransactions(long id, Transactions Transactions)
        {
            if (id != Transactions.TransactionId)
            {
                return BadRequest();
            }

            _context.Entry(Transactions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionsExists(id))
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

        // POST: api/Transactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ViewTransactionsDTO>> PostTransactions(NewTransacton NewTransaction)
        {
          if (_context.Transactions == null)
          {
              return Problem("Entity set 'FourtitudeIntegratedContext.Transactions'  is null.");
          }

          var res = await transactionService.AddTransaction(NewTransaction);

          return CreatedAtAction("GetTransactions", new { id = res.TransactionId }, res);
        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactions(long id)
        {
            if (_context.Transactions == null)
            {
                return NotFound();
            }
            var Transactions = await _context.Transactions.FindAsync(id);
            if (Transactions == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(Transactions);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionsExists(long id)
        {
            return (_context.Transactions?.Any(e => e.TransactionId == id)).GetValueOrDefault();
        }
    }
}
