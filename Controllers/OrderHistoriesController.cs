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
    public class OrderHistoriesController : ControllerBase
    {
        private readonly DiazNaturalsContext _context;

        public OrderHistoriesController(DiazNaturalsContext context)
        {
            _context = context;
        }

        // GET: api/OrderHistories
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<GetOrderDTO>>> GetOrderHistories()
        {
            if (_context.OrderHistories == null)
            {
                return NotFound();
            }

            var orders = _context.OrderHistories
                .Include(o => o.IdOrderNavigation.IdClientNavigation)
                .Include(o => o.IdStatusNavigation)
                .Select(c => new GetOrderDTO
                {
                    IdOrder = c.IdOrder,
                    IdClient = c.IdOrderNavigation.IdClient,
                    nitClient = c.IdOrderNavigation.IdClientNavigation.NitClient,
                    nameClient = c.IdOrderNavigation.IdClientNavigation.NameClient,
                    stateClient = c.IdOrderNavigation.IdClientNavigation.StateClient,
                    cityClient = c.IdOrderNavigation.IdClientNavigation.CityClient,
                    addressClient = c.IdOrderNavigation.IdClientNavigation.AddressClient,
                    phoneClient = c.IdOrderNavigation.IdClientNavigation.PhoneClient,
                    nameContactClient = c.IdOrderNavigation.IdClientNavigation.NameContactClient,
                    StartDateOrder = c.IdOrderNavigation.StartDateOrder,
                    ImageOrder = c.IdOrderNavigation.ImageOrder,
                    StatusOrder = c.IdStatusNavigation.NameStatus,
                    TotalPriceOrder = c.IdOrderNavigation.TotalPriceOrder,
                }).ToList();

            return Ok(orders);
        }



        [HttpGet]
        [Route("client")]
        public async Task<ActionResult<IEnumerable<GetOrderDTO>>> GetOrderHistoriesForClient(int idClient)
        {
            if (_context.OrderHistories == null)
            {
                return NotFound();
            }
            var orders = _context.OrderHistories
                .Where(c=> c.IdOrderNavigation.IdClientNavigation.IdClient== idClient)
                 .Select(c => new GetOrderDTO
                 {
                     IdOrder = c.IdOrder,
                     StartDateOrder = c.IdOrderNavigation.StartDateOrder,
                     ImageOrder = c.IdOrderNavigation.ImageOrder,
                     StatusOrder = c.IdStatusNavigation.NameStatus,
                     TotalPriceOrder = c.IdOrderNavigation.TotalPriceOrder,
                 }).ToList();
            return Ok(orders);
        }



       
      /*  [HttpGet("{id}")]
        public async Task<ActionResult<OrderHistory>> GetOrderHistory(int id)
        {
          if (_context.OrderHistories == null)
          {
              return NotFound();
          }
            var orderHistory = await _context.OrderHistories.FindAsync(id);

            if (orderHistory == null)
            {
                return NotFound();
            }

            return orderHistory;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderHistory(int id, OrderHistory orderHistory)
        {
            if (id != orderHistory.IdOrder)
            {
                return BadRequest();
            }

            _context.Entry(orderHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderHistoryExists(id))
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
        public async Task<ActionResult<OrderHistory>> PostOrderHistory(OrderHistory orderHistory)
        {
          if (_context.OrderHistories == null)
          {
              return Problem("Entity set 'DiazNaturalsContext.OrderHistories'  is null.");
          }
            _context.OrderHistories.Add(orderHistory);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OrderHistoryExists(orderHistory.IdOrder))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrderHistory", new { id = orderHistory.IdOrder }, orderHistory);
        }*/

       /* [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderHistory(int id)
        {
            if (_context.OrderHistories == null)
            {
                return NotFound();
            }
            var orderHistory = await _context.OrderHistories.FindAsync(id);
            if (orderHistory == null)
            {
                return NotFound();
            }

            _context.OrderHistories.Remove(orderHistory);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        private bool OrderHistoryExists(int id)
        {
            return (_context.OrderHistories?.Any(e => e.IdOrder == id)).GetValueOrDefault();
        }
    }
}
