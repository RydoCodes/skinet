
using System.Linq;
using System.Reflection;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductType> ProductTypes {get;set;}

        public DbSet<ProductBrand> ProductBrands {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // This will apply the settings for migration in ProductConfiguration.cs


        // Order By Price does not work because Price property is set to decimal and SQLLite cannot order by a property in decimal. So we need to convert the Price from Decima
        // to float when hitting the database and back to decimal when taking data back from database.
            if(Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach(var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));

                    foreach(var property in properties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double>();
                    //HasConversion : Configures the property so that the property value is converted to the given type before writing to the database and converted back when reading from the
                                                                                                                 
                    }
                }
            }
        }
    }
}   