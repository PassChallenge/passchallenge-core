using PassChallenge.Core.Producer;
using PassChallenge.Core.Solver;
using PassChallenge.Core.Tests.Tools;
using Moq;
using NUnit.Framework;

namespace PassChallenge.Core.Tests.Solver;

public class ChallengeSolverBuilderTests
{
    [Test]
    public void Constructor_When_Producer_Is_Interface_Throws_ArgumentNullException()
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentException>(() => new ChallengeSolverBuilder<IProducer>());
    }

    [Test]
    public void Build_Is_Correct()
    {
        Mock<IServiceProvider> serviceProviderMock = new();
        ChallengeSolverBuilder<TestProducer> builder = new();
        builder.Build(serviceProviderMock.Object);
        Assert.Pass();
    }

    [Test]
    public void Build_Producer_With_Dependencies_Call_ServiceProvider()
    {
        Mock<IServiceProvider> serviceProviderMock = new();
        serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(type => type == typeof(Action))));

        ChallengeSolverBuilder<TestProducerWithOneArgument> builder = new();

        Assert.Throws<InvalidOperationException>(() => builder.Build(serviceProviderMock.Object));
        serviceProviderMock.Verify(x => x.GetService(It.Is<Type>(type => type == typeof(Action))), Times.Once);
    }

    [Test]
    public void Build_Producer_Is_Null_And_Throws_ArgumentNullException()
    {
        ChallengeSolverBuilder<TestProducer> builder = new();
        Assert.Throws<ArgumentNullException>(() => builder.Build(null!));
    }

    [Test]
    public void AddConfigureProducerAction_Action_Is_Called_And_Returns_Builder()
    {
        Mock<IServiceProvider> serviceProviderMock = new();

        Mock<Action<IProducer>> action = new();
        action.Setup(x => x.Invoke(It.IsAny<IProducer>()));

        ChallengeSolverBuilder<TestProducerWithChallengeHandlerFactory> builder = new();
        builder.AddConfigureProducerAction(action.Object);

        TestProducerWithChallengeHandlerFactory producer = builder.Build(serviceProviderMock.Object);

        Assert.That(producer, Is.InstanceOf<TestProducerWithChallengeHandlerFactory>());
        action.Verify(x => x.Invoke(It.IsAny<IProducer>()), Times.Once);
    }

    [Test]
    public void AddConfigureProducerAction_Action_Is_Null_And_Throws_ArgumentNullException()
    {
        ChallengeSolverBuilder<TestProducerWithChallengeHandlerFactory> builder = new();
        Assert.Throws<ArgumentNullException>(() => builder.AddConfigureProducerAction(null!));
    }

    [Test]
    public void Build_Call_Producer_SetAvailableChallengeAndSolutionStorage()
    {
        Mock<IServiceProvider> serviceProviderMock = new();

        ChallengeSolverBuilder<TestProducer> builder = new();
        TestProducer producer = builder.Build(serviceProviderMock.Object);

        Assert.IsNotNull(producer.AvailableChallengeAndSolutionStorage);
    }
}