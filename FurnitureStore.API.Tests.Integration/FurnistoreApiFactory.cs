using FurnitureStore.API.Data;
using FurnitureStore.API.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
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
            services.RemoveAll(typeof(FurnitureStoreContext));
            services.AddNpgsql<FurnitureStoreContext>(_dbContainer.GetConnectionString())
            .AddScoped<ICategoryRepository, CategoryRepository>();
            
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
