using System.Threading.Tasks;
using PassChallenge.Core.Challenges;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Consumer;

public interface IConsumerHandler<in TChallenge, TSolution> where TChallenge : IChallenge where TSolution : ISolution
{
    Task<TSolution> Handle(TChallenge challenge);
}