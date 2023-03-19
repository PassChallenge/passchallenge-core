using PassChallenge.Core.Solver;

namespace PassChallenge.Core.Handlers;

public interface ICaptchaHandlerDescriptorAvailableStorage : ICaptchaHandlerDescriptorStorage,
    IAvailableCaptchaAndSolutionStorage
{
}