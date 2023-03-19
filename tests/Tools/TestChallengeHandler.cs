using PassChallenge.Core.Challenges;
using PassChallenge.Core.Handlers;

namespace PassChallenge.Core.Tests.Tools;

public class TestChallengeHandler<TChallenge, TSolution> : IChallengeHandler<TChallenge, TSolution>
    where TChallenge : IChallenge where TSolution : PassChallenge.Core.Solutions.ISolution
{
    public Task<TSolution> Handle(TChallenge challenge, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}