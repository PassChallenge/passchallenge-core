using PassChallenge.Core.Captcha;
using PassChallenge.Core.Handlers;
using PassChallenge.Core.Solutions;
using PassChallenge.Core.Tests.Tools;
using Moq;
using NUnit.Framework;

namespace PassChallenge.Core.Tests.Handlers;

public class CaptchaHandlerDescriptorTests
{
    [Test]
    public void Create_With_HandlerFunc_Is_Correct()
    {
        Mock<Func<IServiceProvider, ICaptcha, Task<ISolution>>> func = new();

        CaptchaHandlerDescriptor captchaHandlerDescriptor =
            CaptchaHandlerDescriptor.Create<ICaptcha, ISolution>((provider, captcha) =>
                func.Object.Invoke(provider, captcha));

        Assert.Multiple(() =>
        {
            Assert.That(captchaHandlerDescriptor.CaptchaType, Is.EqualTo(typeof(ICaptcha)));
            Assert.That(captchaHandlerDescriptor.SolutionType, Is.EqualTo(typeof(ISolution)));
            Assert.Null(captchaHandlerDescriptor.HandlerType);
            Assert.Null(captchaHandlerDescriptor.ImplementationFactory);
            Assert.NotNull(captchaHandlerDescriptor.SolverFunction);
        });

        captchaHandlerDescriptor.SolverFunction!.Invoke(new Mock<IServiceProvider>().Object, It.IsAny<ICaptcha>());

        func.Verify(x => x.Invoke(It.IsAny<IServiceProvider>(), It.IsAny<ICaptcha>()), Times.Once);
    }

    [Test]
    public void Create_With_HandlerFactory_Is_Correct()
    {
        Mock<Func<TestCaptchaHandler<ICaptcha, ISolution>>> handlerMock = new();
        handlerMock.Setup(x => x.Invoke()).Returns(new Mock<TestCaptchaHandler<ICaptcha, ISolution>>().Object);

        CaptchaHandlerDescriptor captchaHandlerDescriptor =
            CaptchaHandlerDescriptor.Create<ICaptcha, ISolution, TestCaptchaHandler<ICaptcha, ISolution>>(_ =>
                handlerMock.Object.Invoke());

        Assert.Multiple(() =>
        {
            Assert.That(captchaHandlerDescriptor.CaptchaType, Is.EqualTo(typeof(ICaptcha)));
            Assert.That(captchaHandlerDescriptor.SolutionType, Is.EqualTo(typeof(ISolution)));
            Assert.That(captchaHandlerDescriptor.HandlerType,
                Is.EqualTo(typeof(TestCaptchaHandler<ICaptcha, ISolution>)));
            Assert.Null(captchaHandlerDescriptor.SolverFunction);
            Assert.NotNull(captchaHandlerDescriptor.ImplementationFactory);
        });

        captchaHandlerDescriptor.ImplementationFactory!.Invoke(It.IsAny<IServiceProvider>());

        handlerMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Test]
    public void Create_With_HandlerFactory_Is_Interface_IsCorrect()
    {
        Assert.DoesNotThrow(() =>
            CaptchaHandlerDescriptor.Create<ICaptcha, ISolution, ICaptchaHandler<ICaptcha, ISolution>>(_ =>
                It.IsAny<ICaptchaHandler<ICaptcha, ISolution>>()));
    }

    [Test]
    public void Create_With_Handler_Is_Correct()
    {
        CaptchaHandlerDescriptor captchaHandlerDescriptor =
            CaptchaHandlerDescriptor.Create<TestCaptcha, TestSolution, TestCaptchaHandler<TestCaptcha, TestSolution>>();

        Assert.Multiple(() =>
        {
            Assert.That(captchaHandlerDescriptor.CaptchaType, Is.EqualTo(typeof(TestCaptcha)));
            Assert.That(captchaHandlerDescriptor.SolutionType, Is.EqualTo(typeof(TestSolution)));
            Assert.That(captchaHandlerDescriptor.HandlerType,
                Is.EqualTo(typeof(TestCaptchaHandler<TestCaptcha, TestSolution>)));
            Assert.Null(captchaHandlerDescriptor.SolverFunction);
            Assert.Null(captchaHandlerDescriptor.ImplementationFactory);
        });
    }

    [Test]
    public void Create_With_Handler_Is_Interface_Throws_ArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            CaptchaHandlerDescriptor.Create<ICaptcha, ISolution, ICaptchaHandler<ICaptcha, ISolution>>());
    }

    [Test]
    public void ToString_With_HandlerFunc()
    {
        Mock<Func<IServiceProvider, ICaptcha, Task<ISolution>>> func = new();

        CaptchaHandlerDescriptor captchaHandlerDescriptor =
            CaptchaHandlerDescriptor.Create<ICaptcha, ISolution>((provider, captcha) =>
                func.Object.Invoke(provider, captcha));

        string expected =
            $"{captchaHandlerDescriptor.CaptchaType}: {captchaHandlerDescriptor.SolutionType}. Has handler function.";
        string actual = captchaHandlerDescriptor.ToString();

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void ToString_With_HandlerFactory()
    {
        Mock<Func<TestCaptchaHandler<ICaptcha, ISolution>>> handlerMock = new();
        handlerMock.Setup(x => x.Invoke()).Returns(new Mock<TestCaptchaHandler<ICaptcha, ISolution>>().Object);

        CaptchaHandlerDescriptor captchaHandlerDescriptor =
            CaptchaHandlerDescriptor.Create<ICaptcha, ISolution, TestCaptchaHandler<ICaptcha, ISolution>>(_ =>
                handlerMock.Object.Invoke());

        string expected =
            $"{captchaHandlerDescriptor.CaptchaType}: {captchaHandlerDescriptor.SolutionType}, Handler: {captchaHandlerDescriptor.HandlerType}, " +
            $"has implementation factory";
        string actual = captchaHandlerDescriptor.ToString();

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void ToString_With_Handler()
    {
        CaptchaHandlerDescriptor captchaHandlerDescriptor =
            CaptchaHandlerDescriptor.Create<TestCaptcha, TestSolution, TestCaptchaHandler<TestCaptcha, TestSolution>>();

        string expected =
            $"{captchaHandlerDescriptor.CaptchaType}: {captchaHandlerDescriptor.SolutionType}, Handler: {captchaHandlerDescriptor.HandlerType}";
        string actual = captchaHandlerDescriptor.ToString();

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void CloneWithNewName_Is_Correct()
    {
        CaptchaHandlerDescriptor captchaHandlerDescriptor =
            CaptchaHandlerDescriptor.Create<TestCaptcha, TestSolution, TestCaptchaHandler<TestCaptcha, TestSolution>>();

        string expectedHandlerName = "handler-name";
        CaptchaHandlerDescriptor newCaptchaHandlerDescriptor =
            captchaHandlerDescriptor.CloneWithNewName(expectedHandlerName);

        Assert.That(newCaptchaHandlerDescriptor.HandlerName, Is.EqualTo(expectedHandlerName));
    }

    [Test]
    public void CloneWithNewName_When_HandlerName_Is_Null_Throws_ArgumentException()
    {
        CaptchaHandlerDescriptor captchaHandlerDescriptor =
            CaptchaHandlerDescriptor.Create<TestCaptcha, TestSolution, TestCaptchaHandler<TestCaptcha, TestSolution>>();

        string expectedHandlerName = null!;
        Assert.Throws<ArgumentException>(() =>
            captchaHandlerDescriptor.CloneWithNewName(expectedHandlerName));
    }

    [Test]
    public void CloneWithNewName_When_HandlerName_Is_WhiteSpace_Throws_ArgumentException()
    {
        CaptchaHandlerDescriptor captchaHandlerDescriptor =
            CaptchaHandlerDescriptor.Create<TestCaptcha, TestSolution, TestCaptchaHandler<TestCaptcha, TestSolution>>();

        string expectedHandlerName = "";
        Assert.Throws<ArgumentException>(() =>
            captchaHandlerDescriptor.CloneWithNewName(expectedHandlerName));
    }
}