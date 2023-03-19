using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PassChallenge.Core.Captcha;
using PassChallenge.Core.Solutions;
using PassChallenge.Core.Solver;

namespace PassChallenge.Core.Producer;

public interface IProducer
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