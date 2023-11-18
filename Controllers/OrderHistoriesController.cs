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
using Serilog;

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
                Log.Error($"Error en el acceso al servidor al intentar extraer informacion de OrderHistories, cod error 500, Internal Server error");
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
                    DateOrderHistory = c.DateOrderHistory
                }).ToList();
  
            return Ok(orders);
        }




        [HttpGet]
        [Route("all/last")]
        public async Task<ActionResult<IEnumerable<GetOrderDTO>>> GetOrderHistoriesLast()
        {
            if (_context.OrderHistories == null)
            {
                Log.Error($"Error en el acceso al servidor al intentar extraer informacion de OrderHistories, cod error 500, Internal Server error");
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
                   DateOrderHistory = c.DateOrderHistory
               }).GroupBy(c => c.IdOrder)
               .Select(group => group.OrderByDescending(c => c.DateOrderHistory).First())
               .ToList();
            return Ok(orders);
        }




        [HttpGet]
        [Route("client")]
        public async Task<ActionResult<IEnumerable<GetOrderDTO>>> GetOrderHistoriesForClient(int idClient)
        {
            if (_context.OrderHistories == null)
            {
                Log.Error($"Error en el acceso al servidor al intentar extraer informacion de OrderHistories, cod error 500, Internal Server error");
                return NotFound();
            }
            var orders = _context.OrderHistories
                .Where(c => c.IdOrderNavigation.IdClientNavigation.IdClient == idClient)
                 .Select(c => new GetOrderDTO
                 {
                     IdOrder = c.IdOrder,
                     IdClient = c.IdOrderNavigation.IdClient,
                     StartDateOrder = c.IdOrderNavigation.StartDateOrder,
                     ImageOrder = c.IdOrderNavigation.ImageOrder,
                     StatusOrder = c.IdStatusNavigation.NameStatus,
                     TotalPriceOrder = c.IdOrderNavigation.TotalPriceOrder,
                     DateOrderHistory = c.DateOrderHistory
                 }).ToList();
            return Ok(orders);
        }




        [HttpGet]
        [Route("client/last")]
        public async Task<ActionResult<IEnumerable<GetOrderDTO>>> GetLastOrderHistoriesForClient(int idClient)
        {
            var orders = _context.OrderHistories
                .Where(c => c.IdOrderNavigation.IdClientNavigation.IdClient == idClient)
                .Select(c => new GetOrderDTO
                {
                    IdOrder = c.IdOrder,
                    IdClient = c.IdOrderNavigation.IdClient,
                    StartDateOrder = c.IdOrderNavigation.StartDateOrder,
                    ImageOrder = c.IdOrderNavigation.ImageOrder,
                    StatusOrder = c.IdStatusNavigation.NameStatus,
                    TotalPriceOrder = c.IdOrderNavigation.TotalPriceOrder,
                    DateOrderHistory = c.DateOrderHistory
                }).GroupBy(c => c.IdOrder)
                .Select(group => group.OrderByDescending(c => c.DateOrderHistory).First())
                .ToList();

            //esta es otra forma de hacer la consulta mediante join y tambien es funcional
            /* var orders = _context.OrderHistories
                  .Where(c => c.IdOrderNavigation.IdClientNavigation.IdClient == idClient)
                  .Join(
                      _context.Orders,
                      orderHistory => orderHistory.IdOrder,
                      order => order.IdOrder,
                      (orderHistory, order) => new { OrderHistory = orderHistory, Order = order }
                  )
                  .Join(
                      _context.Statuses,
                      combined => combined.OrderHistory.IdStatus,
                      status => status.IdStatus,
                      (combined, status) => new
                      {
                          IdOrder = combined.OrderHistory.IdOrder,
                          StartDateOrder = combined.Order.StartDateOrder,
                          ImageOrder = combined.Order.ImageOrder,
                          StatusOrder = status.NameStatus,
                          TotalPriceOrder = combined.Order.TotalPriceOrder,
                          DateOrderHistory = combined.OrderHistory.DateOrderHistory
                      })
                  .GroupBy(c => c.IdOrder)
                  .Select(group => group.OrderByDescending(c => c.DateOrderHistory).First())
                  .ToList();*/
            return Ok(orders);
        }




        [HttpGet]
        [Route("client/Order")]
        public async Task<ActionResult<IEnumerable<GetOrderDTO>>> GetOrderHistoriesForOrder(int idOrder)
        {
            var orders = _context.OrderHistories
                .Where(c => c.IdOrderNavigation.IdOrder == idOrder)
                .Select(c => new GetOrderDTO
                {
                    IdOrder = c.IdOrder,
                    IdClient = c.IdOrderNavigation.IdClient,
                    StatusOrder = c.IdStatusNavigation.NameStatus,
                    DateOrderHistory = c.DateOrderHistory
                })
                .ToList();
            return Ok(orders);
        }




        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(PatchOrderDTO orderDTO)
        {
            if (_context.Orders == null)
            {
                Log.Error($"Error en el acceso al servidor al intentar extraer informacion de OrderHistories, cod error 500, Internal Server error");
                return Problem("Entity set 'DiazNaturalsContext.Orders' is null.");
            }
            var status = _context.Statuses.FirstOrDefault(o => o.NameStatus == orderDTO.NameStatus);
            var or = _context.Orders.FirstOrDefault(o => o.IdOrder == orderDTO.IdOrder);
            if (status == null || or == null)
            {
                Log.Error("Error en la peticion para registrar un nuevo historial de una orden de compra: {@Order}, ", orderDTO+ $"cod error {NotFound().StatusCode}");
                return NotFound();
            }
            var order = _context.OrderHistories
                .Where(o => o.IdOrder == orderDTO.IdOrder && o.IdStatus == status.IdStatus)
                .FirstOrDefault();


            if (order != null)
            {
                Log.Error("Error en la busqueda de la orden: {@Order}", orderDTO + $"cod error {Conflict().StatusCode}");
                return Conflict();
            }

            var orderHistory = new OrderHistory
            {
                IdOrder = orderDTO.IdOrder,
                IdStatus = status.IdStatus,
                DateOrderHistory = orderDTO.DateOrderHistory,
            };
            if (orderDTO.NameStatus == "Despachado")
            {
                SearchOrder(orderDTO.IdOrder);
            }
            _context.OrderHistories.Add(orderHistory);
            await _context.SaveChangesAsync();
            Log.Warning($"Nuevo historial agregado para la orden: {orderHistory.IdStatus}, con estado {orderDTO.NameStatus}");
            return Ok();
        }

        private void SearchOrder(int idOrder)
        {
            List<Product> products = new List<Product>();
            var order = _context.Carts
                .Include(c => c.IdProductNavigation)
                .Include(c => c.IdProductNavigation.IdPresentationNavigation)
                .Include(c => c.IdProductNavigation.IdSupplierNavigation)
                .Where(i => i.IdOrder == idOrder)
                .ToList();
            order.ForEach(o =>
            {
                var p = new ProductSearchDTO
                {
                    search = o.IdProductNavigation.NameProduct,
                    presentation = o.IdProductNavigation.IdPresentationNavigation.NamePresentation,
                    suppliers = o.IdProductNavigation.IdSupplierNavigation.NameSupplier
                };
                products.Add(SearchProduct(p));

            });
            products.ForEach(o =>
            {
                int quantity= _context.Carts.FirstOrDefault(i => i.IdOrder == idOrder).QuantityProductCart;
                Log.Information($"Registro de cantidad saliente del producto: {o.NameProduct}, numero de unidades: {quantity}");
                o.QuantityProduct -= quantity;
            });
            foreach (var productToUpdate in products)
            {
                _context.Entry(productToUpdate).State = EntityState.Modified;
            }

        }
        private Product SearchProduct(ProductSearchDTO productSearchDTO)
        {
            var supplier = _context.Suppliers.FirstOrDefault(p => p.NameSupplier == productSearchDTO.suppliers);
            var presentation = _context.Presentations.FirstOrDefault(p => p.NamePresentation == productSearchDTO.presentation);
            if (presentation == null || supplier == null) return null;
            var product = _context.Products
             .FirstOrDefault(p => p.NameProduct == productSearchDTO.search &&
                                  p.IdSupplier == supplier.IdSupplier &&
                                  p.IdPresentation == presentation.IdPresentation);
            return product;
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


        [HttpDelete("{idOrder}/{idStatus}")]
        public async Task<IActionResult> DeleteOrderHistory(int idOrder, int idStatus)
        {
            if (_context.OrderHistories == null)
            {
                return NotFound();
            }
            var orderHistory = _context.OrderHistories.FirstOrDefault(c => c.IdOrder == idOrder & c.IdStatus == idStatus);
            if (orderHistory == null)
            {
                return NotFound();
            }

            _context.OrderHistories.Remove(orderHistory);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool OrderHistoryExists(int id)
        {
            return (_context.OrderHistories?.Any(e => e.IdOrder == id)).GetValueOrDefault();
        }
    }
}
