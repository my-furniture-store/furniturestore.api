using FurnitureStore.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FurnitureStore.API.Data;

public static class DataExtensions
{
    public static async Task InitialiseDB(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FurnitureStoreContext>();
        await dbContext.Database.MigrateAsync();
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("FurnitureStoreDB");

        services.AddNpgsql<FurnitureStoreContext>(connectionString)
            .AddScoped<ICategoryRepository, CategoryRepository>();

        return services;
    }
}
