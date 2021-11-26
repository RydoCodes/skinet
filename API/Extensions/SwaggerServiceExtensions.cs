using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
	public static class SwaggerServiceExtensions
	{
		public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
		{
			services.AddSwaggerGen(rydoswag =>
			{
				rydoswag.SwaggerDoc("v1", new OpenApiInfo { Title = "RydoApi", Version = "v1" }); // Creates the Swagger Documentation and Swagger json file.
			});
			
			return services;
		}

		public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
		{
			app.UseSwagger(); // helps to generate the json
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rydo Skinet API"); // helps generate the webpage that we are looking at.
			});

			return app;
		}

	}
}
