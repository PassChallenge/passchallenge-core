using System;
using System.Collections.Generic;
using PassChallenge.Core.Challenges;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Handlers;

public interface IChallengeHandlerFactory
{
    string GetDefaultHandlerName<TChallenge, TSolution>() where TChallenge : IChallenge where TSolution : ISolution;

    IReadOnlyCollection<string> GetHandlerNames<TChallenge, TSolution>()
        where TChallenge : IChallenge where TSolution : ISolution;

    bool CanProduce<TChallenge, TSolution>(string? handlerName = default)
        where TChallenge : IChallenge where TSolution : ISolution;

    IChallengeHandler<TChallenge, TSolution> CreateHandler<TChallenge, TSolution>(
        IServiceProvider serviceProvider, string? handlerName = default)
        where TChallenge : IChallenge
        where TSolution : ISolution;
}