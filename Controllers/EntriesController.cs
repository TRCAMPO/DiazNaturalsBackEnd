using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BACK_END_DIAZNATURALS.Model;

namespace BACK_END_DIAZNATURALS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntriesController : ControllerBase
    {
        private readonly DiazNaturalsContext _context;

        public EntriesController(DiazNaturalsContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entry>>> GetEntries()
        {
          if (_context.Entries == null)
          {
              return NotFound();
          }
            return await _context.Entries.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Entry>> GetEntry(int id)
        {
          if (_context.Entries == null)
          {
              return NotFound();
          }
            var entry = await _context.Entries.FindAsync(id);

            if (entry == null)
            {
                return NotFound();
            }

            return entry;
        }


       /* [HttpPut("{id}")]
        public async Task<IActionResult> PutEntry(int id, Entry entry)
        {
            if (id != entry.IdEntry)
            {
                return BadRequest();
            }

            _context.Entry(entry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntryExists(id))
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
        public async Task<ActionResult<Entry>> PostEntry(Entry entry)
        {
          if (_context.Entries == null)
          {
              return Problem("Entity set 'DiazNaturalsContext.Entries'  is null.");
          }
            _context.Entries.Add(entry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEntry", new { id = entry.IdEntry }, entry);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntry(int id)
        {
            if (_context.Entries == null)
            {
                return NotFound();
            }
            var entry = await _context.Entries.FindAsync(id);
            if (entry == null)
            {
                return NotFound();
            }

            _context.Entries.Remove(entry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EntryExists(int id)
        {
            return (_context.Entries?.Any(e => e.IdEntry == id)).GetValueOrDefault();
        }*/
    }
}
