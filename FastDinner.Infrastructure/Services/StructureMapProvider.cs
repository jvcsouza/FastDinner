using Microsoft.Extensions.DependencyInjection;
using StructureMap;

namespace FastDinner.Infrastructure.Services
{
    public class StructureMapProvider : IServiceProviderFactory<Container>
    {
        public Container CreateBuilder(IServiceCollection services)
            => DependencyResolver.Initialize(services);

        public IServiceProvider CreateServiceProvider(Container containerBuilder)
            => containerBuilder.GetInstance<IServiceProvider>();
    }
}
