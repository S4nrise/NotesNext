using Microsoft.EntityFrameworkCore;
using NotesApiNext.Configuration;
using NotesApiNext.Database;
using NotesApiNext.Interfaces;
using NotesApiNext.Mapping;
using NotesApiNext.Middlewares;
using NotesApiNext.Services;
using NotesApiNext.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddControllersWithViews();
builder.Services.AddNotesNext(builder.Configuration);
var dateTimeProvider = new DateTimeProvider();
builder.Services.AddSingleton<IDateTimeProvider>(dateTimeProvider);
builder.Services.AddAutoMapper(config =>
     {
         config.AddProfile(new NoteMappingProfile(dateTimeProvider));
         config.AddProfile(new UserMappingProfile(dateTimeProvider));
     });

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
