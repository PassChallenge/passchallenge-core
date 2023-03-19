using PassChallenge.Core.Handlers;

namespace PassChallenge.Core.Tests.Tools;

public class TestCaptchaHandler<TCaptcha, TSolution> : ICaptchaHandler<TCaptcha, TSolution>
    where TCaptcha : PassChallenge.Core.Captcha.ICaptcha where TSolution : PassChallenge.Core.Solutions.ISolution
{
    public Task<TSolution> Handle(TCaptcha captcha, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}