using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Producer;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core.Tests.Tools;

public class TestProducerWithOneArgument : IProducer
{
    public TestProducerWithOneArgument(Action action)
    {
    }

    public Task<TSolution> ProduceAndWaitSolution<TCaptcha, TSolution>(TCaptcha captcha,
        CancellationToken cancellationToken = default) where TCaptcha : ICaptcha where TSolution : ISolution
    {
        throw new NotImplementedException();
    }
}