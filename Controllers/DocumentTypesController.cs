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
    public class DocumentTypesController : ControllerBase
    {
        private readonly FourtitudeIntegratedContext _context;

        public DocumentTypesController(FourtitudeIntegratedContext context)
        {
            _context = context;
        }

        // GET: api/DocumentTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentTypes>>> GetDocumentTypes()
        {
          if (_context.DocumentTypes == null)
          {
              return NotFound();
          }
            return await _context.DocumentTypes.ToListAsync();
        }

        // GET: api/DocumentTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentTypes>> GetDocumentTypes(int id)
        {
          if (_context.DocumentTypes == null)
          {
              return NotFound();
          }
            var DocumentTypes = await _context.DocumentTypes.FindAsync(id);

            if (DocumentTypes == null)
            {
                return NotFound();
            }

            return DocumentTypes;
        }

        // PUT: api/DocumentTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocumentTypes(int id, DocumentTypes DocumentTypes)
        {
            if (id != DocumentTypes.DocumentTypeId)
            {
                return BadRequest();
            }

            _context.Entry(DocumentTypes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentTypesExists(id))
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

        // POST: api/DocumentTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DocumentTypes>> PostDocumentTypes(DocumentTypes DocumentTypes)
        {
          if (_context.DocumentTypes == null)
          {
              return Problem("Entity set 'FourtitudeIntegratedContext.DocumentTypes'  is null.");
          }
            _context.DocumentTypes.Add(DocumentTypes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocumentTypes", new { id = DocumentTypes.DocumentTypeId }, DocumentTypes);
        }

        // DELETE: api/DocumentTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocumentTypes(int id)
        {
            if (_context.DocumentTypes == null)
            {
                return NotFound();
            }
            var DocumentTypes = await _context.DocumentTypes.FindAsync(id);
            if (DocumentTypes == null)
            {
                return NotFound();
            }

            _context.DocumentTypes.Remove(DocumentTypes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocumentTypesExists(int id)
        {
            return (_context.DocumentTypes?.Any(e => e.DocumentTypeId == id)).GetValueOrDefault();
        }
    }
}
