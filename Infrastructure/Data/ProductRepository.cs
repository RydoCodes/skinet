using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Data;
using Core.Entities;
using Core.interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    // This is a NON Generic Repository.
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;

        public ProductRepository(StoreContext context)
        {
            _context=context;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            //return await _context.Products.ToListAsync(); // Without including Product Type and Product Brand

            return await _context.Products //including Product Type and Product Brand
            .Include(p => p.ProductType)
            .Include(p => p.ProductBrand)
            .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            //return await _context.Products.FindAsync(id); // Without including Product Type and Product Brand

            return await _context.Products //including Product Type and Product Brand
            .Include(p => p.ProductType)
            .Include(p => p.ProductBrand)
            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }
}