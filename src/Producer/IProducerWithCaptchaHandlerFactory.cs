using PassChallenge.Core.Handlers;

namespace PassChallenge.Core.Producer;

public interface IProducerWithCaptchaHandlerFactory : IProducer
{
    void SetCaptchaHandlerFactory(ICaptchaHandlerFactory captchaHandlerFactory);
}