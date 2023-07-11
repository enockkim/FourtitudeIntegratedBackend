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

namespace FourtitudeIntegrated.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly FourtitudeIntegratedContext _context;

        public TransactionsController(FourtitudeIntegratedContext context)
        {
            _context = context;
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

            List<GeneralLedger> GeneralLedgerEntries = new List<GeneralLedger>();

            //Create a transaction record
            Transactions Transactions = _context.Transactions.Add(new Transactions()
            {
                TransactionDate = NewTransaction.TransactionDetails.TransactionDate,
                TransactionType = NewTransaction.TransactionDetails.TransactionType,
                Description = NewTransaction.TransactionDetails.Description,
                TransactionRef = NewTransaction.TransactionDetails.TransactionRef,
            }).Entity;

            await _context.SaveChangesAsync();

            //Create a general ledger entry
            if (NewTransaction.TransactionDetails.TransactionType == Enum.TransactionType.Transfer)
            {
                //Debit
                var debit = new GeneralLedger()
                {
                    AccountId = (int)NewTransaction.AccountTo,
                    Amount = NewTransaction.TransactionDetails.Amount,
                    UserId = NewTransaction.TransactionDetails.UserId,
                    TransactionId = Transactions.TransactionId,
                    EntryType = Enum.EntryType.Debit
                };
                GeneralLedgerEntries.Add(debit);

                //Credit
                var credit = new GeneralLedger()
                {
                    AccountId = (int)NewTransaction.AccountFrom,
                    Amount = NewTransaction.TransactionDetails.Amount,
                    UserId = NewTransaction.TransactionDetails.UserId,
                    TransactionId = Transactions.TransactionId,
                    EntryType = Enum.EntryType.Credit
                };
                GeneralLedgerEntries.Add(credit);
            }
            else
            {
                var DepositOrWithdrawal = new GeneralLedger()
                {
                    AccountId = (int)(NewTransaction.AccountFrom ?? NewTransaction.AccountTo),
                    Amount = NewTransaction.TransactionDetails.Amount,
                    UserId = NewTransaction.TransactionDetails.UserId,
                    TransactionId = Transactions.TransactionId,
                    EntryType = NewTransaction.TransactionDetails.TransactionType == Enum.TransactionType.Withdrawal ? Enum.EntryType.Credit : Enum.EntryType.Debit
                };
                GeneralLedgerEntries.Add(DepositOrWithdrawal);
            }

            foreach(var entry in GeneralLedgerEntries)
            {
                _context.GeneralLedger.Add(entry);
            }

            await _context.SaveChangesAsync();

            ViewTransactionsDTO viewTransactionsDTO = new ViewTransactionsDTO()
            {
                TransactionId = Transactions.TransactionId,
                TransactionDate = Transactions.TransactionDate,
                TransactionRef = Transactions.TransactionRef,
                Description = Transactions.Description,
                TransactionType = Transactions.TransactionType.ToString(),
                Amount = NewTransaction.TransactionDetails.Amount,
                //AccountId = Transactions.
            };

            return CreatedAtAction("GetTransactions", new { id = viewTransactionsDTO.TransactionId }, viewTransactionsDTO);
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
