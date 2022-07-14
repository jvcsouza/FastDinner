using System;
using FastDinner.Application.Common.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FastDinner.Api.Services;

public class Container : IContainer
{
    private readonly IServiceProvider _provider;

    public Container(IServiceProvider provider)
    {
        _provider = provider;
    }

    public T? Resolve<T>()
    {
        return _provider.GetService<T>();
    }
}

public class DependencyResolver : IDependencyResolver
{
    private IServiceScope _serviceScope;

    public DependencyResolver(IServiceScope serviceScope)
    {
        _serviceScope = serviceScope;
    }

    public IContainer Instance => new Container(_serviceScope.ServiceProvider);
}