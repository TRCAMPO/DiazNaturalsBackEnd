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
    public class SuppliersController : ControllerBase
    {
        private readonly DiazNaturalsContext _context;

        public SuppliersController(DiazNaturalsContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetSuppliers()
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
                  AddressSupplier= c.AddressSupplier,
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

            if (supplier == null)
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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupplier(int id, Supplier supplier)
        {
            if (id != supplier.IdSupplier)
            {
                return BadRequest();
            }

            _context.Entry(supplier).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(id))
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
            };

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSupplier", new { id = supplier.IdSupplier }, supplier);
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
