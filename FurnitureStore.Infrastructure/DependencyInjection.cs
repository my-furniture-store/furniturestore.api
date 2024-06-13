using FluentMigrator.Runner;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Infrastructure.Authentication;
using FurnitureStore.Infrastructure.Categories;
using FurnitureStore.Infrastructure.Common;
using FurnitureStore.Infrastructure.Products;
using FurnitureStore.Infrastructure.SubCategories;
using FurnitureStore.Infrastructure.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace FurnitureStore.Infrastructure;

public static class DependencyInjection
{
    public static void RunMigarions(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("FurnitureStoreDB");

        var datasourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        datasourceBuilder.EnableDynamicJson();
        var datasource = datasourceBuilder.Build();

        services.AddDbContext<FurnitureStoreDbContext>(options =>
                {
                    options.UseNpgsql(datasource);
                })
                .AddScoped<ICategoriesRepository, CategoriesRepository>()
                .AddScoped<ISubCategoriesRepository, SubCategoriesRepository>()
                .AddScoped<IProductsRepository, ProductsRepository>()
                .AddScoped<IUsersRepository, UsersRepository>()
                .AddScoped<IUnitofWork>(serviceProvider => serviceProvider.GetRequiredService<FurnitureStoreDbContext>());

        // Configure FluentMigrator
        services
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                // Add Postgres support
                .AddPostgres11_0()
                // Set the connection string
                .WithGlobalConnectionString(connectionString)
                // Define the assemblies containing the migrations
                .ScanIn(typeof(DependencyInjection).Assembly).For.Migrations()
                )
            ;

        // Configure JwtOptions
        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

        // Register JwtProvider
        services.AddSingleton<IJwtProvider, JwtProvider>();

        return services;
    }
}
