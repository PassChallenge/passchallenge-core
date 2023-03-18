using KillDNS.CaptchaSolver.Core.Handlers;

namespace KillDNS.CaptchaSolver.Core.Tests.Tools;

public class TestCaptchaHandler<TCaptcha, TSolution> : ICaptchaHandler<TCaptcha, TSolution>
    where TCaptcha : Core.Captcha.ICaptcha where TSolution : Core.Solutions.ISolution
{
    public Task<TSolution> Handle(TCaptcha captcha, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}