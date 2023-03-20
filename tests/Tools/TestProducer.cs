using PassChallenge.Core.Challenges;
using PassChallenge.Core.Producer;
using PassChallenge.Core.Solutions;
using PassChallenge.Core.Solver;

namespace PassChallenge.Core.Tests.Tools;

public class TestProducer : IProducer
{
    public IAvailableChallengeAndSolutionStorage AvailableChallengeAndSolutionStorage { get; private set; } = null!;

    public void SetAvailableChallengeAndSolutionStorage(
        IAvailableChallengeAndSolutionStorage availableChallengeAndSolutionStorage)
    {
        AvailableChallengeAndSolutionStorage = availableChallengeAndSolutionStorage;
    }

    public Task<TSolution> ProduceAndWaitSolution<TChallenge, TSolution>(TChallenge challenge, string? handlerName = default,
        CancellationToken cancellationToken = default) where TChallenge : IChallenge where TSolution : ISolution
    {
        throw new NotImplementedException();
    }

    public bool CanProduce<TChallenge, TSolution>(string? handlerName = default)
        where TChallenge : IChallenge where TSolution : ISolution
    {
        return AvailableChallengeAndSolutionStorage.IsAvailable<TChallenge, TSolution>(handlerName);
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