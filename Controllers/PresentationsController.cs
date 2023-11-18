using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BACK_END_DIAZNATURALS.Model;
using BACK_END_DIAZNATURALS.DTO;
using Microsoft.AspNetCore.Authorization;
using Serilog;

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
        [Authorize]
        public async Task<ActionResult<IEnumerable<PresentationDTO>>> GetPresentations()
        {
            if (_context.Presentations == null)
            {
                Log.Error($"Error en el acceso al servidor al intentar extraer informacion de Presentations, cod error 500, Internal Server error");
                return NotFound();
            }
            var presentationDTOs = await _context.Presentations
           .Select(c => new PresentationDTO
           {
               IdPresentation = c.IdPresentation,
               NamePresentation = c.NamePresentation,
           })
           .ToListAsync();
            return Ok(presentationDTOs);
        }



        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<PresentationDTO>> GetPresentation(int id)
        {
            if (_context.Presentations == null)
            {
                Log.Error($"Error en el acceso al servidor al intentar extraer informacion de Presentations, cod error 500, Internal Server error");
                return NotFound();
            }
            var presentation = await _context.Presentations.FindAsync(id);

            if (presentation == null)
            {
                return NotFound();
            }
            PresentationDTO presentationDTO = new PresentationDTO
            {
                IdPresentation = presentation.IdPresentation,
                NamePresentation = presentation.NamePresentation,
            };
            return presentationDTO;
        }



        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutPresentation(int id, PresentationDTO presentationDTO)
        {
            if (id != presentationDTO.IdPresentation)
            {
                Log.Error($"Error en el contenido de la peticion para editar la presentacion, {id}, " + $"cod error {BadRequest().StatusCode}");
                return BadRequest();
            }
            var presentation = _context.Presentations.Find(id);
            if (presentation == null)
            {
                Log.Error($"Presentacion no encontrada: {id}, Cod error {NotFound().StatusCode}");
                return NotFound();
            }
            presentation.NamePresentation = presentationDTO.NamePresentation;
            _context.Entry(presentation).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                Log.Information("Información de la presentacion actualizada: {@Presentation}", presentationDTO);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PresentationExists(id))
                {
                    return NotFound();
                }
                else { throw; }
            }
            return NoContent();
        }



        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Presentation>> PostPresentation(PresentationAddDTO presentationDTO)
        {
            if (_context.Presentations == null)
            {
                Log.Error($"Error en el acceso al servidor al intentar extraer informacion de Presentations, cod error 500, Internal Server error");
                return Problem("Entity set 'DiazNaturalsContext.Presentations'  is null.");
            }
            if (presentationDTO == null) return NoContent();
            if (PresentationExistsName(presentationDTO.NamePresentation))
            {
                Log.Information($"Se intento agregar una presentacion que ya existe: {presentationDTO.NamePresentation}, cod error {Conflict().StatusCode}");
                return new ConflictObjectResult("Ya existe la presentacion que intenta crear");
            }
            var presentation = new Presentation
            {
                NamePresentation = presentationDTO.NamePresentation,
            };
            _context.Presentations.Add(presentation);
            await _context.SaveChangesAsync();
            Log.Information($"Se agrego la presentacion: {presentationDTO.NamePresentation}");
            return CreatedAtAction("GetPresentation", new { id = presentation.IdPresentation }, presentation);
        }



        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePresentation(int id)
        {
            if (_context.Presentations == null) return NotFound();
            var presentation = await _context.Presentations.FindAsync(id);
            if (presentation == null) return NotFound();
            _context.Presentations.Remove(presentation);
            await _context.SaveChangesAsync();
            return NoContent();
        }



        private bool PresentationExists(int id)
        {
            return (_context.Presentations?.Any(e => e.IdPresentation == id)).GetValueOrDefault();
        }

        private bool PresentationExistsName(string name)
        {
            return (_context.Presentations?.Any(e => e.NamePresentation == name)).GetValueOrDefault();
        }
    }
}
