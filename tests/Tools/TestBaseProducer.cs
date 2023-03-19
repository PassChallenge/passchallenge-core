using PassChallenge.Core.Producer;

namespace PassChallenge.Core.Tests.Tools;

public class TestBaseProducer : BaseProducer
{
    public override string GetDefaultHandlerName<TChallenge, TSolution>()
    {
        throw new NotImplementedException();
    }

    public override IReadOnlyCollection<string> GetHandlerNames<TChallenge, TSolution>()
    {
        throw new NotImplementedException();
    }

    public override Task<TSolution> ProduceAndWaitSolution<TChallenge, TSolution>(TChallenge challenge,
        string? handlerName = default,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}