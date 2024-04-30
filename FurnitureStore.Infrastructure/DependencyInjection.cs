﻿using FluentMigrator.Runner;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Infrastructure.Categories;
using FurnitureStore.Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        services.AddNpgsql<FurnitureStoreDbContext>(connectionString)
                .AddScoped<ICategoriesRepository, CategoriesRepository>();

        // Configure FluentMigrator
        services
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                // Add Postgres support
                .AddPostgres11_0()
                // Set the connection string
                .WithGlobalConnectionString(connectionString)
                // Define the assemblies containing the migrations
                .ScanIn(typeof(DependencyInjection).Assembly).For.Migrations());

        return services;
    }
}