using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
	public class BasketRepository : IBasketRepository
	{
		private readonly IDatabase _database;

		public BasketRepository(IConnectionMultiplexer redis) // IConnectionMultiplexer :Represents the abstract multiplexer API
		{
			_database = redis.GetDatabase(); //GetDatabase : Obtain an interactive connection to a database inside redis
		}

		public async Task<CustomerBasket> GetBasketAsync(string basketId)
		{
			RedisValue data = await _database.StringGetAsync(basketId); // RedisValue : Represents values that can be stored in redis
			return data.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(data); // we are convering a redisvalue into customerbasket
		}

		public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
		{
			bool created = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
			// StringSetAsync : Set key to hold the string value. If key already holds a value, it is overwritte  regardless of its type.

			if (!created) return null;

			CustomerBasket updatedbasket = await GetBasketAsync(basket.Id);
			return updatedbasket;
		}

		public async Task<bool> DeleteBasketAsnc(string basketId)
		{
			bool result = await _database.KeyDeleteAsync(basketId);
			return result; // true or false
		}
	}
}
