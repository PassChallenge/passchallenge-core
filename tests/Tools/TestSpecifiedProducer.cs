using KillDNS.CaptchaSolver.Core.Producer;
using KillDNS.CaptchaSolver.Core.Solver;

namespace KillDNS.CaptchaSolver.Core.Tests.Tools;

public class TestSpecifiedProducer : BaseProducerWithSpecifiedCaptchaAndSolutions
{
    public IAvailableCaptchaAndSolutionStorage AvailableCaptchaAndSolutionStorage { get; private set; } = null!;
    
    public override void SetAvailableCaptchaAndSolutionStorage(IAvailableCaptchaAndSolutionStorage availableCaptchaAndSolutionStorage)
    {
        AvailableCaptchaAndSolutionStorage = availableCaptchaAndSolutionStorage;
    }

    public override string GetDefaultHandlerName<TCaptcha, TSolution>()
    {
        throw new NotImplementedException();
    }

    public override IReadOnlyCollection<string> GetHandlerNames<TCaptcha, TSolution>()
    {
        throw new NotImplementedException();
    }

    public override Task<TSolution> ProduceAndWaitSolution<TCaptcha, TSolution>(TCaptcha captcha, string? handlerName = default,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}