using KillDNS.CaptchaSolver.Core.Handlers;

namespace KillDNS.CaptchaSolver.Core.Producer;

public interface IProducerWithCaptchaHandlerFactory : IProducer
{
    public void SetCaptchaHandlerFactory(ICaptchaHandlerFactory captchaHandlerFactory);
}