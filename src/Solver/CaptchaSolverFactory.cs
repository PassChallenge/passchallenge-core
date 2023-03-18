using System;
using System.Collections.Generic;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Producer;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core.Solver;

internal class CaptchaSolverFactory : ICaptchaSolverFactory
{
    private readonly IProducer _producer;

    public CaptchaSolverFactory(IProducer producer, string solverName)
    {
        _producer = producer ?? throw new ArgumentNullException(nameof(producer));

        if (string.IsNullOrWhiteSpace(solverName))
            throw new ArgumentException("Is null or whitespace.", nameof(solverName));

        SolverName = solverName;
    }

    public string SolverName { get; }

    public ICaptchaSolver<TCaptcha, TSolution> CreateSolver<TCaptcha, TSolution>(string? handlerName = default)
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        if (CanCreateSolver<TCaptcha, TSolution>(handlerName) == false)
            throw new InvalidOperationException(
                $"Can't create solver for '{_producer.GetType()}' producer. Captcha '{typeof(TCaptcha)}', Solution '{typeof(TSolution)}', Handler name {(handlerName == default ? "default" : $"'{handlerName}'")}.");

        handlerName = handlerName == default &&
                      _producer is IProducerWithSpecifiedCaptchaAndSolutions specifiedProducer
            ? specifiedProducer.GetDefaultHandlerName<TCaptcha, TSolution>()
            : handlerName;

        return new CaptchaSolver<TCaptcha, TSolution>(_producer, handlerName);
    }

    public bool CanCreateSolver<TCaptcha, TSolution>(string? handlerName = default)
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        return _producer is not IProducerWithSpecifiedCaptchaAndSolutions specifiedProducer ||
               specifiedProducer.CanProduce<TCaptcha, TSolution>(handlerName);
    }

    public IReadOnlyCollection<string> GetHandlerNames<TCaptcha, TSolution>()
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        if (_producer is not IProducerWithSpecifiedCaptchaAndSolutions specifiedProducer)
            throw new InvalidOperationException(
                $"This method only supports producers implementing the interface {typeof(IProducerWithSpecifiedCaptchaAndSolutions)}.");

        return specifiedProducer.GetHandlerNames<TCaptcha, TSolution>();
    }
}