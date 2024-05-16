using FurnitureStore.Application;
using FurnitureStore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
config.AddEnvironmentVariables("FurnitureStoreAPI_");

builder.Services.AddControllers();

builder.Services.AddApplication()
                .AddInfrastructure(config);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Services.RunMigarions();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
