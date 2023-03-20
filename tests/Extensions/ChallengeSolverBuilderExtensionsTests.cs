using PassChallenge.Core.Extensions;
using PassChallenge.Core.Handlers;
using PassChallenge.Core.Solver;
using PassChallenge.Core.Tests.Tools;
using Moq;
using NUnit.Framework;

namespace PassChallenge.Core.Tests.Extensions;

public class ChallengeSolverBuilderExtensionsTests
{
    [Test]
    public void SetChallengeHandlerFactory_Is_Correct()
    {
        IServiceProvider serviceProvider = new Mock<IServiceProvider>().Object;

        ChallengeSolverBuilder<TestProducerWithChallengeHandlerFactory> builder = new();
        builder.SetChallengeHandlerFactory(new Mock<IChallengeHandlerFactory>().Object);
        builder.Build(serviceProvider);

        Assert.Pass();
    }

    [Test]
    public void SetChallengeHandlerFactory_When_Builder_Is_Null_Throw_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            ChallengeSolverBuilderExtensions.SetChallengeHandlerFactory<TestProducerWithChallengeHandlerFactory>(null!,
                new Mock<IChallengeHandlerFactory>().Object));
    }

    [Test]
    public void SetChallengeHandlerFactory_When_ChallengeHandlerFactory_Is_Null_Throw_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new Mock<ChallengeSolverBuilder<TestProducerWithChallengeHandlerFactory>>().Object
                .SetChallengeHandlerFactory(null!));
    }

    [Test]
    public void SetChallengeHandlerFactory_Returns_Same_Builder()
    {
        Mock<ChallengeSolverBuilder<TestProducerWithChallengeHandlerFactory>> mockedBuilder = new();
        ChallengeSolverBuilder<TestProducerWithChallengeHandlerFactory> builder = mockedBuilder.Object;

        ChallengeSolverBuilder<TestProducerWithChallengeHandlerFactory> returnedValue =
            builder.SetChallengeHandlerFactory(new Mock<IChallengeHandlerFactory>().Object);

        Assert.That(returnedValue, Is.EqualTo(builder));
    }
}