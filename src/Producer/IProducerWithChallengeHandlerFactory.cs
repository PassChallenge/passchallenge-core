using PassChallenge.Core.Handlers;

namespace PassChallenge.Core.Producer;

public interface IProducerWithChallengeHandlerFactory : IProducer
{
    void SetChallengeHandlerFactory(IChallengeHandlerFactory challengeHandlerFactory);
}