using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Categories;
using FurnitureStore.Domain.Products;
using FurnitureStore.Domain.SubCategories;
using FurnitureStore.Domain.Users;
using Microsoft.EntityFrameworkCore;
using SmartEnum.EFCore;
using System.Reflection;

namespace FurnitureStore.Infrastructure.Common;

public class FurnitureStoreDbContext : DbContext, IUnitofWork
{
    public FurnitureStoreDbContext(DbContextOptions<FurnitureStoreDbContext> options) : base(options)
    {
        
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }  

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.ConfigureSmartEnum();
    }

    public async Task CommitChangesAsync()
    {
        await base.SaveChangesAsync();
    }
}
