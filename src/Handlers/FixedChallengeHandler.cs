using System;
using System.Threading;
using System.Threading.Tasks;
using PassChallenge.Core.Challenges;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Handlers;

internal class FixedChallengeHandler<TChallenge, TSolution> : IChallengeHandler<TChallenge, TSolution>
    where TChallenge : IChallenge
    where TSolution : ISolution
{
    private readonly Func<IServiceProvider, TChallenge, Task<TSolution>> _resolveFunc;
    private readonly IServiceProvider _serviceProvider;

    public FixedChallengeHandler(IServiceProvider serviceProvider,
        Func<IServiceProvider, TChallenge, Task<TSolution>> resolveFunc)
    {
        _serviceProvider = serviceProvider;
        _resolveFunc = resolveFunc ?? throw new ArgumentNullException(nameof(resolveFunc));
    }

    public Task<TSolution> Handle(TChallenge challenge, CancellationToken cancellationToken = default)
    {
        return _resolveFunc.Invoke(_serviceProvider, challenge);
    }
}