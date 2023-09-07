using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BACK_END_DIAZNATURALS.Model;
using BACK_END_DIAZNATURALS.DTO;

namespace BACK_END_DIAZNATURALS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PresentationsController : ControllerBase
    {
        private readonly DiazNaturalsContext _context;

        public PresentationsController(DiazNaturalsContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Presentation>>> GetPresentations()
        {
          if (_context.Presentations == null)
          {
              return NotFound();
          }
            return await _context.Presentations.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Presentation>> GetPresentation(int id)
        {
          if (_context.Presentations == null)
          {
              return NotFound();
          }
            var presentation = await _context.Presentations.FindAsync(id);

            if (presentation == null)
            {
                return NotFound();
            }

            return presentation;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutPresentation(int id, Presentation presentation)
        {
            if (id != presentation.IdPresentation)
            {
                return BadRequest();
            }

            _context.Entry(presentation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PresentationExists(id))
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


        [HttpPost]
        public async Task<ActionResult<Presentation>> PostPresentation(PresentationAddDTO presentationDTO)
        {
          if (_context.Presentations == null)
          {
              return Problem("Entity set 'DiazNaturalsContext.Presentations'  is null.");
          }
          if (presentationDTO == null)
            {
                return NoContent();
            }
            var presentation = new Presentation
            {
                NamePresentation = presentationDTO.NamePresentation,
            };

            _context.Presentations.Add(presentation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPresentation", new { id = presentation.IdPresentation }, presentation);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePresentation(int id)
        {
            if (_context.Presentations == null)
            {
                return NotFound();
            }
            var presentation = await _context.Presentations.FindAsync(id);
            if (presentation == null)
            {
                return NotFound();
            }

            _context.Presentations.Remove(presentation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PresentationExists(int id)
        {
            return (_context.Presentations?.Any(e => e.IdPresentation == id)).GetValueOrDefault();
        }
    }
}
