using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProjectNet.Api;
using ProjectNet.Application;
using ProjectNet.Domain.Common;
using ProjectNet.Infrastructure;
using ProjectNet.Infrastructure.Persistence;
using ProjectNet.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ProjectNet Orders API",
        Version = "v1",
        Description = "Demo microservice (Hexagonal Architecture, CQRS, DDD, Event-Driven)."
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy => policy
        .WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (app.Environment.IsDevelopment())
    {
        await DatabaseSeeder.SeedAsync(db);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Frontend");
app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    try { await next(); }
    catch (ProjectNet.Domain.Common.DomainException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
    catch (FluentValidation.ValidationException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
    }
});

app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }


