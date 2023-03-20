using PassChallenge.Core.Producer;
using PassChallenge.Core.Solutions;
using PassChallenge.Core.Solver;
using PassChallenge.Core.Tests.Tools;
using Moq;
using NUnit.Framework;
using PassChallenge.Core.Challenges;

namespace PassChallenge.Core.Tests.Solver;

public class ChallengeSolverTests
{
    [Test]
    public void Constructor_Is_Correct()
    {
        Mock<IProducer> mock = new();

        string expectedHandlerName = "handler-name";

        ChallengeSolver<IChallenge, ISolution> solver = new(mock.Object, expectedHandlerName);
        Assert.That(solver.HandlerName, Is.EqualTo(expectedHandlerName));
    }

    [Test]
    public void Constructor_When_Producer_Is_Null_Throws_ArgumentNullException()
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentNullException>(() => new ChallengeSolver<IChallenge, ISolution>(null!));
    }

    [Test]
    public async Task Solve_Is_Correct()
    {
        Mock<IProducer> mock = new();
        mock.Setup(x =>
            x.ProduceAndWaitSolution<IChallenge, ISolution>(It.IsAny<IChallenge>(), It.IsAny<string>(),
                It.IsAny<CancellationToken>()));

        string expectedHandlerName = "handler-name";
        IChallenge expectedChallenge = new TestChallenge();
        CancellationToken expectedCancellationToken = new CancellationTokenSource(10).Token;

        ChallengeSolver<IChallenge, ISolution> solver = new(mock.Object, expectedHandlerName);

        await solver.Solve(expectedChallenge, expectedCancellationToken);

        mock.Verify(x =>
            x.ProduceAndWaitSolution<IChallenge, ISolution>(It.Is<IChallenge>(mo => mo == expectedChallenge),
                It.Is<string>(mo => mo == expectedHandlerName),
                It.Is<CancellationToken>(mo => mo == expectedCancellationToken)), Times.Once);
    }

    [Test]
    public async Task Solve_Returns_Solution()
    {
        TestSolution expectedSolution = new(SolutionResultType.Solved);

        Mock<IProducer> mock = new();
        mock.Setup(x =>
                x.ProduceAndWaitSolution<TestChallenge, TestSolution>(It.IsAny<TestChallenge>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(expectedSolution));

        ChallengeSolver<TestChallenge, TestSolution> solver = new(mock.Object);
        TestSolution actualSolution = await solver.Solve(new TestChallenge());

        mock.Verify(
            x => x.ProduceAndWaitSolution<TestChallenge, TestSolution>(It.IsAny<TestChallenge>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        Assert.That(actualSolution, Is.EqualTo(expectedSolution));
    }

    [Test]
    public void Solve_When_Challenge_Is_Null_Throws_ArgumentNullException()
    {
        Mock<IProducer> mock = new();
        ChallengeSolver<IChallenge, ISolution> solver = new(mock.Object);

        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentNullException>(() => solver.Solve(null!));
    }
}