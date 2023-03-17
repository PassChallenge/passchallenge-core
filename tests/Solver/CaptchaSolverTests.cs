using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Producer;
using KillDNS.CaptchaSolver.Core.Solutions;
using KillDNS.CaptchaSolver.Core.Solver;
using KillDNS.CaptchaSolver.Core.Tests.Tools;
using Moq;
using NUnit.Framework;

namespace KillDNS.CaptchaSolver.Core.Tests.Solver;

public class CaptchaSolverTests
{
    [Test]
    public void CaptchaSolver_Constructor_Is_Correct()
    {
        Mock<IProducer> mock = new();

        string expectedHandlerName = "handler-name";

        CaptchaSolver<ICaptcha, ISolution> solver = new(mock.Object, expectedHandlerName);
        Assert.That(solver.HandlerName, Is.EqualTo(expectedHandlerName));
    }

    [Test]
    public void CaptchaSolver_Constructor_When_Producer_Is_Null_Throws_ArgumentNullException()
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentNullException>(() => new CaptchaSolver<ICaptcha, ISolution>(null!));
    }

    [Test]
    public async Task CaptchaSolver_Constructor_When_Producer_Is_IProducerWithSpecifiedCaptchaAndSolutions()
    {
        Mock<IProducerWithSpecifiedCaptchaAndSolutions> mock = new();
        mock.Setup(x =>
            x.ProduceAndWaitSolution<ICaptcha, ISolution>(It.IsAny<ICaptcha>(), It.IsAny<string>(),
                It.IsAny<CancellationToken>()));

        string expectedHandlerName = "handler-name";
        ICaptcha expectedCaptcha = new TestCaptcha();
        CancellationToken expectedCancellationToken = new CancellationTokenSource(10).Token;

        CaptchaSolver<ICaptcha, ISolution> solver = new(mock.Object, expectedHandlerName);

        await solver.Solve(expectedCaptcha, expectedCancellationToken);

        mock.Verify(x =>
            x.ProduceAndWaitSolution<ICaptcha, ISolution>(It.Is<ICaptcha>(mo => mo == expectedCaptcha),
                It.Is<string>(mo => mo == expectedHandlerName),
                It.Is<CancellationToken>(mo => mo == expectedCancellationToken)), Times.Once);
    }

    [Test]
    public async Task CaptchaSolver_Solve_Returns_Solution()
    {
        TestSolution expectedSolution = new(SolutionResultType.Solved);

        Mock<IProducer> mock = new();
        mock.Setup(x =>
                x.ProduceAndWaitSolution<TestCaptcha, TestSolution>(It.IsAny<TestCaptcha>(),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(expectedSolution));

        CaptchaSolver<TestCaptcha, TestSolution> solver = new(mock.Object);
        TestSolution actualSolution = await solver.Solve(new TestCaptcha());

        mock.Verify(
            x => x.ProduceAndWaitSolution<TestCaptcha, TestSolution>(It.IsAny<TestCaptcha>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        Assert.That(actualSolution, Is.EqualTo(expectedSolution));
    }

    [Test]
    public void CaptchaSolver_Solve_When_Captcha_Is_Null_Throws_ArgumentNullException()
    {
        Mock<IProducer> mock = new();
        CaptchaSolver<ICaptcha, ISolution> solver = new(mock.Object);

        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentNullException>(() => solver.Solve(null!));
    }
}