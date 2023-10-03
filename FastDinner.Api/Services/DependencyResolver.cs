using System;
using Microsoft.Extensions.DependencyInjection;

namespace FastDinner.Api.Services;

public static class DependencyResolver
{
    private static IServiceProvider _provider;

    public static void InitializeIoc(IServiceProvider provider)
    {
        _provider = provider;
    }

    public static T Resolve<T>()
    {
        return _provider.GetService<T>();
    }
}