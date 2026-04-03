using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.DTOs;
using ProductApi.Models;
using ProductApi.Repositories;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        //private readonly AppDbContext _context;
        private readonly IProductRepository _repository;
        public ProductController(IProductRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {
            var Products = await _repository.GetAllAsync();
            var result = Products.Select( p => new ProductDto
            {
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                Author = p.Author,
                Quantity = p.Quantity,
            });
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(CreateProductDto dto)
        {
            var exist = await _repository.ExistsAsync(dto.Name, dto.Author);
            if (exist)
                return BadRequest("Already Exists");
            var product = new Products
            {
                Name = dto.Name,
                Author = dto.Author,
                Description = dto.Description,
                Quantity = dto.Quantity,
                Price = dto.Price,
            };
            await _repository.AddAsync(product);
            return Ok(product);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            var productdto = new ProductDto
            {
                Name = result.Name,
                Author = result.Author,
                Description = result.Description,
                Quantity = result.Quantity,
                Price = result.Price,
            };
            return Ok(productdto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto dto)
        {
            var result = await _repository.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            result.Name = dto.Name;
            result.Description = dto.Description;
            result.Price = dto.Price;
            result.Author = dto.Author;
            result.Quantity = dto.Quantity;  
            await _repository.UpdateAsync(result);

            var productDTo = new ProductDto
            {
                Name= result.Name,
                Author = result.Author,
                Description = result.Description,
                Price = result.Price,
                Quantity= result.Quantity,
            };
            return Ok(productDTo);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            await _repository.DeleteAsync(result);
            return Ok($"{result.Name} Removed Successful");
        }
    }
}
