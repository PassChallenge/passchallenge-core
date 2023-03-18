using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Producer;
using KillDNS.CaptchaSolver.Core.Solutions;
using KillDNS.CaptchaSolver.Core.Solver;
using KillDNS.CaptchaSolver.Core.Tests.Tools;
using Moq;
using NUnit.Framework;

namespace KillDNS.CaptchaSolver.Core.Tests.Producer;

public class BaseProducerTests
{
    [Test]
    public void SetAvailableCaptchaAndSolutionStorage_CanProduce_Is_Correct()
    {
        TestBaseProducer producer = new();
        producer.SetAvailableCaptchaAndSolutionStorage(new AvailableCaptchaAndSolutionStorage(
            new Dictionary<Type, Dictionary<Type, HashSet<string>>>()
            {
                {
                    typeof(TestCaptcha), new Dictionary<Type, HashSet<string>>()
                    {
                        { typeof(TestSolution), new HashSet<string>() }
                    }
                }
            })
        );

        Assert.Multiple(() =>
        {
            Assert.That(producer.CanProduce<TestCaptcha, TestSolution>(), Is.True);
            Assert.That(producer.CanProduce<ICaptcha, ISolution>(), Is.False);
        });
    }

    [Test]
    public void
        SetAvailableCaptchaAndSolutionStorage_When_AvailableCaptchaAndSolutionStorage_Is_Null_Throws_ArgumentNullException()
    {
        TestBaseProducer producer = new();
        Assert.Throws<ArgumentNullException>(() => producer.SetAvailableCaptchaAndSolutionStorage(null!));
    }

    [Test]
    public void ProduceAndWaitSolution_Is_Correct()
    {
        Mock<BaseProducer> mock = new();

        ICaptcha expectedCaptcha = It.IsAny<ICaptcha>();
        CancellationToken expectedCancellationToken = new CancellationTokenSource(TimeSpan.FromHours(1)).Token;

        mock.Object.ProduceAndWaitSolution<ICaptcha, ISolution>(expectedCaptcha, It.IsAny<string>(),
            expectedCancellationToken);
        mock.Verify(x =>
            x.ProduceAndWaitSolution<ICaptcha, ISolution>(It.Is<ICaptcha>(mo => mo == expectedCaptcha),
                It.Is<string>(mo => mo == default),
                It.Is<CancellationToken>(mo => mo == expectedCancellationToken)));
    }
}