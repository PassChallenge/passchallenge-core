using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Producer;
using KillDNS.CaptchaSolver.Core.Solutions;
using KillDNS.CaptchaSolver.Core.Solver;
using Moq;
using NUnit.Framework;

namespace KillDNS.CaptchaSolver.Core.Tests.Solver;

public class CaptchaSolverFactoryTests
{
    [Test]
    public void Constructor_Is_Correct()
    {
        Mock<IProducer> mock = new();
        string expectedSolverName = "solver-0";
        CaptchaSolverFactory factory = new(mock.Object, expectedSolverName);
        Assert.That(factory.SolverName, Is.EqualTo(expectedSolverName));
    }

    [Test]
    public void Constructor_When_Producer_Is_Null_Throws_ArgumentNullException()
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentNullException>(() => new CaptchaSolverFactory(null!, "solver-0"));
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    public void Constructor_When_SolverName_Is_NullOrEmpty_Throws_ArgumentException(string? solverName)
    {
        Mock<IProducer> mock = new();
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentException>(() => new CaptchaSolverFactory(mock.Object, solverName!));
    }

    [Test]
    public void CanCreateSolver_When_Producer_Returns_True()
    {
        Mock<IProducer> mock = new();
        mock.Setup(x => x.CanProduce<ICaptcha, ISolution>(default)).Returns(true);

        CaptchaSolverFactory factory = new(mock.Object, "solver-0");

        bool expected = true;
        bool actual = factory.CanCreateSolver<ICaptcha, ISolution>();

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void CaptchaSolverFactory_CanCreateSolver_Returns_False()
    {
        Mock<IProducer> mock = new();

        CaptchaSolverFactory factory = new(mock.Object, "solver-0");

        bool expected = false;
        bool actual = factory.CanCreateSolver<ICaptcha, ISolution>();

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSolver_Returns_CaptchaSolver()
    {
        Mock<IProducer> mock = new();
        mock.Setup(x => x.CanProduce<ICaptcha, ISolution>(It.IsAny<string>())).Returns(true);
        CaptchaSolverFactory factory = new(mock.Object, "solver-0");

        ICaptchaSolver<ICaptcha, ISolution> solver = factory.CreateSolver<ICaptcha, ISolution>();

        Assert.IsInstanceOf<CaptchaSolver<ICaptcha, ISolution>>(solver);
    }

    [Test]
    public void CreateSolver_When_Throws_InvalidOperationException()
    {
        Mock<IProducer> mock = new();
        mock.Setup(x => x.CanProduce<ICaptcha, ISolution>(It.IsAny<string>())).Returns(false);
        CaptchaSolverFactory factory = new(mock.Object, "solver-0");

        Assert.Throws<InvalidOperationException>(() => factory.CreateSolver<ICaptcha, ISolution>());
    }

    [Test]
    public void GetHandlerNames_Is_Correct()
    {
        Mock<IProducer> mock = new();
        mock.Setup(x => x.GetHandlerNames<ICaptcha, ISolution>()).Returns(new List<string>());

        CaptchaSolverFactory factory = new(mock.Object, "solver-0");
        factory.GetHandlerNames<ICaptcha, ISolution>();

        mock.Verify(x => x.GetHandlerNames<ICaptcha, ISolution>(), Times.Once);
    }
}