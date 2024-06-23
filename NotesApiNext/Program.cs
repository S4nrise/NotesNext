using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NotesApiNext.Database;
using NotesApiNext.Interfaces;
using NotesApiNext.Mapping;
using NotesApiNext.Services;
using NotesApiNext.Settings;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();
var dateTimeProvider = new DateTimeProvider();
builder.Services.AddSingleton<IDateTimeProvider>(dateTimeProvider);
builder.Services.AddAutoMapper(config =>
     {
         config.AddProfile(new NoteMappingProfile(dateTimeProvider));
     });

var postgreSqlConnection = builder.Configuration
    .GetRequiredSection(nameof(PostgreSQLConnection))
    .Get<PostgreSQLConnection>();
builder.Services.AddDbContext<NotesNextDbContext>(options =>
{
    options.UseNpgsql(postgreSqlConnection!.ConnectionString);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.MapControllers();

app.Run();
