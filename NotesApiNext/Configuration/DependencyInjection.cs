using System.Reflection;

namespace NotesApiNext.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNotesNext(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddControllers()
                
                ;
            //services.AddAutoMapper(cfg => cfg.AddProfile(/*new NoteMappingProfile(dateTimeProvider)*/));
            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {

            return services;
        }

    }
}
