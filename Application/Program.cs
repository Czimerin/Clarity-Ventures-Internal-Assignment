using Core.Interfaces;
using Services.EmailService;
using Core.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logFolder = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
if (!Directory.Exists(logFolder))
{
    Directory.CreateDirectory(logFolder);
}

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(Path.Combine(logFolder, "log-.txt"), rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddControllers();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MailSettingsModel>(builder.Configuration.GetSection("MailSettings"));

/*-----Services-----*/
builder.Services.AddTransient<IEmailService, EmailService>();
/*-----------------*/

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
