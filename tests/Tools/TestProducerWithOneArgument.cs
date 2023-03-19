using PassChallenge.Core.Captcha;
using PassChallenge.Core.Producer;
using PassChallenge.Core.Solutions;
using PassChallenge.Core.Solver;

namespace PassChallenge.Core.Tests.Tools;

public class TestProducerWithOneArgument : IProducer
{
    // ReSharper disable once UnusedParameter.Local
    public TestProducerWithOneArgument(Action action)
    {
    }

    public void SetAvailableCaptchaAndSolutionStorage(
        IAvailableCaptchaAndSolutionStorage availableCaptchaAndSolutionStorage)
    {
        throw new NotImplementedException();
    }

    public Task<TSolution> ProduceAndWaitSolution<TCaptcha, TSolution>(TCaptcha captcha, string? handlerName = default,
        CancellationToken cancellationToken = default) where TCaptcha : ICaptcha where TSolution : ISolution
    {
        throw new NotImplementedException();
    }

    public bool CanProduce<TCaptcha, TSolution>(string? handlerName = default)
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        throw new NotImplementedException();
    }

    public string GetDefaultHandlerName<TCaptcha, TSolution>() where TCaptcha : ICaptcha where TSolution : ISolution
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<string> GetHandlerNames<TCaptcha, TSolution>()
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        throw new NotImplementedException();
    }
}