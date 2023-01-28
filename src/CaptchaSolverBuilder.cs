using System;
using System.Collections.Generic;
using System.Linq;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Producer;
using KillDNS.CaptchaSolver.Core.Solutions;
using Microsoft.Extensions.DependencyInjection;

namespace KillDNS.CaptchaSolver.Core;

public class CaptchaSolverBuilder<TProducer> where TProducer : IProducer
{
    private readonly Dictionary<Type, HashSet<Type>> _availableCaptchaAndSolutionTypes = new();

    public CaptchaSolverBuilder(IServiceCollection serviceCollection)
    {
        ServiceCollection = serviceCollection;
    }

    internal IServiceCollection ServiceCollection { get; }
    
    internal List<Action<TProducer>?> ConfigureProducerActions { get; } = new();

    internal IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> AvailableCaptchaAndSolutionTypes =>
        _availableCaptchaAndSolutionTypes.ToDictionary(x => x.Key, x => (IReadOnlyCollection<Type>)x.Value);


    public CaptchaSolverBuilder<TProducer> AddSupportCaptchaAndSolution<TCaptcha, TSolution>()
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        if (!_availableCaptchaAndSolutionTypes.ContainsKey(typeof(TCaptcha)))
        {
            _availableCaptchaAndSolutionTypes.Add(typeof(TCaptcha), new HashSet<Type>());
        }

        _availableCaptchaAndSolutionTypes[typeof(TCaptcha)].Add(typeof(TSolution));

        return this;
    }

    internal CaptchaSolverBuilder<TProducer> AddConfigureProducerAction(Action<TProducer> action)
    {
        ConfigureProducerActions.Add(action);
        return this;
    }
}