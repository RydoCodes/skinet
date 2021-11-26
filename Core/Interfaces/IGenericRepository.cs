using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
         Task<T> GetByIdAsync(int id); // Get Product, ProductBrand and ProductType by its ID
         Task<IReadOnlyList<T>> ListAllAsync(); // Get a list of Product, ProductBrand and ProductType
 
         Task<T> GetEntityWithSpec(ISpecification<T> spec);

         Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
    }
}