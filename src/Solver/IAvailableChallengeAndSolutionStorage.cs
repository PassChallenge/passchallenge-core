using PassChallenge.Core.Challenges;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Solver;

public interface IAvailableChallengeAndSolutionStorage
{
    bool IsAvailable<TChallenge, TSolution>(string? handlerName = default)
        where TChallenge : IChallenge where TSolution : ISolution;
}