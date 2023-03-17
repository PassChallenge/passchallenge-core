using System;
using KillDNS.CaptchaSolver.Core.Handlers;
using KillDNS.CaptchaSolver.Core.Producer;
using KillDNS.CaptchaSolver.Core.Solver;

namespace KillDNS.CaptchaSolver.Core.Extensions;

public static class CaptchaSolverBuilderExtensions
{
    public static CaptchaSolverBuilder<TProducer> SetCaptchaHandlerFactory<TProducer>(
        this CaptchaSolverBuilder<TProducer> builder,
        ICaptchaHandlerFactory captchaHandlerFactory) where TProducer : class, IProducerWithCaptchaHandlerFactory
    {
        if (builder == null)
            throw new ArgumentNullException(nameof(builder));

        if (captchaHandlerFactory == null)
            throw new ArgumentNullException(nameof(captchaHandlerFactory));

        builder.AddConfigureProducerAction(producer => producer.SetCaptchaHandlerFactory(captchaHandlerFactory));

        return builder;
    }
}