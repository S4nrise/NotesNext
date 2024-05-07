using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NotesApiNext.Database;
using NotesApiNext.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddDbContext<NotesNextDbContext>(options => options.UseNpgsql(
//    builder.Configuration.GetConnectionString("PostgreSqlConnection")));
var con = builder.Configuration.GetConnectionString("PostgreSqlConnection");
builder.Services.AddDbContext<NotesNextDbContext>(options =>
{
    options.UseNpgsql(con);
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

app.MapControllers();

app.Run();
