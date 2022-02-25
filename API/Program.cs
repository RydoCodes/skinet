using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Infrastructure;
using API.Infrastructure.Data;
using Core.Entities.Identity;
using Infrastructure.Data.SeedData;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            using(IServiceScope scope = host.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                ILoggerFactory loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    StoreContext context = services.GetRequiredService<StoreContext>();
                    await context.Database.MigrateAsync(); //Asynchronously applies any pending migrations for the context to the database. 
                                                           //Will create the database if it does not already exist everytime when we start our application.

                    await StoreContextSeed.SeedAsync(context,loggerFactory); // After creating the database, seed it if its empty.

                    UserManager<AppUser> userManager = services.GetRequiredService<UserManager<AppUser>>();
                    AppIdentityDbContext identityContext = services.GetRequiredService<AppIdentityDbContext>();
                    await identityContext.Database.MigrateAsync();//Asynchronously applies any pending migrations for the context to the database. 
                                                                  //Will create the database if it does not already exist everytime when we start our application.

                    await AppIdentityDbContextSeed.SeedUsersAsync(userManager); // Seeds an AppUser with its address with a default password.
                     
                }
                catch(Exception ex)
                {
                    ILogger<Program> rydologger = loggerFactory.CreateLogger<Program>();
                    rydologger.LogError(ex,"An Error has been occured while attempting to either Create Migration or Creating a Database");
                }

            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
