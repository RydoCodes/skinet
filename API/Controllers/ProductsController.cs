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
        private IGenericRepository<ProductBrand> _productBrandRepo;
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

        //[HttpGet]
        //public async Task<ActionResult<List<Product>>> GetProducts()
        //{
        //    var Products = await _nongenericrepo.GetProductsAsync();
        //    return Ok(Products);
        //}

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams productParams)
        //public async Task<ActionResult<List<Product>>> GetProducts()
        {

            //var products = await _nongenericrepo.GetProductsAsync(); // Non Generic Repository Pattern
            // return Ok(products);

            //var Products = await _productsRepo.ListAllAsync();     // Generic Repository Pattern with no soecification
            // return Ok(products);

            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);         // Setting the Specification -> Includes and Criteria
            var Products = await _productsRepo.ListAsyncwithSpec(spec);                            // using Generic Repository Pattern with Specification Pattern

            var countSpec = new ProductWithFiltersForCountSpecification(productParams);     // Setting Specification when getting the total products -> NO Includes and ONLY Criteria
            var totalitems = await _productsRepo.CountAsyncwithSpec(countSpec);                     // using Generic Repository Pattern with Specification Pattern



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

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(Products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalitems, data));


        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // We do not need to add here as swagger alredy knows it
        //[ProducesResponseType(StatusCodes.Status404NotFound)] // need to add this but this would return default error parameters which are returned by NotFound() method :)
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)] // if we remove typeof(apiresponse) then swagger would get 404 but the response header would be of one returned by NotFound() and not NotFound(new ApiResponse(404)
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        //public async Task<ActionResult<Product>> GetProduct(int id)
        {
            // Product Product = await _nongenericrepo.GetProductByIdAsync(id); // Non Generic Repository Pattern
            // return Ok(products);

            //Product product = await _productsRepo.GetByIdAsync(id);  // Generic Repository Pattern with no soecification
            // return Ok(products);

            var spec = new ProductsWithTypesAndBrandsSpecification(id); // using Generic Repository Pattern with Specification Pattern
            Product product = await _productsRepo.GetEntityWithSpec(spec); // using Generic Repository Pattern with Specification Pattern

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

           if (product == null) return NotFound(new ApiResponse(404)); // NotFound() originally results 404 but using new ApiResponse(404) as parameter overrides what NotFound() sends back

            var ProductToReturnDto = _mapper.Map<Product,ProductToReturnDto>(product);
            return Ok(ProductToReturnDto);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<ProductBrand>> GetProductBrand()
        {
           // return Ok(await _nongenericrepo.GetProductBrandsAsync());

            IReadOnlyList<ProductBrand> productbrand = await _productBrandRepo.ListAllAsync();
            return Ok(productbrand);
        }

        [HttpGet("types")]
        public async Task<ActionResult<ProductType>> GetProductType()
        {
            // return Ok(await _nongenericrepo.GetProductTypesAsync());
            IReadOnlyList<ProductType> producttype = await _productTypeRepo.ListAllAsync();
            return Ok(producttype);
        }             
        
    }
}