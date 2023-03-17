using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KillDNS.CaptchaSolver.Core.Producer;
using Microsoft.Extensions.DependencyInjection;

namespace KillDNS.CaptchaSolver.Core.Solver;

public class CaptchaSolverBuilder<TProducer> where TProducer : class, IProducer
{
    private readonly List<Action<TProducer>> _configureProducerActions = new();

    public TProducer Build(IServiceProvider provider)
    {
        if (provider == null)
            throw new ArgumentNullException(nameof(provider));

        TProducer producer = InternalBuild(provider);
        producer = PostConfigureProducer(producer);
        return producer;
    }

    internal CaptchaSolverBuilder<TProducer> AddConfigureProducerAction(Action<TProducer> action)
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

        return (TProducer)Activator.CreateInstance(typeof(TProducer), parameters);
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