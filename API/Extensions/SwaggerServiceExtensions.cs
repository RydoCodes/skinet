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

				// Swagger Setup to support Bearer Authentication
				var securitychema = new OpenApiSecurityScheme
				{
					Description = "JWT Auth Bearer Scheme",
					Name = "Authorzaton",
					In = ParameterLocation.Header, // we tell it where authentication needs to be provided which is in header called Authorization.
					Type = SecuritySchemeType.Http, // as our request are Http Request
					Scheme = "bearer",
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
					}
				};

				// AddSecurityDefinition : Add one or more "securityDefinitions", describing how your API is protected to the generated Swagger
				rydoswag.AddSecurityDefinition("Bearer", securitychema);
				var securityRequirement = new OpenApiSecurityRequirement
				{
					{
						securitychema,
						new[]
						{
							"Bearer"
						}
					}
				};


				// AddSecurityRequirement - Adds a global security requirement
				rydoswag.AddSecurityRequirement(securityRequirement);

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
