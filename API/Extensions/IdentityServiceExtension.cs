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


			//// Changing .Net Core setup to accept weak Passwords.
			//// Before testing comment out Annotations on RegisterDTO and LoginDTO used to handle weak password on the dto level.
			//services.AddIdentity<AppUser, IdentityRole>(rydoconfigureoptions => {
			//	rydoconfigureoptions.Password.RequiredLength = 2;
			//	rydoconfigureoptions.Password.RequiredUniqueChars = 0;
			//	rydoconfigureoptions.Password.RequireUppercase = false;
			//	rydoconfigureoptions.Password.RequireNonAlphanumeric = false;
			//	rydoconfigureoptions.Password.RequireLowercase = false;
			//	rydoconfigureoptions.Password.RequireDigit = false;
			//}) // Add Identity services to the App. 

		 // .AddEntityFrameworkStores<AppIdentityDbContext>(); // Using Entity Framework core to retrieve user and role information from the underlying sql servr databas using EF Core.

			//we actually receive this token here from the client
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					//TokenValidationParameters:  we need to tell Identity what we want to validate here. We are validating the key and issuer for now.
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true, //False might leave anonymous authentication ON 
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])), // we set this up when creating the token

						ValidateIssuer = true, // which in readl is Token: Issuer = https://Localhost:5001,
						ValidIssuer = config["Token:Issuer"], //https://Localhost:5001

						ValidateAudience = false // false because we did not set up Audience in the Token Descriptor.
					};
				});

			return services;

		}
	}
}
