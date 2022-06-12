using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Data;
using ServerApp.DTOs;
using ServerApp.Models;

namespace ServerApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly SocialContext _context;

        public ProductsController(SocialContext context)
        {
            _context = context;
        }

        // localhost:5000/api/products
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products.Select(p => ProductToDTO(p)).ToListAsync();
            return Ok(products);
        }


        // localhost:5000/api/products/2
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products.Select(p => ProductToDTO(p)).FirstOrDefaultAsync(p => p.Id == id);
            
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction( // 201 döner
                actionName: nameof(GetProduct),
                routeValues: new { id = product.Id },
                value: ProductToDTO(product)
            );
        }
    

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            var result =  await _context.Products.FindAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            result.Name = product.Name;
            result.Price = product.Price;
            result.IsActive = product.IsActive;
            await _context.SaveChangesAsync();
            return NoContent();    
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    
    
    
    
    
        private static ProductDTO ProductToDTO(Product product)
        {
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                IsActive = product.IsActive
            };
        }
    }
}