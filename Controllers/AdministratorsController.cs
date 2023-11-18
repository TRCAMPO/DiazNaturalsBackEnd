using BACK_END_DIAZNATURALS.DTO;
using BACK_END_DIAZNATURALS.Encrypt;
using BACK_END_DIAZNATURALS.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

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
        [Authorize]
        public async Task<ActionResult<IEnumerable<AdministratorGetDTO>>> GetAdministrators()
        {
            if (_context.Administrators == null)
            {
                Log.Error($"Error en el acceso al servidor al intentar extraer informacion de administradores, cod error 500, Internal Server error");
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
        [Authorize]
        public async Task<ActionResult<AdministratorGetDTO>> GetAdministrator(int id)
        {
            if (_context.Administrators == null)
            {
                Log.Error($"Error en el acceso al servidor al intentar extraer informacion de administradores, cod error 500, Internal Server error");
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
        [Authorize]
        public async Task<IActionResult> PutAdministrator(string email, AdministratorEditDTO administratorDTO)
        {
            if (administratorDTO == null)
            {
                Log.Error($"Error en la peticion para editar el usuario {email}, cod error {NotFound().StatusCode}");
                return NotFound();
            }
            var administrator= _context.Administrators.FirstOrDefault(i=>i.EmailAdministrator == email);
            if (administrator == null)
            {
                Log.Warning($"Intento de cambio de nombre de usuario mediante el correo  {administratorDTO.EmailAdministrator}, al nombre {administratorDTO.NameAdministrator}");
                return NotFound(administratorDTO);
            }

            administrator.EmailAdministrator = administratorDTO.EmailAdministrator;
            administrator.NameAdministrator = administratorDTO.NameAdministrator;

            _context.Entry(administrator).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                Log.Warning($"Cambio de nombre de usuario mediante el correo  {administratorDTO.EmailAdministrator}, al nombre {administratorDTO.NameAdministrator}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdministratorExists(administrator.IdAdministrator)) return NotFound();
                else { throw; }
            }
            return NoContent();
        }



        [HttpPost]
        //[Authorize]
        public async Task<ActionResult<Administrator>> PostAdministrator(AdministratorDTO administrator)
        {
            if (_context.Administrators == null)
            {
                Log.Error($"Error en el acceso al servidor al intentar extraer informacion de administradores, cod error 500, Internal Server error");
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
            Log.Warning($"Se registra un nuevo administrador con el correo {administrator.EmailAdministrator}, y el nombre {administrator.NameAdministrator}");
            return CreatedAtAction("GetAdministrator", new { id = auxAdministrator.IdAdministrator }, administrator);
        }



        private bool AdministratorExists(int id)
        {
            return (_context.Administrators?.Any(e => e.IdAdministrator == id)).GetValueOrDefault();
        }
    }
}
