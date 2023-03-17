using KillDNS.CaptchaSolver.Core.Solver;
using KillDNS.CaptchaSolver.Core.Tests.Tools;
using Moq;
using NUnit.Framework;

namespace KillDNS.CaptchaSolver.Core.Tests.Solver;

public class CaptchaSolverSpecifiedBuilderTests
{
    [Test]
    public void Build_Call_Producer_SetAvailableCaptchaAndSolutionStorage()
    {
        Mock<IServiceProvider> serviceProviderMock = new();

        CaptchaSolverSpecifiedBuilder<TestSpecifiedProducer> builder = new();
        TestSpecifiedProducer producer = builder.Build(serviceProviderMock.Object);

        Assert.IsNotNull(producer.AvailableCaptchaAndSolutionStorage);
    }
}