using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Handlers;
using KillDNS.CaptchaSolver.Core.Producer;
using KillDNS.CaptchaSolver.Core.Solutions;
using Microsoft.Extensions.DependencyInjection;

namespace KillDNS.CaptchaSolver.Core.Solver;

public class CaptchaSolverBuilder<TProducer> where TProducer : IProducer
{
    private readonly Dictionary<Type, HashSet<Type>> _availableCaptchaAndSolutionTypes = new();

    private readonly List<Action<TProducer>?> _configureProducerActions = new();

    private IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> AvailableCaptchaAndSolutionTypesOnlyDictionary =>
        _availableCaptchaAndSolutionTypes.ToDictionary(x => x.Key, x => (IReadOnlyCollection<Type>)x.Value);

    public CaptchaSolverBuilder<TProducer> AddSupportCaptchaAndSolution<TCaptcha, TSolution>()
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        return AddSupportCaptchaAndSolution(typeof(TCaptcha), typeof(TSolution));
    }

    public CaptchaSolverBuilder<TProducer> AddSupportCaptchaAndSolution(CaptchaHandlerDescriptor descriptor)
    {
        if (descriptor == null)
            throw new ArgumentNullException(nameof(descriptor));

        return AddSupportCaptchaAndSolution(descriptor.CaptchaType, descriptor.SolutionType);
    }

    private CaptchaSolverBuilder<TProducer> AddSupportCaptchaAndSolution(Type captchaType, Type solutionType)
    {
        if (!_availableCaptchaAndSolutionTypes.ContainsKey(captchaType))
        {
            _availableCaptchaAndSolutionTypes.Add(captchaType, new HashSet<Type>());
        }

        _availableCaptchaAndSolutionTypes[captchaType].Add(solutionType);

        return this;
    }

    public TProducer Build(IServiceProvider provider)
    {
        if (provider == null)
            throw new ArgumentNullException(nameof(provider));

        var parameters = typeof(TProducer).GetConstructors(BindingFlags.Instance | BindingFlags.Public)[0]
            .GetParameters()
            .Select(x => provider.GetRequiredService(x.ParameterType)).ToArray();

        TProducer producer = (TProducer)Activator.CreateInstance(typeof(TProducer), parameters);

        if (producer is IProducerWithSpecifiedCaptchaAndSolutions specifiedProducer)
        {
            specifiedProducer.SetAvailableCaptchaAndSolutionTypes(AvailableCaptchaAndSolutionTypesOnlyDictionary);
        }

        foreach (var configureProducerAction in _configureProducerActions)
        {
            configureProducerAction?.Invoke(producer);
        }

        return producer;
    }

    internal CaptchaSolverBuilder<TProducer> AddConfigureProducerAction(Action<TProducer> action)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));

        _configureProducerActions.Add(action);
        return this;
    }
}