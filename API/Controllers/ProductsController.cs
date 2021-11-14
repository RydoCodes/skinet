using System.Collections.Generic;
using System.Linq;
using API.Data;
using Microsoft.AspNetCore.Mvc;
using API.Infrastructure;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        public StoreContext _context { get; }

        public ProductsController(StoreContext context)
        {
            _context=context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var Products = await _context.Products.ToListAsync();
            return Ok(Products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            Product Product = await _context.Products.FindAsync(id);
            return Ok(Product);
        }         
        
    }
}