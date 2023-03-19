using PassChallenge.Core.Producer;

namespace PassChallenge.Core.Tests.Tools;

public class TestBaseProducer : BaseProducer
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