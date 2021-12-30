using Core.Entities.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Extensions
{
	public static class IdentityServiceExtension
	{
		public static IServiceCollection  AddIdentityServices(this IServiceCollection services, IConfiguration config)
		  {
			IdentityBuilder builder = services.AddIdentityCore<AppUser>(); // AddIdentityCore: Adds and configures the identity system for the specified User type
			builder = new IdentityBuilder(builder.UserType, builder.Services);
			builder.AddEntityFrameworkStores<AppIdentityDbContext>(); // Adds an Entity Framework implementation of identity information stores, UserManager Service is containted in EFStores.

			builder.AddSignInManager<SignInManager<AppUser>>(); //Adds a Microsoft.AspNetCore.Identity.SignInManager for the UserType.

			//we actually receive this token here from the client
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					//TokenValidationParameters:  we need to tell Identity what we want to validate here.
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true, //False might leave anonymous authentication ON 
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])), // we set this up when creating the token
						ValidIssuer = config["Token:Issuer"], //https://Localhost:5001
						ValidateIssuer=true, // which in readl is Token: Issuer = https://Localhost:5001,
						ValidateAudience = false
					};
				});

			return services;

		}
	}
}
