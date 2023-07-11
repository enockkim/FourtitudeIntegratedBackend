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
    public class AccountCategoriesController : ControllerBase
    {
        private readonly FourtitudeIntegratedContext _context;

        public AccountCategoriesController(FourtitudeIntegratedContext context)
        {
            _context = context;
        }

        private static Mapper mapper = MapperConfig.InitializeAutomapper();

        // GET: api/AccountCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountCategoriesDTO>>> GetAccountCategories()
        {
            if (_context.AccountCategories == null)
            {
                return NotFound();
            }
            else if (LocalCache.AccountCategoriesCache.Count == 0)
            {
                var accountCategories = await _context.AccountCategories.ToListAsync();
                foreach(var type in accountCategories)
                {
                    LocalCache.AccountCategoriesCache.Add(mapper.Map<AccountCategoriesDTO>(type));
                }
            }

            return LocalCache.AccountCategoriesCache;
        }

        // GET: api/AccountCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountCategories>> GetAccountCategories(int id)
        {
          if (_context.AccountCategories == null)
          {
              return NotFound();
          }
            var AccountCategories = await _context.AccountCategories.FindAsync(id);

            if (AccountCategories == null)
            {
                return NotFound();
            }

            return AccountCategories;
        }

        // PUT: api/AccountCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccountCategories(int id, AccountCategories AccountCategories)
        {
            if (id != AccountCategories.AccountCategoryId)
            {
                return BadRequest();
            }

            _context.Entry(AccountCategories).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountCategoriesExists(id))
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

        // POST: api/AccountCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AccountCategories>> PostAccountCategories(AccountCategories AccountCategories)
        {
          if (_context.AccountCategories == null)
          {
              return Problem("Entity set 'FourtitudeIntegratedContext.AccountCategories'  is null.");
          }
            _context.AccountCategories.Add(AccountCategories);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccountCategories", new { id = AccountCategories.AccountCategoryId }, AccountCategories);
        }

        // DELETE: api/AccountCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccountCategories(int id)
        {
            if (_context.AccountCategories == null)
            {
                return NotFound();
            }
            var AccountCategories = await _context.AccountCategories.FindAsync(id);
            if (AccountCategories == null)
            {
                return NotFound();
            }

            _context.AccountCategories.Remove(AccountCategories);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountCategoriesExists(int id)
        {
            return (_context.AccountCategories?.Any(e => e.AccountCategoryId == id)).GetValueOrDefault();
        }
    }
}
