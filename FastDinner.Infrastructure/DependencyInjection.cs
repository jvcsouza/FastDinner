using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Infrastructure.Persistence;
using FastDinner.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using FastDinner.Infrastructure.Auth;
using FastDinner.Application.Common.Interfaces.Auth;
using FastDinner.Infrastructure.Services;
using FastDinner.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FastDinner.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddConfigurations(configuration);
            services.AddAuthorization(configuration);
            services.AddRepositories();
            services.AddServices();

            return services;
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<ITokenGenerator, JwtTokenGenerator>();
        }

        private static void AddConfigurations(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<DatabaseConfig>(configuration.GetSection(DatabaseConfig.SectionName));
            services.Configure<JwtConfig>(configuration.GetSection(JwtConfig.SectionName));
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddDbContext<DinnerContext>((IServiceProvider provider, DbContextOptionsBuilder options)
                => options.UseSqlServer(provider.GetService<IOptions<DatabaseConfig>>().Value.ConnectionString));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IRestaurantRepository, RestaurantRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            return services;
        }

        private static IServiceCollection AddAuthorization(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtConfig = configuration.GetSection(JwtConfig.SectionName).Get<JwtConfig>();

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret))
                };
                // options.Authority = configuration.GetValue<string>("Auth0:Authority");
                options.Audience = jwtConfig.Audience;
            });
            return services;
        }
    }
}