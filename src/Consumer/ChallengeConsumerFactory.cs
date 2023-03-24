namespace PassChallenge.Core.Consumer;

class ChallengeConsumerFactory<TConsumer> : IChallengeConsumerFactory where TConsumer : IConsumer
{
    private readonly string _name;

    public ChallengeConsumerFactory(string name)
    {
        _name = name;
    }

    public IConsumer Create()
    {
        throw new System.NotImplementedException();
    }
}