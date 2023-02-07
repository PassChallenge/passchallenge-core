using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Handlers;
using KillDNS.CaptchaSolver.Core.Producer;
using KillDNS.CaptchaSolver.Core.Solutions;
using KillDNS.CaptchaSolver.Core.Solver;
using KillDNS.CaptchaSolver.Core.Tests.Tools;
using Moq;
using NUnit.Framework;

namespace KillDNS.CaptchaSolver.Core.Tests.Solver;

public class CaptchaSolverSpecifiedBuilderTests
{
    [Test]
    public void AddSupportCaptchaAndSolution_Generics_Is_Correct()
    {
        Mock<IServiceProvider> serviceProviderMock = new();
        CaptchaSolverSpecifiedBuilder<TestSpecifiedProducer> builder = new();
        builder.AddSupportCaptchaAndSolution<ICaptcha, ISolution>();
        IProducerWithSpecifiedCaptchaAndSolutions producer = builder.Build(serviceProviderMock.Object);

        Assert.That(producer.CanProduce<ICaptcha, ISolution>(), Is.True);
    }

    [Test]
    public void AddSupportCaptchaAndSolution_CaptchaHandlerDescriptor_Is_Correct()
    {
        Mock<IServiceProvider> serviceProviderMock = new();
        CaptchaSolverSpecifiedBuilder<TestSpecifiedProducer> builder = new();
        builder.AddSupportCaptchaAndSolution(
            CaptchaHandlerDescriptor.Create<ICaptcha, ISolution>((_, _) => It.IsAny<Task<ISolution>>()));

        IProducerWithSpecifiedCaptchaAndSolutions producer = builder.Build(serviceProviderMock.Object);

        Assert.That(producer.CanProduce<ICaptcha, ISolution>(), Is.True);
    }

    [Test]
    public void
        AddSupportCaptchaAndSolution_CaptchaHandlerDescriptor_When_Descriptor_Is_Null_Throws_ArgumentNullException()
    {
        CaptchaSolverSpecifiedBuilder<TestSpecifiedProducer> builder = new();
        Assert.Throws<ArgumentNullException>(() => builder.AddSupportCaptchaAndSolution(null!));
    }
}