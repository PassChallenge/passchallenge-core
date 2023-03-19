using PassChallenge.Core.Captcha;
using PassChallenge.Core.Handlers;
using PassChallenge.Core.Solutions;
using PassChallenge.Core.Solver;
using Moq;
using NUnit.Framework;

namespace PassChallenge.Core.Tests.Solver;

public class AvailableCaptchaAndSolutionStorageBuilderTests
{
    [Test]
    public void AddSupportCaptchaAndSolution_Generics_Is_Correct()
    {
        AvailableCaptchaAndSolutionStorageBuilder builder = new();
        builder.AddSupportCaptchaAndSolution<ICaptcha, ISolution>();

        IAvailableCaptchaAndSolutionStorage availableCaptchaAndSolutionStorage = builder.Build();

        Assert.That(availableCaptchaAndSolutionStorage.IsAvailable<ICaptcha, ISolution>(), Is.True);
    }

    [Test]
    public void AddSupportCaptchaAndSolution_Generics_With_HandlerName_Is_Correct()
    {
        AvailableCaptchaAndSolutionStorageBuilder builder = new();
        string handlerName = "handler-name";

        builder.AddSupportCaptchaAndSolution<ICaptcha, ISolution>(handlerName);

        IAvailableCaptchaAndSolutionStorage availableCaptchaAndSolutionStorage = builder.Build();

        Assert.That(availableCaptchaAndSolutionStorage.IsAvailable<ICaptcha, ISolution>(handlerName), Is.True);
    }

    [Test]
    public void
        AddSupportCaptchaAndSolution_Generics_When_Add_Same_HandlerName_Captcha_And_Solution_Throws_InvalidOperationException()
    {
        AvailableCaptchaAndSolutionStorageBuilder builder = new();
        string handlerName = "handler-name";
        builder.AddSupportCaptchaAndSolution<ICaptcha, ISolution>(handlerName);

        Assert.Throws<InvalidOperationException>(() =>
            builder.AddSupportCaptchaAndSolution<ICaptcha, ISolution>(handlerName));
    }

    [Test]
    public void
        AddSupportCaptchaAndSolution_CaptchaHandlerDescriptor_When_Descriptor_Is_Null_Throws_ArgumentNullException()
    {
        AvailableCaptchaAndSolutionStorageBuilder builder = new();

        Assert.Throws<ArgumentNullException>(() =>
            builder.AddSupportCaptchaAndSolution(null!));
    }

    [Test]
    public void AddSupportCaptchaAndSolution_CaptchaHandlerDescriptor_Is_Correct()
    {
        AvailableCaptchaAndSolutionStorageBuilder builder = new();
        builder.AddSupportCaptchaAndSolution(
            CaptchaHandlerDescriptor.Create<ICaptcha, ISolution>((_, _) => Task.FromResult(It.IsAny<ISolution>()),
                "handler"));

        IAvailableCaptchaAndSolutionStorage availableCaptchaAndSolutionStorage = builder.Build();

        Assert.That(availableCaptchaAndSolutionStorage.IsAvailable<ICaptcha, ISolution>(), Is.True);
    }

    [Test]
    public void AddSupportCaptchaAndSolution_CaptchaHandlerDescriptor_With_HandlerName_Is_Correct()
    {
        AvailableCaptchaAndSolutionStorageBuilder builder = new();
        string handlerName = "handler-name";
        builder.AddSupportCaptchaAndSolution(
            CaptchaHandlerDescriptor.Create<ICaptcha, ISolution>((_, _) => Task.FromResult(It.IsAny<ISolution>()),
                handlerName));

        IAvailableCaptchaAndSolutionStorage availableCaptchaAndSolutionStorage = builder.Build();

        Assert.That(availableCaptchaAndSolutionStorage.IsAvailable<ICaptcha, ISolution>(handlerName), Is.True);
    }

    [Test]
    public void SetStorage_Is_Correct()
    {
        Mock<IAvailableCaptchaAndSolutionStorage> mock = new();

        AvailableCaptchaAndSolutionStorageBuilder builder = new();
        builder.SetStorage(mock.Object);

        IAvailableCaptchaAndSolutionStorage availableCaptchaAndSolutionStorage = builder.Build();

        Assert.That(availableCaptchaAndSolutionStorage, Is.EqualTo(mock.Object));
    }
}