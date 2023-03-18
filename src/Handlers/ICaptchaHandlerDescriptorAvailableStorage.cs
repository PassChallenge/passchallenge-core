using KillDNS.CaptchaSolver.Core.Solver;

namespace KillDNS.CaptchaSolver.Core.Handlers;

public interface ICaptchaHandlerDescriptorAvailableStorage : ICaptchaHandlerDescriptorStorage,
    IAvailableCaptchaAndSolutionStorage
{
}