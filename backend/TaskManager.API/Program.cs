using System.Text.Json.Serialization;

using TaskManager.Application;
using TaskManager.Infrastructure;
using TaskManager.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddOpenApi();

builder.Services
    .AddApplication()
    .AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/openapi/v1.json", "TaskManager API"));
}

app.UseHttpsRedirection();

app.MapControllers();

await DbSeeder.SeedAsync(app.Services);

app.Run();
