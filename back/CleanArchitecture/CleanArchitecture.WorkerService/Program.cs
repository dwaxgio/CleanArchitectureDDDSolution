using CleanArchitecture.WorkerService.Jobs;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Repositories;
using CleanArchitecture.Domain.Interfaces;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Configuración de Hangfire
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage("data source=PC_EDWARD\\SQLEXPRESS; initial catalog=DB_CleanArchitectureDDDSolution; MultipleActiveResultSets=true; TrustServerCertificate=True; Integrated Security=True"));
builder.Services.AddHangfireServer();

// Registro de DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("data source=PC_EDWARD\\SQLEXPRESS; initial catalog=DB_CleanArchitectureDDDSolution; MultipleActiveResultSets=true; TrustServerCertificate=True; Integrated Security=True"));

// Registro del repositorio
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Registro del job
builder.Services.AddScoped<UserSyncJob>();

var app = builder.Build();

// Configuración del dashboard de Hangfire
app.UseHangfireDashboard();

// Registro del job recurrente
RecurringJob.AddOrUpdate<UserSyncJob>(
    "sync-users",
    job => job.SyncUsers(),
    Cron.Hourly);

app.Run();
