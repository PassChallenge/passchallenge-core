using System;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Producer;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core;

class CaptchaSolverFactory : ICaptchaSolverFactory
{
    private readonly IProducer _producer;

    public CaptchaSolverFactory(IProducer producer)
    {
        _producer = producer ?? throw new ArgumentNullException(nameof(producer));
    }

    public ICaptchaSolver<TCaptcha, TSolution> CreateSolver<TCaptcha, TSolution>()
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        if (CanCreateSolver<TCaptcha, TSolution>()) 
            return new CaptchaSolver<TCaptcha, TSolution>(_producer);

        throw new InvalidOperationException(
            $"Captcha '{typeof(TCaptcha)}' and solution '{typeof(TSolution)}' are not allowed for producer '{_producer.GetType()}'.");
    }

    public bool CanCreateSolver<TCaptcha, TSolution>() where TCaptcha : ICaptcha where TSolution : ISolution
    {
        return _producer is not IProducerWithSpecifiedCaptchaAndSolutions specifiedProducer ||
               specifiedProducer.CanProduce<TCaptcha, TSolution>();
    }
}