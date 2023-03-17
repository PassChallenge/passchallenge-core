using KillDNS.CaptchaSolver.Core.Extensions;
using KillDNS.CaptchaSolver.Core.Handlers;
using KillDNS.CaptchaSolver.Core.Solver;
using KillDNS.CaptchaSolver.Core.Tests.Tools;
using Moq;
using NUnit.Framework;

namespace KillDNS.CaptchaSolver.Core.Tests.Extensions;

public class CaptchaSolverBuilderExtensionsTests
{
    [Test]
    public void SetCaptchaHandlerFactory_Is_Correct()
    {
        IServiceProvider serviceProvider = new Mock<IServiceProvider>().Object;

        CaptchaSolverBuilder<TestProducerWithCaptchaHandlerFactory> builder = new();
        builder.SetCaptchaHandlerFactory(new Mock<ICaptchaHandlerFactory>().Object);
        builder.Build(serviceProvider);

        Assert.Pass();
    }

    [Test]
    public void SetCaptchaHandlerFactory_When_Builder_Is_Null_Throw_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            CaptchaSolverBuilderExtensions.SetCaptchaHandlerFactory<TestProducerWithCaptchaHandlerFactory>(null!,
                new Mock<ICaptchaHandlerFactory>().Object));
    }

    [Test]
    public void SetCaptchaHandlerFactory_When_CaptchaHandlerFactory_Is_Null_Throw_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new Mock<CaptchaSolverBuilder<TestProducerWithCaptchaHandlerFactory>>().Object
                .SetCaptchaHandlerFactory(null!));
    }

    [Test]
    public void SetCaptchaHandlerFactory_Returns_Same_Builder()
    {
        Mock<CaptchaSolverBuilder<TestProducerWithCaptchaHandlerFactory>> mockedBuilder = new();
        CaptchaSolverBuilder<TestProducerWithCaptchaHandlerFactory> builder = mockedBuilder.Object;

        CaptchaSolverBuilder<TestProducerWithCaptchaHandlerFactory> returnedValue =
            builder.SetCaptchaHandlerFactory(new Mock<ICaptchaHandlerFactory>().Object);

        Assert.That(returnedValue, Is.EqualTo(builder));
    }
}