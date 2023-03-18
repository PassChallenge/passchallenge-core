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
    public void Constructor_Is_Correct()
    {
        CaptchaHandlerFactory _ = new(new Mock<ICaptchaHandlerDescriptorStorage>().Object);
        Assert.Pass();
    }

    [Test]
    public void Constructor_Handlers_Is_Null_Throws_ArgumentNullException()
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentNullException>(() => new CaptchaHandlerFactory(null!));
    }

    [Test]
    public void CreateHandler_ServiceProvider_Is_Null_Throws_ArgumentNullException()
    {
        CaptchaHandlerFactory factory = new(new Mock<ICaptchaHandlerDescriptorStorage>().Object);
        Assert.Throws<ArgumentNullException>(() => factory.CreateHandler<ICaptcha, ISolution>(null!));
    }

    [Test]
    public async Task CreateHandler_Descriptor_With_HandlerFunction_Is_Correct()
    {
        Mock<IServiceProvider> mock = new();
        Mock<Func<Task<ISolution>>> funcMock = new();


        CaptchaHandlerDescriptor descriptor =
            CaptchaHandlerDescriptor.Create<ICaptcha, ISolution>((_, _) => funcMock.Object.Invoke());

        Mock<ICaptchaHandlerDescriptorStorage> handlerDescriptorMock = new();
        handlerDescriptorMock.Setup(x => x.GetDescriptor<ICaptcha, ISolution>(It.IsAny<string?>()))
            .Returns(descriptor);

        CaptchaHandlerFactory factory = new(handlerDescriptorMock.Object);

        ICaptchaHandler<ICaptcha, ISolution> handler = factory.CreateHandler<ICaptcha, ISolution>(mock.Object);
        await handler.Handle(It.IsAny<ICaptcha>());

        Assert.IsInstanceOf<FixedCaptchaHandler<ICaptcha, ISolution>>(handler);
        funcMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Test]
    public void CreateHandler_Descriptor_With_HandlerFactory_Is_Correct()
    {
        Mock<IServiceProvider> mock = new();
        Mock<TestCaptchaHandler<ICaptcha, ISolution>> handlerMock = new();

        CaptchaHandlerDescriptor descriptor =
            CaptchaHandlerDescriptor.Create<ICaptcha, ISolution, TestCaptchaHandler<ICaptcha, ISolution>>(_ =>
                handlerMock.Object);

        Mock<ICaptchaHandlerDescriptorStorage> handlerDescriptorMock = new();
        handlerDescriptorMock.Setup(x => x.GetDescriptor<ICaptcha, ISolution>(It.IsAny<string?>()))
            .Returns(descriptor);

        CaptchaHandlerFactory factory = new(handlerDescriptorMock.Object);

        ICaptchaHandler<ICaptcha, ISolution> handler = factory.CreateHandler<ICaptcha, ISolution>(mock.Object);

        Assert.That(handler, Is.EqualTo(handlerMock.Object));
    }

    [Test]
    public void CreateHandler_Descriptor_With_Handler_Is_Correct()
    {
        Mock<IServiceProvider> mock = new();

        CaptchaHandlerDescriptor descriptor =
            CaptchaHandlerDescriptor.Create<TestCaptcha, TestSolution, TestCaptchaHandler<TestCaptcha, TestSolution>>();

        Mock<ICaptchaHandlerDescriptorStorage> handlerDescriptorMock = new();
        handlerDescriptorMock.Setup(x => x.GetDescriptor<TestCaptcha, TestSolution>(It.IsAny<string?>()))
            .Returns(descriptor);

        CaptchaHandlerFactory factory = new(handlerDescriptorMock.Object);

        ICaptchaHandler<TestCaptcha, TestSolution> handler =
            factory.CreateHandler<TestCaptcha, TestSolution>(mock.Object);

        Assert.IsInstanceOf<ICaptchaHandler<TestCaptcha, TestSolution>>(handler);
        Assert.IsInstanceOf<TestCaptchaHandler<TestCaptcha, TestSolution>>(handler);
    }

    [Test]
    public void CreateHandler_Descriptor_With_Parameterized_Handler_Is_Correct()
    {
        Mock<IServiceProvider> mock = new();
        mock.Setup(x => x.GetService(It.Is<Type>(type => type == typeof(object)))).Returns(new object());

        CaptchaHandlerDescriptor descriptor =
            CaptchaHandlerDescriptor.Create<TestCaptcha, TestSolution, TestCaptchaHandlerWithOneArgument>();

        Mock<ICaptchaHandlerDescriptorStorage> handlerDescriptorMock = new();
        handlerDescriptorMock.Setup(x => x.GetDescriptor<TestCaptcha, TestSolution>(It.IsAny<string?>()))
            .Returns(descriptor);

        CaptchaHandlerFactory factory = new(handlerDescriptorMock.Object);

        ICaptchaHandler<TestCaptcha, TestSolution> handler =
            factory.CreateHandler<TestCaptcha, TestSolution>(mock.Object);

        Assert.IsInstanceOf<ICaptchaHandler<TestCaptcha, TestSolution>>(handler);
        Assert.IsInstanceOf<TestCaptchaHandlerWithOneArgument>(handler);
        mock.Verify(x => x.GetService(It.Is<Type>(type => type == typeof(object))), Times.Once);
    }

    [Test]
    public void GetDefaultHandlerName_Is_Correct()
    {
        Mock<ICaptchaHandlerDescriptorStorage> handlerDescriptorMock = new();
        handlerDescriptorMock.Setup(x => x.GetDefaultDescriptorName<ICaptcha, ISolution>())
            .Returns(It.IsAny<string>());

        CaptchaHandlerFactory factory = new(handlerDescriptorMock.Object);

        factory.GetDefaultHandlerName<ICaptcha, ISolution>();

        handlerDescriptorMock.Verify(x => x.GetDefaultDescriptorName<ICaptcha, ISolution>(), Times.Once);
    }

    [Test]
    public void GetHandlerNames_Is_Correct()
    {
        Mock<ICaptchaHandlerDescriptorStorage> handlerDescriptorMock = new();
        List<string> expectedHandlerNames = new()
        {
            "handler-name-1",
            "handler-name-2"
        };

        handlerDescriptorMock.Setup(x => x.GetDescriptors<ICaptcha, ISolution>())
            .Returns(new List<CaptchaHandlerDescriptor>()
            {
                CaptchaHandlerDescriptor.Create<ICaptcha, ISolution>((_, _) => Task.FromResult(It.IsAny<ISolution>()),
                    expectedHandlerNames[0]),
                CaptchaHandlerDescriptor.Create<ICaptcha, ISolution>((_, _) => Task.FromResult(It.IsAny<ISolution>()),
                    expectedHandlerNames[1]),

            });

        CaptchaHandlerFactory factory = new(handlerDescriptorMock.Object);

        IReadOnlyCollection<string> actualHandlerNames = factory.GetHandlerNames<ICaptcha, ISolution>();

        handlerDescriptorMock.Verify(x => x.GetDescriptors<ICaptcha, ISolution>(), Times.Once);
        Assert.That(actualHandlerNames, Is.EquivalentTo(expectedHandlerNames));
    }

    [Test]
    public void CanProduce_Is_Correct()
    {
        string expectedHandlerName = "handler-name";

        Mock<ICaptchaHandlerDescriptorStorage> handlerDescriptorMock = new();
        handlerDescriptorMock.Setup(x => x.ContainsDescriptor<ICaptcha, ISolution>(It.IsAny<string>()))
            .Returns(true);

        CaptchaHandlerFactory factory = new(handlerDescriptorMock.Object);

        factory.CanProduce<ICaptcha, ISolution>(expectedHandlerName);

        handlerDescriptorMock.Verify(
            x => x.ContainsDescriptor<ICaptcha, ISolution>(It.Is<string>(mo => mo == expectedHandlerName)), Times.Once);
    }
}