using KillDNS.CaptchaSolver.Core.Producer;

namespace KillDNS.CaptchaSolver.Core.Tests.Tools;

public class TestSpecifiedProducer : ProducerWithSpecifiedCaptchaAndSolutions
{
    public override Task<TSolution> ProduceAndWaitSolution<TCaptcha, TSolution>(TCaptcha captcha,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}