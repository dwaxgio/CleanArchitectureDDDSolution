
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Custom code
builder.Services.AddDbContext<AppDbContext>(options =>
   //options.UseSqlServer("ConnectionString"));  // *validate db connection
   options.UseSqlServer("data source=PC_EDWARD\\SQLEXPRESS; initial catalog=DB_CleanArchitectureDDDSolution; MultipleActiveResultSets=true; TrustServerCertificate=True; Integrated Security=True", 
   b => b.MigrationsAssembly("CleanArchitecture.Infrastructure")));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddControllers();
// Custom code //

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
