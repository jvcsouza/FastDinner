namespace FastDinner.Application.Common.Interfaces.Services;

public interface IContainer
{
    T Resolve<T>();
}

public interface IDependencyResolver
{
    IContainer Instance { get; }
}