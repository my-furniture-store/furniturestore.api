using FurnitureStore.Application;
using FurnitureStore.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
config.AddEnvironmentVariables("FurnitureStoreAPI_");

builder.Services.AddControllers();

builder.Services.AddApplication()
                .AddInfrastructure(config);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Furniture Store API", Version = "v1" });
});

builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Furniture Store API");
        c.RoutePrefix = string.Empty;
    });
}

app.Services.RunMigarions();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
