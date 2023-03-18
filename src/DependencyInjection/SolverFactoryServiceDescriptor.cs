using System;
using Microsoft.Extensions.DependencyInjection;

namespace KillDNS.CaptchaSolver.Core.DependencyInjection;

internal class SolverFactoryServiceDescriptor : ServiceDescriptor
{
    public SolverFactoryServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime lifetime,
        string solverName) : base(serviceType, implementationType, lifetime)
    {
        SolverName = solverName;
    }

    public SolverFactoryServiceDescriptor(Type serviceType, object instance, string solverName) : base(serviceType,
        instance)
    {
        SolverName = solverName;
    }

    public SolverFactoryServiceDescriptor(Type serviceType, Func<IServiceProvider, object> factory,
        ServiceLifetime lifetime, string solverName) : base(serviceType, factory, lifetime)
    {
        SolverName = solverName;
    }

    public string SolverName { get; }
}