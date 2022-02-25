using API.Errors;
using API.Extensions;
using API.Helpers;
using API.Infrastructure;
using API.Infrastructure.Data;
using API.Middleware;
using Core.interfaces;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Linq;
using System.Reflection;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddHttpContextAccessor();
            services.AddControllers(); // Adds services for controllers to the specified IServiceCollection.
                                       //This method will not register services used for views or pages.

            services.AddDbContext<StoreContext>(x => x.UseSqlite(_config.GetConnectionString("SqliteConnection")));
            // "SqliteConnection": "Data source=skinet.db",


            //Registering MappingProfile as the configuration for AutoMapper
            // GetExecutingAssembly- Gets the assembly that contains the code that is currently executing.
            // services.AddAutoMapper(typeof(MappingProfiles));// good when you have only one Mapping Profile
            services.AddAutoMapper(Assembly.GetExecutingAssembly());  // good when yo have multiple mapping profiles 


            services.AddDbContext<AppIdentityDbContext>(x => x.UseSqlite(_config.GetConnectionString("IdentityConnection")));
            // x - DbContextOptionsBuilder //"IdentityConnection": "Data source=identity.db",

            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                ConfigurationOptions configuration = ConfigurationOptions.Parse(_config.GetConnectionString("Redis"),true);
                return ConnectionMultiplexer.Connect(configuration);
            });


            services.AddApplicationServices();        // Extension : ApplicationServicesExtension.cs
            services.AddSwaggerDocumentation();       // Extension : SwaggerServiceExtensions.cs
            services.AddIdentityServices(_config);    // Extension : IdentityServiceExtension.cs

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseMiddleware<ExceptionMiddleware>(); // Handles Null reference Exceptions. OR  app.UseExceptionHandler("/Error");

            app.UseSwaggerDocumentation(); // Extension : SwaggerServiceExtensions.cs

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}"); // If aN endpoint is unavailable. Handling 404 URL Not Found Error and 401 Authorised

            app.UseHttpsRedirection();

            // Choses and sets up the endoint from the list of endpoints set up by  endpoints.MapControllers();
            app.UseRouting();

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
                
            app.UseAuthorization();

            app.UseSwaggerUI();

            // Executes the selected endpoint
            app.UseEndpoints(endpoints =>
            {
                // All ActionMethods will be registered as an Endpoint
                endpoints.MapControllers();
            });
        }
    }
}
