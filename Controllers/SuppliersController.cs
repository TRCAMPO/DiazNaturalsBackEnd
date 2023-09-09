using BACK_END_DIAZNATURALS.DTO;
using BACK_END_DIAZNATURALS.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetAllSuppliers()
        {
            if (_context.Suppliers == null)
            {
                return NotFound();
            }
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
            return supplierDTOs;
        }

        [HttpGet]
        [Route("active")]
        public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetActiveSuppliers()
        {
            if (_context.Suppliers == null)
            {
                return NotFound();
            }
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
            return supplierDTOs;
        }

        [HttpGet]
        [Route("inactive")]
        public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetInactiveSuppliers()
        {
            if (_context.Suppliers == null)
            {
                return NotFound();
            }
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
            return supplierDTOs;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplier(int id)
        {
            if (_context.Suppliers == null)
            {
                return NotFound();
            }
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null || !supplier.IsActiveSupplier)
            {
                return NotFound();
            }
            SupplierDTO supplierDTO = new SupplierDTO
            {
                IdSupplier = supplier.IdSupplier,
                NameSupplier = supplier.NameSupplier,
                AddressSupplier = supplier.AddressSupplier,
                EmailSupplier = supplier.EmailSupplier,
                PhoneSupplier = supplier.PhoneSupplier,
                NitSupplier = supplier.NitSupplier,
            };
            return supplier;
        }


        [HttpPut("{nitSupplier}")]
        public async Task<IActionResult> PutSupplier(string name, SupplierAddDTO supplierDTO)
        {
            var supplier = _context.Suppliers.FirstOrDefault(c => c.NameSupplier == name);
            if (supplier == null || !supplier.IsActiveSupplier)
            {
                return NotFound(supplier);
            }

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
                if (!SupplierExists(supplier.IdSupplier))
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
        public async Task<ActionResult<Supplier>> PostSupplier(SupplierAddDTO supplierDTO)
        {
            if (_context.Suppliers == null)
            {
                return Problem("Entity set 'DiazNaturalsContext.Suppliers'  is null.");
            }
            var supplier = new Supplier
            {
                NitSupplier = supplierDTO.NitSupplier,
                NameSupplier = supplierDTO.NameSupplier,
                AddressSupplier = supplierDTO.AddressSupplier,
                PhoneSupplier = supplierDTO.PhoneSupplier,
                EmailSupplier = supplierDTO.EmailSupplier,
                IsActiveSupplier = true,

            };

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSupplier", new { id = supplier.IdSupplier }, supplier);
        }

        [HttpPatch]
        [Route("EditState")]

        public async Task<ActionResult> PatchSupplier(SupplierDeleteDTO supplierDTO)
        {
            var supplier = _context.Suppliers.FirstOrDefault(i => i.NitSupplier == supplierDTO.nitSupplier);

            if (supplierDTO == null || supplier == null)
            {
                return NotFound();
            }

            supplier.IsActiveSupplier = supplierDTO.isActive;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(supplier.IdSupplier))
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


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            if (_context.Suppliers == null)
            {
                return NotFound();
            }
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SupplierExists(int id)
        {
            return (_context.Suppliers?.Any(e => e.IdSupplier == id)).GetValueOrDefault();
        }
    }
}
