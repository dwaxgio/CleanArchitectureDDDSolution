using CleanArchitecture.WorkerService.Jobs;
using Hangfire;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHangfire(config => config.UseSqlServerStorage("YourConnectionString"));
builder.Services.AddHangfireServer();

var app = builder.Build();

app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<UserSyncJob>("sync-users", job => job.SyncUsers(), Cron.Hourly);

app.Run();
