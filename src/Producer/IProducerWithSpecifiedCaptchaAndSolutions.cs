using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Solutions;
using KillDNS.CaptchaSolver.Core.Solver;

namespace KillDNS.CaptchaSolver.Core.Producer;

public interface IProducerWithSpecifiedCaptchaAndSolutions : IProducer
{
    void SetAvailableCaptchaAndSolutionStorage(IAvailableCaptchaAndSolutionStorage availableCaptchaAndSolutionStorage);

    Task<TSolution> ProduceAndWaitSolution<TCaptcha, TSolution>(TCaptcha captcha, string? handlerName = default,
        CancellationToken cancellationToken = default) where TCaptcha : ICaptcha
        where TSolution : ISolution;

    bool CanProduce<TCaptcha, TSolution>(string? handlerName = default)
        where TCaptcha : ICaptcha where TSolution : ISolution;

    string GetDefaultHandlerName<TCaptcha, TSolution>() where TCaptcha : ICaptcha where TSolution : ISolution;

    IReadOnlyCollection<string> GetHandlerNames<TCaptcha, TSolution>()
        where TCaptcha : ICaptcha where TSolution : ISolution;
}