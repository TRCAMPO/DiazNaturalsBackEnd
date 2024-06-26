﻿using BACK_END_DIAZNATURALS.DTO;
using BACK_END_DIAZNATURALS.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BACK_END_DIAZNATURALS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DiazNaturalsContext _context;



        public CategoriesController(DiazNaturalsContext context)
        {
            _context = context;
        }



        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            if (_context.Categories == null)
            {
                Log.Error($"Error en el acceso al servidor al intentar extraer informacion de Categories, cod error 500, Internal Server error");
                return NotFound();
            }
            var categoryDto = await _context.Categories
            .Select(c => new CategoryDTO
            {
                IdCategory = c.IdCategory,
                NameCategory = c.NameCategory
            })
            .ToListAsync();
            return Ok(categoryDto);
        }



        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            if (_context.Categories == null)
            {
                Log.Error($"Error en el acceso al servidor al intentar extraer informacion de Categories, cod error 500, Internal Server error");
                return NotFound();
            }
            var category = await _context.Categories.FindAsync(id);

            if (category == null) return NotFound();

            CategoryDTO categoryDTO = new CategoryDTO
            {
                IdCategory = category.IdCategory,
                NameCategory = category.NameCategory,
            };
            return categoryDTO;
        }



        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCategory(int id, CategoryDTO categoryDTO)
        {
            if (id != categoryDTO.IdCategory)
            {
                Log.Error($"Error en el contenido de la peticion para editar la categoria, {id}, " + $"cod error {BadRequest().StatusCode}");
                return BadRequest();
            }
            var category= _context.Categories.FirstOrDefault(i=>i.IdCategory == id);
            if (category == null)
            {
                Log.Information($"Intento de cambio del nombre de una categoria que no existe {categoryDTO.NameCategory}");
                return NotFound();
            }
            categoryDTO.NameCategory = categoryDTO.NameCategory;
            _context.Entry(category).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                Log.Information($"Se cambio el nombre de la categoria {categoryDTO.NameCategory}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id)) return NotFound();
                else { throw; }
            }
            return NoContent();
        }



        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Category>> PostCategory(CategoriesAddDTO categoryDTO)
        {
            if (_context.Categories == null)
            {
                Log.Error($"Error en el acceso al servidor al intentar extraer informacion de Categories, cod error 500, Internal Server error");
                return Problem("Entity set 'DiazNaturalsContext.Categories'  is null.");
            }
            if (categoryDTO == null) return NoContent();

            var category = new Category
            {
                NameCategory = categoryDTO.NameCategory,
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            Log.Information($"Se registro la categoria {categoryDTO.NameCategory}");
            return CreatedAtAction("GetCategory", new { id = category.IdCategory }, category);
        }



        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (_context.Categories == null) return NotFound();
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }



        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.IdCategory == id)).GetValueOrDefault();
        }
    }
}
