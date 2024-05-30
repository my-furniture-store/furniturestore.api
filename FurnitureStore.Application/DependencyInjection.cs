using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using MediatR;
using FurnitureStore.Application.Common.Behaviors;
using System.Reflection;

namespace FurnitureStore.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
        });

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


        return services;
    }
}
