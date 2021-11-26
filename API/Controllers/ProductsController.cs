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

namespace API.Controllers
{
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

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {

            //var products = await _nongenericrepo.GetProductsAsync();
           // return Ok(products);
             
            //var Products = await _productsRepo.ListAllAsync(); // using Repository Pattern

            var spec = new ProductsWithTypesAndBrandsSpecification(); // Setting the Specification -> Includes and Criteria
            var Products = await _productsRepo.ListAsync(spec); // using Repository Pattern with Specification Pattern


            // var productToReturnDto = Products.Select(product=> new ProductToReturnDto // without using Automapper
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

            var  productToReturnDtoslst = _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto>>(Products);

            return Ok(productToReturnDtoslst);


        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // We do not need to add here as swagger alredy knows it
        //[ProducesResponseType(StatusCodes.Status404NotFound)] // need to add this but this would return default error parameters which are returned by NotFound() method :)
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            //Product product = await _productsRepo.GetByIdAsync(id);  // using Repository Pattern

            var spec = new ProductsWithTypesAndBrandsSpecification(id); // using Repository Pattern with Specification Pattern
            Product product = await _productsRepo.GetEntityWithSpec(spec); // using Repository Pattern with Specification Pattern

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

            if (product == null) return NotFound(new ApiResponse(404));

            var ProductToReturnDto = _mapper.Map<Product,ProductToReturnDto>(product);
            return Ok(ProductToReturnDto);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<ProductBrand>> GetProductBrand()
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