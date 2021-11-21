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

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productsRepo,
        IGenericRepository<ProductBrand> productBrandRepo,IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
        {
            _productsRepo=productsRepo;
            _productBrandRepo=productBrandRepo;
            _productTypeRepo=productTypeRepo;
            _mapper=mapper;

        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
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

            var ProductToReturnDto = _mapper.Map<Product,ProductToReturnDto>(product);
            return ProductToReturnDto;
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