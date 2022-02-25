using API.Errors;
using API.Helpers;
using API.Infrastructure;
using API.Infrastructure.Data;
using API.Middleware;
using Core.interfaces;
using Core.Interfaces;
using Core.Interfaces.Identity;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Linq;

namespace API.Extensions
{
	public static class ApplicationServicesExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
            // Normal Repository Registration
            services.AddScoped<ITokenService, TokenService>();
			services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBasketRepository, BasketRepository>();

            // Generic Repository Registration
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRespository<>)));

            // This is to display validation errors -
            //{{url}}/api/products/4 was expected
            //{{url}}/api/products/xyz causes this.
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actioncontext =>
                {
                    string[] errors = actioncontext.ModelState
                                    .Where(e => e.Value.Errors.Count > 0)
                                    .SelectMany(x => x.Value.Errors)
                                    .Select(x => x.ErrorMessage).ToArray();

                    ApiValidationErrorResponse errorresponse = new ApiValidationErrorResponse
                    {
                        Errors = errors.Append("Rydo Validation Error")
                    };

                    return new BadRequestObjectResult(errorresponse); // BadRequestObjectResult is a class expects an instance of response type and returns status code as 400
                                                                      // Although BadRequest() also returns 400 bad request but its a function present in ControllerBase class.

                };
            });

            return services;
        }
	}
}
