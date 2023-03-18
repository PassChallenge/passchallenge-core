using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Solutions;
using KillDNS.CaptchaSolver.Core.Solver;

namespace KillDNS.CaptchaSolver.Core.Producer;

public abstract class BaseProducer : IProducer
{
    private IAvailableCaptchaAndSolutionStorage? _availableCaptchaAndSolutionStorage;

    public virtual void SetAvailableCaptchaAndSolutionStorage(
        IAvailableCaptchaAndSolutionStorage availableCaptchaAndSolutionStorage)
    {
        _availableCaptchaAndSolutionStorage = availableCaptchaAndSolutionStorage ??
                                              throw new ArgumentNullException(
                                                  nameof(availableCaptchaAndSolutionStorage));
    }

    public virtual bool CanProduce<TCaptcha, TSolution>(string? handlerName = default)
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        return _availableCaptchaAndSolutionStorage?.IsAvailable<TCaptcha, TSolution>(handlerName) ?? false;
    }

    public abstract string GetDefaultHandlerName<TCaptcha, TSolution>()
        where TCaptcha : ICaptcha where TSolution : ISolution;

    public abstract IReadOnlyCollection<string> GetHandlerNames<TCaptcha, TSolution>()
        where TCaptcha : ICaptcha where TSolution : ISolution;

    public abstract Task<TSolution> ProduceAndWaitSolution<TCaptcha, TSolution>(TCaptcha captcha,
        string? handlerName = default, CancellationToken cancellationToken = default)
        where TCaptcha : ICaptcha where TSolution : ISolution;
}