using FurnitureStore.API.Data;
using FurnitureStore.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
config.AddEnvironmentVariables("FurnitureStoreAPI_");

builder.Services.AddControllers();

builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.Services.InitialiseDB();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapCategoryEndpoints();

app.Run();
