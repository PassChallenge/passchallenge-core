using System;
using System.Linq;
using KillDNS.CaptchaSolver.Core.DependencyInjection;
using KillDNS.CaptchaSolver.Core.Producer;
using KillDNS.CaptchaSolver.Core.Solver;
using Microsoft.Extensions.DependencyInjection;

namespace KillDNS.CaptchaSolver.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCaptchaSolver<TProducer>(this IServiceCollection serviceCollection,
        string? solverName = default, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TProducer : IProducer
    {
        if (typeof(TProducer).IsInterface)
            throw new ArgumentException("The producer must be a class.");

        if (serviceCollection == null)
            throw new ArgumentNullException(nameof(serviceCollection));

        return AddCaptchaSolver<TProducer>(serviceCollection, _ => { }, solverName, lifetime);
    }

    public static IServiceCollection AddCaptchaSolver<TProducer>(this IServiceCollection serviceCollection,
        Action<CaptchaSolverBuilder<TProducer>> configure, string? solverName = default,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TProducer : IProducer
    {
        if (typeof(TProducer).IsInterface)
            throw new ArgumentException("The producer must be a class.");

        if (serviceCollection == null)
            throw new ArgumentNullException(nameof(serviceCollection));

        if (configure == null)
            throw new ArgumentNullException(nameof(configure));

        CaptchaSolverBuilder<TProducer> builder = new();
        configure.Invoke(builder);

        AddFactoryToServiceCollection(serviceCollection, builder, lifetime, solverName);

        return serviceCollection;
    }

    private static void AddFactoryToServiceCollection<TProducer>(IServiceCollection serviceCollection,
        CaptchaSolverBuilder<TProducer> builder, ServiceLifetime lifetime, string? solverName = default)
        where TProducer : IProducer
    {
        solverName ??= GetNewSolverIdentifier<TProducer>(serviceCollection);

        if (SolverFactoryIsContainsInContainer(serviceCollection, solverName))
            throw new InvalidOperationException(
                $"Solver '{solverName}' already added.");

        serviceCollection.Add(new SolverFactoryServiceDescriptor(typeof(ICaptchaSolverFactory), provider =>
            new CaptchaSolverFactory(builder.Build(provider), solverName), lifetime, solverName));
    }

    private static string GetNewSolverIdentifier<TProducer>(IServiceCollection serviceCollection)
        where TProducer : IProducer
    {
        string baseName = $"{typeof(TProducer).Name}-".ToLower();
        for (int i = 0;; i++)
        {
            string finalName = baseName + i;

            if (SolverFactoryIsContainsInContainer(serviceCollection, finalName) == false)
                return finalName;
        }
    }

    private static bool SolverFactoryIsContainsInContainer(IServiceCollection serviceCollection, string solverName)
    {
        return serviceCollection.OfType<SolverFactoryServiceDescriptor>()
            .Any(x => x.SolverName == solverName);
    }
}