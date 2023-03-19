using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PassChallenge.Core.Challenges;
using PassChallenge.Core.Solutions;
using PassChallenge.Core.Solver;

namespace PassChallenge.Core.Producer;

public interface IProducer
{
    void SetAvailableChallengeAndSolutionStorage(IAvailableChallengeAndSolutionStorage availableChallengeAndSolutionStorage);

    Task<TSolution> ProduceAndWaitSolution<TChallenge, TSolution>(TChallenge challenge, string? handlerName = default,
        CancellationToken cancellationToken = default) where TChallenge : IChallenge
        where TSolution : ISolution;

    bool CanProduce<TChallenge, TSolution>(string? handlerName = default)
        where TChallenge : IChallenge where TSolution : ISolution;

    string GetDefaultHandlerName<TChallenge, TSolution>() where TChallenge : IChallenge where TSolution : ISolution;

    IReadOnlyCollection<string> GetHandlerNames<TChallenge, TSolution>()
        where TChallenge : IChallenge where TSolution : ISolution;
}