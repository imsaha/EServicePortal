using EServicePortal.Application;
using EServicePortal.Infrastructure;
using EServicePortal.Infrastructure.Persistence;
using EServicePortal.Middlewares;
using EServicePortal.MySqlPersistence;
using EServicePortal.PostgresPersistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddGlobalErrorHandler();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

if (builder.Configuration["UseMySql"] == "True")
{
    builder.Services.AddMySqlPersistence(builder.Configuration);
}
else
{
    builder.Services.AddPostgresPersistence(builder.Configuration);
}

var app = builder.Build();

using var scope = app.Services.CreateScope();
var serviceProvider = scope.ServiceProvider;
var context = serviceProvider.GetRequiredService<AppDbContext>();
await context.Database.MigrateAsync();

app.UseCors(b => b
    .SetIsOriginAllowed(host => true)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalErrorHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
