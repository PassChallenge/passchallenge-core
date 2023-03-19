using System.Threading;
using System.Threading.Tasks;
using PassChallenge.Core.Challenges;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Solver;

public interface IChallengeSolver<in TChallenge, TSolution> where TChallenge : IChallenge where TSolution : ISolution
{
    public string? HandlerName { get; }

    Task<TSolution> Solve(TChallenge challenge, CancellationToken cancellationToken = default);
}