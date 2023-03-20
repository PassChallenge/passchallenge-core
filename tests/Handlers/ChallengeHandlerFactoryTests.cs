using PassChallenge.Core.Handlers;
using PassChallenge.Core.Solutions;
using PassChallenge.Core.Tests.Tools;
using Moq;
using NUnit.Framework;
using PassChallenge.Core.Challenges;

namespace PassChallenge.Core.Tests.Handlers;

public class ChallengeHandlerFactoryTests
{
    [Test]
    public void Constructor_Is_Correct()
    {
        ChallengeHandlerFactory _ = new(new Mock<IChallengeHandlerDescriptorStorage>().Object);
        Assert.Pass();
    }

    [Test]
    public void Constructor_Handlers_Is_Null_Throws_ArgumentNullException()
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentNullException>(() => new ChallengeHandlerFactory(null!));
    }

    [Test]
    public void CreateHandler_ServiceProvider_Is_Null_Throws_ArgumentNullException()
    {
        ChallengeHandlerFactory factory = new(new Mock<IChallengeHandlerDescriptorStorage>().Object);
        Assert.Throws<ArgumentNullException>(() => factory.CreateHandler<IChallenge, ISolution>(null!));
    }

    [Test]
    public async Task CreateHandler_Descriptor_With_HandlerFunction_Is_Correct()
    {
        Mock<IServiceProvider> mock = new();
        Mock<Func<Task<ISolution>>> funcMock = new();


        ChallengeHandlerDescriptor descriptor =
            ChallengeHandlerDescriptor.Create<IChallenge, ISolution>((_, _) => funcMock.Object.Invoke());

        Mock<IChallengeHandlerDescriptorStorage> handlerDescriptorMock = new();
        handlerDescriptorMock.Setup(x => x.GetDescriptor<IChallenge, ISolution>(It.IsAny<string?>()))
            .Returns(descriptor);

        ChallengeHandlerFactory factory = new(handlerDescriptorMock.Object);

        IChallengeHandler<IChallenge, ISolution> handler = factory.CreateHandler<IChallenge, ISolution>(mock.Object);
        await handler.Handle(It.IsAny<IChallenge>());

        Assert.IsInstanceOf<FixedChallengeHandler<IChallenge, ISolution>>(handler);
        funcMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Test]
    public void CreateHandler_Descriptor_With_HandlerFactory_Is_Correct()
    {
        Mock<IServiceProvider> mock = new();
        Mock<TestChallengeHandler<IChallenge, ISolution>> handlerMock = new();

        ChallengeHandlerDescriptor descriptor =
            ChallengeHandlerDescriptor.Create<IChallenge, ISolution, TestChallengeHandler<IChallenge, ISolution>>(_ =>
                handlerMock.Object);

        Mock<IChallengeHandlerDescriptorStorage> handlerDescriptorMock = new();
        handlerDescriptorMock.Setup(x => x.GetDescriptor<IChallenge, ISolution>(It.IsAny<string?>()))
            .Returns(descriptor);

        ChallengeHandlerFactory factory = new(handlerDescriptorMock.Object);

        IChallengeHandler<IChallenge, ISolution> handler = factory.CreateHandler<IChallenge, ISolution>(mock.Object);

        Assert.That(handler, Is.EqualTo(handlerMock.Object));
    }

    [Test]
    public void CreateHandler_Descriptor_With_Handler_Is_Correct()
    {
        Mock<IServiceProvider> mock = new();

        ChallengeHandlerDescriptor descriptor =
            ChallengeHandlerDescriptor.Create<TestChallenge, TestSolution, TestChallengeHandler<TestChallenge, TestSolution>>();

        Mock<IChallengeHandlerDescriptorStorage> handlerDescriptorMock = new();
        handlerDescriptorMock.Setup(x => x.GetDescriptor<TestChallenge, TestSolution>(It.IsAny<string?>()))
            .Returns(descriptor);

        ChallengeHandlerFactory factory = new(handlerDescriptorMock.Object);

        IChallengeHandler<TestChallenge, TestSolution> handler =
            factory.CreateHandler<TestChallenge, TestSolution>(mock.Object);

        Assert.IsInstanceOf<IChallengeHandler<TestChallenge, TestSolution>>(handler);
        Assert.IsInstanceOf<TestChallengeHandler<TestChallenge, TestSolution>>(handler);
    }

    [Test]
    public void CreateHandler_Descriptor_With_Parameterized_Handler_Is_Correct()
    {
        Mock<IServiceProvider> mock = new();
        mock.Setup(x => x.GetService(It.Is<Type>(type => type == typeof(object)))).Returns(new object());

        ChallengeHandlerDescriptor descriptor =
            ChallengeHandlerDescriptor.Create<TestChallenge, TestSolution, TestChallengeHandlerWithOneArgument>();

        Mock<IChallengeHandlerDescriptorStorage> handlerDescriptorMock = new();
        handlerDescriptorMock.Setup(x => x.GetDescriptor<TestChallenge, TestSolution>(It.IsAny<string?>()))
            .Returns(descriptor);

        ChallengeHandlerFactory factory = new(handlerDescriptorMock.Object);

        IChallengeHandler<TestChallenge, TestSolution> handler =
            factory.CreateHandler<TestChallenge, TestSolution>(mock.Object);

        Assert.IsInstanceOf<IChallengeHandler<TestChallenge, TestSolution>>(handler);
        Assert.IsInstanceOf<TestChallengeHandlerWithOneArgument>(handler);
        mock.Verify(x => x.GetService(It.Is<Type>(type => type == typeof(object))), Times.Once);
    }

    [Test]
    public void GetDefaultHandlerName_Is_Correct()
    {
        Mock<IChallengeHandlerDescriptorStorage> handlerDescriptorMock = new();
        handlerDescriptorMock.Setup(x => x.GetDefaultDescriptorName<IChallenge, ISolution>())
            .Returns(It.IsAny<string>());

        ChallengeHandlerFactory factory = new(handlerDescriptorMock.Object);

        factory.GetDefaultHandlerName<IChallenge, ISolution>();

        handlerDescriptorMock.Verify(x => x.GetDefaultDescriptorName<IChallenge, ISolution>(), Times.Once);
    }

    [Test]
    public void GetHandlerNames_Is_Correct()
    {
        Mock<IChallengeHandlerDescriptorStorage> handlerDescriptorMock = new();
        List<string> expectedHandlerNames = new()
        {
            "handler-name-1",
            "handler-name-2"
        };

        handlerDescriptorMock.Setup(x => x.GetDescriptors<IChallenge, ISolution>())
            .Returns(new List<ChallengeHandlerDescriptor>()
            {
                ChallengeHandlerDescriptor.Create<IChallenge, ISolution>((_, _) => Task.FromResult(It.IsAny<ISolution>()),
                    expectedHandlerNames[0]),
                ChallengeHandlerDescriptor.Create<IChallenge, ISolution>((_, _) => Task.FromResult(It.IsAny<ISolution>()),
                    expectedHandlerNames[1]),

            });

        ChallengeHandlerFactory factory = new(handlerDescriptorMock.Object);

        IReadOnlyCollection<string> actualHandlerNames = factory.GetHandlerNames<IChallenge, ISolution>();

        handlerDescriptorMock.Verify(x => x.GetDescriptors<IChallenge, ISolution>(), Times.Once);
        Assert.That(actualHandlerNames, Is.EquivalentTo(expectedHandlerNames));
    }

    [Test]
    public void CanProduce_Is_Correct()
    {
        string expectedHandlerName = "handler-name";

        Mock<IChallengeHandlerDescriptorStorage> handlerDescriptorMock = new();
        handlerDescriptorMock.Setup(x => x.ContainsDescriptor<IChallenge, ISolution>(It.IsAny<string>()))
            .Returns(true);

        ChallengeHandlerFactory factory = new(handlerDescriptorMock.Object);

        factory.CanProduce<IChallenge, ISolution>(expectedHandlerName);

        handlerDescriptorMock.Verify(
            x => x.ContainsDescriptor<IChallenge, ISolution>(It.Is<string>(mo => mo == expectedHandlerName)), Times.Once);
    }
}