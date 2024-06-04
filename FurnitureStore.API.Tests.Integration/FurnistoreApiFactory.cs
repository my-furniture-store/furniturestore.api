using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Infrastructure.Categories;
using FurnitureStore.Infrastructure.Common;
using FurnitureStore.Infrastructure.Products;
using FurnitureStore.Infrastructure.SubCategories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Npgsql;
using Testcontainers.PostgreSql;

namespace FurnitureStore.API.Tests.Integration;

public class FurnistoreApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = 
        new PostgreSqlBuilder()
                .WithImage("postgres:latest")
                .WithDatabase("furniture_store")
                .WithUsername("fs_test")
                .WithPassword("test1234!")
                .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
        });


        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(FurnitureStoreDbContext));

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(_dbContainer.GetConnectionString());
            dataSourceBuilder.EnableDynamicJson();
            var dataSource = dataSourceBuilder.Build();
            
            services.AddDbContext<FurnitureStoreDbContext>(options =>
            {
                options.UseNpgsql(dataSource);
            })
            .AddScoped<ICategoriesRepository, CategoriesRepository>()
            .AddScoped<ISubCategoriesRepository, SubCategoriesRepository>()
            .AddScoped<IProductsRepository, ProductsRepository>()
            .AddScoped<IUnitofWork>(serviceProvider => serviceProvider.GetRequiredService<FurnitureStoreDbContext>());
            
        });
    }


    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}
