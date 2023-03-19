using PassChallenge.Core.Handlers;

namespace PassChallenge.Core.Tests.Tools;

public class TestChallengeHandlerWithOneArgument : IChallengeHandler<TestChallenge, TestSolution>
{
    // ReSharper disable once UnusedParameter.Local
    public TestChallengeHandlerWithOneArgument(object obj)
    {
    }

    public Task<TestSolution> Handle(TestChallenge challenge, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}