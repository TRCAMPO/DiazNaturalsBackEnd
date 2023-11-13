using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BACK_END_DIAZNATURALS.Model;
using BACK_END_DIAZNATURALS.DTO;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Serilog;

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
        [Route("all")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProductAddDTO>>> GetAllProducts()
        {
            if (_context.Products == null) return NotFound();
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




        [HttpGet]
        [Route("active")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProductAddDTO>>> GetActiveProducts()
        {
            if (_context.Products == null) return NotFound();
            var products = _context.Products
              .Where(p => p.IsActiveProduct == true)
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
            Log.Information("o en:");
        }




        [HttpGet]
        [Route("inactive")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProductAddDTO>>> GetInactiveProducts()
        {
            if (_context.Products == null) return NotFound();
            var products = _context.Products
              .Where(p => p.IsActiveProduct == false)
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
        [Authorize]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            if (_context.Products == null) return NotFound();
            var productDTO = await _context.Products
            .Where(p => p.IdProduct == id)
            .Select(p => new ProductDTO
            {
                IdProduct = p.IdProduct,
                name = p.NameProduct,
                supplier = _context.Suppliers.FirstOrDefault(s => s.IdSupplier == p.IdSupplier).NameSupplier ?? "Proveedor no encontrado",
                price = p.PriceProduct,
                amount = p.QuantityProduct,
                presentation = _context.Presentations.FirstOrDefault(pr => pr.IdPresentation == p.IdPresentation).NamePresentation,
                category = _context.Categories.FirstOrDefault(c => c.IdCategory == p.IdCategory).NameCategory,
                description = p.DescriptionProduct,
                image = p.ImageProduct
            })
            .FirstOrDefaultAsync();
            if(productDTO == null) return NotFound();
            return productDTO;
        }



        [HttpGet()]
        [Route("search")]
        [Authorize]
        public async Task<ActionResult<ProductDTO>> GetProduct(string search, string suppliers, string presentation)
        {
            if (_context.Products == null || search == null || suppliers == null || presentation == null) return BadRequest();
            ProductSearchDTO productSearch = new ProductSearchDTO
            {
                presentation = presentation,
                suppliers = suppliers,
                search = search,
            };
            Product p = SearchProduct(productSearch);
            if (p == null) return NotFound();
            if (p.IsActiveProduct)
            {
                var productDto = new ProductDTO
                {
                    IdProduct = p.IdProduct,
                    name = p.NameProduct,
                    supplier = _context.Suppliers.FirstOrDefault(s => s.IdSupplier == p.IdSupplier).NameSupplier ?? "Proveedor no encontrado",
                    price = p.PriceProduct,
                    amount = p.QuantityProduct,
                    presentation = _context.Presentations.FirstOrDefault(pr => pr.IdPresentation == p.IdPresentation).NamePresentation,
                    category = _context.Categories.FirstOrDefault(c => c.IdCategory == p.IdCategory).NameCategory,
                    description = p.DescriptionProduct,
                    image = p.ImageProduct
                };
                return productDto;
            }
            return NotFound();
        }



        /* [HttpGet()]
         [Route("ValidateQuantity")]
         [Authorize]
         public IActionResult GetValidateQuantityProduct(string search, string suppliers, string presentation, int quantityProduct)
         {
             if (_context.Products == null || search == null || suppliers == null || presentation == null) return BadRequest();
             ProductSearchDTO productSearch = new ProductSearchDTO
             {
                 presentation = presentation,
                 suppliers = suppliers,
                 search = search,
             };
             Product p = SearchProduct(productSearch);
             if (p == null) return NotFound();
             if (!p.IsActiveProduct) return NotFound();
             if (p.QuantityProduct < quantityProduct)
             {
                 return StatusCode(StatusCodes.Status400BadRequest, new { quantity = "Cantidad insuficiente", nameProduct = search, supplierProduct = suppliers, presentation = presentation });
             }
             else if (p.QuantityProduct >= quantityProduct)
             {
                 return StatusCode(StatusCodes.Status200OK, new { quantity = "Cantidad suficiente", nameProduct = search, supplierProduct = suppliers, presentation = presentation });
             }

             return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Error interno del servidor" });
         }*/




        [HttpGet()]
        [Route("ValidateQuantity")]
  
        public IActionResult GetValidateQuantityProduct(string search, string suppliers, string presentation, int quantityProduct)
        {
            if (_context.Products == null || search == null || suppliers == null || presentation == null) return BadRequest();
            ProductSearchDTO productSearch = new ProductSearchDTO
            {
                presentation = presentation,
                suppliers = suppliers,
                search = search,
            };
            Product p = SearchProduct(productSearch);
            if (p == null) return NotFound();
            if (!p.IsActiveProduct) return NotFound();
            if (p.QuantityProduct < quantityProduct)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { quantity = "Cantidad insuficiente", nameProduct = search, supplierProduct = suppliers, presentation = presentation });
            }
            else if (p.QuantityProduct >= quantityProduct)
            {
                return StatusCode(StatusCodes.Status200OK, new { quantity = "Cantidad suficiente", nameProduct = search, supplierProduct = suppliers, presentation = presentation });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Error interno del servidor" });
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



        [HttpGet]
        [Route("lowQuantity")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProductAddDTO>>> GetLowQuantityProducts()
        {

            if (_context.Products == null) return NotFound();
            var products = _context.Products
              .Where(p => p.IsActiveProduct == true)
              .Include(p => p.IdSupplierNavigation)
              .Include(p => p.IdPresentationNavigation)
              .Include(p => p.IdCategoryNavigation)
              .Where(p => p.QuantityProduct < 10)
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



        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutProduct(int id, ProductDTO productDTO)
        {
            var product = _context.Products.FirstOrDefault(p => p.IdProduct == id);
            if (product == null || product == null || !product.IsActiveProduct) return NotFound();
            var suplier = _context.Suppliers.FirstOrDefault(i => i.NameSupplier == productDTO.supplier);
            var presentation = _context.Presentations.FirstOrDefault(i => i.NamePresentation == productDTO.presentation);
            var category = _context.Categories.FirstOrDefault(i => i.NameCategory == productDTO.category);
            if (suplier == null || presentation == null || category == null) return NotFound();
            if (ProductsExistsEdit(suplier.IdSupplier, presentation.IdPresentation, productDTO.name, productDTO.IdProduct)) return Conflict("Ya existe un producto con el nombre \"" + productDTO.name + "\" el proveedor \"" + productDTO.supplier + "\" y la presentacion \"" + productDTO.presentation + "\".");
            product.NameProduct = productDTO.name;
            product.DescriptionProduct = productDTO.description;
            product.ImageProduct = productDTO.image;
            product.PriceProduct = productDTO.price;
            product.IdSupplier = suplier.IdSupplier;
            product.IdPresentation = presentation.IdPresentation;
            product.IdCategory = category.IdCategory;
            product.QuantityProduct = productDTO.amount;
            _context.Entry(product).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.IdProduct)) return NotFound();
                else { throw; }
            }
            return NoContent();
        }




        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Product>> PostProduct(ProductAddDTO productDTO)
        {
            if (_context.Products == null || productDTO == null) return Problem("Entity set 'DiazNaturalsContext.Products'  is null.");
            if (productDTO.supplier == null || productDTO.presentation == null || productDTO.name == null || productDTO.category==null) return NotFound("Existen parametros nulos en la peticion");

            var suplier = _context.Suppliers.FirstOrDefault(i => i.NameSupplier == productDTO.supplier);
            var presentation = _context.Presentations.FirstOrDefault(i => i.NamePresentation == productDTO.presentation);
            var category = _context.Categories.FirstOrDefault(i => i.NameCategory == productDTO.category);
            if (suplier == null || presentation == null || category == null) return NotFound();
            if(ProductsExists(suplier.IdSupplier, presentation.IdPresentation, productDTO.name))return Conflict("Ya existe un producto con el nombre \""+ productDTO.name + "\" el proveedor \"" + productDTO.supplier+ "\" y la presentacion \"" + productDTO.presentation+ "\".");
            var product = new Product
            {
                IdSupplier = suplier.IdSupplier,
                IdPresentation = presentation.IdPresentation,
                IdCategory = category.IdCategory,
                NameProduct = productDTO.name,
                PriceProduct = productDTO.price,
                QuantityProduct = productDTO.amount,
                DescriptionProduct = productDTO.description,
                ImageProduct = productDTO.image,
                IsActiveProduct = true,
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            if (product.QuantityProduct > 0)
            {
                var entry = new Entry()
                {
                    DateEntry = DateTime.Now,
                    IdProduct = product.IdProduct,
                    QuantityProductEntry = product.QuantityProduct,
                };
                _context.Entries.Add(entry);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }




        [HttpPatch]
        [Route("EditState")]
        [Authorize]
        public async Task<ActionResult> Patch([Required] int id, ProductDeleteDTO productDTO)
        {
            var product = _context.Products.FirstOrDefault(i => i.IdProduct == productDTO.idProduct);
            if (productDTO == null || product == null) return NotFound();
            product.IsActiveProduct = productDTO.isActive;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.IdSupplier)) return NotFound();
                else { throw; }
            }
            return NoContent();
        }

        [HttpPatch]
        [Route("UpdateQuantity")]
        [Authorize]
        public async Task<ActionResult> PatchUpdateQuantity(int quantity, ProductSearchDTO productSearchDTO)
        {
            if (_context.Products == null || productSearchDTO== null|| productSearchDTO.search == null || productSearchDTO.suppliers == null || productSearchDTO.presentation == null || quantity<0) return BadRequest();

            Product p = SearchProduct(productSearchDTO);
            if (p == null) return NotFound();
            if (!p.IsActiveProduct) return NotFound();

            p.QuantityProduct += quantity;

            try
            {
                await _context.SaveChangesAsync();
                if (quantity > 0)
                {
                    var entry = new Entry()
                    {
                        DateEntry = DateTime.Now,
                        IdProduct = p.IdProduct,
                        QuantityProductEntry = quantity
                    };
                    _context.Entries.Add(entry);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(p.IdProduct)) return NotFound();
                else { throw; }
            }
            return NoContent();
        }



        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null) return NotFound();
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }



        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.IdProduct == id)).GetValueOrDefault();
        }
        private bool ProductsExists(int suplier, int presentattion, string name)
        {
            bool producExist= _context.Products?.Any(e =>
            e.NameProduct == name &&
            e.IdSupplier == suplier &&
            e.IdPresentation == presentattion
            )??false;
            return producExist;
        }
        private bool ProductsExistsEdit(int suplier, int presentattion, string name, int  id)
        {
            bool producExist = _context.Products?
                .Where(i=> i.IdProduct!=id)
                .Any(e =>
            e.NameProduct == name &&
            e.IdSupplier == suplier &&
            e.IdPresentation == presentattion
            ) ?? false;
            return producExist;
        }
    }
}
