using TimesheetReportGenerator.Models;
using TimesheetReportGenerator.Repositories;
using TimesheetReportGenerator.Repositories.Interfaces;
using TimesheetReportGenerator.Services;
using TimesheetReportGenerator.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21)))); // Change this to your MySQL version

builder.Services.AddScoped<ITicketTrackingRepository, TicketTrackingRepository>();
builder.Services.AddScoped<ITicketService, TicketService>();

builder.Services.AddScoped<IAzureDevOpsService, AzureDevOpsService>();
builder.Services.AddScoped<IExcelService, ExcelService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(builder =>
    builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

app.MapControllers();

app.Run();

