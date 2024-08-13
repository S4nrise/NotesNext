using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NotesApiNext.Interfaces;
using NotesApiNext.Mapping;
using NotesApiNext.Politics;
using NotesApiNext.Services;
using NotesApiNext.Settings;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Security.Claims;
using System.Text;

namespace NotesApiNext.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNotesNext(this IServiceCollection services, IConfiguration configuration)
        {
            var dateTimeProvider = new DateTimeProvider();
            services.AddSingleton<IDateTimeProvider>(dateTimeProvider);
            services.AddAutoMapper(config =>
            {
                config.AddProfile(new NoteMappingProfile(dateTimeProvider));
                config.AddProfile(new UserMappingProfile(dateTimeProvider));
            });

            services.AddControllers();
            services.AddRepositories();
            services.AddHttpContextAccessor();
            services.AddEndpointsApiExplorer();

            services.AddTransient<IPasswordHashProvider, Services.PasswordHashProvider>();
            Settings.PasswordHashProvider passwordHashProvider = new();
            configuration.Bind(nameof(Settings.PasswordHashProvider), passwordHashProvider);
            services.AddSingleton(Options.Create(passwordHashProvider));

            JwtSettings jwtSettings = new JwtSettings();
            configuration.Bind(nameof(JwtSettings), jwtSettings);
            services.AddSingleton(Options.Create(jwtSettings));
            services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddAuth(jwtSettings, dateTimeProvider);

            services.AddSwaggerGen();
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<INoteRepository, NoteRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSingleton<IJwtTokensRepository, JwtTokensRepository>();

            return services;
        }

        private static IServiceCollection AddAuth(this IServiceCollection services, JwtSettings jwtSettings, IDateTimeProvider dateTimeProvider)
        {
            services.AddAuthentication(
                defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var tokensRepository = context
                            .HttpContext
                            .RequestServices
                            .GetRequiredService<IJwtTokensRepository>();

                            var userId = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                            if (userId == null
                            || context.SecurityToken.ValidTo < dateTimeProvider.UtcNow
                            || !tokensRepository.Verify(
                                Guid.Parse(userId),
                                context.SecurityToken.UnsafeToString()))
                            {
                                context.Fail("Unauthorized");
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddScoped<IAuthorizationHandler, NoteOwnerRequirementHandler>();
            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder =
                new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
                defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();

                options.AddPolicy("NotesOwner", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AddRequirements(new NoteOwnerRequirement());
                });
            });

            return services;
        }
    }
}
