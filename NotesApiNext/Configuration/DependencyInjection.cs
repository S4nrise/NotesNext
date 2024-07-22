using Microsoft.Extensions.Options;
using NotesApiNext.Interfaces;
using NotesApiNext.Services;
using System.Reflection;

namespace NotesApiNext.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNotesNext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddRepositories();
            services.AddTransient<IPasswordHashProvider, PasswordHashProvider>();
            //services.AddOptions<Settings.PasswordHashProvider>("PasswordHashProvider");
            Settings.PasswordHashProvider passwordHashProvider = new ();
            configuration.Bind(nameof(Settings.PasswordHashProvider), passwordHashProvider);
            services.AddSingleton(Options.Create(passwordHashProvider));
            //services.AddAutoMapper(cfg => cfg.AddProfile(/*new NoteMappingProfile(dateTimeProvider)*/));
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            return services;
        }

        
    }
}
