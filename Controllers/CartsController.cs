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
    public class CartsController : ControllerBase
    {
        private readonly DiazNaturalsContext _context;

        public CartsController(DiazNaturalsContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCartDTO>>> GetCarts(int idOrder)
        {
          if (_context.Carts == null)
          {
              return NotFound();
          }
            var cart = _context.Carts
                  .Where(c => c.IdOrder == idOrder)
                  .Select(c => new GetCartDTO
                  {
                      name= c.IdProductNavigation.NameProduct,
                      supplier = c.IdProductNavigation.IdSupplierNavigation.NameSupplier,
                      presentation = c.IdProductNavigation.IdPresentationNavigation.NamePresentation,
                      quantity= c.QuantityProductCart,
                      image= c.IdProductNavigation.ImageProduct,
                      price= c.IdProductNavigation.PriceProduct                    
                  }).ToList();
            if(cart.Count==0) { return  NotFound(); }

            return Ok(cart);
        }


        /*[HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(int id)
        {
          if (_context.Carts == null)
          {
              return NotFound();
          }
            var cart = await _context.Carts.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }

            return cart;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, Cart cart)
        {
            if (id != cart.IdOrder)
            {
                return BadRequest();
            }

            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
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

        // POST: api/Carts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
          if (_context.Carts == null)
          {
              return Problem("Entity set 'DiazNaturalsContext.Carts'  is null.");
          }
            _context.Carts.Add(cart);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CartExists(cart.IdOrder))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCart", new { id = cart.IdOrder }, cart);
        }

        // DELETE: api/Carts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            if (_context.Carts == null)
            {
                return NotFound();
            }
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        private bool CartExists(int id)
        {
            return (_context.Carts?.Any(e => e.IdOrder == id)).GetValueOrDefault();
        }
    }
}
