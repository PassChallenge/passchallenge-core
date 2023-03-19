using PassChallenge.Core.Handlers;

namespace PassChallenge.Core.Tests.Tools;

public class TestCaptchaHandlerWithOneArgument : ICaptchaHandler<TestCaptcha, TestSolution>
{
    public TestCaptchaHandlerWithOneArgument(object obj)
    {
    }

    public Task<TestSolution> Handle(TestCaptcha captcha, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}