using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BACK_END_DIAZNATURALS.Model;
using BACK_END_DIAZNATURALS.DTO;
using System.Net.Sockets;
using System.Collections;

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
        [Route("all/last")]
        public async Task<ActionResult<IEnumerable<GetOrderDTO>>> GetOrderHistoriesLast()
        {
            if (_context.OrderHistories == null)
            {
                return NotFound();
            }
            var orders = _context.OrderHistories
            .GroupBy(c => c.IdOrder)
            .Select(group => group.OrderByDescending(c => c.DateOrderHistory).First())
            .ToList();
        

            ArrayList s = new ArrayList ();
            foreach ( var c in orders)
            {
                GetOrderDTO ss = new GetOrderDTO
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
                };
                s.Add(ss);
                }    
               
            
            return Ok(s);
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

        [HttpGet]
        [Route("client/last")]
        public async Task<ActionResult<IEnumerable<GetOrderDTO>>> GetLastOrderHistoriesForClient(int idClient)
        {
            var orders = _context.OrderHistories
                .Where(c => c.IdOrderNavigation.IdClientNavigation.IdClient == idClient)
                .GroupBy(c => c.IdOrder)
                .Select(group => group.OrderByDescending(c => c.DateOrderHistory).First())
                .ToList();

            if (orders.Count == null)
            {
                return NotFound();
            }
            System.Console.WriteLine(orders);
            var ordersDTO = orders
                .Select(c => new GetOrderDTO
                {
                    IdOrder = _context.OrderHistories.FirstOrDefault(v => v.IdOrder == c.IdOrder).IdOrder,
                    StartDateOrder = _context.OrderHistories.FirstOrDefault(v => v.IdOrderNavigation.StartDateOrder == c.IdOrderNavigation.StartDateOrder).IdOrderNavigation.StartDateOrder,
                    ImageOrder = _context.OrderHistories.FirstOrDefault(v => v.IdOrderNavigation.ImageOrder == c.IdOrderNavigation.ImageOrder).IdOrderNavigation.ImageOrder,
                    StatusOrder = _context.OrderHistories.FirstOrDefault(v => v.IdStatusNavigation.NameStatus == c.IdStatusNavigation.NameStatus).IdStatusNavigation.NameStatus,
                    TotalPriceOrder = _context.OrderHistories.FirstOrDefault(v => v.IdOrderNavigation.TotalPriceOrder == c.IdOrderNavigation.TotalPriceOrder).IdOrderNavigation.TotalPriceOrder,
                }).ToList();


            return Ok(ordersDTO);
        }



        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(PatchOrderDTO orderDTO)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'DiazNaturalsContext.Orders' is null.");
            }
            var status = _context.Statuses.FirstOrDefault(o => o.NameStatus == orderDTO.NameStatus);
            var or = _context.Orders.FirstOrDefault(o => o.IdOrder == orderDTO.IdOrder);
            if (status == null || or == null)
            {
                return NotFound();
            }
            var order = _context.OrderHistories
                .Where(o => o.IdOrder == orderDTO.IdOrder && o.IdStatus == status.IdStatus)
                .FirstOrDefault();


            if (order != null)
            {
                return Conflict();
            }

            var orderHistory = new OrderHistory
            {
                IdOrder = orderDTO.IdOrder,
                IdStatus = status.IdStatus,
                DateOrderHistory = orderDTO.DateOrderHistory,
            };
            _context.OrderHistories.Add(orderHistory);
            await _context.SaveChangesAsync();
            return Ok();
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
