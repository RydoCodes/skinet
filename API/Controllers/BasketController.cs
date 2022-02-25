using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos.BasketDTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketrepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketrepository, IMapper mapper)
        {
            _basketrepository = basketrepository;
            _mapper = mapper;
        }

        [HttpGet]
        // If the basket is null then it creates a new basket with Id as id
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            CustomerBasket basket = await _basketrepository.GetBasketAsync(id);

            if(basket==null)
            {
                return new CustomerBasket(id);
            }

            return basket;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            CustomerBasket customerbasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);

            CustomerBasket updatedbasket = await _basketrepository.UpdateBasketAsync(customerbasket);
            return Ok(updatedbasket);
        }

        [HttpDelete]
        public async Task DeleteBasketAsync(string id)
        {
            await _basketrepository.DeleteBasketAsnc(id);
        }
    }
}