using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Handlers;
using KillDNS.CaptchaSolver.Core.Solutions;
using KillDNS.CaptchaSolver.Core.Tests.Tools;
using Moq;
using NUnit.Framework;

namespace KillDNS.CaptchaSolver.Core.Tests.Handlers;

public class CaptchaHandlerFactoryTests
{
    [Test]
    public void CaptchaHandlerFactory_Constructor_Is_Correct()
    {
        CaptchaHandlerFactory _ = new(Array.Empty<CaptchaHandlerDescriptor>());
        Assert.Pass();
    }

    [Test]
    public void CaptchaHandlerFactory_Constructor_Handlers_Is_Null_Throws_ArgumentNullException()
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentNullException>(() => new CaptchaHandlerFactory(null!));
    }

    [Test]
    public void CreateHandler_ServiceProvider_Is_Null_Throws_ArgumentNullException()
    {
        CaptchaHandlerFactory factory = new(Array.Empty<CaptchaHandlerDescriptor>());
        Assert.Throws<ArgumentNullException>(() => factory.CreateHandler<ICaptcha, ISolution>(null!));
    }

    [Test]
    public void CreateHandler_HandlerTypes_Not_Contains_Captcha_Pair_Throws_InvalidOperationException()
    {
        Mock<IServiceProvider> mock = new();
        CaptchaHandlerFactory factory = new(Array.Empty<CaptchaHandlerDescriptor>());
        Assert.Throws<InvalidOperationException>(() => factory.CreateHandler<ICaptcha, ISolution>(mock.Object));
    }

    [Test]
    public async Task CreateHandler_Descriptor_With_HandlerFunction_Is_Correct()
    {
        Mock<IServiceProvider> mock = new();
        Mock<Func<Task<ISolution>>> funcMock = new();


        CaptchaHandlerDescriptor descriptor =
            CaptchaHandlerDescriptor.Create<ICaptcha, ISolution>((_, _) => funcMock.Object.Invoke());

        CaptchaHandlerFactory factory = new(new[] { descriptor });

        ICaptchaHandler<ICaptcha, ISolution> handler = factory.CreateHandler<ICaptcha, ISolution>(mock.Object);
        ISolution solution = await handler.Handle(It.IsAny<ICaptcha>());

        Assert.IsInstanceOf<FixedCaptchaHandler<ICaptcha, ISolution>>(handler);
        funcMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Test]
    public void CreateHandler_Descriptor_With_HandlerFactory_Is_Correct()
    {
        Mock<IServiceProvider> mock = new();
        Mock<ICaptchaHandler<ICaptcha, ISolution>> handlerMock = new();

        CaptchaHandlerDescriptor descriptor =
            CaptchaHandlerDescriptor.Create<ICaptcha, ISolution, ICaptchaHandler<ICaptcha, ISolution>>(_ =>
                handlerMock.Object);

        CaptchaHandlerFactory factory = new(new[] { descriptor });

        ICaptchaHandler<ICaptcha, ISolution> handler = factory.CreateHandler<ICaptcha, ISolution>(mock.Object);

        Assert.That(handler, Is.EqualTo(handlerMock.Object));
    }

    [Test]
    public void CreateHandler_Descriptor_With_Handler_Is_Correct()
    {
        Mock<IServiceProvider> mock = new();

        CaptchaHandlerDescriptor descriptor =
            CaptchaHandlerDescriptor.Create<TestCaptcha, TestSolution, TestCaptchaHandler>();

        CaptchaHandlerFactory factory = new(new[] { descriptor });

        ICaptchaHandler<TestCaptcha, TestSolution> handler =
            factory.CreateHandler<TestCaptcha, TestSolution>(mock.Object);

        Assert.IsInstanceOf<ICaptchaHandler<TestCaptcha, TestSolution>>(handler);
        Assert.IsInstanceOf<TestCaptchaHandler>(handler);
    }

    [Test]
    public void CreateHandler_Descriptor_With_Parameterized_Handler_Is_Correct()
    {
        Mock<IServiceProvider> mock = new();
        mock.Setup(x => x.GetService(It.Is<Type>(type => type == typeof(object)))).Returns(new object());

        CaptchaHandlerDescriptor descriptor =
            CaptchaHandlerDescriptor.Create<TestCaptcha, TestSolution, TestCaptchaHandlerWithOneArgument>();

        CaptchaHandlerFactory factory = new(new[] { descriptor });

        ICaptchaHandler<TestCaptcha, TestSolution> handler =
            factory.CreateHandler<TestCaptcha, TestSolution>(mock.Object);

        Assert.IsInstanceOf<ICaptchaHandler<TestCaptcha, TestSolution>>(handler);
        Assert.IsInstanceOf<TestCaptchaHandlerWithOneArgument>(handler);
        mock.Verify(x => x.GetService(It.Is<Type>(type => type == typeof(object))), Times.Once);
    }
}