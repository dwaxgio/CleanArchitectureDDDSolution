using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin() // Permite cualquier origen
               .AllowAnyMethod() // Permite cualquier m�todo (GET, POST, etc.)
               .AllowAnyHeader(); // Permite cualquier encabezado
    });
});

// Registro de DbContext y Repositorio
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("data source=PC_EDWARD\\SQLEXPRESS; initial catalog=DB_CleanArchitectureDDDSolution; MultipleActiveResultSets=true; TrustServerCertificate=True; Integrated Security=True",
    b => b.MigrationsAssembly("CleanArchitecture.Infrastructure")));
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Agregar controladores
builder.Services.AddControllers();

// Configuraci�n de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Uso de CORS
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
