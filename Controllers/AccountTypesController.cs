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
    public class AccountTypesController : ControllerBase
    {
        private readonly FourtitudeIntegratedContext _context;

        public AccountTypesController(FourtitudeIntegratedContext context)
        {
            _context = context;
        }

        private static Mapper mapper = MapperConfig.InitializeAutomapper();

        // GET: api/AccountTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountTypesDTO>>> GetAccountTypes()
        {
            if (_context.AccountTypes == null)
            {
                return NotFound();
            }
            else if (LocalCache.AccountTypesCache.Count == 0)
            {
                var accountTypes = await _context.AccountTypes.ToListAsync();
                foreach(var type in accountTypes)
                {
                    LocalCache.AccountTypesCache.Add(mapper.Map<AccountTypesDTO>(type));
                }
            }

            return LocalCache.AccountTypesCache;
        }

        // GET: api/AccountTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountTypes>> GetAccountTypes(int id)
        {
          if (_context.AccountTypes == null)
          {
              return NotFound();
          }
            var AccountTypes = await _context.AccountTypes.FindAsync(id);

            if (AccountTypes == null)
            {
                return NotFound();
            }

            return AccountTypes;
        }

        // PUT: api/AccountTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccountTypes(int id, AccountTypes AccountTypes)
        {
            if (id != AccountTypes.AccountTypeId)
            {
                return BadRequest();
            }

            _context.Entry(AccountTypes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountTypesExists(id))
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

        // POST: api/AccountTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AccountTypes>> PostAccountTypes(AccountTypes AccountTypes)
        {
          if (_context.AccountTypes == null)
          {
              return Problem("Entity set 'FourtitudeIntegratedContext.AccountTypes'  is null.");
          }
            _context.AccountTypes.Add(AccountTypes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccountTypes", new { id = AccountTypes.AccountTypeId }, AccountTypes);
        }

        // DELETE: api/AccountTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccountTypes(int id)
        {
            if (_context.AccountTypes == null)
            {
                return NotFound();
            }
            var AccountTypes = await _context.AccountTypes.FindAsync(id);
            if (AccountTypes == null)
            {
                return NotFound();
            }

            _context.AccountTypes.Remove(AccountTypes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountTypesExists(int id)
        {
            return (_context.AccountTypes?.Any(e => e.AccountTypeId == id)).GetValueOrDefault();
        }
    }
}
