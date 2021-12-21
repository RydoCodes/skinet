using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Infrastructure.Data;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GenericRespository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;

        public GenericRespository(StoreContext context)
        {
            _context =context;
        }

        // Get ProductByID without specification pattern but then you wont get ProductType and ProductBrand
        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id); //Set<T> - Creates a DbSet<TEntity> that can be used to query and save instances of TEntity
        }

        
        public async Task<IReadOnlyList<T>> ListAllAsync()// Get All Product without specification pattern but then you wont get ProductType and ProductBrand                                                        
        {                                                  // Used to get ProductBrands and ProductTypes
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)  // Get ProducctByID with specification pattern and get ProductType and ProductBrand
        {
            IQueryable<T> finalquery = ApplySpecification(spec);

            return await finalquery.FirstOrDefaultAsync(); // FirstOrDefaultAsync() is defined for a variable of type IQueryable but FindAsync is not.
        }

        public async Task<IReadOnlyList<T>> ListAsyncwithSpec(ISpecification<T> spec) // Get All Product with specification pattern and get ProductType and ProductBrand
        {
            IQueryable<T> finalquery = ApplySpecification(spec); //Applying the Specification->Includes and Criteria to the Enntity which is Product.

            return await finalquery.ToListAsync(); // ToListAsync() : Asynchronously creates a List<T> from an IQueryable<out T> by enumerating it asynchronously.
        }

        public async Task<int> CountAsyncwithSpec(ISpecification<T> spec) // Get list of total products sorted, or, searched, or by brand or type
        {
            IQueryable<T> finalquery = ApplySpecification(spec);
            return await finalquery.CountAsync();
        }

        ////////// Below is Private Method ////////////
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }

    }
}