using PassChallenge.Core.Challenges;
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

    public void SetAvailableChallengeAndSolutionStorage(
        IAvailableChallengeAndSolutionStorage availableChallengeAndSolutionStorage)
    {
        throw new NotImplementedException();
    }

    public Task<TSolution> ProduceAndWaitSolution<TChallenge, TSolution>(TChallenge challenge, string? handlerName = default,
        CancellationToken cancellationToken = default) where TChallenge : IChallenge where TSolution : ISolution
    {
        throw new NotImplementedException();
    }

    public bool CanProduce<TChallenge, TSolution>(string? handlerName = default)
        where TChallenge : IChallenge where TSolution : ISolution
    {
        throw new NotImplementedException();
    }

    public string GetDefaultHandlerName<TChallenge, TSolution>() where TChallenge : IChallenge where TSolution : ISolution
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<string> GetHandlerNames<TChallenge, TSolution>()
        where TChallenge : IChallenge where TSolution : ISolution
    {
        throw new NotImplementedException();
    }
}