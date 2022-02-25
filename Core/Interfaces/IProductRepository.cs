using Core.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Core.interfaces
{
    // This repository does not use generics
    public interface IProductRepository
    {
        /* ------------------Without including Product Type and Product Brand navigational properties------------------*/
        Task<Product> GetProductById__NoBrandTypes(int id);

        Task<IReadOnlyList<Product>> GetProductsAsync_NoBrandTypes();

        /* ------------------Get List of ProductBrand and ProductType------------------*/
        Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();

        Task<IReadOnlyList<ProductType>> GetProductTypesAsync();

        /* ------------------including Product Type and Product Brand navigational properties------------------*/

        Task<Product> GetProductById_Eager(int id);

        Task<IReadOnlyList<Product>> GetProduct_Eager();

    }
}