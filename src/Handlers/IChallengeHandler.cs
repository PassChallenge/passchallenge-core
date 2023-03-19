using System.Threading;
using System.Threading.Tasks;
using PassChallenge.Core.Challenges;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Handlers;

public interface IChallengeHandler<in TChallenge, TSolution> where TChallenge : IChallenge
    where TSolution : ISolution
{
    Task<TSolution> Handle(TChallenge challenge, CancellationToken cancellationToken = default);
}