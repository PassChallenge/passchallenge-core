using System;
using System.Collections.Generic;
using System.Linq;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Handlers;
using KillDNS.CaptchaSolver.Core.Producer;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core.Solver;

public class CaptchaSolverSpecifiedBuilder<TProducer> : CaptchaSolverBuilder<TProducer>
    where TProducer : IProducerWithSpecifiedCaptchaAndSolutions
{
    private readonly Dictionary<Type, HashSet<Type>> _availableCaptchaAndSolutionTypes = new();

    private IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> AvailableCaptchaAndSolutionTypesOnlyDictionary =>
        _availableCaptchaAndSolutionTypes.ToDictionary(x => x.Key, x => (IReadOnlyCollection<Type>)x.Value);

    public CaptchaSolverSpecifiedBuilder<TProducer> AddSupportCaptchaAndSolution<TCaptcha, TSolution>()
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        return AddSupportCaptchaAndSolution(typeof(TCaptcha), typeof(TSolution));
    }

    public CaptchaSolverSpecifiedBuilder<TProducer> AddSupportCaptchaAndSolution(CaptchaHandlerDescriptor descriptor)
    {
        if (descriptor == null)
            throw new ArgumentNullException(nameof(descriptor));

        return AddSupportCaptchaAndSolution(descriptor.CaptchaType, descriptor.SolutionType);
    }

    private CaptchaSolverSpecifiedBuilder<TProducer> AddSupportCaptchaAndSolution(Type captchaType, Type solutionType)
    {
        if (!_availableCaptchaAndSolutionTypes.ContainsKey(captchaType))
        {
            _availableCaptchaAndSolutionTypes.Add(captchaType, new HashSet<Type>());
        }

        _availableCaptchaAndSolutionTypes[captchaType].Add(solutionType);

        return this;
    }

    protected override TProducer InternalBuild(IServiceProvider provider)
    {
        TProducer producer = base.InternalBuild(provider);
        producer.SetAvailableCaptchaAndSolutionTypes(AvailableCaptchaAndSolutionTypesOnlyDictionary);
        return producer;
    }
}