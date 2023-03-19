using PassChallenge.Core.Handlers;
using PassChallenge.Core.Solutions;
using PassChallenge.Core.Solver;
using Moq;
using NUnit.Framework;
using PassChallenge.Core.Challenges;

namespace PassChallenge.Core.Tests.Solver;

public class AvailableChallengeAndSolutionStorageBuilderTests
{
    [Test]
    public void AddSupportChallengeAndSolution_Generics_Is_Correct()
    {
        AvailableChallengeAndSolutionStorageBuilder builder = new();
        builder.AddSupportChallengeAndSolution<IChallenge, ISolution>();

        IAvailableChallengeAndSolutionStorage availableChallengeAndSolutionStorage = builder.Build();

        Assert.That(availableChallengeAndSolutionStorage.IsAvailable<IChallenge, ISolution>(), Is.True);
    }

    [Test]
    public void AddSupportChallengeAndSolution_Generics_With_HandlerName_Is_Correct()
    {
        AvailableChallengeAndSolutionStorageBuilder builder = new();
        string handlerName = "handler-name";

        builder.AddSupportChallengeAndSolution<IChallenge, ISolution>(handlerName);

        IAvailableChallengeAndSolutionStorage availableChallengeAndSolutionStorage = builder.Build();

        Assert.That(availableChallengeAndSolutionStorage.IsAvailable<IChallenge, ISolution>(handlerName), Is.True);
    }

    [Test]
    public void
        AddSupportChallengeAndSolution_Generics_When_Add_Same_HandlerName_Challenge_And_Solution_Throws_InvalidOperationException()
    {
        AvailableChallengeAndSolutionStorageBuilder builder = new();
        string handlerName = "handler-name";
        builder.AddSupportChallengeAndSolution<IChallenge, ISolution>(handlerName);

        Assert.Throws<InvalidOperationException>(() =>
            builder.AddSupportChallengeAndSolution<IChallenge, ISolution>(handlerName));
    }

    [Test]
    public void
        AddSupportChallengeAndSolution_ChallengeHandlerDescriptor_When_Descriptor_Is_Null_Throws_ArgumentNullException()
    {
        AvailableChallengeAndSolutionStorageBuilder builder = new();

        Assert.Throws<ArgumentNullException>(() =>
            builder.AddSupportChallengeAndSolution(null!));
    }

    [Test]
    public void AddSupportChallengeAndSolution_ChallengeHandlerDescriptor_Is_Correct()
    {
        AvailableChallengeAndSolutionStorageBuilder builder = new();
        builder.AddSupportChallengeAndSolution(
            ChallengeHandlerDescriptor.Create<IChallenge, ISolution>((_, _) => Task.FromResult(It.IsAny<ISolution>()),
                "handler"));

        IAvailableChallengeAndSolutionStorage availableChallengeAndSolutionStorage = builder.Build();

        Assert.That(availableChallengeAndSolutionStorage.IsAvailable<IChallenge, ISolution>(), Is.True);
    }

    [Test]
    public void AddSupportChallengeAndSolution_ChallengeHandlerDescriptor_With_HandlerName_Is_Correct()
    {
        AvailableChallengeAndSolutionStorageBuilder builder = new();
        string handlerName = "handler-name";
        builder.AddSupportChallengeAndSolution(
            ChallengeHandlerDescriptor.Create<IChallenge, ISolution>((_, _) => Task.FromResult(It.IsAny<ISolution>()),
                handlerName));

        IAvailableChallengeAndSolutionStorage availableChallengeAndSolutionStorage = builder.Build();

        Assert.That(availableChallengeAndSolutionStorage.IsAvailable<IChallenge, ISolution>(handlerName), Is.True);
    }

    [Test]
    public void SetStorage_Is_Correct()
    {
        Mock<IAvailableChallengeAndSolutionStorage> mock = new();

        AvailableChallengeAndSolutionStorageBuilder builder = new();
        builder.SetStorage(mock.Object);

        IAvailableChallengeAndSolutionStorage availableChallengeAndSolutionStorage = builder.Build();

        Assert.That(availableChallengeAndSolutionStorage, Is.EqualTo(mock.Object));
    }
}