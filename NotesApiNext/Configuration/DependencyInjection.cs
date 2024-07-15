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
