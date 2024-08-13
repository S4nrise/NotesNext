using Microsoft.EntityFrameworkCore;
using NotesApiNext.Configuration;
using NotesApiNext.Database;
using NotesApiNext.Interfaces;
using NotesApiNext.Middlewares;
using NotesApiNext.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNotesNext(builder.Configuration);

var postgreSqlConnection = builder.Configuration
    .GetRequiredSection(nameof(PostgreSQLConnection))
    .Get<PostgreSQLConnection>();
builder.Services.AddDbContext<NotesNextDbContext>(options =>
{
    options.UseNpgsql(postgreSqlConnection!.ConnectionString);
});
builder.Services.AddScoped<INotesNextDbContext>(provider => provider.GetRequiredService<NotesNextDbContext>());
var app = builder.Build();

app.UseException();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.MapControllers();

app.Run();
