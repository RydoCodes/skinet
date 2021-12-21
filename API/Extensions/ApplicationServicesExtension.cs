using API.Errors;
using API.Helpers;
using API.Infrastructure;
using API.Infrastructure.Data;
using API.Middleware;
using Core.interfaces;
using Core.Interfaces;
using Infrastructure.Data;
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
			services.AddScoped<IProductRepository, ProductRepository>(); // Normal Repository
			services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRespository<>)));

            // This is to display validation errors -
            //{{url}}/api/products/4 was expected
            //{{url}}/api/products/xyz causes this.
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actioncontext =>
                {
                    var errors = actioncontext.ModelState
                                    .Where(e => e.Value.Errors.Count > 0)
                                    .SelectMany(x => x.Value.Errors)
                                    .Select(x => x.ErrorMessage).ToArray();

                    var errorresponse = new ApiValidationErrorResponse
                    {
                        Errors = errors.Append("Rydo check Validation Error")
                    };

                    return new BadRequestObjectResult(errorresponse); // BadRequestObjectResult expects an instance of response type and returns status code as 400
                                                                      // Although BadRequest() also returns 400 bad request but its a function present in ControllerBase class.

                };
            });

            return services;
        }
	}
}
