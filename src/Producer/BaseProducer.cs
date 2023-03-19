using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PassChallenge.Core.Challenges;
using PassChallenge.Core.Solutions;
using PassChallenge.Core.Solver;

namespace PassChallenge.Core.Producer;

public abstract class BaseProducer : IProducer
{
    private IAvailableChallengeAndSolutionStorage? _availableChallengeAndSolutionStorage;

    public virtual void SetAvailableChallengeAndSolutionStorage(
        IAvailableChallengeAndSolutionStorage availableChallengeAndSolutionStorage)
    {
        _availableChallengeAndSolutionStorage = availableChallengeAndSolutionStorage ??
                                              throw new ArgumentNullException(
                                                  nameof(availableChallengeAndSolutionStorage));
    }

    public virtual bool CanProduce<TChallenge, TSolution>(string? handlerName = default)
        where TChallenge : IChallenge where TSolution : ISolution
    {
        return _availableChallengeAndSolutionStorage?.IsAvailable<TChallenge, TSolution>(handlerName) ?? false;
    }

    public abstract string GetDefaultHandlerName<TChallenge, TSolution>()
        where TChallenge : IChallenge where TSolution : ISolution;

    public abstract IReadOnlyCollection<string> GetHandlerNames<TChallenge, TSolution>()
        where TChallenge : IChallenge where TSolution : ISolution;

    public abstract Task<TSolution> ProduceAndWaitSolution<TChallenge, TSolution>(TChallenge challenge,
        string? handlerName = default, CancellationToken cancellationToken = default)
        where TChallenge : IChallenge where TSolution : ISolution;
}