using BACK_END_DIAZNATURALS.DTO;
using BACK_END_DIAZNATURALS.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BACK_END_DIAZNATURALS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly DiazNaturalsContext _context;



        public SuppliersController(DiazNaturalsContext context)
        {
            _context = context;
        }




        [HttpGet]
        [Route("all")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetAllSuppliers()
        {
            var supplierDTOs = await _context.Suppliers
              .Select(c => new SupplierDTO
              {
                  IdSupplier = c.IdSupplier,
                  NameSupplier = c.NameSupplier,
                  AddressSupplier = c.AddressSupplier,
                  EmailSupplier = c.EmailSupplier,
                  NitSupplier = c.NitSupplier,
                  PhoneSupplier = c.PhoneSupplier
              })
              .ToListAsync();
            return Ok(supplierDTOs);
        }




        [HttpGet]
        [Route("active")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetActiveSuppliers()
        {
            if (_context.Suppliers == null) return NotFound();
            var supplierDTOs = await _context.Suppliers
                .Where(c => c.IsActiveSupplier == true)
              .Select(c => new SupplierDTO
              {
                  IdSupplier = c.IdSupplier,
                  NameSupplier = c.NameSupplier,
                  AddressSupplier = c.AddressSupplier,
                  EmailSupplier = c.EmailSupplier,
                  NitSupplier = c.NitSupplier,
                  PhoneSupplier = c.PhoneSupplier
              })
              .ToListAsync();
            return Ok(supplierDTOs);
        }




        [HttpGet]
        [Route("inactive")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetInactiveSuppliers()
        {
            if (_context.Suppliers == null) return NotFound();
            var supplierDTOs = await _context.Suppliers
                .Where(c => c.IsActiveSupplier == false)
              .Select(c => new SupplierDTO
              {
                  IdSupplier = c.IdSupplier,
                  NameSupplier = c.NameSupplier,
                  AddressSupplier = c.AddressSupplier,
                  EmailSupplier = c.EmailSupplier,
                  NitSupplier = c.NitSupplier,
                  PhoneSupplier = c.PhoneSupplier
              })
              .ToListAsync();
            return Ok(supplierDTOs);
        }




        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Supplier>> GetSupplier(int id)
        {
            if (_context.Suppliers == null) return NotFound();
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null || !supplier.IsActiveSupplier) return NotFound();
            SupplierDTO supplierDTO = new SupplierDTO
            {
                IdSupplier = supplier.IdSupplier,
                NameSupplier = supplier.NameSupplier,
                AddressSupplier = supplier.AddressSupplier,
                EmailSupplier = supplier.EmailSupplier,
                PhoneSupplier = supplier.PhoneSupplier,
                NitSupplier = supplier.NitSupplier,
            };
            return Ok(supplier);
        }




        [HttpGet()]
        [Route("search")]
        [Authorize]
        public async Task<ActionResult<SupplierDTO>> GetSearchClients(string search)
        {
            Supplier supplier = SearchClient(search);
            if (supplier==null) return NotFound();
            if (!supplier.IsActiveSupplier) return NotFound();
            var supplierDTO = new SupplierDTO
            {
                IdSupplier = supplier.IdSupplier,
                NameSupplier = supplier.NameSupplier,
                AddressSupplier = supplier.AddressSupplier,
                EmailSupplier = supplier.EmailSupplier,
                PhoneSupplier = supplier.PhoneSupplier,
                NitSupplier = supplier.NitSupplier,
            };
            return Ok(supplierDTO);
        }



        private Supplier SearchClient(string name)
        {
            var supplier = _context.Suppliers.FirstOrDefault(i => i.NameSupplier == name);
            if (supplier == null)
            {
                supplier = _context.Suppliers.FirstOrDefault(c => c.NitSupplier == name);
            }
            return supplier;
        }




        [HttpPut("{idSupplier}")]
        [Authorize]
        public async Task<IActionResult> PutSupplier([Required] int  idSupplier, SupplierAddDTO supplierDTO)
        {
            if (supplierDTO == null) return BadRequest();
            if(supplierDTO.NameSupplier==null || supplierDTO.AddressSupplier==null || supplierDTO.EmailSupplier==null || supplierDTO.NitSupplier==null || supplierDTO.PhoneSupplier==null) return BadRequest();
            if (SupplierNameExistsEdit(supplierDTO.NameSupplier, supplierDTO.IdSupplier)) return Conflict("El Nombre del proveedor \"" + supplierDTO.NameSupplier + "\" ya existe.");
            if (SupplierNitExistsEdit(supplierDTO.NitSupplier, supplierDTO.IdSupplier)) return Conflict("El Nit del proveedor \"" + supplierDTO.NitSupplier + "\" ya existe.");
            var supplier = _context.Suppliers.FirstOrDefault(c => c.IdSupplier == idSupplier);
            if (supplier == null || !supplier.IsActiveSupplier) return NotFound();
            supplier.NameSupplier = supplierDTO.NameSupplier;
            supplier.AddressSupplier = supplierDTO.AddressSupplier;
            supplier.EmailSupplier = supplierDTO.EmailSupplier;
            supplier.PhoneSupplier = supplierDTO.PhoneSupplier;
            supplier.NitSupplier = supplierDTO.NitSupplier;
            _context.Entry(supplier).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(supplier.IdSupplier)) return NotFound();
                else { throw; }
            }
            return NoContent();
        }




        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Supplier>> PostSupplier(SupplierAddDTO supplierDTO)
        {
            if (_context.Suppliers == null) return Problem("Entity set 'DiazNaturalsContext.Suppliers'  is null.");
            if (supplierDTO == null) return BadRequest();
            if(SupplierNameExists(supplierDTO.NameSupplier))return Conflict("El Nombre del proveedor \""+ supplierDTO.NameSupplier+"\" ya existe.");
            if(SupplierNitExists(supplierDTO.NitSupplier))return Conflict("El Nit \"" + supplierDTO.NitSupplier + "\" para proveedor ya existe.");
            var supplier = new Supplier
            {
                NitSupplier = supplierDTO.NitSupplier,
                NameSupplier = supplierDTO.NameSupplier,
                AddressSupplier = supplierDTO.AddressSupplier,
                PhoneSupplier = supplierDTO.PhoneSupplier,
                EmailSupplier = supplierDTO.EmailSupplier,
                IsActiveSupplier = true,

            };
            try
            {
                _context.Suppliers.Add(supplier);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest();
            }

            return CreatedAtAction("GetSupplier", new { id = supplier.IdSupplier }, supplier);
        }




        [HttpPatch]
        [Route("EditState")]
        [Authorize]
        public async Task<ActionResult> PatchSupplier(SupplierDeleteDTO supplierDTO)
        {
            var supplier = _context.Suppliers.FirstOrDefault(i => i.NitSupplier == supplierDTO.nitSupplier);

            if (supplierDTO == null || supplier == null) return NotFound();
            supplier.IsActiveSupplier = supplierDTO.isActive;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(supplier.IdSupplier)) return NotFound();
                else { throw; }
            }
            return NoContent();
        }




        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            if (_context.Suppliers == null) return NotFound();
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null) return NotFound();
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return NoContent();
        }




        private bool SupplierExists(int id)
        {
            return (_context.Suppliers?.Any(e => e.IdSupplier == id)).GetValueOrDefault();
        }
        private bool SupplierNitExists(string  nit)
        {
            return (_context.Suppliers?.Any(e => e.NitSupplier == nit)).GetValueOrDefault();
        }
        private bool SupplierNameExists(string name)
        {
            return (_context.Suppliers?.Any(e => e.NameSupplier == name)).GetValueOrDefault();
        }
        private bool SupplierNitExistsEdit(string nit, int id)
        {
            return (_context.Suppliers?
                .Where(e => e.IdSupplier != id)
                .Any(e => e.NitSupplier == nit)).GetValueOrDefault();
        }
        private bool SupplierNameExistsEdit(string name, int id)
        {
            return (_context.Suppliers?
                .Where (e => e.IdSupplier != id)
                .Any(e => e.NameSupplier == name)).GetValueOrDefault();
        }
    }
}
