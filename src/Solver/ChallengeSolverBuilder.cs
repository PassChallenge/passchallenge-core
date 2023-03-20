using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using PassChallenge.Core.Producer;

namespace PassChallenge.Core.Solver;

public class ChallengeSolverBuilder<TProducer> where TProducer : IProducer
{
    private readonly List<Action<TProducer>> _configureProducerActions = new();

    public ChallengeSolverBuilder()
    {
        if (typeof(TProducer).IsInterface)
            throw new ArgumentException("The producer must be a class.");
    }

    public AvailableChallengeAndSolutionStorageBuilder AvailableChallengeAndSolutionStorageBuilder { get; } = new();

    public TProducer Build(IServiceProvider provider)
    {
        if (provider == null)
            throw new ArgumentNullException(nameof(provider));

        TProducer producer = InternalBuild(provider);
        producer = PostConfigureProducer(producer);
        return producer;
    }

    internal ChallengeSolverBuilder<TProducer> AddConfigureProducerAction(Action<TProducer> action)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));

        _configureProducerActions.Add(action);
        return this;
    }

    protected virtual TProducer InternalBuild(IServiceProvider provider)
    {
        var parameters = typeof(TProducer).GetConstructors(BindingFlags.Instance | BindingFlags.Public)[0]
            .GetParameters()
            .Select(x => provider.GetRequiredService(x.ParameterType)).ToArray();

        TProducer producer = (TProducer)Activator.CreateInstance(typeof(TProducer), parameters);

        producer.SetAvailableChallengeAndSolutionStorage(AvailableChallengeAndSolutionStorageBuilder.Build());

        return producer;
    }

    private TProducer PostConfigureProducer(TProducer producer)
    {
        foreach (var configureProducerAction in _configureProducerActions)
        {
            configureProducerAction?.Invoke(producer);
        }

        return producer;
    }
}