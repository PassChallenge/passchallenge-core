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
        Assert.That(new CaptchaSolverFactory(mock.Object), Is.Not.Null);
    }

    [Test]
    public void Constructor_When_Producer_Is_Null_Throws_ArgumentNullException()
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentNullException>(() => new CaptchaSolverFactory(null!));
    }

    [Test]
    public void CanCreateSolver_When_Producer_Is_Not_Specified_Returns_True()
    {
        Mock<IProducer> mock = new();
        CaptchaSolverFactory factory = new(mock.Object);

        bool expected = true;
        bool actual = factory.CanCreateSolver<ICaptcha, ISolution>();

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void CanCreateSolver_When_Producer_Is_Specified_Returns_True()
    {
        Mock<IProducerWithSpecifiedCaptchaAndSolutions> mock = new();
        mock.Setup(x => x.CanProduce<ICaptcha, ISolution>(default)).Returns(true);

        CaptchaSolverFactory factory = new(mock.Object);

        bool expected = true;
        bool actual = factory.CanCreateSolver<ICaptcha, ISolution>();

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void CaptchaSolverFactory_CanCreateSolver_When_Producer_Is_Specified_Returns_False()
    {
        Mock<IProducerWithSpecifiedCaptchaAndSolutions> mock = new();

        CaptchaSolverFactory factory = new(mock.Object);

        bool expected = false;
        bool actual = factory.CanCreateSolver<ICaptcha, ISolution>();

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void CreateSolver_Returns_CaptchaSolver()
    {
        Mock<IProducer> mock = new();
        CaptchaSolverFactory factory = new(mock.Object);

        ICaptchaSolver<ICaptcha, ISolution> solver = factory.CreateSolver<ICaptcha, ISolution>();

        Assert.IsInstanceOf<CaptchaSolver<ICaptcha, ISolution>>(solver);
    }

    [Test]
    public void CreateSolver_When_Producer_Is_Specified_Throws_InvalidOperationException()
    {
        Mock<IProducerWithSpecifiedCaptchaAndSolutions> mock = new();
        CaptchaSolverFactory factory = new(mock.Object);

        Assert.Throws<InvalidOperationException>(() => factory.CreateSolver<ICaptcha, ISolution>());
    }

    [Test]
    public void GetHandlerNames_Is_Correct()
    {
        Mock<IProducerWithSpecifiedCaptchaAndSolutions> mock = new();
        mock.Setup(x => x.GetHandlerNames<ICaptcha, ISolution>()).Returns(new List<string>());

        CaptchaSolverFactory factory = new(mock.Object);
        factory.GetHandlerNames<ICaptcha, ISolution>();

        mock.Verify(x => x.GetHandlerNames<ICaptcha, ISolution>(), Times.Once);
    }

    [Test]
    public void GetHandlerNames_When_Producer_Is_IProducer_Throws_InvalidOperationException()
    {
        Mock<IProducer> mock = new();

        CaptchaSolverFactory factory = new(mock.Object);

        Assert.Throws<InvalidOperationException>(() => factory.GetHandlerNames<ICaptcha, ISolution>());
    }
}