using System;
using PassChallenge.Core.Handlers;
using PassChallenge.Core.Producer;
using PassChallenge.Core.Solver;

namespace PassChallenge.Core.Extensions;

public static class CaptchaSolverBuilderExtensions
{
    public static CaptchaSolverBuilder<TProducer> SetCaptchaHandlerFactory<TProducer>(
        this CaptchaSolverBuilder<TProducer> builder,
        ICaptchaHandlerFactory captchaHandlerFactory) where TProducer : IProducerWithCaptchaHandlerFactory
    {
        if (builder == null)
            throw new ArgumentNullException(nameof(builder));

        if (captchaHandlerFactory == null)
            throw new ArgumentNullException(nameof(captchaHandlerFactory));

        builder.AddConfigureProducerAction(producer => producer.SetCaptchaHandlerFactory(captchaHandlerFactory));

        return builder;
    }
}