using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using API.Infrastructure.Data;
using Core.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.SeedData
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.ProductBrands.Any())
                {
                    // This code will execute from Program.cs file and hence we need to go to upper level to reach infrastructure folder.
                    string brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");

                    List<ProductBrand> brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    foreach (ProductBrand item in brands)
                    {
                        context.ProductBrands.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

                if(!context.ProductTypes.Any())
                {
                    string typesdata = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");

                    List<ProductType> types = JsonSerializer.Deserialize<List<ProductType>>(typesdata);

                    foreach(ProductType item in types)
                    {
                        context.ProductTypes.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

                if(!context.Products.Any())
                {
                    string productsdata = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");

                    List<Product> products = JsonSerializer.Deserialize<List<Product>>(productsdata);

                    foreach(Product item in products)
                    {
                        context.Products.Add(item);
                    }

                    await context.SaveChangesAsync();
                    
                    
                }
            }
            catch(Exception ex)
            {
                ILogger<StoreContextSeed> logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}