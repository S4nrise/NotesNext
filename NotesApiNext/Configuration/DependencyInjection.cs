namespace NotesApiNext.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNotesNext(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddControllers()
                
                ;

            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {

            return services;
        }
    }
}
