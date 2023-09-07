﻿using System;
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
    public class ProductsController : ControllerBase
    {
        private readonly DiazNaturalsContext _context;

        public ProductsController(DiazNaturalsContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
          if (_context.Products == null)
          {
              return NotFound();
          }

            var products = _context.Products
              .Include(p => p.IdSupplierNavigation) 
              .Include(p => p.IdPresentationNavigation) 
              .Include(p => p.IdCategoryNavigation) 
              .Select(p => new
              {
                  IdProduct = p.IdProduct,
                  supplier = p.IdSupplierNavigation.NameSupplier, 
                  presentation = p.IdPresentationNavigation.NamePresentation, 
                  category = p.IdCategoryNavigation.NameCategory, 
                  name = p.NameProduct,
                  price = p.PriceProduct,
                  amount = p.QuantityProduct,
                  description = p.DescriptionProduct,
                  image = p.ImageProduct
              })
              .ToList();

            return Ok(products);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

    
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.IdProduct)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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
        public async Task<ActionResult<Product>> PostProduct(ProductDTO productDTO)
        {
          if (_context.Products == null || productDTO==null)
          {
              return Problem("Entity set 'DiazNaturalsContext.Products'  is null.");
          }
            var suplier = _context.Suppliers.FirstOrDefault(i => i.NameSupplier == productDTO.supplier);
            var presentation = _context.Presentations.FirstOrDefault(i => i.NamePresentation == productDTO.presentation);
            var category = _context.Categories.FirstOrDefault(i => i.NameCategory == productDTO.category);
            if (suplier == null || presentation == null || category == null)
            {
                return NotFound();
            }
            var product = new Product
            {
                IdSupplier = suplier.IdSupplier,
                IdPresentation= presentation.IdPresentation,
                IdCategory= category.IdCategory,
                NameProduct= productDTO.name,
                PriceProduct= productDTO.price,
                QuantityProduct= productDTO.amount,
                DescriptionProduct= productDTO.description,
                ImageProduct= productDTO.image,
        };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.IdProduct }, product);
        }

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.IdProduct == id)).GetValueOrDefault();
        }
    }
}