using Application.Common.Services.CleanerData;
using Application.Features.ConfigManager;
using ASPNET.BackEnd;
using ASPNET.BackEnd.Common.Middlewares;
using ASPNET.FrontEnd;
using Infrastructure.DataClean;

var builder = WebApplication.CreateBuilder(args);

//>>> Create Logs folder for Serilog
var logPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "app_data", "logs");
if (!Directory.Exists(logPath))
{
    Directory.CreateDirectory(logPath);
}

builder.Services.AddBackEndServices(builder.Configuration);
builder.Services.AddScoped<IDatabaseCleanerService, DatabaseCleanerService>();
builder.Services.AddScoped<GetConfigByNameRequest>(provider => new GetConfigByNameRequest("defaultName"));
builder.Services.AddFrontEndServices();

var app = builder.Build();

app.RegisterBackEndBuilder(app.Environment, app, builder.Configuration);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseRouting();
app.UseCors();
app.UseMiddleware<GlobalApiExceptionHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();

app.MapFrontEndRoutes();
app.MapBackEndRoutes();

app.Run();
