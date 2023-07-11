using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FourtitudeIntegrated.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations.Schema;
using FourtitudeIntegrated.DbContexts;
using FourtitudeIntegrated.Cache;
using FourtitudeIntegrated.AutoMapper;

namespace FourtitudeIntegrated.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly FourtitudeIntegratedContext _context;

        public AccountsController(FourtitudeIntegratedContext context)
        {
            _context = context;
        }


        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAccountsDto>>> GetAccounts()
        {
            var accounts = await _context.Accounts.ToListAsync();
            if (accounts == null)
            {
                return NotFound();
            }

            if (LocalCache.AccountsCache.Count == 0)
            {
                var accountTypes = await _context.AccountTypes.ToListAsync();
                var accountCategory = await _context.AccountCategories.ToListAsync();
                var formatted = accounts
                    .Join(accountTypes,
                        account => account.AccountTypeId,
                        type => type.AccountTypeId,
                        (account, type) => new GetAccountsDto
                        {
                            AccId = account.AccountId,
                            AccName = account.AccountName,
                            AccType = type.TypeName,
                            AccCategory = accountCategory.First(a => a.AccountCategoryId == account.AccountCategoryId).CategoryName,
                            AccBalance = account.AccountBalance,
                            DateCreated = account.DateCreated,
                            CreatedBy = account.CreatedBy,
                            DateUpdated = account.DateUpdated,
                            UpdatedBy = account.UpdatedBy,
                            AccStatus = account.AccountStatus == 1 ? "Active" : "Inactive"
                        }).ToList();

                LocalCache.AccountsCache = formatted;
            }

            return LocalCache.AccountsCache;
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAccountsDto>> GetAccounts(string id)
        {
          if (_context.Accounts == null)
          {
              return NotFound();
          }
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            var accountTypes = await _context.AccountTypes.ToListAsync();

            var formattedAccount = new GetAccountsDto
            {
                AccId = account.AccountId,
                AccName = account.AccountName,
                AccType = accountTypes.Where(i => i.AccountTypeId == account.AccountTypeId).Select(i => i.TypeName).FirstOrDefault(),
                AccBalance = account.AccountBalance,
                DateCreated = account.DateCreated,
                CreatedBy = account.CreatedBy,
                DateUpdated = account.DateUpdated,
                UpdatedBy = account.UpdatedBy,
                AccStatus = account.AccountStatus == 1 ? "Active" : "Inactive"
            };

            return formattedAccount;
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccounts(int id, AccountsDTO AccountsDTO)
        {
            if (id != AccountsDTO.AccountId)
            {
                return BadRequest();
            }

            var Accounts = new Accounts
            {
                AccountId = AccountsDTO.AccountId,
                AccountTypeId = AccountsDTO.AccountTypeId,
                AccountName = AccountsDTO.AccountName,
                AccountBalance = AccountsDTO.AccountBalance,
                DateCreated = AccountsDTO.DateCreated,
                CreatedBy = AccountsDTO.CreatedBy,
                DateUpdated = AccountsDTO.DateUpdated,
                UpdatedBy = AccountsDTO.UpdatedBy,
                AccountStatus = AccountsDTO.AccountStatus
            };

            _context.Entry(Accounts).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountsExists(id))
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

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Accounts>> PostAccounts(AccountsDTO AccountsDTO)
        {
          if (_context.Accounts == null)
          {
              return Problem("Entity set 'FourtitudeIntegratedContext.Accounts'  is null.");
          }

            Accounts Accounts = new Accounts
            {
                AccountId = AccountsDTO.AccountId,
                AccountTypeId = AccountsDTO.AccountTypeId,
                AccountCategoryId = AccountsDTO.AccountCategoryId,
                AccountName = AccountsDTO.AccountName,
                AccountBalance = AccountsDTO.AccountBalance,
                DateCreated = AccountsDTO.DateCreated,
                CreatedBy = AccountsDTO.CreatedBy,
                AccountStatus = AccountsDTO.AccountStatus
            };

            _context.Accounts.Add(Accounts);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AccountsExists(Accounts.AccountId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            var newAcc = new GetAccountsDto
            {
                AccId = Accounts.AccountId,
                AccType = LocalCache.AccountTypesCache.Where(predicate: a => a.AccountTypeId == Accounts.AccountTypeId).Select(a => a.TypeName).FirstOrDefault(),
                AccCategory = LocalCache.AccountCategoriesCache.Where(predicate: a => a.AccountCategoryId == Accounts.AccountCategoryId).Select(a => a.CategoryName).FirstOrDefault(),
                AccName = Accounts.AccountName,
                AccBalance = Accounts.AccountBalance,
                DateCreated = Accounts.DateCreated,
                CreatedBy = Accounts.CreatedBy,
                DateUpdated = Accounts.DateUpdated,
                UpdatedBy = Accounts.UpdatedBy,
                AccStatus = Accounts.AccountStatus == 1 ? "Active" : "Inactive"
            };

            LocalCache.AccountsCache.Add(newAcc); //Add new acc to cache

            return CreatedAtAction("GetAccounts", new { id = Accounts.AccountId }, Accounts);
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccounts(string id)
        {
            if (_context.Accounts == null)
            {
                return NotFound();
            }
            var Accounts = await _context.Accounts.FindAsync(id);
            if (Accounts == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(Accounts);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountsExists(int id)
        {
            return (_context.Accounts?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }
    }
}
