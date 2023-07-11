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
    public class DocumentsController : ControllerBase
    {
        private readonly FourtitudeIntegratedContext _context;

        public DocumentsController(FourtitudeIntegratedContext context)
        {
            _context = context;
        }

        // GET: api/Documents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Documents>>> GetDocuments()
        {
          if (_context.Documents == null)
          {
              return NotFound();
          }
            return await _context.Documents.ToListAsync();
        }

        // GET: api/Documents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Documents>> GetDocuments(int id)
        {
          if (_context.Documents == null)
          {
              return NotFound();
          }
            var Documents = await _context.Documents.FindAsync(id);

            if (Documents == null)
            {
                return NotFound();
            }

            return Documents;
        }

        // PUT: api/Documents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocuments(int id, Documents Documents)
        {
            if (id != Documents.DocumentId)
            {
                return BadRequest();
            }

            _context.Entry(Documents).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentsExists(id))
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

        // POST: api/Documents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Documents>> PostDocuments(Documents Documents)
        {
          if (_context.Documents == null)
          {
              return Problem("Entity set 'FourtitudeIntegratedContext.Documents'  is null.");
          }
            _context.Documents.Add(Documents);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocuments", new { id = Documents.DocumentId }, Documents);
        }

        // DELETE: api/Documents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocuments(int id)
        {
            if (_context.Documents == null)
            {
                return NotFound();
            }
            var Documents = await _context.Documents.FindAsync(id);
            if (Documents == null)
            {
                return NotFound();
            }

            _context.Documents.Remove(Documents);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocumentsExists(int id)
        {
            return (_context.Documents?.Any(e => e.DocumentId == id)).GetValueOrDefault();
        }
    }
}
