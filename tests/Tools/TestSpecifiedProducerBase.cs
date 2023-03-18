using KillDNS.CaptchaSolver.Core.Producer;

namespace KillDNS.CaptchaSolver.Core.Tests.Tools;

public class TestSpecifiedProducerBase : BaseProducerWithSpecifiedCaptchaAndSolutions
{
    public override string GetDefaultHandlerName<TCaptcha, TSolution>()
    {
        throw new NotImplementedException();
    }

    public override IReadOnlyCollection<string> GetHandlerNames<TCaptcha, TSolution>()
    {
        throw new NotImplementedException();
    }

    public override Task<TSolution> ProduceAndWaitSolution<TCaptcha, TSolution>(TCaptcha captcha,
        string? handlerName = default,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}