using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BACK_END_DIAZNATURALS.Model;
using BACK_END_DIAZNATURALS.DTO;
using Serilog;

namespace BACK_END_DIAZNATURALS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DiazNaturalsContext _context;

        public OrdersController(DiazNaturalsContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
          if (_context.Orders == null)
          {
                Log.Error($"Error en el acceso al servidor al intentar extraer informacion de Orders, cod error 500, Internal Server error");
                return NotFound();
          }
            return await _context.Orders.ToListAsync();
        }


       /* [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
          if (_context.Orders == null)
          {
              return NotFound();
          }
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }*/


        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, PutImageOrderDTO orderDTO)
        {
            if (id != orderDTO.IdOrder)
            {
                Log.Warning($"Solicitud incorrecta para editar order: {id}, cod error {BadRequest().StatusCode} ");
                return BadRequest();
            }

            var order =  _context.Orders.FirstOrDefault(o => o.IdOrder == id);
            if(order == null) {
                Log.Warning($"No se encontro la orden : {id}, para poder editarla, cod error {NotFound().StatusCode}");
                return NotFound(); 
            }
            var c = _context.Orders.Where(c => c.IdOrder != order.IdOrder)
                .Where(c => c.ImageOrder != "SinComprobanteDePago.jpeg")
                .Any(z => z.ImageOrder == orderDTO.ImageOrder);

            if (c) {
                Log.Warning($"Conflicto en los nuevos datos a agregar a la orden: {id}, cod error {Conflict().StatusCode} ");
                return Conflict(); }
            order.ImageOrder = orderDTO.ImageOrder;

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                Log.Information("Informacion de orden actualizada: {@Order}", orderDTO);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    Log.Warning($"No se encontro la orden : {id} para poder editarla, cod error {NotFound().StatusCode} ");
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
        public async Task<ActionResult<Order>> PostOrder(PostOrderDTO orderDTO)
        {
            if (_context.Orders == null)
            {
                Log.Error($"Error en el acceso al servidor al intentar extraer informacion de Orders, cod error 500, Internal Server error");
                return Problem("Entity set 'DiazNaturalsContext.Orders' is null.");
            }
            bool checkClient = _context.Clients.Any(c => c.IdClient == orderDTO.IdClient);

            if (!checkClient) {
                Log.Warning($"No se encontro el cliente: {orderDTO.IdClient} para" +
                    $" asignarle nueva orden de compra, cod error {NotFound().StatusCode},");
                return NotFound(); 
            };

            var order = new Order
            {
                IdClient = orderDTO.IdClient,
                StartDateOrder = orderDTO.StartDateOrder,
                TotalPriceOrder = orderDTO.TotalPriceOrder,
                ImageOrder = "SinComprobanteDePago.jpeg",
                Carts = new List<Cart>()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            Log.Information("Orden de compra agregada: {@Order}", orderDTO);
            List<Cart> carts = new List<Cart>();
            foreach (var aux in orderDTO.AddCart)
            {
                bool checkProduct = _context.Products.Any(c => c.IdProduct == aux.ProductId);
                if (!checkClient)
                {
                    Log.Warning($"No se encontro el producto : {aux.ProductId} para agregar producto de la orden {order.IdOrder}, cod error {NotFound().StatusCode}");
                    _context.Orders.Remove(order);
                    return NoContent();
                }

                var c = new Cart
                {
                    IdOrder = order.IdOrder,
                    IdProduct = aux.ProductId,
                    QuantityProductCart = aux.Quantity
                };
                carts.Add(c);
            }

            order.Carts = carts;
            await _context.SaveChangesAsync();

            OrderHistory orderHistory = new OrderHistory
            {
                IdOrder = order.IdOrder,
                IdStatus = 1,
                DateOrderHistory = DateTime.Now,
            };

            _context.OrderHistories.Add(orderHistory);
            await _context.SaveChangesAsync();
            Log.Warning($"Nuevo historial agregado para la orden: {orderHistory.IdStatus}");
            return Ok();
        }



       /* [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.IdOrder == id)).GetValueOrDefault();
        }
    }
}
