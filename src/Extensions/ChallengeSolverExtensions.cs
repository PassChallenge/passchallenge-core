using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using PassChallenge.Core.DependencyInjection;
using PassChallenge.Core.Producer;
using PassChallenge.Core.Solver;

namespace PassChallenge.Core.Extensions;

public static class ChallengeSolverExtensions
{
    public static IServiceCollection AddChallengeSolver<TProducer>(this IServiceCollection serviceCollection,
        string? solverName = default, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TProducer : IProducer
    {
        if (typeof(TProducer).IsInterface)
            throw new ArgumentException("The producer must be a class.");

        if (serviceCollection == null)
            throw new ArgumentNullException(nameof(serviceCollection));

        return AddChallengeSolver<TProducer>(serviceCollection, _ => { }, solverName, lifetime);
    }

    public static IServiceCollection AddChallengeSolver<TProducer>(this IServiceCollection serviceCollection,
        Action<ChallengeSolverBuilder<TProducer>> configure, string? solverName = default,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TProducer : IProducer
    {
        if (typeof(TProducer).IsInterface)
            throw new ArgumentException("The producer must be a class.");

        if (serviceCollection == null)
            throw new ArgumentNullException(nameof(serviceCollection));

        if (configure == null)
            throw new ArgumentNullException(nameof(configure));

        ChallengeSolverBuilder<TProducer> builder = new();
        configure.Invoke(builder);

        AddFactoryToServiceCollection(serviceCollection, builder, lifetime, solverName);

        return serviceCollection;
    }

    private static void AddFactoryToServiceCollection<TProducer>(IServiceCollection serviceCollection,
        ChallengeSolverBuilder<TProducer> builder, ServiceLifetime lifetime, string? solverName = default)
        where TProducer : IProducer
    {
        solverName ??= GetNewSolverIdentifier<TProducer>(serviceCollection);

        if (SolverFactoryIsContainsInContainer(serviceCollection, solverName))
            throw new InvalidOperationException(
                $"Solver '{solverName}' already added.");

        serviceCollection.Add(new SolverFactoryServiceDescriptor(typeof(IChallengeSolverFactory), provider =>
            new ChallengeSolverFactory(builder.Build(provider), solverName), lifetime, solverName));
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