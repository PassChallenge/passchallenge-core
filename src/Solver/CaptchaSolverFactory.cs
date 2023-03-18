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

        handlerName ??= _producer.GetDefaultHandlerName<TCaptcha, TSolution>();

        return new CaptchaSolver<TCaptcha, TSolution>(_producer, handlerName);
    }

    public bool CanCreateSolver<TCaptcha, TSolution>(string? handlerName = default)
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        return _producer.CanProduce<TCaptcha, TSolution>(handlerName);
    }

    public IReadOnlyCollection<string> GetHandlerNames<TCaptcha, TSolution>()
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        return _producer.GetHandlerNames<TCaptcha, TSolution>();
    }
}