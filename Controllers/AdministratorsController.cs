using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Firebase.Auth;
using BACK_END_DIAZNATURALS.DTO;
using BACK_END_DIAZNATURALS.Encrypt;
using BACK_END_DIAZNATURALS.Model;

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
        public async Task<ActionResult<IEnumerable<Administrator>>> GetAdministrators()
        {
          if (_context.Administrators == null)
          {
              return NotFound();
          }
            return await _context.Administrators.ToListAsync();
        }

    
        [HttpGet("{id}")]
        public async Task<ActionResult<Administrator>> GetAdministrator(int id)
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

            return administrator;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdministrator(int id, Administrator administrator)
        {
            if (id != administrator.IdAdministrator)
            {
                return BadRequest();
            }

            _context.Entry(administrator).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdministratorExists(id))
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
