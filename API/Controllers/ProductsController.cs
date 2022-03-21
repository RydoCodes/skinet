using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using API.Infrastructure;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Core.interfaces;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;
using API.Errors;
using Microsoft.AspNetCore.Http;
using API.Helpers;

namespace API.Controllers
{
    // NotFound() :  Creates an Microsoft.AspNetCore.Mvc.NotFoundResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound
    //ok ()       :  Creates an Microsoft.AspNetCore.Mvc.OkObjectResult object that produces an Microsoft.AspNetCore.Http.StatusCodes.Status200OK


    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;
        private readonly IProductRepository _nongenericrepo;

        public ProductsController(IGenericRepository<Product> productsRepo,
        IGenericRepository<ProductBrand> productBrandRepo,IGenericRepository<ProductType> productTypeRepo, IMapper mapper, IProductRepository nongenericrepo)
        {
            _productsRepo=productsRepo;
            _productBrandRepo=productBrandRepo;
            _productTypeRepo=productTypeRepo;
            _mapper=mapper;
            _nongenericrepo = nongenericrepo;
        }

        /* ---------------------------------Without including Product Type and Product Brand navigational properties----------------------------------------------------------*/
        [HttpGet("GetProductsNoBrandTypes")]
        public async Task<ActionResult<List<Product>>> GetProductsNoBrandTypes()
        {
            IReadOnlyList<Product> Products = await _nongenericrepo.GetProductsAsync_NoBrandTypes();
            return Ok(Products);
        }

        [HttpGet("GetProductByIDNoBrandTypes/{id}")]
        public async Task<ActionResult<Product>> GetProductByIDNoBrandTypes(int id)
        {
            Product Product = await _nongenericrepo.GetProductById__NoBrandTypes(id);   
            return Ok(Product);
        }

        [HttpGet("GetProductBrandNonGenericCopy1")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductBrandNonGenericCopy1()
        {
            IReadOnlyList<ProductBrand> productbrands = await _nongenericrepo.GetProductBrandsAsync();
            return Ok(productbrands);
        }

        [HttpGet("GetProductTypeNonGenericCopy1")]
        public async Task<ActionResult<ProductType>> GetProductTypeNonGenericCopy1()
        {
            IReadOnlyList<ProductType> producttype = await _nongenericrepo.GetProductTypesAsync();
            return Ok(producttype);
        }
        
        /* -----------------------------------------including Product Type and Product Brand navigational properties---------------------------------------------------*/
        [HttpGet("GetProductsEager")]
        public async Task<ActionResult<List<Product>>> GetProductsEager()
        {
            IReadOnlyList<Product> Products = await _nongenericrepo.GetProduct_Eager();
            return Ok(Products);
        }

        [HttpGet("GetProductsEager/{id}")]
        public async Task<ActionResult<Product>> GetProductbyIDEager(int id)
        {
            Product Product = await _nongenericrepo.GetProductById_Eager(id); // Non Generic Repository Pattern
            return Ok(Product);
        }

        [HttpGet("GetProductBrandNonGenericCopy2")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductBrandNonGenericCopy2()
        {
            IReadOnlyList<ProductBrand> productbrands = await _nongenericrepo.GetProductBrandsAsync();
            return Ok(productbrands);
        }

        [HttpGet("GetProductTypeNonGenericCopy2")]
        public async Task<ActionResult<ProductType>> GetProductTypeNonGenericCopy2()
        {
            IReadOnlyList<ProductType> producttype = await _nongenericrepo.GetProductTypesAsync();
            return Ok(producttype);
        }

        /* ------------Generic Repository Pattern with no specification------------------------*/

        [HttpGet("GetProductsGeneric_NoSpecification")]
        public async Task<ActionResult<List<Product>>> GetProductsGeneric_NoSpecification()
        {
            var Products = await _productsRepo.ListAllAsync(); 
            return Ok(Products);
        }

        [HttpGet("GetProductGeneric_NoSpecification")]
        public async Task<ActionResult<Product>> GetProductGeneric_NoSpecification(int id)
        {
            Product product = await _productsRepo.GetByIdAsync(id);
            return Ok(product);
        }

        /* ------------using Generic Repository Pattern with Specification Pattern------------------------*/
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {

            ProductsWithTypesAndBrandsSpecification spec = new ProductsWithTypesAndBrandsSpecification(productParams);  // Setting the Specification -> Includes and Criteria
            IReadOnlyList<Product> Products = await _productsRepo.ListAsyncwithSpec(spec);                            // using Generic Repository Pattern with Specification Pattern

            ProductWithFiltersForCountSpecification countSpec = new ProductWithFiltersForCountSpecification(productParams);     // Setting Specification when getting the total products -> NO Includes and ONLY Criteria
            int totalitems = await _productsRepo.CountAsyncwithSpec(countSpec);                     // using Generic Repository Pattern with Specification Pattern

            IReadOnlyList<ProductToReturnDto> data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(Products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalitems, data));

        }

        
        [ProducesResponseType(StatusCodes.Status200OK)] // We do not need to add here as swagger alredy knows it
        //[ProducesResponseType(StatusCodes.Status404NotFound)] // need to add this but this would return default error parameters which are returned by NotFound() method :)
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)] // if we remove typeof(apiresponse) then swagger would get 404 but the response header would be of one returned by NotFound() and not NotFound(new ApiResponse(404)
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {

            ProductsWithTypesAndBrandsSpecification spec = new ProductsWithTypesAndBrandsSpecification(id); // using Generic Repository Pattern with Specification Pattern
            Product product = await _productsRepo.GetEntityWithSpec(spec); // using Generic Repository Pattern with Specification Pattern

           if (product == null) return NotFound(new ApiResponse(404)); // NotFound() originally results 404 but using new ApiResponse(404) as parameter overrides what NotFound() sends back

            ProductToReturnDto ProductToReturnDto = _mapper.Map<Product,ProductToReturnDto>(product);
            return Ok(ProductToReturnDto);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductBrand()
        {
            IReadOnlyList<ProductBrand> productbrand = await _productBrandRepo.ListAllAsync();
            return Ok(productbrand);

        }

        [HttpGet("types")]
        public async Task<ActionResult<ProductType>> GetProductType()
        {
            IReadOnlyList<ProductType> producttype = await _productTypeRepo.ListAllAsync();
            return Ok(producttype);
        }

    }
}

// var productToReturnDto = Products.Select(product=> new ProductToReturnDto    // without using Automapper
// {
//     Id = product.Id,
//     Name = product.Name,
//     Description = product.Description,
//     PictureUrl = product.PictureUrl,
//     Price = product.Price,
//     ProductBrand = product.ProductBrand.Name,
//     ProductType = product.ProductType.Name
// });

// return Ok(productToReturnDto);



// var productToReturnDto = new ProductToReturnDto // without using Automapper
// { 
//     Id = product.Id,
//     Name = product.Name,
//     Description = product.Description,
//     PictureUrl = product.PictureUrl,
//     Price = product.Price,
//     ProductBrand = product.ProductBrand.Name,
//     ProductType = product.ProductType.Name
// };

// return Ok(productToReturnDto);
