using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using PassChallenge.Core.Handlers;
using PassChallenge.Core.Solver;

namespace PassChallenge.Core.Consumer;

public class ChallengeConsumerBuilder<TConsumer> : IBuilderWithChallengeHandlers where TConsumer : IConsumer
{
    public ChallengeConsumerBuilder(IServiceCollection serviceCollection)
    {
        
    }

    public ChallengeHandlerDescriptorStorageBuilder DescriptorStorageBuilder { get; } = new();
    
    public TConsumer Build(IServiceProvider provider)
    {
        if (provider == null)
            throw new ArgumentNullException(nameof(provider));

        TConsumer consumer = InternalBuild(provider);
        //producer = PostConfigureProducer(producer);
        return consumer;
    }

    protected virtual TConsumer InternalBuild(IServiceProvider provider)
    {
        var parameters = typeof(TConsumer).GetConstructors(BindingFlags.Instance | BindingFlags.Public)[0]
            .GetParameters()
            .Select(x => provider.GetRequiredService(x.ParameterType)).ToArray();

        TConsumer consumer = (TConsumer)Activator.CreateInstance(typeof(TConsumer), parameters);

        //consumer.SetAvailableChallengeAndSolutionStorage(AvailableChallengeAndSolutionStorageBuilder.Build());

        return consumer;
    }
}