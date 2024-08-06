using Core.Interfaces;
using Services.EmailService;
using Core.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.Configure<MailSettingsModel>(builder.Configuration.GetSection("MailSettings"));

/*-----Services-----*/
builder.Services.AddTransient<IEmailService, EmailService>();
/*-----------------*/

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
