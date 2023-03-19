using PassChallenge.Core.Handlers;
using PassChallenge.Core.Solutions;
using PassChallenge.Core.Tests.Tools;
using Moq;
using NUnit.Framework;
using PassChallenge.Core.Challenges;

namespace PassChallenge.Core.Tests.Handlers;

public class ChallengeHandlerDescriptorTests
{
    [Test]
    public void Create_With_HandlerFunc_Is_Correct()
    {
        Mock<Func<IServiceProvider, IChallenge, Task<ISolution>>> func = new();

        ChallengeHandlerDescriptor challengeHandlerDescriptor =
            ChallengeHandlerDescriptor.Create<IChallenge, ISolution>((provider, challenge) =>
                func.Object.Invoke(provider, challenge));

        Assert.Multiple(() =>
        {
            Assert.That(challengeHandlerDescriptor.ChallengeType, Is.EqualTo(typeof(IChallenge)));
            Assert.That(challengeHandlerDescriptor.SolutionType, Is.EqualTo(typeof(ISolution)));
            Assert.Null(challengeHandlerDescriptor.HandlerType);
            Assert.Null(challengeHandlerDescriptor.ImplementationFactory);
            Assert.NotNull(challengeHandlerDescriptor.SolverFunction);
        });

        challengeHandlerDescriptor.SolverFunction!.Invoke(new Mock<IServiceProvider>().Object, It.IsAny<IChallenge>());

        func.Verify(x => x.Invoke(It.IsAny<IServiceProvider>(), It.IsAny<IChallenge>()), Times.Once);
    }

    [Test]
    public void Create_With_HandlerFactory_Is_Correct()
    {
        Mock<Func<TestChallengeHandler<IChallenge, ISolution>>> handlerMock = new();
        handlerMock.Setup(x => x.Invoke()).Returns(new Mock<TestChallengeHandler<IChallenge, ISolution>>().Object);

        ChallengeHandlerDescriptor challengeHandlerDescriptor =
            ChallengeHandlerDescriptor.Create<IChallenge, ISolution, TestChallengeHandler<IChallenge, ISolution>>(_ =>
                handlerMock.Object.Invoke());

        Assert.Multiple(() =>
        {
            Assert.That(challengeHandlerDescriptor.ChallengeType, Is.EqualTo(typeof(IChallenge)));
            Assert.That(challengeHandlerDescriptor.SolutionType, Is.EqualTo(typeof(ISolution)));
            Assert.That(challengeHandlerDescriptor.HandlerType,
                Is.EqualTo(typeof(TestChallengeHandler<IChallenge, ISolution>)));
            Assert.Null(challengeHandlerDescriptor.SolverFunction);
            Assert.NotNull(challengeHandlerDescriptor.ImplementationFactory);
        });

        challengeHandlerDescriptor.ImplementationFactory!.Invoke(It.IsAny<IServiceProvider>());

        handlerMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Test]
    public void Create_With_HandlerFactory_Is_Interface_IsCorrect()
    {
        Assert.DoesNotThrow(() =>
            ChallengeHandlerDescriptor.Create<IChallenge, ISolution, IChallengeHandler<IChallenge, ISolution>>(_ =>
                It.IsAny<IChallengeHandler<IChallenge, ISolution>>()));
    }

    [Test]
    public void Create_With_Handler_Is_Correct()
    {
        ChallengeHandlerDescriptor challengeHandlerDescriptor =
            ChallengeHandlerDescriptor.Create<TestChallenge, TestSolution, TestChallengeHandler<TestChallenge, TestSolution>>();

        Assert.Multiple(() =>
        {
            Assert.That(challengeHandlerDescriptor.ChallengeType, Is.EqualTo(typeof(TestChallenge)));
            Assert.That(challengeHandlerDescriptor.SolutionType, Is.EqualTo(typeof(TestSolution)));
            Assert.That(challengeHandlerDescriptor.HandlerType,
                Is.EqualTo(typeof(TestChallengeHandler<TestChallenge, TestSolution>)));
            Assert.Null(challengeHandlerDescriptor.SolverFunction);
            Assert.Null(challengeHandlerDescriptor.ImplementationFactory);
        });
    }

    [Test]
    public void Create_With_Handler_Is_Interface_Throws_ArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            ChallengeHandlerDescriptor.Create<IChallenge, ISolution, IChallengeHandler<IChallenge, ISolution>>());
    }

    [Test]
    public void ToString_With_HandlerFunc()
    {
        Mock<Func<IServiceProvider, IChallenge, Task<ISolution>>> func = new();

        ChallengeHandlerDescriptor challengeHandlerDescriptor =
            ChallengeHandlerDescriptor.Create<IChallenge, ISolution>((provider, challenge) =>
                func.Object.Invoke(provider, challenge));

        string expected =
            $"{challengeHandlerDescriptor.ChallengeType}: {challengeHandlerDescriptor.SolutionType}. Has handler function.";
        string actual = challengeHandlerDescriptor.ToString();

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void ToString_With_HandlerFactory()
    {
        Mock<Func<TestChallengeHandler<IChallenge, ISolution>>> handlerMock = new();
        handlerMock.Setup(x => x.Invoke()).Returns(new Mock<TestChallengeHandler<IChallenge, ISolution>>().Object);

        ChallengeHandlerDescriptor challengeHandlerDescriptor =
            ChallengeHandlerDescriptor.Create<IChallenge, ISolution, TestChallengeHandler<IChallenge, ISolution>>(_ =>
                handlerMock.Object.Invoke());

        string expected =
            $"{challengeHandlerDescriptor.ChallengeType}: {challengeHandlerDescriptor.SolutionType}, Handler: {challengeHandlerDescriptor.HandlerType}, " +
            $"has implementation factory";
        string actual = challengeHandlerDescriptor.ToString();

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void ToString_With_Handler()
    {
        ChallengeHandlerDescriptor challengeHandlerDescriptor =
            ChallengeHandlerDescriptor.Create<TestChallenge, TestSolution, TestChallengeHandler<TestChallenge, TestSolution>>();

        string expected =
            $"{challengeHandlerDescriptor.ChallengeType}: {challengeHandlerDescriptor.SolutionType}, Handler: {challengeHandlerDescriptor.HandlerType}";
        string actual = challengeHandlerDescriptor.ToString();

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void CloneWithNewName_Is_Correct()
    {
        ChallengeHandlerDescriptor challengeHandlerDescriptor =
            ChallengeHandlerDescriptor.Create<TestChallenge, TestSolution, TestChallengeHandler<TestChallenge, TestSolution>>();

        string expectedHandlerName = "handler-name";
        ChallengeHandlerDescriptor newChallengeHandlerDescriptor =
            challengeHandlerDescriptor.CloneWithNewName(expectedHandlerName);

        Assert.That(newChallengeHandlerDescriptor.HandlerName, Is.EqualTo(expectedHandlerName));
    }

    [Test]
    public void CloneWithNewName_When_HandlerName_Is_Null_Throws_ArgumentException()
    {
        ChallengeHandlerDescriptor challengeHandlerDescriptor =
            ChallengeHandlerDescriptor.Create<TestChallenge, TestSolution, TestChallengeHandler<TestChallenge, TestSolution>>();

        string expectedHandlerName = null!;
        Assert.Throws<ArgumentException>(() =>
            challengeHandlerDescriptor.CloneWithNewName(expectedHandlerName));
    }

    [Test]
    public void CloneWithNewName_When_HandlerName_Is_WhiteSpace_Throws_ArgumentException()
    {
        ChallengeHandlerDescriptor challengeHandlerDescriptor =
            ChallengeHandlerDescriptor.Create<TestChallenge, TestSolution, TestChallengeHandler<TestChallenge, TestSolution>>();

        string expectedHandlerName = "";
        Assert.Throws<ArgumentException>(() =>
            challengeHandlerDescriptor.CloneWithNewName(expectedHandlerName));
    }
}