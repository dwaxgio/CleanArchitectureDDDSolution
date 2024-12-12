using CleanArchitecture.WorkerService.Jobs;
using Hangfire;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddHangfire(config => config.UseSqlServerStorage("ConnectionString"));
builder.Services.AddHangfire(config => config.UseSqlServerStorage("data source=PC_EDWARD\\SQLEXPRESS; initial catalog=DB_CleanArchitectureDDDSolution; MultipleActiveResultSets=true; TrustServerCertificate=True; Integrated Security=True"));
builder.Services.AddHangfireServer();

var app = builder.Build();

app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<UserSyncJob>("sync-users", job => job.SyncUsers(), Cron.Hourly);

app.Run();
