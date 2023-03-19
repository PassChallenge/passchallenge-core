using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KillDNS.ChallengeSolver.Core.Challenge;
using KillDNS.ChallengeSolver.Core.Consumer;
using KillDNS.ChallengeSolver.Core.Handlers;
using KillDNS.ChallengeSolver.Core.Solutions;
using Microsoft.Extensions.DependencyInjection;

namespace KillDNS.ChallengeSolver.Core.Resolver;

//TODO: WIP
public class ChallengeResolverBuilder : IBuilderWithChallengeHandlers
{
    public ChallengeResolverBuilder(IServiceCollection serviceCollection)
    {
        ServiceCollection = serviceCollection;
        MessageHandlers = new List<Type>();
    }

    public IServiceCollection ServiceCollection { get; }

    internal List<Type> MessageHandlers { get; }

    internal Type? ConsumerType { get; private set; }

    internal Func<IServiceProvider, IConsumer>? Factory { get; private set; }

    public ChallengeResolverBuilder SetConsumer(Func<IServiceProvider, IConsumer> factory)
    {
        Factory = factory;
        return this;
    }

    public ChallengeResolverBuilder SetConsumer<TConsumer>() where TConsumer : IConsumer
    {
        ConsumerType = typeof(TConsumer);
        return this;
    }

    public IBuilderWithChallengeHandlers AddChallengeHandler<TChallenge, TSolution, THandler>(string? handlerName = default)
        where TChallenge : IChallenge
        where TSolution : ISolution
        where THandler : IChallengeHandler<TChallenge, TSolution>
    {
        MessageHandlers.Add(typeof(THandler));
        return this;
    }

    public IBuilderWithChallengeHandlers AddChallengeHandler<TChallenge, TSolution, THandler>(
        Func<IServiceProvider, THandler> factory, string? handlerName = default) where TChallenge : IChallenge
        where TSolution : ISolution
        where THandler : IChallengeHandler<TChallenge, TSolution>
    {
        throw new NotImplementedException();
    }

    public IBuilderWithChallengeHandlers AddChallengeHandler<TChallenge, TSolution>(
        Func<IServiceProvider, TChallenge, Task<TSolution>> func, string? handlerName = default)
        where TChallenge : IChallenge where TSolution : ISolution
    {
        throw new NotImplementedException();
    }
}