using System;
using System.Threading;
using System.Threading.Tasks;
using PassChallenge.Core.Challenges;
using PassChallenge.Core.Producer;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Solver;

public class ChallengeSolver<TChallenge, TSolution> : IChallengeSolver<TChallenge, TSolution>
    where TChallenge : IChallenge where TSolution : ISolution
{
    private readonly IProducer _producer;

    public ChallengeSolver(IProducer producer, string? handlerName = default)
    {
        _producer = producer ?? throw new ArgumentNullException(nameof(producer));
        HandlerName = handlerName;
    }

    public string? HandlerName { get; }

    public Task<TSolution> Solve(TChallenge challenge, CancellationToken cancellationToken = default)
    {
        if (challenge == null)
            throw new ArgumentNullException(nameof(challenge));

        return _producer.ProduceAndWaitSolution<TChallenge, TSolution>(challenge, HandlerName,
            cancellationToken);
    }
}