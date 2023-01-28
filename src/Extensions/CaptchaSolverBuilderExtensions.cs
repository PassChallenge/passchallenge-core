using KillDNS.CaptchaSolver.Core.Handlers;
using KillDNS.CaptchaSolver.Core.Producer;

namespace KillDNS.CaptchaSolver.Core.Extensions;

public static class CaptchaSolverBuilderExtensions
{
    public static void SetProducerHandlerFactory<TProducer>(this CaptchaSolverBuilder<TProducer> builder,
        ICaptchaHandlerFactory captchaHandlerFactory) where TProducer : IProducerWithCaptchaHandlerFactory
    {
        builder.AddConfigureProducerAction(factory => factory.SetCaptchaHandlerFactory(captchaHandlerFactory));
    }
}