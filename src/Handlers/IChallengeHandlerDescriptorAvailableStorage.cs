using PassChallenge.Core.Solver;

namespace PassChallenge.Core.Handlers;

public interface IChallengeHandlerDescriptorAvailableStorage : IChallengeHandlerDescriptorStorage,
    IAvailableChallengeAndSolutionStorage
{
}