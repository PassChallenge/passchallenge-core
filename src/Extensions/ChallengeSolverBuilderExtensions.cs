using System;
using PassChallenge.Core.Handlers;
using PassChallenge.Core.Producer;
using PassChallenge.Core.Solver;

namespace PassChallenge.Core.Extensions;

public static class ChallengeSolverBuilderExtensions
{
    public static ChallengeSolverBuilder<TProducer> SetChallengeHandlerFactory<TProducer>(
        this ChallengeSolverBuilder<TProducer> builder,
        IChallengeHandlerFactory challengeHandlerFactory) where TProducer : IProducerWithChallengeHandlerFactory
    {
        if (builder == null)
            throw new ArgumentNullException(nameof(builder));

        if (challengeHandlerFactory == null)
            throw new ArgumentNullException(nameof(challengeHandlerFactory));

        builder.AddConfigureProducerAction(producer => producer.SetChallengeHandlerFactory(challengeHandlerFactory));

        return builder;
    }
}