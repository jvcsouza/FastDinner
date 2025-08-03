using FastDinner.Application.Common;
using FastDinner.Application.Common.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace FastDinner.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddMediatR(typeof(DependencyInjection).Assembly);
            services.AddScoped<IMultiTenancyService, MultiTenancyService>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

            return services;
        }
    }
}