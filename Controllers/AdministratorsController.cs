using BACK_END_DIAZNATURALS.DTO;
using BACK_END_DIAZNATURALS.Encrypt;
using BACK_END_DIAZNATURALS.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BACK_END_DIAZNATURALS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorsController : ControllerBase
    {
        private readonly DiazNaturalsContext _context;



        public AdministratorsController(DiazNaturalsContext context)
        {
            _context = context;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdministratorGetDTO>>> GetAdministrators()
        {
            if (_context.Administrators == null)
            {
                return NotFound();
            }
            var administrator = await _context.Administrators.Select(p => new AdministratorGetDTO
            {
                IdAdministrator = p.IdAdministrator,
                NameAdministrator = p.NameAdministrator,
                EmailAdministrator = p.EmailAdministrator,
            }).ToListAsync();
            return Ok(administrator);
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<AdministratorGetDTO>> GetAdministrator(int id)
        {
            if (_context.Administrators == null)
            {
                return NotFound();
            }
            var administrator = await _context.Administrators.FindAsync(id);

            if (administrator == null)
            {
                return NotFound();
            }
            var administratorDTO = new AdministratorGetDTO
            {
                EmailAdministrator = administrator.EmailAdministrator,
                NameAdministrator = administrator.NameAdministrator,
                IdAdministrator = administrator.IdAdministrator,
            };
            return Ok(administrator);
        }



        [HttpPut("{email}")]
        public async Task<IActionResult> PutAdministrator(string email, AdministratorEditDTO administratorDTO)
        {
            if(administratorDTO == null)return NotFound();
            var administrator= _context.Administrators.FirstOrDefault(i=>i.EmailAdministrator == email);
            if (administrator == null) return NotFound(administratorDTO);
            administrator.EmailAdministrator = administratorDTO.EmailAdministrator;
            administrator.NameAdministrator = administratorDTO.NameAdministrator;

            _context.Entry(administrator).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdministratorExists(administrator.IdAdministrator)) return NotFound();
                else { throw; }
            }
            return NoContent();
        }



        [HttpPost]
        public async Task<ActionResult<Administrator>> PostAdministrator(AdministratorDTO administrator)
        {
            if (_context.Administrators == null)
            {
                return Problem("Entity set 'DiazNaturalsContext.Administrators'  is null.");
            }

            HashedFormat hash = HashEncryption.Hash(administrator.PasswordAdministrator);
            var credential = new Credential()
            {
                PasswordCredential = hash.Password,
                SaltCredential = hash.HashAlgorithm
            };

            _context.Credentials.Add(credential);
            await _context.SaveChangesAsync();
            int id = credential.IdCredential;
            var auxAdministrator = new Administrator()
            {
                NameAdministrator = administrator.NameAdministrator,
                EmailAdministrator = administrator.EmailAdministrator,
                IdCredential = id,
            };
            _context.Administrators.Add(auxAdministrator);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetAdministrator", new { id = auxAdministrator.IdAdministrator }, administrator);
        }



        private bool AdministratorExists(int id)
        {
            return (_context.Administrators?.Any(e => e.IdAdministrator == id)).GetValueOrDefault();
        }
    }
}
