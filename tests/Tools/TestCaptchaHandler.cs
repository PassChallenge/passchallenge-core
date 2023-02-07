using KillDNS.CaptchaSolver.Core.Handlers;

namespace KillDNS.CaptchaSolver.Core.Tests.Tools;

public class TestCaptchaHandler : ICaptchaHandler<TestCaptcha, TestSolution>
{
    public Task<TestSolution> Handle(TestCaptcha captcha, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}