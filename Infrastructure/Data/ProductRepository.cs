using System.Collections.Generic;
using System.Linq;
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
			_context = context;
		}

		/* ------------------Without including Product Type and Product Brand navigational properties------------------*/
		public async Task<IReadOnlyList<Product>> GetProductsAsync_NoBrandTypes()
		{
			IQueryable<Product> ProductsQuery =  _context.Products;
			IReadOnlyList<Product> Products = await ProductsQuery.ToListAsync();
			return Products;
		}
		public async Task<Product> GetProductById__NoBrandTypes(int id)
		{
			DbSet<Product> ProductQuery = _context.Products;
			Product product = await ProductQuery.FindAsync(id);
			return product;
		}

		/* ------------------Get List of ProductBrand and ProductType------------------*/
		public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
		{
			IReadOnlyList<ProductBrand> ProductBrands = await _context.ProductBrands.ToListAsync();
			return ProductBrands;
		}

		public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
		{
			IReadOnlyList<ProductType> ProductTypes = await _context.ProductTypes.ToListAsync();
			return ProductTypes;
		}

		/* ------------------including Product Type and Product Brand navigational properties------------------*/
		public async Task<Product> GetProductById_Eager(int id)
		{
			IQueryable<Product> ProductQuery = _context.Products
														.Include(p => p.ProductType)
														.Include(p => p.ProductBrand);

			Product product = await ProductQuery.FirstOrDefaultAsync(p => p.Id == id);// point when the query is sent to SQL
			return product;
		}

		//including Product Type and Product Brand navigational properties
		public async Task<IReadOnlyList<Product>> GetProduct_Eager()
		{
			IReadOnlyList<Product> ProductBrands = await _context.Products
														.Include(p => p.ProductType)
														.Include(p => p.ProductBrand)
														.ToListAsync(); //point when the query is sent to SQL
			return ProductBrands;
		}
	}
}