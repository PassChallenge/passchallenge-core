using KillDNS.CaptchaSolver.Core.Handlers;

namespace KillDNS.CaptchaSolver.Core.Producer;

public interface IProducerWithCaptchaHandlerFactory : IProducer
{
    void SetCaptchaHandlerFactory(ICaptchaHandlerFactory captchaHandlerFactory);
}