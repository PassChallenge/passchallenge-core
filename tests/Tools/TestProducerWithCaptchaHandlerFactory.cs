using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Handlers;
using KillDNS.CaptchaSolver.Core.Producer;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core.Tests.Tools;

public class TestProducerWithCaptchaHandlerFactory : IProducerWithCaptchaHandlerFactory
{
    public Task<TSolution> ProduceAndWaitSolution<TCaptcha, TSolution>(TCaptcha captcha,
        CancellationToken cancellationToken = default) where TCaptcha : ICaptcha where TSolution : ISolution
    {
        throw new NotImplementedException();
    }

    public void SetCaptchaHandlerFactory(ICaptchaHandlerFactory captchaHandlerFactory)
    {
    }
}