using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FourtitudeIntegrated.DbContexts;
using FourtitudeIntegrated.Models;
using AutoMapper;
using FourtitudeIntegrated.Cache;
using FourtitudeIntegrated.AutoMapper;

namespace FourtitudeIntegrated.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContributionsController : ControllerBase
    {
        private readonly FourtitudeIntegratedContext _context;

        public ContributionsController(FourtitudeIntegratedContext context)
        {
            _context = context;
        }

        private static Mapper mapper = MapperConfig.InitializeAutomapper();

        // GET: api/Contributions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContributionsDTODetails>>> GetContributions()
        {
          if (_context.Contributions == null)
          {
              return NotFound();
          }
            var acc = await _context.Accounts.Where(a => a.AccountTypeId == 5).ToListAsync();
            var cont = await _context.Contributions.ToListAsync();
            var res = new List<ContributionsDTODetails>();

            foreach(var c in cont)
            {
                res.Add(new ContributionsDTODetails()
                {
                    ContributionId = c.ContributionId,
                    AccId = c.AccountId,
                    AccName = acc.Where(a => a.AccountId == c.AccountId).Select(a => a.AccountName).FirstOrDefault(),
                    AmountDue = c.AmountDue,
                    AmountPaid = c.AmountPaid,
                    DateDue = c.DateDue,
                    DateOfLastPayment = c.DateOfLastPayment,
                    PenaltyDue = c.PenaltyDue,
                    Status = c.Status.ToString()
                });
            }

            return res;
        }

        // GET: api/Contributions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contributions>> GetContributions(string id)
        {
          if (_context.Contributions == null)
          {
              return NotFound();
          }
            var Contributions = await _context.Contributions.FindAsync(id);

            if (Contributions == null)
            {
                return NotFound();
            }

            return Contributions;
        }

        // PUT: api/Contributions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContributions(string id, ContributionsDTO ContributionsUpdated)
        {
            if (id != ContributionsUpdated.ContributionId)
            {
                return BadRequest();
            }


            var Contributions = mapper.Map<Contributions>(ContributionsUpdated);

            _context.Entry(Contributions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContributionsExists(id))
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

        // POST: api/Contributions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ContributionsDTO>> PostContributions(ContributionsDTO ContributionsDto)
        {
          if (_context.Contributions == null)
          {
              return Problem("Entity set 'FourtitudeIntegratedContext.Contributions'  is null.");
          }

            var Contributions = mapper.Map<Contributions>(ContributionsDto);

            _context.Contributions.Add(Contributions);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ContributionsExists(Contributions.ContributionId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetContributions", new { id = Contributions.ContributionId }, Contributions);
        }

        // DELETE: api/Contributions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContributions(string id)
        {
            if (_context.Contributions == null)
            {
                return NotFound();
            }
            var Contributions = await _context.Contributions.FindAsync(id);
            if (Contributions == null)
            {
                return NotFound();
            }

            _context.Contributions.Remove(Contributions);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool ContributionsExists(string id)
        {
            return (_context.Contributions?.Any(e => e.ContributionId == id)).GetValueOrDefault();
        }
    }
}
