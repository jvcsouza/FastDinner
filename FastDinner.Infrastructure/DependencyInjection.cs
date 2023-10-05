﻿using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Infrastructure.Persistence;
using FastDinner.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using FastDinner.Infrastructure.Auth;
using FastDinner.Application.Common.Interfaces.Auth;
using FastDinner.Infrastructure.Services;
using FastDinner.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FastDinner.Infrastructure.Store;
using Microsoft.Data.SqlClient;
using FastDinner.Application.Common;

namespace FastDinner.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            return services
                .AddConfigurations(configuration)
                .AddAuthorization(configuration)
                .AddServices(configuration)
                .AddRepositories();
        }

        private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<ITokenGenerator, JwtTokenGenerator>();
            services.AddSingleton<ICacheProvider, SimpleMemoryCache>();
            services.AddScoped<AppScope>();

            services.AddSingleton<IAppSettings>(provider =>
            {
                var config = configuration.GetSection(TableConfig.SectionName).Get<TableConfig>();
                var cache = provider.GetService<ICacheProvider>();
                var appSettings = new AppSettings(new AzureTableStore(Environment.GetEnvironmentVariable("TABLECONFIGSTR"), config.TableName), cache);
                return appSettings;

            });

            return services;
        }

        private static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseConfig>(configuration.GetSection(DatabaseConfig.SectionName));
            services.Configure<JwtConfig>(configuration.GetSection(JwtConfig.SectionName));
            services.Configure<TableConfig>(configuration.GetSection(TableConfig.SectionName));

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            //services.AddDbContext<DinnerContext>((IServiceProvider provider, DbContextOptionsBuilder options)
            //    => options.UseSqlServer(provider.GetService<IOptions<DatabaseConfig>>().Value.ConnectionString));

            services.AddDbContext<DinnerContext>(options
                =>
            {
                var settings = AppScope.Tenant;

                var passBytes = Convert.FromBase64String(
                        settings.Password ?? Environment.GetEnvironmentVariable("DBDATAPASS")!);

                var connString = new SqlConnectionStringBuilder
                {
                    DataSource = settings.DataSource,
                    InitialCatalog = settings.Catalog,
                    UserID = settings.User,
                    Password = Encoding.UTF8.GetString(passBytes),
                    MultipleActiveResultSets = true,
                    ConnectTimeout = 60
                };

                options.UseSqlServer(connString.ConnectionString);
            });

            //var serviceProvider = services.BuildServiceProvider();
            //var dinnerContext = serviceProvider.GetService<DinnerContext>();
            //dinnerContext.Database.Migrate();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IRestaurantRepository, RestaurantRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ITableRepository, TableRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();

            return services;
        }

        private static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration)
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
                options.TokenValidationParameters = new TokenValidationParameters
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