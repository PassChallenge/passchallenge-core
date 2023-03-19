using System;
using System.Collections.Generic;
using PassChallenge.Core.Challenges;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Solver;

public class AvailableChallengeAndSolutionStorage : IAvailableChallengeAndSolutionStorage
{
    private readonly Dictionary<Type, Dictionary<Type, HashSet<string>>> _availableChallengeAndSolutions;

    internal AvailableChallengeAndSolutionStorage(
        Dictionary<Type, Dictionary<Type, HashSet<string>>> availableChallengeAndSolutions)
    {
        _availableChallengeAndSolutions = availableChallengeAndSolutions ??
                                        throw new ArgumentNullException(nameof(availableChallengeAndSolutions));
    }

    public bool IsAvailable<TChallenge, TSolution>(string? handlerName = default)
        where TChallenge : IChallenge where TSolution : ISolution
    {
        return _availableChallengeAndSolutions.TryGetValue(typeof(TChallenge),
                   out Dictionary<Type, HashSet<string>> solutionTypes) &&
               (handlerName == default ||
                solutionTypes.TryGetValue(typeof(TSolution), out HashSet<string> handlerNames) &&
                handlerNames.Contains(handlerName));
    }
}