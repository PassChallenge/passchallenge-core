using System.Collections.Generic;
using PassChallenge.Core.Challenges;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Solver;

public interface IChallengeSolverFactory
{
    public string SolverName { get; }

    IChallengeSolver<TChallenge, TSolution> CreateSolver<TChallenge, TSolution>(string? handlerName = default)
        where TChallenge : IChallenge where TSolution : ISolution;

    bool CanCreateSolver<TChallenge, TSolution>(string? handlerName = default)
        where TChallenge : IChallenge where TSolution : ISolution;

    IReadOnlyCollection<string> GetHandlerNames<TChallenge, TSolution>()
        where TChallenge : IChallenge where TSolution : ISolution;
}