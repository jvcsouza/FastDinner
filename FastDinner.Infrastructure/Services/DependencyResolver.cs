using Microsoft.Extensions.DependencyInjection;
using StructureMap;

namespace FastDinner.Infrastructure.Services;

public static class DependencyResolver
{
    private static Container _container;
    private static Container Container => _container ??= new Container();

    public static IServiceProvider Collection { get; set; }

    public static Container Initialize(IServiceCollection services)
    {
        Container.Configure(x => x.Populate(services));
        return _container;
    }

    public static T Resolve<T>() => Container.GetInstance<T>();
    public static T Get<T>() => Collection.GetService<T>();
}