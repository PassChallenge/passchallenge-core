using System;
using System.Collections.Generic;
using PassChallenge.Core.Challenges;
using PassChallenge.Core.Producer;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Solver;

internal class ChallengeSolverFactory : IChallengeSolverFactory
{
    private readonly IProducer _producer;

    public ChallengeSolverFactory(IProducer producer, string solverName)
    {
        _producer = producer ?? throw new ArgumentNullException(nameof(producer));

        if (string.IsNullOrWhiteSpace(solverName))
            throw new ArgumentException("Is null or whitespace.", nameof(solverName));

        SolverName = solverName;
    }

    public string SolverName { get; }

    public IChallengeSolver<TChallenge, TSolution> CreateSolver<TChallenge, TSolution>(string? handlerName = default)
        where TChallenge : IChallenge where TSolution : ISolution
    {
        if (CanCreateSolver<TChallenge, TSolution>(handlerName) == false)
            throw new InvalidOperationException(
                $"Can't create solver for '{_producer.GetType()}' producer. Challenge '{typeof(TChallenge)}', Solution '{typeof(TSolution)}', Handler name {(handlerName == default ? "default" : $"'{handlerName}'")}.");

        handlerName ??= _producer.GetDefaultHandlerName<TChallenge, TSolution>();

        return new ChallengeSolver<TChallenge, TSolution>(_producer, handlerName);
    }

    public bool CanCreateSolver<TChallenge, TSolution>(string? handlerName = default)
        where TChallenge : IChallenge where TSolution : ISolution
    {
        return _producer.CanProduce<TChallenge, TSolution>(handlerName);
    }

    public IReadOnlyCollection<string> GetHandlerNames<TChallenge, TSolution>()
        where TChallenge : IChallenge where TSolution : ISolution
    {
        return _producer.GetHandlerNames<TChallenge, TSolution>();
    }
}