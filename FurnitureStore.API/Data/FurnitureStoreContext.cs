using FurnitureStore.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace FurnitureStore.API.Data;

public class FurnitureStoreContext : DbContext
{
    public FurnitureStoreContext(DbContextOptions<FurnitureStoreContext> options):base(options)
    {
        
    }

    public DbSet<Category> Categories { get; set; }
}
