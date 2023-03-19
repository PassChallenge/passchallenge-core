using System.Collections.Generic;
using PassChallenge.Core.Captcha;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Solver;

public interface ICaptchaSolverFactory
{
    public string SolverName { get; }

    ICaptchaSolver<TCaptcha, TSolution> CreateSolver<TCaptcha, TSolution>(string? handlerName = default)
        where TCaptcha : ICaptcha where TSolution : ISolution;

    bool CanCreateSolver<TCaptcha, TSolution>(string? handlerName = default)
        where TCaptcha : ICaptcha where TSolution : ISolution;

    IReadOnlyCollection<string> GetHandlerNames<TCaptcha, TSolution>()
        where TCaptcha : ICaptcha where TSolution : ISolution;
}