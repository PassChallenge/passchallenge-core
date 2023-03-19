using PassChallenge.Core.Producer;
using PassChallenge.Core.Solutions;
using PassChallenge.Core.Solver;
using Moq;
using NUnit.Framework;
using PassChallenge.Core.Challenges;

namespace PassChallenge.Core.Tests.Solver;

public class ChallengeSolverFactoryTests
{
    [Test]
    public void Constructor_Is_Correct()
    {
        Mock<IProducer> mock = new();
        string expectedSolverName = "solver-0";
        ChallengeSolverFactory factory = new(mock.Object, expectedSolverName);
        Assert.That(factory.SolverName, Is.EqualTo(expectedSolverName));
    }

    [Test]
    public void Constructor_When_Producer_Is_Null_Throws_ArgumentNullException()
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentNullException>(() => new ChallengeSolverFactory(null!, "solver-0"));
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    public void Constructor_When_SolverName_Is_NullOrEmpty_Throws_ArgumentException(string? solverName)
    {
        Mock<IProducer> mock = new();
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentException>(() => new ChallengeSolverFactory(mock.Object, solverName!));
    }

    [Test]
    public void CanCreateSolver_When_Producer_Returns_True()
    {
        Mock<IProducer> mock = new();
        mock.Setup(x => x.CanProduce<IChallenge, ISolution>(default)).Returns(true);

        ChallengeSolverFactory factory = new(mock.Object, "solver-0");

        bool expected = true;
        bool actual = factory.CanCreateSolver<IChallenge, ISolution>();

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void ChallengeSolverFactory_CanCreateSolver_Returns_False()
    {
        Mock<IProducer> mock = new();

        ChallengeSolverFactory factory = new(mock.Object, "solver-0");

        bool expected = false;
        bool actual = factory.CanCreateSolver<IChallenge, ISolution>();

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSolver_Returns_ChallengeSolver()
    {
        Mock<IProducer> mock = new();
        mock.Setup(x => x.CanProduce<IChallenge, ISolution>(It.IsAny<string>())).Returns(true);
        ChallengeSolverFactory factory = new(mock.Object, "solver-0");

        IChallengeSolver<IChallenge, ISolution> solver = factory.CreateSolver<IChallenge, ISolution>();

        Assert.IsInstanceOf<ChallengeSolver<IChallenge, ISolution>>(solver);
    }

    [Test]
    public void CreateSolver_When_Throws_InvalidOperationException()
    {
        Mock<IProducer> mock = new();
        mock.Setup(x => x.CanProduce<IChallenge, ISolution>(It.IsAny<string>())).Returns(false);
        ChallengeSolverFactory factory = new(mock.Object, "solver-0");

        Assert.Throws<InvalidOperationException>(() => factory.CreateSolver<IChallenge, ISolution>());
    }

    [Test]
    public void GetHandlerNames_Is_Correct()
    {
        Mock<IProducer> mock = new();
        mock.Setup(x => x.GetHandlerNames<IChallenge, ISolution>()).Returns(new List<string>());

        ChallengeSolverFactory factory = new(mock.Object, "solver-0");
        factory.GetHandlerNames<IChallenge, ISolution>();

        mock.Verify(x => x.GetHandlerNames<IChallenge, ISolution>(), Times.Once);
    }
}